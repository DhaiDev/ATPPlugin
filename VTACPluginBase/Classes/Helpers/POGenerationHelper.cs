using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using AutoCount;
using AutoCount.Authentication;
using AutoCount.Invoicing.Purchase.PurchaseOrder;

using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.Helpers
{
    /// <summary>
    /// Static helper class for generating Purchase Orders from Sales Orders
    /// Created by SCChang's Copilot on 20251203: v2.2.1.0, Task 3.2 - PO Generation Mechanism
    /// Refactored to static class on 20251204: removed constructor, uses myDBSetting and UserSession.CurrentUserSession
    /// Modified on 20251208: Added dual-mode support (PO Form mode / Direct Save mode)
    /// Modified on 20251231: Added BatchLimit property for configurable batch size
    /// </summary>
    public static class POGenerationHelper
    {
        #region " Configuration Properties "

        private const string CONFIG_MODULE = "POGeneration";
        private const string CONFIG_KEY_BATCH_LIMIT = "BatchLimit";
        private const int DEFAULT_BATCH_LIMIT = 100;

        /// <summary>
        /// Gets the batch limit for PO generation from zVTS_SysConfig.
        /// If config not exists, creates it with default value 100.
        /// ConfigModule: POGeneration, ConfigKey: BatchLimit
        /// </summary>
        public static int BatchLimit
        {
            get
            {
                try
                {
                    // Use SystemHelper.GetSysConfigValue to query config
                    dynamic configValue = SystemHelper.GetSysConfigValue(CONFIG_MODULE, CONFIG_KEY_BATCH_LIMIT);

                    if (configValue != null)
                    {
                        int value;
                        if (int.TryParse(configValue.ToString(), out value) && value > 0)
                            return value;
                    }

                    // Config not exists or invalid, create with default value
                    SystemHelper.SetSysConfigValue(
                        CONFIG_MODULE, 
                        CONFIG_KEY_BATCH_LIMIT, 
                        DEFAULT_BATCH_LIMIT, 
                        "Maximum number of items per PO generation batch");

                    return DEFAULT_BATCH_LIMIT;
                }
                catch (Exception ex)
                {
                    ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(BatchLimit)}", ex);
                    return DEFAULT_BATCH_LIMIT;
                }
            }
        }

        #endregion " Configuration Properties "

        /// <summary>
        /// Generate PO from selected SO Detail items
        /// Supports two modes: PO Form mode (opens PO for user verification) and Direct Save mode
        /// </summary>
        /// <param name="selectedSODtlKeys">Array of SODTL.DtlKey to generate PO from</param>
        /// <param name="mergeItems">Whether to merge same ItemCode+UOM (default: false)</param>
        /// <param name="usePOFormMode">Whether to open PO Form for user verification (default: true)</param>
        /// <returns>
        /// Direct Save mode: Dictionary of CreditorCode -> Generated PO DocNo
        /// PO Form mode: Empty dictionary (PO not saved yet, AfterSave script will handle SODTL update)
        /// </returns>
        public static Dictionary<string, string> GeneratePOFromSelectedSO(
            long[] selectedSODtlKeys, 
            bool mergeItems = false,
            bool usePOFormMode = true)
        {
            var result = new Dictionary<string, string>();

            try
            {
                // Step 1: Load SO Detail data
                DataTable dtSOItems = LoadSODetailData(selectedSODtlKeys);
                if (dtSOItems.Rows.Count == 0)
                {
                    ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GeneratePOFromSelectedSO)}()", 
                        new Exception("No valid SO items found for the selected keys."));
                    return result;
                }

                // Step 2: Group by Creditor (via Item.MainSupplier), separating items without MainSupplier
                DataTable itemsWithoutSupplier;
                var groupedByCreditor = GroupByCreditorWithNoSupplier(myDBSetting, dtSOItems, out itemsWithoutSupplier);

                // Step 2.1: Check for items without MainSupplier
                if (itemsWithoutSupplier.Rows.Count > 0)
                {
                    if (!usePOFormMode)
                    {
                        // Direct Save mode: Show warning and abort
                        var itemCodesWithoutSupplier = itemsWithoutSupplier.AsEnumerable()
                            .Select(r => r.Field<string>("ItemCode"))
                            .Distinct()
                            .ToList();
                        
                        string itemList = string.Join(", ", itemCodesWithoutSupplier.Take(10));
                        if (itemCodesWithoutSupplier.Count > 10)
                            itemList += $" ... and {itemCodesWithoutSupplier.Count - 10} more";
                        
                        AppMessage.ShowWarningMessage(
                            $"The following items do not have Main Supplier assigned:\n\n{itemList}\n\nPlease assign Main Supplier to these items before generating PO.");
                        
                        GeneralLogger_Cls.Write(
                            $"PO generation aborted: {itemCodesWithoutSupplier.Count} items without MainSupplier", LogLevel.WARNING);
                        return result;
                    }
                    // PO Form mode: Will create a separate PO without Creditor for these items (handled below)
                }

                // Step 3: Generate PO for each Creditor
                foreach (var creditorGroup in groupedByCreditor)
                {
                    string creditorCode = creditorGroup.Key;
                    DataTable creditorItems = creditorGroup.Value;

                    try
                    {
                        // Pass usePOFormMode to CreatePurchaseOrder
                        string poDocNo = CreatePurchaseOrder(creditorCode, creditorItems, mergeItems, usePOFormMode);
                        
                        if (!usePOFormMode && !string.IsNullOrEmpty(poDocNo))
                        {
                            // Direct Save mode: Update SODTL UDF immediately
                            result[creditorCode] = poDocNo;
                            UpdateSODTLAfterPOGenerated(creditorItems, poDocNo);
                        }
                        // PO Form mode: SODTL UDF will be updated by PO AfterSave Script
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GeneratePOFromSelectedSO)}(Creditor:{creditorCode})", ex);
                    }
                }

                // Step 4: PO Form mode - Create a separate PO for items without MainSupplier (no Creditor assigned)
                if (usePOFormMode && itemsWithoutSupplier.Rows.Count > 0)
                {
                    try
                    {
                        // Create PO without Creditor Code - user must assign manually
                        CreatePurchaseOrderWithoutCreditor(itemsWithoutSupplier, mergeItems);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GeneratePOFromSelectedSO)}(NoCreditor)", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GeneratePOFromSelectedSO)}()", ex);
            }

            return result;
        }

        /// <summary>
        /// Load SO Detail data using direct SQL query
        /// Modified by SCChang's Copilot on 20251231: Replaced LoadBackOrderFromSO with direct SQL (v2.2 API compatibility)
        /// </summary>
        private static DataTable LoadSODetailData(long[] selectedSODtlKeys)
        {
            try
            {
                if (selectedSODtlKeys == null || selectedSODtlKeys.Length == 0)
                    return new DataTable();

                // Build IN clause for DtlKeys
                string dtlKeyList = string.Join(",", selectedSODtlKeys);
                
                // Query SO Detail data with relevant fields for PO generation
                string sql = $@"
                    SELECT 
                        d.DtlKey,
                        d.DocKey,
                        h.DocNo,
                        h.DebtorCode,
                        d.ItemCode,
                        d.Description,
                        d.Qty,
                        d.UOM,
                        d.Rate,
                        d.SmallestQty,
                        d.UnitPrice,
                        d.SubTotal,
                        d.TaxType,
                        d.TaxRate,
                        d.TaxAmt,
                        d.Seq
                    FROM SODTL d
                    INNER JOIN SO h ON d.DocKey = h.DocKey
                    WHERE d.DtlKey IN ({dtlKeyList})
                    ORDER BY h.DocNo, d.Seq";

                DataTable dt = myDBSetting.GetDataTable(sql, false);
                return dt;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(LoadSODetailData)}()", ex);
                return new DataTable();
            }
        }

        /// <summary>
        /// Group SO items by CreditorCode (from Item.MainSupplier), separating items without MainSupplier
        /// Modified by SCChang's Copilot on 20251208: Changed to lookup MainSupplier from Item table
        /// Modified by SCChang's Copilot on 20251208: Added itemsWithoutSupplier output parameter
        /// </summary>
        /// <param name="dbSetting">DBSetting for database queries</param>
        /// <param name="dtSOItems">SO items DataTable</param>
        /// <param name="itemsWithoutSupplier">Output: DataTable of items that have no MainSupplier</param>
        /// <returns>Dictionary of CreditorCode -> DataTable of items</returns>
        private static Dictionary<string, DataTable> GroupByCreditorWithNoSupplier(
            AutoCount.Data.DBSetting dbSetting, 
            DataTable dtSOItems,
            out DataTable itemsWithoutSupplier)
        {
            var result = new Dictionary<string, DataTable>();
            itemsWithoutSupplier = dtSOItems.Clone();

            try
            {
                // Step 1: Collect all unique ItemCodes from SO items
                var itemCodes = dtSOItems.AsEnumerable()
                    .Select(r => r.Field<string>("ItemCode"))
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .ToList();

                if (itemCodes.Count == 0)
                {
                    ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GroupByCreditorWithNoSupplier)}()",
                        new Exception("No ItemCodes found in SO items."));
                    return result;
                }

                // Step 2: Get MainSupplier for each ItemCode from Item table
                var itemMainSuppliers = GetItemMainSuppliers(dbSetting, itemCodes);

                // Step 3: Group SO items by their Item's MainSupplier
                var creditorGroups = new Dictionary<string, List<DataRow>>();
                var noSupplierRows = new List<DataRow>();

                foreach (DataRow row in dtSOItems.Rows)
                {
                    string itemCode = row["ItemCode"].ToString();
                    string creditorCode;

                    if (itemMainSuppliers.TryGetValue(itemCode, out creditorCode) && !string.IsNullOrEmpty(creditorCode))
                    {
                        // Item has MainSupplier - group by CreditorCode
                        if (!creditorGroups.ContainsKey(creditorCode))
                        {
                            creditorGroups[creditorCode] = new List<DataRow>();
                        }
                        creditorGroups[creditorCode].Add(row);
                    }
                    else
                    {
                        // Item has no MainSupplier - collect separately
                        noSupplierRows.Add(row);
                        GeneralLogger_Cls.Write(
                            $"Item {itemCode} has no MainSupplier assigned.", LogLevel.WARNING);
                    }
                }

                // Step 4: Convert grouped rows to DataTables
                foreach (var group in creditorGroups)
                {
                    DataTable dtCreditorItems = dtSOItems.Clone();
                    foreach (DataRow row in group.Value)
                    {
                        dtCreditorItems.ImportRow(row);
                    }

                    if (dtCreditorItems.Rows.Count > 0)
                    {
                        result[group.Key] = dtCreditorItems;
                    }
                }

                // Step 5: Populate itemsWithoutSupplier output
                foreach (DataRow row in noSupplierRows)
                {
                    itemsWithoutSupplier.ImportRow(row);
                }

                GeneralLogger_Cls.Write(
                    $"Grouped {dtSOItems.Rows.Count} SO items: {result.Count} Creditor groups, {itemsWithoutSupplier.Rows.Count} without supplier", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GroupByCreditorWithNoSupplier)}()", ex);
            }

            return result;
        }

        /// <summary>
        /// Get MainSupplier (CreditorCode) for each ItemCode from Item table
        /// Created by SCChang's Copilot on 20251208: Query Item.MainSupplier for grouping
        /// </summary>
        /// <param name="dbSetting">DBSetting for database queries</param>
        /// <param name="itemCodes">List of ItemCodes to query</param>
        /// <returns>Dictionary of ItemCode -> MainSupplier (CreditorCode)</returns>
        private static Dictionary<string, string> GetItemMainSuppliers(AutoCount.Data.DBSetting dbSetting, List<string> itemCodes)
        {
            var result = new Dictionary<string, string>();

            try
            {
                if (itemCodes == null || itemCodes.Count == 0)
                    return result;

                // Build IN clause with quoted values
                string inClause = string.Join(",", itemCodes.Select(c => $"'{c.Replace("'", "''")}'"));
                string sql = $"SELECT ItemCode, MainSupplier FROM Item WHERE ItemCode IN ({inClause})";

                // Execute query
                DataTable dt = dbSetting.GetDataTable(sql, false);

                foreach (DataRow row in dt.Rows)
                {
                    string itemCode = row["ItemCode"].ToString();
                    string mainSupplier = row["MainSupplier"]?.ToString() ?? string.Empty;
                    result[itemCode] = mainSupplier;
                }

                GeneralLogger_Cls.Write(
                    $"Retrieved MainSupplier for {result.Count}/{itemCodes.Count} items", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GetItemMainSuppliers)}()", ex);
            }

            return result;
        }

        /// <summary>
        /// Create a Purchase Order for a specific Creditor
        /// Modified by SCChang's Copilot on 20251208: Added usePOFormMode parameter for dual-mode support
        /// </summary>
        /// <param name="creditorCode">Creditor code</param>
        /// <param name="soItems">SO items DataTable</param>
        /// <param name="mergeItems">Whether to merge same ItemCode</param>
        /// <param name="usePOFormMode">true = open PO Form for verification, false = direct save</param>
        /// <returns>PO DocNo (empty string in PO Form mode)</returns>
        private static string CreatePurchaseOrder(string creditorCode, DataTable soItems, bool mergeItems, bool usePOFormMode)
        {
            try
            {
                PurchaseOrderCommand cmd = PurchaseOrderCommand.Create(UserSession.CurrentUserSession, myDBSetting);
                PurchaseOrder po = cmd.AddNew(true);

                // Set PO Header
                po.CreditorCode = creditorCode;
                po.DocDate = DateTime.Today;
                po.Description = $"Generated from SO (Auto)";

                // Collect source SODtlKeys for this PO
                var sourceDtlKeys = soItems.AsEnumerable()
                    .Select(r => r.Field<long>("DtlKey").ToString())
                    .ToList();

                // Add PO Details
                if (mergeItems)
                {
                    AddPODetailsMerged(po, soItems, sourceDtlKeys);
                }
                else
                {
                    // One-to-one mapping (preserve transfer linkage)
                    AddPODetailsOneToOne(po, soItems);
                }

                if (usePOFormMode)
                {
                    // PO Form mode: Open PO Form for user verification (do NOT save)
                    // Create FormPurchaseOrderEntry and show it
                    var poForm = new FormPurchaseOrderEntry(po);
                    poForm.Show();
                    
                    GeneralLogger_Cls.Write($"PO Form opened for Creditor {creditorCode} (PO Form mode)", LogLevel.INFO);
                    return string.Empty; // PO not saved yet
                }
                else
                {
                    // Direct Save mode: Save PO immediately
                    po.Save();

                    string poDocNo = po.DocNo;
                    GeneralLogger_Cls.Write($"PO {poDocNo} created successfully for Creditor {creditorCode} (Direct Save mode)", LogLevel.SUCCESS);

                    return poDocNo;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(CreatePurchaseOrder)}()", ex);
                throw;
            }
        }

        /// <summary>
        /// Create a Purchase Order WITHOUT Creditor Code for items that have no MainSupplier
        /// Only used in PO Form mode - user must manually assign Creditor before saving
        /// Created by SCChang's Copilot on 20251208: Handle items without MainSupplier
        /// </summary>
        /// <param name="soItems">SO items DataTable (items without MainSupplier)</param>
        /// <param name="mergeItems">Whether to merge same ItemCode</param>
        private static void CreatePurchaseOrderWithoutCreditor(DataTable soItems, bool mergeItems)
        {
            try
            {
                PurchaseOrderCommand cmd = PurchaseOrderCommand.Create(UserSession.CurrentUserSession, myDBSetting);
                PurchaseOrder po = cmd.AddNew(true);

                // Set PO Header - NO CreditorCode assigned
                po.DocDate = DateTime.Today;
                po.Description = $"Generated from SO (Auto) - Please assign Creditor";

                // Collect source SODtlKeys for this PO
                var sourceDtlKeys = soItems.AsEnumerable()
                    .Select(r => r.Field<long>("DtlKey").ToString())
                    .ToList();

                // Log item codes without MainSupplier
                var itemCodesWithoutSupplier = soItems.AsEnumerable()
                    .Select(r => r.Field<string>("ItemCode"))
                    .Distinct()
                    .ToList();
                GeneralLogger_Cls.Write(
                    $"Creating PO without Creditor for {itemCodesWithoutSupplier.Count} items: {string.Join(", ", itemCodesWithoutSupplier.Take(5))}...", LogLevel.WARNING);

                // Add PO Details
                if (mergeItems)
                {
                    AddPODetailsMerged(po, soItems, sourceDtlKeys);
                }
                else
                {
                    AddPODetailsOneToOne(po, soItems);
                }

                // PO Form mode only: Open PO Form for user verification (do NOT save)
                // Create FormPurchaseOrderEntry and show it
                var poForm = new FormPurchaseOrderEntry(po);
                poForm.Show();
                
                GeneralLogger_Cls.Write($"PO Form opened (no Creditor assigned) - User must assign Creditor manually", LogLevel.INFO);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(CreatePurchaseOrderWithoutCreditor)}()", ex);
                throw;
            }
        }

        /// <summary>
        /// Add PO Details with one-to-one mapping to SO Details
        /// Uses AutoCount's AddSOToPOTransferDetail for proper Transfer linkage
        /// Modified by SCChang's Copilot on 20251208: Record SourceSODtlKeys to PODTL UDF
        /// </summary>
        private static void AddPODetailsOneToOne(PurchaseOrder po, DataTable soItems)
        {
            try
            {
                foreach (DataRow soRow in soItems.Rows)
                {
                    long soDtlKey = Convert.ToInt64(soRow["DtlKey"]);
                    
                    // Use AutoCount's standard Transfer method
                    var transferDetail = po.AddSOToPOTransferDetail(soDtlKey);
                    
                    // Set Qty (TransferDetail will auto-fill ItemCode, UOM, etc. from SO)
                    // v2.2 API uses QtyWantToTransfer instead of Qty
                    transferDetail.QtyWantToTransfer = Convert.ToDecimal(soRow["Qty"]);
                    
                    // Record source SODtlKey to PODTL UDF for AfterSave script
                    // v2.2 API uses indexer syntax: detail["UDF_FieldName"] = value
                    transferDetail["UDF_SourceSODtlKeys"] = soDtlKey.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(AddPODetailsOneToOne)}()", ex);
                throw;
            }
        }

        /// <summary>
        /// Add PO Details with merged items (Task 3.3)
        /// Groups items by ItemCode, converts to SmallestQty using Rate, then splits by UOM hierarchy
        /// Creates multiple PO detail rows per ItemCode to avoid fractional quantities
        /// WARNING: Merging breaks standard Transfer linkage (no FromDoc set)
        /// Design Decision: Only SODTL UDF records PODocNo/PODocKey (Option C)
        /// Modified by SCChang's Copilot on 20251208: Added sourceDtlKeys parameter, record to PODTL UDF
        /// </summary>
        /// <param name="po">PurchaseOrder object</param>
        /// <param name="soItems">SO items DataTable</param>
        /// <param name="sourceDtlKeys">List of source SODtl DtlKeys (for PODTL UDF)</param>
        private static void AddPODetailsMerged(PurchaseOrder po, DataTable soItems, List<string> sourceDtlKeys)
        {
            try
            {
                int totalPODetails = 0;
                
                // All source SODtlKeys for this merged PO (comma separated)
                string allSourceKeys = string.Join(",", sourceDtlKeys);

                // Group by ItemCode only (not UOM), sum SmallestQty
                // Also collect source DtlKeys per ItemCode
                var groupedItems = soItems.AsEnumerable()
                    .GroupBy(r => r.Field<string>("ItemCode"))
                    .Select(g => new
                    {
                        ItemCode = g.Key,
                        // Sum all SmallestQty (already converted by Rate in SO)
                        TotalSmallestQty = g.Sum(r => Convert.ToDecimal(r["SmallestQty"])),
                        SourceCount = g.Count(),
                        // Collect DtlKeys for this ItemCode
                        SourceDtlKeys = string.Join(",", g.Select(r => r.Field<long>("DtlKey").ToString()))
                    })
                    .ToList();

                foreach (var item in groupedItems)
                {
                    // Get all UOMs for this item, ordered by Rate DESC
                    var allUOMs = GetItemAllUOMs(item.ItemCode);

                    if (allUOMs.Count == 0)
                    {
                        // Fallback: If no UOM found, use PurchaseUOM
                        string purchaseUOM;
                        decimal purchaseUOMRate;
                        GetItemPurchaseUOMInfo(item.ItemCode, out purchaseUOM, out purchaseUOMRate);

                        decimal qty = purchaseUOMRate > 0
                            ? item.TotalSmallestQty / purchaseUOMRate
                            : item.TotalSmallestQty;

                        var dtl = po.AddDetail();
                        dtl.ItemCode = item.ItemCode;
                        if (!string.IsNullOrEmpty(purchaseUOM))
                        {
                            dtl.UOM = purchaseUOM;
                        }
                        dtl.Qty = qty;
                        
                        // Record source SODtlKeys to PODTL UDF
                        // Use UDF property indexer: dtl.UDF["FieldName"] = value
                        dtl.UDF["SourceSODtlKeys"] = item.SourceDtlKeys;
                        
                        totalPODetails++;

                        GeneralLogger_Cls.Write(
                            $"No UOM hierarchy found for {item.ItemCode}, using PurchaseUOM: {qty} {purchaseUOM}", LogLevel.WARNING);
                        continue;
                    }

                    // Split quantity by UOM hierarchy
                    var splitResult = SplitQtyByUOMHierarchy(item.TotalSmallestQty, allUOMs);

                    // Create a PO detail row for each non-zero UOM quantity
                    // For merged items with multiple UOM splits, each PODTL shares the same SourceDtlKeys
                    foreach (var uomQty in splitResult)
                    {
                        string uom = uomQty.Item1;
                        decimal qty = uomQty.Item2;

                        var dtl = po.AddDetail();
                        dtl.ItemCode = item.ItemCode;
                        dtl.UOM = uom;
                        dtl.Qty = qty;
                        
                        // Record source SODtlKeys to PODTL UDF
                        // Use UDF property indexer: dtl.UDF["FieldName"] = value
                        dtl.UDF["SourceSODtlKeys"] = item.SourceDtlKeys;
                        
                        totalPODetails++;
                    }

                    // Log merging info
                    if (item.SourceCount > 1 || splitResult.Count > 1)
                    {
                        string splitInfo = string.Join(" + ", splitResult.Select(s => $"{s.Item2} {s.Item1}"));
                        GeneralLogger_Cls.Write(
                            $"Merged {item.SourceCount} SO items for {item.ItemCode} (SmallestQty: {item.TotalSmallestQty}) → {splitInfo}", LogLevel.INFO);
                    }
                }

                GeneralLogger_Cls.Write(
                    $"Added {totalPODetails} PO details from {soItems.Rows.Count} SO items ({groupedItems.Count} unique items)", LogLevel.SUCCESS);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(AddPODetailsMerged)}()", ex);
                throw;
            }
        }

        /// <summary>
        /// Get Item's PurchaseUOM and its Rate from Item Master
        /// </summary>
        private static void GetItemPurchaseUOMInfo(string itemCode, out string purchaseUOM, out decimal rate)
        {
            purchaseUOM = string.Empty;
            rate = 1m;

            try
            {
                string escapedItemCode = itemCode.Replace("'", "''");
                
                // Get PurchaseUOM from Item table
                object objPurchaseUOM = myDBSetting.ExecuteScalar(
                    $"SELECT PurchaseUOM FROM Item WHERE ItemCode = '{escapedItemCode}'");
                
                if (objPurchaseUOM != null && objPurchaseUOM != DBNull.Value)
                {
                    purchaseUOM = objPurchaseUOM.ToString();
                    string escapedUOM = purchaseUOM.Replace("'", "''");
                    
                    // Get Rate for this UOM from ItemUOM table
                    object objRate = myDBSetting.ExecuteScalar(
                        $"SELECT Rate FROM ItemUOM WHERE ItemCode = '{escapedItemCode}' AND UOM = '{escapedUOM}'");
                    
                    if (objRate != null && objRate != DBNull.Value)
                    {
                        rate = Convert.ToDecimal(objRate);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GetItemPurchaseUOMInfo)}({itemCode})", ex);
            }
        }

        /// <summary>
        /// Get all UOMs for an Item, ordered by Rate DESC (highest first)
        /// Returns list of (UOM, Rate) tuples
        /// </summary>
        private static List<Tuple<string, decimal>> GetItemAllUOMs(string itemCode)
        {
            var result = new List<Tuple<string, decimal>>();

            try
            {
                // Query all UOMs for this item, ordered by Rate DESC
                DataTable dt = myDBSetting.GetDataTable(
                    $"SELECT UOM, Rate FROM ItemUOM WHERE ItemCode = '{itemCode.Replace("'", "''")}' ORDER BY Rate DESC", false);

                foreach (DataRow row in dt.Rows)
                {
                    string uom = row["UOM"].ToString();
                    decimal rate = Convert.ToDecimal(row["Rate"]);
                    result.Add(Tuple.Create(uom, rate));
                }

                // If no UOM found, get BaseUOM from Item table with Rate=1
                if (result.Count == 0)
                {
                    object objBaseUOM = myDBSetting.ExecuteScalar(
                        $"SELECT BaseUOM FROM Item WHERE ItemCode = '{itemCode.Replace("'", "''")}'" );
                    
                    if (objBaseUOM != null && objBaseUOM != DBNull.Value)
                    {
                        result.Add(Tuple.Create(objBaseUOM.ToString(), 1m));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(GetItemAllUOMs)}({itemCode})", ex);
            }

            return result;
        }

        /// <summary>
        /// Split SmallestQty by UOM hierarchy to avoid fractional quantities
        /// Returns list of (UOM, Qty) tuples with non-zero integer quantities
        /// Example: SmallestQty=50, UOMs=[CARTON(48), BOX(12), PCS(1)] → [(CARTON,1), (PCS,2)]
        /// </summary>
        private static List<Tuple<string, decimal>> SplitQtyByUOMHierarchy(decimal smallestQty, List<Tuple<string, decimal>> uomList)
        {
            var result = new List<Tuple<string, decimal>>();
            decimal remainingQty = smallestQty;

            // Process from highest Rate to lowest
            foreach (var uomInfo in uomList)
            {
                string uom = uomInfo.Item1;
                decimal rate = uomInfo.Item2;

                if (rate <= 0) continue;

                // Calculate integer quantity for this UOM
                decimal qty = Math.Floor(remainingQty / rate);

                if (qty > 0)
                {
                    result.Add(Tuple.Create(uom, qty));
                    remainingQty -= qty * rate;
                }

                // If no remainder, we're done
                if (remainingQty <= 0) break;
            }

            // Handle any remaining fractional quantity with the smallest UOM (Rate=1)
            if (remainingQty > 0 && result.Count == 0)
            {
                // If no UOM was used (shouldn't happen), use the smallest UOM
                var smallestUOM = uomList.LastOrDefault();
                if (smallestUOM != null)
                {
                    result.Add(Tuple.Create(smallestUOM.Item1, smallestQty));
                }
            }

            return result;
        }

        /// <summary>
        /// Update SODTL UDF after PO is generated
        /// Sets: HasPO=True, PODocNo, PODocKey, POGeneratedTimestamp, Status
        /// Modified by SCChang's Copilot on 20251209: Use original UOM for RemainQty storage
        /// </summary>
        private static void UpdateSODTLAfterPOGenerated(DataTable soItems, string poDocNo)
        {
            try
            {
                string escapedPoDocNo = poDocNo.Replace("'", "''");
                
                // Get PO DocKey
                object objPODocKey = myDBSetting.ExecuteScalar(
                    $"SELECT DocKey FROM PO WHERE DocNo = '{escapedPoDocNo}'");
                
                if (objPODocKey == null)
                {
                    ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(UpdateSODTLAfterPOGenerated)}()",
                        new Exception($"PO {poDocNo} not found in database."));
                    return;
                }

                long poDocKey = Convert.ToInt64(objPODocKey);

                foreach (DataRow soRow in soItems.Rows)
                {
                    long soDtlKey = Convert.ToInt64(soRow["DtlKey"]);
                    // Use SmallestQty for accurate UOM-converted calculation
                    decimal smallestQtyOrdered = Convert.ToDecimal(soRow["SmallestQty"]);

                    // Update SODTL UDF
                    // RemainQty is stored in original UOM (same as Qty)
                    // Calculation: Convert SmallestQty to original UOM using Rate
                    // Modified by SCChang's Copilot on 20251209: Changed to use original UOM for RemainQty storage
                    // Status logic: 
                    //   Completed = RemainQty <= 0 (all qty has PO)
                    //   Partial = 0 < RemainQty < Qty (some qty has PO)
                    string sql = $@"
                        UPDATE SODTL SET 
                            UDF_HasPO = 'T',
                            UDF_PODocNo = '{escapedPoDocNo}',
                            UDF_PODocKey = {poDocKey},
                            UDF_POGeneratedTimestamp = GETDATE(),
                            UDF_RemainQty = (ISNULL(UDF_RemainQty, Qty) * Rate - {smallestQtyOrdered}) / Rate,
                            UDF_Status = CASE 
                                WHEN ((ISNULL(UDF_RemainQty, Qty) * Rate - {smallestQtyOrdered}) / Rate) <= 0 THEN 'Completed'
                                WHEN ((ISNULL(UDF_RemainQty, Qty) * Rate - {smallestQtyOrdered}) / Rate) < Qty THEN 'Partial'
                                ELSE 'Pending'
                            END
                        WHERE DtlKey = {soDtlKey}";

                    myDBSetting.ExecuteNonQuery(sql);
                }

                GeneralLogger_Cls.Write($"Updated {soItems.Rows.Count} SODTL records for PO {poDocNo}", LogLevel.SUCCESS);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(POGenerationHelper)}.{nameof(UpdateSODTLAfterPOGenerated)}()", ex);
            }
        }
    }
}
