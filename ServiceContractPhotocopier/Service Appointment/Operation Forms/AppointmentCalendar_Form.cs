using System;
using System.Data;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    /// <summary>
    /// Service Appointments — matches UI/05-service-appointment/01-calendar-view.png.
    /// Shows all appointments in a grid grouped by service person with date range filters.
    /// </summary>
    public partial class AppointmentCalendar_Form : XtraForm
    {
        private DBSetting _dbSetting;

        public AppointmentCalendar_Form() { InitializeComponent(); }
        public AppointmentCalendar_Form(UserSession userSession) : this() { if (userSession != null) _dbSetting = userSession.DBSetting; this.Load += new EventHandler(OnFormLoad); }
        public AppointmentCalendar_Form(DBSetting dbSetting) : this() { _dbSetting = dbSetting; this.Load += new EventHandler(OnFormLoad); }

        private void OnFormLoad(object sender, EventArgs e)
        {
            DtFrom.DateTime = DateTime.Today;
            DtTo.DateTime = DateTime.Today.AddDays(30);
            if (_dbSetting == null) return;
            try
            {
                var dtPer = _dbSetting.GetDataTable("SELECT ServicePersonCode FROM [dbo].[zSCP_ServicePerson] WHERE Inactive='N' ORDER BY ServicePersonCode", false);
                CmbServicePerson.Properties.Items.Clear();
                CmbServicePerson.Properties.Items.Add("(All)");
                foreach (DataRow r in dtPer.Rows) CmbServicePerson.Properties.Items.Add(r[0].ToString());
                CmbServicePerson.SelectedIndex = 0;
                LoadAppointments();
            }
            catch { }
        }

        private void LoadAppointments()
        {
            try
            {
                string where = "StartTime >= '" + DtFrom.DateTime.ToString("yyyy-MM-dd") + "' AND StartTime <= '" + DtTo.DateTime.ToString("yyyy-MM-dd") + " 23:59:59'";
                if (!string.IsNullOrEmpty(CmbServicePerson.Text) && CmbServicePerson.Text != "(All)")
                    where += " AND ServicePersonCode = N'" + CmbServicePerson.Text.Replace("'", "''") + "'";
                string sql = "SELECT * FROM [dbo].[zvSCP_AppointmentCalendar] WHERE " + where + " ORDER BY StartTime";
                GridAppointments.DataSource = _dbSetting.GetDataTable(sql, false);
            }
            catch (Exception ex) { XtraMessageBox.Show("Load failed:\r\n" + ex.Message, "Error"); }
        }

        private void OnRefresh(object sender, EventArgs e) { LoadAppointments(); }

        private void OnNew(object sender, EventArgs e)
        {
            using (var f = new Appointment_Form(_dbSetting))
            {
                if (f.ShowDialog(this) == DialogResult.OK) LoadAppointments();
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            int rh = GridViewAppts.FocusedRowHandle;
            if (rh < 0) return;
            var row = GridViewAppts.GetDataRow(rh);
            if (row == null) return;
            using (var f = new Appointment_Form(_dbSetting, row))
            {
                if (f.ShowDialog(this) == DialogResult.OK) LoadAppointments();
            }
        }

        private void OnExit(object sender, EventArgs e) { this.Close(); }
    }
}
