using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

using AutoCount.Data;

using VTACPluginBase.Classes.TextLogger;
using VTACPluginBase.Classes.Helpers;
using VTACPluginBase.Classes.DI;

using Sales = VTACPluginBase.Classes.AutoCountGenerator.CategorySales_Cls;
using GLARAP = VTACPluginBase.Classes.AutoCountGenerator.CategoryGLARAP_Cls;

using static VTACPluginBase.Classes.Helpers.AutoCountHelper;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.BusinessBase
{
    /// <summary>
    /// V2.0.0 - Modern Business Base Class with Dependency Injection and Async Support
    /// Preserves all original functionality while adding modern patterns for .NET Framework 4.8
    /// </summary>
    public abstract class BusinessBaseV2_0_0_Cls
    {
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

        #region " Attribute Classes "
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
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

        #region " Protected DataTables "
        protected DataTable mydt_Master = null;
        protected DataTable mydt_MasterOri = null;
        private Type typ_BO = null;
        #endregion " Protected DataTables "

        #region " Abstract Properties (Must be Inherited) "
        [MyAttributes(IsDisplayField = true)]
        public abstract string Name { get; }

        protected abstract string dbTableNameMaster { get; }
        protected abstract string dbQueryNameMaster { get; }

        [MyAttributes(IsPrimaryKey = true, PrimaryKeyFieldName = "DocKey", IsDeleteKey = true)]
        public abstract long PrimaryKey { get; set; }

        protected abstract string SysConfigDocNoFormat { get; }
        protected abstract string DocNoFormat { get; }

        [MyAttributes(IsMainChildClass = true)]
        public abstract BusinessBaseDTLV2_0_0_Cls Detail { get; }
        #endregion " Abstract Properties (Must be Inherited) "

        #region " Standard Properties "
        [MyAttributes(IsDisplayField = true)]
        public string PrimaryKeyFieldName { get; private set; } = "DocKey";

        [MyAttributes(IsChildClass = true)]
        public List<BusinessBaseDTLV2_0_0_Cls> OtherDetails { get; private set; } = new List<BusinessBaseDTLV2_0_0_Cls>();

        [MyAttributes(IsDataTable = true)]
        public DataTable MasterDataTable { get => mydt_Master; }

        [MyAttributes(IsDisplayField = true)]
        public string MasterTableName { get => dbTableNameMaster; }
        [MyAttributes(IsDisplayField = true)]
        public string MasterTableQryName { get => dbQueryNameMaster; }

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

        /// <summary>
        /// Gets validation errors for the current instance
        /// </summary>
        public List<string> ValidationErrors { get; private set; } = new List<string>();

        /// <summary>
        /// Indicates if the entity has validation errors
        /// </summary>
        public bool HasValidationErrors => ValidationErrors.Any();
        #endregion " Standard Properties "

        #region " Constructor "
        public BusinessBaseV2_0_0_Cls()
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
                if (obj_Value != null && obj_Value.GetType().BaseType == typeof(BusinessBaseDTLV2_0_0_Cls))
                    ((BusinessBaseDTLV2_0_0_Cls)obj_Value).DocKeyFieldName = PrimaryKeyFieldName;
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
                    if (obj_Value.GetType().BaseType == typeof(BusinessBaseDTLV2_0_0_Cls))
                    {
                        ((BusinessBaseDTLV2_0_0_Cls)obj_Value).DocKeyFieldName = PrimaryKeyFieldName;

                        OtherDetails.Add((BusinessBaseDTLV2_0_0_Cls)obj_Value);
                    }
                }
            }
            #endregion " to get DisplayKey and collect ALL OtherDetails "
        }
        #endregion " Constructor "

        #region " Validation Methods "
        /// <summary>
        /// Virtual method for custom validation logic
        /// </summary>
        /// <returns>True if validation passes</returns>
        protected virtual bool ValidateEntity()
        {
            ValidationErrors.Clear();

            // Basic validation
            if (string.IsNullOrWhiteSpace(DocNo))
                ValidationErrors.Add("Document number is required");

            if (DocDate == DateTime.MinValue)
                ValidationErrors.Add("Document date is required");

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

        #region " Save Operations - Restored Original Design "
        /// <summary>
        /// Main save operation - preserves original synchronous behavior with Transaction management
        /// </summary>
        public virtual string Save()
        {
            string str_rtn = "";
            DBSetting newDBSetting = myDBSetting.StartTransaction();

            try
            {
                string str_cmdMast = "";
                int int_execOK = 0;

                // Validation
                if (!ValidateEntity())
                {
                    return $"Validation failed: {string.Join(", ", ValidationErrors)}";
                }

                // to assign DocKey value
                if (PrimaryKey <= 0) PrimaryKey = GetDocKey(dbTableNameMaster, PrimaryKeyFieldName);

                // ----------------------------------------------------------------------------------------------------------------------------
                // here to check the DocNo duplication
                // ===================================
                PropertyInfo propertyInfo = typ_BO.GetProperty(DisplayKeyFieldName);
                object obj_DisplayKey = propertyInfo.GetValue(this);

                if (newDBSetting.GetDataTable($"SELECT * FROM {this.dbTableNameMaster} WHERE {DisplayKeyFieldName}='{SQLString(obj_DisplayKey.ToString())}' AND {PrimaryKeyFieldName}<>{PrimaryKey}", false).Rows.Count > 0)
                {
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
                        foreach (BusinessBaseDTLV2_0_0_Cls otherbodtl in OtherDetails)
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

                Logger.Write(this.GetType().Name + "." + nameof(Save) + "()", ex);

                // if has errors then return the error messages
                str_rtn = ex.Message;
            }
            finally
            {
                newDBSetting.EndTransaction();

                if (Detail != null) Detail.DeletedDetailDataTable.Clear();

                if (OtherDetails.Count > 0)
                {
                    foreach (BusinessBaseDTLV2_0_0_Cls otherbodtl in OtherDetails)
                        otherbodtl.DeletedDetailDataTable.Clear();
                }
            }

            return str_rtn;
        }

        /// <summary>
        /// Async save operation (modern addition)
        /// </summary>
        public virtual async Task<OperationResult> SaveAsync()
        {
            try
            {
                var result = await Task.Run(() =>
                {
                    var saveResult = Save();
                    return string.IsNullOrEmpty(saveResult)
                        ? OperationResult.Success(this, "Save completed successfully")
                        : OperationResult.Failure(saveResult);
                }).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SaveAsync)}", ex);
                return OperationResult.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Save with external DBSetting (for advanced scenarios)
        /// </summary>
        public virtual string Save(DBSetting dbSetting)
        {
            // For compatibility with advanced scenarios, but main Save() is preferred
            string str_rtn = "";

            try
            {
                // Validation
                if (!ValidateEntity())
                {
                    return $"Validation failed: {string.Join(", ", ValidationErrors)}";
                }

                // Core save implementation
                str_rtn = PerformSave(dbSetting);
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(Save)}", ex);
                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        /// <summary>
        /// Template method hooks for save operation
        /// </summary>
        protected virtual string BeforeSave(DBSetting dbSetting) => "";
        protected virtual string AfterSave(DBSetting dbSetting) => "";

        /// <summary>
        /// Core save implementation - preserves original logic
        /// </summary>
        protected virtual string PerformSave(DBSetting dbSetting)
        {
            string str_rtn = "";

            try
            {
                // Generate new DocKey if needed (use PrimaryKey instead of DocKey)
                if (PrimaryKey <= 0)
                {
                    PrimaryKey = GetDocKey(dbTableNameMaster, PrimaryKeyFieldName);
                }

                // Update record in main table
                str_rtn = UpdateRecMainDataTable(dbSetting);
                if (!string.IsNullOrEmpty(str_rtn)) return str_rtn;

                // Save main detail
                if (Detail != null)
                {
                    Detail.DocKey = PrimaryKey;
                    Detail.DocKeyFieldName = PrimaryKeyFieldName;

                    if (!Detail.UpdateRecDetailDataTable(dbSetting))
                    {
                        return $"Failed to save detail: {Detail.Name}";
                    }
                }

                // Save other details
                foreach (var detail in OtherDetails)
                {
                    detail.DocKey = PrimaryKey;
                    detail.DocKeyFieldName = PrimaryKeyFieldName;

                    if (!detail.UpdateRecDetailDataTable(dbSetting))
                    {
                        return $"Failed to save detail: {detail.Name}";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(PerformSave)}", ex);
                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        /// <summary>
        /// Update main data table - uses Property-based SQL generation (restored original approach)
        /// </summary>
        protected virtual string UpdateRecMainDataTable(DBSetting dbSetting)
        {
            string str_rtn = "";

            try
            {
                // Try UPDATE first
                string str_cmdSub = SQLUpdateStmt();
                int int_execOK = dbSetting.ExecuteNonQuery(str_cmdSub);

                // If UPDATE failed (record doesn't exist), try INSERT
                if (int_execOK <= 0)
                {
                    str_cmdSub = SQLInsertStmt();
                    int_execOK = dbSetting.ExecuteNonQuery(str_cmdSub);

                    if (int_execOK <= 0)
                        str_rtn = "Failed to insert/update record";
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(UpdateRecMainDataTable)}", ex);
                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        public abstract void RollBackDetailInfo();
        #endregion " Save Operations "

        #region " Delete Operations "
        /// <summary>
        /// Delete operation - preserves original logic
        /// </summary>
        public virtual string Delete(DBSetting dbSetting)
        {
            string str_rtn = "";

            try
            {
                // Delete main detail first
                if (Detail != null)
                {
                    str_rtn = Detail.Delete(dbSetting);
                    if (!string.IsNullOrEmpty(str_rtn))
                        return $"Failed to delete detail [{Detail.Name}]: {str_rtn}";
                }

                // Delete other details
                foreach (var detail in OtherDetails)
                {
                    str_rtn = detail.Delete(dbSetting);
                    if (!string.IsNullOrEmpty(str_rtn))
                        return $"Failed to delete detail [{detail.Name}]: {str_rtn}";
                }

                // Delete main record
                string str_sqlcmd = $"DELETE FROM {dbTableNameMaster} WHERE {PrimaryKeyFieldName}={PrimaryKey}";
                int int_execOK = dbSetting.ExecuteNonQuery(str_sqlcmd);

                if (int_execOK < 0)
                    str_rtn = $"Failed to delete main record for [{Name}]";
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
        #endregion " Delete Operations "

        #region " Load Operations "
        /// <summary>
        /// Load data - preserves original logic
        /// </summary>
        public virtual void Load(string str_DocKey)
        {
            try
            {
                PrimaryKey = Convert.ToInt64(str_DocKey);

                mydt_Master = myDBSetting.GetDataTable(
                    $"SELECT * FROM {dbQueryNameMaster} WHERE {PrimaryKeyFieldName}={str_DocKey}", false);

                if (mydt_Master.Rows.Count > 0)
                {
                    LoadFieldsFromDataTable(mydt_Master.Rows[0]);
                }

                // Load main detail
                if (Detail != null)
                {
                    Detail.Load(str_DocKey);
                }

                // Load other details
                foreach (var detail in OtherDetails)
                {
                    detail.Load(str_DocKey);
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

        /// <summary>
        /// Load fields from DataRow to object properties
        /// </summary>
        protected virtual void LoadFieldsFromDataTable(DataRow dr)
        {
            try
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    if (dr[column] != DBNull.Value)
                    {
                        SetBOFieldValue(column.ColumnName, dr[column]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(LoadFieldsFromDataTable)}", ex);
                throw;
            }
        }
        #endregion " Load Operations "

        #region " AutoCount Document Generation (Preserved Original Logic) "
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
        #endregion " AutoCount Document Generation "

        #region " Field Access Methods (Preserved Original Logic) "
        public dynamic GetBOFieldValue(string str_FieldName)
        {
            dynamic dyn_rtn = null;

            try
            {
                if (str_FieldName == PrimaryKeyFieldName) str_FieldName = nameof(PrimaryKey);

                var fieldInfo = typ_BO.GetField(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                    var propertyInfo = typ_BO.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                Logger.Write($"{this.GetType().Name}.{nameof(GetBOFieldValue)}", ex);
                MessageBox.Show(ex.Message, $"{this.GetType().Name}.{nameof(GetBOFieldValue)}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dyn_rtn;
        }

        public string SetBOFieldValue(string str_FieldName, object obj_FieldValue)
        {
            string str_rtn = "";

            try
            {
                if (str_FieldName == PrimaryKeyFieldName) str_FieldName = nameof(PrimaryKey);

                var fieldInfo = typ_BO.GetField(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(this, obj_FieldValue);
                }
                else
                {
                    var propertyInfo = typ_BO.GetProperty(str_FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (propertyInfo?.CanWrite == true)
                    {
                        propertyInfo.SetValue(this, obj_FieldValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SetBOFieldValue)}", ex);
                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " Field Access Methods "

        #region " SQL Generation Methods (Restored Original Property-Based Logic) "

        /// <summary>
        /// SQL UPDATE Statement - Uses Property-based approach like original version
        /// Only updates LastModified and LastModifiedUserID for audit compliance
        /// </summary>
        protected virtual string SQLUpdateStmt()
        {
            string str_rtn = "UPDATE " + SQLString(dbTableNameMaster) + " SET ";

            try
            {
                object obj_Value = null;
                MyAttributesAttribute attribute = null;

                PropertyInfo[] propertyInfos = typ_BO.GetProperties();
                for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
                {
                    // Skip unnecessary fields - preserve original filtering logic
                    if (propertyInfos[int_i].Name.Contains("Temp") ||
                        propertyInfos[int_i].Name.Contains("DocDesc")) continue;

                    // Check attributes and skip display fields, data tables, child classes
                    attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
                    if (attribute != null && (attribute.IsPrimaryKey || attribute.IsDisplayField ||
                        attribute.IsDataTable)) continue;

                    // Skip audit creation fields in UPDATE to preserve original values
                    if (propertyInfos[int_i].Name == "CreatedTimeStamp" ||
                        propertyInfos[int_i].Name == "CreatedUserID") continue;

                    // Get property value
                    obj_Value = propertyInfos[int_i].GetValue(this);

                    // Handle null values
                    if (obj_Value == null || (propertyInfos[int_i].PropertyType.Name == "DateTime" &&
                        Convert.ToDateTime(obj_Value) == DateTime.MinValue))
                    {
                        // If detected null, update back to null
                        str_rtn += "[" + propertyInfos[int_i].Name + "]=null,";
                        continue;
                    }

                    str_rtn += "[" + propertyInfos[int_i].Name + "]=";

                    // Format value based on type
                    str_rtn += FormatSQLValueForProperty(obj_Value, propertyInfos[int_i].PropertyType.Name) + ",";
                }

                // Add audit fields for UPDATE (preserve original behavior)
                str_rtn += "[LastModified]=getdate(), ";

                // Get current user from AutoCount session if available
                try
                {
                    var currentUser = AutoCount.Authentication.UserSession.CurrentUserSession?.LoginUserID ?? "SYSTEM";
                    str_rtn += "[LastModifiedUserID]=N'" + SQLString(currentUser) + "' ";
                }
                catch
                {
                    str_rtn += "[LastModifiedUserID]=N'SYSTEM' ";
                }

                str_rtn += $" WHERE [{PrimaryKeyFieldName}]=" + PrimaryKey;
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SQLUpdateStmt)}", ex);
            }

            return str_rtn;
        }

        /// <summary>
        /// SQL INSERT Statement - Uses Property-based approach like original version
        /// Includes all audit fields for new records
        /// </summary>
        protected virtual string SQLInsertStmt()
        {
            string str_rtn = "INSERT INTO " + SQLString(dbTableNameMaster) + " (";
            string str_Values = "VALUES(";

            try
            {
                object obj_Value = null;
                MyAttributesAttribute attribute = null;
                string str_FieldName = "";

                PropertyInfo[] propertyInfos = typ_BO.GetProperties();
                for (int int_i = 0; int_i < propertyInfos.Length; int_i++)
                {
                    // Skip unnecessary fields - preserve original filtering logic
                    if (propertyInfos[int_i].Name.Contains("Temp") ||
                        propertyInfos[int_i].Name.Contains("DocDesc")) continue;

                    // Check attributes and skip display fields, data tables, child classes
                    attribute = (MyAttributesAttribute)propertyInfos[int_i].GetCustomAttribute(typeof(MyAttributesAttribute));
                    if (attribute != null && (attribute.IsDisplayField || attribute.IsDataTable)) continue;

                    // Get property value
                    obj_Value = propertyInfos[int_i].GetValue(this);

                    // Skip null values for INSERT
                    if (obj_Value == null || (propertyInfos[int_i].PropertyType.Name == "DateTime" &&
                        Convert.ToDateTime(obj_Value) == DateTime.MinValue)) continue;

                    str_FieldName = propertyInfos[int_i].Name;
                    if (str_FieldName == nameof(PrimaryKey)) str_FieldName = PrimaryKeyFieldName;

                    str_rtn += "[" + str_FieldName + "], ";
                    str_Values += FormatSQLValueForProperty(obj_Value, propertyInfos[int_i].PropertyType.Name) + ",";
                }

                // Add audit fields for INSERT (all audit fields for new records)
                str_rtn += "[CreatedTimeStamp], [LastModified], ";
                str_Values += "getdate(), getdate(), ";

                try
                {
                    var currentUser = AutoCount.Authentication.UserSession.CurrentUserSession?.LoginUserID ?? "SYSTEM";
                    str_rtn += "[CreatedUserID], [LastModifiedUserID], ";
                    str_Values += "N'" + SQLString(currentUser) + "', N'" + SQLString(currentUser) + "', ";
                }
                catch
                {
                    str_rtn += "[CreatedUserID], [LastModifiedUserID], ";
                    str_Values += "N'SYSTEM', N'SYSTEM', ";
                }

                // Remove trailing commas and close statements
                if (str_rtn.EndsWith(", ")) str_rtn = str_rtn.Substring(0, str_rtn.Length - 2) + ") ";
                if (str_Values.EndsWith(", ")) str_Values = str_Values.Substring(0, str_Values.Length - 2) + ") ";

                str_rtn = str_rtn + str_Values;
            }
            catch (Exception ex)
            {
                Logger.Write($"{this.GetType().Name}.{nameof(SQLInsertStmt)}", ex);
            }

            return str_rtn;
        }

        /// <summary>
        /// Format SQL value based on property type (enhanced from original)
        /// </summary>
        private string FormatSQLValueForProperty(object obj_Value, string typeName)
        {
            switch (typeName)
            {
                case "String":
                    return "N'" + SQLString(obj_Value.ToString()) + "'";
                case "DateTime":
                    return "'" + Convert.ToDateTime(obj_Value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                case "Int":
                case "Int32":
                    return Convert.ToInt32(obj_Value).ToString();
                case "Int64":
                    return Convert.ToInt64(obj_Value).ToString();
                case "Decimal":
                    return Convert.ToDecimal(obj_Value).ToString();
                case "Boolean":
                    return (Convert.ToBoolean(obj_Value) ? 1 : 0).ToString();
                case "DocumentStatus":
                    return ((int)obj_Value).ToString();
                default:
                    return "'" + SQLString(obj_Value.ToString()) + "'";
            }
        }

        /// <summary>
        /// Legacy DataRow-based approach for backward compatibility
        /// </summary>
        private void DataRowValuesAssignment(DataRow dr)
        {
            for (int int_idx = 0; int_idx <= dr.Table.Columns.Count - 1; int_idx++)
            {
                if (dr.Table.Columns[int_idx].ColumnName == PrimaryKeyFieldName)
                {
                    dr[int_idx] = PrimaryKey;
                    continue;
                }

                dr[int_idx] = GetBOFieldValue(dr.Table.Columns[int_idx].ColumnName) ?? DBNull.Value;
            }
        }
        #endregion " SQL Generation Methods "

        #region " Utility Methods "
        /// <summary>
        /// Copy values from another BusinessBase instance
        /// </summary>
        public virtual void CopyFrom(BusinessBaseV2_0_0_Cls source)
        {
            if (source == null) return;

            try
            {
                var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.Name != nameof(PrimaryKey))
                    {
                        var sourceValue = source.GetBOFieldValue(field.Name);
                        if (sourceValue != null)
                        {
                            SetBOFieldValue(field.Name, sourceValue);
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
        public virtual T Clone<T>() where T : BusinessBaseV2_0_0_Cls, new()
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
