using System;
using System.Data;
using System.Windows.Forms;
using AutoCount.Authentication;
using AutoCount.Data;
using DevExpress.XtraEditors;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    /// <summary>Service Item Tag Search — free-text search across Remark1..Remark4 fields.</summary>
    public partial class ServiceItemTagSearch_Form : XtraForm
    {
        private DBSetting _dbSetting;

        public ServiceItemTagSearch_Form() { InitializeComponent(); }
        public ServiceItemTagSearch_Form(UserSession userSession) : this() { if (userSession != null) _dbSetting = userSession.DBSetting; }
        public ServiceItemTagSearch_Form(DBSetting dbSetting) : this() { _dbSetting = dbSetting; }

        private void OnSearch(object sender, EventArgs e)
        {
            try
            {
                string term = SQLString(TxtSearchTerm.Text ?? "");
                if (string.IsNullOrWhiteSpace(term))
                { XtraMessageBox.Show("Enter a search term.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string sql = "SELECT ServiceItemCode, StockCode, [Description], DebtorCode, Remark1, Remark2, Remark3, Remark4 " +
                    "FROM [dbo].[zSCP_ServiceItem] WHERE " +
                    "Remark1 LIKE N'%" + term + "%' OR Remark2 LIKE N'%" + term + "%' OR Remark3 LIKE N'%" + term + "%' OR Remark4 LIKE N'%" + term + "%' " +
                    "OR [Description] LIKE N'%" + term + "%' OR ServiceItemCode LIKE N'%" + term + "%' " +
                    "ORDER BY ServiceItemCode";
                GridResults.DataSource = _dbSetting.GetDataTable(sql, false);
            }
            catch (Exception ex) { XtraMessageBox.Show("Search failed:\r\n" + ex.Message, "Error"); }
        }

        private void OnClose(object sender, EventArgs e) { this.Close(); }
    }
}
