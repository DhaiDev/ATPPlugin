using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    partial class MeterTypeLst_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private SimpleButton BtnNew, BtnRefresh, BtnEdit, BtnSave, BtnCancel, BtnDelete, BtnExit;
        private GridControl GridMT; private GridView GridViewMT;
        private GroupControl GrpDetail;
        private LabelControl LblCode, LblDesc, LblStock, LblMultiPrice, LblMinCharges, LblChargesRate, LblRebateQty, LblFOCQty;
        private TextEdit TxtCode, TxtDesc, TxtStockCode, TxtMultiPriceCode, TxtMinCharges, TxtChargesRate, TxtRebateQty, TxtFOCQty;
        private CheckEdit ChkInactive;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.BtnNew = new SimpleButton(); this.BtnRefresh = new SimpleButton(); this.BtnEdit = new SimpleButton(); this.BtnSave = new SimpleButton();
            this.BtnCancel = new SimpleButton(); this.BtnDelete = new SimpleButton(); this.BtnExit = new SimpleButton();
            this.GridMT = new GridControl(); this.GridViewMT = new GridView();
            this.GrpDetail = new GroupControl();
            this.LblCode = new LabelControl(); this.TxtCode = new TextEdit();
            this.LblDesc = new LabelControl(); this.TxtDesc = new TextEdit();
            this.LblStock = new LabelControl(); this.TxtStockCode = new TextEdit();
            this.LblMultiPrice = new LabelControl(); this.TxtMultiPriceCode = new TextEdit();
            this.LblMinCharges = new LabelControl(); this.TxtMinCharges = new TextEdit();
            this.LblChargesRate = new LabelControl(); this.TxtChargesRate = new TextEdit();
            this.LblRebateQty = new LabelControl(); this.TxtRebateQty = new TextEdit();
            this.LblFOCQty = new LabelControl(); this.TxtFOCQty = new TextEdit();
            this.ChkInactive = new CheckEdit();

            this.SuspendLayout();
            this.Text = "Meter Type"; this.ClientSize = new Size(1050, 650);
            this.StartPosition = FormStartPosition.CenterParent; this.MinimumSize = new Size(900, 580);

            this.LblTitle.Text = "Meter Type";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 8);

            Tb(this.BtnNew, "New", 780, 10, 60); this.BtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnNew.Click += new System.EventHandler(this.OnNew);
            Tb(this.BtnRefresh, "Refresh", 845, 10, 75); this.BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnRefresh.Click += new System.EventHandler(this.OnRefresh);
            Tb(this.BtnExit, "Exit", 980, 10, 60); this.BtnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnExit.Click += new System.EventHandler(this.OnExit);

            this.GridMT.Location = new Point(14, 44); this.GridMT.Size = new Size(1020, 260);
            this.GridMT.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewMT.GridControl = this.GridMT; this.GridViewMT.OptionsView.ShowGroupPanel = false; this.GridViewMT.OptionsBehavior.Editable = false;
            this.GridMT.MainView = this.GridViewMT; this.GridMT.ViewCollection.Add(this.GridViewMT);
            AddCol(this.GridViewMT, "MeterTypeCode", "Meter Type Code", 150); AddCol(this.GridViewMT, "Description", "Description", 220);
            AddCol(this.GridViewMT, "StockCode", "Stock Code", 130); AddCol(this.GridViewMT, "MeterMultiPriceCode", "Multi Price Code", 140);
            AddCol(this.GridViewMT, "ChargesRate", "Charges Rate", 100); AddCol(this.GridViewMT, "MinimumCharges", "Min. Charges", 100);
            AddCol(this.GridViewMT, "RebateQtyInPercent", "Rebate Qty (%)", 100); AddCol(this.GridViewMT, "FOCQty", "FOC (Qty)", 80);
            AddCol(this.GridViewMT, "Inactive", "Inactive", 60);

            this.GrpDetail.Text = "Detail"; this.GrpDetail.Location = new Point(14, 312); this.GrpDetail.Size = new Size(1020, 330);
            this.GrpDetail.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            int lX = 14, eX = 170, eW = 300; int y = 28, gap = 26;
            Lbl(this.LblCode, "Meter Type Code", lX, y); this.TxtCode.Location = new Point(eX, y); this.TxtCode.Width = eW;
            this.ChkInactive.Properties.Caption = "Inactive"; this.ChkInactive.Location = new Point(eX + eW + 20, y); y += gap;

            Lbl(this.LblDesc, "Description", lX, y); this.TxtDesc.Location = new Point(eX, y); this.TxtDesc.Width = 560; y += gap;

            Lbl(this.LblStock, "Service/Stock Item", lX, y); this.TxtStockCode.Location = new Point(eX, y); this.TxtStockCode.Width = eW; y += gap;

            Lbl(this.LblMultiPrice, "Meter Multi Price Code", lX, y); this.TxtMultiPriceCode.Location = new Point(eX, y); this.TxtMultiPriceCode.Width = eW; y += gap;

            Lbl(this.LblChargesRate, "Charges Rate (Base UOM)", lX, y); this.TxtChargesRate.Location = new Point(eX, y); this.TxtChargesRate.Width = 150; y += gap;

            Lbl(this.LblMinCharges, "Minimum Charges", lX, y); this.TxtMinCharges.Location = new Point(eX, y); this.TxtMinCharges.Width = 150; y += gap;

            Lbl(this.LblFOCQty, "FOC Qty", lX, y); this.TxtFOCQty.Location = new Point(eX, y); this.TxtFOCQty.Width = 150; y += gap;

            Lbl(this.LblRebateQty, "Rebate Qty (%)", lX, y); this.TxtRebateQty.Location = new Point(eX, y); this.TxtRebateQty.Width = 150; y += gap + 8;

            int bX = 14, bY = y, bW = 90;
            Tb(this.BtnSave,   "Save (F7)",   bX + bW * 0 + 0,  bY, bW); this.BtnSave.Click   += new System.EventHandler(this.OnSave);
            Tb(this.BtnCancel, "Cancel (F8)", bX + bW * 1 + 6,  bY, bW); this.BtnCancel.Click += new System.EventHandler(this.OnCancel);
            Tb(this.BtnEdit,   "Edit (F6)",   bX + bW * 2 + 12, bY, bW); this.BtnEdit.Click   += new System.EventHandler(this.OnEdit);
            Tb(this.BtnDelete, "Delete (F9)", bX + bW * 3 + 18, bY, bW); this.BtnDelete.Click += new System.EventHandler(this.OnDelete);

            this.GrpDetail.Controls.Add(this.LblCode); this.GrpDetail.Controls.Add(this.TxtCode); this.GrpDetail.Controls.Add(this.ChkInactive);
            this.GrpDetail.Controls.Add(this.LblDesc); this.GrpDetail.Controls.Add(this.TxtDesc);
            this.GrpDetail.Controls.Add(this.LblStock); this.GrpDetail.Controls.Add(this.TxtStockCode);
            this.GrpDetail.Controls.Add(this.LblMultiPrice); this.GrpDetail.Controls.Add(this.TxtMultiPriceCode);
            this.GrpDetail.Controls.Add(this.LblChargesRate); this.GrpDetail.Controls.Add(this.TxtChargesRate);
            this.GrpDetail.Controls.Add(this.LblMinCharges); this.GrpDetail.Controls.Add(this.TxtMinCharges);
            this.GrpDetail.Controls.Add(this.LblFOCQty); this.GrpDetail.Controls.Add(this.TxtFOCQty);
            this.GrpDetail.Controls.Add(this.LblRebateQty); this.GrpDetail.Controls.Add(this.TxtRebateQty);
            this.GrpDetail.Controls.Add(this.BtnSave); this.GrpDetail.Controls.Add(this.BtnCancel);
            this.GrpDetail.Controls.Add(this.BtnEdit); this.GrpDetail.Controls.Add(this.BtnDelete);

            this.Controls.Add(this.LblTitle); this.Controls.Add(this.BtnNew); this.Controls.Add(this.BtnRefresh); this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.GridMT); this.Controls.Add(this.GrpDetail);
            this.ResumeLayout(false);
        }

        private static void Tb(SimpleButton b, string t, int x, int y, int w) { b.Text=t; b.Location=new Point(x,y); b.Width=w; b.Height=28; }
        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text=t; l.Location=new Point(x,y+3); }
        private static void AddCol(GridView gv, string f, string c, int w)
        { var col=new GridColumn(); col.FieldName=f; col.Caption=c; col.Visible=true; col.Width=w; col.VisibleIndex=gv.Columns.Count; gv.Columns.Add(col); }
    }
}
