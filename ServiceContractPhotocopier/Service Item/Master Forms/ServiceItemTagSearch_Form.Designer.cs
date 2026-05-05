using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    partial class ServiceItemTagSearch_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblSearch;
        private TextEdit TxtSearchTerm;
        private SimpleButton BtnSearch, BtnClose;
        private GridControl GridResults;
        private GridView GridViewResults;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblSearch = new LabelControl(); this.TxtSearchTerm = new TextEdit();
            this.BtnSearch = new SimpleButton(); this.BtnClose = new SimpleButton();
            this.GridResults = new GridControl(); this.GridViewResults = new GridView();

            this.SuspendLayout();
            this.Text = "Service Item Tag Search";
            this.ClientSize = new Size(960, 540);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(800, 500);

            this.LblTitle.Text = "Service Item Tag Search";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            Lbl(this.LblSearch, "Search Term", 14, 50);
            this.TxtSearchTerm.Location = new Point(110, 47); this.TxtSearchTerm.Width = 500;
            this.BtnSearch.Text = "Search"; this.BtnSearch.Location = new Point(620, 47); this.BtnSearch.Width = 85; this.BtnSearch.Height = 28;
            this.BtnSearch.Click += new System.EventHandler(this.OnSearch);
            this.BtnClose.Text = "Close"; this.BtnClose.Location = new Point(860, 47); this.BtnClose.Width = 85; this.BtnClose.Height = 28;
            this.BtnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BtnClose.Click += new System.EventHandler(this.OnClose);

            this.GridResults.Location = new Point(14, 85);
            this.GridResults.Size = new Size(935, 440);
            this.GridResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewResults.GridControl = this.GridResults;
            this.GridViewResults.OptionsView.ShowGroupPanel = false;
            this.GridViewResults.OptionsBehavior.Editable = false;
            this.GridResults.MainView = this.GridViewResults;
            this.GridResults.ViewCollection.Add(this.GridViewResults);
            AddCol(this.GridViewResults, "ServiceItemCode", "Service Item Code", 160);
            AddCol(this.GridViewResults, "StockCode", "Stock Code", 140);
            AddCol(this.GridViewResults, "Description", "Description", 240);
            AddCol(this.GridViewResults, "DebtorCode", "Debtor Code", 100);
            AddCol(this.GridViewResults, "Remark1", "Remark 1", 140);
            AddCol(this.GridViewResults, "Remark2", "Remark 2", 140);
            AddCol(this.GridViewResults, "Remark3", "Remark 3", 140);
            AddCol(this.GridViewResults, "Remark4", "Remark 4", 140);

            Control[] cs = new Control[] { this.LblTitle, this.LblSearch, this.TxtSearchTerm, this.BtnSearch, this.BtnClose, this.GridResults };
            foreach (var c in cs) this.Controls.Add(c);
            this.AcceptButton = this.BtnSearch;
            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
        private static void AddCol(GridView gv, string f, string c, int w)
        {
            var col = new GridColumn(); col.FieldName = f; col.Caption = c; col.Visible = true; col.Width = w;
            col.VisibleIndex = gv.Columns.Count; gv.Columns.Add(col);
        }
    }
}
