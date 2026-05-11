using System;
using System.Data;

using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.Helpers
{
    /// <summary>
    /// SysConfig/SysRef Table 版本
    /// Added by SCChang's Copilot on 20251208: support both legacy and new table structures
    /// </summary>
    public enum SysTableVersion
    {
        /// <summary>
        /// V1 (2018): 舊版簡易結構
        /// Tables: z_SysConfig (ConfigName, ConfigDesc, ConfigDataType, ConfigValue)
        ///         z_SysRef (RefID, RefGroup, RefName, RefDesc, RefSeq, RefValue)
        /// </summary>
        V1_2018 = 1,

        /// <summary>
        /// V2 (2025): 新版完整結構
        /// Tables: zVTS_SysConfig (ConfigModule, ConfigKey, ConfigValue, DataType, ConfigDescription)
        ///         zVTS_SysRef (RefCategory, RefCode, RefValue, RefDescription, SortOrder)
        /// 首次使用: BookHub v2.2.1.0 (2025-09)
        /// 特點: 支援 Module 分類、Audit Trail、IsActive
        /// </summary>
        V2_2025 = 2,
    }

    public class SystemHelper
    {
        #region " Configurable Table Names " //added by SCChang on 20251106: make table names dynamic while maintaining backward compatibility

        private static string _sysConfigTableName = "z_SysConfig";
        private static string _sysRefTableName = "z_SysRef";

        /// <summary>
        /// SysConfig table name (configurable, default: z_SysConfig for backward compatibility)
        /// </summary>
        public static string SysConfigTableName
        {
            get { return _sysConfigTableName; }
        }

        /// <summary>
        /// SysRef table name (configurable, default: z_SysRef for backward compatibility)
        /// </summary>
        public static string SysRefTableName
        {
            get { return _sysRefTableName; }
        }

        /// <summary>
        /// Set custom SysConfig table name (call this in your plugin's initialization)
        /// </summary>
        /// <param name="tableName">Custom table name (e.g., "zVTS_SysConfig")</param>
        public static void SetSysConfigTableName(string tableName)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
                _sysConfigTableName = tableName;
        }

        /// <summary>
        /// Set custom SysRef table name (call this in your plugin's initialization)
        /// </summary>
        /// <param name="tableName">Custom table name (e.g., "zVTS_SysRef")</param>
        public static void SetSysRefTableName(string tableName)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
                _sysRefTableName = tableName;
        }

        #endregion " Configurable Table Names "

        #region " Table Version Support " //added by SCChang's Copilot on 20251208: support both legacy and new table structures

        private static SysTableVersion _sysConfigTableVersion = SysTableVersion.V1_2018;
        private static SysTableVersion _sysRefTableVersion = SysTableVersion.V1_2018;

        /// <summary>
        /// Current SysConfig table version (Legacy or New)
        /// </summary>
        public static SysTableVersion SysConfigTableVersion => _sysConfigTableVersion;

        /// <summary>
        /// Current SysRef table version (Legacy or New)
        /// </summary>
        public static SysTableVersion SysRefTableVersion => _sysRefTableVersion;

        /// <summary>
        /// Set SysConfig table version (call this in your plugin's initialization)
        /// </summary>
        /// <param name="version">V1_2018 = z_SysConfig structure, V2_2025 = zVTS_SysConfig structure</param>
        public static void SetSysConfigTableVersion(SysTableVersion version)
        {
            _sysConfigTableVersion = version;
        }

        /// <summary>
        /// Set SysRef table version (call this in your plugin's initialization)
        /// </summary>
        /// <param name="version">V1_2018 = z_SysRef structure, V2_2025 = zVTS_SysRef structure</param>
        public static void SetSysRefTableVersion(SysTableVersion version)
        {
            _sysRefTableVersion = version;
        }

        #endregion " Table Version Support "

        #region " SysConfig Methods "

        /// <summary>
        /// Use to set value in SysConfig table (supports both Legacy and New structure)
        /// For New structure, use the overload with configModule parameter
        /// </summary>
        /// <remarks>This method uses Legacy structure. For New structure, call SetSysConfigValue(module, key, value, description)</remarks>
        /// <remarks></remarks>
        public static bool SetSysConfigValue(string str_ConfigName, object obj_Value, string str_Description = "")
        {
            bool boo_rtn = false;

            try
            {
                //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysConfig"
                //DataTable dt_config = myDBSetting.GetDataTable("select * from z_SysConfig where ConfigName='" + str_ConfigName + "'", false);
                DataTable dt_config = myDBSetting.GetDataTable($"select * from {SysConfigTableName} where ConfigName='" + str_ConfigName + "'", false);
                if (dt_config != null)
                {
                    if (dt_config.Rows.Count <= 0)
                    {
                        // until here means no any record create yet, so just create and update it
                        DataRow dr_new = dt_config.NewRow();

                        dr_new["ConfigName"] = str_ConfigName;
                        dr_new["ConfigDesc"] = str_Description;
                        // '' ''dr_new("Size") = 0
                        dr_new["ConfigValueMin"] = 0;
                        dr_new["ConfigValueMax"] = 0;

                        if (obj_Value.GetType().ToString() == typeof(string).ToString())
                        {
                            dr_new["ConfigValue"] = System.Convert.ToString(obj_Value);
                            dr_new["ConfigDataType"] = "STRING";
                        }
                        if (obj_Value.GetType().ToString() == typeof(bool).ToString())
                        {
                            dr_new["ConfigValue"] = System.Convert.ToBoolean(obj_Value).ToString();
                            dr_new["ConfigDataType"] = "BOOL";
                        }
                        if (obj_Value.GetType().ToString() == typeof(DateTime).ToString())
                        {
                            dr_new["ConfigValue"] = System.Convert.ToDateTime(obj_Value).ToString();
                            dr_new["ConfigDataType"] = "DATE";
                        }
                        if (obj_Value.GetType().ToString() == typeof(int).ToString() ||
                            obj_Value.GetType().ToString() == typeof(byte).ToString() ||
                            obj_Value.GetType().ToString() == typeof(Int16).ToString() ||
                            obj_Value.GetType().ToString() == typeof(Int32).ToString() ||
                            obj_Value.GetType().ToString() == typeof(Int64).ToString())
                        {
                            dr_new["ConfigValue"] = System.Convert.ToInt32(obj_Value).ToString();
                            dr_new["ConfigDataType"] = "INTEGER";
                            dr_new["ConfigValueMin"] = int.MinValue;
                            dr_new["ConfigValueMax"] = int.MaxValue;
                        }
                        if (obj_Value.GetType().ToString() == typeof(decimal).ToString())
                        {
                            dr_new["ConfigValue"] = System.Convert.ToDecimal(obj_Value).ToString();
                            dr_new["ConfigDataType"] = "DECIMAL";
                            dr_new["ConfigValueMin"] = -1.79E+308;
                            dr_new["ConfigValueMax"] = 1.79E+308;
                        }
                        if (obj_Value.GetType().ToString() == typeof(double).ToString() ||
                            obj_Value.GetType().ToString() == typeof(float).ToString())
                        {
                            dr_new["ConfigValue"] = System.Convert.ToDouble(obj_Value).ToString();
                            dr_new["ConfigDataType"] = "DOUBLE";
                            dr_new["ConfigValueMin"] = -1.79E+308;
                            dr_new["ConfigValueMax"] = 1.79E+308;
                        }

                        dt_config.Rows.Add(dr_new);
                    }
                    else
                        // until here means oredy got record, so just update it only
                        switch (dt_config.Rows[0]["ConfigDataType"].ToString().ToUpper())
                        {
                            case "BOOLEAN":
                            case "BOOL":
                                {
                                    dt_config.Rows[0]["ConfigValue"] = System.Convert.ToBoolean(obj_Value).ToString();
                                    break;
                                }

                            case "DATE":
                            case "DATE/TIME":
                            case "TIME":
                                {
                                    dt_config.Rows[0]["ConfigValue"] = System.Convert.ToDateTime(obj_Value).ToString();
                                    break;
                                }

                            case "INTEGER":
                            case "INT":
                                {
                                    dt_config.Rows[0]["ConfigValue"] = System.Convert.ToInt32(obj_Value).ToString();
                                    break;
                                }

                            case "DECIMAL":
                            case "DEC":
                                {
                                    dt_config.Rows[0]["ConfigValue"] = System.Convert.ToDecimal(obj_Value).ToString();
                                    break;
                                }

                            case "CURRENCY":
                            case "CURR":
                            case "DOUBLE":
                            case "DBL":
                            case "SINGLE":
                            case "SNG":
                                {
                                    dt_config.Rows[0]["ConfigValue"] = System.Convert.ToDouble(obj_Value).ToString();
                                    break;
                                }

                            default:
                                {
                                    dt_config.Rows[0]["ConfigValue"] = System.Convert.ToString(obj_Value).ToString();
                                    break;
                                }
                        }
                }

                //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysConfig"
                //if (myDBSetting.SimpleSaveDataTable(dt_config, "select * from z_SysConfig") > 0)
                if (myDBSetting.SimpleSaveDataTable(dt_config, $"select * from {SysConfigTableName}") > 0)
                    boo_rtn = true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(SetSysConfigValue)}()", ex);
            }

            return boo_rtn;
        }

        /// <summary>
        /// Use to set value in SysConfig table (New structure: zVTS_SysConfig)
        /// Added by SCChang's Copilot on 20251208: support new table structure
        /// </summary>
        /// <param name="configModule">Module name (e.g., "OutstandingPO", "DeliveryManagement")</param>
        /// <param name="configKey">Configuration key</param>
        /// <param name="value">Value to set</param>
        /// <param name="description">Optional description</param>
        /// <returns>True if successful</returns>
        public static bool SetSysConfigValue(string configModule, string configKey, object value, string description = "")
        {
            // If using V1 (Legacy) structure, delegate to original method with combined key
            if (_sysConfigTableVersion == SysTableVersion.V1_2018)
            {
                return SetSysConfigValue($"{configModule}.{configKey}", value, description);
            }

            bool boo_rtn = false;

            try
            {
                // New structure: zVTS_SysConfig
                string sql = $"SELECT * FROM {SysConfigTableName} WHERE ConfigModule = '{configModule}' AND ConfigKey = '{configKey}'";
                DataTable dt_config = myDBSetting.GetDataTable(sql, false);

                if (dt_config != null)
                {
                    string dataType = GetDataTypeString(value);
                    string valueStr = Convert.ToString(value);

                    if (dt_config.Rows.Count <= 0)
                    {
                        // Insert new record
                        DataRow dr_new = dt_config.NewRow();
                        dr_new["ConfigModule"] = configModule;
                        dr_new["ConfigKey"] = configKey;
                        dr_new["ConfigValue"] = valueStr;
                        dr_new["ConfigDescription"] = description;
                        dr_new["DataType"] = dataType;
                        dr_new["IsActive"] = true;
                        dr_new["CreatedTimeStamp"] = DateTime.Now;
                        dr_new["LastModified"] = DateTime.Now;

                        dt_config.Rows.Add(dr_new);
                    }
                    else
                    {
                        // Update existing record
                        dt_config.Rows[0]["ConfigValue"] = valueStr;
                        dt_config.Rows[0]["LastModified"] = DateTime.Now;
                        if (!string.IsNullOrEmpty(description))
                            dt_config.Rows[0]["ConfigDescription"] = description;
                    }

                    if (myDBSetting.SimpleSaveDataTable(dt_config, sql) > 0)
                        boo_rtn = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(SetSysConfigValue)}(Module)", ex);
            }

            return boo_rtn;
        }

        /// <summary>
        /// Helper method to determine DataType string from value type
        /// Added by SCChang's Copilot on 20251208
        /// </summary>
        private static string GetDataTypeString(object value)
        {
            if (value == null) return "String";

            Type t = value.GetType();
            if (t == typeof(bool)) return "Boolean";
            if (t == typeof(DateTime)) return "DateTime";
            if (t == typeof(int) || t == typeof(Int16) || t == typeof(Int32) || t == typeof(Int64) || t == typeof(byte)) return "Integer";
            if (t == typeof(decimal)) return "Decimal";
            if (t == typeof(double) || t == typeof(float)) return "Double";
            return "String";
        }

        /// <summary>
        /// Use to get value in z_SysConfig table
        /// </summary>
        /// <remarks></remarks>
        public static dynamic GetSysConfigValue(string str_ConfigName)
        {
            dynamic dyn_rtn = null;

            try
            {
                //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysConfig"
                //DataTable dt_config = myDBSetting.GetDataTable("select * from z_SysConfig where ConfigName='" + str_ConfigName + "'", false);
                DataTable dt_config = myDBSetting.GetDataTable($"select * from {SysConfigTableName} where ConfigName='" + str_ConfigName + "'", false);
                if (dt_config != null)
                {
                    if (dt_config.Rows.Count > 0)
                    {
                        // until here means oredy got record, so just get value from it
                        switch (dt_config.Rows[0]["ConfigDataType"].ToString().ToUpper())
                        {
                            case "BOOLEAN":
                            case "BOOL":
                                {
                                    dyn_rtn = Convert.ToBoolean(dt_config.Rows[0]["ConfigValue"]);
                                    break;
                                }

                            case "DATE":
                            case "DATE/TIME":
                            case "TIME":
                                {
                                    dyn_rtn = Convert.ToDateTime(dt_config.Rows[0]["ConfigValue"]);
                                    break;
                                }

                            case "INTEGER":
                            case "INT":
                                {
                                    dyn_rtn = Convert.ToInt32(dt_config.Rows[0]["ConfigValue"]);
                                    break;
                                }

                            case "DECIMAL":
                            case "DEC":
                                {
                                    dyn_rtn = Convert.ToDecimal(dt_config.Rows[0]["ConfigValue"]);
                                    break;
                                }

                            case "CURRENCY":
                            case "CURR":
                            case "DOUBLE":
                            case "DBL":
                            case "SINGLE":
                            case "SNG":
                                {
                                    dyn_rtn = Convert.ToDouble(dt_config.Rows[0]["ConfigValue"]);
                                    break;
                                }

                            case "STRING" // Add by LF on 20110530 for STRING selection
                           :
                                {
                                    dyn_rtn = Convert.ToString(dt_config.Rows[0]["ConfigValue"]);
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetSysConfigValue)}()", ex);
            }

            return dyn_rtn;
        }

        /// <summary>
        /// Use to get value in SysConfig table (New structure: zVTS_SysConfig)
        /// Added by SCChang's Copilot on 20251208: support new table structure
        /// </summary>
        /// <param name="configModule">Module name (e.g., "OutstandingPO", "DeliveryManagement")</param>
        /// <param name="configKey">Configuration key</param>
        /// <returns>Value as dynamic type, or null if not found</returns>
        public static dynamic GetSysConfigValue(string configModule, string configKey)
        {
            // If using V1 (Legacy) structure, delegate to original method with combined key
            if (_sysConfigTableVersion == SysTableVersion.V1_2018)
            {
                return GetSysConfigValue($"{configModule}.{configKey}");
            }

            dynamic dyn_rtn = null;

            try
            {
                // New structure: zVTS_SysConfig
                string sql = $"SELECT * FROM {SysConfigTableName} WHERE ConfigModule = '{configModule}' AND ConfigKey = '{configKey}' AND IsActive = 1";
                DataTable dt_config = myDBSetting.GetDataTable(sql, false);

                if (dt_config != null && dt_config.Rows.Count > 0)
                {
                    string dataType = dt_config.Rows[0]["DataType"]?.ToString()?.ToUpper() ?? "STRING";
                    object configValue = dt_config.Rows[0]["ConfigValue"];

                    if (configValue == null || configValue == DBNull.Value)
                        return null;

                    switch (dataType)
                    {
                        case "BOOLEAN":
                        case "BOOL":
                            dyn_rtn = configValue.ToString().ToUpper() == "TRUE" || configValue.ToString() == "1";
                            break;

                        case "DATETIME":
                        case "DATE":
                            dyn_rtn = Convert.ToDateTime(configValue);
                            break;

                        case "INTEGER":
                        case "INT":
                            dyn_rtn = Convert.ToInt32(configValue);
                            break;

                        case "DECIMAL":
                            dyn_rtn = Convert.ToDecimal(configValue);
                            break;

                        case "DOUBLE":
                            dyn_rtn = Convert.ToDouble(configValue);
                            break;

                        default:
                            dyn_rtn = Convert.ToString(configValue);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetSysConfigValue)}(Module)", ex);
            }

            return dyn_rtn;
        }

        #endregion " SysConfig Methods "

        /// <summary>
        /// Use to set Counter's value in z_SysConfig table
        /// </summary>
        /// <remarks></remarks>
        public static int SysConfigGetIntegerCounter(string str_ConfigName)
        {
            try
            {
                //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysConfig"
                //string str_SQL = "UPDATE z_SysConfig SET Value = Value + 1 Output INSERTED.Value WHERE (ConfigName = '" + SQLString(str_ConfigName) + "')";
                string str_SQL = $"UPDATE {SysConfigTableName} SET Value = Value + 1 Output INSERTED.Value WHERE (ConfigName = '" + SQLString(str_ConfigName) + "')";
                DataTable dt = myDBSetting.GetDataTable(str_SQL, true);
                if (dt != null && dt.Rows.Count > 0)
                    return (int)GetDBValue(dt.Rows[0], "Value", -1);
                // Dim int_Counter As Integer = GetSysConfigValue(str_ConfigName, myDBSetting)
                SetSysConfigValue(str_ConfigName, 1);
                return 1;
            }
            catch (Exception ex)
            {
                TextLogger.ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(SysConfigGetIntegerCounter)}()", ex);

                return 1; //just try by default return 1 if hits exception
            }
        }

        /// <summary>
        /// Use to get value in z_SysRef table (by list)
        /// </summary>
        /// <remarks></remarks>
        public static DataTable GetSysRefList(string str_RefGroup, string str_OrderBy = "") //edited by chang on 20211231: added optional param for 'Order By' in SQL stmt
        {
            // If using V2 (New) structure, delegate to new method
            if (_sysRefTableVersion == SysTableVersion.V2_2025)
            {
                return GetSysRefListByCategory(str_RefGroup, str_OrderBy);
            }

            DataTable dt_rtn = null;

            try
            {
                //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysRef"
                //dt_rtn = myDBSetting.GetDataTable("select * from z_SysRef where RefGroup='" + SQLString(str_RefGroup) + "'" + str_OrderBy, false); //edited by chang on 20211231: added optional param for 'Order By' in SQL stmt
                dt_rtn = myDBSetting.GetDataTable($"select * from {SysRefTableName} where RefGroup='" + SQLString(str_RefGroup) + "'" + str_OrderBy, false); //edited by chang on 20211231: added optional param for 'Order By' in SQL stmt
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetSysRefList)}()", ex);
            }

            return dt_rtn;
        }

        /// <summary>
        /// Use to get value in z_SysRef table (by single row)
        /// </summary>
        /// <remarks></remarks>
        public static DataRow GetSysRefRow(string str_RefGroup, string str_RefName)
        {
            // If using V2 (New) structure, delegate to new method
            if (_sysRefTableVersion == SysTableVersion.V2_2025)
            {
                return GetSysRefRowByCategory(str_RefGroup, str_RefName);
            }

            DataRow dr_rtn = null;

            try
            {
                //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysRef"
                //DataTable dt_results = myDBSetting.GetDataTable("select * from z_SysRef where RefGroup='" + SQLString(str_RefGroup) + "' and RefName='" + SQLString(str_RefName) + "'", false);
                DataTable dt_results = myDBSetting.GetDataTable($"select * from {SysRefTableName} where RefGroup='" + SQLString(str_RefGroup) + "' and RefName='" + SQLString(str_RefName) + "'", false);
                if (dt_results != null && dt_results.Rows.Count > 0)
                    dr_rtn = dt_results.Rows[0];
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetSysRefRow)}()", ex);
            }

            return dr_rtn;
        }

        #region " SysRef New Structure Methods " //added by SCChang's Copilot on 20251208

        /// <summary>
        /// Get SysRef list by RefCategory (New structure: zVTS_SysRef)
        /// </summary>
        /// <param name="refCategory">Reference category (e.g., "POStatus", "DeliveryStatus")</param>
        /// <param name="orderBy">Optional ORDER BY clause</param>
        /// <returns>DataTable with matching records</returns>
        public static DataTable GetSysRefListByCategory(string refCategory, string orderBy = "")
        {
            DataTable dt_rtn = null;

            try
            {
                string defaultOrder = string.IsNullOrEmpty(orderBy) ? " ORDER BY SortOrder" : orderBy;
                string sql = $"SELECT * FROM {SysRefTableName} WHERE RefCategory = '{SQLString(refCategory)}' AND IsActive = 1{defaultOrder}";
                dt_rtn = myDBSetting.GetDataTable(sql, false);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetSysRefListByCategory)}()", ex);
            }

            return dt_rtn;
        }

        /// <summary>
        /// Get SysRef single row by RefCategory and RefCode (New structure: zVTS_SysRef)
        /// </summary>
        /// <param name="refCategory">Reference category</param>
        /// <param name="refCode">Reference code</param>
        /// <returns>DataRow if found, null otherwise</returns>
        public static DataRow GetSysRefRowByCategory(string refCategory, string refCode)
        {
            DataRow dr_rtn = null;

            try
            {
                string sql = $"SELECT * FROM {SysRefTableName} WHERE RefCategory = '{SQLString(refCategory)}' AND RefCode = '{SQLString(refCode)}' AND IsActive = 1";
                DataTable dt_results = myDBSetting.GetDataTable(sql, false);
                if (dt_results != null && dt_results.Rows.Count > 0)
                    dr_rtn = dt_results.Rows[0];
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetSysRefRowByCategory)}()", ex);
            }

            return dr_rtn;
        }

        /// <summary>
        /// Get SysRef value by RefCategory and RefCode (New structure: zVTS_SysRef)
        /// </summary>
        /// <param name="refCategory">Reference category</param>
        /// <param name="refCode">Reference code</param>
        /// <returns>RefValue string if found, null otherwise</returns>
        public static string GetSysRefValue(string refCategory, string refCode)
        {
            DataRow row = GetSysRefRowByCategory(refCategory, refCode);
            return row?["RefValue"]?.ToString();
        }

        #region " SysRef Write Operations "

        /// <summary>
        /// Set or update a SysRef row with version support
        /// Added by SCChang's Copilot on 20260108
        /// </summary>
        public static bool SetSysRefRow(string refCategory, string refCode, string refValue, string refDescription = "", int sortOrder = 0)
        {
            // If using V2 (New) structure, delegate to new method
            if (_sysRefTableVersion == SysTableVersion.V2_2025)
            {
                return SetSysRefRowByCategory(refCategory, refCode, refValue, refDescription, sortOrder);
            }

            // Legacy V1 Fallback
            return SetSysRefRow_Legacy(refCategory, refCode, refValue, refDescription, sortOrder);
        }

        /// <summary>
        /// Delete a SysRef row with version support
        /// Added by SCChang's Copilot on 20260108
        /// </summary>
        public static bool DeleteSysRefRow(string refCategory, string refCode, bool hardDelete = false)
        {
            // If using V2 (New) structure, delegate to new method
            if (_sysRefTableVersion == SysTableVersion.V2_2025)
            {
                return DeleteSysRefRowByCategory(refCategory, refCode, hardDelete);
            }

            // Legacy V1 Fallback (Delete usually hard delete in V1)
            return DeleteSysRefRow_Legacy(refCategory, refCode);
        }

        /// <summary>
        /// Get next sort order for a category
        /// Added by SCChang's Copilot on 20260108
        /// </summary>
        public static int GetNextSortOrder(string refCategory)
        {
            // If using V2 (New) structure, delegate to new method
            if (_sysRefTableVersion == SysTableVersion.V2_2025)
            {
                return GetNextSortOrderByCategory(refCategory);
            }

            // Legacy V1 Fallback
            return GetNextSortOrder_Legacy(refCategory);
        }

        /// <summary>
        /// Implementation for V2 (zVTS_SysRef) Set Row
        /// </summary>
        private static bool SetSysRefRowByCategory(string refCategory, string refCode, string refValue, string refDescription, int sortOrder)
        {
            bool boo_rtn = false;
            try
            {
                string sql = $"SELECT * FROM {SysRefTableName} WHERE RefCategory = '{SQLString(refCategory)}' AND RefCode = '{SQLString(refCode)}'";
                DataTable dt = myDBSetting.GetDataTable(sql, false);

                if (dt != null)
                {
                    if (dt.Rows.Count <= 0)
                    {
                        // Insert
                        DataRow dr = dt.NewRow();
                        dr["RefCategory"] = refCategory;
                        dr["RefCode"] = refCode;
                        dr["RefValue"] = refValue;
                        dr["RefDescription"] = refDescription;
                        dr["SortOrder"] = sortOrder;
                        dr["IsActive"] = true;
                        dr["CreatedTimeStamp"] = DateTime.Now;
                        dr["LastModified"] = DateTime.Now;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        // Update
                        DataRow dr = dt.Rows[0];
                        dr["RefValue"] = refValue;
                        dr["RefDescription"] = refDescription;
                        dr["SortOrder"] = sortOrder;
                        dr["LastModified"] = DateTime.Now;
                    }

                    if (myDBSetting.SimpleSaveDataTable(dt, sql) > 0)
                        boo_rtn = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(SetSysRefRowByCategory)}()", ex);
            }
            return boo_rtn;
        }

        /// <summary>
        /// Implementation for V2 (zVTS_SysRef) Delete Row
        /// </summary>
        private static bool DeleteSysRefRowByCategory(string refCategory, string refCode, bool hardDelete)
        {
            bool boo_rtn = false;
            try
            {
                string sql = hardDelete
                    ? $"DELETE FROM {SysRefTableName} WHERE RefCategory = '{SQLString(refCategory)}' AND RefCode = '{SQLString(refCode)}'"
                    : $"UPDATE {SysRefTableName} SET IsActive = 0, LastModified = GETDATE() WHERE RefCategory = '{SQLString(refCategory)}' AND RefCode = '{SQLString(refCode)}'";

                if (myDBSetting.ExecuteNonQuery(sql) > 0)
                    boo_rtn = true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(DeleteSysRefRowByCategory)}()", ex);
            }
            return boo_rtn;
        }

        /// <summary>
        /// Implementation for V2 (zVTS_SysRef) Get Next Sort Order
        /// </summary>
        private static int GetNextSortOrderByCategory(string refCategory)
        {
            int nextSort = 1;
            try
            {
                string sql = $"SELECT MAX(SortOrder) FROM {SysRefTableName} WHERE RefCategory = '{SQLString(refCategory)}'";
                object obj = myDBSetting.ExecuteScalar(sql);
                if (obj != null && obj != DBNull.Value)
                {
                    nextSort = Convert.ToInt32(obj) + 1;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetNextSortOrderByCategory)}()", ex);
            }
            return nextSort;
        }

        /// <summary>
        /// Implementation for V1 (Legacy z_SysRef) Set Row
        /// </summary>
        private static bool SetSysRefRow_Legacy(string refCategory, string refCode, string refValue, string refDescription, int sortOrder)
        {
            bool boo_rtn = false;
            try
            {
                // V1 Mapping: RefGroup -> refCategory, RefName -> refCode, RefSeq -> sortOrder
                string sql = $"SELECT * FROM {SysRefTableName} WHERE RefGroup = '{SQLString(refCategory)}' AND RefName = '{SQLString(refCode)}'";
                DataTable dt = myDBSetting.GetDataTable(sql, false);

                if (dt != null)
                {
                    if (dt.Rows.Count <= 0)
                    {
                        // Insert
                        DataRow dr = dt.NewRow();
                        dr["RefID"] = GetSysRefID(); // Need manual ID for Legacy
                        dr["RefGroup"] = refCategory;
                        dr["RefName"] = refCode;
                        dr["RefValue"] = refValue;
                        dr["RefDesc"] = refDescription;
                        dr["RefSeq"] = sortOrder;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        // Update
                        DataRow dr = dt.Rows[0];
                        dr["RefValue"] = refValue;
                        dr["RefDesc"] = refDescription;
                        dr["RefSeq"] = sortOrder;
                    }

                    if (myDBSetting.SimpleSaveDataTable(dt, sql) > 0)
                        boo_rtn = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(SetSysRefRow_Legacy)}()", ex);
            }
            return boo_rtn;
        }

        /// <summary>
        /// Implementation for V1 (Legacy z_SysRef) Delete Row
        /// </summary>
        private static bool DeleteSysRefRow_Legacy(string refCategory, string refCode)
        {
            bool boo_rtn = false;
            try
            {
                string sql = $"DELETE FROM {SysRefTableName} WHERE RefGroup = '{SQLString(refCategory)}' AND RefName = '{SQLString(refCode)}'";
                if (myDBSetting.ExecuteNonQuery(sql) > 0)
                    boo_rtn = true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(DeleteSysRefRow_Legacy)}()", ex);
            }
            return boo_rtn;
        }

        /// <summary>
        /// Implementation for V1 (Legacy z_SysRef) Get Next Sort Order
        /// </summary>
        private static int GetNextSortOrder_Legacy(string refCategory)
        {
            int nextSort = 1;
            try
            {
                string sql = $"SELECT MAX(RefSeq) FROM {SysRefTableName} WHERE RefGroup = '{SQLString(refCategory)}'";
                object obj = myDBSetting.ExecuteScalar(sql);
                if (obj != null && obj != DBNull.Value)
                {
                    nextSort = Convert.ToInt32(obj) + 1;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(SystemHelper)}.{nameof(GetNextSortOrder_Legacy)}()", ex);
            }
            return nextSort;
        }

        #endregion " SysRef Write Operations "

        #endregion " SysRef New Structure Methods "

        /// <summary>
        /// Use to get Unique Primary Key in z_SysRef table
        /// </summary>
        /// <remarks></remarks>
        public static long GetSysRefID()
        {
            //modified by SCChang on 20251106: use configurable table name instead of hard-coded "z_SysRef"
            //DataTable dt = myDBSetting.GetDataTable("SELECT RefID FROM z_SysRef", false);
            DataTable dt = myDBSetting.GetDataTable($"SELECT RefID FROM {SysRefTableName}", false);
            long lng_count = dt.Rows.Count;
            if (lng_count == 0)
                return 1;

            object obj_LastKey = dt.Compute("MAX(RefID)", "");
            long lng_lastKey = (obj_LastKey != null && obj_LastKey != DBNull.Value ? System.Convert.ToInt64(obj_LastKey) : 0);
            if (lng_lastKey == 0)
                return 1;

            long lng_key = lng_lastKey + 1;
            if (lng_lastKey != lng_count)
            {
                // ''''For i As Integer = 1 To lng_lastKey 'removed by chang on 20140613
                for (long i = 1; i <= lng_lastKey; i++) // added by chang on 20140613: changed from Integer to Long
                {
                    if (dt.Select("RefID='" + i.ToString() + "'").Length == 0)
                    {
                        lng_key = i;

                        break;
                    }
                }
            }

            return lng_key;
        }
    }
}
