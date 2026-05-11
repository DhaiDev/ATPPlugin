namespace VTACPluginBase.BaseForms.Editor
{
    // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Visual inheritance in VS Designer works better with non-abstract base form
    //abstract partial class BaseEditor_Form
    // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Make BaseEditor_Form non-abstract (align with BaseList_Form pattern)
    partial class BaseEditor_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Allow derived designers to access base InitializeComponent when needed
        //private void InitializeComponent()
        // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Align with BaseList_Form pattern (public InitializeComponent)
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseEditor_Form));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            this.DG_DtlStatus_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Header_GrpCtrl = new DevExpress.XtraEditors.GroupControl();
            this.Details_Pnl = new DevExpress.XtraEditors.PanelControl();
            this.DetailsInfo_Pnl = new DevExpress.XtraEditors.PanelControl();
            this.Details_TabCtrl = new DevExpress.XtraTab.XtraTabControl();
            this.Details_XTabPg = new DevExpress.XtraTab.XtraTabPage();
            this.Detail_DXGrid = new DevExpress.XtraGrid.GridControl();
            this.Icon16x16_ImgColl = new DevExpress.Utils.ImageCollection(this.components);
            this.Detail_DXGridVw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DG_DtlKey_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DG_DocKey_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Note_XTabPg = new DevExpress.XtraTab.XtraTabPage();
            this.Notes_ACMemoEditCtrl = new AutoCount.Controls.MemoEdit();
            this.popupControlContainer1 = new DevExpress.XtraBars.PopupControlContainer(this.components);
            this.OprButtons_Pnl = new DevExpress.XtraEditors.PanelControl();
            this.DeleteDetail_SpBtn = new DevExpress.XtraEditors.SimpleButton();
            this.EditDetail_SpBtn = new DevExpress.XtraEditors.SimpleButton();
            this.AddDetail_SpBtn = new DevExpress.XtraEditors.SimpleButton();
            this.Header_TabCtrl = new DevExpress.XtraTab.XtraTabControl();
            this.HeaderInfo_TabPg = new DevExpress.XtraTab.XtraTabPage();
            this.Toolbar_RbCtl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.Save_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.SaveAndClose_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Print_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Print_PopupMenu = new DevExpress.XtraBars.PopupMenu(this.components);
            this.PrintWithPrinterDialog_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.PrintWithDesigner_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Delete_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Activate_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Deactivate_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Complete_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Close_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.General_BarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.Progress_BarStaticItem = new DevExpress.XtraBars.BarStaticItem();
            this.Progress_BarEditItem = new DevExpress.XtraBars.BarEditItem();
            this.Progress_RepPBar = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.PrintWithDefaultPrinter_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Edit_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.GenerateACDocs_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.OpenAutocountDoc_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.New_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.ProceedAfterSave_BarChkItem = new DevExpress.XtraBars.BarCheckItem();
            this.Print_BarBtnGrp = new DevExpress.XtraBars.BarButtonGroup();
            this.Cancel_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Terminate_BarBtn = new DevExpress.XtraBars.BarButtonItem();
            this.Icon32x32_ImgColl = new DevExpress.Utils.ImageCollection(this.components);
            this.Editor_TopRbnPg = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.FuncCreate_RbnPgGrp = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.FuncDoc_RbnPgGrp = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.Exit_RbnPgGrp = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.RepositoryItemFontEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemFontEdit();
            this.RepositoryItemRichEditFontSizeEdit1 = new DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit();
            this.RepositoryItemBorderLineStyle1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle();
            this.RepositoryItemBorderLineWeight1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineWeight();
            this.RepositoryItemFloatingObjectOutlineWeight1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemFloatingObjectOutlineWeight();
            this.Bottom_RbnSttBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.DocDate_Lbl = new DevExpress.XtraEditors.LabelControl();
            this.DocDate_DteEdit = new DevExpress.XtraEditors.DateEdit();
            this.DocNo_Lbl = new DevExpress.XtraEditors.LabelControl();
            this.DocNo_TxtEdit = new DevExpress.XtraEditors.TextEdit();
            this.Boolean_MyRChkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Shelf_MyRGLEdit = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.RepositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Date_MyRDtEdit = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.LineStatus_MyRGLEditVw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DtlStatus_MyRGLEdit = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.SelectItemUOMCF_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SelectItemUOM_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SelectItemUOM_MyRGLEditVw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SelectItemUOM_MyRGLEdit = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.SelectItemDescription_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SelectUOM_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SelectItemNo_GCol = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Item_MyRGLEditVw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Item_MyRGLEdit = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.General_MyPRItems = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.Status_Lbl = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.Header_GrpCtrl)).BeginInit();
            this.Header_GrpCtrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Details_Pnl)).BeginInit();
            this.Details_Pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailsInfo_Pnl)).BeginInit();
            this.DetailsInfo_Pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Details_TabCtrl)).BeginInit();
            this.Details_TabCtrl.SuspendLayout();
            this.Details_XTabPg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Detail_DXGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Icon16x16_ImgColl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Detail_DXGridVw)).BeginInit();
            this.Note_XTabPg.SuspendLayout();
            this.Notes_ACMemoEditCtrl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OprButtons_Pnl)).BeginInit();
            this.OprButtons_Pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Header_TabCtrl)).BeginInit();
            this.Header_TabCtrl.SuspendLayout();
            this.HeaderInfo_TabPg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Toolbar_RbCtl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Print_PopupMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Progress_RepPBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Icon32x32_ImgColl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemFontEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemRichEditFontSizeEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemBorderLineStyle1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemBorderLineWeight1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemFloatingObjectOutlineWeight1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocDate_DteEdit.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocDate_DteEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocNo_TxtEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Boolean_MyRChkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Shelf_MyRGLEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Date_MyRDtEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Date_MyRDtEdit.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineStatus_MyRGLEditVw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtlStatus_MyRGLEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectItemUOM_MyRGLEditVw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectItemUOM_MyRGLEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Item_MyRGLEditVw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Item_MyRGLEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // DG_DtlStatus_GCol
            // 
            this.DG_DtlStatus_GCol.Caption = "Detail Status";
            this.DG_DtlStatus_GCol.FieldName = "DtlStatus";
            this.DG_DtlStatus_GCol.Name = "DG_DtlStatus_GCol";
            this.DG_DtlStatus_GCol.OptionsColumn.AllowEdit = false;
            this.DG_DtlStatus_GCol.OptionsColumn.ReadOnly = true;
            // 
            // Header_GrpCtrl
            // 
            this.Header_GrpCtrl.Controls.Add(this.Details_Pnl);
            this.Header_GrpCtrl.Controls.Add(this.Header_TabCtrl);
            this.Header_GrpCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header_GrpCtrl.Location = new System.Drawing.Point(0, 100);
            this.Header_GrpCtrl.Name = "Header_GrpCtrl";
            this.Header_GrpCtrl.ShowCaption = false;
            this.Header_GrpCtrl.Size = new System.Drawing.Size(1209, 554);
            this.Header_GrpCtrl.TabIndex = 6;
            this.Header_GrpCtrl.Text = "Header_GrpCtrl";
            // 
            // Details_Pnl
            // 
            this.Details_Pnl.Controls.Add(this.DetailsInfo_Pnl);
            this.Details_Pnl.Controls.Add(this.OprButtons_Pnl);
            this.Details_Pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Details_Pnl.Location = new System.Drawing.Point(2, 305);
            this.Details_Pnl.Name = "Details_Pnl";
            this.Details_Pnl.Size = new System.Drawing.Size(1205, 247);
            this.Details_Pnl.TabIndex = 3;
            // 
            // DetailsInfo_Pnl
            // 
            this.DetailsInfo_Pnl.Controls.Add(this.Details_TabCtrl);
            this.DetailsInfo_Pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailsInfo_Pnl.Location = new System.Drawing.Point(2, 45);
            this.DetailsInfo_Pnl.Name = "DetailsInfo_Pnl";
            this.DetailsInfo_Pnl.Size = new System.Drawing.Size(1201, 200);
            this.DetailsInfo_Pnl.TabIndex = 4;
            // 
            // Details_TabCtrl
            // 
            this.Details_TabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Details_TabCtrl.Location = new System.Drawing.Point(2, 2);
            this.Details_TabCtrl.Name = "Details_TabCtrl";
            this.Details_TabCtrl.SelectedTabPage = this.Details_XTabPg;
            this.Details_TabCtrl.Size = new System.Drawing.Size(1197, 196);
            this.Details_TabCtrl.TabIndex = 2;
            this.Details_TabCtrl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.Details_XTabPg,
            this.Note_XTabPg});
            // 
            // Details_XTabPg
            // 
            this.Details_XTabPg.Controls.Add(this.Detail_DXGrid);
            this.Details_XTabPg.Name = "Details_XTabPg";
            this.Details_XTabPg.Size = new System.Drawing.Size(1195, 171);
            this.Details_XTabPg.Text = "Details";
            // 
            // Detail_DXGrid
            // 
            this.Detail_DXGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.ImageList = this.Icon16x16_ImgColl;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.Remove.ImageIndex = 33;
            this.Detail_DXGrid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.Detail_DXGrid.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.Detail_DXGrid_EmbeddedNavigator_ButtonClick);
            this.Detail_DXGrid.Location = new System.Drawing.Point(0, 0);
            this.Detail_DXGrid.MainView = this.Detail_DXGridVw;
            this.Detail_DXGrid.Name = "Detail_DXGrid";
            this.Detail_DXGrid.Size = new System.Drawing.Size(1195, 171);
            this.Detail_DXGrid.TabIndex = 1;
            this.Detail_DXGrid.UseEmbeddedNavigator = true;
            this.Detail_DXGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.Detail_DXGridVw});
            // 
            // Icon16x16_ImgColl
            // 
            this.Icon16x16_ImgColl.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("Icon16x16_ImgColl.ImageStream")));
            this.Icon16x16_ImgColl.Images.SetKeyName(0, "Close.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(1, "Delete Document.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(2, "Save And Close.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(3, "Save.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(4, "Printer.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(5, "New Document.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(6, "Duplicate Document.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(7, "Open Document.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(8, "Edit Document.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(9, "Printer Dialog.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(10, "Printer Preview.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(11, "Document Import Lines.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(12, "Document Export Lines.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(13, "Document - Cancel.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(14, "Document - Completed.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(15, "Document - OK.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(16, "Document - Rollback.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(17, "WMS_StockAdjustment.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(18, "printer.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(19, "Report.ico");
            this.Icon16x16_ImgColl.Images.SetKeyName(20, "Close_16.png");
            this.Icon16x16_ImgColl.Images.SetKeyName(21, "Delete_16.png");
            this.Icon16x16_ImgColl.Images.SetKeyName(22, "Edit_16.png");
            this.Icon16x16_ImgColl.Images.SetKeyName(23, "New_16.png");
            this.Icon16x16_ImgColl.Images.SetKeyName(24, "Save_16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("export_16x16.png", "images/export/export_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/export/export_16x16.png"), 25);
            this.Icon16x16_ImgColl.Images.SetKeyName(25, "export_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("saveto_16x16.png", "images/save/saveto_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/save/saveto_16x16.png"), 26);
            this.Icon16x16_ImgColl.Images.SetKeyName(26, "saveto_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("saveandclose_16x16.png", "images/save/saveandclose_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/save/saveandclose_16x16.png"), 27);
            this.Icon16x16_ImgColl.Images.SetKeyName(27, "saveandclose_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("print_16x16.png", "images/print/print_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/print_16x16.png"), 28);
            this.Icon16x16_ImgColl.Images.SetKeyName(28, "print_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("printdialog_16x16.png", "images/print/printdialog_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/printdialog_16x16.png"), 29);
            this.Icon16x16_ImgColl.Images.SetKeyName(29, "printdialog_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("issue_16x16.png", "images/support/issue_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/issue_16x16.png"), 30);
            this.Icon16x16_ImgColl.Images.SetKeyName(30, "issue_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("design_16x16.png", "images/miscellaneous/design_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/miscellaneous/design_16x16.png"), 31);
            this.Icon16x16_ImgColl.Images.SetKeyName(31, "design_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("preview_16x16.png", "images/print/preview_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/preview_16x16.png"), 32);
            this.Icon16x16_ImgColl.Images.SetKeyName(32, "preview_16x16.png");
            this.Icon16x16_ImgColl.InsertGalleryImage("cancel_16x16.png", "images/actions/cancel_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_16x16.png"), 33);
            this.Icon16x16_ImgColl.Images.SetKeyName(33, "cancel_16x16.png");
            this.Icon16x16_ImgColl.Images.SetKeyName(34, "Invoice01.png");
            this.Icon16x16_ImgColl.Images.SetKeyName(35, "Invoice02.png");
            this.Icon16x16_ImgColl.InsertImage(global::VTACPluginBase.Properties.Resources.removeitem_16x16, "removeitem_16x16", typeof(global::VTACPluginBase.Properties.Resources), 36);
            this.Icon16x16_ImgColl.Images.SetKeyName(36, "removeitem_16x16");
            // 
            // Detail_DXGridVw
            // 
            this.Detail_DXGridVw.Appearance.GroupPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Detail_DXGridVw.Appearance.GroupPanel.Options.UseFont = true;
            this.Detail_DXGridVw.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.Detail_DXGridVw.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.Detail_DXGridVw.ColumnPanelRowHeight = 40;
            this.Detail_DXGridVw.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DG_DtlKey_GCol,
            this.DG_DocKey_GCol,
            this.DG_DtlStatus_GCol});
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Column = this.DG_DtlStatus_GCol;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Expression;
            formatConditionRuleValue1.Expression = "[DtlStatus] = \'AR PAYMENT GENERATED\'";
            formatConditionRuleValue1.PredefinedName = "Green Fill";
            gridFormatRule1.Rule = formatConditionRuleValue1;
            this.Detail_DXGridVw.FormatRules.Add(gridFormatRule1);
            this.Detail_DXGridVw.GridControl = this.Detail_DXGrid;
            this.Detail_DXGridVw.Name = "Detail_DXGridVw";
            this.Detail_DXGridVw.OptionsBehavior.AllowIncrementalSearch = true;
            this.Detail_DXGridVw.OptionsView.ColumnAutoWidth = false;
            this.Detail_DXGridVw.OptionsView.ShowGroupPanel = false;
            this.Detail_DXGridVw.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.Detail_DXGridVw_FocusedRowChanged);
            this.Detail_DXGridVw.DoubleClick += new System.EventHandler(this.Detail_DXGridVw_DoubleClick);
            // 
            // DG_DtlKey_GCol
            // 
            this.DG_DtlKey_GCol.Caption = "DtlKey";
            this.DG_DtlKey_GCol.FieldName = "DtlKey";
            this.DG_DtlKey_GCol.Name = "DG_DtlKey_GCol";
            this.DG_DtlKey_GCol.OptionsColumn.AllowEdit = false;
            this.DG_DtlKey_GCol.OptionsColumn.ReadOnly = true;
            // 
            // DG_DocKey_GCol
            // 
            this.DG_DocKey_GCol.Caption = "DocKey";
            this.DG_DocKey_GCol.FieldName = "DocKey";
            this.DG_DocKey_GCol.Name = "DG_DocKey_GCol";
            this.DG_DocKey_GCol.OptionsColumn.AllowEdit = false;
            this.DG_DocKey_GCol.OptionsColumn.ReadOnly = true;
            this.DG_DocKey_GCol.Width = 97;
            // 
            // Note_XTabPg
            // 
            this.Note_XTabPg.Controls.Add(this.Notes_ACMemoEditCtrl);
            this.Note_XTabPg.Name = "Note_XTabPg";
            this.Note_XTabPg.PageEnabled = false;
            this.Note_XTabPg.PageVisible = false;
            this.Note_XTabPg.Size = new System.Drawing.Size(1195, 171);
            this.Note_XTabPg.Text = "Note";
            // 
            // Notes_ACMemoEditCtrl
            // 
            this.Notes_ACMemoEditCtrl.Controls.Add(this.popupControlContainer1);
            this.Notes_ACMemoEditCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Notes_ACMemoEditCtrl.Location = new System.Drawing.Point(0, 0);
            this.Notes_ACMemoEditCtrl.Name = "Notes_ACMemoEditCtrl";
            this.Notes_ACMemoEditCtrl.Size = new System.Drawing.Size(1195, 171);
            this.Notes_ACMemoEditCtrl.TabIndex = 1;
            // 
            // popupControlContainer1
            // 
            this.popupControlContainer1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.popupControlContainer1.Location = new System.Drawing.Point(160, 64);
            this.popupControlContainer1.Name = "popupControlContainer1";
            this.popupControlContainer1.Size = new System.Drawing.Size(181, 138);
            this.popupControlContainer1.TabIndex = 4;
            this.popupControlContainer1.Visible = false;
            // 
            // OprButtons_Pnl
            // 
            this.OprButtons_Pnl.Controls.Add(this.DeleteDetail_SpBtn);
            this.OprButtons_Pnl.Controls.Add(this.EditDetail_SpBtn);
            this.OprButtons_Pnl.Controls.Add(this.AddDetail_SpBtn);
            this.OprButtons_Pnl.Dock = System.Windows.Forms.DockStyle.Top;
            this.OprButtons_Pnl.Location = new System.Drawing.Point(2, 2);
            this.OprButtons_Pnl.Name = "OprButtons_Pnl";
            this.OprButtons_Pnl.Size = new System.Drawing.Size(1201, 43);
            this.OprButtons_Pnl.TabIndex = 3;
            this.OprButtons_Pnl.Visible = false;
            // 
            // DeleteDetail_SpBtn
            // 
            this.DeleteDetail_SpBtn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("DeleteDetail_SpBtn.ImageOptions.SvgImage")));
            this.DeleteDetail_SpBtn.Location = new System.Drawing.Point(170, 5);
            this.DeleteDetail_SpBtn.Name = "DeleteDetail_SpBtn";
            this.DeleteDetail_SpBtn.Size = new System.Drawing.Size(75, 31);
            this.DeleteDetail_SpBtn.TabIndex = 2;
            this.DeleteDetail_SpBtn.Text = "&Delete";
            this.DeleteDetail_SpBtn.Click += new System.EventHandler(this.DeleteDetail_SpBtn_Click);
            // 
            // EditDetail_SpBtn
            // 
            this.EditDetail_SpBtn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("EditDetail_SpBtn.ImageOptions.SvgImage")));
            this.EditDetail_SpBtn.Location = new System.Drawing.Point(89, 5);
            this.EditDetail_SpBtn.Name = "EditDetail_SpBtn";
            this.EditDetail_SpBtn.Size = new System.Drawing.Size(75, 31);
            this.EditDetail_SpBtn.TabIndex = 1;
            this.EditDetail_SpBtn.Text = "&Edit";
            this.EditDetail_SpBtn.Click += new System.EventHandler(this.EditDetail_SpBtn_Click);
            // 
            // AddDetail_SpBtn
            // 
            this.AddDetail_SpBtn.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("AddDetail_SpBtn.ImageOptions.SvgImage")));
            this.AddDetail_SpBtn.Location = new System.Drawing.Point(8, 5);
            this.AddDetail_SpBtn.Name = "AddDetail_SpBtn";
            this.AddDetail_SpBtn.Size = new System.Drawing.Size(75, 31);
            this.AddDetail_SpBtn.TabIndex = 0;
            this.AddDetail_SpBtn.Text = "&Add";
            this.AddDetail_SpBtn.Click += new System.EventHandler(this.AddDetail_SpBtn_Click);
            // 
            // Header_TabCtrl
            // 
            this.Header_TabCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header_TabCtrl.Location = new System.Drawing.Point(2, 2);
            this.Header_TabCtrl.Name = "Header_TabCtrl";
            this.Header_TabCtrl.SelectedTabPage = this.HeaderInfo_TabPg;
            this.Header_TabCtrl.Size = new System.Drawing.Size(1205, 303);
            this.Header_TabCtrl.TabIndex = 0;
            this.Header_TabCtrl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.HeaderInfo_TabPg});
            // 
            // HeaderInfo_TabPg
            // 
            this.HeaderInfo_TabPg.Controls.Add(this.DocDate_Lbl);
            this.HeaderInfo_TabPg.Controls.Add(this.DocDate_DteEdit);
            this.HeaderInfo_TabPg.Controls.Add(this.DocNo_Lbl);
            this.HeaderInfo_TabPg.Controls.Add(this.DocNo_TxtEdit);
            this.HeaderInfo_TabPg.Name = "HeaderInfo_TabPg";
            this.HeaderInfo_TabPg.Size = new System.Drawing.Size(1203, 278);
            this.HeaderInfo_TabPg.Text = "Header";
            // 
            // Toolbar_RbCtl
            // 
            this.Toolbar_RbCtl.ExpandCollapseItem.Id = 0;
            this.Toolbar_RbCtl.Images = this.Icon16x16_ImgColl;
            this.Toolbar_RbCtl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Toolbar_RbCtl.ExpandCollapseItem,
            this.Toolbar_RbCtl.SearchEditItem,
            this.Save_BarBtn,
            this.SaveAndClose_BarBtn,
            this.Print_BarBtn,
            this.Delete_BarBtn,
            this.Activate_BarBtn,
            this.Deactivate_BarBtn,
            this.Complete_BarBtn,
            this.Close_BarBtn,
            this.General_BarStaticItem,
            this.Progress_BarStaticItem,
            this.Progress_BarEditItem,
            this.PrintWithDefaultPrinter_BarBtn,
            this.PrintWithPrinterDialog_BarBtn,
            this.Edit_BarBtn,
            this.GenerateACDocs_BarBtn,
            this.PrintWithDesigner_BarBtn,
            this.OpenAutocountDoc_BarBtn,
            this.New_BarBtn,
            this.ProceedAfterSave_BarChkItem,
            this.Print_BarBtnGrp,
            this.Cancel_BarBtn,
            this.Terminate_BarBtn});
            this.Toolbar_RbCtl.LargeImages = this.Icon32x32_ImgColl;
            this.Toolbar_RbCtl.Location = new System.Drawing.Point(0, 0);
            this.Toolbar_RbCtl.MaxItemId = 270;
            this.Toolbar_RbCtl.Name = "Toolbar_RbCtl";
            this.Toolbar_RbCtl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.Editor_TopRbnPg});
            this.Toolbar_RbCtl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Progress_RepPBar,
            this.RepositoryItemFontEdit1,
            this.RepositoryItemRichEditFontSizeEdit1,
            this.RepositoryItemBorderLineStyle1,
            this.RepositoryItemBorderLineWeight1,
            this.RepositoryItemFloatingObjectOutlineWeight1});
            this.Toolbar_RbCtl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.Toolbar_RbCtl.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.Toolbar_RbCtl.Size = new System.Drawing.Size(1209, 100);
            this.Toolbar_RbCtl.StatusBar = this.Bottom_RbnSttBar;
            this.Toolbar_RbCtl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // Save_BarBtn
            // 
            this.Save_BarBtn.Caption = "Save";
            this.Save_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Save_BarBtn.Id = 2;
            this.Save_BarBtn.ImageOptions.DisabledImageIndex = 24;
            this.Save_BarBtn.ImageOptions.DisabledLargeImageIndex = 24;
            this.Save_BarBtn.ImageOptions.ImageIndex = 24;
            this.Save_BarBtn.ImageOptions.LargeImageIndex = 24;
            this.Save_BarBtn.Name = "Save_BarBtn";
            this.Save_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Save_BarBtn_ItemClick);
            // 
            // SaveAndClose_BarBtn
            // 
            this.SaveAndClose_BarBtn.Caption = "Save And Close";
            this.SaveAndClose_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.SaveAndClose_BarBtn.Id = 3;
            this.SaveAndClose_BarBtn.ImageOptions.DisabledImageIndex = 27;
            this.SaveAndClose_BarBtn.ImageOptions.DisabledLargeImageIndex = 27;
            this.SaveAndClose_BarBtn.ImageOptions.ImageIndex = 27;
            this.SaveAndClose_BarBtn.ImageOptions.LargeImageIndex = 27;
            this.SaveAndClose_BarBtn.Name = "SaveAndClose_BarBtn";
            // 
            // Print_BarBtn
            // 
            this.Print_BarBtn.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.Print_BarBtn.Caption = "Print";
            this.Print_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Print_BarBtn.DropDownControl = this.Print_PopupMenu;
            this.Print_BarBtn.Id = 4;
            this.Print_BarBtn.ImageOptions.DisabledImageIndex = 32;
            this.Print_BarBtn.ImageOptions.DisabledLargeImageIndex = 32;
            this.Print_BarBtn.ImageOptions.ImageIndex = 32;
            this.Print_BarBtn.ImageOptions.LargeImageIndex = 32;
            this.Print_BarBtn.Name = "Print_BarBtn";
            this.Print_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Print_BarBtn_ItemClick);
            // 
            // Print_PopupMenu
            // 
            this.Print_PopupMenu.ItemLinks.Add(this.PrintWithPrinterDialog_BarBtn);
            this.Print_PopupMenu.ItemLinks.Add(this.PrintWithDesigner_BarBtn);
            this.Print_PopupMenu.Name = "Print_PopupMenu";
            this.Print_PopupMenu.Ribbon = this.Toolbar_RbCtl;
            // 
            // PrintWithPrinterDialog_BarBtn
            // 
            this.PrintWithPrinterDialog_BarBtn.Caption = "Print...";
            this.PrintWithPrinterDialog_BarBtn.Id = 18;
            this.PrintWithPrinterDialog_BarBtn.ImageOptions.DisabledImageIndex = 29;
            this.PrintWithPrinterDialog_BarBtn.ImageOptions.DisabledLargeImageIndex = 29;
            this.PrintWithPrinterDialog_BarBtn.ImageOptions.ImageIndex = 29;
            this.PrintWithPrinterDialog_BarBtn.ImageOptions.LargeImageIndex = 29;
            this.PrintWithPrinterDialog_BarBtn.Name = "PrintWithPrinterDialog_BarBtn";
            this.PrintWithPrinterDialog_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintWithPrinterDialog_BarBtn_ItemClick);
            // 
            // PrintWithDesigner_BarBtn
            // 
            this.PrintWithDesigner_BarBtn.Caption = "Design";
            this.PrintWithDesigner_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.PrintWithDesigner_BarBtn.Id = 22;
            this.PrintWithDesigner_BarBtn.ImageOptions.DisabledImageIndex = 18;
            this.PrintWithDesigner_BarBtn.ImageOptions.DisabledLargeImageIndex = 18;
            this.PrintWithDesigner_BarBtn.ImageOptions.ImageIndex = 18;
            this.PrintWithDesigner_BarBtn.ImageOptions.LargeImageIndex = 18;
            this.PrintWithDesigner_BarBtn.Name = "PrintWithDesigner_BarBtn";
            this.PrintWithDesigner_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintWithDesigner_BarBtn_ItemClick);
            // 
            // Delete_BarBtn
            // 
            this.Delete_BarBtn.Caption = "Delete";
            this.Delete_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Delete_BarBtn.Id = 5;
            this.Delete_BarBtn.ImageOptions.DisabledImageIndex = 21;
            this.Delete_BarBtn.ImageOptions.DisabledLargeImageIndex = 21;
            this.Delete_BarBtn.ImageOptions.ImageIndex = 21;
            this.Delete_BarBtn.ImageOptions.LargeImageIndex = 21;
            this.Delete_BarBtn.Name = "Delete_BarBtn";
            this.Delete_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Delete_BarBtn_ItemClick);
            // 
            // Activate_BarBtn
            // 
            this.Activate_BarBtn.Caption = "Activate";
            this.Activate_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Activate_BarBtn.Id = 7;
            this.Activate_BarBtn.ImageOptions.DisabledImageIndex = 14;
            this.Activate_BarBtn.ImageOptions.DisabledLargeImageIndex = 14;
            this.Activate_BarBtn.ImageOptions.ImageIndex = 14;
            this.Activate_BarBtn.ImageOptions.LargeImageIndex = 14;
            this.Activate_BarBtn.Name = "Activate_BarBtn";
            // 
            // Deactivate_BarBtn
            // 
            this.Deactivate_BarBtn.Caption = "Deactivate";
            this.Deactivate_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Deactivate_BarBtn.Id = 8;
            this.Deactivate_BarBtn.ImageOptions.DisabledImageIndex = 16;
            this.Deactivate_BarBtn.ImageOptions.DisabledLargeImageIndex = 16;
            this.Deactivate_BarBtn.ImageOptions.ImageIndex = 16;
            this.Deactivate_BarBtn.ImageOptions.LargeImageIndex = 16;
            this.Deactivate_BarBtn.Name = "Deactivate_BarBtn";
            // 
            // Complete_BarBtn
            // 
            this.Complete_BarBtn.Caption = "Complete";
            this.Complete_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Complete_BarBtn.Id = 12;
            this.Complete_BarBtn.ImageOptions.DisabledImageIndex = 30;
            this.Complete_BarBtn.ImageOptions.DisabledLargeImageIndex = 30;
            this.Complete_BarBtn.ImageOptions.ImageIndex = 30;
            this.Complete_BarBtn.ImageOptions.LargeImageIndex = 30;
            this.Complete_BarBtn.Name = "Complete_BarBtn";
            this.Complete_BarBtn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.Complete_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Complete_BarBtn_ItemClick);
            // 
            // Close_BarBtn
            // 
            this.Close_BarBtn.Caption = "Close";
            this.Close_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Close_BarBtn.Id = 13;
            this.Close_BarBtn.ImageOptions.DisabledImageIndex = 20;
            this.Close_BarBtn.ImageOptions.DisabledLargeImageIndex = 20;
            this.Close_BarBtn.ImageOptions.ImageIndex = 20;
            this.Close_BarBtn.ImageOptions.LargeImageIndex = 20;
            this.Close_BarBtn.Name = "Close_BarBtn";
            this.Close_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Close_BarBtn_ItemClick);
            // 
            // General_BarStaticItem
            // 
            this.General_BarStaticItem.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.General_BarStaticItem.Id = 14;
            this.General_BarStaticItem.Name = "General_BarStaticItem";
            // 
            // Progress_BarStaticItem
            // 
            this.Progress_BarStaticItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.Progress_BarStaticItem.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Progress_BarStaticItem.Id = 15;
            this.Progress_BarStaticItem.Name = "Progress_BarStaticItem";
            // 
            // Progress_BarEditItem
            // 
            this.Progress_BarEditItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.Progress_BarEditItem.CaptionAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.Progress_BarEditItem.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Progress_BarEditItem.Edit = this.Progress_RepPBar;
            this.Progress_BarEditItem.EditWidth = 100;
            this.Progress_BarEditItem.Id = 16;
            this.Progress_BarEditItem.Name = "Progress_BarEditItem";
            // 
            // Progress_RepPBar
            // 
            this.Progress_RepPBar.Name = "Progress_RepPBar";
            // 
            // PrintWithDefaultPrinter_BarBtn
            // 
            this.PrintWithDefaultPrinter_BarBtn.Caption = "Print";
            this.PrintWithDefaultPrinter_BarBtn.Id = 17;
            this.PrintWithDefaultPrinter_BarBtn.ImageOptions.DisabledImageIndex = 28;
            this.PrintWithDefaultPrinter_BarBtn.ImageOptions.DisabledLargeImageIndex = 28;
            this.PrintWithDefaultPrinter_BarBtn.ImageOptions.ImageIndex = 28;
            this.PrintWithDefaultPrinter_BarBtn.ImageOptions.LargeImageIndex = 28;
            this.PrintWithDefaultPrinter_BarBtn.Name = "PrintWithDefaultPrinter_BarBtn";
            this.PrintWithDefaultPrinter_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.PrintWithDefaultPrinter_BarBtn_ItemClick);
            // 
            // Edit_BarBtn
            // 
            this.Edit_BarBtn.Caption = "Edit";
            this.Edit_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Edit_BarBtn.Id = 19;
            this.Edit_BarBtn.ImageOptions.DisabledImageIndex = 22;
            this.Edit_BarBtn.ImageOptions.DisabledLargeImageIndex = 22;
            this.Edit_BarBtn.ImageOptions.ImageIndex = 22;
            this.Edit_BarBtn.ImageOptions.LargeImageIndex = 22;
            this.Edit_BarBtn.Name = "Edit_BarBtn";
            this.Edit_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Edit_BarBtn_ItemClick);
            // 
            // GenerateACDocs_BarBtn
            // 
            this.GenerateACDocs_BarBtn.Caption = "Generate AutoCount Docs";
            this.GenerateACDocs_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.GenerateACDocs_BarBtn.Id = 21;
            this.GenerateACDocs_BarBtn.ImageOptions.DisabledImageIndex = 26;
            this.GenerateACDocs_BarBtn.ImageOptions.DisabledLargeImageIndex = 26;
            this.GenerateACDocs_BarBtn.ImageOptions.ImageIndex = 26;
            this.GenerateACDocs_BarBtn.ImageOptions.LargeImageIndex = 26;
            this.GenerateACDocs_BarBtn.Name = "GenerateACDocs_BarBtn";
            // 
            // OpenAutocountDoc_BarBtn
            // 
            this.OpenAutocountDoc_BarBtn.Caption = "Open Autocount Document";
            this.OpenAutocountDoc_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.OpenAutocountDoc_BarBtn.Id = 23;
            this.OpenAutocountDoc_BarBtn.ImageOptions.DisabledImageIndex = 7;
            this.OpenAutocountDoc_BarBtn.ImageOptions.DisabledLargeImageIndex = 7;
            this.OpenAutocountDoc_BarBtn.ImageOptions.ImageIndex = 7;
            this.OpenAutocountDoc_BarBtn.ImageOptions.LargeImageIndex = 7;
            this.OpenAutocountDoc_BarBtn.Name = "OpenAutocountDoc_BarBtn";
            // 
            // New_BarBtn
            // 
            this.New_BarBtn.Caption = "New";
            this.New_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.New_BarBtn.Id = 24;
            this.New_BarBtn.ImageOptions.DisabledImageIndex = 23;
            this.New_BarBtn.ImageOptions.DisabledLargeImageIndex = 23;
            this.New_BarBtn.ImageOptions.ImageIndex = 23;
            this.New_BarBtn.ImageOptions.LargeImageIndex = 23;
            this.New_BarBtn.Name = "New_BarBtn";
            this.New_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.New_BarBtn_ItemClick);
            // 
            // ProceedAfterSave_BarChkItem
            // 
            this.ProceedAfterSave_BarChkItem.Caption = "After save, proceed with new ";
            this.ProceedAfterSave_BarChkItem.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.ProceedAfterSave_BarChkItem.CheckBoxVisibility = DevExpress.XtraBars.CheckBoxVisibility.BeforeText;
            this.ProceedAfterSave_BarChkItem.Id = 25;
            this.ProceedAfterSave_BarChkItem.Name = "ProceedAfterSave_BarChkItem";
            this.ProceedAfterSave_BarChkItem.Tag = "Installment";
            // 
            // Print_BarBtnGrp
            // 
            this.Print_BarBtnGrp.Caption = "Print";
            this.Print_BarBtnGrp.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Print_BarBtnGrp.Id = 26;
            this.Print_BarBtnGrp.ItemLinks.Add(this.Print_BarBtn);
            this.Print_BarBtnGrp.Name = "Print_BarBtnGrp";
            // 
            // Cancel_BarBtn
            // 
            this.Cancel_BarBtn.Caption = "Cancel";
            this.Cancel_BarBtn.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
            this.Cancel_BarBtn.Id = 28;
            this.Cancel_BarBtn.ImageOptions.DisabledImageIndex = 33;
            this.Cancel_BarBtn.ImageOptions.DisabledLargeImageIndex = 33;
            this.Cancel_BarBtn.ImageOptions.ImageIndex = 33;
            this.Cancel_BarBtn.ImageOptions.LargeImageIndex = 33;
            this.Cancel_BarBtn.Name = "Cancel_BarBtn";
            this.Cancel_BarBtn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.Cancel_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Cancel_BarBtn_ItemClick);
            // 
            // Terminate_BarBtn
            // 
            this.Terminate_BarBtn.Caption = "Terminate";
            this.Terminate_BarBtn.Id = 269;
            this.Terminate_BarBtn.ImageOptions.DisabledImageIndex = 36;
            this.Terminate_BarBtn.ImageOptions.DisabledLargeImageIndex = 36;
            this.Terminate_BarBtn.ImageOptions.ImageIndex = 36;
            this.Terminate_BarBtn.ImageOptions.LargeImageIndex = 36;
            this.Terminate_BarBtn.Name = "Terminate_BarBtn";
            this.Terminate_BarBtn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Terminate_BarBtn_ItemClick);
            // 
            // Icon32x32_ImgColl
            // 
            this.Icon32x32_ImgColl.ImageSize = new System.Drawing.Size(32, 32);
            this.Icon32x32_ImgColl.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("Icon32x32_ImgColl.ImageStream")));
            this.Icon32x32_ImgColl.Images.SetKeyName(0, "Close.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(1, "Delete Document.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(2, "Save And Close.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(3, "Save.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(4, "Printer.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(5, "New Document.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(6, "Duplicate Document.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(7, "Open Document.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(8, "Edit Document.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(9, "Printer Dialog.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(10, "Printer Preview.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(11, "Document Import Lines.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(12, "Document Export Lines.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(13, "Document - Cancel.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(14, "Document - Completed.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(15, "Document - OK.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(16, "Document - Rollback.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(17, "WMS_StockAdjustment.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(18, "printer.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(19, "Report.ico");
            this.Icon32x32_ImgColl.Images.SetKeyName(20, "Close_32.png");
            this.Icon32x32_ImgColl.Images.SetKeyName(21, "Delete_32.png");
            this.Icon32x32_ImgColl.Images.SetKeyName(22, "Edit_32.png");
            this.Icon32x32_ImgColl.Images.SetKeyName(23, "New_32.png");
            this.Icon32x32_ImgColl.Images.SetKeyName(24, "Save_32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("export_32x32.png", "images/export/export_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/export/export_32x32.png"), 25);
            this.Icon32x32_ImgColl.Images.SetKeyName(25, "export_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("saveto_32x32.png", "images/save/saveto_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/save/saveto_32x32.png"), 26);
            this.Icon32x32_ImgColl.Images.SetKeyName(26, "saveto_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("saveandclose_32x32.png", "images/save/saveandclose_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/save/saveandclose_32x32.png"), 27);
            this.Icon32x32_ImgColl.Images.SetKeyName(27, "saveandclose_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("print_32x32.png", "images/print/print_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/print_32x32.png"), 28);
            this.Icon32x32_ImgColl.Images.SetKeyName(28, "print_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("printdialog_32x32.png", "images/print/printdialog_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/printdialog_32x32.png"), 29);
            this.Icon32x32_ImgColl.Images.SetKeyName(29, "printdialog_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("issue_32x32.png", "images/support/issue_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/issue_32x32.png"), 30);
            this.Icon32x32_ImgColl.Images.SetKeyName(30, "issue_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("design_32x32.png", "images/miscellaneous/design_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/miscellaneous/design_32x32.png"), 31);
            this.Icon32x32_ImgColl.Images.SetKeyName(31, "design_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("preview_32x32.png", "images/print/preview_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/preview_32x32.png"), 32);
            this.Icon32x32_ImgColl.Images.SetKeyName(32, "preview_32x32.png");
            this.Icon32x32_ImgColl.InsertGalleryImage("cancel_32x32.png", "images/actions/cancel_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/cancel_32x32.png"), 33);
            this.Icon32x32_ImgColl.Images.SetKeyName(33, "cancel_32x32.png");
            this.Icon32x32_ImgColl.Images.SetKeyName(34, "Invoice01.png");
            this.Icon32x32_ImgColl.Images.SetKeyName(35, "Invoice02.png");
            this.Icon32x32_ImgColl.InsertImage(global::VTACPluginBase.Properties.Resources.removeitem_32x32, "removeitem_32x32", typeof(global::VTACPluginBase.Properties.Resources), 36);
            this.Icon32x32_ImgColl.Images.SetKeyName(36, "removeitem_32x32");
            // 
            // Editor_TopRbnPg
            // 
            this.Editor_TopRbnPg.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.FuncCreate_RbnPgGrp,
            this.FuncDoc_RbnPgGrp,
            this.Exit_RbnPgGrp});
            this.Editor_TopRbnPg.Name = "Editor_TopRbnPg";
            this.Editor_TopRbnPg.Text = "Editor";
            // 
            // FuncCreate_RbnPgGrp
            // 
            this.FuncCreate_RbnPgGrp.ItemLinks.Add(this.New_BarBtn);
            this.FuncCreate_RbnPgGrp.Name = "FuncCreate_RbnPgGrp";
            this.FuncCreate_RbnPgGrp.Text = "Create";
            // 
            // FuncDoc_RbnPgGrp
            // 
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Edit_BarBtn, true);
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Save_BarBtn, true);
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Terminate_BarBtn, true);
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Complete_BarBtn, true);
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Cancel_BarBtn, true);
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Delete_BarBtn);
            this.FuncDoc_RbnPgGrp.ItemLinks.Add(this.Print_BarBtn, true);
            this.FuncDoc_RbnPgGrp.Name = "FuncDoc_RbnPgGrp";
            this.FuncDoc_RbnPgGrp.Text = "Document";
            // 
            // Exit_RbnPgGrp
            // 
            this.Exit_RbnPgGrp.ItemLinks.Add(this.Close_BarBtn);
            this.Exit_RbnPgGrp.Name = "Exit_RbnPgGrp";
            this.Exit_RbnPgGrp.Text = "Exit";
            // 
            // RepositoryItemFontEdit1
            // 
            this.RepositoryItemFontEdit1.AutoHeight = false;
            this.RepositoryItemFontEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepositoryItemFontEdit1.Name = "RepositoryItemFontEdit1";
            // 
            // RepositoryItemRichEditFontSizeEdit1
            // 
            this.RepositoryItemRichEditFontSizeEdit1.AutoHeight = false;
            this.RepositoryItemRichEditFontSizeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepositoryItemRichEditFontSizeEdit1.Control = null;
            this.RepositoryItemRichEditFontSizeEdit1.Name = "RepositoryItemRichEditFontSizeEdit1";
            // 
            // RepositoryItemBorderLineStyle1
            // 
            this.RepositoryItemBorderLineStyle1.AutoHeight = false;
            this.RepositoryItemBorderLineStyle1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepositoryItemBorderLineStyle1.Control = null;
            this.RepositoryItemBorderLineStyle1.Name = "RepositoryItemBorderLineStyle1";
            // 
            // RepositoryItemBorderLineWeight1
            // 
            this.RepositoryItemBorderLineWeight1.AutoHeight = false;
            this.RepositoryItemBorderLineWeight1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepositoryItemBorderLineWeight1.Control = null;
            this.RepositoryItemBorderLineWeight1.Name = "RepositoryItemBorderLineWeight1";
            // 
            // RepositoryItemFloatingObjectOutlineWeight1
            // 
            this.RepositoryItemFloatingObjectOutlineWeight1.AutoHeight = false;
            this.RepositoryItemFloatingObjectOutlineWeight1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepositoryItemFloatingObjectOutlineWeight1.Control = null;
            this.RepositoryItemFloatingObjectOutlineWeight1.Name = "RepositoryItemFloatingObjectOutlineWeight1";
            // 
            // Bottom_RbnSttBar
            // 
            this.Bottom_RbnSttBar.ItemLinks.Add(this.ProceedAfterSave_BarChkItem);
            this.Bottom_RbnSttBar.ItemLinks.Add(this.General_BarStaticItem);
            this.Bottom_RbnSttBar.ItemLinks.Add(this.Progress_BarStaticItem);
            this.Bottom_RbnSttBar.ItemLinks.Add(this.Progress_BarEditItem);
            this.Bottom_RbnSttBar.Location = new System.Drawing.Point(0, 654);
            this.Bottom_RbnSttBar.Name = "Bottom_RbnSttBar";
            this.Bottom_RbnSttBar.Ribbon = this.Toolbar_RbCtl;
            this.Bottom_RbnSttBar.ShowSizeGrip = false;
            this.Bottom_RbnSttBar.Size = new System.Drawing.Size(1209, 27);
            // 
            // DocDate_Lbl
            // 
            this.DocDate_Lbl.Location = new System.Drawing.Point(587, 48);
            this.DocDate_Lbl.Name = "DocDate_Lbl";
            this.DocDate_Lbl.Size = new System.Drawing.Size(23, 13);
            this.DocDate_Lbl.TabIndex = 48;
            this.DocDate_Lbl.Text = "Date";
            // 
            // DocDate_DteEdit
            // 
            this.DocDate_DteEdit.EditValue = null;
            this.DocDate_DteEdit.Location = new System.Drawing.Point(616, 45);
            this.DocDate_DteEdit.MenuManager = this.Toolbar_RbCtl;
            this.DocDate_DteEdit.Name = "DocDate_DteEdit";
            this.DocDate_DteEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.DocDate_DteEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.DocDate_DteEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DocDate_DteEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DocDate_DteEdit.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.DocDate_DteEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.DocDate_DteEdit.Size = new System.Drawing.Size(128, 20);
            this.DocDate_DteEdit.TabIndex = 11;
            // 
            // DocNo_Lbl
            // 
            this.DocNo_Lbl.Location = new System.Drawing.Point(542, 22);
            this.DocNo_Lbl.Name = "DocNo_Lbl";
            this.DocNo_Lbl.Size = new System.Drawing.Size(68, 13);
            this.DocNo_Lbl.TabIndex = 40;
            this.DocNo_Lbl.Text = "Document No.";
            // 
            // DocNo_TxtEdit
            // 
            this.DocNo_TxtEdit.EditValue = "";
            this.DocNo_TxtEdit.Location = new System.Drawing.Point(616, 19);
            this.DocNo_TxtEdit.MenuManager = this.Toolbar_RbCtl;
            this.DocNo_TxtEdit.Name = "DocNo_TxtEdit";
            this.DocNo_TxtEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.DocNo_TxtEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.DocNo_TxtEdit.Properties.MaxLength = 20;
            this.DocNo_TxtEdit.Properties.NullText = "<<New>>";
            this.DocNo_TxtEdit.Size = new System.Drawing.Size(128, 20);
            this.DocNo_TxtEdit.TabIndex = 10;
            // 
            // Boolean_MyRChkEdit
            // 
            this.Boolean_MyRChkEdit.AutoHeight = false;
            this.Boolean_MyRChkEdit.Caption = "Check";
            this.Boolean_MyRChkEdit.Name = "Boolean_MyRChkEdit";
            // 
            // Shelf_MyRGLEdit
            // 
            this.Shelf_MyRGLEdit.AutoHeight = false;
            this.Shelf_MyRGLEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Shelf_MyRGLEdit.DisplayMember = "SysShelf";
            this.Shelf_MyRGLEdit.Name = "Shelf_MyRGLEdit";
            this.Shelf_MyRGLEdit.NullText = "[Select Shelf]";
            this.Shelf_MyRGLEdit.PopupView = this.RepositoryItemGridLookUpEdit1View;
            this.Shelf_MyRGLEdit.ValueMember = "SysShelf";
            // 
            // RepositoryItemGridLookUpEdit1View
            // 
            this.RepositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.RepositoryItemGridLookUpEdit1View.Name = "RepositoryItemGridLookUpEdit1View";
            this.RepositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.RepositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // Date_MyRDtEdit
            // 
            this.Date_MyRDtEdit.AutoHeight = false;
            this.Date_MyRDtEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Date_MyRDtEdit.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Date_MyRDtEdit.Name = "Date_MyRDtEdit";
            // 
            // LineStatus_MyRGLEditVw
            // 
            this.LineStatus_MyRGLEditVw.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.LineStatus_MyRGLEditVw.Name = "LineStatus_MyRGLEditVw";
            this.LineStatus_MyRGLEditVw.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.LineStatus_MyRGLEditVw.OptionsView.ShowGroupPanel = false;
            // 
            // DtlStatus_MyRGLEdit
            // 
            this.DtlStatus_MyRGLEdit.AutoHeight = false;
            this.DtlStatus_MyRGLEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtlStatus_MyRGLEdit.DisplayMember = "DtlDesc";
            this.DtlStatus_MyRGLEdit.Name = "DtlStatus_MyRGLEdit";
            this.DtlStatus_MyRGLEdit.PopupView = this.LineStatus_MyRGLEditVw;
            this.DtlStatus_MyRGLEdit.ValueMember = "DtlStatus";
            // 
            // SelectItemUOMCF_GCol
            // 
            this.SelectItemUOMCF_GCol.Caption = "Rate";
            this.SelectItemUOMCF_GCol.FieldName = "Rate";
            this.SelectItemUOMCF_GCol.Name = "SelectItemUOMCF_GCol";
            this.SelectItemUOMCF_GCol.Visible = true;
            this.SelectItemUOMCF_GCol.VisibleIndex = 1;
            // 
            // SelectItemUOM_GCol
            // 
            this.SelectItemUOM_GCol.Caption = "UOM";
            this.SelectItemUOM_GCol.FieldName = "UOM";
            this.SelectItemUOM_GCol.Name = "SelectItemUOM_GCol";
            this.SelectItemUOM_GCol.Visible = true;
            this.SelectItemUOM_GCol.VisibleIndex = 0;
            // 
            // SelectItemUOM_MyRGLEditVw
            // 
            this.SelectItemUOM_MyRGLEditVw.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SelectItemUOM_GCol,
            this.SelectItemUOMCF_GCol});
            this.SelectItemUOM_MyRGLEditVw.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.SelectItemUOM_MyRGLEditVw.Name = "SelectItemUOM_MyRGLEditVw";
            this.SelectItemUOM_MyRGLEditVw.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.SelectItemUOM_MyRGLEditVw.OptionsView.ColumnAutoWidth = false;
            this.SelectItemUOM_MyRGLEditVw.OptionsView.ShowGroupPanel = false;
            // 
            // SelectItemUOM_MyRGLEdit
            // 
            this.SelectItemUOM_MyRGLEdit.AutoHeight = false;
            this.SelectItemUOM_MyRGLEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SelectItemUOM_MyRGLEdit.DisplayMember = "UOM";
            this.SelectItemUOM_MyRGLEdit.Name = "SelectItemUOM_MyRGLEdit";
            this.SelectItemUOM_MyRGLEdit.NullText = "[Select UOM here]";
            this.SelectItemUOM_MyRGLEdit.PopupFormMinSize = new System.Drawing.Size(320, 150);
            this.SelectItemUOM_MyRGLEdit.PopupFormSize = new System.Drawing.Size(320, 150);
            this.SelectItemUOM_MyRGLEdit.PopupView = this.SelectItemUOM_MyRGLEditVw;
            this.SelectItemUOM_MyRGLEdit.ValueMember = "UOM";
            // 
            // SelectItemDescription_GCol
            // 
            this.SelectItemDescription_GCol.Caption = "Description";
            this.SelectItemDescription_GCol.FieldName = "Description";
            this.SelectItemDescription_GCol.Name = "SelectItemDescription_GCol";
            this.SelectItemDescription_GCol.Visible = true;
            this.SelectItemDescription_GCol.VisibleIndex = 2;
            this.SelectItemDescription_GCol.Width = 300;
            // 
            // SelectUOM_GCol
            // 
            this.SelectUOM_GCol.Caption = "UOM";
            this.SelectUOM_GCol.FieldName = "UOM";
            this.SelectUOM_GCol.Name = "SelectUOM_GCol";
            this.SelectUOM_GCol.Visible = true;
            this.SelectUOM_GCol.VisibleIndex = 1;
            // 
            // SelectItemNo_GCol
            // 
            this.SelectItemNo_GCol.Caption = "Item Code";
            this.SelectItemNo_GCol.FieldName = "ItemCode";
            this.SelectItemNo_GCol.Name = "SelectItemNo_GCol";
            this.SelectItemNo_GCol.Visible = true;
            this.SelectItemNo_GCol.VisibleIndex = 0;
            this.SelectItemNo_GCol.Width = 120;
            // 
            // Item_MyRGLEditVw
            // 
            this.Item_MyRGLEditVw.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.SelectItemNo_GCol,
            this.SelectUOM_GCol,
            this.SelectItemDescription_GCol});
            this.Item_MyRGLEditVw.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.Item_MyRGLEditVw.Name = "Item_MyRGLEditVw";
            this.Item_MyRGLEditVw.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.Item_MyRGLEditVw.OptionsView.ColumnAutoWidth = false;
            this.Item_MyRGLEditVw.OptionsView.ShowAutoFilterRow = true;
            this.Item_MyRGLEditVw.OptionsView.ShowGroupPanel = false;
            // 
            // Item_MyRGLEdit
            // 
            this.Item_MyRGLEdit.AutoHeight = false;
            this.Item_MyRGLEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Item_MyRGLEdit.DisplayMember = "ItemCode";
            this.Item_MyRGLEdit.Name = "Item_MyRGLEdit";
            this.Item_MyRGLEdit.NullText = "[Select Item]";
            this.Item_MyRGLEdit.PopupFormMinSize = new System.Drawing.Size(495, 0);
            this.Item_MyRGLEdit.PopupFormSize = new System.Drawing.Size(495, 0);
            this.Item_MyRGLEdit.PopupView = this.Item_MyRGLEditVw;
            this.Item_MyRGLEdit.ValueMember = "ItemID";
            // 
            // General_MyPRItems
            // 
            this.General_MyPRItems.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Item_MyRGLEdit,
            this.SelectItemUOM_MyRGLEdit,
            this.DtlStatus_MyRGLEdit,
            this.Date_MyRDtEdit,
            this.Shelf_MyRGLEdit,
            this.Boolean_MyRChkEdit});
            // 
            // Status_Lbl
            // 
            this.Status_Lbl.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.Status_Lbl.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
            this.Status_Lbl.Appearance.Options.UseFont = true;
            this.Status_Lbl.Appearance.Options.UseForeColor = true;
            this.Status_Lbl.Appearance.Options.UseTextOptions = true;
            this.Status_Lbl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Status_Lbl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.Status_Lbl.Location = new System.Drawing.Point(493, 101);
            this.Status_Lbl.Name = "Status_Lbl";
            this.Status_Lbl.Size = new System.Drawing.Size(198, 16);
            this.Status_Lbl.TabIndex = 61;
            this.Status_Lbl.Text = "NEW";
            // 
            // BaseEditor_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 681);
            this.Controls.Add(this.Status_Lbl);
            this.Controls.Add(this.Header_GrpCtrl);
            this.Controls.Add(this.Toolbar_RbCtl);
            this.Controls.Add(this.Bottom_RbnSttBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BaseEditor_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "Installment";
            this.Text = "Installment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseEditor_Form_FormClosing);
            this.Load += new System.EventHandler(this.BaseEditor_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Header_GrpCtrl)).EndInit();
            this.Header_GrpCtrl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Details_Pnl)).EndInit();
            this.Details_Pnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetailsInfo_Pnl)).EndInit();
            this.DetailsInfo_Pnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Details_TabCtrl)).EndInit();
            this.Details_TabCtrl.ResumeLayout(false);
            this.Details_XTabPg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Detail_DXGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Icon16x16_ImgColl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Detail_DXGridVw)).EndInit();
            this.Note_XTabPg.ResumeLayout(false);
            this.Notes_ACMemoEditCtrl.ResumeLayout(false);
            this.Notes_ACMemoEditCtrl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupControlContainer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OprButtons_Pnl)).EndInit();
            this.OprButtons_Pnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Header_TabCtrl)).EndInit();
            this.Header_TabCtrl.ResumeLayout(false);
            this.HeaderInfo_TabPg.ResumeLayout(false);
            this.HeaderInfo_TabPg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Toolbar_RbCtl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Print_PopupMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Progress_RepPBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Icon32x32_ImgColl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemFontEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemRichEditFontSizeEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemBorderLineStyle1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemBorderLineWeight1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemFloatingObjectOutlineWeight1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocDate_DteEdit.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocDate_DteEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DocNo_TxtEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Boolean_MyRChkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Shelf_MyRGLEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Date_MyRDtEdit.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Date_MyRDtEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LineStatus_MyRGLEditVw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtlStatus_MyRGLEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectItemUOM_MyRGLEditVw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectItemUOM_MyRGLEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Item_MyRGLEditVw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Item_MyRGLEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.GroupControl Header_GrpCtrl;
        public DevExpress.XtraTab.XtraTabControl Details_TabCtrl;
        public DevExpress.XtraTab.XtraTabPage Details_XTabPg;
        public DevExpress.XtraGrid.GridControl Detail_DXGrid;
        public DevExpress.Utils.ImageCollection Icon16x16_ImgColl;
        public DevExpress.XtraGrid.Views.Grid.GridView Detail_DXGridVw;
        public DevExpress.XtraGrid.Columns.GridColumn DG_DtlKey_GCol;
        public DevExpress.XtraGrid.Columns.GridColumn DG_DocKey_GCol;
        public DevExpress.XtraTab.XtraTabPage Note_XTabPg;
        internal AutoCount.Controls.MemoEdit Notes_ACMemoEditCtrl;
        private DevExpress.XtraBars.PopupControlContainer popupControlContainer1;
        public DevExpress.XtraTab.XtraTabControl Header_TabCtrl;
        public DevExpress.XtraTab.XtraTabPage HeaderInfo_TabPg;
        public DevExpress.XtraBars.Ribbon.RibbonControl Toolbar_RbCtl;
        internal DevExpress.XtraBars.BarButtonItem Save_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem SaveAndClose_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Print_BarBtn;
        internal DevExpress.XtraBars.PopupMenu Print_PopupMenu;
        internal DevExpress.XtraBars.BarButtonItem PrintWithPrinterDialog_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem PrintWithDesigner_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Delete_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Activate_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Deactivate_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Complete_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Close_BarBtn;
        internal DevExpress.XtraBars.BarStaticItem General_BarStaticItem;
        internal DevExpress.XtraBars.BarStaticItem Progress_BarStaticItem;
        internal DevExpress.XtraBars.BarEditItem Progress_BarEditItem;
        internal DevExpress.XtraEditors.Repository.RepositoryItemProgressBar Progress_RepPBar;
        internal DevExpress.XtraBars.BarButtonItem PrintWithDefaultPrinter_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem Edit_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem GenerateACDocs_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem OpenAutocountDoc_BarBtn;
        internal DevExpress.XtraBars.BarButtonItem New_BarBtn;
        // [modified] by scchang's Claude Sonnet 4.6 on 20260309: v2.2.1.0, Changed internal → protected so derived forms in other assemblies can set Caption/Tag
        protected DevExpress.XtraBars.BarCheckItem ProceedAfterSave_BarChkItem;
        internal DevExpress.XtraBars.BarButtonGroup Print_BarBtnGrp;
        internal DevExpress.XtraBars.BarButtonItem Cancel_BarBtn;
        internal DevExpress.Utils.ImageCollection Icon32x32_ImgColl;
        public DevExpress.XtraBars.Ribbon.RibbonPage Editor_TopRbnPg;
        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup FuncCreate_RbnPgGrp;
        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup FuncDoc_RbnPgGrp;
        internal DevExpress.XtraBars.Ribbon.RibbonPageGroup Exit_RbnPgGrp;
        internal DevExpress.XtraEditors.Repository.RepositoryItemFontEdit RepositoryItemFontEdit1;
        internal DevExpress.XtraRichEdit.Design.RepositoryItemRichEditFontSizeEdit RepositoryItemRichEditFontSizeEdit1;
        internal DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle RepositoryItemBorderLineStyle1;
        internal DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineWeight RepositoryItemBorderLineWeight1;
        internal DevExpress.XtraRichEdit.Forms.Design.RepositoryItemFloatingObjectOutlineWeight RepositoryItemFloatingObjectOutlineWeight1;
        internal DevExpress.XtraBars.Ribbon.RibbonStatusBar Bottom_RbnSttBar;
        internal DevExpress.XtraEditors.LabelControl DocDate_Lbl;
        internal DevExpress.XtraEditors.DateEdit DocDate_DteEdit;
        internal DevExpress.XtraEditors.LabelControl DocNo_Lbl;
        internal DevExpress.XtraEditors.TextEdit DocNo_TxtEdit;
        internal DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit Boolean_MyRChkEdit;
        internal DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit Shelf_MyRGLEdit;
        internal DevExpress.XtraGrid.Views.Grid.GridView RepositoryItemGridLookUpEdit1View;
        internal DevExpress.XtraEditors.Repository.RepositoryItemDateEdit Date_MyRDtEdit;
        internal DevExpress.XtraGrid.Views.Grid.GridView LineStatus_MyRGLEditVw;
        internal DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit DtlStatus_MyRGLEdit;
        internal DevExpress.XtraGrid.Columns.GridColumn SelectItemUOMCF_GCol;
        internal DevExpress.XtraGrid.Columns.GridColumn SelectItemUOM_GCol;
        internal DevExpress.XtraGrid.Views.Grid.GridView SelectItemUOM_MyRGLEditVw;
        internal DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit SelectItemUOM_MyRGLEdit;
        internal DevExpress.XtraGrid.Columns.GridColumn SelectItemDescription_GCol;
        internal DevExpress.XtraGrid.Columns.GridColumn SelectUOM_GCol;
        internal DevExpress.XtraGrid.Columns.GridColumn SelectItemNo_GCol;
        internal DevExpress.XtraGrid.Views.Grid.GridView Item_MyRGLEditVw;
        internal DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit Item_MyRGLEdit;
        internal DevExpress.XtraEditors.Repository.PersistentRepository General_MyPRItems;
        public DevExpress.XtraEditors.PanelControl Details_Pnl;
        public DevExpress.XtraEditors.PanelControl DetailsInfo_Pnl;
        public DevExpress.XtraEditors.PanelControl OprButtons_Pnl;
        private DevExpress.XtraEditors.SimpleButton AddDetail_SpBtn;
        private DevExpress.XtraEditors.SimpleButton DeleteDetail_SpBtn;
        private DevExpress.XtraEditors.SimpleButton EditDetail_SpBtn;
        internal DevExpress.XtraEditors.LabelControl Status_Lbl;
        private DevExpress.XtraGrid.Columns.GridColumn DG_DtlStatus_GCol;
        internal DevExpress.XtraBars.BarButtonItem Terminate_BarBtn;
    }
}