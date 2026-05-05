using System;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    /// <summary>
    /// Generate Service Item From Serial No — pick debtor + serial number prefix → create ServiceItem row per serial.
    /// </summary>
    public partial class GenerateServiceItemFromSerial_Form : XtraForm
    {
        private DBSetting _dbSetting;

        public GenerateServiceItemFromSerial_Form() { InitializeComponent(); }
        public GenerateServiceItemFromSerial_Form(UserSession userSession) : this() { if (userSession != null) _dbSetting = userSession.DBSetting; }
        public GenerateServiceItemFromSerial_Form(DBSetting dbSetting) : this() { _dbSetting = dbSetting; }

        private void OnGenerate(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtDebtorCode.Text))
            { XtraMessageBox.Show("Debtor Code is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(TxtSerialPrefix.Text))
            { XtraMessageBox.Show("Serial Prefix is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            int count = 0; int.TryParse(TxtCount.Text, out count);
            if (count <= 0)
            { XtraMessageBox.Show("Count must be > 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                string debtor = SQLString(TxtDebtorCode.Text.Trim());
                string stock = SQLString(TxtStockCode.Text ?? "");
                string prefix = SQLString(TxtSerialPrefix.Text.Trim());
                int generated = 0;
                for (int i = 1; i <= count; i++)
                {
                    string code = prefix + "-" + i.ToString("0000");
                    _dbSetting.ExecuteNonQuery("INSERT INTO [dbo].[zSCP_ServiceItem] (ServiceItemCode, StockCode, DebtorCode, Created, Modified) VALUES (N'" + code + "', N'" + stock + "', N'" + debtor + "', GETDATE(), GETDATE())");
                    generated++;
                }
                XtraMessageBox.Show("Generated " + generated + " service item(s).", "Generate",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { XtraMessageBox.Show("Generate failed:\r\n" + ex.Message, "Error"); }
        }

        private void OnCancel(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
    }
}
