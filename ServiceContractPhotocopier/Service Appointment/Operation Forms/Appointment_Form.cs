using System;
using System.Data;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    /// <summary>Edit a single appointment row — referenced from AppointmentCalendar_Form / AppointmentLst_Form.</summary>
    public partial class Appointment_Form : XtraForm
    {
        private DBSetting _dbSetting;
        private long _appointmentKey = 0;
        private bool _isNew = true;

        public Appointment_Form() { InitializeComponent(); }
        public Appointment_Form(UserSession userSession) : this() { if (userSession != null) _dbSetting = userSession.DBSetting; this.Load += new EventHandler(OnFormLoad); }
        public Appointment_Form(DBSetting dbSetting) : this() { _dbSetting = dbSetting; this.Load += new EventHandler(OnFormLoad); }
        public Appointment_Form(DBSetting dbSetting, DataRow existing) : this()
        {
            _dbSetting = dbSetting;
            if (existing != null && existing.Table.Columns.Contains("AppointmentKey"))
            { _appointmentKey = Convert.ToInt64(existing["AppointmentKey"]); _isNew = false; }
            this.Load += new EventHandler(OnFormLoad);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_dbSetting == null) return;
            LoadLookups();
            if (!_isNew) LoadExisting();
            else
            {
                DtStart.DateTime = DateTime.Now;
                DtFinish.DateTime = DateTime.Now.AddHours(1);
            }
        }

        private void LoadLookups()
        {
            try
            {
                Fill(CmbType, "SELECT AppointmentTypeCode FROM [dbo].[zSCP_LK_AppointmentType] WHERE Inactive='N' ORDER BY AppointmentTypeCode");
                Fill(CmbPriority, "SELECT AppointmentPriorityCode FROM [dbo].[zSCP_LK_AppointmentPriority] WHERE Inactive='N' ORDER BY AppointmentPriorityCode");
                Fill(CmbServicePerson, "SELECT ServicePersonCode FROM [dbo].[zSCP_ServicePerson] WHERE Inactive='N' ORDER BY ServicePersonCode");
            }
            catch { }
        }

        private void Fill(ComboBoxEdit cmb, string sql)
        {
            var dt = _dbSetting.GetDataTable(sql, false);
            cmb.Properties.Items.Clear();
            foreach (DataRow r in dt.Rows) cmb.Properties.Items.Add(r[0].ToString());
        }

        private void LoadExisting()
        {
            try
            {
                var dt = _dbSetting.GetDataTable("SELECT * FROM [dbo].[zSCP_Appointment] WHERE AppointmentKey = " + _appointmentKey, false);
                if (dt.Rows.Count == 0) { _isNew = true; return; }
                var r = dt.Rows[0];
                TxtSubject.Text = S(r, "Subject");
                if (r["StartTime"] != DBNull.Value) DtStart.DateTime = Convert.ToDateTime(r["StartTime"]);
                if (r["FinishTime"] != DBNull.Value) DtFinish.DateTime = Convert.ToDateTime(r["FinishTime"]);
                CmbType.Text = S(r, "AppointmentTypeCode");
                CmbPriority.Text = S(r, "AppointmentPriorityCode");
                CmbServicePerson.Text = S(r, "ServicePersonCode");
                TxtDebtorCode.Text = S(r, "DebtorCode");
                TxtContactPerson.Text = S(r, "ContactPerson");
                TxtStaffCode.Text = S(r, "StaffCode");
                TxtAreaCode.Text = S(r, "AreaCode");
                TxtDescription.Text = S(r, "Description");
                TxtMessage.Text = r["Message"] == DBNull.Value ? "" : r["Message"].ToString();
                ChkDone.Checked = S(r, "Done") == "Y";
                TxtAddress.Text = S(r, "Address1") + "\r\n" + S(r, "Address2") + "\r\n" + S(r, "Address3") + "\r\n" + S(r, "Address4");
            }
            catch (Exception ex) { XtraMessageBox.Show("Load failed:\r\n" + ex.Message, "Error"); }
        }

        private static string S(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col)) return "";
            return r[col] == DBNull.Value ? "" : r[col].ToString();
        }

        private void OnSave(object sender, EventArgs e)
        {
            try
            {
                string subj = SQLString(TxtSubject.Text ?? "");
                string typ = SQLString(CmbType.Text ?? "");
                string pri = SQLString(CmbPriority.Text ?? "");
                string per = SQLString(CmbServicePerson.Text ?? "");
                string debtor = SQLString(TxtDebtorCode.Text ?? "");
                string contact = SQLString(TxtContactPerson.Text ?? "");
                string staff = SQLString(TxtStaffCode.Text ?? "");
                string area = SQLString(TxtAreaCode.Text ?? "");
                string desc = SQLString(TxtDescription.Text ?? "");
                string msg = SQLString(TxtMessage.Text ?? "");
                string done = ChkDone.Checked ? "Y" : "N";
                string dStart = "'" + DtStart.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                string dFinish = "'" + DtFinish.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                string[] addr = (TxtAddress.Text ?? "").Replace("\r\n", "\n").Split('\n');
                string a1 = SQLString(addr.Length > 0 ? addr[0] : "");
                string a2 = SQLString(addr.Length > 1 ? addr[1] : "");
                string a3 = SQLString(addr.Length > 2 ? addr[2] : "");
                string a4 = SQLString(addr.Length > 3 ? addr[3] : "");

                string sql;
                if (_isNew)
                {
                    sql = "INSERT INTO [dbo].[zSCP_Appointment] " +
                        "(Subject, StartTime, FinishTime, AppointmentTypeCode, AppointmentPriorityCode, ServicePersonCode, " +
                        " DebtorCode, ContactPerson, StaffCode, AreaCode, [Description], [Message], " +
                        " Address1, Address2, Address3, Address4, Done) VALUES " +
                        "(N'" + subj + "', " + dStart + ", " + dFinish + ", N'" + typ + "', N'" + pri + "', N'" + per + "', " +
                        "N'" + debtor + "', N'" + contact + "', N'" + staff + "', N'" + area + "', N'" + desc + "', N'" + msg + "', " +
                        "N'" + a1 + "', N'" + a2 + "', N'" + a3 + "', N'" + a4 + "', '" + done + "'); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
                    var key = _dbSetting.ExecuteScalar(sql);
                    _appointmentKey = Convert.ToInt64(key);
                    _isNew = false;
                }
                else
                {
                    sql = "UPDATE [dbo].[zSCP_Appointment] SET " +
                        "Subject=N'" + subj + "', StartTime=" + dStart + ", FinishTime=" + dFinish + ", " +
                        "AppointmentTypeCode=N'" + typ + "', AppointmentPriorityCode=N'" + pri + "', ServicePersonCode=N'" + per + "', " +
                        "DebtorCode=N'" + debtor + "', ContactPerson=N'" + contact + "', StaffCode=N'" + staff + "', AreaCode=N'" + area + "', " +
                        "[Description]=N'" + desc + "', [Message]=N'" + msg + "', " +
                        "Address1=N'" + a1 + "', Address2=N'" + a2 + "', Address3=N'" + a3 + "', Address4=N'" + a4 + "', Done='" + done + "' " +
                        "WHERE AppointmentKey=" + _appointmentKey;
                    _dbSetting.ExecuteNonQuery(sql);
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { XtraMessageBox.Show("Save failed:\r\n" + ex.Message, "Error"); }
        }

        private void OnCancel(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
    }
}
