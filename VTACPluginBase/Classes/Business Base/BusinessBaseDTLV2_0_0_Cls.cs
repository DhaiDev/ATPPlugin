using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;

using AutoCount.Data;

using VTACPluginBase.Classes.TextLogger;
using VTACPluginBase.Classes.DI;

using static VTACPluginBase.Classes.Helpers.AutoCountHelper;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.BusinessBase
{
    /// <summary>
    /// V2.0.0 - Modern Business Base Detail Class with Dependency Injection and Async Support
    /// Preserves all original functionality while adding modern patterns for .NET Framework 4.8
    /// </summary>
    public abstract class BusinessBaseDTLV2_0_0_Cls
    {
        #region " Attribute Classes "
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
        #endregion " Attribute Classes "

        #region " Operation Result Pattern "
        public class OperationResult
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
            public object Data { get; set; }

            public static OperationResult Success(object data = null, string message = "")
                => new OperationResult { IsSuccess = true, Data = data, Message = message };

            public static OperationResult Failure(string message, Exception exception = null)
                => new OperationResult { IsSuccess = false, Message = message, Exception = exception };
        }

        public class OperationResult<T> : OperationResult
        {
            public new T Data { get; set; }

            public static OperationResult<T> Success(T data, string message = "")
                => new OperationResult<T> { IsSuccess = true, Data = data, Message = message };

            public new static OperationResult<T> Failure(string message, Exception exception = null)
                => new OperationResult<T> { IsSuccess = false, Message = message, Exception = exception };
        }
        #endregion " Operation Result Pattern "

        #region " Dependency Injection "
        /// <summary>
        /// Service Locator for Dependency Injection
        /// </summary>
        protected IServiceLocator ServiceLocator => ServiceLocatorProvider.Current;

        /// <summary>
        /// Logger service from DI container
        /// </summary>
        protected IErrorLogger Logger => ServiceLocator?.GetService<IErrorLogger>() ?? new DefaultErrorLogger();

        /// <summary>
        /// Default Error Logger implementation
        /// </summary>
        private class DefaultErrorLogger : IErrorLogger
        {
            public void Write(string methodName, Exception ex) => ErrorLogger_Cls.Write(methodName, ex);
        }

        /// <summary>
        /// Error Logger Interface for DI
        /// </summary>
        public interface IErrorLogger
        {
            void Write(string methodName, Exception ex);
        }
        #endregion " Dependency Injection "

        #region " Enumeration (Preserved) "
        public enum DetailStatus
        {
            NEW,
            PROCESSING,
            CANCELLED,
            COMPLETED,
            ARPYMTGENERATED
        }
        #endregion " Enumeration "

        #region " Original Fields (Preserved) "
        private Type typ_BODTL = null;

        public long DocKey = -1;
        public string DocKeyFieldName = "DocKey";

        // To keep the Delete Keys
        public ArrayList DeleteKeys = new ArrayList();
        #endregion " Original Fields (Preserved) "

        #region " Abstract Properties (Must be Inherited) "
        public abstract string Name { get; }

        [MyAttributes(IsPrimaryKey = true, PrimaryKeyFieldName = "DtlKey", IsDeleteKey = true)]
        public abstract long PrimaryKey { get; set; }

        public abstract string DetailTableName { get; }
        public abstract string DetailTableQryName { get; }
        #endregion " Abstract Properties (Must be Inherited) "

        #region " Properties "
        public string PrimaryKeyFieldName { get; private set; } = "DtlKey";

        public List<BusinessBaseDTLV2_0_0_Cls> Details { get; private set; } = new List<BusinessBaseDTLV2_0_0_Cls>();

        private DataTable _DetailDataTable;
        public DataTable DetailDataTable
        {
            get
            {
                if (_DetailDataTable != null && _DetailDataTable.TableName == "") 
                    _DetailDataTable.TableName = "Tbl_" + this.GetType().Name;
                return _DetailDataTable;
            }
            set
            {
                _DetailDataTable = value;
                if (_DetailDataTable != null)
                {
                    _DetailDataTable.TableName = "Tbl_" + this.GetType().Name;
                    DetailOriDataTable = _DetailDataTable.Copy();
                }
            }
        }

        public DataTable DetailOriDataTable { get; private set; } // Original record(s) in datatable
        public DataTable DeletedDetailDataTable { get; private set; } // Deleted row(s)

        private long Llng_TempDtlKey = 0;
        public long TempDtlKey
        {
            get
            {
                return Llng_TempDtlKey -= 1;
            }
        }

        /// <summary>
        /// Gets validation errors for the current instance
        /// </summary>
        public List<string> ValidationErrors { get; private set; } = new List<string>();

        /// <summary>
        /// Indicates if the entity has validation errors
        /// </summary>
        public bool HasValidationErrors => ValidationErrors.Any();
        #endregion " Properties "

        #region " Constructor "
        public BusinessBaseDTLV2_0_0_Cls()
        {
            try
            {
                InitializeDataStructure();
                InitializeDependencies();
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.Constructor", ex);
                throw;
            }
        }

        private void InitializeDataStructure()
        {
            DetailDataTable = new DataTable(DetailTableName);
            typ_BODTL = this.GetType();

            // Initialize primary key field
            var propertyInfo = typ_BODTL.GetProperty(nameof(PrimaryKey));
            var attribute = propertyInfo?.GetCustomAttribute<MyAttributesAttribute>();
            if (attribute?.IsPrimaryKey == true)
            {
                PrimaryKeyFieldName = attribute.PrimaryKeyFieldName;
                DetailDataTable.Columns.Add(PrimaryKeyFieldName, propertyInfo.PropertyType);
            }
            if (attribute?.IsDeleteKey == true)
                DeleteKeys.Add(PrimaryKeyFieldName);

            // Initialize other fields using reflection
            var fieldInfos = typ_BODTL.GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                if (ShouldIncludeField(fieldInfo))
                {
                    var fieldAttribute = fieldInfo.GetCustomAttribute<MyAttributesAttribute>();
                    if (fieldAttribute?.IsDeleteKey == true)
                        DeleteKeys.Add(fieldInfo.Name);

                    if (fieldInfo.Name == nameof(DocKey))
                        DetailDataTable.Columns.Add(DocKeyFieldName, fieldInfo.FieldType);
                    else
                        DetailDataTable.Columns.Add(fieldInfo.Name, fieldInfo.FieldType);
                }
            }

            DetailDataTable.AcceptChanges();
            DetailOriDataTable = DetailDataTable.Copy();

            // Initialize deleted detail datatable
            if (DeleteKeys.Count > 0)
            {
                DeletedDetailDataTable = new DataTable("Del" + DetailTableName);
                foreach (string deleteKey in DeleteKeys)
                {
                    DeletedDetailDataTable.Columns.Add(deleteKey, typeof(long));
                }
                DeletedDetailDataTable.AcceptChanges();
            }

            // Collect child details
            CollectChildDetails();
        }

        private bool ShouldIncludeField(FieldInfo fieldInfo)
        {
            var skipFields = new[] { "typ_BODTL", "DeleteKeys", "DocKeyFieldName", "ValidationErrors" };
            return !skipFields.Contains(fieldInfo.Name) && 
                   fieldInfo.FieldType.FullName != "System.Data.DataTable" &&
                   !fieldInfo.FieldType.IsGenericType;
        }

        private void CollectChildDetails()
        {
            var propertyInfos = typ_BODTL.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var attribute = propertyInfo.GetCustomAttribute<MyAttributesAttribute>();
                if (attribute?.IsChildClass == true)
                {
                    var obj_Value = propertyInfo.GetValue(this);
                    if (obj_Value?.GetType().BaseType == typeof(BusinessBaseDTLV2_0_0_Cls))
                    {
                        Details.Add((BusinessBaseDTLV2_0_0_Cls)obj_Value);
                    }
                }
            }
        }

        private void InitializeDependencies()
        {
            // Initialize any dependencies from DI container
            // This allows for future extensibility without breaking existing code
        }
        #endregion " Constructor "

        #region " Validation Methods "
        /// <summary>
        /// Virtual method for custom validation logic
        /// </summary>
        protected virtual bool ValidateEntity()
        {
            ValidationErrors.Clear();

            // Basic validation can be added here
            // Child classes can override for specific validation

            return !HasValidationErrors;
        }

        /// <summary>
        /// Async validation method
        /// </summary>
        protected virtual async Task<bool> ValidateEntityAsync()
        {
            return await Task.Run(() => ValidateEntity()).ConfigureAwait(false);
        }
        #endregion " Validation Methods "

        #region " Save Record(s) Section (Preserved Original Logic) "
        public virtual bool UpdateRecDetailDataTable(DBSetting dbsetting)
        {
            bool boo_rtn = false;

            try
            {
                // Validation
                if (!ValidateEntity())
                {
                    Logger.Write($"{this.GetType().Name}.{nameof(UpdateRecDetailDataTable)}", 
                        new InvalidOperationException($"Validation failed: {string.Join(", ", ValidationErrors)}"));
                    return false;
                }

                // Process child details first
                if (Details.Count > 0)
                {
                    foreach (var bodtl in Details)
                        bodtl.UpdateRecDetailDataTable(dbsetting);
                }

                // Delete records section
                if (DeletedDetailDataTable?.Rows.Count > 0)
                {
                    for (int int_delidx = 0; int_delidx <= DeletedDetailDataTable.Rows.Count - 1; int_delidx++)
                    {
                        string str_cmdSub = $"DELETE FROM {DetailTableName} WHERE {PrimaryKeyFieldName}={DeletedDetailDataTable.Rows[int_delidx][PrimaryKeyFieldName]}";
                        int int_execOK = dbsetting.ExecuteNonQuery(str_cmdSub);

                        if (int_execOK < 0) return boo_rtn;
                    }

                    // Clear deleted datatable after successful deletion
                    DeletedDetailDataTable.Clear();
                }

                // Insert or Update records section
                foreach (DataRow dr in DetailDataTable.Rows)
                {
                    if ((long)dr[PrimaryKeyFieldName] < 0) 
                        dr[PrimaryKeyFieldName] = GetDtlKey(DetailTableName, dbsetting, PrimaryKeyFieldName);
                    if ((long)dr[DocKeyFieldName] < 0) 
                        dr[DocKeyFieldName] = DocKey;

                    DataRowValuesAssignment(dr);

                    string str_cmdSub = SQLUpdateStmt(dr);
                    int int_execOK = dbsetting.ExecuteNonQuery(str_cmdSub);

                    // If update failed, try insert
                    if (int_execOK <= 0)
                    {
                        str_cmdSub = SQLInsertStmt(dr);
                        int_execOK = dbsetting.ExecuteNonQuery(str_cmdSub);
                    }

                    if (int_execOK >= 0) boo_rtn = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(UpdateRecDetailDataTable)}", ex);
                boo_rtn = false;
            }

            return boo_rtn;
        }

        /// <summary>
        /// Async version of UpdateRecDetailDataTable
        /// </summary>
        public virtual async Task<OperationResult<bool>> UpdateRecDetailDataTableAsync(DBSetting dbsetting)
        {
            try
            {
                // Async validation
                if (!await ValidateEntityAsync().ConfigureAwait(false))
                {
                    return OperationResult<bool>.Failure($"Validation failed: {string.Join(", ", ValidationErrors)}");
                }

                var result = await Task.Run(() => UpdateRecDetailDataTable(dbsetting)).ConfigureAwait(false);
                
                return result 
                    ? OperationResult<bool>.Success(true, "Update completed successfully")
                    : OperationResult<bool>.Failure("Update failed");
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(UpdateRecDetailDataTableAsync)}", ex);
                return OperationResult<bool>.Failure(ex.Message, ex);
            }
        }
        #endregion " Save Record(s) Section "

        #region " Delete Record(s) Section (Preserved Original Logic) "
        public virtual string Delete(DBSetting dbSetting)
        {
            string str_rtn = "";

            try
            {
                // Delete sub table records first
                if (Details.Count > 0)
                {
                    foreach (var bodtl in Details)
                    {
                        str_rtn = bodtl.Delete(dbSetting);
                        if (!string.IsNullOrEmpty(str_rtn))
                            return $"Class Object [{bodtl.Name}] Failed to Delete due to reason: {str_rtn}";
                    }
                }

                // Delete main table record
                string str_sqlcmd = $"DELETE FROM {DetailTableName} WHERE {DocKeyFieldName}={DocKey}";
                int int_execOK = dbSetting.ExecuteNonQuery(str_sqlcmd);
                if (int_execOK < 0) 
                    str_rtn = $"Class Object [{Name}] Failed to Delete.";
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(Delete)}", ex);
                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        /// <summary>
        /// Async delete operation
        /// </summary>
        public virtual async Task<OperationResult> DeleteAsync(DBSetting dbSetting)
        {
            try
            {
                var result = await Task.Run(() =>
                {
                    var deleteResult = Delete(dbSetting);
                    return string.IsNullOrEmpty(deleteResult)
                        ? OperationResult.Success(message: "Delete completed successfully")
                        : OperationResult.Failure(deleteResult);
                }).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(DeleteAsync)}", ex);
                return OperationResult.Failure(ex.Message, ex);
            }
        }
        #endregion " Delete Record(s) Section "

        #region " Load Data Section (Preserved Original Logic) "
        public virtual void Load(string str_DocKey)
        {
            try
            {
                // Assign the DocKey
                DocKey = Convert.ToInt64(str_DocKey);

                DetailDataTable = myDBSetting.GetDataTable(
                    $"SELECT * FROM {DetailTableQryName} WHERE {DocKeyFieldName}={str_DocKey}", false);

                // Load other details
                if (Details.Count > 0)
                {
                    foreach (var bodtl in Details)
                    {
                        bodtl.Load(str_DocKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(Load)}", ex);
                throw;
            }
        }

        /// <summary>
        /// Async load operation
        /// </summary>
        public virtual async Task<OperationResult> LoadAsync(string str_DocKey)
        {
            try
            {
                await Task.Run(() => Load(str_DocKey)).ConfigureAwait(false);
                return OperationResult.Success(this, "Load completed successfully");
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(LoadAsync)}", ex);
                return OperationResult.Failure(ex.Message, ex);
            }
        }
        #endregion " Load Data Section "

        #region " Methods/Functions/Procedures (Preserved Original Logic) "
        public virtual string GetDtlStatusString(DetailStatus detailStatus)
        {
            string str_rtn = detailStatus.ToString().ToUpper();

            try
            {
                if (detailStatus == DetailStatus.ARPYMTGENERATED) 
                    str_rtn = "AR Payment Generated".ToUpper();
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(GetDtlStatusString)}", ex);
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
                Logger.Write($"{this.GetType().Name}.{nameof(GetBODTLDTFieldValue)}", ex);
                MessageBox.Show(ex.Message, $"{this.GetType().Name}.{nameof(GetBODTLDTFieldValue)}", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Logger.Write($"{this.GetType().Name}.{nameof(SetBODTLDTFieldValue)}", ex);
            }
        }

        public dynamic GetBODTLFieldValue(string str_FieldName)
        {
            dynamic dyn_rtn = null;

            try
            {
                if (str_FieldName == DocKeyFieldName) str_FieldName = nameof(DocKey);
                if (str_FieldName == PrimaryKeyFieldName) str_FieldName = nameof(PrimaryKey);

                var fieldInfo = typ_BODTL.GetField(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    var value = fieldInfo.GetValue(this);
                    if (fieldInfo.FieldType == typeof(DateTime) && value != null && (DateTime)value == DateTime.MinValue)
                        dyn_rtn = DBNull.Value;
                    else
                        dyn_rtn = value;
                }
                else
                {
                    var propertyInfo = typ_BODTL.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propertyInfo != null)
                    {
                        var value = propertyInfo.GetValue(this);
                        if (propertyInfo.PropertyType == typeof(DateTime) && value != null && (DateTime)value == DateTime.MinValue)
                            dyn_rtn = DBNull.Value;
                        else
                            dyn_rtn = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(GetBODTLFieldValue)}", ex);
                MessageBox.Show(ex.Message, $"{this.GetType().Name}.{nameof(GetBODTLFieldValue)}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dyn_rtn;
        }

        public string SetBODTLFieldValue(string str_FieldName, object obj_FieldValue)
        {
            string str_rtn = "";

            try
            {
                if (str_FieldName == DocKeyFieldName) str_FieldName = nameof(DocKey);

                var fieldInfo = typ_BODTL.GetField(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(this, obj_FieldValue);
                }
                else
                {
                    var propertyInfo = typ_BODTL.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propertyInfo?.CanWrite == true)
                    {
                        propertyInfo.SetValue(this, obj_FieldValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SetBODTLFieldValue)}", ex);
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
                        if (dr[int_idx] == DBNull.Value) dr[int_idx] = "";
                        break;
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Decimal":
                    case "System.Double":
                    case "System.Boolean":
                        if (dr[int_idx] == DBNull.Value) dr[int_idx] = 0;
                        break;
                    case "System.DateTime":
                        if (dr[int_idx] == DBNull.Value) dr[int_idx] = DateTime.MinValue;
                        break;
                }
            }
        }

        #region " For Details (Preserved Original Logic) "
        public virtual void AddNewDetail(BusinessBaseDTLV2_0_0_Cls BODTL)
        {
            try
            {
                var dr_newRow = DetailDataTable.NewRow();
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
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(AddNewDetail)}", ex);
                throw;
            }
        }

        public virtual void DeleteDetail(DataRow dr_del, bool boo_Remove = true)
        {
            try
            {
                if (DetailDataTable?.Rows.Count > 0)
                {
                    if (DeleteKeys.Count > 0)
                    {
                        var dr_newdel = DeletedDetailDataTable.NewRow();
                        foreach (string deleteKey in DeleteKeys)
                        {
                            dr_newdel[deleteKey] = dr_del[deleteKey];
                        }

                        DeletedDetailDataTable.Rows.Add(dr_newdel);
                        DeletedDetailDataTable.AcceptChanges();
                    }

                    if (boo_Remove) DetailDataTable.Rows.Remove(dr_del);
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(DeleteDetail)}", ex);
                throw;
            }
        }

        public virtual void DeleteAllDetail()
        {
            try
            {
                if (DetailDataTable?.Rows.Count > 0)
                {
                    if (DeleteKeys.Count > 0)
                    {
                        for (int int_DtlIdx = 0; int_DtlIdx <= DetailDataTable.Rows.Count - 1; int_DtlIdx++)
                        {
                            var dr_newdel = DeletedDetailDataTable.NewRow();
                            foreach (string deleteKey in DeleteKeys)
                            {
                                dr_newdel[deleteKey] = DetailDataTable.Rows[int_DtlIdx][deleteKey];
                            }

                            DeletedDetailDataTable.Rows.Add(dr_newdel);
                            DeletedDetailDataTable.AcceptChanges();
                        }
                    }

                    DetailDataTable.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(DeleteAllDetail)}", ex);
                throw;
            }
        }
        #endregion " For Details "
        #endregion " Methods/Functions/Procedures "

        #region " Construct SQL Stmt (Preserved Original Logic) "
        public string SQLUpdateStmt(DataRow dr)
        {
            string str_rtn = "UPDATE " + SQLString(DetailTableName) + " SET ";

            try
            {
                for (int int_i = 0; int_i < dr.Table.Columns.Count; int_i++)
                {
                    // Skip primary key
                    if (dr.Table.Columns[int_i].ColumnName.Equals(PrimaryKeyFieldName)) continue;

                    // Check for display field attribute
                    var fieldInfo = this.GetType().GetField(dr.Table.Columns[int_i].ColumnName);
                    var attribute = fieldInfo?.GetCustomAttribute<MyAttributesAttribute>();
                    if (attribute?.IsDisplayField == true) continue;

                    var obj_Value = dr[int_i];

                    // Handle null values
                    if (obj_Value == DBNull.Value || (obj_Value.GetType().Name == "DateTime" && Convert.ToDateTime(obj_Value) == DateTime.MinValue))
                    {
                        str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "]=null,";
                        continue;
                    }

                    str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "]=";
                    str_rtn += FormatSQLValue(obj_Value) + ", ";
                }

                // Remove trailing comma and add WHERE clause
                if (str_rtn.Length > 2) str_rtn = str_rtn.Substring(0, str_rtn.Length - 2);
                str_rtn += $" WHERE [{PrimaryKeyFieldName}]={dr[PrimaryKeyFieldName]}";
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SQLUpdateStmt)}", ex);
            }

            return str_rtn;
        }

        public string SQLInsertStmt(DataRow dr)
        {
            string str_rtn = "INSERT INTO " + SQLString(DetailTableName) + " (";
            string str_Values = "VALUES(";

            try
            {
                for (int int_i = 0; int_i < dr.Table.Columns.Count; int_i++)
                {
                    if (dr.Table.Columns[int_i].ColumnName != PrimaryKeyFieldName && 
                        dr.Table.Columns[int_i].ColumnName != DocKeyFieldName)
                    {
                        // Check for display field attribute
                        var fieldInfo = this.GetType().GetField(dr.Table.Columns[int_i].ColumnName);
                        var attribute = fieldInfo?.GetCustomAttribute<MyAttributesAttribute>();
                        if (attribute?.IsDisplayField == true) continue;
                    }

                    var obj_Value = dr[int_i];

                    // Skip null values
                    if (obj_Value == DBNull.Value || (obj_Value.GetType().Name == "DateTime" && Convert.ToDateTime(obj_Value) == DateTime.MinValue)) 
                        continue;

                    str_rtn += "[" + dr.Table.Columns[int_i].ColumnName + "], ";
                    str_Values += FormatSQLValue(obj_Value) + ",";
                }

                // Combine fields and values
                if (str_rtn.Length > 2) str_rtn = str_rtn.Substring(0, str_rtn.Length - 2) + ") ";
                if (str_Values.Length > 2) str_Values = str_Values.Substring(0, str_Values.Length - 1) + ") ";
                str_rtn = str_rtn + str_Values;
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SQLInsertStmt)}", ex);
            }

            return str_rtn;
        }

        private string FormatSQLValue(object obj_Value)
        {
            switch (obj_Value.GetType().Name)
            {
                case "String":
                    return "N'" + SQLString(obj_Value.ToString()) + "'";
                case "DateTime":
                    return "'" + Convert.ToDateTime(obj_Value).ToString("yyyy-MM-dd") + "'";
                case "Int":
                case "Int32":
                    return Convert.ToInt32(obj_Value).ToString();
                case "Int64":
                    return Convert.ToInt64(obj_Value).ToString();
                case "Decimal":
                    return Convert.ToDecimal(obj_Value).ToString();
                case "Boolean":
                    return (Convert.ToBoolean(obj_Value) ? 1 : 0).ToString();
                case "DetailStatus":
                    return Convert.ToInt32(obj_Value).ToString();
                default:
                    return "'" + SQLString(obj_Value.ToString()) + "'";
            }
        }
        #endregion " Construct SQL Stmt "

        #region " Utility Methods "
        /// <summary>
        /// Copy values from another BusinessBaseDTL instance
        /// </summary>
        public virtual void CopyFrom(BusinessBaseDTLV2_0_0_Cls source)
        {
            if (source == null) return;

            try
            {
                var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.Name != nameof(DocKey) && field.Name != nameof(PrimaryKey))
                    {
                        var sourceValue = source.GetBODTLFieldValue(field.Name);
                        if (sourceValue != null)
                        {
                            SetBODTLFieldValue(field.Name, sourceValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(CopyFrom)}", ex);
                throw;
            }
        }

        /// <summary>
        /// Create a deep copy of the current instance
        /// </summary>
        public virtual T Clone<T>() where T : BusinessBaseDTLV2_0_0_Cls, new()
        {
            try
            {
                var clone = new T();
                clone.CopyFrom(this);
                return clone;
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(Clone)}", ex);
                throw;
            }
        }
        #endregion " Utility Methods "
    }
}
