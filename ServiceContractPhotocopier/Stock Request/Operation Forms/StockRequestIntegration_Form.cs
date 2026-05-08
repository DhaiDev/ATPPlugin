using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AutoCount.Data;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.StockRequest.OperationForms
{
    /// <summary>
    /// UI clone of https://atgroup.asia/stockStandby/ ("ALL STANDBY STOCKS").
    /// Currently dummy data only — no live AutoCount stock movement.
    /// The "Perform Stock Movement" button is a stub that summarizes selected rows.
    /// </summary>
    public partial class StockRequestIntegration_Form : XtraForm
    {
        private DBSetting _dbSetting;
        private DataTable _data;

        // Technician list (exact spelling/case)
        private static readonly string[] Technicians = new string[]
        {
            "AMIRUL", "AUTOCOUNT", "AZRI", "CANON MARKETING (MALAYSIA) SDN BHD", "CHIAM JIA CHIN",
            "CPS ADVANCE SOLUTION", "DEDICATED OFFICE SOLUTION", "FAUZAN", "Hafizudin", "HAZIQ",
            "ILYAS", "IRWAN", "ISMAIL", "MIKE", "PENDING TASK", "Rahman", "RAZALI", "REDHA", "SAFUAN",
            "Saifudin", "SAN KHAN", "TIOMAN", "WAN MOHAMMAD NASIHI", "Zakaria", "ZULKARNAIN"
        };

        public StockRequestIntegration_Form()
        {
            InitializeComponent();
            BuildDummyData();
            WireUp();
        }

        public StockRequestIntegration_Form(DBSetting dbSetting) : this()
        {
            _dbSetting = dbSetting;
        }

        private void WireUp()
        {
            // Default date range: last 90 days
            DtDateFrom.EditValue = DateTime.Today.AddDays(-90);
            DtDateTo.EditValue = DateTime.Today;

            // Technician combo
            CmbTechnician.Properties.Items.Clear();
            CmbTechnician.Properties.Items.Add("(All)");
            foreach (string t in Technicians)
            {
                CmbTechnician.Properties.Items.Add(t);
            }
            CmbTechnician.SelectedIndex = 0;

            // Filter mutually-exclusive-ish behaviour for "Is Collected" and "Approval"
            // is not enforced — they're independent checkboxes per spec.

            // Bind grid
            Grid.DataSource = _data;

            // Update selection count when checkbox toggles or data changes
            GridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(OnCellValueChanged);
            GridView.RowCountChanged += new EventHandler(OnRowCountChanged);

            // Buttons
            BtnRefresh.Click += new EventHandler(OnRefresh);
            BtnExportCsv.Click += new EventHandler(OnExportCsv);
            BtnApplyFilter.Click += new EventHandler(OnApplyFilter);
            BtnResetAll.Click += new EventHandler(OnResetAll);
            BtnPerformMovement.Click += new EventHandler(OnPerformMovement);

            UpdateSelectionLabel();
        }

        private void BuildDummyData()
        {
            _data = new DataTable("StockRequest");
            _data.Columns.Add("Sel", typeof(bool));
            _data.Columns.Add("ID", typeof(int));
            _data.Columns.Add("PID", typeof(string));
            _data.Columns.Add("Date", typeof(DateTime));
            _data.Columns.Add("Tech", typeof(string));
            _data.Columns.Add("PartsSupplies", typeof(string));
            _data.Columns.Add("ReqQty", typeof(decimal));
            _data.Columns.Add("Qty", typeof(decimal));
            _data.Columns.Add("Balance", typeof(decimal));
            _data.Columns.Add("Type", typeof(string));
            _data.Columns.Add("Unit", typeof(string));
            _data.Columns.Add("Color", typeof(string));
            _data.Columns.Add("IsCollected", typeof(string));
            _data.Columns.Add("CollectedAt", typeof(string));
            _data.Columns.Add("Approval", typeof(string));
            _data.Columns.Add("Remark", typeof(string));
            _data.Columns.Add("TicketID", typeof(string));

            AddRow(50642, "",       "2026-05-08 12:45:00", "Rahman",   "FM1-T767-010000",   "LIFTER DRIVE ASSEMBLY",                                       1m,  1m,  1m,  "IN",  "UNIT", "",  "No",                  "",              "Waiting Approval", "Click to edit",                       "");
            AddRow(50639, "47579",  "2026-05-08 11:58:58", "FAUZAN",   "7045393",           "FOAM",                                                        0m, -2m, -2m,  "OUT", "PCS",  "",  "Yes",                 "",              "",                 "Click to edit",                       "RPS-260508-013");
            AddRow(50640, "47580",  "2026-05-08 11:58:58", "FAUZAN",   "1070028009",        "MUFFLER FAN CPL",                                             0m, -1m, -1m,  "OUT", "PCS",  "",  "Yes",                 "",              "",                 "Click to edit",                       "RPS-260508-013");
            AddRow(50641, "47576",  "2026-05-08 11:58:58", "FAUZAN",   "TC001",             "1060032721 GRIP",                                             0m, -2m, -2m,  "OUT", "PCS",  "",  "Yes",                 "",              "",                 "Click to edit",                       "RPS-260508-013");
            AddRow(50638, "36334",  "2026-05-08 11:26:17", "REDHA",    "NPG73C T",          "TONER B/W - IRADV4525/35/4725/35/45/51-COMPACTIBLE",          0m, -1m, -1m,  "OUT", "UNIT", "B", "Yes",                 "",              "",                 "Click to edit",                       "MR-260407-268");
            AddRow(50637, "50490",  "2026-05-08 11:23:30", "ISMAIL",   "EXV51C T/BK",       "TONER BK - IRADVC5535i/5550i/5560i (800GM)",                  0m, -1m, -1m,  "OUT", "TUBE", "B", "Yes",                 "",              "",                 "Click to edit",                       "RPS-260504-043");
            AddRow(50636, "50418",  "2026-05-08 11:09:03", "Saifudin", "NPG-1001",          "TONER BK - JP/OTH",                                           0m, -1m, -1m,  "OUT", "TUBE", "B", "Yes",                 "",              "",                 "MBJB",                                "UP-260410-056");
            AddRow(50635, "50137",  "2026-05-08 10:55:10", "Rahman",   "NPG45/46 D (FUJI)", "DRUM OPC IRADV5030/35/45/51 - COMPATIBLE",                    0m, -1m, -1m,  "OUT", "PCS",  "",  "Yes",                 "",              "",                 "CHANGE CODE FROM (CHI) TO (FUJI)",    "MR-260507-012");
            AddRow(50634, "50522",  "2026-05-08 10:25:17", "Saifudin", "NPG-1001 Y",        "TONER Y - JP/OTH",                                            0m, -1m, -1m,  "OUT", "TUBE", "Y", "Yes",                 "",              "",                 "MBJB Projek Bangunan L11",            "PM-260506-302");
            AddRow(50633, "50418",  "2026-05-08 10:16:52", "Saifudin", "NPG-1001",          "TONER BK - JP/OTH",                                           0m, -1m, -1m,  "OUT", "TUBE", "B", "Yes",                 "",              "",                 "MBJB",                                "PM-260508-016");
            AddRow(50632, "50418",  "2026-05-08 10:00:59", "Saifudin", "NPG-1001",          "TONER BK - JP/OTH",                                           0m, -1m, -1m,  "OUT", "TUBE", "B", "Yes",                 "",              "",                 "MBJB",                                "RPS-260507-378");
            AddRow(50631, "49717",  "2026-05-08 09:56:19", "Rahman",   "NPG88C T/BK",       "TONER BK - IRADVDXC3935i/C3930i/C3926i/C3922i - 700GM W/CHIP",0m, -1m, -1m,  "OUT", "TUBE", "B", "Yes",                 "",              "",                 "Click to edit",                       "SN-260508-008");
            AddRow(50630, "",       "2026-05-08 09:16:16", "Rahman*",  "NPG88C T/C",        "TONER C - IRADVDXC3935i - 400GM WITH CHIP",                   0m,  2m,  2m,  "IN",  "TUBE", "C", "Waiting for Collect", "Yes by INTERN", "",                 "CHANGE CODE TO COMPACT",              "");
            AddRow(50629, "",       "2026-05-08 09:15:44", "Rahman*",  "NPG88C T/M",        "TONER M - IRADVDXC3935i - 400GM WITH CHIP",                   0m,  2m,  2m,  "IN",  "TUBE", "M", "Waiting for Collect", "Yes by INTERN", "",                 "CHANGE CODE TO COMPACT",              "");
            AddRow(50628, "",       "2026-05-08 09:15:16", "Rahman*",  "NPG88C T/Y",        "TONER Y - IRADVDXC3935i - 400GM WITH CHIP",                   0m,  2m,  2m,  "IN",  "TUBE", "Y", "Waiting for Collect", "Yes by INTERN", "",                 "CHANGE CODE TO COMPACT",              "");
            AddRow(50627, "",       "2026-05-08 09:02:23", "Rahman",   "NPG88 T/Y",         "TONER Y - IRADVDXC3935i",                                     2m,  2m,  2m,  "IN",  "UNIT", "Y", "No",                  "No by INTERN",  "",                 "CHANGE CODE",                         "");
            AddRow(50626, "",       "2026-05-08 09:02:08", "Rahman",   "NPG88 T/M",         "TONER M - IRADVDXC3935i",                                     2m,  2m,  2m,  "IN",  "UNIT", "M", "No",                  "No by INTERN",  "",                 "CHANGE CODE",                         "");
            AddRow(50625, "",       "2026-05-08 09:02:04", "Rahman",   "NPG88 T/C",         "TONER C - IRADVDXC3935i",                                     2m,  2m,  2m,  "IN",  "UNIT", "C", "No",                  "No by INTERN",  "",                 "CHANGE CODE",                         "");
            AddRow(50624, "",       "2026-05-08 09:01:59", "Rahman",   "NPG88 T/BK",        "TONER BK - IRADVDXC3935i",                                    3m,  3m,  3m,  "IN",  "UNIT", "B", "No",                  "No by INTERN",  "",                 "NO STOCK ORI AND COMPACT",            "");
            AddRow(50623, "50343",  "2026-05-07 19:36:41", "ILYAS",    "T06/T BK",          "TONER BK-IR1643i/ 1643iF",                                    0m, -1m, -1m,  "OUT", "UNIT", "B", "Yes",                 "",              "",                 "Click to edit",                       "RPS-260430-054");
            AddRow(50622, "49675",  "2026-05-07 18:01:38", "Hafizudin","CB/NPG46 (TONE)",   "CLEANING BLADE IRADV5030/5035 W SEAL - COMPATIBLE",           0m, -5m, -5m,  "OUT", "PCS",  "",  "Yes by Zira",         "",              "",                 "Return",                              "");
            AddRow(50621, "",       "2026-05-07 17:31:05", "SAFUAN*",  "NPG88C T/C",        "TONER C - IRADVDXC3935i - 400GM WITH CHIP",                   0m,  3m,  3m,  "IN",  "TUBE", "C", "Waiting for Collect", "Yes by INTERN", "",                 "CHANGE CODE FROM (PKT) TO (TUBE)",    "");
            AddRow(50620, "50604",  "2026-05-07 17:30:08", "SAFUAN",   "NPG88C T/BK",       "TONER BK - IRADVDXC3935i - 700GM WITH CHIP",                  0m,  1m,  1m,  "IN",  "TUBE", "B", "Waiting for Collect", "Yes by INTERN", "",                 "Click to edit",                       "");
            AddRow(50619, "48901",  "2026-05-07 17:28:28", "SAFUAN",   "NPG89 T/BK",        "TONER BK - IRADVDX4945i/4935i/4925i",                         0m, -1m, -1m,  "OUT", "PCS",  "B", "Yes",                 "",              "",                 "Click to edit",                       "SN-260507-376");
            AddRow(50613, "50510",  "2026-05-07 16:49:17", "REDHA",    "NPG46C T/BK",       "TONER BK - IRADVC5030/5035/5235/5240",                        0m, -1m, -1m,  "OUT", "TUBE", "B", "Yes",                 "",              "",                 "Click to edit",                       "RPS-260507-300");
        }

        private void AddRow(int id, string pid, string dateStr, string tech, string code, string desc,
            decimal reqQty, decimal qty, decimal balance, string type, string unit, string color,
            string isCollected, string collectedAt, string approval, string remark, string ticket)
        {
            DataRow r = _data.NewRow();
            r["Sel"] = false;
            r["ID"] = id;
            r["PID"] = pid;
            r["Date"] = DateTime.Parse(dateStr, System.Globalization.CultureInfo.InvariantCulture);
            r["Tech"] = tech;
            // Code on first line, description in brackets on second
            r["PartsSupplies"] = code + System.Environment.NewLine + "(" + desc + ")";
            r["ReqQty"] = reqQty;
            r["Qty"] = qty;
            r["Balance"] = balance;
            r["Type"] = type;
            r["Unit"] = unit;
            r["Color"] = color;
            r["IsCollected"] = isCollected;
            r["CollectedAt"] = collectedAt;
            r["Approval"] = approval;
            r["Remark"] = remark;
            r["TicketID"] = ticket;
            _data.Rows.Add(r);
        }

        private int CountSelected()
        {
            int n = 0;
            foreach (DataRow r in _data.Rows)
            {
                if (r["Sel"] != DBNull.Value && (bool)r["Sel"]) n++;
            }
            return n;
        }

        private void UpdateSelectionLabel()
        {
            int total = _data == null ? 0 : _data.Rows.Count;
            int sel = CountSelected();
            LblSelectionCount.Text = "Selected: " + sel + " of " + total;
        }

        private void OnCellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column == ColSel) UpdateSelectionLabel();
        }

        private void OnRowCountChanged(object sender, EventArgs e)
        {
            UpdateSelectionLabel();
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            // No-op: dummy data only.
            XtraMessageBox.Show("Refresh: dummy data only — no live source connected yet.",
                "Stock Request Integration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnExportCsv(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Export CSV: not implemented in this UI proof.",
                "Stock Request Integration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnApplyFilter(object sender, EventArgs e)
        {
            // For the proof: no real filter, just refresh selection label.
            UpdateSelectionLabel();
        }

        private void OnResetAll(object sender, EventArgs e)
        {
            DtDateFrom.EditValue = DateTime.Today.AddDays(-90);
            DtDateTo.EditValue = DateTime.Today;
            CmbTechnician.SelectedIndex = 0;
            TxtPartsSearch.Text = string.Empty;
            TxtTicketSearch.Text = string.Empty;
            ChkCollWaiting.Checked = false;
            ChkCollYes.Checked = false;
            ChkCollNo.Checked = false;
            ChkCollAll.Checked = true;
            ChkApprWaiting.Checked = false;
            ChkApprYes.Checked = false;
            ChkApprNo.Checked = false;
            ChkApprBackorder.Checked = false;
            foreach (DataRow r in _data.Rows) r["Sel"] = false;
            GridView.RefreshData();
            UpdateSelectionLabel();
        }

        private void OnPerformMovement(object sender, EventArgs e)
        {
            int count = CountSelected();
            if (count == 0)
            {
                XtraMessageBox.Show("No rows selected.", "Stock Request Integration",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataRow r in _data.Rows)
            {
                if (r["Sel"] == DBNull.Value || !(bool)r["Sel"]) continue;
                int id = (int)r["ID"];
                string tech = r["Tech"] == DBNull.Value ? "" : r["Tech"].ToString();
                string type = r["Type"] == DBNull.Value ? "" : r["Type"].ToString();
                decimal qty = r["Qty"] == DBNull.Value ? 0m : (decimal)r["Qty"];
                string action;
                if (qty < 0m)
                {
                    action = "Stock Transfer " + tech + " -> HQ (return)";
                }
                else if (string.Equals(type, "IN", StringComparison.OrdinalIgnoreCase))
                {
                    action = "Stock Transfer HQ -> " + tech;
                }
                else
                {
                    action = "Stock Issue from " + tech;
                }
                sb.Append("  #").Append(id).Append("  ").Append(action)
                  .Append("  qty=").Append(qty.ToString("0.0000"))
                  .AppendLine();
            }

            XtraMessageBox.Show(
                "Would perform stock movement for " + count + " row(s):" +
                System.Environment.NewLine + System.Environment.NewLine + sb.ToString(),
                "Stock Request Integration (stub)",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
