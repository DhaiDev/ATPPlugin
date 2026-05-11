using System;
using System.Data;
using System.IO;

using Microsoft.VisualBasic;

using AutoCount.Data;

using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.Helpers
{
    public class AutoCountHelper
    {
        /// <summary>
        /// Use for define the AutoCount default System UDF Types
        /// </summary>
        /// <remarks></remarks>
        public enum UDFSystemType
        {
            Account,
            Area,
            CNType,
            Creditor,
            CreditorType,
            CreditTerm,
            Currency,
            Debtor,
            DebtorType,
            Department,
            DNType,
            Item,
            ItemBrand,
            ItemCategory,
            ItemClass,
            ItemGroup,
            ItemType,
            Location,
            PaymentMethod,
            PriceCategory,
            Project,
            PurchaseAgent,
            SalesAgent,
            ShippingMethod,
            None
        }

        /// <summary>
        /// Use for define the doc type
        /// </summary>
        /// <remarks></remarks>
        public enum enm_DocType
        {
            ST, // for stock take rec
            SR, // for stock return rec
            SI, // for stock issue rec
            SA, // for slitting advise rec
            WO, // for work order rec
            MS, // for machine schedule rec
            GR  // for goods receive rec
        }

        /// <summary>
        /// Use to read the database schema Embedded streams from calling Assembly resources
        /// </summary>
        /// <remarks></remarks>
        private static string ReadEmbeddedDatabaseSchema(string fileName, System.Reflection.Assembly calledAssembly, string nameSpace)
        {
            string DDL = "";
            //System.Reflection.Assembly thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            //string resourceName = typeof(PlugIn_Cls).Namespace + ".SQL." + fileName;
            string resourceName = $"{nameSpace}.SQL.{fileName}";
            //using (Stream stream = thisAssembly.GetManifestResourceStream(resourceName))
            using (Stream stream = calledAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new AutoCount.AppException(string.Format("Unable to get manifest resource {0}.", resourceName));
                else
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        DDL = sr.ReadToEnd();
                    }
            }
            return DDL;
        }

        /// <summary>
        /// Use to run or execute the database schema from calling Assembly Embedded Resources
        /// </summary>
        /// <remarks></remarks>
        public static bool RunEmbeddedDatabaseSchema(DBSetting dbSetting, string fileName, System.Reflection.Assembly calledAssembly, string nameSpace)
        {
            try
            {
                string DDL = ReadEmbeddedDatabaseSchema(fileName, calledAssembly, nameSpace);

                DBUtils dbUtils_1 = DBUtils.Create(dbSetting);
                dbUtils_1.ExecuteDDLText(DDL);
                return true;
            }
            catch (AutoCount.AppException ex2)
            {
                AutoCount.AppMessage.ShowErrorMessage(null, ex2.Message);
                return false;
            }
        }

        /// <summary>
        /// Use to check the creating UDFList existence
        /// </summary>
        /// <remarks></remarks>
        public static bool IsCreatedUDFList(string str_ListName)
        {
            bool boo_rtn = false;

            AutoCount.UDF.UDFList udfList_Check = new AutoCount.UDF.UDFList(myDBSetting);
            string[] str_ACListNames = udfList_Check.GetNames();
            if (str_ACListNames.Length > 0)
            {
                for (int int_i = 0; int_i < str_ACListNames.Length; int_i++)
                {
                    if (str_ACListNames[int_i] == str_ListName)
                    {
                        boo_rtn = true;
                        break;
                    }
                }
            }

            return boo_rtn;
        }
        /// <summary>
        /// Use to create the UDFList
        /// </summary>
        /// <remarks></remarks>
        public static void CreateRequiredUDFList(string str_ListName, string[] str_UDFListItems)
        {
            try
            {
                //check is ListName valid
                if (!AutoCount.UDF.UDFList.IsValidName(str_ListName))
                {
                    Exception ex = new Exception(string.Format("Error while creating new UDF List '{0}'." + Constants.vbCrLf + Constants.vbCrLf + "{1}", str_ListName, $"The UDF List NAME '{str_ListName}' was INVALID."));
                    ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(CreateRequiredUDFList)}()", ex);
                    throw ex;
                }

                AutoCount.UDF.UDFList udfList = new AutoCount.UDF.UDFList(myDBSetting);
                if (IsCreatedUDFList(str_ListName))
                {
                    //if found then delete 1st
                    udfList.Delete(str_ListName);
                }

                udfList.Add(str_ListName, str_UDFListItems);
                udfList.Save();
            }
            catch (AutoCount.AppException ex)
            {
                Exception ex1 = new Exception(string.Format("Error while creating new UDF List '{0}'." + Constants.vbCrLf + Constants.vbCrLf + "{1}", str_ListName, ex.Message));
                ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(CreateRequiredUDFList)}()", ex1);
                throw ex1;
            }
        }

        /// <summary>
        /// Use to check the creating UDF existence
        /// </summary>
        /// <remarks></remarks>
        public static bool IsCreatedUDF(string str_TableName, string str_RequiredUDF)
        {
            bool boo_rtn = true;

            if (myDBSetting != null)
            {
                AutoCount.UDF.UDFTable ut = new AutoCount.UDF.UDFTable(str_TableName, myDBSetting);
                DataRow r = ut.Table.Rows.Find(new object[] { str_TableName, str_RequiredUDF });
                boo_rtn = (r == null ? false : true);
            }

            return boo_rtn;
        }

        #region " UDF DefaultValue Helper Methods " //added by SCChang's Copilot on 20251110

        /// <summary>
        /// Convert defaultValue to appropriate type based on UDF type
        /// </summary>
        /// <param name="defaultValue">Default value to convert (can be any type)</param>
        /// <param name="udfType">Target UDF type</param>
        /// <param name="tableName">Table name (for logging)</param>
        /// <param name="fieldName">Field name (for logging)</param>
        /// <param name="conversionSucceeded">Output: whether conversion succeeded</param>
        /// <returns>Converted value or null if conversion failed</returns>
        /// <remarks>
        /// Added by SCChang's Copilot on 20251110: v2.2.1.0
        /// Handles automatic type conversion with comprehensive logging using GeneralLogger_Cls
        /// </remarks>
        private static object ConvertDefaultValueByType(
            object defaultValue,
            AutoCount.UDF.UDFType udfType,
            string tableName,
            string fieldName,
            out bool conversionSucceeded)
        {
            conversionSucceeded = false;

            // If defaultValue is null, no conversion needed
            if (defaultValue == null)
            {
                return null;
            }

            try
            {
                object convertedValue = null;

                switch (udfType)
                {
                    case AutoCount.UDF.UDFType.Text:
                    case AutoCount.UDF.UDFType.Memo:
                    case AutoCount.UDF.UDFType.RichText:
                    case AutoCount.UDF.UDFType.ImageLink:
                        // Convert to string
                        convertedValue = defaultValue.ToString();
                        conversionSucceeded = true;
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion SUCCESS: {tableName}.{fieldName} (Text) = '{convertedValue}'", LogLevel.SUCCESS);
                        break;

                    case AutoCount.UDF.UDFType.Decimal:
                        // Convert to decimal
                        convertedValue = Convert.ToDecimal(defaultValue);
                        conversionSucceeded = true;
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion SUCCESS: {tableName}.{fieldName} (Decimal) = {convertedValue}", LogLevel.SUCCESS);
                        break;

                    case AutoCount.UDF.UDFType.Integer:
                        // Convert to int
                        convertedValue = Convert.ToInt32(defaultValue);
                        conversionSucceeded = true;
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion SUCCESS: {tableName}.{fieldName} (Integer) = {convertedValue}", LogLevel.SUCCESS);
                        break;

                    case AutoCount.UDF.UDFType.Date:
                        // Convert to DateTime
                        if (defaultValue is DateTime)
                        {
                            convertedValue = (DateTime)defaultValue;
                        }
                        else if (DateTime.TryParse(defaultValue.ToString(), out DateTime dateResult))
                        {
                            convertedValue = dateResult;
                        }
                        else
                        {
                            throw new FormatException($"Cannot convert '{defaultValue}' to DateTime");
                        }
                        conversionSucceeded = true;
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion SUCCESS: {tableName}.{fieldName} (Date) = {((DateTime)convertedValue).ToString("yyyy-MM-dd")}", LogLevel.SUCCESS);
                        break;

                    case AutoCount.UDF.UDFType.Boolean:
                        // Handle multiple boolean formats
                        bool boolResult;
                        string strValue = defaultValue.ToString().ToUpperInvariant().Trim();
                        
                        if (strValue == "T" || strValue == "TRUE" || strValue == "1")
                        {
                            boolResult = true;
                        }
                        else if (strValue == "F" || strValue == "FALSE" || strValue == "0")
                        {
                            boolResult = false;
                        }
                        else if (bool.TryParse(defaultValue.ToString(), out boolResult))
                        {
                            // Standard boolean parsing
                        }
                        else
                        {
                            throw new FormatException($"Cannot convert '{defaultValue}' to Boolean");
                        }
                        
                        convertedValue = boolResult;
                        conversionSucceeded = true;
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion SUCCESS: {tableName}.{fieldName} (Boolean) = {convertedValue}", LogLevel.SUCCESS);
                        break;

                    case AutoCount.UDF.UDFType.System:
                        // System type needs validation - will be handled separately
                        convertedValue = defaultValue.ToString();
                        conversionSucceeded = true;
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion SUCCESS: {tableName}.{fieldName} (System) = '{convertedValue}'", LogLevel.SUCCESS);
                        break;

                    default:
                        GeneralLogger_Cls.Write($"UDF DefaultValue conversion FAILED: {tableName}.{fieldName} - Unsupported UDF type '{udfType}'", LogLevel.ERROR);
                        break;
                }

                return convertedValue;
            }
            catch (Exception ex)
            {
                conversionSucceeded = false;
                GeneralLogger_Cls.Write($"UDF DefaultValue conversion FAILED: {tableName}.{fieldName} ({udfType}) - Cannot convert '{defaultValue}' ({defaultValue.GetType().Name}): {ex.Message}", LogLevel.ERROR);
                ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(ConvertDefaultValueByType)}({tableName}.{fieldName})", ex);
                return null;
            }
        }

        /// <summary>
        /// Validate System UDF entity code exists in database
        /// </summary>
        /// <param name="entityCode">Entity code to validate</param>
        /// <param name="systemType">System UDF type</param>
        /// <param name="tableName">Table name (for logging)</param>
        /// <param name="fieldName">Field name (for logging)</param>
        /// <returns>True if entity exists, false otherwise</returns>
        /// <remarks>
        /// Added by SCChang's Copilot on 20251110: v2.2.1.0
        /// Validates System UDF entity exists in database before setting DefaultValue
        /// Logs to both GeneralLogger_Cls and ErrorLogger_Cls for validation failures
        /// </remarks>
        private static bool ValidateSystemUDFEntity(
            string entityCode,
            UDFSystemType systemType,
            string tableName,
            string fieldName)
        {
            if (string.IsNullOrWhiteSpace(entityCode))
            {
                GeneralLogger_Cls.Write($"UDF System validation SKIPPED: {tableName}.{fieldName} - Entity code is empty", LogLevel.WARNING);
                return true; // Empty is allowed
            }

            if (myDBSetting == null)
            {
                GeneralLogger_Cls.Write($"UDF System validation FAILED: {tableName}.{fieldName} - DBSetting is null", LogLevel.ERROR);
                return false;
            }

            try
            {
                string queryTable = "";
                string queryField = "";

                // Map UDFSystemType to database table and field
                switch (systemType)
                {
                    case UDFSystemType.Account:
                        queryTable = "Account";
                        queryField = "AccNo";
                        break;
                    case UDFSystemType.Area:
                        queryTable = "Area";
                        queryField = "AreaCode";
                        break;
                    case UDFSystemType.Creditor:
                        queryTable = "Creditor";
                        queryField = "CreditorCode";
                        break;
                    case UDFSystemType.CreditorType:
                        queryTable = "CreditorType";
                        queryField = "CreditorTypeCode";
                        break;
                    case UDFSystemType.CreditTerm:
                        queryTable = "CreditTerm";
                        queryField = "CreditTermCode";
                        break;
                    case UDFSystemType.Currency:
                        queryTable = "Currency";
                        queryField = "CurrencyCode";
                        break;
                    case UDFSystemType.Debtor:
                        queryTable = "Debtor";
                        queryField = "DebtorCode";
                        break;
                    case UDFSystemType.DebtorType:
                        queryTable = "DebtorType";
                        queryField = "DebtorTypeCode";
                        break;
                    case UDFSystemType.Department:
                        queryTable = "Department";
                        queryField = "DepartmentCode";
                        break;
                    case UDFSystemType.Item:
                        queryTable = "Item";
                        queryField = "ItemCode";
                        break;
                    case UDFSystemType.ItemBrand:
                        queryTable = "ItemBrand";
                        queryField = "BrandCode";
                        break;
                    case UDFSystemType.ItemCategory:
                        queryTable = "ItemCategory";
                        queryField = "CategoryCode";
                        break;
                    case UDFSystemType.ItemClass:
                        queryTable = "ItemClass";
                        queryField = "ClassCode";
                        break;
                    case UDFSystemType.ItemGroup:
                        queryTable = "ItemGroup";
                        queryField = "GroupCode";
                        break;
                    case UDFSystemType.ItemType:
                        queryTable = "ItemType";
                        queryField = "TypeCode";
                        break;
                    case UDFSystemType.Location:
                        queryTable = "Location";
                        queryField = "LocationCode";
                        break;
                    case UDFSystemType.PaymentMethod:
                        queryTable = "PaymentMethod";
                        queryField = "PaymentMethodCode";
                        break;
                    case UDFSystemType.PriceCategory:
                        queryTable = "PriceCategory";
                        queryField = "PriceCategoryCode";
                        break;
                    case UDFSystemType.Project:
                        queryTable = "Project";
                        queryField = "ProjectCode";
                        break;
                    case UDFSystemType.PurchaseAgent:
                        queryTable = "PurchaseAgent";
                        queryField = "PurchaseAgentCode";
                        break;
                    case UDFSystemType.SalesAgent:
                        queryTable = "SalesAgent";
                        queryField = "SalesAgentCode";
                        break;
                    case UDFSystemType.ShippingMethod:
                        queryTable = "ShippingMethod";
                        queryField = "ShippingMethodCode";
                        break;
                    default:
                        GeneralLogger_Cls.Write($"UDF System validation SKIPPED: {tableName}.{fieldName} - Unknown SystemType '{systemType}'", LogLevel.WARNING);
                        return true; // Skip validation for unknown types
                }

                // Execute validation query
                string query = $"SELECT COUNT(*) FROM {queryTable} WHERE {queryField} = N'{entityCode.Replace("'", "''")}'";
                object result = myDBSetting.ExecuteScalar(query);
                int count = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                if (count > 0)
                {
                    GeneralLogger_Cls.Write($"UDF System validation SUCCESS: {tableName}.{fieldName} ({systemType}) - Entity '{entityCode}' exists", LogLevel.SUCCESS);
                    return true;
                }
                else
                {
                    string errorMsg = $"UDF System validation FAILED: {tableName}.{fieldName} ({systemType}) - Entity '{entityCode}' does NOT exist in {queryTable}";
                    GeneralLogger_Cls.Write(errorMsg, LogLevel.ERROR);
                    ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(ValidateSystemUDFEntity)}({tableName}.{fieldName})", new Exception(errorMsg));
                    return false;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = $"UDF System validation ERROR: {tableName}.{fieldName} ({systemType}) - {ex.Message}";
                GeneralLogger_Cls.Write(errorMsg, LogLevel.ERROR);
                ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(ValidateSystemUDFEntity)}({tableName}.{fieldName})", ex);
                return false;
            }
        }

        #endregion " UDF DefaultValue Helper Methods " //added by SCChang's Copilot on 20251110

        /// <summary>
        /// Use to create the UDFs
        /// </summary>
        /// <remarks>
        /// Enhanced by Copilot on 20251010: v2.2.1.1
        /// - Added full support for all UDF types based on AutoCount.UDF API
        /// - Added Required/Unique support for all applicable types
        /// - Added Precision support for Decimal type
        /// - Improved SystemType mapping to match AutoCount.UDF.SystemType enum
        /// Enhanced by SCChang's Copilot on 20251110: v2.2.1.0
        /// - Added DefaultValue parameter support for all UDF types
        /// - Automatic type conversion with comprehensive logging via GeneralLogger_Cls
        /// - System UDF validation with database queries
        /// </remarks>
        //////////public static void CreateRequiredUDF(string str_TableName, string str_RequiredUDF, string str_UDFDisplayText, AutoCount.UDF.UDFType udftyp_utype, UDFSystemType udfsystyp_UDFSystemType, int int_Size, string str_ListName = "", bool boo_ShowLabel = true) //removed by chang on 20230209: add 2 new params
        //////////public static void CreateRequiredUDF(string str_TableName, string str_RequiredUDF, string str_UDFDisplayText, AutoCount.UDF.UDFType udftyp_utype, UDFSystemType udfsystyp_UDFSystemType, int int_Size, string str_ListName = "", bool boo_ShowLabel = true, bool boo_Unique = false, bool boo_Required = false) //removed by SCChang's Copilot on 20251110: add defaultValue param
        public static void CreateRequiredUDF(string str_TableName, string str_RequiredUDF, string str_UDFDisplayText, AutoCount.UDF.UDFType udftyp_utype, UDFSystemType udfsystyp_UDFSystemType, int int_Size, string str_ListName = "", bool boo_ShowLabel = true, bool boo_Unique = false, bool boo_Required = false, object defaultValue = null) //added by SCChang's Copilot on 20251110: add defaultValue param
        {
            AutoCount.UDF.UDFTable ut = new AutoCount.UDF.UDFTable(str_TableName, myDBSetting);
            DataRow r = ut.Table.Rows.Find(new object[] { str_TableName, str_RequiredUDF });
            if (r == null)
            {
                AutoCount.UDF.Field f = new AutoCount.UDF.Field();
                f.Name = str_RequiredUDF;
                f.Caption = str_UDFDisplayText;
                f.Type = udftyp_utype;

                switch (udftyp_utype)
                {
                    case AutoCount.UDF.UDFType.Text:
                        {
                            f.TextProperties.ListName = str_ListName; //added by chang on 20220704: to set the customized UDFList
                            f.TextProperties.Size = int_Size > 0 ? int_Size : 50; // Default to 50 if not specified
                            f.TextProperties.Unique = boo_Unique;
                            f.TextProperties.Required = boo_Required;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.TextProperties.DefaultValue = convertedValue.ToString();
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Decimal:
                        {
                            // int_Size is used as Scale, Precision defaults to 18 (max digits)
                            int precision = 18; // Default precision
                            int scale = int_Size >= 0 ? int_Size : 2; // Default scale to 2 decimal places
                            
                            // Ensure scale doesn't exceed precision
                            if (scale > precision)
                                scale = precision;
                            
                            f.DecimalProperties.Precision = precision;
                            f.DecimalProperties.Scale = scale;
                            f.DecimalProperties.Required = boo_Required;
                            f.DecimalProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.DecimalProperties.DefaultValue = (decimal)convertedValue;
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Integer:
                        {
                            f.IntegerProperties.Required = boo_Required;
                            f.IntegerProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.IntegerProperties.DefaultValue = (int)convertedValue;
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Date:
                        {
                            f.DateProperties.DateType = AutoCount.UDF.DateType.Date;
                            f.DateProperties.Required = boo_Required;
                            f.DateProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.DateProperties.DefaultValue = (DateTime)convertedValue;
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Boolean:
                        {
                            f.BooleanProperties.Required = boo_Required;
                            f.BooleanProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.BooleanProperties.DefaultValue = (bool)convertedValue;
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Memo:
                        {
                            f.MemoProperties.Required = boo_Required;
                            f.MemoProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.MemoProperties.DefaultValue = convertedValue.ToString();
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.RichText:
                        {
                            f.RichTextProperties.Required = boo_Required;
                            f.RichTextProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.RichTextProperties.DefaultValue = convertedValue.ToString();
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.ImageLink:
                        {
                            f.ImageLinkProperties.Required = boo_Required;
                            f.ImageLinkProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    f.ImageLinkProperties.DefaultValue = convertedValue.ToString();
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.System:
                        {
                            // Map our UDFSystemType enum to AutoCount's SystemType enum string
                            // AutoCount uses specific names like "AreaCode", "ItemCode" etc.
                            string customDataType = GetAutoCountSystemTypeName(udfsystyp_UDFSystemType);
                            f.SystemProperties.CustomDataType = customDataType;
                            f.SystemProperties.ShowLabel = boo_ShowLabel;
                            f.SystemProperties.Required = boo_Required;
                            f.SystemProperties.Unique = boo_Unique;
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support with validation
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    string entityCode = convertedValue.ToString();
                                    // Validate entity exists in database
                                    if (ValidateSystemUDFEntity(entityCode, udfsystyp_UDFSystemType, str_TableName, str_RequiredUDF))
                                    {
                                        f.SystemProperties.DefaultValue = entityCode;
                                    }
                                    else
                                    {
                                        GeneralLogger_Cls.Write($"UDF DefaultValue NOT SET: {str_TableName}.{str_RequiredUDF} - System entity '{entityCode}' validation failed", LogLevel.WARNING);
                                    }
                                }
                            }
                            
                            break;
                        }
                    default:
                        {
                            throw new Exception($"The provided UDF Type '{udftyp_utype}' is not supported in this coding");
                        }
                }

                try
                {
                    ut.Add(f);
                    ut.Save();
                }
                catch (AutoCount.AppException ex)
                {
                    Exception ex1 = new Exception(string.Format("Error while creating new UDF '{0}'." + Constants.vbCrLf + Constants.vbCrLf + "{1}", f.Name, ex.Message));
                    ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(CreateRequiredUDF)}()", ex1);
                    throw ex1;
                }
            }
            else
            {
                #region " if found then try to update " //added by chang on 20230209, enhanced by Copilot on 20251010, enhanced by SCChang's Copilot on 20251110
                r["FieldName"] = str_RequiredUDF;
                r["Caption"] = str_UDFDisplayText;
                r["FieldType"] = Convert.ToInt32(udftyp_utype);

                string str_Properties = "";
                switch (udftyp_utype)
                {
                    case AutoCount.UDF.UDFType.Text:
                        {
                            int size = int_Size > 0 ? int_Size : 50;
                            str_Properties += $"Size={size}\n";
                            if (!string.IsNullOrWhiteSpace(str_ListName))
                                str_Properties += $"ListName={str_ListName}\n";
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: escape value for SQL compatibility
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Escape single quotes and remove semicolons for SQL safety
                                    string escapedValue = convertedValue.ToString().Replace("'", "''").Replace(";", " ");
                                    str_Properties += $"DefaultValue={escapedValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Decimal:
                        {
                            int precision = 18;
                            int scale = int_Size >= 0 ? int_Size : 2;
                            if (scale > precision)
                                scale = precision;
                            
                            str_Properties += $"Precision={precision}\n";
                            str_Properties += $"Scale={scale}\n";
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: use InvariantCulture for consistent number format
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Use InvariantCulture to ensure consistent decimal format (e.g., 10.5 not 10,5)
                                    string decimalValue = ((decimal)convertedValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    str_Properties += $"DefaultValue={decimalValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Integer:
                        {
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: use InvariantCulture for consistent number format
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Use InvariantCulture to ensure consistent integer format
                                    string intValue = ((int)convertedValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    str_Properties += $"DefaultValue={intValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Date:
                        {
                            str_Properties += $"DateType=Date\n";
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: use InvariantCulture for consistent date format
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Use InvariantCulture to ensure consistent date format
                                    string dateValue = ((DateTime)convertedValue).Date.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    str_Properties += $"DefaultValue={dateValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Boolean:
                        {
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: convert bool to T/F format for AutoCount UDF
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // AutoCount Boolean UDF uses "T"/"F" string format internally
                                    string boolValue = ((bool)convertedValue) ? "T" : "F";
                                    str_Properties += $"DefaultValue={boolValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.Memo:
                        {
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: escape value for SQL compatibility
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Escape single quotes and remove semicolons for SQL safety
                                    string escapedValue = convertedValue.ToString().Replace("'", "''").Replace(";", " ");
                                    str_Properties += $"DefaultValue={escapedValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.RichText:
                        {
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: escape value for SQL compatibility
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Escape single quotes and remove semicolons for SQL safety
                                    string escapedValue = convertedValue.ToString().Replace("'", "''").Replace(";", " ");
                                    str_Properties += $"DefaultValue={escapedValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.ImageLink:
                        {
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support in update mode
                            //edited by SCChang's Copilot on 20251112: escape value for SQL compatibility
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    // Escape single quotes and remove semicolons for SQL safety
                                    string escapedValue = convertedValue.ToString().Replace("'", "''").Replace(";", " ");
                                    str_Properties += $"DefaultValue={escapedValue}\n";
                                }
                            }
                            
                            break;
                        }
                    case AutoCount.UDF.UDFType.System:
                        {
                            string customDataType = GetAutoCountSystemTypeName(udfsystyp_UDFSystemType);
                            str_Properties += $"CustomDataType={customDataType}\n";
                            str_Properties += $"ShowLabel={boo_ShowLabel}\n";
                            str_Properties += $"Unique={boo_Unique}\n";
                            str_Properties += $"Required={boo_Required}\n";
                            
                            //added by SCChang's Copilot on 20251110: DefaultValue support with validation in update mode
                            //edited by SCChang's Copilot on 20251112: escape value for SQL compatibility
                            if (defaultValue != null)
                            {
                                bool conversionSuccess;
                                object convertedValue = ConvertDefaultValueByType(defaultValue, udftyp_utype, str_TableName, str_RequiredUDF, out conversionSuccess);
                                if (conversionSuccess && convertedValue != null)
                                {
                                    string entityCode = convertedValue.ToString();
                                    // Validate entity exists in database
                                    if (ValidateSystemUDFEntity(entityCode, udfsystyp_UDFSystemType, str_TableName, str_RequiredUDF))
                                    {
                                        // Escape single quotes and remove semicolons for SQL safety
                                        string escapedValue = entityCode.Replace("'", "''").Replace(";", " ");
                                        str_Properties += $"DefaultValue={escapedValue}\n";
                                    }
                                    else
                                    {
                                        GeneralLogger_Cls.Write($"UDF DefaultValue NOT SET (Update mode): {str_TableName}.{str_RequiredUDF} - System entity '{entityCode}' validation failed", LogLevel.WARNING);
                                    }
                                }
                            }
                            
                            break;
                        }
                    default:
                        {
                            throw new Exception($"The provided UDF Type '{udftyp_utype}' is not supported in this coding");
                        }
                }
                r["Properties"] = str_Properties;

                try
                {
                    ut.Save();
                    //int int_save = myDBSetting.SimpleSaveDataTable(ut.Table, $"select * from UDF where TableName=N'{str_TableName}'");
                }
                catch (AutoCount.AppException ex)
                {
                    Exception ex1 = new Exception(string.Format("Error while updating UDF '{0}'." + Constants.vbCrLf + Constants.vbCrLf + "{1}", r["FieldName"].ToString(), ex.Message));
                    ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(CreateRequiredUDF)}()", ex1);
                    throw ex1;
                }
                #endregion " if found then try to update " //added by chang on 20230209, enhanced by Copilot on 20251010
            }
        }

        /// <summary>
        /// Use to create UserFormula for document fields
        /// </summary>
        /// <param name="str_FormulaName">Formula name (e.g., "SalesOrder", "DeliveryOrder", "Invoice")</param>
        /// <param name="str_ColumnName">Column name to apply formula to</param>
        /// <param name="formulaType">Formula type: "Init" or "ColumnChanged"</param>
        /// <param name="str_Formula">Formula expression/script</param>
        /// <remarks>
        /// Created by Copilot on 20251014: v2.2.1.1
        /// - Uses AutoCount.Evaluation.FormulaEditorCommand to save formulas
        /// - Formulas are stored in UserFormula database table
        /// - Supports Init (initialization) and ColumnChanged (field change) events
        /// - Empty formulas will delete existing formula records
        /// </remarks>
        public static void CreateRequiredUserFormula(string str_FormulaName, string str_ColumnName, string formulaType, string str_Formula)
        {
            try
            {
                if (AutoCount.Authentication.UserSession.CurrentUserSession == null)
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] CreateRequiredUserFormula: UserSession is null");
                    return;
                }

                if (myDBSetting == null)
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] CreateRequiredUserFormula: DBSetting is null");
                    return;
                }

                // Validate input parameters
                if (string.IsNullOrWhiteSpace(str_FormulaName))
                {
                    System.Diagnostics.Debug.WriteLine("[WARNING] CreateRequiredUserFormula: FormulaName is empty");
                    return;
                }

                if (string.IsNullOrWhiteSpace(str_ColumnName))
                {
                    System.Diagnostics.Debug.WriteLine("[WARNING] CreateRequiredUserFormula: ColumnName is empty");
                    return;
                }

                // Parse FormulaType enum
                AutoCount.Evaluation.FormulaType enumFormulaType;
                if (formulaType.Equals("Init", StringComparison.OrdinalIgnoreCase))
                {
                    enumFormulaType = AutoCount.Evaluation.FormulaType.Init;
                }
                else if (formulaType.Equals("ColumnChanged", StringComparison.OrdinalIgnoreCase))
                {
                    enumFormulaType = AutoCount.Evaluation.FormulaType.ColumnChanged;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[WARNING] CreateRequiredUserFormula: Invalid FormulaType '{formulaType}', defaulting to Init");
                    enumFormulaType = AutoCount.Evaluation.FormulaType.Init;
                }

                // Create FormulaEditorCommand instance
                AutoCount.Evaluation.FormulaEditorCommand formulaEditor = 
                    AutoCount.Evaluation.FormulaEditorCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);

                if (formulaEditor == null)
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] CreateRequiredUserFormula: Failed to create FormulaEditorCommand");
                    return;
                }

                // Normalize formula (null to empty string)
                string formulaToSave = str_Formula ?? string.Empty;

                // Save formula to database
                // Note: Empty formula will delete the record from UserFormula table
                formulaEditor.SaveFormula(str_FormulaName, str_ColumnName, enumFormulaType, formulaToSave);

                System.Diagnostics.Debug.WriteLine($"[SUCCESS] UserFormula saved: {str_FormulaName}.{str_ColumnName} ({formulaType}) - Length: {formulaToSave.Length}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] CreateRequiredUserFormula failed for {str_FormulaName}.{str_ColumnName}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                
                // Log to ErrorLogger if available
                try
                {
                    ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(CreateRequiredUserFormula)}({str_FormulaName}, {str_ColumnName}, {formulaType})", ex);
                }
                catch
                {
                    // Ignore if ErrorLogger not available
                }
            }
        }

        /// <summary>
        /// Use to get primary key value of main table(s)
        /// </summary>
        /// <remarks></remarks>
        public static long GetDocKey(string str_dbTableNameMaster)
        {
            // Dim dt As DataTable = myDBSetting.GetDataTable("SELECT DocKey FROM " + str_dbTableNameMaster, False)
            DataTable dt = myDBSetting.GetDataTable("SELECT DocKey FROM " + str_dbTableNameMaster, false);
            long lng_count = dt.Rows.Count;
            if (lng_count == 0)
                return 1;

            object obj_LastKey = dt.Compute("MAX(DocKey)", "");
            long lng_lastKey = (obj_LastKey != null && obj_LastKey != DBNull.Value ? System.Convert.ToInt64(obj_LastKey) : 0);
            if (lng_lastKey == 0)
                return 1;

            long lng_key = lng_lastKey + 1;
            if (lng_lastKey != lng_count)
            {
                // ''''For i As Integer = 1 To lng_lastKey 'removed by chang on 20140613
                for (long i = 1; i <= lng_lastKey; i++) // added by chang on 20140613: changed from Integer to Long
                {
                    if (dt.Select("DocKey='" + i.ToString() + "'").Length == 0)
                    {
                        lng_key = i;

                        break;
                    }
                }
            }

            return lng_key;
        }
        public static long GetDocKey(string str_dbTableNameMaster, string DocKeyFieldName = "DocKey")
        {
            // Dim dt As DataTable = myDBSetting.GetDataTable("SELECT DocKey FROM " + str_dbTableNameMaster, False)
            DataTable dt = myDBSetting.GetDataTable($"SELECT {DocKeyFieldName} FROM " + str_dbTableNameMaster, false);
            long lng_count = dt.Rows.Count;
            if (lng_count == 0)
                return 1;

            object obj_LastKey = dt.Compute($"MAX({DocKeyFieldName})", "");
            long lng_lastKey = (obj_LastKey != null && obj_LastKey != DBNull.Value ? System.Convert.ToInt64(obj_LastKey) : 0);
            if (lng_lastKey == 0)
                return 1;

            long lng_key = lng_lastKey + 1;
            if (lng_lastKey != lng_count)
            {
                // ''''For i As Integer = 1 To lng_lastKey 'removed by chang on 20140613
                for (long i = 1; i <= lng_lastKey; i++) // added by chang on 20140613: changed from Integer to Long
                {
                    if (dt.Select($"{DocKeyFieldName}='" + i.ToString() + "'").Length == 0)
                    {
                        lng_key = i;

                        break;
                    }
                }
            }

            return lng_key;
        }

        /// <summary>
        /// Use to get primary key value of sub table(s)
        /// </summary>
        /// <remarks></remarks>
        public static long GetDtlKey(string str_dbTableNameDetail)
        {
            // Dim dt As DataTable = myDBSetting.GetDataTable("SELECT DtlKey FROM " + str_dbTableNameDetail, False)
            DataTable dt = myDBSetting.GetDataTable("SELECT DtlKey FROM " + str_dbTableNameDetail, false);
            long lng_count = dt.Rows.Count;
            if (lng_count == 0)
                return 1;

            object obj_LastKey = dt.Compute("MAX(DtlKey)", "");
            long lng_lastKey = (obj_LastKey != null && obj_LastKey != DBNull.Value ? System.Convert.ToInt64(obj_LastKey) : 0);
            if (lng_lastKey == 0)
                return 1;

            long lng_key = lng_lastKey + 1;
            if (lng_lastKey != lng_count)
            {
                for (int i = 1; i <= lng_lastKey; i++)
                {
                    if (dt.Select("DtlKey='" + i.ToString() + "'").Length == 0)
                    {
                        lng_key = i;

                        break;
                    }
                }
            }

            return lng_key;
        }
        /// <summary>
        /// Use to get primary key value of sub table(s)
        /// </summary>
        /// <remarks>With extra Parameter DBSetting</remarks>
        //////////public static long GetDtlKey(string str_dbTableNameDetail, DBSetting dBSetting) //removed by chang on 20220907
        public static long GetDtlKey(string str_dbTableNameDetail, DBSetting dBSetting, string DtlKeyFieldName = "DtlKey") //added by chang on 20220907
        {
            //////////DataTable dt = dBSetting.GetDataTable("SELECT DtlKey FROM " + str_dbTableNameDetail, false); //removed by chang on 20220907
            DataTable dt = dBSetting.GetDataTable($"SELECT {DtlKeyFieldName} FROM " + str_dbTableNameDetail, false); //added by chang on 20220907
            long lng_count = dt.Rows.Count;
            if (lng_count == 0)
                return 1;

            //////////object obj_LastKey = dt.Compute("MAX(DtlKey)", ""); //removed by chang on 20220907
            object obj_LastKey = dt.Compute($"MAX({DtlKeyFieldName})", ""); //added by chang on 20220907
            long lng_lastKey = (obj_LastKey != null && obj_LastKey != DBNull.Value ? System.Convert.ToInt64(obj_LastKey) : 0);
            if (lng_lastKey == 0)
                return 1;

            long lng_key = lng_lastKey + 1;
            if (lng_lastKey != lng_count)
            {
                for (int i = 1; i <= lng_lastKey; i++)
                {
                    //////////if (dt.Select("DtlKey='" + i.ToString() + "'").Length == 0) //removed by chang on 20220907
                    if (dt.Select($"{DtlKeyFieldName}='" + i.ToString() + "'").Length == 0) //added by chang on 20220907
                    {
                        lng_key = i;

                        break;
                    }
                }
            }

            return lng_key;
        }

        public static decimal GetItemUnitPrice(string str_ItemCode) //added by chang on 20230609
        {
            decimal dec_UnitPrice = 0;

            try
            {
                AutoCount.Stock.Item.ItemDataAccess itemDataAccess = AutoCount.Stock.Item.ItemDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
                AutoCount.Stock.Item.ItemEntity itemEntity = itemDataAccess.LoadItem(str_ItemCode, AutoCount.Stock.Item.ItemEntryAction.View);
                if (itemEntity != null) dec_UnitPrice = itemEntity.BaseUomRecord.StandardSellingPrice;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(GetItemUnitPrice)}()", ex);
            }

            return dec_UnitPrice;
        }

        /// <summary>
        /// Use to identify the user's access rights
        /// </summary>
        /// <remarks></remarks>
        public static bool CheckPermissions(string str_CmdID)
        {
            bool boo_rtn = false;

            try
            {
                // edited by SCChang's Copilot on 20251126: added Design-Time check and null-safe operator
                // Original code (caused exception in Visual Studio Designer):
                // boo_rtn = AutoCount.Authentication.UserSession.CurrentUserSession.AccessRight.IsAccessibleByUserID(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID, str_CmdID);
                //
                // Problem: Property getters in BaseList_Form call this method, and PropertyGrid/TypeDescriptor may access these properties in Designer.
                // CurrentUserSession is null in Designer context, causing NullReferenceException.
                // Solution: Return false (no permission) in Design-Time or if CurrentUserSession is null.
                
                // If in Design Time or CurrentUserSession is null, return false (no permission)
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime ||
                    AutoCount.Authentication.UserSession.CurrentUserSession == null)
                {
                    return false;
                }
                
                // Runtime: check actual access rights
                // ''''boo_rtn = AutoCount.Authentication.UserAuthentication.GetOrCreate(dbs_dbsetting).AccessRight.IsAccessibleByUserID(AutoCount.Authentication.UserAuthentication.GetOrCreate(dbs_dbsetting).LoginUserID, str_CmdID)
                boo_rtn = AutoCount.Authentication.UserSession.CurrentUserSession.AccessRight.IsAccessibleByUserID(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID, str_CmdID);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(AutoCountHelper)}.{nameof(CheckPermissions)}()", ex);
            }

            return boo_rtn;
        }

        public static bool IsValidDatabase()
        {
            object obj = AutoCount.Authentication.UserSession.CurrentUserSession.DBSetting.ExecuteScalar("select count(*) from dbo.sysobjects where id = object_id(N'[dbo].[Registry]') and OBJECTPROPERTY(id, N'IsUserTable') = 1");
            if ((obj != null && obj != DBNull.Value))
                return (System.Convert.ToInt32(obj) > 0);
            else
                return false;
        }

        public static bool IsValidPlugInKeyName(string str_KeyName)
        {
            const string _id = "82432";
            bool boo_isValid = false;
            object obj = AutoCount.Authentication.UserSession.CurrentUserSession.DBSetting.ExecuteScalar(string.Format("select regvalue from registry where regid={0}", _id));

            // ''''Dim str_value As String = IIf(obj IsNot Nothing And obj IsNot DBNull.Value, obj.ToString(), "") 'removed by chang on 20140521
            string str_value = ""; // added by chang on 20140521
            if (obj != null && obj != DBNull.Value)
                str_value = obj.ToString(); // added by chang on 20140521: changed to using If Then Else instead of IIf() function 

            // isValid = value.Contains(KeyName)

            foreach (string s in str_value.Split(System.Convert.ToChar(",")))
            {
                if (s.TrimStart().TrimEnd() == str_KeyName)
                {
                    boo_isValid = true;

                    break;
                }
            }

            return boo_isValid;
        }

        // edited by Copilot on 20251007: v2.2.1.1, added UDF Data Dictionary classes for Gov Loan system
        #region " UDF Data Dictionary Classes "

        /// <summary>
        /// Data Dictionary class for UDF table structure
        /// Represents the design and data of User Defined Fields in AutoCount
        /// </summary>
        public class UDFDefinition
        {
            /// <summary>
            /// Primary Key (AutoKey from UDF table)
            /// </summary>
            public long AutoKey { get; set; }

            /// <summary>
            /// Table Name (e.g., SO, DO, IV, Item, SODTL, DODTL, IVDTL)
            /// </summary>
            public string TableName { get; set; }

            /// <summary>
            /// Field Name (e.g., TEN, Email, AGREEMENT)
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// Sequence number for display order
            /// </summary>
            public int Seq { get; set; }

            /// <summary>
            /// Field Type: T=Text, D=Decimal, I=Integer, A=Date, B=Boolean, M=Memo, L=ImageLink, R=RichText, S=System
            /// </summary>
            public char FieldType { get; set; }

            /// <summary>
            /// Display caption/label for the field
            /// </summary>
            public string Caption { get; set; }

            /// <summary>
            /// Properties string containing: ListName, Size, Precision, Scale, Required, etc.
            /// Format: key=value pairs separated by newlines
            /// </summary>
            public string Properties { get; set; }

            /// <summary>
            /// Unique identifier (GUID)
            /// </summary>
            public Guid Guid { get; set; }

            /// <summary>
            /// Parsed properties dictionary (lazy loaded)
            /// </summary>
            private System.Collections.Generic.Dictionary<string, string> _parsedProperties;

            /// <summary>
            /// Get parsed properties as Dictionary
            /// </summary>
            /// <returns>Dictionary of property key-value pairs</returns>
            public System.Collections.Generic.Dictionary<string, string> GetParsedProperties()
            {
                if (_parsedProperties == null)
                {
                    _parsedProperties = ParseUDFProperties(Properties);
                }
                return _parsedProperties;
            }
        }

        /// <summary>
        /// Data Dictionary class for UDFList table structure
        /// Represents dropdown list values for UDF fields
        /// </summary>
        public class UDFListDefinition
        {
            /// <summary>
            /// Primary Key (AutoKey from UDFList table)
            /// </summary>
            public long AutoKey { get; set; }

            /// <summary>
            /// List Name (e.g., PNNUMBER, PANGKAT, BANK, KAKITANGANPEMBEKAL, AREA)
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// List values (multi-line text, newline separated)
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Unique identifier (GUID)
            /// </summary>
            public Guid Guid { get; set; }
        }

        /// <summary>
        /// Data Dictionary class for UserFormula table structure
        /// Represents calculation formulas for UDF fields
        /// </summary>
        public class UserFormulaDefinition
        {
            /// <summary>
            /// Formula Name (e.g., SalesOrder, DeliveryOrder, Invoice, SalesOrderDetail, etc.)
            /// </summary>
            public string FormulaName { get; set; }

            /// <summary>
            /// Column Name (UDF field name, e.g., UDF_HARGARUNCIT, UDF_GPROFIT)
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// Formula Type: Init or ColumnChanged
            /// </summary>
            public string FormulaType { get; set; }

            /// <summary>
            /// Formula expression/calculation logic
            /// </summary>
            public string Formula { get; set; }

            /// <summary>
            /// Last update timestamp
            /// </summary>
            public long LastUpdate { get; set; }
        }

        #endregion " UDF Data Dictionary Classes "

        // edited by Copilot on 20251007: v2.2.1.1, added static data structures for UDF dictionaries
        // edited by Copilot on 20251009: v2.2.1.1, changed to public static properties to allow external initialization
        #region " UDF Data Dictionary Static Collections "

        /// <summary>
        /// UDF Dictionary: Organized by TableName, containing list of UDF field definitions
        /// Key: TableName (SO, DO, IV, Item, SODTL, DODTL, IVDTL)
        /// Value: List of UDFDefinition for that table
        /// </summary>
        /// <remarks>
        /// Dictionary provides fast lookup by table name, while List maintains field order by Seq.
        /// Initialized by UDFDataInitializer_Cls during plugin startup.
        /// </remarks>
        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<UDFDefinition>> UDFDictionary { get; set; }
            = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<UDFDefinition>>(7, System.StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// UDFList Dictionary: Dropdown list values for UDF fields
        /// Key: List Name (PNNUMBER, PANGKAT, BANK, KAKITANGANPEMBEKAL, AREA)
        /// Value: Array of list items
        /// </summary>
        /// <remarks>
        /// Dictionary provides fast lookup by list name.
        /// Initialized by UDFDataInitializer_Cls during plugin startup.
        /// </remarks>
        public static System.Collections.Generic.Dictionary<string, string[]> UDFListDictionary { get; set; }
            = new System.Collections.Generic.Dictionary<string, string[]>(5, System.StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// UserFormula List: Contains all formula definitions for UDF calculations
        /// </summary>
        /// <remarks>
        /// Simple list structure for sequential processing of all formulas.
        /// Initialized by UDFDataInitializer_Cls during plugin startup.
        /// </remarks>
        public static System.Collections.Generic.List<UserFormulaDefinition> UserFormulaList { get; set; }
            = new System.Collections.Generic.List<UserFormulaDefinition>(30);

        #endregion " UDF Data Dictionary Static Collections "

        // edited by Copilot on 20251009: v2.2.1.1, removed parser methods - now handled by UDFDataInitializer_Cls
        // UDF data initialization is now done in VTSAutocountPlugin.Classes.UDFDataInitializer_Cls
        // edited by Copilot on 20251009: v2.2.1.1, added helper methods for UDF property parsing and type conversion
        #region " UDF Data Dictionary Helper Methods "

        /// <summary>
        /// Get UDF definitions for a specific table
        /// </summary>
        /// <param name="tableName">Table name (SO, DO, IV, Item, SODTL, DODTL, IVDTL)</param>
        /// <returns>List of UDF definitions, or empty list if table not found</returns>
        public static System.Collections.Generic.List<UDFDefinition> GetUDFsByTable(string tableName)
        {
            if (UDFDictionary != null && UDFDictionary.ContainsKey(tableName))
            {
                return new System.Collections.Generic.List<UDFDefinition>(UDFDictionary[tableName]);
            }
            return new System.Collections.Generic.List<UDFDefinition>();
        }

        /// <summary>
        /// Get UDFList values by list name
        /// </summary>
        /// <param name="listName">List name (PNNUMBER, PANGKAT, BANK, KAKITANGANPEMBEKAL, AREA)</param>
        /// <returns>Array of list values, or empty array if list not found</returns>
        public static string[] GetUDFList(string listName)
        {
            if (UDFListDictionary != null && UDFListDictionary.ContainsKey(listName))
            {
                return (string[])UDFListDictionary[listName].Clone();
            }
            return new string[0];
        }

        /// <summary>
        /// Parse UDF Properties string into Dictionary
        /// </summary>
        /// <param name="properties">Properties string with key=value pairs separated by newlines</param>
        /// <returns>Dictionary of property key-value pairs</returns>
        /// <remarks>
        /// Format: key=value\nkey=value\n...
        /// Example: ListName=PNNUMBER\nSize=50\nRequired=False
        /// </remarks>
        public static System.Collections.Generic.Dictionary<string, string> ParseUDFProperties(string properties)
        {
            var result = new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(properties))
                return result;

            // Split by newline and parse key=value pairs
            string[] lines = properties.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                int equalIndex = trimmedLine.IndexOf('=');
                if (equalIndex > 0)
                {
                    string key = trimmedLine.Substring(0, equalIndex).Trim();
                    string value = trimmedLine.Substring(equalIndex + 1).Trim();
                    result[key] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert FieldType character to AutoCount.UDF.UDFType enum
        /// </summary>
        /// <param name="fieldType">Single character representing UDF type</param>
        /// <returns>Corresponding AutoCount.UDF.UDFType enum value</returns>
        /// <remarks>
        /// Based on AutoCount UDF field type mapping:
        /// T = Text, D = Decimal, I = Integer, A = Date, B = Boolean, 
        /// M = Memo, L = ImageLink, R = RichText, S = System
        /// </remarks>
        public static AutoCount.UDF.UDFType GetUDFTypeFromChar(char fieldType)
        {
            switch (fieldType)
            {
                case 'T': return AutoCount.UDF.UDFType.Text;
                case 'D': return AutoCount.UDF.UDFType.Decimal;
                case 'I': return AutoCount.UDF.UDFType.Integer;
                case 'A': return AutoCount.UDF.UDFType.Date;
                case 'B': return AutoCount.UDF.UDFType.Boolean;
                case 'M': return AutoCount.UDF.UDFType.Memo;
                case 'L': return AutoCount.UDF.UDFType.ImageLink;
                case 'R': return AutoCount.UDF.UDFType.RichText;
                case 'S': return AutoCount.UDF.UDFType.System;
                default:
                    System.Diagnostics.Debug.WriteLine($"[AutoCountHelper] Unknown FieldType '{fieldType}', defaulting to Text");
                    return AutoCount.UDF.UDFType.Text;
            }
        }

        /// <summary>
        /// Convert our UDFSystemType enum to AutoCount's SystemType enum string name
        /// </summary>
        /// <param name="systemType">Our UDFSystemType enum value</param>
        /// <returns>AutoCount SystemType enum string name (e.g., "AreaCode", "ItemCode")</returns>
        /// <remarks>
        /// Maps our simplified enum to AutoCount's exact enum names.
        /// AutoCount uses specific names like "AreaCode" instead of "Area", "ItemCode" instead of "Item".
        /// Reference: AutoCount.UDF.SystemType enum
        /// </remarks>
        public static string GetAutoCountSystemTypeName(UDFSystemType systemType)
        {
            switch (systemType)
            {
                case UDFSystemType.Account:
                    return "Account";
                case UDFSystemType.Area:
                    return "AreaCode"; // AutoCount uses "AreaCode" not "Area"
                case UDFSystemType.CNType:
                    return "CNType";
                case UDFSystemType.Creditor:
                    return "Creditor";
                case UDFSystemType.CreditorType:
                    return "CreditorType";
                case UDFSystemType.CreditTerm:
                    return "CreditTerm";
                case UDFSystemType.Currency:
                    return "Currency";
                case UDFSystemType.Debtor:
                    return "Debtor";
                case UDFSystemType.DebtorType:
                    return "DebtorType";
                case UDFSystemType.Department:
                    return "Department";
                case UDFSystemType.DNType:
                    return "DNType";
                case UDFSystemType.Item:
                    return "ItemCode"; // AutoCount uses "ItemCode" not "Item"
                case UDFSystemType.ItemBrand:
                    return "ItemBrand";
                case UDFSystemType.ItemCategory:
                    return "ItemCategory";
                case UDFSystemType.ItemClass:
                    return "ItemClass";
                case UDFSystemType.ItemGroup:
                    return "ItemGroup";
                case UDFSystemType.ItemType:
                    return "ItemType";
                case UDFSystemType.Location:
                    return "Location";
                case UDFSystemType.PaymentMethod:
                    return "PaymentMethod";
                case UDFSystemType.PriceCategory:
                    return "PriceCategory";
                case UDFSystemType.Project:
                    return "Project";
                case UDFSystemType.PurchaseAgent:
                    return "PurchaseAgent";
                case UDFSystemType.SalesAgent:
                    return "SalesAgent";
                case UDFSystemType.ShippingMethod:
                    return "ShippingMethod";
                case UDFSystemType.None:
                default:
                    System.Diagnostics.Debug.WriteLine($"[AutoCountHelper] UDFSystemType '{systemType}' has no AutoCount mapping, using empty string");
                    return "";
            }
        }

        /// <summary>
        /// Convert CustomDataType string to UDFSystemType enum
        /// </summary>
        /// <param name="customDataType">CustomDataType string from UDF Properties (e.g., "Payment Method", "Sales Agent")</param>
        /// <returns>Corresponding UDFSystemType enum value</returns>
        /// <remarks>
        /// Based on AutoCount.UDF.SystemType enum:
        /// - "Account" -> Account
        /// - "AreaCode" -> Area (AutoCount uses AreaCode)
        /// - "CNType" -> CNType
        /// - "CreditTerm" -> CreditTerm
        /// - "Creditor" -> Creditor
        /// - "CreditorType" -> CreditorType
        /// - "Currency" -> Currency
        /// - "DNType" -> DNType
        /// - "Debtor" -> Debtor
        /// - "DebtorType" -> DebtorType
        /// - "Department" -> Department
        /// - "ItemCode" -> Item (AutoCount uses ItemCode)
        /// - "ItemBrand" -> ItemBrand
        /// - "ItemCategory" -> ItemCategory
        /// - "ItemClass" -> ItemClass
        /// - "ItemGroup" -> ItemGroup
        /// - "ItemType" -> ItemType
        /// - "Location" -> Location
        /// - "PaymentMethod" -> PaymentMethod
        /// - "PriceCategory" -> PriceCategory
        /// - "Project" -> Project
        /// - "PurchaseAgent" -> PurchaseAgent
        /// - "SalesAgent" -> SalesAgent
        /// - "ShippingMethod" -> ShippingMethod
        /// </remarks>
        public static UDFSystemType GetUDFSystemType(string customDataType)
        {
            if (string.IsNullOrWhiteSpace(customDataType))
                return UDFSystemType.None;

            // Remove spaces and special characters for enum parsing
            string normalizedType = customDataType.Replace(" ", "").Replace("/", "");

            // Handle special mappings from AutoCount.UDF.SystemType to our UDFSystemType enum
            switch (normalizedType.ToUpperInvariant())
            {
                case "ACCOUNT":
                    return UDFSystemType.Account;
                case "AREA":
                case "AREACODE": // AutoCount uses AreaCode
                    return UDFSystemType.Area;
                case "CNTYPE":
                    return UDFSystemType.CNType;
                case "CREDITOR":
                    return UDFSystemType.Creditor;
                case "CREDITORTYPE":
                    return UDFSystemType.CreditorType;
                case "CREDITTERM":
                    return UDFSystemType.CreditTerm;
                case "CURRENCY":
                    return UDFSystemType.Currency;
                case "DEBTOR":
                    return UDFSystemType.Debtor;
                case "DEBTORTYPE":
                    return UDFSystemType.DebtorType;
                case "DEPARTMENT":
                    return UDFSystemType.Department;
                case "DNTYPE":
                    return UDFSystemType.DNType;
                case "ITEM":
                case "ITEMCODE": // AutoCount uses ItemCode
                    return UDFSystemType.Item;
                case "ITEMBRAND":
                    return UDFSystemType.ItemBrand;
                case "ITEMCATEGORY":
                    return UDFSystemType.ItemCategory;
                case "ITEMCLASS":
                    return UDFSystemType.ItemClass;
                case "ITEMGROUP":
                    return UDFSystemType.ItemGroup;
                case "ITEMTYPE":
                    return UDFSystemType.ItemType;
                case "LOCATION":
                    return UDFSystemType.Location;
                case "PAYMENTMETHOD":
                    return UDFSystemType.PaymentMethod;
                case "PRICECATEGORY":
                    return UDFSystemType.PriceCategory;
                case "PROJECT":
                    return UDFSystemType.Project;
                case "PURCHASEAGENT":
                    return UDFSystemType.PurchaseAgent;
                case "SALESAGENT":
                    return UDFSystemType.SalesAgent;
                case "SHIPPINGMETHOD":
                    return UDFSystemType.ShippingMethod;
                default:
                    System.Diagnostics.Debug.WriteLine($"[AutoCountHelper] Unknown CustomDataType '{customDataType}', defaulting to None");
                    return UDFSystemType.None;
            }
        }

        /// <summary>
        /// Get all UserFormula definitions
        /// </summary>
        /// <returns>Copy of UserFormula list</returns>
        public static System.Collections.Generic.List<UserFormulaDefinition> GetUserFormulas()
        {
            if (UserFormulaList != null)
            {
                return new System.Collections.Generic.List<UserFormulaDefinition>(UserFormulaList);
            }
            return new System.Collections.Generic.List<UserFormulaDefinition>();
        }

        /// <summary>
        /// Parse UDF section from text file
        /// Handles multi-line Properties field
        /// </summary>
        private static void ParseUDFSection(string section)
        {
            UDFDictionary.Clear();

            // Find "Data" section start
            int dataStartIndex = section.IndexOf("Data\r\n>>>>");
            if (dataStartIndex == -1) return;

            string dataSection = section.Substring(dataStartIndex + 10); // Skip "Data\r\n>>>>"
            string[] lines = dataSection.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);

            int i = 0;
            while (i < lines.Length)
            {
                string line = lines[i].Trim();
                
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    continue;
                }

                // Parse main data line (tab-separated)
                string[] parts = line.Split('\t');
                
                if (parts.Length >= 6)
                {
                    UDFDefinition udf = new UDFDefinition
                    {
                        AutoKey = long.Parse(parts[0]),
                        TableName = parts[1],
                        FieldName = parts[2],
                        Seq = int.Parse(parts[3]),
                        FieldType = parts[4].Length > 0 ? parts[4][0] : 'T',
                        Caption = parts[5]
                    };

                    // Properties field spans multiple lines until we hit a GUID line
                    System.Text.StringBuilder propertiesBuilder = new System.Text.StringBuilder();
                    i++;
                    
                    while (i < lines.Length)
                    {
                        string nextLine = lines[i];
                        
                        // Check if this line is a GUID (starts with tab and contains GUID format)
                        if (nextLine.StartsWith("\t") && System.Guid.TryParse(nextLine.Trim(), out System.Guid guid))
                        {
                            udf.Properties = propertiesBuilder.ToString().TrimEnd('\r', '\n');
                            udf.Guid = guid;
                            i++;
                            break;
                        }
                        else
                        {
                            // This is part of Properties
                            propertiesBuilder.AppendLine(nextLine);
                            i++;
                        }
                    }

                    // Add to dictionary
                    if (!UDFDictionary.ContainsKey(udf.TableName))
                    {
                        UDFDictionary[udf.TableName] = new System.Collections.Generic.List<UDFDefinition>();
                    }
                    UDFDictionary[udf.TableName].Add(udf);
                }
                else
                {
                    i++;
                }
            }

            // Sort each list by Seq to maintain order
            foreach (var list in UDFDictionary.Values)
            {
                list.Sort((a, b) => a.Seq.CompareTo(b.Seq));
            }
        }

        /// <summary>
        /// Parse UDFList section from text file
        /// Handles multi-line Value arrays (like PNNUMBER with 47 items)
        /// </summary>
        private static void ParseUDFListSection(string section)
        {
            UDFListDictionary.Clear();

            // Find "Data" section start
            int dataStartIndex = section.IndexOf("Data\r\n>>>>");
            if (dataStartIndex == -1) return;

            string dataSection = section.Substring(dataStartIndex + 10);
            string[] lines = dataSection.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.None);

            int i = 0;
            while (i < lines.Length)
            {
                string line = lines[i].Trim();
                
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    i++;
                    continue;
                }

                // Parse main data line (tab-separated)
                string[] parts = line.Split('\t');
                
                if (parts.Length >= 2)
                {
                    string name = parts[1];
                    System.Collections.Generic.List<string> valuesList = new System.Collections.Generic.List<string>();

                    // Check if there's a value on the same line
                    if (parts.Length >= 3 && !string.IsNullOrWhiteSpace(parts[2]))
                    {
                        // Value on same line, might contain first item
                        string firstValue = parts[2];
                        
                        // Check if this line also has GUID
                        if (parts.Length >= 4 && System.Guid.TryParse(parts[3], out _))
                        {
                            // Value and GUID on same line
                            if (!string.IsNullOrWhiteSpace(firstValue))
                            {
                                valuesList.Add(firstValue);
                            }
                            i++;
                        }
                        else
                        {
                            // Multi-line values
                            valuesList.Add(firstValue);
                            i++;
                            
                            // Read subsequent value lines until we hit a GUID
                            while (i < lines.Length)
                            {
                                string nextLine = lines[i];
                                
                                // Check if this line contains a GUID (last part after tab)
                                string[] valueParts = nextLine.Split('\t');
                                bool hasGuid = false;
                                
                                foreach (string part in valueParts)
                                {
                                    if (System.Guid.TryParse(part.Trim(), out _))
                                    {
                                        hasGuid = true;
                                        break;
                                    }
                                }
                                
                                if (hasGuid)
                                {
                                    // Last value line - add non-GUID parts
                                    for (int j = 0; j < valueParts.Length; j++)
                                    {
                                        string part = valueParts[j].Trim();
                                        if (!string.IsNullOrWhiteSpace(part) && !System.Guid.TryParse(part, out _))
                                        {
                                            valuesList.Add(part);
                                        }
                                    }
                                    i++;
                                    break;
                                }
                                else if (!string.IsNullOrWhiteSpace(nextLine.Trim()))
                                {
                                    // Regular value line
                                    valuesList.Add(nextLine.Trim());
                                    i++;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Empty value (like PANGKAT)
                        i++;
                        
                        // Skip to GUID line
                        while (i < lines.Length)
                        {
                            string nextLine = lines[i].Trim();
                            if (System.Guid.TryParse(nextLine, out _))
                            {
                                i++;
                                break;
                            }
                            i++;
                        }
                    }

                    UDFListDictionary[name] = valuesList.ToArray();
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Parse UserFormula section from text file
        /// Handles multi-line Formula field
        /// </summary>
        private static void ParseUserFormulaSection(string section)
        {
            UserFormulaList.Clear();

            // Find "Data" section start
            int dataStartIndex = section.IndexOf("Data\r\n>>>>");
            if (dataStartIndex == -1) return;

            string dataSection = section.Substring(dataStartIndex + 10);
            string[] lines = dataSection.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('\t');
                
                if (parts.Length >= 5)
                {
                    UserFormulaDefinition formula = new UserFormulaDefinition
                    {
                        FormulaName = parts[0],
                        ColumnName = parts[1],
                        FormulaType = parts[2],
                        Formula = parts[3],
                        LastUpdate = long.Parse(parts[4])
                    };

                    UserFormulaList.Add(formula);
                }
            }
        }

        /// <summary>
        /// Get UDF definitions for a specific table
        /// </summary>
        /// <param name="tableName">Table name (SO, DO, IV, Item, SODTL, DODTL, IVDTL)</param>
        /// <returns>List of UDF definitions, or empty list if table not found</returns>
        public static System.Collections.Generic.List<UDFDefinition> GetUDFDefinitions(string tableName)
        {
            if (UDFDictionary.ContainsKey(tableName))
            {
                return UDFDictionary[tableName];
            }
            return new System.Collections.Generic.List<UDFDefinition>();
        }

        /// <summary>
        /// Get UDFList values for a specific list name
        /// </summary>
        /// <param name="listName">List name (PNNUMBER, PANGKAT, BANK, KAKITANGANPEMBEKAL, AREA)</param>
        /// <returns>Array of list values, or empty array if list not found</returns>
        public static string[] GetUDFListValues(string listName)
        {
            if (UDFListDictionary.ContainsKey(listName))
            {
                return UDFListDictionary[listName];
            }
            return new string[0];
        }

        #endregion " UDF Data File Parser "
    }
}
