using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    partial class MeterMultiPricingLst_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private SimpleButton BtnNew, BtnRefresh, BtnEdit, BtnSave, BtnCancel, BtnDelete, BtnExit;
        private GridControl GridMP; private GridView GridViewMP;
        private LabelControl LblCode, LblDesc;
        private TextEdit TxtCode, TxtDesc;
        private LabelControl LblItems;
        private GridControl GridItems; private GridView GridViewItems;
        private SimpleButton BtnAddRow, BtnDelRow, BtnRowUp, BtnRowDown;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.BtnNew = new SimpleButton(); this.BtnRefresh = new SimpleButton(); this.BtnEdit = new SimpleButton();
            this.BtnSave = new SimpleButton(); this.BtnCancel = new SimpleButton();
            this.BtnDelete = new SimpleButton(); this.BtnExit = new SimpleButton();
            this.GridMP = new GridControl(); this.GridViewMP = new GridView();
            this.LblCode = new LabelControl(); this.TxtCode = new TextEdit();
            this.LblDesc = new LabelControl(); this.TxtDesc = new TextEdit();
            this.LblItems = new LabelControl();
            this.GridItems = new GridControl(); this.GridViewItems = new GridView();
            this.BtnAddRow = new SimpleButton(); this.BtnDelRow = new SimpleButton();
            this.BtnRowUp = new SimpleButton(); this.BtnRowDown = new SimpleButton();

            this.SuspendLayout();
            this.Text = "Meter Multi Pricing"; this.ClientSize = new Size(1050, 700);
            this.StartPosition = FormStartPosition.CenterParent; this.MinimumSize = new Size(900, 600);

            this.LblTitle.Text = "Meter Multi Pricing";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 8);

            Tb(this.BtnNew, "New", 780, 10, 60); this.BtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnNew.Click += new System.EventHandler(this.OnNew);
            Tb(this.BtnRefresh, "Refresh", 845, 10, 75); this.BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnRefresh.Click += new System.EventHandler(this.OnRefresh);
            Tb(this.BtnExit, "Exit (F2)", 980, 10, 60); this.BtnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnExit.Click += new System.EventHandler(this.OnExit);

            // Top grid — MultiPrice codes
            this.GridMP.Location = new Point(14, 44); this.GridMP.Size = new Size(1020, 230);
            this.GridMP.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewMP.GridControl = this.GridMP; this.GridViewMP.OptionsView.ShowGroupPanel = false;
            this.GridViewMP.OptionsBehavior.Editable = false;
            this.GridMP.MainView = this.GridViewMP; this.GridMP.ViewCollection.Add(this.GridViewMP);
            AddCol(this.GridViewMP, "MeterMultiPriceCode", "Multi Pricing Code", 260);
            AddCol(this.GridViewMP, "Description", "Description", 720);

            // Detail fields
            Lbl(this.LblCode, "Multi Pricing Code :", 14, 288);
            this.TxtCode.Location = new Point(160, 285); this.TxtCode.Width = 300;
            Lbl(this.LblDesc, "Description :", 14, 318);
            this.TxtDesc.Location = new Point(160, 315); this.TxtDesc.Width = 720;

            // Pricing rates grid
            Lbl(this.LblItems, "Meter Pricing Rates :", 14, 348);
            Tb(this.BtnAddRow,  "+ Add Row",    780, 345, 85); this.BtnAddRow.Anchor  = AnchorStyles.Top | AnchorStyles.Right; this.BtnAddRow.Click  += new System.EventHandler(this.OnAddItemRow);
            Tb(this.BtnDelRow,  "- Delete Row", 870, 345, 95); this.BtnDelRow.Anchor  = AnchorStyles.Top | AnchorStyles.Right; this.BtnDelRow.Click  += new System.EventHandler(this.OnDeleteItemRow);
            Tb(this.BtnRowUp,   "Move Up",      970, 345, 30); this.BtnRowUp.Text = "↑"; this.BtnRowUp.Anchor   = AnchorStyles.Top | AnchorStyles.Right; this.BtnRowUp.Click   += new System.EventHandler(this.OnMoveItemUp);
            Tb(this.BtnRowDown, "Move Down",    1005, 345, 30); this.BtnRowDown.Text = "↓"; this.BtnRowDown.Anchor = AnchorStyles.Top | AnchorStyles.Right; this.BtnRowDown.Click += new System.EventHandler(this.OnMoveItemDown);
            this.GridItems.Location = new Point(14, 370); this.GridItems.Size = new Size(1020, 240);
            this.GridItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewItems.GridControl = this.GridItems; this.GridViewItems.OptionsView.ShowGroupPanel = false;
            this.GridViewItems.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.GridItems.MainView = this.GridViewItems; this.GridItems.ViewCollection.Add(this.GridViewItems);
            AddCol(this.GridViewItems, "MeterReading", "Meter Reading (<=)", 350);
            AddCol(this.GridViewItems, "UnitPrice", "Unit Price (Base UOM)", 350);

            // Buttons below items grid (inside detail area)
            int bX = 14, bY = 620, bW = 90;
            Tb(this.BtnSave,    "Save (F7)",   bX + 0,            bY, bW); this.BtnSave.Anchor   = AnchorStyles.Bottom | AnchorStyles.Left; this.BtnSave.Click   += new System.EventHandler(this.OnSave);
            Tb(this.BtnCancel,  "Cancel (F8)", bX + (bW + 6) * 1, bY, bW); this.BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; this.BtnCancel.Click += new System.EventHandler(this.OnCancel);
            Tb(this.BtnEdit,    "Edit (F6)",   bX + (bW + 6) * 2, bY, bW); this.BtnEdit.Anchor   = AnchorStyles.Bottom | AnchorStyles.Left; this.BtnEdit.Click   += new System.EventHandler(this.OnEdit);
            Tb(this.BtnDelete,  "Delete (F9)", bX + (bW + 6) * 3, bY, bW); this.BtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; this.BtnDelete.Click += new System.EventHandler(this.OnDelete);

            this.Controls.Add(this.LblTitle); this.Controls.Add(this.BtnNew); this.Controls.Add(this.BtnRefresh); this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.GridMP);
            this.Controls.Add(this.LblCode); this.Controls.Add(this.TxtCode);
            this.Controls.Add(this.LblDesc); this.Controls.Add(this.TxtDesc);
            this.Controls.Add(this.LblItems); this.Controls.Add(this.GridItems);
            this.Controls.Add(this.BtnAddRow); this.Controls.Add(this.BtnDelRow);
            this.Controls.Add(this.BtnRowUp); this.Controls.Add(this.BtnRowDown);
            this.Controls.Add(this.BtnSave); this.Controls.Add(this.BtnCancel); this.Controls.Add(this.BtnEdit); this.Controls.Add(this.BtnDelete);
            this.ResumeLayout(false);
        }

        private static void Tb(SimpleButton b, string t, int x, int y, int w) { b.Text = t; b.Location = new Point(x, y); b.Width = w; b.Height = 28; }
        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
        private static void AddCol(GridView gv, string f, string c, int w)
        { var col = new GridColumn(); col.FieldName = f; col.Caption = c; col.Visible = true; col.Width = w; col.VisibleIndex = gv.Columns.Count; gv.Columns.Add(col); }
    }
}
