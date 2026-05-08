using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;
using ServiceContractPhotocopier.Classes;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    public partial class ServiceItem_Form : XtraForm
    {
        private enum FormMode { New, Edit, View }

        private DBSetting _dbSetting;
        private long _serviceItemKey = 0;
        private bool _isNew = true;
        private DataTable _dtMeterTypes;

        // CRUD state machine
        private FormMode _mode = FormMode.New;
        private bool _isDirty = false;
        private DateTime? _rowVersion;       // captured LastModified for optimistic concurrency
        private string _currentUserCode = "ADMIN";
        private bool _suppressDirty = false; // set during programmatic loads to avoid spurious dirty flags

        public ServiceItem_Form() { InitializeComponent(); }

        public ServiceItem_Form(UserSession userSession) : this()
        {
            if (userSession != null) _dbSetting = userSession.DBSetting;
            this.Load += new EventHandler(OnFormLoad);
        }

        public ServiceItem_Form(DBSetting dbSetting) : this()
        {
            _dbSetting = dbSetting;
            this.Load += new EventHandler(OnFormLoad);
        }

        public ServiceItem_Form(DBSetting dbSetting, DataRow existing) : this()
        {
            _dbSetting = dbSetting;
            if (existing != null && existing.Table.Columns.Contains("ServiceItemKey"))
            {
                _serviceItemKey = Convert.ToInt64(existing["ServiceItemKey"]);
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
                _dtMeterTypes = NewMeterTypesTable();
                GridMeterTypes.DataSource = _dtMeterTypes;

                if (!_isNew)
                {
                    LoadExisting();
                    _mode = FormMode.View;
                }
                else
                {
                    DtPurchaseDate.DateTime = DateTime.Today;
                    DtServiceStartDate.DateTime = DateTime.Today;
                    DtServiceEndDate.DateTime = DateTime.Today;
                    DtPMStartDate.DateTime = DateTime.Today;
                    DtPMLastServiceDate.DateTime = DateTime.Today;
                    DtPMNextServiceDate.DateTime = DateTime.Today;
                    CmbPMIntervalType.Text = "NONE";
                    OnAutoTag(null, EventArgs.Empty); // pre-fill the next code so user can start typing
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

        private void LoadLookups()
        {
            try
            {
                // Grade: Code + Description
                var dtGrade = _dbSetting.GetDataTable(
                    "SELECT ServiceItemGradeCode, [Description] FROM [dbo].[zSCP_LK_ServiceItemGrade] WHERE Inactive='N' ORDER BY ServiceItemGradeCode", false);
                CmbServiceTag.Properties.DataSource = dtGrade;
                CmbServiceTag.Properties.DisplayMember = "ServiceItemGradeCode";
                CmbServiceTag.Properties.ValueMember = "ServiceItemGradeCode";
                CmbServiceTag.Properties.Columns.Clear();
                CmbServiceTag.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ServiceItemGradeCode", "Grade Code", 100));
                CmbServiceTag.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description", 220));

                // Stock Code — Item table
                try
                {
                    DataTable dtItem = _dbSetting.GetDataTable(
                        "SELECT ItemCode, [Description] FROM [dbo].[Item] ORDER BY ItemCode", false);
                    LkStockCode.Properties.DataSource = dtItem;
                    LkStockCode.Properties.DisplayMember = "ItemCode";
                    LkStockCode.Properties.ValueMember = "ItemCode";
                    LkStockCode.Properties.PopupFormWidth = 520;
                    EnsureDropdownButton(LkStockCode);
                    DevExpress.XtraGrid.Views.Grid.GridView v = LkStockCode.Properties.View;
                    v.Columns.Clear();
                    v.OptionsView.ShowGroupPanel = false;
                    v.Columns.AddField("ItemCode").VisibleIndex = 0;
                    v.Columns.AddField("Description").VisibleIndex = 1;
                    LkStockCode.EditValueChanged += LkStockCode_EditValueChanged;
                }
                catch { }

                // Debtor Code
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
                    v.Columns.Clear();
                    v.OptionsView.ShowGroupPanel = false;
                    v.Columns.AddField("DebtorCode").VisibleIndex = 0;
                    v.Columns.AddField("CompanyName").VisibleIndex = 1;
                    LkDebtorCode.EditValueChanged += LkDebtorCode_EditValueChanged;
                }
                catch { }

                // Agent Code — SalesAgent table
                try
                {
                    DataTable dtAg = _dbSetting.GetDataTable(
                        "SELECT SalesAgent, [Description] FROM [dbo].[SalesAgent] WHERE IsActive='Y' OR IsActive IS NULL ORDER BY SalesAgent", false);
                    LkAgentCode.Properties.DataSource = dtAg;
                    LkAgentCode.Properties.DisplayMember = "SalesAgent";
                    LkAgentCode.Properties.ValueMember = "SalesAgent";
                    LkAgentCode.Properties.PopupFormWidth = 420;
                    EnsureDropdownButton(LkAgentCode);
                    DevExpress.XtraGrid.Views.Grid.GridView vAg = LkAgentCode.Properties.View;
                    vAg.Columns.Clear(); vAg.OptionsView.ShowGroupPanel = false;
                    vAg.Columns.AddField("SalesAgent").VisibleIndex = 0;
                    vAg.Columns.AddField("Description").VisibleIndex = 1;
                    LkAgentCode.EditValueChanged += LkAgentCode_EditValueChanged;
                }
                catch { }

                // Term — AutoCount Terms table
                try
                {
                    DataTable dtTerm = _dbSetting.GetDataTable(
                        "SELECT Terms, ISNULL(DisplayTerm, Terms) AS DisplayTerm, ISNULL(TermType,'') AS TermType, ISNULL(TermDays,0) AS TermDays FROM [dbo].[Terms] ORDER BY Terms", false);
                    LkTerm.Properties.DataSource = dtTerm;
                    LkTerm.Properties.DisplayMember = "Terms";
                    LkTerm.Properties.ValueMember = "Terms";
                    LkTerm.Properties.PopupFormWidth = 460;
                    EnsureDropdownButton(LkTerm);
                    DevExpress.XtraGrid.Views.Grid.GridView vT = LkTerm.Properties.View;
                    vT.Columns.Clear(); vT.OptionsView.ShowGroupPanel = false;
                    vT.Columns.AddField("Terms").VisibleIndex = 0;
                    vT.Columns.AddField("DisplayTerm").VisibleIndex = 1;
                    vT.Columns.AddField("TermType").VisibleIndex = 2;
                    vT.Columns.AddField("TermDays").VisibleIndex = 3;
                }
                catch { }

                // Area — AutoCount Area table
                try
                {
                    DataTable dtArea = _dbSetting.GetDataTable(
                        "SELECT AreaCode, ISNULL([Description],'') AS [Description] FROM [dbo].[Area] ORDER BY AreaCode", false);
                    LkArea.Properties.DataSource = dtArea;
                    LkArea.Properties.DisplayMember = "AreaCode";
                    LkArea.Properties.ValueMember = "AreaCode";
                    LkArea.Properties.PopupFormWidth = 360;
                    EnsureDropdownButton(LkArea);
                    DevExpress.XtraGrid.Views.Grid.GridView vA = LkArea.Properties.View;
                    vA.Columns.Clear(); vA.OptionsView.ShowGroupPanel = false;
                    vA.Columns.AddField("AreaCode").VisibleIndex = 0;
                    vA.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch { }

                // Department — AutoCount Dept table
                try
                {
                    DataTable dtDept = _dbSetting.GetDataTable(
                        "SELECT DeptNo, ISNULL([Description],'') AS [Description] FROM [dbo].[Dept] WHERE IsActive='T' OR IsActive IS NULL ORDER BY DeptNo", false);
                    LkDepartment.Properties.DataSource = dtDept;
                    LkDepartment.Properties.DisplayMember = "DeptNo";
                    LkDepartment.Properties.ValueMember = "DeptNo";
                    LkDepartment.Properties.PopupFormWidth = 420;
                    EnsureDropdownButton(LkDepartment);
                    DevExpress.XtraGrid.Views.Grid.GridView vD = LkDepartment.Properties.View;
                    vD.Columns.Clear(); vD.OptionsView.ShowGroupPanel = false;
                    vD.Columns.AddField("DeptNo").VisibleIndex = 0;
                    vD.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch { }

                // Job — AutoCount 2.x ships with [Project] (the Project & Job Costing table).
                // Fall back to [Job] if a legacy build is installed; degrade to an empty dropdown otherwise.
                try
                {
                    DataTable dtJob;
                    try
                    {
                        dtJob = _dbSetting.GetDataTable(
                            "SELECT ProjNo AS JobNo, ISNULL([Description],'') AS [Description] FROM [dbo].[Project] WHERE IsActive='T' OR IsActive IS NULL ORDER BY ProjNo", false);
                    }
                    catch
                    {
                        dtJob = _dbSetting.GetDataTable(
                            "SELECT JobNo, ISNULL([Description],'') AS [Description] FROM [dbo].[Job] ORDER BY JobNo", false);
                    }
                    LkJob.Properties.DataSource = dtJob;
                    LkJob.Properties.DisplayMember = "JobNo";
                    LkJob.Properties.ValueMember = "JobNo";
                    LkJob.Properties.PopupFormWidth = 420;
                    EnsureDropdownButton(LkJob);
                    DevExpress.XtraGrid.Views.Grid.GridView vJ = LkJob.Properties.View;
                    vJ.Columns.Clear(); vJ.OptionsView.ShowGroupPanel = false;
                    vJ.Columns.AddField("JobNo").VisibleIndex = 0;
                    vJ.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch
                {
                    // Neither Project nor Job table available — still show a dropdown button for UI consistency.
                    EnsureDropdownButton(LkJob);
                }

                // Location — AutoCount Location table
                try
                {
                    DataTable dtLoc = _dbSetting.GetDataTable(
                        "SELECT Location, ISNULL([Description],'') AS [Description] FROM [dbo].[Location] ORDER BY Location", false);
                    LkLocation.Properties.DataSource = dtLoc;
                    LkLocation.Properties.DisplayMember = "Location";
                    LkLocation.Properties.ValueMember = "Location";
                    LkLocation.Properties.PopupFormWidth = 420;
                    EnsureDropdownButton(LkLocation);
                    DevExpress.XtraGrid.Views.Grid.GridView vL = LkLocation.Properties.View;
                    vL.Columns.Clear(); vL.OptionsView.ShowGroupPanel = false;
                    vL.Columns.AddField("Location").VisibleIndex = 0;
                    vL.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch { }

                // Contract No
                try
                {
                    var dtCon = _dbSetting.GetDataTable(
                        "SELECT ServiceContractCode, [Description] FROM [dbo].[zSCP_ServiceContract] ORDER BY ServiceContractCode", false);
                    LkContractNo.Properties.DataSource = dtCon;
                    LkContractNo.Properties.DisplayMember = "ServiceContractCode";
                    LkContractNo.Properties.ValueMember = "ServiceContractCode";
                    LkContractNo.Properties.PopupFormWidth = 520;
                    var v = LkContractNo.Properties.View;
                    v.Columns.Clear();
                    v.OptionsView.ShowGroupPanel = false;
                    v.Columns.AddField("ServiceContractCode").VisibleIndex = 0;
                    v.Columns.AddField("Description").VisibleIndex = 1;
                }
                catch { }

                // PM Interval Type
                CmbPMIntervalType.Properties.Items.Clear();
                CmbPMIntervalType.Properties.Items.AddRange(new[] { "NONE", "DAILY", "WEEKLY", "MONTHLY", "YEARLY" });

                // Meter Type + MultiPrice lookup repositories (grid on tab 7)
                try
                {
                    var dtMT = _dbSetting.GetDataTable(
                        "SELECT MeterTypeCode, [Description] FROM [dbo].[zSCP_MeterType] WHERE Inactive='N' ORDER BY MeterTypeCode", false);
                    RepoMeterTypeCode.DataSource = dtMT;
                }
                catch { }
                try
                {
                    var dtMP = _dbSetting.GetDataTable(
                        "SELECT MeterMultiPriceCode, [Description] FROM [dbo].[zSCP_MeterMultiPrice] ORDER BY MeterMultiPriceCode", false);
                    RepoMultiPriceCode.DataSource = dtMP;
                }
                catch { }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lookup load warning:\r\n" + ex.Message, "Load", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private DataTable NewMeterTypesTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ServiceItemMeterTypeKey", typeof(long));
            dt.Columns.Add("MeterTypeCode", typeof(string));
            dt.Columns.Add("MeterTypeName", typeof(string));
            dt.Columns.Add("MeterMultiPriceCode", typeof(string));
            dt.Columns.Add("ChargesRate", typeof(decimal));
            dt.Columns.Add("MinimumCharges", typeof(decimal));
            dt.Columns.Add("FOCQty", typeof(decimal));
            dt.Columns.Add("RebateQtyInPercent", typeof(decimal));
            dt.Columns.Add("InitialReading", typeof(decimal));
            DataColumn cLRD = dt.Columns.Add("LastReadingDate", typeof(DateTime)); cLRD.AllowDBNull = true;
            DataColumn cLRM = dt.Columns.Add("LastReadingMeter", typeof(decimal)); cLRM.AllowDBNull = true;
            return dt;
        }

        private void LoadExisting()
        {
            try
            {
                DataTable dt = _dbSetting.GetDataTable("SELECT * FROM [dbo].[zSCP_ServiceItem] WHERE ServiceItemKey = " + _serviceItemKey, false);
                if (dt.Rows.Count == 0) { _isNew = true; return; }
                DataRow r = dt.Rows[0];
                _rowVersion = r["LastModified"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["LastModified"]);
                TxtServiceItemCode.Text = S(r, "ServiceItemCode");
                CmbServiceTag.EditValue = S(r, "ServiceItemGradeCode");
                if (r["PurchaseDate"] != DBNull.Value) DtPurchaseDate.DateTime = Convert.ToDateTime(r["PurchaseDate"]);
                LkStockCode.EditValue = S(r, "StockCode");
                LkDebtorCode.EditValue = S(r, "DebtorCode");
                LkAgentCode.EditValue = S(r, "StaffCode");
                LkTerm.EditValue = S(r, "TermCode");
                LkArea.EditValue = S(r, "AreaCode");
                SpnUnitPrice.Value = r["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(r["UnitPrice"]);
                ChkInactive.Checked = S(r, "Inactive") == "Y";
                TxtRefNo.Text = S(r, "RefNo");
                TxtDescription.Text = S(r, "Description");
                LkContractNo.EditValue = S(r, "ContractNo");
                if (r["ServiceStartDate"] != DBNull.Value) DtServiceStartDate.DateTime = Convert.ToDateTime(r["ServiceStartDate"]);
                if (r["ServiceExpiryDate"] != DBNull.Value) DtServiceEndDate.DateTime = Convert.ToDateTime(r["ServiceExpiryDate"]);
                TxtAddress.Text = S(r, "Address1") + "\r\n" + S(r, "Address2") + "\r\n" + S(r, "Address3") + "\r\n" + S(r, "Address4");
                TxtAttention.Text = S(r, "Attention");

                // More Header — main address detail
                TxtMHCity.Text = S(r, "City");
                TxtMHPostalCode.Text = S(r, "PostalCode");
                CmbMHState.Text = S(r, "State");
                CmbMHCountry.Text = S(r, "CountryCode");
                TxtMHPhone.Text = S(r, "Phone");
                TxtMHFax.Text = S(r, "Fax");
                TxtMHRef1.Text = S(r, "Ref1");
                TxtMHRef2.Text = S(r, "Ref2");
                TxtMHRef3.Text = S(r, "Ref3");
                TxtMHRef4.Text = S(r, "Ref4");

                // More Header — delivery (Do*) fields
                TxtMHBranchCode.Text = S(r, "BranchCode");
                TxtMHBranchName.Text = S(r, "BranchName");
                TxtMHAddress.Text = S(r, "DOAddress1") + "\r\n" + S(r, "DOAddress2") + "\r\n" + S(r, "DOAddress3") + "\r\n" + S(r, "DOAddress4");
                TxtMHDOCity.Text = S(r, "DOCity");
                TxtMHDOPostalCode.Text = S(r, "DOPostalCode");
                CmbMHDOState.Text = S(r, "DOState");
                CmbMHDOCountry.Text = S(r, "DOCountryCode");
                TxtMHDOPhone.Text = S(r, "DOPhone");
                TxtMHDOFax.Text = S(r, "DOFax");
                TxtMHDOEmail.Text = S(r, "DOEmail");
                TxtMHDOContactPerson.Text = S(r, "DOContactPerson");
                TxtNote.Text = r["Note"] == DBNull.Value ? "" : r["Note"].ToString();
                ChkPMActive.Checked = S(r, "PMActive") == "Y";
                CmbPMIntervalType.Text = S(r, "PMIntervalType");
                if (r["PMStartDate"] != DBNull.Value) DtPMStartDate.DateTime = Convert.ToDateTime(r["PMStartDate"]);
                if (r["PMLastServiceDate"] != DBNull.Value) DtPMLastServiceDate.DateTime = Convert.ToDateTime(r["PMLastServiceDate"]);
                if (r.Table.Columns.Contains("PMNextServiceDate") && r["PMNextServiceDate"] != DBNull.Value)
                    DtPMNextServiceDate.DateTime = Convert.ToDateTime(r["PMNextServiceDate"]);
                LkDepartment.EditValue = S(r, "DepartmentCode");
                LkJob.EditValue = S(r, "JobCode");
                LkLocation.EditValue = S(r, "StockLocationCode");
                SpnPMIntervalValue.Value = r["PMIntervalValue"] == DBNull.Value ? 0 : Convert.ToDecimal(r["PMIntervalValue"]);
                TxtRemark1.Text = S(r, "Remark1");
                TxtRemark2.Text = S(r, "Remark2");
                TxtRemark3.Text = S(r, "Remark3");
                TxtRemark4.Text = S(r, "Remark4");

                string mtSql =
                    "SELECT smt.ServiceItemMeterTypeKey, smt.MeterTypeCode, " +
                    "       ISNULL(mt.[Description], '') AS MeterTypeName, " +
                    "       ISNULL(smt.MeterMultiPriceCode, '') AS MeterMultiPriceCode, " +
                    "       ISNULL(smt.ChargesRate, 0)        AS ChargesRate, " +
                    "       ISNULL(smt.MinimumCharges, 0)     AS MinimumCharges, " +
                    "       ISNULL(smt.FOCQty, 0)             AS FOCQty, " +
                    "       ISNULL(smt.RebateQtyInPercent, 0) AS RebateQtyInPercent, " +
                    "       ISNULL(smt.InitialReading, 0)     AS InitialReading, " +
                    "       lr.MeterTransDate                 AS LastReadingDate, " +
                    "       lr.MeterTransReading              AS LastReadingMeter " +
                    "FROM   [dbo].[zSCP_ServiceItemMeterType] smt " +
                    "LEFT JOIN [dbo].[zSCP_MeterType] mt ON mt.MeterTypeCode = smt.MeterTypeCode " +
                    "OUTER APPLY ( " +
                    "  SELECT TOP 1 MeterTransDate, MeterTransReading " +
                    "  FROM   [dbo].[zSCP_MeterTrans] " +
                    "  WHERE  ServiceItemMeterTypeKey = smt.ServiceItemMeterTypeKey " +
                    "  ORDER BY MeterTransDate DESC, MeterTransKey DESC " +
                    ") lr " +
                    "WHERE  smt.ServiceItemKey = " + _serviceItemKey + " " +
                    "ORDER BY smt.ServiceItemMeterTypeKey";
                DataTable dtMT = _dbSetting.GetDataTable(mtSql, false);
                _dtMeterTypes.Clear();
                foreach (DataRow row in dtMT.Rows)
                {
                    DataRow nr = _dtMeterTypes.NewRow();
                    foreach (DataColumn c in _dtMeterTypes.Columns)
                    {
                        if (dtMT.Columns.Contains(c.ColumnName) && row[c.ColumnName] != DBNull.Value)
                            nr[c.ColumnName] = row[c.ColumnName];
                    }
                    _dtMeterTypes.Rows.Add(nr);
                }

                // Tab 5 — Service Meter History (read-only)
                try
                {
                    GridMeterHistory.DataSource = _dbSetting.GetDataTable(
                        "SELECT MeterTransDate, MeterTypeCode, MeterTransReading, Remark " +
                        "FROM [dbo].[zSCP_MeterTrans] WHERE ServiceItemKey = " + _serviceItemKey +
                        " ORDER BY MeterTransDate DESC, MeterTransKey DESC", false);
                }
                catch { }

                // Tab 6 — Debtor Ownership History (read-only)
                try
                {
                    GridDebtorHistory.DataSource = _dbSetting.GetDataTable(
                        "SELECT DebtorCode, StartDate, EndDate, CurrencyCode, CurrencyRate, Remark " +
                        "FROM [dbo].[zSCP_ServiceItemDebtorHistory] WHERE ServiceItemKey = " + _serviceItemKey +
                        " ORDER BY StartDate DESC, ServiceItemDebtorHistoryKey DESC", false);
                }
                catch { }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Failed to load service item:\r\n" + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatusLabel()
        {
            if (DtServiceEndDate.DateTime == DateTime.MinValue) { LblStatus.Text = "Status:  No Stated"; return; }
            var days = (DateTime.Today - DtServiceEndDate.DateTime).Days;
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

        private static string EV(Control c) { return c.Text ?? ""; }
        private static string EV(BaseEdit b) { return b.EditValue == null ? "" : b.EditValue.ToString(); }

        private void OnSave(object sender, EventArgs e)
        {
            if (!ValidateForSave()) return;

            try
            {
                GridViewMT.CloseEditor(); GridViewMT.UpdateCurrentRow();

                string code = SQLString(TxtServiceItemCode.Text.Trim());
                string tag = SQLString(EV(CmbServiceTag));
                string stk = SQLString(EV(LkStockCode));
                string debtor = SQLString(EV(LkDebtorCode));
                string agent = SQLString(EV(LkAgentCode));
                string term = SQLString(EV(LkTerm));
                string area = SQLString(EV(LkArea));
                string refNo = SQLString(TxtRefNo.Text ?? "");
                string desc = SQLString(TxtDescription.Text ?? "");
                string contract = SQLString(EV(LkContractNo));
                string attn = SQLString(TxtAttention.Text ?? "");
                string inactive = ChkInactive.Checked ? "Y" : "N";
                decimal unitPrice = SpnUnitPrice.Value;
                string note = SQLString(TxtNote.Text ?? "");
                string r1 = SQLString(TxtRemark1.Text ?? ""), r2 = SQLString(TxtRemark2.Text ?? ""), r3 = SQLString(TxtRemark3.Text ?? ""), r4 = SQLString(TxtRemark4.Text ?? "");
                string pmActive = ChkPMActive.Checked ? "Y" : "N";
                string pmType = SQLString(CmbPMIntervalType.Text ?? "NONE");
                int pmVal = (int)SpnPMIntervalValue.Value;
                string dtPurchase = "'" + DtPurchaseDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtStart = "'" + DtServiceStartDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtEnd = "'" + DtServiceEndDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtPMStart = DtPMStartDate.DateTime == DateTime.MinValue ? "NULL" : "'" + DtPMStartDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtPMLast = DtPMLastServiceDate.DateTime == DateTime.MinValue ? "NULL" : "'" + DtPMLastServiceDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtPMNext = DtPMNextServiceDate.DateTime == DateTime.MinValue ? "NULL" : "'" + DtPMNextServiceDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dept = SQLString(EV(LkDepartment));
                string job = SQLString(EV(LkJob));
                string loc = SQLString(EV(LkLocation));

                string[] addr = (TxtAddress.Text ?? "").Replace("\r\n", "\n").Split('\n');
                string a1 = SQLString(addr.Length > 0 ? addr[0] : "");
                string a2 = SQLString(addr.Length > 1 ? addr[1] : "");
                string a3 = SQLString(addr.Length > 2 ? addr[2] : "");
                string a4 = SQLString(addr.Length > 3 ? addr[3] : "");

                // More Header — main
                string mhCity = SQLString(TxtMHCity.Text ?? "");
                string mhPostal = SQLString(TxtMHPostalCode.Text ?? "");
                string mhState = SQLString(CmbMHState.Text ?? "");
                string mhCountry = SQLString(CmbMHCountry.Text ?? "");
                string mhPhone = SQLString(TxtMHPhone.Text ?? "");
                string mhFax = SQLString(TxtMHFax.Text ?? "");
                string mhRef1 = SQLString(TxtMHRef1.Text ?? "");
                string mhRef2 = SQLString(TxtMHRef2.Text ?? "");
                string mhRef3 = SQLString(TxtMHRef3.Text ?? "");
                string mhRef4 = SQLString(TxtMHRef4.Text ?? "");

                // More Header — delivery
                string mhBranchCode = SQLString(TxtMHBranchCode.Text ?? "");
                string mhBranchName = SQLString(TxtMHBranchName.Text ?? "");
                string[] doAddr = (TxtMHAddress.Text ?? "").Replace("\r\n", "\n").Split('\n');
                string doA1 = SQLString(doAddr.Length > 0 ? doAddr[0] : "");
                string doA2 = SQLString(doAddr.Length > 1 ? doAddr[1] : "");
                string doA3 = SQLString(doAddr.Length > 2 ? doAddr[2] : "");
                string doA4 = SQLString(doAddr.Length > 3 ? doAddr[3] : "");
                string doCity = SQLString(TxtMHDOCity.Text ?? "");
                string doPostal = SQLString(TxtMHDOPostalCode.Text ?? "");
                string doState = SQLString(CmbMHDOState.Text ?? "");
                string doCountry = SQLString(CmbMHDOCountry.Text ?? "");
                string doPhone = SQLString(TxtMHDOPhone.Text ?? "");
                string doFax = SQLString(TxtMHDOFax.Text ?? "");
                string doEmail = SQLString(TxtMHDOEmail.Text ?? "");
                string doContact = SQLString(TxtMHDOContactPerson.Text ?? "");

                string user = SQLString(_currentUserCode ?? "ADMIN");

                using (SqlConnection cn = new SqlConnection(_dbSetting.ConnectionString))
                {
                    cn.Open();
                    using (SqlTransaction tx = cn.BeginTransaction("ScpItemSave"))
                    {
                        try
                        {
                            string sql;
                            if (_mode == FormMode.New)
                            {
                                sql = "INSERT INTO [dbo].[zSCP_ServiceItem] " +
                                    "(ServiceItemCode, StockCode, ServiceItemGradeCode, PurchaseDate, UnitPrice, RefNo, [Description], " +
                                    " ContractNo, ServiceStartDate, ServiceExpiryDate, DebtorCode, StaffCode, TermCode, AreaCode, " +
                                    " Address1, Address2, Address3, Address4, City, PostalCode, [State], CountryCode, Phone, Fax, Attention, " +
                                    " BranchCode, BranchName, DOAddress1, DOAddress2, DOAddress3, DOAddress4, DOCity, DOPostalCode, DOState, DOCountryCode, DOPhone, DOFax, DOEmail, DOContactPerson, " +
                                    " Ref1, Ref2, Ref3, Ref4, " +
                                    " Note, Remark1, Remark2, Remark3, Remark4, PMIntervalType, PMIntervalValue, PMStartDate, PMLastServiceDate, PMNextServiceDate, PMActive, DepartmentCode, JobCode, StockLocationCode, Inactive, " +
                                    " Created, Modified, CreatedBy, ModifiedBy) VALUES " +
                                    "(N'" + code + "', N'" + stk + "', N'" + tag + "', " + dtPurchase + ", " + unitPrice.ToString("0.00") + ", N'" + refNo + "', N'" + desc + "', " +
                                    "N'" + contract + "', " + dtStart + ", " + dtEnd + ", N'" + debtor + "', N'" + agent + "', N'" + term + "', N'" + area + "', " +
                                    "N'" + a1 + "', N'" + a2 + "', N'" + a3 + "', N'" + a4 + "', N'" + mhCity + "', N'" + mhPostal + "', N'" + mhState + "', N'" + mhCountry + "', N'" + mhPhone + "', N'" + mhFax + "', N'" + attn + "', " +
                                    "N'" + mhBranchCode + "', N'" + mhBranchName + "', N'" + doA1 + "', N'" + doA2 + "', N'" + doA3 + "', N'" + doA4 + "', N'" + doCity + "', N'" + doPostal + "', N'" + doState + "', N'" + doCountry + "', N'" + doPhone + "', N'" + doFax + "', N'" + doEmail + "', N'" + doContact + "', " +
                                    "N'" + mhRef1 + "', N'" + mhRef2 + "', N'" + mhRef3 + "', N'" + mhRef4 + "', " +
                                    "N'" + note + "', N'" + r1 + "', N'" + r2 + "', N'" + r3 + "', N'" + r4 + "', " +
                                    "N'" + pmType + "', " + pmVal + ", " + dtPMStart + ", " + dtPMLast + ", " + dtPMNext + ", '" + pmActive + "', N'" + dept + "', N'" + job + "', N'" + loc + "', '" + inactive + "', " +
                                    "GETDATE(), GETDATE(), N'" + user + "', N'" + user + "'); " +
                                    "SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
                                using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                                {
                                    object obj = cmd.ExecuteScalar();
                                    _serviceItemKey = Convert.ToInt64(obj);
                                }
                            }
                            else
                            {
                                string verPredicate = _rowVersion.HasValue
                                    ? " AND LastModified = '" + _rowVersion.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"
                                    : "";
                                sql = "UPDATE [dbo].[zSCP_ServiceItem] SET " +
                                    "StockCode=N'" + stk + "', ServiceItemGradeCode=N'" + tag + "', " +
                                    "PurchaseDate=" + dtPurchase + ", UnitPrice=" + unitPrice.ToString("0.00") + ", RefNo=N'" + refNo + "', [Description]=N'" + desc + "', " +
                                    "ContractNo=N'" + contract + "', ServiceStartDate=" + dtStart + ", ServiceExpiryDate=" + dtEnd + ", " +
                                    "DebtorCode=N'" + debtor + "', StaffCode=N'" + agent + "', TermCode=N'" + term + "', AreaCode=N'" + area + "', " +
                                    "Address1=N'" + a1 + "', Address2=N'" + a2 + "', Address3=N'" + a3 + "', Address4=N'" + a4 + "', " +
                                    "City=N'" + mhCity + "', PostalCode=N'" + mhPostal + "', [State]=N'" + mhState + "', CountryCode=N'" + mhCountry + "', " +
                                    "Phone=N'" + mhPhone + "', Fax=N'" + mhFax + "', " +
                                    "BranchCode=N'" + mhBranchCode + "', BranchName=N'" + mhBranchName + "', " +
                                    "DOAddress1=N'" + doA1 + "', DOAddress2=N'" + doA2 + "', DOAddress3=N'" + doA3 + "', DOAddress4=N'" + doA4 + "', " +
                                    "DOCity=N'" + doCity + "', DOPostalCode=N'" + doPostal + "', DOState=N'" + doState + "', DOCountryCode=N'" + doCountry + "', " +
                                    "DOPhone=N'" + doPhone + "', DOFax=N'" + doFax + "', DOEmail=N'" + doEmail + "', DOContactPerson=N'" + doContact + "', " +
                                    "Ref1=N'" + mhRef1 + "', Ref2=N'" + mhRef2 + "', Ref3=N'" + mhRef3 + "', Ref4=N'" + mhRef4 + "', " +
                                    "Attention=N'" + attn + "', Note=N'" + note + "', Remark1=N'" + r1 + "', Remark2=N'" + r2 + "', Remark3=N'" + r3 + "', Remark4=N'" + r4 + "', " +
                                    "PMIntervalType=N'" + pmType + "', PMIntervalValue=" + pmVal + ", PMStartDate=" + dtPMStart + ", PMLastServiceDate=" + dtPMLast + ", PMNextServiceDate=" + dtPMNext + ", " +
                                    "PMActive='" + pmActive + "', DepartmentCode=N'" + dept + "', JobCode=N'" + job + "', StockLocationCode=N'" + loc + "', " +
                                    "Inactive='" + inactive + "', Modified=GETDATE(), LastModified=GETDATE(), ModifiedBy=N'" + user + "' " +
                                    "WHERE ServiceItemKey=" + _serviceItemKey + verPredicate;
                                using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                                {
                                    int rows = cmd.ExecuteNonQuery();
                                    if (rows == 0)
                                        throw new Exception("This Service Item was modified or deleted by another user. Click OK and reopen the record to see the latest values.");
                                }
                            }

                            using (SqlCommand del = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceItemMeterType] WHERE ServiceItemKey=" + _serviceItemKey, cn, tx))
                                del.ExecuteNonQuery();

                            foreach (DataRow row in _dtMeterTypes.Rows)
                            {
                                if (row.RowState == DataRowState.Deleted) continue;
                                string mtc = SQLString(row["MeterTypeCode"] == DBNull.Value ? "" : row["MeterTypeCode"].ToString());
                                if (string.IsNullOrEmpty(mtc)) continue;
                                string mmpc = SQLString(row["MeterMultiPriceCode"] == DBNull.Value ? "" : row["MeterMultiPriceCode"].ToString());
                                decimal mc = row["MinimumCharges"] == DBNull.Value ? 0m : Convert.ToDecimal(row["MinimumCharges"]);
                                decimal cr = row["ChargesRate"] == DBNull.Value ? 0m : Convert.ToDecimal(row["ChargesRate"]);
                                decimal rq = row["RebateQtyInPercent"] == DBNull.Value ? 0m : Convert.ToDecimal(row["RebateQtyInPercent"]);
                                decimal fq = row["FOCQty"] == DBNull.Value ? 0m : Convert.ToDecimal(row["FOCQty"]);
                                decimal ir = row["InitialReading"] == DBNull.Value ? 0m : Convert.ToDecimal(row["InitialReading"]);
                                using (SqlCommand ins = new SqlCommand(
                                    "INSERT INTO [dbo].[zSCP_ServiceItemMeterType] " +
                                    "(ServiceItemKey, MeterTypeCode, MeterMultiPriceCode, MinimumCharges, ChargesRate, RebateQtyInPercent, FOCQty, InitialReading) VALUES (" +
                                    _serviceItemKey + ", N'" + mtc + "', N'" + mmpc + "', " +
                                    mc.ToString("0.00") + ", " + cr.ToString("0.000000") + ", " + rq.ToString("0.00") + ", " + fq.ToString("0.00") + ", " + ir.ToString("0.00") + ")",
                                    cn, tx))
                                    ins.ExecuteNonQuery();
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
                XtraMessageBox.Show("Saved.", "Service Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Save failed:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnMTAdd(object sender, EventArgs e)
        {
            GridViewMT.CloseEditor(); GridViewMT.UpdateCurrentRow();
            var nr = _dtMeterTypes.NewRow();
            nr["MinimumCharges"] = 0m; nr["ChargesRate"] = 0m;
            nr["RebateQtyInPercent"] = 0m; nr["FOCQty"] = 0m; nr["InitialReading"] = 0m;
            _dtMeterTypes.Rows.Add(nr);
            int last = _dtMeterTypes.Rows.Count - 1;
            GridViewMT.FocusedRowHandle = GridViewMT.GetRowHandle(last);
            GridViewMT.FocusedColumn = GridViewMT.Columns["MeterTypeCode"];
            GridViewMT.ShowEditor();
        }

        private void OnMTDelete(object sender, EventArgs e)
        {
            int rh = GridViewMT.FocusedRowHandle;
            if (rh < 0) return;
            var row = GridViewMT.GetDataRow(rh);
            if (row == null) return;
            if (XtraMessageBox.Show("Delete this meter type row?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            GridViewMT.CloseEditor();
            row.Delete();
            _dtMeterTypes.AcceptChanges();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            if (_mode == FormMode.View) { this.DialogResult = DialogResult.Cancel; this.Close(); return; }
            if (_isDirty && !ConfirmDiscard()) return;
            if (_mode == FormMode.Edit && _serviceItemKey > 0)
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

        // Description / name display wires ---------------------------------
        private void LkStockCode_EditValueChanged(object sender, EventArgs e)
        {
            LblStockDesc.Text = LookupDescription(LkStockCode, "ItemCode", "Description");
        }
        private void LkDebtorCode_EditValueChanged(object sender, EventArgs e)
        {
            LblDebtorName.Text = LookupDescription(LkDebtorCode, "DebtorCode", "CompanyName");
        }
        private void LkAgentCode_EditValueChanged(object sender, EventArgs e)
        {
            LblAgentName.Text = LookupDescription(LkAgentCode, "SalesAgent", "Description");
        }

        private static string LookupDescription(DevExpress.XtraEditors.SearchLookUpEdit edit, string keyCol, string descCol)
        {
            if (edit.EditValue == null) return "";
            DataTable dt = edit.Properties.DataSource as DataTable;
            if (dt == null || !dt.Columns.Contains(keyCol) || !dt.Columns.Contains(descCol)) return "";
            string key = edit.EditValue.ToString().Replace("'", "''");
            DataRow[] found = dt.Select(keyCol + " = '" + key + "'");
            if (found.Length == 0) return "";
            return found[0][descCol] == DBNull.Value ? "" : found[0][descCol].ToString();
        }

        // Force a visible dropdown button on a SearchLookUpEdit.
        private static void EnsureDropdownButton(DevExpress.XtraEditors.SearchLookUpEdit edit)
        {
            edit.Properties.Buttons.Clear();
            edit.Properties.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo));
            edit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            edit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        }

        // Header CRUD handlers ---------------------------------------------
        private void OnAdd(object sender, EventArgs e)
        {
            if (_isDirty && !ConfirmDiscard()) return;
            ResetFormToBlankNew();
            _mode = FormMode.New;
            ApplyMode();
            UpdateStatusLabel();
            TxtServiceItemCode.Focus();
        }

        private void OnEditClicked(object sender, EventArgs e)
        {
            if (_mode != FormMode.View || _serviceItemKey <= 0)
            {
                XtraMessageBox.Show("Open or save a record before editing.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _mode = FormMode.Edit;
            ApplyMode();
            LkStockCode.Focus();
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_mode != FormMode.View || _serviceItemKey <= 0)
            {
                XtraMessageBox.Show("Open a saved record before deleting.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (XtraMessageBox.Show("Permanently delete this Service Item and all its meter configuration / readings?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(_dbSetting.ConnectionString))
                {
                    cn.Open();
                    using (SqlTransaction tx = cn.BeginTransaction("ScpItemDelete"))
                    {
                        try
                        {
                            string verPredicate = _rowVersion.HasValue
                                ? " AND LastModified = '" + _rowVersion.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"
                                : "";
                            using (SqlCommand cmd = new SqlCommand(
                                "DELETE FROM [dbo].[zSCP_ServiceItem] WHERE ServiceItemKey=" + _serviceItemKey + verPredicate, cn, tx))
                            {
                                int rows = cmd.ExecuteNonQuery();
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
                XtraMessageBox.Show("Deleted.", "Service Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Delete failed:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    if (_isDirty) { e.Cancel = true; return; } // save failed
                }
            }
            base.OnFormClosing(e);
        }

        private bool ValidateForSave()
        {
            if (string.IsNullOrWhiteSpace(TxtServiceItemCode.Text))
            {
                XtraMessageBox.Show("Service Tag is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TxtServiceItemCode.Focus(); return false;
            }
            if (DtServiceStartDate.DateTime != DateTime.MinValue && DtServiceEndDate.DateTime != DateTime.MinValue
                && DtServiceEndDate.DateTime < DtServiceStartDate.DateTime)
            {
                XtraMessageBox.Show("Service End date is earlier than Start date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DtServiceEndDate.Focus(); return false;
            }
            return true;
        }

        private bool ConfirmDiscard()
        {
            return XtraMessageBox.Show(
                "Discard unsaved changes?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void ApplyMode()
        {
            bool editable = (_mode != FormMode.View);
            bool isNew = (_mode == FormMode.New);

            // Header fields (everything in PanelHeader except the read-only desc/name labels)
            foreach (Control c in PanelHeader.Controls)
            {
                if (c is LabelControl) continue;
                if (c == BtnResetContract || c == BtnResetDebtorOwnership || c == BtnAutoTag) { c.Enabled = editable; continue; }
                c.Enabled = editable;
            }
            TxtServiceItemCode.Enabled = isNew;        // code locked once saved
            BtnAutoTag.Enabled = isNew;

            // All tab pages: enable container; individual editors follow Enabled cascade.
            TabMain.Enabled = editable;

            // Header buttons (BtnAdd/BtnSearch removed during LayoutControl checkpoint)
            BtnEdit.Enabled = (_mode == FormMode.View && _serviceItemKey > 0);
            BtnSave.Enabled = editable;
            BtnCancel.Enabled = true;
            BtnDelete.Enabled = (_mode == FormMode.View && _serviceItemKey > 0);
            BtnPrint.Enabled = (_mode == FormMode.View && _serviceItemKey > 0);
            BtnAttachments.Enabled = (_mode == FormMode.View && _serviceItemKey > 0);
            BtnCopyFrom.Enabled = (_mode == FormMode.New);
            BtnGenerate.Enabled = editable;
            BtnNavFirst.Enabled = BtnNavPrev.Enabled = BtnNavNext.Enabled = BtnNavLast.Enabled = (_mode == FormMode.View);
            BtnFillTest.Enabled = editable; // dev aid — only useful while mutating
        }

        private void ResetFormToBlankNew()
        {
            _suppressDirty = true;
            try
            {
                _serviceItemKey = 0;
                _isNew = true;
                _rowVersion = null;
                TxtServiceItemCode.Text = "";
                ChkInactive.Checked = false;
                LkStockCode.EditValue = null; LblStockDesc.Text = "";
                LkDebtorCode.EditValue = null; LblDebtorName.Text = "";
                LkAgentCode.EditValue = null; LblAgentName.Text = "";
                LkTerm.EditValue = null;
                LkArea.EditValue = null;
                CmbServiceTag.EditValue = null;
                SpnUnitPrice.Value = 0;
                TxtRefNo.Text = "";
                TxtDescription.Text = "";
                LkContractNo.EditValue = null;
                DtPurchaseDate.DateTime = DateTime.Today;
                DtServiceStartDate.DateTime = DateTime.Today;
                DtServiceEndDate.DateTime = DateTime.Today;
                TxtAddress.Text = "";
                TxtAttention.Text = "";
                TxtNote.Text = "";
                TxtRemark1.Text = ""; TxtRemark2.Text = ""; TxtRemark3.Text = ""; TxtRemark4.Text = "";
                ChkPMActive.Checked = false;
                CmbPMIntervalType.Text = "NONE";
                SpnPMIntervalValue.Value = 0;
                DtPMStartDate.DateTime = DateTime.Today;
                DtPMLastServiceDate.DateTime = DateTime.Today;
                DtPMNextServiceDate.DateTime = DateTime.Today;
                LkDepartment.EditValue = null; LkJob.EditValue = null; LkLocation.EditValue = null;
                // More Header
                TxtMHCity.Text = ""; TxtMHPostalCode.Text = ""; CmbMHState.Text = ""; CmbMHCountry.Text = "";
                TxtMHPhone.Text = ""; TxtMHFax.Text = "";
                TxtMHRef1.Text = ""; TxtMHRef2.Text = ""; TxtMHRef3.Text = ""; TxtMHRef4.Text = "";
                TxtMHBranchCode.Text = ""; TxtMHBranchName.Text = ""; TxtMHAddress.Text = "";
                TxtMHDOCity.Text = ""; TxtMHDOPostalCode.Text = ""; CmbMHDOState.Text = ""; CmbMHDOCountry.Text = "";
                TxtMHDOPhone.Text = ""; TxtMHDOFax.Text = ""; TxtMHDOEmail.Text = ""; TxtMHDOContactPerson.Text = "";
                _dtMeterTypes.Clear();
                if (GridMeterHistory.DataSource is DataTable mh) mh.Clear();
                if (GridDebtorHistory.DataSource is DataTable dh) dh.Clear();
                OnAutoTag(null, EventArgs.Empty);
            }
            finally { _suppressDirty = false; _isDirty = false; }
        }

        private void WireDirtyTracking()
        {
            EventHandler markDirty = (s, e) => { if (!_suppressDirty) _isDirty = true; };
            // Header text/lookup edits
            TxtServiceItemCode.EditValueChanged += markDirty;
            ChkInactive.CheckedChanged += markDirty;
            LkStockCode.EditValueChanged += markDirty;
            LkDebtorCode.EditValueChanged += markDirty;
            LkAgentCode.EditValueChanged += markDirty;
            LkTerm.EditValueChanged += markDirty;
            LkArea.EditValueChanged += markDirty;
            CmbServiceTag.EditValueChanged += markDirty;
            SpnUnitPrice.EditValueChanged += markDirty;
            TxtRefNo.EditValueChanged += markDirty;
            TxtDescription.EditValueChanged += markDirty;
            LkContractNo.EditValueChanged += markDirty;
            DtPurchaseDate.EditValueChanged += markDirty;
            DtServiceStartDate.EditValueChanged += markDirty;
            DtServiceEndDate.EditValueChanged += markDirty;
            TxtAddress.EditValueChanged += markDirty;
            TxtAttention.EditValueChanged += markDirty;
            TxtNote.EditValueChanged += markDirty;
            TxtRemark1.EditValueChanged += markDirty;
            TxtRemark2.EditValueChanged += markDirty;
            TxtRemark3.EditValueChanged += markDirty;
            TxtRemark4.EditValueChanged += markDirty;
            ChkPMActive.CheckedChanged += markDirty;
            CmbPMIntervalType.EditValueChanged += markDirty;
            SpnPMIntervalValue.EditValueChanged += markDirty;
            DtPMStartDate.EditValueChanged += markDirty;
            DtPMLastServiceDate.EditValueChanged += markDirty;
            DtPMNextServiceDate.EditValueChanged += markDirty;
            LkDepartment.EditValueChanged += markDirty;
            LkJob.EditValueChanged += markDirty;
            LkLocation.EditValueChanged += markDirty;
            // More Header
            TxtMHCity.EditValueChanged += markDirty; TxtMHPostalCode.EditValueChanged += markDirty;
            CmbMHState.EditValueChanged += markDirty; CmbMHCountry.EditValueChanged += markDirty;
            TxtMHPhone.EditValueChanged += markDirty; TxtMHFax.EditValueChanged += markDirty;
            TxtMHRef1.EditValueChanged += markDirty; TxtMHRef2.EditValueChanged += markDirty;
            TxtMHRef3.EditValueChanged += markDirty; TxtMHRef4.EditValueChanged += markDirty;
            TxtMHBranchCode.EditValueChanged += markDirty; TxtMHBranchName.EditValueChanged += markDirty;
            TxtMHAddress.EditValueChanged += markDirty;
            TxtMHDOCity.EditValueChanged += markDirty; TxtMHDOPostalCode.EditValueChanged += markDirty;
            CmbMHDOState.EditValueChanged += markDirty; CmbMHDOCountry.EditValueChanged += markDirty;
            TxtMHDOPhone.EditValueChanged += markDirty; TxtMHDOFax.EditValueChanged += markDirty;
            TxtMHDOEmail.EditValueChanged += markDirty; TxtMHDOContactPerson.EditValueChanged += markDirty;
            // Meter Types grid
            _dtMeterTypes.RowChanged += (s, e) => { if (!_suppressDirty) _isDirty = true; };
            _dtMeterTypes.RowDeleted += (s, e) => { if (!_suppressDirty) _isDirty = true; };
        }
        private void OnAttachments(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Attachments — not yet implemented.", "Attachments", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void OnCopyFrom(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Copy From — not yet implemented.", "Copy From", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void OnAutoTag(object sender, EventArgs e)
        {
            if (_dbSetting == null) return;
            try
            {
                const string prefix = "SI-";
                const int width = 6;
                string sql =
                    "SELECT ISNULL(MAX(CAST(SUBSTRING(ServiceItemCode, " + (prefix.Length + 1) + ", " + width + ") AS INT)), 0) + 1 " +
                    "FROM [dbo].[zSCP_ServiceItem] " +
                    "WHERE ServiceItemCode LIKE '" + prefix + "%' AND LEN(ServiceItemCode) = " + (prefix.Length + width);
                object scalar = _dbSetting.ExecuteScalar(sql);
                int next = scalar == null || scalar == DBNull.Value ? 1 : Convert.ToInt32(scalar);
                TxtServiceItemCode.Text = prefix + next.ToString(new string('0', width));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Auto-generate failed:\r\n" + ex.Message, "Auto Tag", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void OnResetDebtorOwnership(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Reset Debtor Ownership — not yet implemented.", "Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void OnNavFirst(object sender, EventArgs e) { XtraMessageBox.Show("Navigate First — not yet implemented.", "Nav", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void OnNavPrev(object sender, EventArgs e)  { XtraMessageBox.Show("Navigate Previous — not yet implemented.", "Nav", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void OnNavNext(object sender, EventArgs e)  { XtraMessageBox.Show("Navigate Next — not yet implemented.", "Nav", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void OnNavLast(object sender, EventArgs e)  { XtraMessageBox.Show("Navigate Last — not yet implemented.", "Nav", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        // Tab 7 extra handlers ---------------------------------------------
        private void OnMTEdit(object sender, EventArgs e)
        {
            int rh = GridViewMT.FocusedRowHandle;
            if (rh < 0) { XtraMessageBox.Show("Select a row to edit.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            GridViewMT.FocusedColumn = GridViewMT.Columns["MeterTypeCode"];
            GridViewMT.ShowEditor();
        }
        private void OnMTTransaction(object sender, EventArgs e)
        {
            int rh = GridViewMT.FocusedRowHandle;
            if (rh < 0)
            {
                XtraMessageBox.Show("Pick a meter type row first.", "Transaction",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow row = GridViewMT.GetDataRow(rh);
            if (row == null) return;

            object simtKeyObj = row["ServiceItemMeterTypeKey"];
            if (simtKeyObj == null || simtKeyObj == DBNull.Value || Convert.ToInt64(simtKeyObj) <= 0)
            {
                XtraMessageBox.Show("Save this meter type row first — it has no key yet.",
                    "Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            long simtKey = Convert.ToInt64(simtKeyObj);
            string mtCode = row["MeterTypeCode"] != DBNull.Value ? row["MeterTypeCode"].ToString() : "";
            string mtName = row.Table.Columns.Contains("MeterTypeName") && row["MeterTypeName"] != DBNull.Value
                ? row["MeterTypeName"].ToString()
                : "";

            using (ServiceContractPhotocopier.ServiceAppointment.OperationForms.MeterTypeTransactionList_Form frm =
                new ServiceContractPhotocopier.ServiceAppointment.OperationForms.MeterTypeTransactionList_Form(
                    _dbSetting, simtKey, mtCode, mtName))
            {
                frm.ShowDialog(this);
            }
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

                if (_mode == FormMode.New) OnAutoTag(null, EventArgs.Empty); // ensures unique code

                string stock   = PickFirst(LkStockCode,  "ItemCode");
                string debtor  = PickFirst(LkDebtorCode, "DebtorCode");
                string agent   = PickFirst(LkAgentCode,  "SalesAgent");
                string term    = PickFirst(LkTerm,       "Terms");
                string area    = PickFirst(LkArea,       "AreaCode");
                string dept    = PickFirst(LkDepartment, "DeptNo");
                string loc     = PickFirst(LkLocation,   "Location");
                string grade   = PickFirstLookUp(CmbServiceTag, "ServiceItemGradeCode");

                if (!string.IsNullOrEmpty(stock))  LkStockCode.EditValue  = stock;
                if (!string.IsNullOrEmpty(debtor)) LkDebtorCode.EditValue = debtor;
                if (!string.IsNullOrEmpty(agent))  LkAgentCode.EditValue  = agent;
                if (!string.IsNullOrEmpty(term))   LkTerm.EditValue       = term;
                if (!string.IsNullOrEmpty(area))   LkArea.EditValue       = area;
                if (!string.IsNullOrEmpty(dept))   LkDepartment.EditValue = dept;
                if (!string.IsNullOrEmpty(loc))    LkLocation.EditValue   = loc;
                if (!string.IsNullOrEmpty(grade))  CmbServiceTag.EditValue = grade;

                SpnUnitPrice.Value = 1500m;
                TxtRefNo.Text = "TEST-" + stamp;
                TxtDescription.Text = "TEST Service Item (" + stamp + ")";
                DtPurchaseDate.DateTime = DateTime.Today;
                DtServiceStartDate.DateTime = DateTime.Today;
                DtServiceEndDate.DateTime = DateTime.Today.AddYears(1);
                TxtAddress.Text = "1 Test Lane\r\nTestville\r\n81200 Johor Bahru\r\nJohor";
                TxtAttention.Text = "Test Contact";
                TxtNote.Text = "Filled automatically at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                TxtRemark1.Text = "Remark 1 - " + stamp;
                TxtRemark2.Text = "Remark 2";
                TxtRemark3.Text = "Remark 3";
                TxtRemark4.Text = "Remark 4";

                ChkPMActive.Checked = true;
                CmbPMIntervalType.Text = "MONTHLY";
                SpnPMIntervalValue.Value = 3;
                DtPMStartDate.DateTime = DateTime.Today;
                DtPMLastServiceDate.DateTime = DateTime.Today;
                DtPMNextServiceDate.DateTime = DateTime.Today.AddMonths(3);

                // More Header
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
                TxtMHAddress.Text = "1 Delivery Lane";
                TxtMHDOCity.Text = "Johor Bahru";
                TxtMHDOPostalCode.Text = "81200";
                TxtMHDOPhone.Text = "07-9999999";
                TxtMHDOEmail.Text = "test@example.com";
                TxtMHDOContactPerson.Text = "Test";

                LblStockDesc.Text = LookupDescription(LkStockCode, "ItemCode", "Description");
                LblDebtorName.Text = LookupDescription(LkDebtorCode, "DebtorCode", "CompanyName");
                LblAgentName.Text = LookupDescription(LkAgentCode, "SalesAgent", "Description");

                // Tab 7 — seed one meter row if grid is empty (avoids dup-code violation on repeat clicks)
                GridViewMT.CloseEditor();
                if (_dtMeterTypes.Rows.Count == 0)
                {
                    DataRow mtSrc = FirstRow(RepoMeterTypeCode.DataSource as DataTable);
                    DataRow mpSrc = FirstRow(RepoMultiPriceCode.DataSource as DataTable);
                    if (mtSrc != null)
                    {
                        DataRow nr = _dtMeterTypes.NewRow();
                        nr["MeterTypeCode"] = mtSrc["MeterTypeCode"] ?? "";
                        if (_dtMeterTypes.Columns.Contains("MeterTypeName") && mtSrc.Table.Columns.Contains("Description"))
                            nr["MeterTypeName"] = mtSrc["Description"] ?? "";
                        if (mpSrc != null && _dtMeterTypes.Columns.Contains("MeterMultiPriceCode"))
                            nr["MeterMultiPriceCode"] = mpSrc["MeterMultiPriceCode"] ?? "";
                        nr["MinimumCharges"] = 50m;
                        nr["ChargesRate"] = 0.0285m;
                        nr["RebateQtyInPercent"] = 0m;
                        nr["FOCQty"] = 100m;
                        nr["InitialReading"] = 0m;
                        _dtMeterTypes.Rows.Add(nr);
                    }
                }
            }
            finally { _suppressDirty = false; _isDirty = true; }
        }

        private static DataRow FirstRow(DataTable dt)
        {
            return (dt != null && dt.Rows.Count > 0) ? dt.Rows[0] : null;
        }

        private static string PickFirst(DevExpress.XtraEditors.SearchLookUpEdit edit, string col)
        {
            DataTable dt = edit.Properties.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0 || !dt.Columns.Contains(col)) return "";
            return dt.Rows[0][col] == DBNull.Value ? "" : dt.Rows[0][col].ToString();
        }

        private static string PickFirstLookUp(DevExpress.XtraEditors.LookUpEdit edit, string col)
        {
            DataTable dt = edit.Properties.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0 || !dt.Columns.Contains(col)) return "";
            return dt.Rows[0][col] == DBNull.Value ? "" : dt.Rows[0][col].ToString();
        }

        // More Header tab handlers -----------------------------------------
        private void OnMHSearch(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Branch search — not yet implemented.", "Search Branch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void OnMHCopy(object sender, EventArgs e)
        {
            // Copy main (billing) address into the delivery address fields
            TxtMHAddress.Text = TxtAddress.Text;
            TxtMHDOCity.Text = TxtMHCity.Text;
            TxtMHDOPostalCode.Text = TxtMHPostalCode.Text;
            CmbMHDOState.Text = CmbMHState.Text;
            CmbMHDOCountry.Text = CmbMHCountry.Text;
            TxtMHDOPhone.Text = TxtMHPhone.Text;
            TxtMHDOFax.Text = TxtMHFax.Text;
        }
    }
}
