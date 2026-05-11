using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using AutoCount.Data;

using Sales = VTACPluginBase.Classes.AutoCountGenerator.CategorySales_Cls;
using GLARAP = VTACPluginBase.Classes.AutoCountGenerator.CategoryGLARAP_Cls;
using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.Classes.Helpers.AutoCountHelper;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.Classes.Helpers.SystemHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.BusinessBase
{
    public abstract class BusinessBase_Cls
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class MyAttributesAttribute : Attribute
        {
            public bool IsPrimaryKey { get; set; }
            public string PrimaryKeyFieldName { get; set; }
            public bool IsDisplayKey { get; set; }
            public string DisplayKeyFieldName { get; set; }
            public bool IsDeleteKey { get; set; }
            public bool IsDisplayField { get; set; }
            public bool IsDataTable { get; set; }
            public bool IsMainChildClass { get; set; }
            public bool IsChildClass { get; set; }
        }

        protected DataTable mydt_Master = null;

        protected DataTable mydt_MasterOri = null;

        private Type typ_BO = null;

        #region " Enumeration "
        public enum DocumentStatus
        {
            NEW,
            PROCESSING,
            CANCELLED,
            COMPLETED,
            STARTCOLLECTIONS,
            FINISHCOLLECTIONS,
            TERMINATED,
            ACTIVATED, //added by Ryan 20221008
            EXPRIED, //added by Ryan 20221008
            POSTED, //added by Ryan 20230227
            GENERATED, //added by Ryan 20230227
            VOID //added by Ryan 20230227
        }
        #endregion " Enumeration "

        #region " Properties "
        #region " MUST Inherited Properties "
        [MyAttributes(IsDisplayField = true)]
        public abstract string Name { get; }

        protected abstract string dbTableNameMaster { get; }
        protected abstract string dbQueryNameMaster { get; }

        [MyAttributes(IsPrimaryKey = true, PrimaryKeyFieldName = "DocKey", IsDeleteKey = true)]
        public abstract long PrimaryKey { get; set; }

        protected abstract string SysConfigDocNoFormat { get; }
        protected abstract string DocNoFormat { get; }

        [MyAttributes(IsMainChildClass = true)]
        public abstract BusinessBaseDTL_Cls Detail { get; }
        #endregion " MUST Inherited Properties "

        #region " Standard Properties "
        [MyAttributes(IsDisplayField = true)]
        public string PrimaryKeyFieldName { get; private set; } = "DocKey";

        [MyAttributes(IsChildClass = true)]
        public List<BusinessBaseDTL_Cls> OtherDetails { get; private set; } = new List<BusinessBaseDTL_Cls>();

        [MyAttributes(IsDataTable = true)]
        public DataTable MasterDataTable { get => mydt_Master; }

        [MyAttributes(IsDisplayField = true)]
        public string MasterTableName { get => dbTableNameMaster; }
        [MyAttributes(IsDisplayField = true)]
        public string MasterTableQryName { get => dbQueryNameMaster; }

        //////////public long DocKey { get; protected set; } = -1;
        [MyAttributes(IsDisplayKey = true, DisplayKeyFieldName = "DocNo")]
        public virtual string DocNo { get; set; } = AutoCount.Const.AppConst.NewDocumentNo;
        [MyAttributes(IsDisplayField = true)]
        public string DisplayKeyFieldName { get; private set; } = "DocNo";
        public virtual DateTime DocDate { get; set; } = DateTime.Today;
        public virtual DocumentStatus DocStatusValue { get; set; } = DocumentStatus.NEW;
        public virtual string DocStatus { get; set; } = DocumentStatus.NEW.ToString().ToUpper();

        private long Llng_TempDtlKey = 0;
        public long TempDtlKey
        {
            get
            {
                return Llng_TempDtlKey -= 1;
            }
        }
        #endregion " Standard Properties "
        #endregion " Properties "

        #region " Constructor "
        public BusinessBase_Cls()
        {
            typ_BO = this.GetType();

            MyAttributesAttribute attribute = null;

            PropertyInfo propertyInfo = typ_BO.GetProperty(nameof(PrimaryKey));
            attribute = (MyAttributesAttribute)propertyInfo.GetCustomAttribute(typeof(MyAttributesAttribute));
            if (attribute != null && attribute.IsPrimaryKey)
                PrimaryKeyFieldName = attribute.PrimaryKeyFieldName;

            propertyInfo = typ_BO.GetProperty(nameof(Detail));
            attribute = (MyAttributesAttribute)propertyInfo.GetCustomAttribute(typeof(MyAttributesAttribute));
            if (attribute != null && attribute.IsMainChildClass)
            {
                object obj_Value = propertyInfo.GetValue(this);
                if (obj_Value != null && obj_Value.GetType().BaseType == typeof(BusinessBaseDTL_Cls))
                    ((BusinessBaseDTL_Cls)obj_Value).DocKeyFieldName = PrimaryKeyFieldName;
            }

            #region " to get DisplayKey and collect ALL OtherDetails "
            PropertyInfo[] propertyInfos = typ_BO.GetProperties();
            for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
            {
                attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));

                //to get DisplayKey
                if (attribute != null && attribute.IsDisplayKey)
                    DisplayKeyFieldName = attribute.DisplayKeyFieldName;

                //to collect ALL OtherDetails
                if (attribute != null && attribute.IsChildClass)
                {
                    object obj_Value = propertyInfos[int_i].GetValue(this);
                    if (obj_Value.GetType().BaseType == typeof(BusinessBaseDTL_Cls))
                    {
                        ((BusinessBaseDTL_Cls)obj_Value).DocKeyFieldName = PrimaryKeyFieldName;

                        OtherDetails.Add((BusinessBaseDTL_Cls)obj_Value);
                    }
                }
            }
            #endregion " to get DisplayKey and collect ALL OtherDetails "
        }
        #endregion " Constructor "

        #region " Methods/Subs/Function "
        #region " Save Record(s) section "
        public virtual string Save()
        {
            string str_rtn = "";
            DBSetting newDBSetting = myDBSetting.StartTransaction();
            try
            {
                string str_cmdMast = "";
                int int_execOK = 0;

                // to assign DocKey value
                if (PrimaryKey <= 0) PrimaryKey = GetDocKey(dbTableNameMaster, PrimaryKeyFieldName);

                // ----------------------------------------------------------------------------------------------------------------------------
                // here to check the DocNo duplication
                // ===================================
                PropertyInfo propertyInfo = typ_BO.GetProperty(DisplayKeyFieldName);
                object obj_DisplayKey = propertyInfo.GetValue(this);

                //////////if (newDBSetting.GetDataTable("SELECT * FROM " + this.dbTableNameMaster + " WHERE DocNo='" + SQLString(DocNo) + $"' AND {PrimaryKeyFieldName}<>" + PrimaryKey, false).Rows.Count > 0)
                if (newDBSetting.GetDataTable($"SELECT * FROM {this.dbTableNameMaster} WHERE {DisplayKeyFieldName}='{SQLString(obj_DisplayKey.ToString())}' AND {PrimaryKeyFieldName}<>{PrimaryKey}", false).Rows.Count > 0)
                {
                    //////////str_rtn = "The Document No: [" + DocNo + "] is duplicated, please assign again.";
                    str_rtn = $"The Data with Display Key Value: [{obj_DisplayKey}] is duplicated, please assign again.";
                }
                // ----------------------------------------------------------------------------------------------------------------------------

                if (str_rtn == "")
                {
                    // below stmts is used for update data into table 
                    str_cmdMast = SQLUpdateStmt();
                    int_execOK = newDBSetting.ExecuteNonQuery(str_cmdMast);

                    // below stmts is use for insert data into table
                    if (int_execOK <= 0)
                    {
                        str_cmdMast = SQLInsertStmt();
                        int int_i = newDBSetting.ExecuteNonQuery(str_cmdMast);
                    }

                    // here will update detail rec(s)
                    if (Detail != null && Detail.DetailDataTable.Rows.Count >= 0)
                    {
                        Detail.DocKey = PrimaryKey;
                        Detail.DocKeyFieldName = PrimaryKeyFieldName;

                        if (!Detail.UpdateRecDetailDataTable(newDBSetting))
                        {
                            newDBSetting.Rollback();
                            newDBSetting.EndTransaction();

                            return str_rtn = $"Update Details [{Detail.Name}] Record(s) Failed, please check your Details record(s).";
                        }
                    }

                    // here will update other detail rec(s) if have/exists
                    if (OtherDetails.Count > 0)
                    {
                        foreach (BusinessBaseDTL_Cls otherbodtl in OtherDetails)
                        {
                            if (otherbodtl.DetailDataTable.Rows.Count >= 0)
                            {
                                otherbodtl.DocKey = PrimaryKey;
                                otherbodtl.DocKeyFieldName = PrimaryKeyFieldName;

                                if (!otherbodtl.UpdateRecDetailDataTable(newDBSetting))
                                {
                                    newDBSetting.Rollback();
                                    newDBSetting.EndTransaction();

                                    return str_rtn = $"Update Other Details [{otherbodtl.Name}] Record(s) Failed, please check your Other Details record(s).";
                                }
                            }
                        }
                    }

                    // commit the DB transaction
                    newDBSetting.Commit();
                }
            }
            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowMessage(ex.Message);
                newDBSetting.Rollback();
                //////////AutoCount.Data.DataError.HandleSqlException((SqlException)ex);

                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(Save) + "()", ex);

                // if has errors then return the error messages
                str_rtn = ex.Message;
            }
            finally
            {
                newDBSetting.EndTransaction();

                if (Detail != null) Detail.DeletedDetailDataTable.Clear();

                if (OtherDetails.Count > 0)
                {
                    foreach (BusinessBaseDTL_Cls otherbodtl in OtherDetails)
                        otherbodtl.DeletedDetailDataTable.Clear();
                }
            }

            return str_rtn;
        }
        #endregion " Save Record(s) section "

        #region " Delete Record(s) section "
        // here is the DELETE process
        public virtual string Delete()
        {
            string str_result = "";
            string str_rtn = "";

            DBSetting newDBSetting = myDBSetting.StartTransaction();

            try
            {
                string str_cmdMast = "";
                int int_execOK = 0;

                // -----------------------------------------------------------------
                // here need to check the delete validation
                // =========================================
                str_result = IsCanDelete();
                if (str_result == "")
                {
                    DataTable dt_ChkRecExists = newDBSetting.GetDataTable($"Select * from {dbTableNameMaster} where {PrimaryKeyFieldName}={PrimaryKey}", false);
                    if ((dt_ChkRecExists.Rows.Count <= 0))
                    {
                        str_rtn = $"The Document Key: {PrimaryKey} with DocNo [{DocNo}] Failed to Delete (Record NOT Found).";
                    }

                    if (str_rtn == "")
                    {
                        //// --------------------------------------------------------------------------------------------------------------------------------
                        //// START - added by chang on 20200525: v1.8.2.17, replace by using function CancelAutoCountRCVRec()
                        //// ================================================================================================
                        //// BEFORE really delete just cancel from Autocount Doc 'Invoice' first
                        PropertyInfo propertyInfoDocKey = typ_BO.GetProperty(AutoCountGenerator.ACHelper.DocKeyTypes.IVDocKey.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (propertyInfoDocKey != null)
                            str_rtn = CancelAutoCountIVRec();
                        //// ================================================================================================
                        //// END - added by chang on 20200525: v1.8.2.17, replace by using function CancelAutoCountRCVRec()
                        //// --------------------------------------------------------------------------------------------------------------------------------

                        if (str_rtn == "")
                        {
                            // FIRST : suppose delete the sub table rec(s)
                            if (Detail != null)
                            {
                                str_rtn = Detail.Delete(newDBSetting);
                                if (str_rtn != "")
                                {
                                    newDBSetting.Rollback();
                                    newDBSetting.EndTransaction();

                                    return $"The Document Key: {PrimaryKey} with DocNo [{DocNo}] Failed to Delete due to reason: " + str_rtn;
                                }
                            }

                            //////////str_cmdMast = string.Format("delete from {0} where DocKey={1}", Detail.dbTableNameDetail, DocKey);
                            //////////int_execOK = newDBSetting.ExecuteNonQuery(str_cmdMast);

                            // FIRST 2: suppose delete the OTHER sub table rec(s) also
                            if (OtherDetails.Count > 0)
                            {
                                foreach (BusinessBaseDTL_Cls otherbodtl in OtherDetails)
                                {
                                    str_rtn = otherbodtl.Delete(newDBSetting);
                                    if (str_rtn != "")
                                    {
                                        newDBSetting.Rollback();
                                        newDBSetting.EndTransaction();

                                        return $"The Document Key: {PrimaryKey} with DocNo [{DocNo}] Failed to Delete due to reason: " + str_rtn;
                                    }
                                }
                            }

                            // SECOND : if successful deleted sub table rec(s) then can start delete the main table rec
                            if (int_execOK >= 0)
                            {
                                str_cmdMast = $"DELETE FROM {dbTableNameMaster} WHERE {PrimaryKeyFieldName}={PrimaryKey}";
                                int_execOK = newDBSetting.ExecuteNonQuery(str_cmdMast);
                            }

                            // commit the SQL transactions
                            newDBSetting.Commit();
                        }
                    }
                }
                else
                {
                    // until here means this rec cannot be deleted
                    // ============================================
                    str_rtn = $"The {PrimaryKeyFieldName}: {PrimaryKey} with DocNo [{DocNo}] cannot be deleted, " + Strings.Chr(10) + Strings.Chr(10) + str_result;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(Delete) + "()", ex);

                newDBSetting.Rollback();
                str_rtn = ex.Message;
            }
            finally
            {
                newDBSetting.EndTransaction();
            }

            return str_rtn;
        }

        protected virtual string IsCanDelete()
        {
            string str_rtn = "";

            try
            {
                #region " check DB Tables Relationship "
                str_rtn = CheckDBTableRelationship();
                if (str_rtn != "") return str_rtn;
                #endregion " check DB Tables Relationship "

                #region " check Doc Status "
                string str_DocStatus = "";

                if (this.DocStatus.ToUpper() == DocumentStatus.CANCELLED.ToString().ToUpper() ||
                    this.DocStatus.ToUpper() == DocumentStatus.COMPLETED.ToString().ToUpper())
                    str_DocStatus = this.DocStatus.ToUpper();

                if (str_DocStatus != "") str_rtn = "This Document has been [" + str_DocStatus + "], cannot Delete";
                #endregion " check Doc Status "
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(IsCanDelete) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        //added by chang on 20221008: use to check DB Tables Relationship for Foreign Keys, if exists then NOT ALLOWED to proceed delete action
        protected virtual string CheckDBTableRelationship()
        {
            string str_rtn = "";

            try
            {
                // string str_sql = $"select * from zv_SysTablesRelationship where ReferencedTable='{this.MasterTableName}' and ReferencedTableColumnName='{this.PrimaryKeyFieldName}'";
                // [edited] by scchang's GPT-5.2 on 20260121: v1.0.0.0, Prefer zvVTS_SysTablesRelationship (new) and fallback to zv_SysTablesRelationship (legacy)

                // [added] by scchang's GPT-5.2 on 20260121: v1.0.0.0, Resolve relationship view name dynamically
                string relationshipViewName = "";
                string[] relationshipViewCandidates = new[]
                {
                    "zvVTS_SysTablesRelationship",
                    "zv_SysTablesRelationship",
                };

                foreach (string viewName in relationshipViewCandidates)
                {
                    object obj = myDBSetting.ExecuteScalar($"select count(*) from dbo.sysobjects where id = object_id(N'[dbo].[{viewName}]') and OBJECTPROPERTY(id, N'IsView') = 1");
                    if (obj != null && obj != DBNull.Value && Convert.ToInt32(obj) > 0)
                    {
                        relationshipViewName = viewName;
                        break;
                    }
                }

                if (relationshipViewName == "")
                {
                    return "System relationship view not found: zvVTS_SysTablesRelationship / zv_SysTablesRelationship. Please run plugin initializer to create/update it.";
                }

                string str_sql = $"select * from {relationshipViewName} where ReferencedTable='{this.MasterTableName}' and ReferencedTableColumnName='{this.PrimaryKeyFieldName}'";
                DataTable dt_relation = myDBSetting.GetDataTable(str_sql, false);
                if (dt_relation != null && dt_relation.Rows.Count > 0)
                {
                    DataTable dt_relationdtl;
                    foreach (DataRow dr in dt_relation.Rows)
                    {
                        str_sql = $"select {dr["ParentTableColumnName"]} from {dr["ParentTable"]} where {dr["ParentTableColumnName"]}='{this.PrimaryKey}'";
                        dt_relationdtl = myDBSetting.GetDataTable(str_sql, false);
                        if (dt_relationdtl != null && dt_relationdtl.Rows.Count > 0)
                            return str_rtn = "This record in use, cannot Delete.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(CheckDBTableRelationship) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " Delete Record(s) section "

        #region " AutoCount Portion "
        #region " INVOICE (IV) "
        public virtual string IVDocDescription { get; protected set; }

        // Generate Autocount Documents (Invoice)
        public virtual string SaveAutoCountIVRec()
        {
            return Sales.SaveAutoCountIVRec(this);
        }

        // Cancel Autocount Documents (Invoice)
        public virtual string CancelAutoCountIVRec()
        {
            return Sales.CancelAutoCountIVRec(this);
        }

        // Delete Autocount Documents (Invoice)
        public virtual string DeleteAutoCountIVRec()
        {
            return Sales.DeleteAutoCountIVRec(this);
        }
        #endregion " INVOICE (IV) "

        #region " AR Payment (RP) "
        public virtual string RPDocDescription { get; protected set; }

        // Generate Autocount Documents (AR Payment)
        public virtual string SaveAutoCountRPRec()
        {
            return GLARAP.SaveAutoCountRPRec(this);
        }
        public virtual string SaveAutoCountRPRec(ref DataRow dr_BODTL)
        {
            return GLARAP.SaveAutoCountRPRec(this, ref dr_BODTL);
        }

        // Cancel Autocount Documents (AR Payment)
        public virtual string CancelAutoCountRPRec()
        {
            return GLARAP.CancelAutoCountRPRec(this);
        }
        public virtual string CancelAutoCountRPRec(ref DataRow dr_BODTL)
        {
            return GLARAP.CancelAutoCountRPRec(ref dr_BODTL);
        }
        #endregion " AR Payment (RP) "
        #endregion " AutoCount Portion "

        #region " Load Data section "
        public virtual void Load(string str_DocKey)
        {
            //load master data
            //////////if (mydt_Master == null) mydt_Master = new DataTable();
            mydt_Master = myDBSetting.GetDataTable(string.Format("Select * from {0} where {2}={1}", dbQueryNameMaster, str_DocKey, PrimaryKeyFieldName), true);
            mydt_MasterOri = mydt_Master.Copy(); // to keep a set of ori (not yet edit) main table rec in datatable

            #region " assign data into field(s) properties "
            if (mydt_Master.Rows.Count > 0)
            {
                string str_ColName = "";

                PropertyInfo propertyInfo = null;
                for (int int_i = 0; int_i < mydt_Master.Columns.Count; int_i++)
                {
                    str_ColName = mydt_Master.Columns[int_i].ColumnName;
                    if (str_ColName == PrimaryKeyFieldName) str_ColName = nameof(PrimaryKey);

                    propertyInfo = typ_BO.GetProperty(str_ColName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propertyInfo != null && mydt_Master.Rows[0][int_i] != DBNull.Value)
                        propertyInfo.SetValue(this, mydt_Master.Rows[0][int_i]);
                }
            }
            #endregion " assign data into field(s) properties "

            //load detail data
            if (Detail != null) Detail.Load(str_DocKey);

            #region " to load for OtherDetails "
            if (OtherDetails.Count > 0)
            {
                foreach (BusinessBaseDTL_Cls otherbodtl in OtherDetails)
                {
                    otherbodtl.Load(str_DocKey);
                }
            }
            #endregion " to load for OtherDetails "
        }
        #endregion " Load Data section "

        #region " Get and Set Methods section "
        public bool SetDetailTable(DataTable dt_DtlTable)
        {
            bool boo_rtn = false;
            try
            {
                if (Detail != null)
                {
                    Detail.DetailDataTable = dt_DtlTable;
                    Detail.DetailDataTable.AcceptChanges();
                }

                boo_rtn = true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SetDetailTable) + "()", ex);
            }
            return boo_rtn;
        }
        public bool SetOtherDetailTable(BusinessBaseDTL_Cls OtherDetail, DataTable dt_DtlTable)
        {
            bool boo_rtn = false;
            try
            {
                OtherDetail.DetailDataTable = dt_DtlTable;
                OtherDetail.DetailDataTable.AcceptChanges();
                boo_rtn = true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SetOtherDetailTable) + "()", ex);
            }
            return boo_rtn;
        }

        public dynamic GetBOFieldValue(string str_FieldName)
        {
            dynamic dyn_rtn = null;

            try
            {
                if (str_FieldName == PrimaryKeyFieldName) str_FieldName = nameof(PrimaryKey);

                PropertyInfo propertyInfo = typ_BO.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    if (!(propertyInfo.PropertyType == typeof(DateTime) && propertyInfo.GetValue(this) != null && (DateTime)propertyInfo.GetValue(this) == DateTime.MinValue))
                        dyn_rtn = propertyInfo.GetValue(this);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(GetBOFieldValue) + "()", ex);

                MessageBox.Show(ex.Message, this.GetType().Name + "." + nameof(GetBOFieldValue) + "()", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dyn_rtn;
        }
        public string SetBOFieldValue(string str_FieldName, object obj_FieldValue)
        {
            string str_rtn = "";

            try
            {
                if (str_FieldName == PrimaryKeyFieldName) str_FieldName = nameof(PrimaryKey);

                PropertyInfo propertyInfo = typ_BO.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(this, obj_FieldValue);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SetBOFieldValue) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " Get and Set Methods section "

        #region " Others Method section "
        // use to get DocNo value of main table(s)
        public string GetNewDocNo()
        {
            if (DocNoFormat == "") return DocNoFormat;

            string str_format = "";
            string str_nextno = "";
            string str_newDocNo = "";

            // get the doc no format                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
            str_format = (string)GetSysConfigValue(SysConfigDocNoFormat);
            if (str_format == null || str_format == "")
            {
                str_format = DocNoFormat;

                SetSysConfigValue(SysConfigDocNoFormat, str_format);

                //str_newDocNo = "IT-" + DateTime.Now.ToString("yyMM") + "-000001";
                str_newDocNo = str_format.Replace("{yyMM}", DateTime.Now.ToString("yyMM")).Replace("{000000}", "000001");
            }
            else
            {
                // get the length of the auto number
                str_nextno = GetNextRunDocNo("-" + DateTime.Now.ToString("yyMM") + "-").ToString("000000");

                // assign new doc no according to the selected format
                str_newDocNo = str_format.Replace("{yyMM}", DateTime.Now.ToString("yyMM")).Replace("{000000}", str_nextno);
            }

            return str_newDocNo;
        }
        // use to get running DocNo value of main table(s)
        public int GetNextRunDocNo(string str_YearMonth)
        {
            DataTable dt = myDBSetting.GetDataTable("SELECT cast(substring(DocNo,charindex('" + SQLString(str_YearMonth) + "',DocNo)+6,6) as numeric(18,0)) as intDocNo FROM " + dbTableNameMaster + " where DocNo like '%" + SQLString(str_YearMonth) + "%'", false);
            int int_count = dt.Rows.Count;
            if (int_count == 0) return 1;

            object obj_LastDocNo = dt.Compute("MAX(intDocNo)", "");
            int int_lastDocNo = (obj_LastDocNo != null && obj_LastDocNo != DBNull.Value ? System.Convert.ToInt32(obj_LastDocNo) : 0);
            if (int_lastDocNo == 0) return 1;

            int int_DocNo = int_lastDocNo + 1;
            if (int_lastDocNo != int_count)
            {
                for (int i = 1; i <= int_lastDocNo; i++)
                {
                    if (dt.Select("intDocNo='" + i.ToString() + "'").Length == 0)
                    {
                        int_DocNo = i;

                        break;
                    }
                }
            }

            return int_DocNo;
        }

        public virtual string GetDocStatusString(DocumentStatus documentStatus)
        {
            string str_rtn = documentStatus.ToString().ToUpper();

            try
            {
                if (documentStatus == DocumentStatus.STARTCOLLECTIONS) str_rtn = "Started Collections".ToUpper();
                if (documentStatus == DocumentStatus.FINISHCOLLECTIONS) str_rtn = "Finished Collections".ToUpper();
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(GetDocStatusString) + "()", ex);
            }

            return str_rtn;
        }

        #region " For Main Detail "
        public virtual void AddNewDetail(BusinessBaseDTL_Cls BODTL)
        {
            if (Detail == null) return;

            DataRow dr_newRow = Detail.DetailDataTable.NewRow();
            for (int int_i = 0; int_i < Detail.DetailDataTable.Columns.Count; int_i++)
            {
                if (Detail.DetailDataTable.Columns[int_i].ColumnName == nameof(Detail.PrimaryKey))
                {
                    dr_newRow[int_i] = TempDtlKey;

                    continue;
                }

                dr_newRow[int_i] = BODTL.GetBODTLFieldValue(Detail.DetailDataTable.Columns[int_i].ColumnName);
            }

            Detail.DetailDataTable.Rows.Add(dr_newRow);
        }
        public virtual void DeleteDetail(DataRow dr_del, bool boo_Remove = true) // added by chang on 20200424: v1.8.2.16, to handle delete detail when click on GridControl EmbeddedNavigator
        {
            if (Detail == null) return;

            if (Detail.DetailDataTable != null && Detail.DetailDataTable.Rows.Count > 0)
            {
                if (Detail.DeleteKeys.Count > 0)
                {
                    DataRow dr_newdel = Detail.DeletedDetailDataTable.NewRow();
                    for (int int_i = 0; int_i < Detail.DeleteKeys.Count; int_i++)
                    {
                        dr_newdel[Detail.DeleteKeys[int_i].ToString()] = dr_del[Detail.DeleteKeys[int_i].ToString()];
                    }

                    Detail.DeletedDetailDataTable.Rows.Add(dr_newdel);
                    Detail.DeletedDetailDataTable.AcceptChanges();
                }

                if (boo_Remove) Detail.DetailDataTable.Rows.Remove(dr_del);
            }
        }
        public virtual void DeleteAllDetail()
        {
            if (Detail == null) return;

            if (Detail.DetailDataTable != null && Detail.DetailDataTable.Rows.Count > 0)
            {
                if (Detail.DeleteKeys.Count > 0)
                {
                    DataRow dr_newdel = null;
                    for (int int_DtlIdx = 0; int_DtlIdx <= Detail.DetailDataTable.Rows.Count - 1; int_DtlIdx++)
                    {
                        dr_newdel = Detail.DeletedDetailDataTable.NewRow();
                        for (int int_i = 0; int_i < Detail.DeleteKeys.Count; int_i++)
                        {
                            dr_newdel[Detail.DeleteKeys[int_i].ToString()] = Detail.DetailDataTable.Rows[int_DtlIdx][Detail.DeleteKeys[int_i].ToString()];
                        }

                        Detail.DeletedDetailDataTable.Rows.Add(dr_newdel);
                        Detail.DeletedDetailDataTable.AcceptChanges();
                    }
                }

                Detail.DetailDataTable.Clear();
            }
        }
        #endregion " For Main Detail "

        public abstract void RollBackDetailInfo();
        #endregion " Others Method section "

        #region " Construct SQL Stmt "
        protected string SQLUpdateStmt()
        {
            string str_rtn = "UPDATE " + SQLString(dbTableNameMaster) + " SET ";

            try
            {
                object obj_Value = null;
                MyAttributesAttribute attribute = null;

                PropertyInfo[] propertyInfos = typ_BO.GetProperties();
                for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
                {
                    //to skip some unnessesary field(s)
                    if (propertyInfos[int_i].Name.Contains("Temp") || propertyInfos[int_i].Name.Contains("DocDesc")) continue;

                    //to check is the Display field, if yes then just skipped
                    attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
                    if (attribute != null && (attribute.IsPrimaryKey || attribute.IsDisplayField || attribute.IsDataTable || attribute.IsMainChildClass || attribute.IsChildClass)) continue;

                    //to get the value in property
                    obj_Value = propertyInfos[int_i].GetValue(this);

                    //if value is null/nothing then just skip
                    if (obj_Value == null || (propertyInfos[int_i].PropertyType.Name == "DateTime" && System.Convert.ToDateTime(obj_Value) == DateTime.MinValue))
                    {
                        //if detected null then just update back to null (if previously was some values)
                        str_rtn += "[" + propertyInfos[int_i].Name + "]=null,";

                        continue;
                    }

                    str_rtn += "[" + propertyInfos[int_i].Name + "]=";

                    switch (propertyInfos[int_i].PropertyType.Name)
                    {
                        case "String":
                            str_rtn += "N'" + SQLString(obj_Value.ToString()) + "',";
                            break;
                        case "DateTime":
                            str_rtn += "'" + System.Convert.ToDateTime(obj_Value).ToString("yyyy-MM-dd") + "',";
                            break;
                        case "Int":
                        case "Int32":
                            str_rtn += System.Convert.ToInt32(obj_Value).ToString() + ",";
                            break;
                        case "Int64":
                            str_rtn += System.Convert.ToInt64(obj_Value).ToString() + ",";
                            break;
                        case "Decimal":
                            str_rtn += System.Convert.ToDecimal(obj_Value).ToString() + ",";
                            break;
                        case "Boolean":
                            str_rtn += (System.Convert.ToBoolean(obj_Value) ? 1 : 0) + ",";
                            break;
                        case "DocumentStatus":
                            str_rtn += ((int)DocStatusValue) + ",";
                            //str_rtn += ((int)obj_Value) + ",";
                            break;
                        default:
                            str_rtn += "'" + SQLString(obj_Value.ToString()) + "',";
                            break;
                    }
                }

                //to assign LastModified info
                str_rtn += "[LastModified]=getdate(), ";
                str_rtn += "[LastModifiedUserID]=N'" + SQLString(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID) + "' ";
                str_rtn += $"where [{PrimaryKeyFieldName}]=" + PrimaryKey;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SQLUpdateStmt) + "()", ex);
            }

            return str_rtn;
        }
        protected string SQLInsertStmt()
        {
            string str_rtn = "INSERT INTO " + SQLString(dbTableNameMaster) + " (";

            try
            {
                object obj_Value = null;
                MyAttributesAttribute attribute = null;

                string str_FieldName = "";

                ////////str_rtn += "[DocKey], ";
                ////////string str_Values = "values(" + DocKey + ", ";
                string str_Values = "values(";

                PropertyInfo[] propertyInfos = typ_BO.GetProperties();
                for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
                {
                    //to skip some unnessesary field(s)
                    if (propertyInfos[int_i].Name.Contains("Temp") || propertyInfos[int_i].Name.Contains("DocDesc")) continue;

                    //to check is the Display field, if yes then just skipped
                    attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
                    if (attribute != null && (attribute.IsDisplayField || attribute.IsDataTable || attribute.IsMainChildClass || attribute.IsChildClass)) continue;

                    //to get the value in property
                    obj_Value = propertyInfos[int_i].GetValue(this);

                    //if value is null/nothing then just skip
                    if (obj_Value == null || (propertyInfos[int_i].PropertyType.Name == "DateTime" && System.Convert.ToDateTime(obj_Value) == DateTime.MinValue)) continue;

                    str_FieldName = propertyInfos[int_i].Name;
                    if (str_FieldName == nameof(PrimaryKey)) str_FieldName = PrimaryKeyFieldName;
                    str_rtn += "[" + str_FieldName + "], ";

                    switch (propertyInfos[int_i].PropertyType.Name)
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
                        case "DocumentStatus":
                            str_Values += ((int)DocStatusValue) + ",";
                            break;
                        default:
                            str_Values += "'" + SQLString(obj_Value.ToString()) + "',";
                            break;
                    }
                }

                //to assign LastModified info
                str_rtn += "[LastModified], ";
                str_Values += "getdate(), ";
                str_rtn += "[LastModifiedUserID], ";
                str_Values += "N'" + SQLString(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID) + "', ";
                str_rtn += "[CreatedTimeStamp], ";
                str_Values += "getdate(), ";
                str_rtn += "[CreatedUserID]) ";
                str_Values += "N'" + SQLString(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID) + "') ";
                str_rtn = str_rtn + str_Values;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.GetType().Name + "." + nameof(SQLInsertStmt) + "()", ex);
            }

            return str_rtn;
        }
        #endregion " Construct SQL Stmt "
        #endregion " Methods/Subs/Function "
    }
}
