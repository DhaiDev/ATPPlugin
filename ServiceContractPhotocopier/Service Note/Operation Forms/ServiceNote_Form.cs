using System;
using System.Data;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.ServiceNote.OperationForms
{
    /// <summary>
    /// Service Note — matches UI/03-service-note/02-edit-with-tabs.png.
    /// Red title, toolbar, 3-col header (Date/NoteNo/Debtor/Agent/RefNo/Desc + DebtorName/Term/Area/Validity/DeliveryTerm + Address/Attention),
    /// Service Tag Information group (SvcTag/Stock/Brand + Contract/Start/End + Status/Type/Severity),
    /// 6 tabs (Main / Appointments / Charges Item / More Header / Notes / Remarks).
    /// </summary>
    public partial class ServiceNote_Form : XtraForm
    {
        private DBSetting _dbSetting;
        private long _serviceNoteKey = 0;
        private bool _isNew = true;
        private DataTable _dtChargesItems;

        public ServiceNote_Form() { InitializeComponent(); }
        public ServiceNote_Form(UserSession userSession) : this() { if (userSession != null) _dbSetting = userSession.DBSetting; this.Load += new EventHandler(OnFormLoad); }
        public ServiceNote_Form(DBSetting dbSetting) : this() { _dbSetting = dbSetting; this.Load += new EventHandler(OnFormLoad); }
        public ServiceNote_Form(DBSetting dbSetting, DataRow existing) : this()
        {
            _dbSetting = dbSetting;
            if (existing != null && existing.Table.Columns.Contains("ServiceNoteKey"))
            { _serviceNoteKey = Convert.ToInt64(existing["ServiceNoteKey"]); _isNew = false; }
            this.Load += new EventHandler(OnFormLoad);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_dbSetting == null) return;
            LoadLookups();
            _dtChargesItems = NewChargesItemsTable();
            GridChargesItems.DataSource = _dtChargesItems;
            if (!_isNew) LoadExisting();
            else
            {
                DtNoteDate.DateTime = DateTime.Now;
                DtServiceStartDate.DateTime = DateTime.Today;
                DtServiceEndDate.DateTime = DateTime.Today;
                if (CmbServiceStatus.Properties.Items.Count > 0) CmbServiceStatus.SelectedIndex = 0;
            }
        }

        private void LoadLookups()
        {
            try
            {
                Fill(CmbServiceStatus, "SELECT ServiceStatusCode FROM [dbo].[zSCP_LK_ServiceStatus] WHERE Inactive='N' ORDER BY ServiceStatusCode");
                Fill(CmbSeverity,      "SELECT ServiceSeverityCode FROM [dbo].[zSCP_LK_ServiceSeverity] WHERE Inactive='N' ORDER BY ServiceSeverityCode");
                Fill(CmbServiceType,   "SELECT ServiceTypeCode FROM [dbo].[zSCP_LK_ServiceType] WHERE Inactive='N' ORDER BY ServiceTypeCode");
                Fill(CmbProblem,       "SELECT ServiceProblemCode FROM [dbo].[zSCP_LK_ServiceProblem] WHERE Inactive='N' ORDER BY ServiceProblemCode");
                Fill(CmbSolution,      "SELECT ServiceSolutionCode FROM [dbo].[zSCP_LK_ServiceSolution] WHERE Inactive='N' ORDER BY ServiceSolutionCode");
                Fill(CmbAttendedBy,    "SELECT ServicePersonCode FROM [dbo].[zSCP_ServicePerson] WHERE Inactive='N' ORDER BY ServicePersonCode");
                Fill(CmbAssignTo,      "SELECT ServicePersonCode FROM [dbo].[zSCP_ServicePerson] WHERE Inactive='N' ORDER BY ServicePersonCode");
            }
            catch { }
        }

        private void Fill(ComboBoxEdit cmb, string sql)
        {
            var dt = _dbSetting.GetDataTable(sql, false);
            cmb.Properties.Items.Clear();
            foreach (DataRow r in dt.Rows) cmb.Properties.Items.Add(r[0].ToString());
        }

        private DataTable NewChargesItemsTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("No", typeof(int));
            dt.Columns.Add("StockCode", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("UOM", typeof(string));
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            return dt;
        }

        private void LoadExisting()
        {
            try
            {
                var dt = _dbSetting.GetDataTable("SELECT * FROM [dbo].[zSCP_ServiceNote] WHERE ServiceNoteKey = " + _serviceNoteKey, false);
                if (dt.Rows.Count == 0) { _isNew = true; return; }
                var r = dt.Rows[0];
                TxtServiceNoteNo.Text = S(r, "ServiceNoteCode");
                if (r["ServiceNoteDate"] != DBNull.Value) DtNoteDate.DateTime = Convert.ToDateTime(r["ServiceNoteDate"]);
                CmbServiceStatus.Text = S(r, "ServiceStatusCode");
                TxtServiceItemCode.Text = S(r, "ServiceItemCode");
                CmbServiceType.Text = S(r, "ServiceTypeCode");
                CmbSeverity.Text = S(r, "ServiceSeverityCode");
                CmbProblem.Text = S(r, "ServiceProblemCode");
                CmbSolution.Text = S(r, "ServiceSolutionCode");
                CmbAttendedBy.Text = S(r, "AttendedServicePersonCode");
                CmbAssignTo.Text = S(r, "AssignToServicePersonCode");
                TxtDebtorCode.Text = S(r, "DebtorCode");
                TxtDebtorName.Text = S(r, "DebtorName");
                TxtAgentCode.Text = S(r, "StaffCode");
                TxtTermCode.Text = S(r, "TermCode");
                TxtAreaCode.Text = S(r, "AreaCode");
                TxtRefNo.Text = S(r, "RefNo");
                TxtDescription.Text = S(r, "Description");
                TxtValidity.Text = S(r, "Validity");
                TxtDeliveryTerm.Text = S(r, "DeliveryTerm");
                TxtAddress.Text = S(r, "Address1") + "\r\n" + S(r, "Address2") + "\r\n" + S(r, "Address3") + "\r\n" + S(r, "Address4");
                TxtAttention.Text = S(r, "Attention");
                TxtContractNo.Text = S(r, "ContractNo");
                if (r["ServiceStartDate"] != DBNull.Value) DtServiceStartDate.DateTime = Convert.ToDateTime(r["ServiceStartDate"]);
                if (r["ServiceExpiryDate"] != DBNull.Value) DtServiceEndDate.DateTime = Convert.ToDateTime(r["ServiceExpiryDate"]);
                TxtProblemRemark.Text = r["ServiceProblemRemark"] == DBNull.Value ? "" : r["ServiceProblemRemark"].ToString();
                TxtSolutionRemark.Text = r["ServiceSolutionRemark"] == DBNull.Value ? "" : r["ServiceSolutionRemark"].ToString();
                TxtNote.Text = r["Note"] == DBNull.Value ? "" : r["Note"].ToString();
                TxtRemark1.Text = S(r, "Remark1"); TxtRemark2.Text = S(r, "Remark2");
                TxtRemark3.Text = S(r, "Remark3"); TxtRemark4.Text = S(r, "Remark4");
                ChkClosed.Checked = S(r, "Closed") == "Y";

                var dtItems = _dbSetting.GetDataTable("SELECT Pos, StockCode, [Description], UOM, Qty, UnitPrice, Amount FROM [dbo].[zSCP_ServiceNoteDTL] WHERE ServiceNoteKey = " + _serviceNoteKey + " ORDER BY Pos", false);
                _dtChargesItems.Clear();
                int n = 1;
                foreach (DataRow row in dtItems.Rows)
                {
                    var nr = _dtChargesItems.NewRow();
                    nr["No"] = n++;
                    nr["StockCode"] = row["StockCode"];
                    nr["Description"] = row["Description"];
                    nr["UOM"] = row["UOM"];
                    nr["Qty"] = row["Qty"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Qty"]);
                    nr["UnitPrice"] = row["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["UnitPrice"]);
                    nr["Amount"] = row["Amount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Amount"]);
                    _dtChargesItems.Rows.Add(nr);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Failed to load service note:\r\n" + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string S(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col)) return "";
            return r[col] == DBNull.Value ? "" : r[col].ToString();
        }

        private void OnSave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtServiceNoteNo.Text))
            { XtraMessageBox.Show("Service Note No is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(TxtDebtorCode.Text))
            { XtraMessageBox.Show("Debtor Code is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                string code = SQLString(TxtServiceNoteNo.Text.Trim());
                string status = SQLString(CmbServiceStatus.Text ?? "OPEN");
                string item = SQLString(TxtServiceItemCode.Text ?? "");
                string typ = SQLString(CmbServiceType.Text ?? "");
                string sev = SQLString(CmbSeverity.Text ?? "");
                string prob = SQLString(CmbProblem.Text ?? "");
                string sol = SQLString(CmbSolution.Text ?? "");
                string attd = SQLString(CmbAttendedBy.Text ?? "");
                string asgn = SQLString(CmbAssignTo.Text ?? "");
                string debtor = SQLString(TxtDebtorCode.Text.Trim());
                string debtorName = SQLString(TxtDebtorName.Text ?? "");
                string agent = SQLString(TxtAgentCode.Text ?? "");
                string term = SQLString(TxtTermCode.Text ?? "");
                string area = SQLString(TxtAreaCode.Text ?? "");
                string refNo = SQLString(TxtRefNo.Text ?? "");
                string desc = SQLString(TxtDescription.Text ?? "");
                string valid = SQLString(TxtValidity.Text ?? "");
                string delTerm = SQLString(TxtDeliveryTerm.Text ?? "");
                string attn = SQLString(TxtAttention.Text ?? "");
                string contract = SQLString(TxtContractNo.Text ?? "");
                string probRemark = SQLString(TxtProblemRemark.Text ?? "");
                string solRemark = SQLString(TxtSolutionRemark.Text ?? "");
                string note = SQLString(TxtNote.Text ?? "");
                string r1 = SQLString(TxtRemark1.Text ?? ""), r2 = SQLString(TxtRemark2.Text ?? ""), r3 = SQLString(TxtRemark3.Text ?? ""), r4 = SQLString(TxtRemark4.Text ?? "");
                string closed = ChkClosed.Checked ? "Y" : "N";
                string dtNote = "'" + DtNoteDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                string dtStart = "'" + DtServiceStartDate.DateTime.ToString("yyyy-MM-dd") + "'";
                string dtEnd = "'" + DtServiceEndDate.DateTime.ToString("yyyy-MM-dd") + "'";

                string[] addr = (TxtAddress.Text ?? "").Replace("\r\n", "\n").Split('\n');
                string a1 = SQLString(addr.Length > 0 ? addr[0] : "");
                string a2 = SQLString(addr.Length > 1 ? addr[1] : "");
                string a3 = SQLString(addr.Length > 2 ? addr[2] : "");
                string a4 = SQLString(addr.Length > 3 ? addr[3] : "");

                string sql;
                if (_isNew)
                {
                    sql = "INSERT INTO [dbo].[zSCP_ServiceNote] " +
                        "(ServiceNoteCode, ServiceNoteDate, ServiceStatusCode, ServiceItemCode, ContractNo, ServiceTypeCode, " +
                        " ServiceSeverityCode, ServiceProblemCode, ServiceProblemRemark, ServiceSolutionCode, ServiceSolutionRemark, " +
                        " AttendedServicePersonCode, AssignToServicePersonCode, DebtorCode, DebtorName, TermCode, StaffCode, AreaCode, " +
                        " [Description], Validity, DeliveryTerm, RefNo, Address1, Address2, Address3, Address4, Attention, " +
                        " ServiceStartDate, ServiceExpiryDate, Note, Remark1, Remark2, Remark3, Remark4, Closed, Created, Modified) VALUES " +
                        "(N'" + code + "', " + dtNote + ", N'" + status + "', N'" + item + "', N'" + contract + "', N'" + typ + "', " +
                        "N'" + sev + "', N'" + prob + "', N'" + probRemark + "', N'" + sol + "', N'" + solRemark + "', " +
                        "N'" + attd + "', N'" + asgn + "', N'" + debtor + "', N'" + debtorName + "', N'" + term + "', N'" + agent + "', N'" + area + "', " +
                        "N'" + desc + "', N'" + valid + "', N'" + delTerm + "', N'" + refNo + "', " +
                        "N'" + a1 + "', N'" + a2 + "', N'" + a3 + "', N'" + a4 + "', N'" + attn + "', " +
                        dtStart + ", " + dtEnd + ", N'" + note + "', N'" + r1 + "', N'" + r2 + "', N'" + r3 + "', N'" + r4 + "', '" + closed + "', GETDATE(), GETDATE()); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
                    var key = _dbSetting.ExecuteScalar(sql);
                    _serviceNoteKey = Convert.ToInt64(key);
                    _isNew = false;
                }
                else
                {
                    sql = "UPDATE [dbo].[zSCP_ServiceNote] SET " +
                        "ServiceNoteCode=N'" + code + "', ServiceNoteDate=" + dtNote + ", ServiceStatusCode=N'" + status + "', " +
                        "ServiceItemCode=N'" + item + "', ContractNo=N'" + contract + "', ServiceTypeCode=N'" + typ + "', " +
                        "ServiceSeverityCode=N'" + sev + "', ServiceProblemCode=N'" + prob + "', ServiceProblemRemark=N'" + probRemark + "', " +
                        "ServiceSolutionCode=N'" + sol + "', ServiceSolutionRemark=N'" + solRemark + "', " +
                        "AttendedServicePersonCode=N'" + attd + "', AssignToServicePersonCode=N'" + asgn + "', " +
                        "DebtorCode=N'" + debtor + "', DebtorName=N'" + debtorName + "', TermCode=N'" + term + "', StaffCode=N'" + agent + "', AreaCode=N'" + area + "', " +
                        "[Description]=N'" + desc + "', Validity=N'" + valid + "', DeliveryTerm=N'" + delTerm + "', RefNo=N'" + refNo + "', " +
                        "Address1=N'" + a1 + "', Address2=N'" + a2 + "', Address3=N'" + a3 + "', Address4=N'" + a4 + "', Attention=N'" + attn + "', " +
                        "ServiceStartDate=" + dtStart + ", ServiceExpiryDate=" + dtEnd + ", Note=N'" + note + "', " +
                        "Remark1=N'" + r1 + "', Remark2=N'" + r2 + "', Remark3=N'" + r3 + "', Remark4=N'" + r4 + "', " +
                        "Closed='" + closed + "', Modified=GETDATE(), LastModified=GETDATE() " +
                        "WHERE ServiceNoteKey=" + _serviceNoteKey;
                    _dbSetting.ExecuteNonQuery(sql);
                }

                _dbSetting.ExecuteNonQuery("DELETE FROM [dbo].[zSCP_ServiceNoteDTL] WHERE ServiceNoteKey=" + _serviceNoteKey);
                int pos = 1;
                foreach (DataRow row in _dtChargesItems.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    string sc = SQLString(row["StockCode"] == DBNull.Value ? "" : row["StockCode"].ToString());
                    if (string.IsNullOrEmpty(sc)) continue;
                    string sd = SQLString(row["Description"] == DBNull.Value ? "" : row["Description"].ToString());
                    string uom = SQLString(row["UOM"] == DBNull.Value ? "" : row["UOM"].ToString());
                    decimal q = row["Qty"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Qty"]);
                    decimal up = row["UnitPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(row["UnitPrice"]);
                    decimal amt = row["Amount"] == DBNull.Value ? (q * up) : Convert.ToDecimal(row["Amount"]);
                    _dbSetting.ExecuteNonQuery("INSERT INTO [dbo].[zSCP_ServiceNoteDTL] " +
                        "(ServiceNoteKey, ServiceNoteCode, Pos, StockCode, [Description], UOM, Qty, UnitPrice, Amount) VALUES (" +
                        _serviceNoteKey + ", N'" + code + "', " + pos + ", N'" + sc + "', N'" + sd + "', N'" + uom + "', " +
                        q.ToString("0.000000") + ", " + up.ToString("0.000000") + ", " + amt.ToString("0.00") + ")");
                    pos++;
                }

                XtraMessageBox.Show("Saved.", "Service Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Save failed:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnCancel(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
        private void OnExit(object sender, EventArgs e) { this.Close(); }
    }
}
