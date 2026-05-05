using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    partial class ResetServiceItemDebtorOwnership_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblFromDebtor, LblToDebtor;
        private TextEdit TxtFromDebtor, TxtToDebtor;
        private SimpleButton BtnLoad, BtnReset, BtnClose;
        private GridControl GridItems;
        private GridView GridViewItems;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblFromDebtor = new LabelControl(); this.TxtFromDebtor = new TextEdit();
            this.LblToDebtor = new LabelControl(); this.TxtToDebtor = new TextEdit();
            this.BtnLoad = new SimpleButton(); this.BtnReset = new SimpleButton(); this.BtnClose = new SimpleButton();
            this.GridItems = new GridControl(); this.GridViewItems = new GridView();

            this.SuspendLayout();
            this.Text = "Reset Service Item Debtor Ownership";
            this.ClientSize = new Size(860, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(700, 500);

            this.LblTitle.Text = "Reset Service Item Debtor Ownership";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            Lbl(this.LblFromDebtor, "From Debtor", 14, 50);
            this.TxtFromDebtor.Location = new Point(110, 47); this.TxtFromDebtor.Width = 200;
            this.BtnLoad.Text = "Load Items"; this.BtnLoad.Location = new Point(320, 47); this.BtnLoad.Width = 100; this.BtnLoad.Height = 28;
            this.BtnLoad.Click += new System.EventHandler(this.OnLoadItems);

            Lbl(this.LblToDebtor, "To Debtor", 450, 50);
            this.TxtToDebtor.Location = new Point(540, 47); this.TxtToDebtor.Width = 200;
            this.BtnReset.Text = "Reset Selected"; this.BtnReset.Location = new Point(750, 47); this.BtnReset.Width = 105; this.BtnReset.Height = 28;
            this.BtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BtnReset.Click += new System.EventHandler(this.OnReset);

            this.GridItems.Location = new Point(14, 85);
            this.GridItems.Size = new Size(835, 430);
            this.GridItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewItems.GridControl = this.GridItems;
            this.GridViewItems.OptionsView.ShowGroupPanel = false;
            this.GridViewItems.OptionsSelection.MultiSelect = true;
            this.GridViewItems.OptionsBehavior.Editable = false;
            this.GridItems.MainView = this.GridViewItems;
            this.GridItems.ViewCollection.Add(this.GridViewItems);
            AddCol(this.GridViewItems, "ServiceItemCode", "Service Item Code", 180);
            AddCol(this.GridViewItems, "StockCode", "Stock Code", 180);
            AddCol(this.GridViewItems, "Description", "Description", 400);

            this.BtnClose.Text = "Close"; this.BtnClose.Location = new Point(770, 525); this.BtnClose.Width = 85; this.BtnClose.Height = 30;
            this.BtnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.BtnClose.Click += new System.EventHandler(this.OnClose);

            Control[] cs = new Control[] { this.LblTitle, this.LblFromDebtor, this.TxtFromDebtor, this.BtnLoad,
                this.LblToDebtor, this.TxtToDebtor, this.BtnReset, this.GridItems, this.BtnClose };
            foreach (var c in cs) this.Controls.Add(c);
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
