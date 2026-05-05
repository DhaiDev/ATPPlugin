using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;
using ServiceContractPhotocopier.Classes;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.ServiceContract.OperationForms
{
    public partial class ServiceContract_Form : XtraForm
    {
        private enum FormMode { New, Edit, View }

        private DBSetting _dbSetting;
        private long _serviceContractKey = 0;
        private bool _isNew = true;
        private DataTable _dtSpareParts;
        private DataTable _dtServiceSites;
        private DataTable _dtItems;
        private DataTable _dtItemUOMs;
        private DataTable _dtServiceItemsFull;

        // CRUD state machine
        private FormMode _mode = FormMode.New;
        private bool _isDirty = false;
        private DateTime? _rowVersion;
        private string _currentUserCode = "ADMIN";
        private bool _suppressDirty = false;

        public ServiceContract_Form()
        {
            if (string.IsNullOrEmpty(DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName)
                || DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName == "DevExpress Style")
            {
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("WXI");
            }
            InitializeComponent();
        }

        public ServiceContract_Form(UserSession userSession) : this()
        {
            if (userSession != null) _dbSetting = userSession.DBSetting;
            this.Load += new EventHandler(OnFormLoad);
        }

        public ServiceContract_Form(DBSetting dbSetting) : this()
        {
            _dbSetting = dbSetting;
            this.Load += new EventHandler(OnFormLoad);
        }

        public ServiceContract_Form(DBSetting dbSetting, DataRow existing) : this()
        {
            _dbSetting = dbSetting;
            if (existing != null && existing.Table.Columns.Contains("ServiceContractKey"))
            {
                _serviceContractKey = Convert.ToInt64(existing["ServiceContractKey"]);
                _isNew = false;
            }
            this.Load += new EventHandler(OnFormLoad);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_dbSetting == null) return;
            _suppressDirty = true;
            try
            {
                _currentUserCode = ResolveCurrentUserCode();
                LoadLookups();
                _dtSpareParts = NewSparePartsTable();
                _dtServiceSites = NewServiceSitesTable();
                GridSpareParts.DataSource = _dtSpareParts;
                GridServiceSites.DataSource = _dtServiceSites;
                GridViewSP.CellValueChanged += GridViewSP_CellValueChanged;
                GridViewSV.CellValueChanged += GridViewSV_CellValueChanged;
                GridViewSP.InitNewRow += GridView_InitNewRow_SP;
                GridViewSV.InitNewRow += GridView_InitNewRow_SV;

                if (!_isNew)
                {
                    LoadExisting();
                    _mode = FormMode.View;
                }
                else
                {
                    TxtContractNo.Text = "";
                    DtContractDate.DateTime = DateTime.Today;
                    DtServiceStartDate.DateTime = DateTime.Today;
                    DtServiceEndDate.DateTime = DateTime.Today.AddYears(1);
                    SpnContractAmount.Value = 0;
                    TxtDescription.Text = "Service Contract";
                    ChkInactive.Checked = false;
                    LblStatus.Text = "Status:  New (unsaved)";
                    LblStatus.Appearance.ForeColor = System.Drawing.Color.DarkBlue;
                    AutoPickContractCode();
                    _mode = FormMode.New;
                }

                UpdateStatusLabel();
                WireDirtyTracking();
                ApplyMode();
            }
            finally
            {
                _suppressDirty = false;
                _isDirty = false;
            }
        }

        private string ResolveCurrentUserCode()
        {
            try
            {
                AutoCount.Authentication.UserSession s = AutoCount.Authentication.UserSession.CurrentUserSession;
                if (s != null && !string.IsNullOrEmpty(s.LoginUserID)) return s.LoginUserID;
            }
            catch { }
            return "ADMIN";
        }

        private void AutoPickContractCode()
        {
            try
            {
                const string prefix = "SC-";
                const int width = 6;
                string sql =
                    "SELECT ISNULL(MAX(CAST(SUBSTRING(ServiceContractCode, " + (prefix.Length + 1) + ", " + width + ") AS INT)), 0) + 1 " +
                    "FROM [dbo].[zSCP_ServiceContract] " +
                    "WHERE ServiceContractCode LIKE '" + prefix + "%' AND LEN(ServiceContractCode) = " + (prefix.Length + width);
                object scalar = _dbSetting.ExecuteScalar(sql);
                int next = scalar == null || scalar == DBNull.Value ? 1 : Convert.ToInt32(scalar);
                TxtContractNo.Text = prefix + next.ToString(new string('0', width));
            }
            catch { }
        }

        private void LoadLookups()
        {
            try
            {
                // Contract Type: Code + Description (SearchLookUpEdit)
                DataTable dtTypes = _dbSetting.GetDataTable(
                    "SELECT ServiceContractTypeCode, [Description] FROM [dbo].[zSCP_LK_ServiceContractType] WHERE Inactive='N' ORDER BY ServiceContractTypeCode", false);
                CmbContractType.Properties.DataSource = dtTypes;
                CmbContractType.Properties.DisplayMember = "ServiceContractTypeCode";
                CmbContractType.Properties.ValueMember = "ServiceContractTypeCode";
                CmbContractType.Properties.PopupFormWidth = 400;
                EnsureDropdownButton(CmbContractType);
                DevExpress.XtraGrid.Views.Grid.GridView vct = CmbContractType.Properties.View;
                vct.Columns.Clear(); vct.OptionsView.ShowGroupPanel = false;
                vct.Columns.AddField("ServiceContractTypeCode").VisibleIndex = 0;
                vct.Columns.AddField("Description").VisibleIndex = 1;

                // Debtor
                try
                {
                    DataTable dtDeb = _dbSetting.GetDataTable(
                        "SELECT AccNo AS DebtorCode, CompanyName FROM [dbo].[Debtor] ORDER BY AccNo", false);
                    LkDebtorCode.Properties.DataSource = dtDeb;
                    LkDebtorCode.Properties.DisplayMember = "DebtorCode";
                    LkDebtorCode.Properties.ValueMember = "DebtorCode";
                    LkDebtorCode.Properties.PopupFormWidth = 520;
                    EnsureDropdownButton(LkDebtorCode);
                    DevExpress.XtraGrid.Views.Grid.GridView v = LkDebtorCode.Properties.View;
                    v.Columns.Clear(); v.OptionsView.ShowGroupPanel = false;
                    v.Columns.AddField("DebtorCode").VisibleIndex = 0;
                    v.Columns.AddField("CompanyName").VisibleIndex = 1;
                    LkDebtorCode.EditValueChanged += LkDebtorCode_EditValueChanged;
                }
                catch (Exception exd) { System.Diagnostics.Debug.WriteLine("Debtor load: " + exd.Message); }

                // Agent (SalesAgent)
                try
                {
                    DataTable dtAg = _dbSetting.GetDataTable("SELECT SalesAgent, [Description] FROM [dbo].[SalesAgent] WHERE IsActive='Y' OR IsActive IS NULL ORDER BY SalesAgent", false);
                    LkAgentCode.Properties.DataSource = dtAg;
                    LkAgentCode.Properties.DisplayMember = "SalesAgent";
                    LkAgentCode.Properties.ValueMember = "SalesAgent";
                    LkAgentCode.Properties.PopupFormWidth = 420;
                    EnsureDropdownButton(LkAgentCode);
                    DevExpress.XtraGrid.Views.Grid.GridView v = LkAgentCode.Properties.View;
                    v.Columns.Clear(); v.OptionsView.ShowGroupPanel = false;
                    v.Columns.AddField("SalesAgent").VisibleIndex = 0;
                    v.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch { }

                // Term (AutoCount [Terms] table)
                try
                {
                    DataTable dtTerm = _dbSetting.GetDataTable(
                        "SELECT Terms, ISNULL(DisplayTerm, Terms) AS DisplayTerm, ISNULL(TermType,'') AS TermType, ISNULL(TermDays,0) AS TermDays FROM [dbo].[Terms] ORDER BY Terms", false);
                    TxtTerm.Properties.DataSource = dtTerm;
                    TxtTerm.Properties.DisplayMember = "Terms";
                    TxtTerm.Properties.ValueMember = "Terms";
                    TxtTerm.Properties.PopupFormWidth = 460;
                    EnsureDropdownButton(TxtTerm);
                    DevExpress.XtraGrid.Views.Grid.GridView vTerm = TxtTerm.Properties.View;
                    vTerm.Columns.Clear(); vTerm.OptionsView.ShowGroupPanel = false;
                    vTerm.Columns.AddField("Terms").VisibleIndex = 0;
                    vTerm.Columns.AddField("DisplayTerm").VisibleIndex = 1;
                    vTerm.Columns.AddField("TermType").VisibleIndex = 2;
                    vTerm.Columns.AddField("TermDays").VisibleIndex = 3;
                }
                catch (Exception ext) { System.Diagnostics.Debug.WriteLine("Terms load: " + ext.Message); }

                // Area (AutoCount [Area] table)
                try
                {
                    DataTable dtArea = _dbSetting.GetDataTable(
                        "SELECT AreaCode, ISNULL([Description],'') AS [Description] FROM [dbo].[Area] ORDER BY AreaCode", false);
                    TxtArea.Properties.DataSource = dtArea;
                    TxtArea.Properties.DisplayMember = "AreaCode";
                    TxtArea.Properties.ValueMember = "AreaCode";
                    TxtArea.Properties.PopupFormWidth = 360;
                    EnsureDropdownButton(TxtArea);
                    DevExpress.XtraGrid.Views.Grid.GridView vArea = TxtArea.Properties.View;
                    vArea.Columns.Clear(); vArea.OptionsView.ShowGroupPanel = false;
                    vArea.Columns.AddField("AreaCode").VisibleIndex = 0;
                    vArea.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch (Exception exa) { System.Diagnostics.Debug.WriteLine("Area load: " + exa.Message); }

                // Items for spare parts grid — AutoCount 2.x: Item table, BaseUOM (not UOM), IsActive T/F.
                try
                {
                    _dtItems = _dbSetting.GetDataTable(
                        "SELECT ItemCode, [Description], ISNULL(BaseUOM,'') AS UOM " +
                        "FROM [dbo].[Item] WHERE IsActive='T' ORDER BY ItemCode", false);
                    RepoItemCode.DataSource = _dtItems;
                }
                catch (Exception exi)
                {
                    _dtItems = new DataTable();
                    System.Diagnostics.Debug.WriteLine("Item load: " + exi.Message);
                }

                // Per-item UOMs — used to filter the UOM dropdown in the spare parts grid.
                try
                {
                    _dtItemUOMs = _dbSetting.GetDataTable(
                        "SELECT ItemCode, UOM, Rate FROM [dbo].[ItemUOM] ORDER BY ItemCode, Rate", false);
                }
                catch (Exception exu)
                {
                    _dtItemUOMs = new DataTable();
                    _dtItemUOMs.Columns.Add("ItemCode", typeof(string));
                    _dtItemUOMs.Columns.Add("UOM", typeof(string));
                    _dtItemUOMs.Columns.Add("Rate", typeof(decimal));
                    System.Diagnostics.Debug.WriteLine("ItemUOM load: " + exu.Message);
                }
                GridViewSP.ShowingEditor += GridViewSP_ShowingEditor;

                // Service Items for SV grid — full detail (Service Tag, Stock, Grade, Price, Dept, Loc, Ref).
                // Joined with Item table for StockName.
                try
                {
                    _dtServiceItemsFull = _dbSetting.GetDataTable(
                        "SELECT si.ServiceItemCode, ISNULL(si.StockCode,'') AS StockCode, " +
                        "ISNULL(it.[Description],'') AS StockName, " +
                        "ISNULL(si.ServiceItemGradeCode,'') AS GradeCode, " +
                        "ISNULL(si.UnitPrice, 0) AS UnitPrice, " +
                        "ISNULL(si.DepartmentCode,'') AS DepartmentCode, " +
                        "ISNULL(si.StockLocationCode,'') AS StockLocationCode, " +
                        "ISNULL(si.RefNo,'') AS RefNo " +
                        "FROM [dbo].[zSCP_ServiceItem] si " +
                        "LEFT JOIN [dbo].[Item] it ON it.ItemCode = si.StockCode " +
                        "WHERE si.Inactive='N' " +
                        "ORDER BY si.ServiceItemCode", false);
                    RepoServiceItemCode.DataSource = _dtServiceItemsFull;
                    RepoServiceItemCode.DisplayMember = "ServiceItemCode";
                    RepoServiceItemCode.ValueMember = "ServiceItemCode";
                    RepoServiceItemCode.PopupFormWidth = 720;
                    RepoServiceItemCode.View.Columns.Clear();
                    RepoServiceItemCode.View.OptionsView.ShowGroupPanel = false;
                    RepoServiceItemCode.View.Columns.AddField("ServiceItemCode").VisibleIndex = 0;
                    RepoServiceItemCode.View.Columns.AddField("StockCode").VisibleIndex = 1;
                    RepoServiceItemCode.View.Columns.AddField("StockName").VisibleIndex = 2;
                    RepoServiceItemCode.View.Columns.AddField("DepartmentCode").VisibleIndex = 3;
                    RepoServiceItemCode.View.Columns.AddField("StockLocationCode").VisibleIndex = 4;
                }
                catch (Exception exsi)
                {
                    _dtServiceItemsFull = new DataTable();
                    System.Diagnostics.Debug.WriteLine("ServiceItem load: " + exsi.Message);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lookup load warning:\r\n" + ex.Message, "Load", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LkDebtorCode_EditValueChanged(object sender, EventArgs e)
        {
            // Optional: could auto-fill Address/Attention from Debtor record
        }

        // Force a visible dropdown (Combo) button on a SearchLookUpEdit so the down-arrow shows
        // even when the parent app's skin/look-and-feel hasn't supplied one.
        private static void EnsureDropdownButton(DevExpress.XtraEditors.SearchLookUpEdit edit)
        {
            edit.Properties.Buttons.Clear();
            edit.Properties.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo));
            edit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            edit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        }

        // Auto-generate next Contract No in the register format "SC-000000" (6-digit running).
        private void TxtContractNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (_dbSetting == null) return;
            try
            {
                const string prefix = "SC-";
                const int width = 6;
                string sql =
                    "SELECT ISNULL(MAX(CAST(SUBSTRING(ServiceContractCode, " + (prefix.Length + 1) + ", " + width + ") AS INT)), 0) + 1 " +
                    "FROM [dbo].[zSCP_ServiceContract] " +
                    "WHERE ServiceContractCode LIKE '" + prefix + REPLICATE_DIGITS(width) + "'";
                object scalar = _dbSetting.ExecuteScalar(sql);
                int next = scalar == null || scalar == DBNull.Value ? 1 : Convert.ToInt32(scalar);
                TxtContractNo.Text = prefix + next.ToString(new string('0', width));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Auto-generate failed:\r\n" + ex.Message, "Contract No", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static string REPLICATE_DIGITS(int n)
        {
            string s = "";
            for (int i = 0; i < n; i++) s += "[0-9]";
            return s;
        }

        private void GridViewSP_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow row = GridViewSP.GetDataRow(e.RowHandle);
            if (row == null) return;
            if (e.Column.FieldName == "ItemCode" && _dtItems != null && _dtItems.Columns.Contains("ItemCode"))
            {
                DataRow[] found = _dtItems.Select("ItemCode = '" + (e.Value ?? "").ToString().Replace("'", "''") + "'");
                if (found.Length > 0)
                {
                    row["Description"] = found[0]["Description"];
                    if (_dtItems.Columns.Contains("UOM")) row["UOM"] = found[0]["UOM"]; // default to BaseUOM
                }
            }
            if (e.Column.FieldName == "Quantity" || e.Column.FieldName == "UnitPrice"
                || e.Column.FieldName == "TaxPercent" || e.Column.FieldName == "TaxInclusive")
            {
                decimal q = row["Quantity"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Quantity"]);
                decimal up = row["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["UnitPrice"]);
                decimal amt = q * up;
                row["Amount"] = amt;
                decimal pct = row["TaxPercent"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TaxPercent"]);
                decimal tax = Math.Round(amt * pct / 100m, 2);
                row["TaxAmount"] = tax;
                bool incl = row["TaxInclusive"] != DBNull.Value && Convert.ToBoolean(row["TaxInclusive"]);
                row["AmountAfterTax"] = incl ? amt : amt + tax;
            }
        }

        // When user picks a Service Tag in the SV grid, look up the full service-item record
        // and auto-fill Stock Code / Stock Name / Grade / Unit Price / Department / Location / Ref No.
        private void GridViewSV_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName != "ServiceItemCode") return;
            DataRow row = GridViewSV.GetDataRow(e.RowHandle);
            if (row == null || _dtServiceItemsFull == null) return;
            string code = (e.Value ?? "").ToString().Replace("'", "''");
            DataRow[] found = _dtServiceItemsFull.Select("ServiceItemCode = '" + code + "'");
            if (found.Length == 0) return;
            DataRow src = found[0];
            row["StockCode"] = src["StockCode"];
            row["StockName"] = src["StockName"];
            row["GradeCode"] = src["GradeCode"];
            row["UnitPrice"] = src["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(src["UnitPrice"]);
            row["DepartmentCode"] = src["DepartmentCode"];
            row["StockLocationCode"] = src["StockLocationCode"];
            row["RefNo"] = src["RefNo"];
        }

        // Filter the UOM dropdown to only the UOMs defined for the current row's ItemCode.
        // When no ItemCode yet, show nothing (the user must pick an item first).
        private void GridViewSP_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (GridViewSP.FocusedColumn == null || GridViewSP.FocusedColumn.FieldName != "UOM") return;
            DataRow row = GridViewSP.GetFocusedDataRow();
            string itemCode = (row != null && row["ItemCode"] != DBNull.Value) ? row["ItemCode"].ToString() : "";
            DataView view = new DataView(_dtItemUOMs ?? new DataTable());
            view.RowFilter = string.IsNullOrEmpty(itemCode)
                ? "1 = 0"
                : "ItemCode = '" + itemCode.Replace("'", "''") + "'";
            RepoUOM.DataSource = view;
            RepoUOM.DisplayMember = "UOM";
            RepoUOM.ValueMember = "UOM";
        }

        private DataTable NewSparePartsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("ItemCode", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Unlimited", typeof(bool));
            dt.Columns.Add("UOM", typeof(string));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("Discount", typeof(string));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("TaxType", typeof(string));
            dt.Columns.Add("TaxInclusive", typeof(bool));
            dt.Columns.Add("TaxPercent", typeof(decimal));
            dt.Columns.Add("TaxAmount", typeof(decimal));
            dt.Columns.Add("AmountAfterTax", typeof(decimal));
            return dt;
        }

        private DataTable NewServiceSitesTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("ServiceItemCode", typeof(string));
            dt.Columns.Add("StockCode", typeof(string));
            dt.Columns.Add("StockName", typeof(string));
            dt.Columns.Add("GradeCode", typeof(string));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("DepartmentCode", typeof(string));
            dt.Columns.Add("StockLocationCode", typeof(string));
            dt.Columns.Add("RefNo", typeof(string));
            return dt;
        }

        private void LoadExisting()
        {
            try
            {
                DataTable dt = _dbSetting.GetDataTable("SELECT * FROM [dbo].[zSCP_ServiceContract] WHERE ServiceContractKey = " + _serviceContractKey, false);
                if (dt.Rows.Count == 0) { _isNew = true; return; }
                DataRow r = dt.Rows[0];
                _rowVersion = r["LastModified"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["LastModified"]);
                TxtContractNo.Text = S(r, "ServiceContractCode");
                CmbContractType.EditValue = S(r, "ServiceContractTypeCode");
                if (r["ServiceContractDate"] != DBNull.Value) DtContractDate.DateTime = Convert.ToDateTime(r["ServiceContractDate"]);
                if (r["ServiceStartDate"] != DBNull.Value) DtServiceStartDate.DateTime = Convert.ToDateTime(r["ServiceStartDate"]);
                if (r["ServiceExpiryDate"] != DBNull.Value) DtServiceEndDate.DateTime = Convert.ToDateTime(r["ServiceExpiryDate"]);
                LkDebtorCode.EditValue = S(r, "DebtorCode");
                LkAgentCode.EditValue = S(r, "StaffCode");
                TxtDescription.Text = S(r, "Description");
                SpnContractAmount.Value = r["ServiceContractValue"] == DBNull.Value ? 0m : Convert.ToDecimal(r["ServiceContractValue"]);
                TxtAddress.Text = S(r, "Address1") + "\r\n" + S(r, "Address2") + "\r\n" + S(r, "Address3") + "\r\n" + S(r, "Address4");
                TxtAttention.Text = S(r, "Attention");
                TxtNote.Text = r["Note"] == DBNull.Value ? "" : r["Note"].ToString();
                TxtRemark1.Text = S(r, "Remark1");
                TxtRemark2.Text = S(r, "Remark2");
                TxtRemark3.Text = S(r, "Remark3");
                TxtRemark4.Text = S(r, "Remark4");
                ChkInactive.Checked = S(r, "Inactive") == "Y";

                DataTable dtSP = _dbSetting.GetDataTable("SELECT Pos, ItemStockCode, [Description], Unlimited, UOM, Qty, DiscountFormat, UnitPrice, Amount, GstTypeCode, TaxSubtract, SalesTaxPercentage, SalesTaxAmount, AmountAfterTax FROM [dbo].[zSCP_ServiceContractDTL] WHERE ServiceContractKey = " + _serviceContractKey + " ORDER BY Pos", false);
                _dtSpareParts.Clear();
                int n = 1;
                foreach (DataRow row in dtSP.Rows)
                {
                    DataRow nr = _dtSpareParts.NewRow();
                    nr["No"] = n++;
                    nr["ItemCode"] = row["ItemStockCode"];
                    nr["Description"] = row["Description"];
                    nr["Unlimited"] = (row["Unlimited"] != DBNull.Value && row["Unlimited"].ToString() == "Y");
                    nr["UOM"] = row["UOM"];
                    nr["Quantity"] = row["Qty"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Qty"]);
                    nr["Discount"] = row["DiscountFormat"];
                    nr["UnitPrice"] = row["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["UnitPrice"]);
                    nr["Amount"] = row["Amount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Amount"]);
                    nr["TaxType"] = row["GstTypeCode"] == DBNull.Value ? "" : row["GstTypeCode"].ToString();
                    nr["TaxInclusive"] = (row["TaxSubtract"] != DBNull.Value && row["TaxSubtract"].ToString() == "Y");
                    nr["TaxPercent"] = row["SalesTaxPercentage"] == DBNull.Value ? 0m : Convert.ToDecimal(row["SalesTaxPercentage"]);
                    nr["TaxAmount"] = row["SalesTaxAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["SalesTaxAmount"]);
                    nr["AmountAfterTax"] = row["AmountAfterTax"] == DBNull.Value ? 0m : Convert.ToDecimal(row["AmountAfterTax"]);
                    _dtSpareParts.Rows.Add(nr);
                }

                DataTable dtSV = _dbSetting.GetDataTable("SELECT Pos, ServiceItemCode FROM [dbo].[zSCP_ServiceContractSVI] WHERE ServiceContractKey = " + _serviceContractKey + " ORDER BY Pos", false);
                _dtServiceSites.Clear();
                n = 1;
                foreach (DataRow row in dtSV.Rows)
                {
                    DataRow nr = _dtServiceSites.NewRow();
                    nr["No"] = n++;
                    string siCode = row["ServiceItemCode"] == DBNull.Value ? "" : row["ServiceItemCode"].ToString();
                    nr["ServiceItemCode"] = siCode;
                    if (_dtServiceItemsFull != null && !string.IsNullOrEmpty(siCode))
                    {
                        DataRow[] hit = _dtServiceItemsFull.Select("ServiceItemCode = '" + siCode.Replace("'", "''") + "'");
                        if (hit.Length > 0)
                        {
                            nr["StockCode"] = hit[0]["StockCode"];
                            nr["StockName"] = hit[0]["StockName"];
                            nr["GradeCode"] = hit[0]["GradeCode"];
                            nr["UnitPrice"] = hit[0]["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(hit[0]["UnitPrice"]);
                            nr["DepartmentCode"] = hit[0]["DepartmentCode"];
                            nr["StockLocationCode"] = hit[0]["StockLocationCode"];
                            nr["RefNo"] = hit[0]["RefNo"];
                        }
                    }
                    _dtServiceSites.Rows.Add(nr);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Failed to load contract:\r\n" + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatusLabel()
        {
            if (DtServiceEndDate.DateTime == DateTime.MinValue) { LblStatus.Text = "Status:  No Stated"; return; }
            int days = (DateTime.Today - DtServiceEndDate.DateTime).Days;
            if (DateTime.Today > DtServiceEndDate.DateTime)
            {
                LblStatus.Text = "Status:  Service Expired (" + days + " days)";
                LblStatus.Appearance.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                LblStatus.Text = "Status:  Under Service (" + Math.Abs(days) + " days remaining)";
                LblStatus.Appearance.ForeColor = System.Drawing.Color.Green;
            }
        }

        private static string S(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col)) return "";
            return r[col] == DBNull.Value ? "" : r[col].ToString();
        }
        private static string EV(BaseEdit b) { return b.EditValue == null ? "" : b.EditValue.ToString(); }

        private void OnSave(object sender, EventArgs e)
        {
            if (!ValidateForSave()) return;

            try
            {
                GridViewSP.CloseEditor(); GridViewSP.UpdateCurrentRow();
                GridViewSV.CloseEditor(); GridViewSV.UpdateCurrentRow();

                string code = SQLString(TxtContractNo.Text.Trim());
                string type = SQLString(EV(CmbContractType));
                string debtor = SQLString(EV(LkDebtorCode));
                string agent = SQLString(EV(LkAgentCode));
                string desc = SQLString(TxtDescription.Text ?? "");
                string attn = SQLString(TxtAttention.Text ?? "");
                string note = SQLString(TxtNote.Text ?? "");
                string r1 = SQLString(TxtRemark1.Text ?? ""), r2 = SQLString(TxtRemark2.Text ?? ""), r3 = SQLString(TxtRemark3.Text ?? ""), r4 = SQLString(TxtRemark4.Text ?? "");
                decimal val = SpnContractAmount.Value;

                string[] addr = (TxtAddress.Text ?? "").Replace("\r\n", "\n").Split('\n');
                string a1 = SQLString(addr.Length > 0 ? addr[0] : "");
                string a2 = SQLString(addr.Length > 1 ? addr[1] : "");
                string a3 = SQLString(addr.Length > 2 ? addr[2] : "");
                string a4 = SQLString(addr.Length > 3 ? addr[3] : "");

                string inac = ChkInactive.Checked ? "Y" : "N";
                string dtCon = "'" + DtContractDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtStart = "'" + DtServiceStartDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtEnd = "'" + DtServiceEndDate.DateTime.ToString("yyyy-MM-dd") + "'";

                string user = SQLString(_currentUserCode ?? "ADMIN");

                using (SqlConnection cn = new SqlConnection(_dbSetting.ConnectionString))
                {
                    cn.Open();
                    using (SqlTransaction tx = cn.BeginTransaction("ScpContractSave"))
                    {
                        try
                        {
                            string sql;
                            if (_mode == FormMode.New)
                            {
                                sql = "INSERT INTO [dbo].[zSCP_ServiceContract] " +
                                    "(ServiceContractCode, ServiceContractTypeCode, ServiceContractDate, ServiceContractValue, [Description], " +
                                    " ServiceStartDate, ServiceExpiryDate, DebtorCode, StaffCode, Address1, Address2, Address3, Address4, Attention, " +
                                    " Note, Remark1, Remark2, Remark3, Remark4, Inactive, Created, Modified, CreatedBy, ModifiedBy) VALUES " +
                                    "(N'" + code + "', N'" + type + "', " + dtCon + ", " + val.ToString("0.00") + ", N'" + desc + "', " +
                                    dtStart + ", " + dtEnd + ", N'" + debtor + "', N'" + agent + "', " +
                                    "N'" + a1 + "', N'" + a2 + "', N'" + a3 + "', N'" + a4 + "', N'" + attn + "', " +
                                    "N'" + note + "', N'" + r1 + "', N'" + r2 + "', N'" + r3 + "', N'" + r4 + "', '" + inac + "', " +
                                    "GETDATE(), GETDATE(), N'" + user + "', N'" + user + "'); " +
                                    "SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
                                using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                                {
                                    object obj = cmd.ExecuteScalar();
                                    _serviceContractKey = Convert.ToInt64(obj);
                                }
                            }
                            else
                            {
                                string verPredicate = _rowVersion.HasValue
                                    ? " AND LastModified = '" + _rowVersion.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"
                                    : "";
                                sql = "UPDATE [dbo].[zSCP_ServiceContract] SET " +
                                    "ServiceContractTypeCode=N'" + type + "', ServiceContractDate=" + dtCon + ", " +
                                    "ServiceContractValue=" + val.ToString("0.00") + ", [Description]=N'" + desc + "', " +
                                    "ServiceStartDate=" + dtStart + ", ServiceExpiryDate=" + dtEnd + ", DebtorCode=N'" + debtor + "', StaffCode=N'" + agent + "', " +
                                    "Address1=N'" + a1 + "', Address2=N'" + a2 + "', Address3=N'" + a3 + "', Address4=N'" + a4 + "', Attention=N'" + attn + "', " +
                                    "Note=N'" + note + "', Remark1=N'" + r1 + "', Remark2=N'" + r2 + "', Remark3=N'" + r3 + "', Remark4=N'" + r4 + "', " +
                                    "Inactive='" + inac + "', Modified=GETDATE(), LastModified=GETDATE(), ModifiedBy=N'" + user + "' " +
                                    "WHERE ServiceContractKey=" + _serviceContractKey + verPredicate;
                                using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                                {
                                    int rows = cmd.ExecuteNonQuery();
                                    if (rows == 0)
                                        throw new Exception("This Contract was modified or deleted by another user. Click OK and reopen the record to see the latest values.");
                                }
                            }

                            using (SqlCommand del1 = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceContractDTL] WHERE ServiceContractKey=" + _serviceContractKey, cn, tx))
                                del1.ExecuteNonQuery();

                            int pos = 1;
                            foreach (DataRow row in _dtSpareParts.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted) continue;
                                string itemCode = SQLString(row["ItemCode"] == DBNull.Value ? "" : row["ItemCode"].ToString());
                                if (string.IsNullOrEmpty(itemCode)) continue;
                                string itemDesc = SQLString(row["Description"] == DBNull.Value ? "" : row["Description"].ToString());
                                string unl = (row["Unlimited"] != DBNull.Value && Convert.ToBoolean(row["Unlimited"])) ? "Y" : "N";
                                string uom = SQLString(row["UOM"] == DBNull.Value ? "" : row["UOM"].ToString());
                                decimal q = row["Quantity"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Quantity"]);
                                string disc = SQLString(row["Discount"] == DBNull.Value ? "" : row["Discount"].ToString());
                                decimal up = row["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["UnitPrice"]);
                                decimal amt = row["Amount"] == DBNull.Value ? (q * up) : Convert.ToDecimal(row["Amount"]);
                                string taxType = SQLString(row["TaxType"] == DBNull.Value ? "" : row["TaxType"].ToString());
                                string taxIncl = (row["TaxInclusive"] != DBNull.Value && Convert.ToBoolean(row["TaxInclusive"])) ? "Y" : "N";
                                decimal taxPct = row["TaxPercent"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TaxPercent"]);
                                decimal taxAmt = row["TaxAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TaxAmount"]);
                                decimal amtAfter = row["AmountAfterTax"] == DBNull.Value ? amt : Convert.ToDecimal(row["AmountAfterTax"]);
                                using (SqlCommand ins = new SqlCommand(
                                    "INSERT INTO [dbo].[zSCP_ServiceContractDTL] " +
                                    "(ServiceContractKey, ServiceContractCode, Pos, ItemStockCode, [Description], Unlimited, UOM, Qty, DiscountFormat, UnitPrice, Amount, GstTypeCode, TaxSubtract, SalesTaxPercentage, SalesTaxAmount, AmountAfterTax) VALUES (" +
                                    _serviceContractKey + ", N'" + code + "', " + pos + ", N'" + itemCode + "', N'" + itemDesc + "', '" + unl + "', N'" + uom + "', " +
                                    q.ToString("0.000000") + ", N'" + disc + "', " + up.ToString("0.000000") + ", " + amt.ToString("0.00") + ", " +
                                    "N'" + taxType + "', '" + taxIncl + "', " + taxPct.ToString("0.00") + ", " + taxAmt.ToString("0.00") + ", " + amtAfter.ToString("0.00") + ")",
                                    cn, tx))
                                    ins.ExecuteNonQuery();
                                pos++;
                            }

                            using (SqlCommand del2 = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceContractSVI] WHERE ServiceContractKey=" + _serviceContractKey, cn, tx))
                                del2.ExecuteNonQuery();

                            pos = 1;
                            foreach (DataRow row in _dtServiceSites.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted) continue;
                                string siCode = SQLString(row["ServiceItemCode"] == DBNull.Value ? "" : row["ServiceItemCode"].ToString());
                                if (string.IsNullOrEmpty(siCode)) continue;
                                using (SqlCommand ins = new SqlCommand(
                                    "INSERT INTO [dbo].[zSCP_ServiceContractSVI] " +
                                    "(ServiceContractKey, Pos, ServiceItemCode) VALUES (" + _serviceContractKey + ", " + pos + ", N'" + siCode + "')",
                                    cn, tx))
                                    ins.ExecuteNonQuery();
                                pos++;
                            }

                            tx.Commit();
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { }
                            throw;
                        }
                    }
                }

                _isNew = false;
                _suppressDirty = true;
                try { LoadExisting(); }
                finally { _suppressDirty = false; _isDirty = false; }
                _mode = FormMode.View;
                ApplyMode();
                UpdateStatusLabel();
                XtraMessageBox.Show("Saved.", "Service Contract", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Save failed:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnNew(object sender, EventArgs e)
        {
            if (_isDirty && !ConfirmDiscard()) return;
            ResetFormToBlankNew();
            _mode = FormMode.New;
            ApplyMode();
            UpdateStatusLabel();
            TxtContractNo.Focus();
        }

        private void OnEdit(object sender, EventArgs e)
        {
            if (_mode != FormMode.View || _serviceContractKey <= 0)
            {
                XtraMessageBox.Show("Open or save a record before editing.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _mode = FormMode.Edit;
            ApplyMode();
            CmbContractType.Focus();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            if (_mode != FormMode.View || _serviceContractKey <= 0)
            {
                XtraMessageBox.Show("Open a saved record before deleting.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (XtraMessageBox.Show("Permanently delete this Service Contract and all its line items?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(_dbSetting.ConnectionString))
                {
                    cn.Open();
                    using (SqlTransaction tx = cn.BeginTransaction("ScpContractDelete"))
                    {
                        try
                        {
                            // Children CASCADE on FK, but explicit deletes are safe + fast
                            using (SqlCommand c1 = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceContractDTL] WHERE ServiceContractKey=" + _serviceContractKey, cn, tx)) c1.ExecuteNonQuery();
                            using (SqlCommand c2 = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceContractSVI] WHERE ServiceContractKey=" + _serviceContractKey, cn, tx)) c2.ExecuteNonQuery();
                            string verPredicate = _rowVersion.HasValue
                                ? " AND LastModified = '" + _rowVersion.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"
                                : "";
                            using (SqlCommand c3 = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceContract] WHERE ServiceContractKey=" + _serviceContractKey + verPredicate, cn, tx))
                            {
                                int rows = c3.ExecuteNonQuery();
                                if (rows == 0)
                                    throw new Exception("Record was modified or already deleted by another user.");
                            }
                            tx.Commit();
                        }
                        catch
                        {
                            try { tx.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
                ResetFormToBlankNew();
                _mode = FormMode.New;
                ApplyMode();
                UpdateStatusLabel();
                XtraMessageBox.Show("Deleted.", "Service Contract", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Delete failed:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFormToBlankNew()
        {
            _suppressDirty = true;
            try
            {
                _isNew = true; _serviceContractKey = 0; _rowVersion = null;
                TxtContractNo.Text = ""; CmbContractType.EditValue = null;
                LkDebtorCode.EditValue = null; LkAgentCode.EditValue = null;
                TxtDescription.Text = "Service Contract";
                SpnContractAmount.Value = 0;
                TxtAddress.Text = ""; TxtAttention.Text = "";
                TxtNote.Text = "";
                TxtRemark1.Text = ""; TxtRemark2.Text = ""; TxtRemark3.Text = ""; TxtRemark4.Text = "";
                ChkInactive.Checked = false;
                DtContractDate.DateTime = DateTime.Today;
                DtServiceStartDate.DateTime = DateTime.Today;
                DtServiceEndDate.DateTime = DateTime.Today.AddYears(1);
                _dtSpareParts.Clear(); _dtServiceSites.Clear();
                LblStatus.Text = "Status:  New (unsaved)";
                LblStatus.Appearance.ForeColor = System.Drawing.Color.DarkBlue;
                AutoPickContractCode();
            }
            finally { _suppressDirty = false; _isDirty = false; }
        }

        private bool ValidateForSave()
        {
            if (string.IsNullOrWhiteSpace(TxtContractNo.Text))
            { XtraMessageBox.Show("Contract No is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); TxtContractNo.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(EV(LkDebtorCode)))
            { XtraMessageBox.Show("Debtor Code is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); LkDebtorCode.Focus(); return false; }
            if (DtServiceStartDate.DateTime != DateTime.MinValue && DtServiceEndDate.DateTime != DateTime.MinValue
                && DtServiceEndDate.DateTime < DtServiceStartDate.DateTime)
            { XtraMessageBox.Show("Service End date is earlier than Start date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); DtServiceEndDate.Focus(); return false; }
            return true;
        }

        private bool ConfirmDiscard()
        {
            return XtraMessageBox.Show("Discard unsaved changes?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void ApplyMode()
        {
            bool editable = (_mode != FormMode.View);
            bool isNew = (_mode == FormMode.New);
            foreach (Control c in PanelHeader.Controls)
            {
                if (c is LabelControl) continue;
                c.Enabled = editable;
            }
            TxtContractNo.Enabled = isNew;
            TabMain.Enabled = editable;
            BtnNew.Enabled = (_mode == FormMode.View || _mode == FormMode.New);
            BtnEdit.Enabled = (_mode == FormMode.View && _serviceContractKey > 0);
            BtnSave.Enabled = editable;
            BtnCancel.Enabled = true;
            BtnDelete.Enabled = (_mode == FormMode.View && _serviceContractKey > 0);
            BtnPrint.Enabled = (_mode == FormMode.View && _serviceContractKey > 0);
            BtnSearch.Enabled = (_mode == FormMode.View);
            BtnAttachments.Enabled = (_mode == FormMode.View && _serviceContractKey > 0);
            BtnPreview.Enabled = (_mode == FormMode.New); // wired to OnCopyFrom
            BtnFillTest.Enabled = editable; // dev aid — only useful while mutating
            BtnAutoContractCode.Enabled = isNew; // only assign running no on fresh records
        }

        private void WireDirtyTracking()
        {
            EventHandler markDirty = (s, e) => { if (!_suppressDirty) _isDirty = true; };
            TxtContractNo.EditValueChanged += markDirty;
            CmbContractType.EditValueChanged += markDirty;
            LkDebtorCode.EditValueChanged += markDirty;
            LkAgentCode.EditValueChanged += markDirty;
            TxtDescription.EditValueChanged += markDirty;
            SpnContractAmount.EditValueChanged += markDirty;
            TxtAddress.EditValueChanged += markDirty;
            TxtAttention.EditValueChanged += markDirty;
            TxtNote.EditValueChanged += markDirty;
            TxtRemark1.EditValueChanged += markDirty;
            TxtRemark2.EditValueChanged += markDirty;
            TxtRemark3.EditValueChanged += markDirty;
            TxtRemark4.EditValueChanged += markDirty;
            ChkInactive.CheckedChanged += markDirty;
            DtContractDate.EditValueChanged += markDirty;
            DtServiceStartDate.EditValueChanged += markDirty;
            DtServiceEndDate.EditValueChanged += markDirty;
            _dtSpareParts.RowChanged += (s, e) => { if (!_suppressDirty) _isDirty = true; };
            _dtSpareParts.RowDeleted += (s, e) => { if (!_suppressDirty) _isDirty = true; };
            _dtServiceSites.RowChanged += (s, e) => { if (!_suppressDirty) _isDirty = true; };
            _dtServiceSites.RowDeleted += (s, e) => { if (!_suppressDirty) _isDirty = true; };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_isDirty && (_mode == FormMode.Edit || _mode == FormMode.New))
            {
                DialogResult d = XtraMessageBox.Show(
                    "You have unsaved changes. Save before closing?",
                    "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (d == DialogResult.Cancel) { e.Cancel = true; return; }
                if (d == DialogResult.Yes)
                {
                    OnSave(this, EventArgs.Empty);
                    if (_isDirty) { e.Cancel = true; return; }
                }
            }
            base.OnFormClosing(e);
        }

        // Dev aid — one-click fill-with-test-data -------------------------
        private void OnFillTestData(object sender, EventArgs e)
        {
            if (_mode == FormMode.View)
            {
                XtraMessageBox.Show("Click Add or Edit first — can't fill a view-only record.",
                    "Fill Test Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _suppressDirty = true;
            try
            {
                string stamp = DateTime.Now.ToString("HHmmss");

                if (_mode == FormMode.New) AutoPickContractCode();

                string type    = PickFirst(CmbContractType, "ServiceContractTypeCode");
                string debtor  = PickFirst(LkDebtorCode,    "DebtorCode");
                string agent   = PickFirst(LkAgentCode,     "SalesAgent");
                string term    = PickFirst(TxtTerm,         "Terms");
                string area    = PickFirst(TxtArea,         "AreaCode");

                if (!string.IsNullOrEmpty(type))   CmbContractType.EditValue = type;
                if (!string.IsNullOrEmpty(debtor)) LkDebtorCode.EditValue    = debtor;
                if (!string.IsNullOrEmpty(agent))  LkAgentCode.EditValue     = agent;
                if (!string.IsNullOrEmpty(term))   TxtTerm.EditValue         = term;
                if (!string.IsNullOrEmpty(area))   TxtArea.EditValue         = area;

                DtContractDate.DateTime = DateTime.Today;
                DtServiceStartDate.DateTime = DateTime.Today;
                DtServiceEndDate.DateTime = DateTime.Today.AddYears(1);
                SpnContractAmount.Value = 12000m;
                TxtDescription.Text = "TEST Service Contract (" + stamp + ")";
                TxtAddress.Text = "1 Test Lane\r\nTestville\r\n81200 Johor Bahru\r\nJohor";
                TxtAttention.Text = "Test Contact";
                TxtNote.Text = "Filled automatically at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                TxtRemark1.Text = "Remark 1 - " + stamp;
                TxtRemark2.Text = "Remark 2";
                TxtRemark3.Text = "Remark 3";
                TxtRemark4.Text = "Remark 4";
                ChkInactive.Checked = false;

                // More Header tab (Tab 2 or 3 depending on form)
                TxtMHCity.Text = "Johor Bahru";
                TxtMHPostalCode.Text = "81200";
                CmbMHState.Text = "Johor";
                CmbMHCountry.Text = "Malaysia";
                TxtMHPhone.Text = "07-1234567";
                TxtMHFax.Text = "07-1234568";
                TxtMHRef1.Text = "Ref1-" + stamp;
                TxtMHRef2.Text = "Ref2";
                TxtMHRef3.Text = "Ref3";
                TxtMHRef4.Text = "Ref4";
                TxtMHBranchCode.Text = "HQ";
                TxtMHBranchName.Text = "Headquarter";
                TxtMHAddress.Text = "1 Delivery Lane\r\nTestville\r\n81200 Johor Bahru\r\nJohor";
                TxtMHDOCity.Text = "Johor Bahru";
                TxtMHDOPostalCode.Text = "81200";
                CmbMHDOState.Text = "Johor";
                CmbMHDOCountry.Text = "Malaysia";
                TxtMHDOPhone.Text = "07-9999999";
                TxtMHDOFax.Text = "07-9999998";
                TxtMHDOEmail.Text = "test@example.com";
                TxtMHDOContactPerson.Text = "Test";

                // Tab "3. Service Items" — seed one row from _dtServiceItemsFull (if any)
                GridViewSV.CloseEditor();
                if (_dtServiceSites.Rows.Count == 0 && _dtServiceItemsFull != null && _dtServiceItemsFull.Rows.Count > 0)
                {
                    DataRow src = _dtServiceItemsFull.Rows[0];
                    DataRow nr = _dtServiceSites.NewRow();
                    nr["No"] = _dtServiceSites.Rows.Count + 1;
                    nr["ServiceItemCode"] = src["ServiceItemCode"] ?? "";
                    if (_dtServiceSites.Columns.Contains("StockCode"))         nr["StockCode"] = src["StockCode"] ?? "";
                    if (_dtServiceSites.Columns.Contains("StockName"))         nr["StockName"] = src["StockName"] ?? "";
                    if (_dtServiceSites.Columns.Contains("GradeCode"))         nr["GradeCode"] = src["GradeCode"] ?? "";
                    if (_dtServiceSites.Columns.Contains("UnitPrice"))         nr["UnitPrice"] = src["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(src["UnitPrice"]);
                    if (_dtServiceSites.Columns.Contains("DepartmentCode"))    nr["DepartmentCode"] = src["DepartmentCode"] ?? "";
                    if (_dtServiceSites.Columns.Contains("StockLocationCode")) nr["StockLocationCode"] = src["StockLocationCode"] ?? "";
                    if (_dtServiceSites.Columns.Contains("RefNo"))             nr["RefNo"] = src["RefNo"] ?? "";
                    _dtServiceSites.Rows.Add(nr);
                }

                // Tab "4. Spare Parts" — seed one row from _dtItems (first active stock item)
                GridViewSP.CloseEditor();
                if (_dtSpareParts.Rows.Count == 0 && _dtItems != null && _dtItems.Rows.Count > 0)
                {
                    DataRow src = _dtItems.Rows[0];
                    decimal qty = 1m;
                    decimal up = 100m; // nominal; real price would need ItemPrice lookup
                    DataRow nr = _dtSpareParts.NewRow();
                    nr["No"] = _dtSpareParts.Rows.Count + 1;
                    nr["ItemCode"] = src["ItemCode"] ?? "";
                    nr["Description"] = src.Table.Columns.Contains("Description") && src["Description"] != DBNull.Value ? src["Description"] : "";
                    nr["Unlimited"] = false;
                    nr["UOM"] = src.Table.Columns.Contains("UOM") && src["UOM"] != DBNull.Value ? src["UOM"] : "";
                    nr["Quantity"] = qty;
                    nr["Discount"] = "";
                    nr["UnitPrice"] = up;
                    nr["Amount"] = qty * up;
                    nr["TaxType"] = "";
                    nr["TaxInclusive"] = false;
                    nr["TaxPercent"] = 0m;
                    nr["TaxAmount"] = 0m;
                    nr["AmountAfterTax"] = qty * up;
                    _dtSpareParts.Rows.Add(nr);
                }
            }
            finally { _suppressDirty = false; _isDirty = true; }
        }

        // Dedicated Auto (F12) button next to Contract No — mirrors Service Item's Auto Tag.
        private void OnAutoContractCode(object sender, EventArgs e)
        {
            if (_mode == FormMode.View)
            {
                XtraMessageBox.Show("Switch to Add/Edit mode first.", "Auto Contract No",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AutoPickContractCode();
            _isDirty = true;
        }

        private static string PickFirst(DevExpress.XtraEditors.SearchLookUpEdit edit, string col)
        {
            DataTable dt = edit.Properties.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0 || !dt.Columns.Contains(col)) return "";
            return dt.Rows[0][col] == DBNull.Value ? "" : dt.Rows[0][col].ToString();
        }

        private void OnCopyFrom(object sender, EventArgs e) { XtraMessageBox.Show("Copy From... not implemented yet.", "Copy From"); }
        private void OnPrint(object sender, EventArgs e) { XtraMessageBox.Show("Print not implemented yet.", "Print"); }
        private void OnSearch(object sender, EventArgs e) { XtraMessageBox.Show("Open Inquiry form to search.", "Search"); }
        private void OnAttachments(object sender, EventArgs e) { XtraMessageBox.Show("Attachments dialog not implemented yet.", "Attachments"); }
        private void OnMHSearch(object sender, EventArgs e) { XtraMessageBox.Show("Branch search not implemented yet.", "Search Branch"); }
        private void OnMHCopy(object sender, EventArgs e)
        {
            // Copy main address into delivery address fields
            TxtMHAddress.Text = TxtAddress.Text;
            TxtMHDOCity.Text = TxtMHCity.Text;
            TxtMHDOPostalCode.Text = TxtMHPostalCode.Text;
            CmbMHDOState.Text = CmbMHState.Text;
            CmbMHDOCountry.Text = CmbMHCountry.Text;
            TxtMHDOPhone.Text = TxtMHPhone.Text;
            TxtMHDOFax.Text = TxtMHFax.Text;
        }

        private void OnSPAdd(object sender, EventArgs e)
        {
            GridViewSP.CloseEditor(); GridViewSP.UpdateCurrentRow();
            DataRow nr = _dtSpareParts.NewRow();
            nr["No"] = _dtSpareParts.Rows.Count + 1;
            nr["Unlimited"] = false;
            nr["Quantity"] = 0m; nr["UnitPrice"] = 0m; nr["Amount"] = 0m;
            nr["TaxInclusive"] = false;
            nr["TaxPercent"] = 0m; nr["TaxAmount"] = 0m; nr["AmountAfterTax"] = 0m;
            _dtSpareParts.Rows.Add(nr);
            int last = _dtSpareParts.Rows.Count - 1;
            GridViewSP.FocusedRowHandle = GridViewSP.GetRowHandle(last);
            GridViewSP.FocusedColumn = GridViewSP.Columns["ItemCode"];
            GridViewSP.ShowEditor();
        }

        private void OnSPDelete(object sender, EventArgs e)
        {
            int rh = GridViewSP.FocusedRowHandle;
            if (rh < 0) return;
            DataRow row = GridViewSP.GetDataRow(rh);
            if (row == null) return;
            GridViewSP.CloseEditor();
            row.Delete();
            _dtSpareParts.AcceptChanges();
            int n = 1;
            foreach (DataRow dr in _dtSpareParts.Rows) dr["No"] = n++;
        }

        private void OnSVAdd(object sender, EventArgs e)
        {
            GridViewSV.CloseEditor(); GridViewSV.UpdateCurrentRow();
            DataRow nr = _dtServiceSites.NewRow();
            nr["No"] = _dtServiceSites.Rows.Count + 1;
            _dtServiceSites.Rows.Add(nr);
            int last = _dtServiceSites.Rows.Count - 1;
            GridViewSV.FocusedRowHandle = GridViewSV.GetRowHandle(last);
            GridViewSV.FocusedColumn = GridViewSV.Columns["ServiceItemCode"];
            GridViewSV.ShowEditor();
        }

        private void OnSVDelete(object sender, EventArgs e)
        {
            int rh = GridViewSV.FocusedRowHandle;
            if (rh < 0) return;
            DataRow row = GridViewSV.GetDataRow(rh);
            if (row == null) return;
            GridViewSV.CloseEditor();
            row.Delete();
            _dtServiceSites.AcceptChanges();
            int n = 1;
            foreach (DataRow dr in _dtServiceSites.Rows) dr["No"] = n++;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            if (_mode == FormMode.View) { this.DialogResult = DialogResult.Cancel; this.Close(); return; }
            if (_isDirty && !ConfirmDiscard()) return;
            if (_mode == FormMode.Edit && _serviceContractKey > 0)
            {
                _suppressDirty = true;
                try { LoadExisting(); }
                finally { _suppressDirty = false; _isDirty = false; }
                _mode = FormMode.View;
                ApplyMode();
                UpdateStatusLabel();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void OnExit(object sender, EventArgs e) { this.Close(); }

        private void GridView_InitNewRow_SP(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DataRow row = GridViewSP.GetDataRow(e.RowHandle);
            if (row == null) return;
            row["No"] = _dtSpareParts.Rows.Count;
            row["Unlimited"] = false;
            row["TaxInclusive"] = false;
            if (row["Quantity"] == DBNull.Value) row["Quantity"] = 0m;
            if (row["UnitPrice"] == DBNull.Value) row["UnitPrice"] = 0m;
            if (row["Amount"] == DBNull.Value) row["Amount"] = 0m;
            if (row["TaxPercent"] == DBNull.Value) row["TaxPercent"] = 0m;
            if (row["TaxAmount"] == DBNull.Value) row["TaxAmount"] = 0m;
            if (row["AmountAfterTax"] == DBNull.Value) row["AmountAfterTax"] = 0m;
        }

        private void GridView_InitNewRow_SV(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DataRow row = GridViewSV.GetDataRow(e.RowHandle);
            if (row == null) return;
            row["No"] = _dtServiceSites.Rows.Count;
        }

        private void OnSPMoveUp(object sender, EventArgs e) { MoveGridRow(GridViewSP, _dtSpareParts, -1); }
        private void OnSPMoveDown(object sender, EventArgs e) { MoveGridRow(GridViewSP, _dtSpareParts, +1); }
        private void OnSVMoveUp(object sender, EventArgs e) { MoveGridRow(GridViewSV, _dtServiceSites, -1); }
        private void OnSVMoveDown(object sender, EventArgs e) { MoveGridRow(GridViewSV, _dtServiceSites, +1); }

        private static void MoveGridRow(DevExpress.XtraGrid.Views.Grid.GridView view, DataTable dt, int direction)
        {
            view.CloseEditor();
            int rh = view.FocusedRowHandle;
            if (rh < 0) return;
            DataRow src = view.GetDataRow(rh);
            if (src == null) return;
            int idx = dt.Rows.IndexOf(src);
            int target = idx + direction;
            if (target < 0 || target >= dt.Rows.Count) return;
            object[] a = src.ItemArray;
            object[] b = dt.Rows[target].ItemArray;
            dt.Rows[idx].ItemArray = b;
            dt.Rows[target].ItemArray = a;
            int n = 1;
            foreach (DataRow dr in dt.Rows) dr["No"] = n++;
            view.FocusedRowHandle = view.GetRowHandle(target);
        }
    }
}
