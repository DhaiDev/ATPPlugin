using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

using AutoCount.Data;

using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.Classes.Helpers.AutoCountHelper;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.BusinessBase
{
    public abstract class BusinessBaseDTL_Cls
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public class MyAttributesAttribute : Attribute
        {
            public bool IsPrimaryKey { get; set; }
            public string PrimaryKeyFieldName { get; set; }
            public bool IsDeleteKey { get; set; }
            public bool IsDisplayField { get; set; }
            public bool IsDataTable { get; set; }
            public bool IsChildClass { get; set; }
        }

        private Type typ_BODTL = null;

        #region " Enumeration "
        public enum DetailStatus
        {
            NEW,
            PROCESSING,
            CANCELLED,
            COMPLETED,
            ARPYMTGENERATED
        }
        #endregion " Enumeration "

        #region " Fields (Variables) "
        public long DocKey = -1;
        public string DocKeyFieldName = "DocKey";

        //to keep the Delete Keys
        public ArrayList DeleteKeys = new ArrayList();
        #endregion " Fields (Variables) "

        #region " Properties "
        #region " MUST Inherited Properties "
        public abstract string Name { get; }

        [MyAttributes(IsPrimaryKey = true, PrimaryKeyFieldName = "DtlKey", IsDeleteKey = true)]
        public abstract long PrimaryKey { get; set; }

        public abstract string DetailTableName { get; }
        public abstract string DetailTableQryName { get; }
        #endregion " MUST Inherited Properties "

        public string PrimaryKeyFieldName { get; private set; } = "DtlKey";

        public List<BusinessBaseDTL_Cls> Details { get; private set; } = new List<BusinessBaseDTL_Cls>();

        private DataTable _DetailDataTable;
        public DataTable DetailDataTable
        {
            get
            {
                if (_DetailDataTable != null && _DetailDataTable.TableName == "") _DetailDataTable.TableName = "Tbl_" + this.GetType().Name;
                return _DetailDataTable;
            }
            set
            {
                _DetailDataTable = value;
                _DetailDataTable.TableName = "Tbl_" + this.GetType().Name;
                DetailOriDataTable = _DetailDataTable.Copy();
            }
        }
        public DataTable DetailOriDataTable { get; private set; } // use to keep a set of original record(s) in datatable
        public DataTable DeletedDetailDataTable { get; private set; } // use for captured the deleted row(s)
        public virtual DetailStatus DtlStatusValue { get; set; } = DetailStatus.NEW;
        public virtual string DtlStatus { get; set; } = DetailStatus.NEW.ToString().ToUpper();

        private long Llng_TempDtlKey = 0;
        public long TempDtlKey
        {
            get
            {
                return Llng_TempDtlKey -= 1;
            }
        }
        #endregion " Properties "

        public BusinessBaseDTL_Cls()
        {
            DetailDataTable = new DataTable(DetailTableName);

            //to get the class object
            typ_BODTL = this.GetType();

            MyAttributesAttribute attribute = null;

            #region " initial field(s) into DataTables "
            PropertyInfo propertyInfo = typ_BODTL.GetProperty(nameof(PrimaryKey));
            attribute = (MyAttributesAttribute)propertyInfo.GetCustomAttribute(typeof(MyAttributesAttribute));
            if (attribute != null && attribute.IsPrimaryKey)
            {
                PrimaryKeyFieldName = attribute.PrimaryKeyFieldName;

                DetailDataTable.Columns.Add(PrimaryKeyFieldName, propertyInfo.PropertyType);
            }
            if (attribute != null && attribute.IsDeleteKey)
                DeleteKeys.Add(PrimaryKeyFieldName);
            #endregion " initial field(s) into DataTables "

            #region " initial field(s) into DataTables "
            FieldInfo[] fieldInfos = typ_BODTL.GetFields();
            if (fieldInfos.Length > 0)
            {
                for (int int_i = 0; int_i < fieldInfos.Length; int_i++)
                {
                    if (fieldInfos[int_i].Name != nameof(DeleteKeys) && fieldInfos[int_i].Name != nameof(DocKeyFieldName) && fieldInfos[int_i].FieldType.FullName != "System.Data.DataTable")
                    {
                        attribute = (MyAttributesAttribute)fieldInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
                        if (attribute != null && attribute.IsDeleteKey)
                            DeleteKeys.Add(fieldInfos[int_i].Name);

                        if (fieldInfos[int_i].Name == nameof(DocKey))
                            DetailDataTable.Columns.Add(DocKeyFieldName, fieldInfos[int_i].FieldType);
                        else
                            DetailDataTable.Columns.Add(fieldInfos[int_i].Name, fieldInfos[int_i].FieldType);
                    }
                }

                DetailDataTable.AcceptChanges();

                // need to copy also the detail datatable
                DetailOriDataTable = DetailDataTable.Copy();
            }

            if (DeleteKeys.Count > 0)
            {
                // create structure of the sub table's datatable (for deleted row(s), suppose has same structure)
                DeletedDetailDataTable = new DataTable("Del" + DetailTableName);

                for (int int_i = 0; int_i < DeleteKeys.Count; int_i++)
                {
                    DeletedDetailDataTable.Columns.Add(DeleteKeys[int_i].ToString(), typeof(long));
                }

                DeletedDetailDataTable.AcceptChanges();
            }
            #endregion " initial field(s) into DataTables "

            #region " to collect ALL OtherDetails "
            PropertyInfo[] propertyInfos = typ_BODTL.GetProperties();
            for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
            {
                attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
                if (attribute != null && attribute.IsChildClass)
                {
                    object obj_Value = propertyInfos[int_i].GetValue(this);
                    if (obj_Value.GetType().BaseType == typeof(BusinessBaseDTL_Cls))
                        Details.Add((BusinessBaseDTL_Cls)obj_Value);
                }
            }
            #endregion " to collect ALL OtherDetails "
        }

        #region " Save Record(s) section "
        public virtual bool UpdateRecDetailDataTable(DBSetting dbsetting)
        {
            bool boo_rtn = false;

            string str_cmdSub = "";
            int int_execOK = 0;

            try
            {
                if (Details.Count > 0)
                {
                    foreach (BusinessBaseDTL_Cls bodtl in Details)
                        bodtl.UpdateRecDetailDataTable(dbsetting);
                }

                // ----------------------------------------------------------------------------------------------------------------------------
                // DELETE record(s) section
                // ========================
                if (DeletedDetailDataTable.Rows.Count > 0)
                {
                    // Dim newdbsetting As DBSetting = dbsetting.StartTransaction()
                    for (int int_delidx = 0; int_delidx <= DeletedDetailDataTable.Rows.Count - 1; int_delidx++)
                    {
                        str_cmdSub = string.Format("DELETE from {0} WHERE {1}={2}", DetailTableName, PrimaryKeyFieldName, DeletedDetailDataTable.Rows[int_delidx][PrimaryKeyFieldName]);
                        int_execOK = dbsetting.ExecuteNonQuery(str_cmdSub);

                        if (int_execOK < 0) return boo_rtn;
                    }

                    // dbsetting.Commit()
                    // newdbsetting.EndTransaction()

                    // after successfully deleted record(s) then clear the deleted datatable contents
                    DeletedDetailDataTable.Clear();
                }
                // ----------------------------------------------------------------------------------------------------------------------------

                // ----------------------------------------------------------------------------------------------------------------------------
                // INSERT or UPDATE record(s) section
                // ==================================
                foreach (DataRow dr in DetailDataTable.Rows)
                {
                    // [added] by scchang's GPT-5.2 on 20260126: v1.8.8.4, Performance optimization - skip unchanged rows
                    if (dr.RowState == DataRowState.Unchanged) continue;

                    if ((long)dr[PrimaryKeyFieldName] < 0) dr[PrimaryKeyFieldName] = GetDtlKey(DetailTableName, dbsetting, PrimaryKeyFieldName);
                    if ((long)dr[DocKeyFieldName] < 0) dr[DocKeyFieldName] = DocKey;

                    DataRowValuesAssignment(dr);

                    str_cmdSub = SQLUpdateStmt(dr);
                    int_execOK = dbsetting.ExecuteNonQuery(str_cmdSub);

                    // if update rec failed then try insert rec
                    if (int_execOK <= 0)
                    {
                        str_cmdSub = SQLInsertStmt(dr);
                        int_execOK = dbsetting.ExecuteNonQuery(str_cmdSub);
                    }
                }
                // ----------------------------------------------------------------------------------------------------------------------------

                if (int_execOK >= 0) boo_rtn = true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(UpdateRecDetailDataTable) + "()", ex);

                boo_rtn = false;
            }

            return boo_rtn;
        }
        #endregion " Save Record(s) section "

        #region " Delete Record(s) section "
        // here is the DELETE process
        public virtual string Delete(DBSetting dbSetting)
        {
            string str_rtn = "";

            try
            {
                // FIRST: suppose delete the OTHER sub table rec(s) also
                if (Details.Count > 0)
                {
                    foreach (BusinessBaseDTL_Cls bodtl in Details)
                    {
                        str_rtn = bodtl.Delete(dbSetting);
                        if (str_rtn != "")
                            return $"Class Object [{bodtl.Name}] Failed to Delete due to reason: " + str_rtn;
                    }
                }

                // SECOND : if successful deleted sub table rec(s) then can start delete the main table rec
                string str_sqlcmd = $"delete from {DetailTableName} where {DocKeyFieldName}={DocKey}";
                int int_execOK = dbSetting.ExecuteNonQuery(str_sqlcmd);
                if (int_execOK < 0) str_rtn = $"Class Object [{Name}] Failed to Delete.";
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(Delete) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " Delete Record(s) section "

        #region " Load Data section "
        public virtual void Load(string str_DocKey)
        {
            //to assign the DocKey
            DocKey = System.Convert.ToInt64(str_DocKey);

            DetailDataTable = myDBSetting.GetDataTable(string.Format("Select * from {0} where {2}={1}", DetailTableQryName, str_DocKey, DocKeyFieldName), false);

            #region " to load for OtherDetails "
            if (Details.Count > 0)
            {
                foreach (BusinessBaseDTL_Cls bodtl in Details)
                {
                    bodtl.Load(str_DocKey);
                }
            }
            #endregion " to load for OtherDetails "
        }
        #endregion " Load Data section "

        #region " Methods/Functions/Procedures "
        public virtual string GetDtlStatusString(DetailStatus detailStatus)
        {
            string str_rtn = detailStatus.ToString().ToUpper();

            try
            {
                if (detailStatus == DetailStatus.ARPYMTGENERATED) str_rtn = "AR Payment Generated".ToUpper();
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(GetDtlStatusString) + "()", ex);
            }

            return str_rtn;
        }

        public dynamic GetBODTLDTFieldValue(string str_ColumnName, DataRow dr)
        {
            dynamic dyn_rtn = DBNull.Value;

            try
            {
                if (DetailDataTable.Columns.Contains(str_ColumnName))
                    dyn_rtn = dr[str_ColumnName];
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(GetBODTLDTFieldValue) + "()", ex);

                MessageBox.Show(ex.Message, this.GetType().Name + "." + nameof(GetBODTLDTFieldValue) + "()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dyn_rtn;
        }
        public void SetBODTLDTFieldValue(string str_ColumnName, object obj_ColumnValue, DataRow dr)
        {
            try
            {
                if (DetailDataTable.Columns.Contains(str_ColumnName))
                    dr[str_ColumnName] = obj_ColumnValue;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SetBODTLDTFieldValue) + "()", ex);
            }
        }

        public dynamic GetBODTLFieldValue(string str_FieldName)
        {
            dynamic dyn_rtn = null;

            try
            {
                if (str_FieldName == DocKeyFieldName) str_FieldName = nameof(DocKey);
                if (str_FieldName == PrimaryKeyFieldName) str_FieldName = nameof(PrimaryKey);

                FieldInfo fieldInfo = typ_BODTL.GetField(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    if (fieldInfo.FieldType == typeof(DateTime) && fieldInfo.GetValue(this) != null && (DateTime)fieldInfo.GetValue(this) == DateTime.MinValue)
                        dyn_rtn = DBNull.Value;
                    else
                        dyn_rtn = fieldInfo.GetValue(this);
                }
                else
                {
                    PropertyInfo propertyInfo = typ_BODTL.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        if (propertyInfo.PropertyType == typeof(DateTime) && propertyInfo.GetValue(this) != null && (DateTime)propertyInfo.GetValue(this) == DateTime.MinValue)
                            dyn_rtn = DBNull.Value;
                        else
                            dyn_rtn = propertyInfo.GetValue(this);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(GetBODTLFieldValue) + "()", ex);

                MessageBox.Show(ex.Message, this.GetType().Name + "." + nameof(GetBODTLFieldValue) + "()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dyn_rtn;
        }
        public string SetBODTLFieldValue(string str_FieldName, object obj_FieldValue)
        {
            string str_rtn = "";

            try
            {
                if (str_FieldName == DocKeyFieldName) str_FieldName = nameof(DocKey);

                FieldInfo fieldInfo = typ_BODTL.GetField(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(this, obj_FieldValue);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SetBODTLFieldValue) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        private void DataRowValuesAssignment(DataRow dr)
        {
            for (int int_idx = 0; int_idx <= dr.Table.Columns.Count - 1; int_idx++)
            {
                switch (dr.Table.Columns[int_idx].DataType.ToString())
                {
                    case "System.String":
                        {
                            if (dr[int_idx] == DBNull.Value) dr[int_idx] = "";
                            break;
                        }

                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Decimal":
                    case "System.Double":
                    case "System.Boolean":
                        {
                            if (dr[int_idx] == DBNull.Value) dr[int_idx] = 0;
                            break;
                        }

                    case "System.DateTime":
                        {
                            if (dr[int_idx] == DBNull.Value) dr[int_idx] = System.DateTime.MinValue;
                            break;
                        }
                }
            }
        }

        #region " For Details "
        public virtual void AddNewDetail(BusinessBaseDTL_Cls BODTL)
        {
            DataRow dr_newRow = DetailDataTable.NewRow();
            for (int int_i = 0; int_i < DetailDataTable.Columns.Count; int_i++)
            {
                if (DetailDataTable.Columns[int_i].ColumnName == PrimaryKeyFieldName)
                {
                    dr_newRow[int_i] = TempDtlKey;

                    continue;
                }

                dr_newRow[int_i] = BODTL.GetBODTLFieldValue(DetailDataTable.Columns[int_i].ColumnName);
            }

            DetailDataTable.Rows.Add(dr_newRow);
        }
        public virtual void DeleteDetail(DataRow dr_del, bool boo_Remove = true) // added by chang on 20200424: v1.8.2.16, to handle delete detail when click on GridControl EmbeddedNavigator
        {
            if (DetailDataTable != null && DetailDataTable.Rows.Count > 0)
            {
                if (DeleteKeys.Count > 0)
                {
                    DataRow dr_newdel = DeletedDetailDataTable.NewRow();
                    for (int int_i = 0; int_i < DeleteKeys.Count; int_i++)
                    {
                        dr_newdel[DeleteKeys[int_i].ToString()] = dr_del[DeleteKeys[int_i].ToString()];
                    }

                    DeletedDetailDataTable.Rows.Add(dr_newdel);
                    DeletedDetailDataTable.AcceptChanges();
                }

                if (boo_Remove) DetailDataTable.Rows.Remove(dr_del);
            }
        }
        public virtual void DeleteAllDetail()
        {
            if (DetailDataTable != null && DetailDataTable.Rows.Count > 0)
            {
                if (DeleteKeys.Count > 0)
                {
                    DataRow dr_newdel = null;
                    for (int int_DtlIdx = 0; int_DtlIdx <= DetailDataTable.Rows.Count - 1; int_DtlIdx++)
                    {
                        dr_newdel = DeletedDetailDataTable.NewRow();
                        for (int int_i = 0; int_i < DeleteKeys.Count; int_i++)
                        {
                            dr_newdel[DeleteKeys[int_i].ToString()] = DetailDataTable.Rows[int_DtlIdx][DeleteKeys[int_i].ToString()];
                        }

                        DeletedDetailDataTable.Rows.Add(dr_newdel);
                        DeletedDetailDataTable.AcceptChanges();
                    }
                }

                DetailDataTable.Clear();
            }
        }
        #endregion " For Details "
        #endregion " Methods/Functions/Procedures "

        #region " Construct SQL Stmt "
        public string SQLUpdateStmt(DataRow dr)
        {
            string str_rtn = "UPDATE " + SQLString(DetailTableName) + " SET ";

            try
            {
                object obj_Value = DBNull.Value;
                MyAttributesAttribute attribute = null;

                for (int int_i = 0; int_i < dr.Table.Columns.Count; int_i++)
                {
                    //to skip some unnessesary field(s)
                    //////////if (dr.Table.Columns[int_i].ColumnName.Contains("DtlKey")) continue; //removed by chang on 20220814: seems shdn't use 'Contains' function
                    if (dr.Table.Columns[int_i].ColumnName.Equals(PrimaryKeyFieldName)) continue; //added by chang on 20220814: changed to use 'Equals' since want to skip only for 'DtlKey' cos it's PrimaryKey

                    //to check is the Display field, if yes then just skipped
                    attribute = (MyAttributesAttribute)this.GetType().GetField(dr.Table.Columns[int_i].ColumnName).GetCustomAttribute(typeof(MyAttributesAttribute));
                    if (attribute != null && (attribute.IsDisplayField)) continue;

                    //to get the value in property
                    obj_Value = dr[int_i];

                    //if value is null/nothing then just skip
                    if (obj_Value == DBNull.Value || (obj_Value.GetType().Name == "DateTime" && System.Convert.ToDateTime(obj_Value) == DateTime.MinValue))
                    {
                        //if detected null then just update back to null (if previously was some values)
                        str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "]=null,";

                        continue;
                    }

                    str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "]=";

                    switch (obj_Value.GetType().Name)
                    {
                        case "String":
                            str_rtn += "N'" + SQLString(obj_Value.ToString()) + "', ";
                            break;
                        case "DateTime":
                            str_rtn += "'" + System.Convert.ToDateTime(obj_Value).ToString("yyyy-MM-dd") + "', ";
                            break;
                        case "Int":
                        case "Int32":
                            str_rtn += System.Convert.ToInt32(obj_Value).ToString() + ", ";
                            break;
                        case "Int64":
                            str_rtn += System.Convert.ToInt64(obj_Value).ToString() + ", ";
                            break;
                        case "Decimal":
                            str_rtn += System.Convert.ToDecimal(obj_Value).ToString() + ", ";
                            break;
                        case "Boolean":
                            str_rtn += (System.Convert.ToBoolean(obj_Value) ? 1 : 0) + ", ";
                            break;
                        case "DetailStatus":
                            str_rtn += System.Convert.ToInt32(obj_Value) + ", ";
                            break;
                        default:
                            str_rtn += "'" + SQLString(obj_Value.ToString()) + "', ";
                            break;
                    }
                }

                //to assign 'where' clause
                if (str_rtn != null && str_rtn.Length > 2 && str_rtn != "") str_rtn = str_rtn.Substring(0, str_rtn.Length - 2);
                str_rtn += $" where [{PrimaryKeyFieldName}]=" + dr[PrimaryKeyFieldName].ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SQLUpdateStmt) + "()", ex);
            }

            return str_rtn;
        }
        public string SQLInsertStmt(DataRow dr)
        {
            string str_rtn = "INSERT INTO " + SQLString(DetailTableName) + " (";

            try
            {
                object obj_Value = DBNull.Value;
                MyAttributesAttribute attribute = null;

                string str_Values = "values(";

                for (int int_i = 0; int_i < dr.Table.Columns.Count; int_i++)
                {
                    ////to skip some unnessesary field(s)
                    //if (propertyInfos[int_i].Name.Contains("Temp")) continue;

                    if (dr.Table.Columns[int_i].ColumnName != PrimaryKeyFieldName && dr.Table.Columns[int_i].ColumnName != DocKeyFieldName)
                    {
                        //to check is the Display field, if yes then just skipped
                        attribute = (MyAttributesAttribute)this.GetType().GetField(dr.Table.Columns[int_i].ColumnName).GetCustomAttribute(typeof(MyAttributesAttribute));
                        if (attribute != null && (attribute.IsDisplayField)) continue;
                    }

                    //to get the value in property
                    obj_Value = dr[int_i];

                    //if value is null/nothing then just skip
                    if (obj_Value == DBNull.Value || (obj_Value.GetType().Name == "DateTime" && System.Convert.ToDateTime(obj_Value) == DateTime.MinValue)) continue;

                    str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "], ";

                    switch (obj_Value.GetType().Name)
                    {
                        case "String":
                            str_Values += "N'" + SQLString(obj_Value.ToString()) + "',";
                            break;
                        case "DateTime":
                            str_Values += "'" + System.Convert.ToDateTime(obj_Value).ToString("yyyy-MM-dd") + "',";
                            break;
                        case "Int":
                        case "Int32":
                            str_Values += System.Convert.ToInt32(obj_Value).ToString() + ",";
                            break;
                        case "Int64":
                            str_Values += System.Convert.ToInt64(obj_Value).ToString() + ",";
                            break;
                        case "Decimal":
                            str_Values += System.Convert.ToDecimal(obj_Value).ToString() + ",";
                            break;
                        case "Boolean":
                            str_Values += (System.Convert.ToBoolean(obj_Value) ? 1 : 0) + ",";
                            break;
                        case "DetailStatus":
                            str_Values += System.Convert.ToInt32(obj_Value) + ",";
                            break;
                        default:
                            str_Values += "'" + SQLString(obj_Value.ToString()) + "',";
                            break;
                    }
                }

                //to combined all fields and values sql stmt
                if (str_rtn != null && str_rtn.Length > 2 && str_rtn != "") str_rtn = str_rtn.Substring(0, str_rtn.Length - 2) + ") ";
                if (str_Values != null && str_Values.Length > 2 && str_Values != "") str_Values = str_Values.Substring(0, str_Values.Length - 1) + ") ";
                str_rtn = str_rtn + str_Values;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SQLInsertStmt) + "()", ex);
            }

            return str_rtn;
        }
        #endregion " Construct SQL Stmt "
    }
}
