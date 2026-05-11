namespace VTACPluginBase
{
    partial class MainPI_Form : System.Windows.Forms.Form
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPI_Form));
            this.Plugin_BarMgr = new DevExpress.XtraBars.BarManager(this.components);
            this.Menu_Bar = new DevExpress.XtraBars.Bar();
            this.Master_BarSItm = new DevExpress.XtraBars.BarSubItem();
            this.Mas_BarButtonItem01 = new DevExpress.XtraBars.BarButtonItem();
            this.Mas_BarButtonItem02 = new DevExpress.XtraBars.BarButtonItem();
            this.Mas_BarButtonItem03 = new DevExpress.XtraBars.BarButtonItem();
            this.Operations_BarSItm = new DevExpress.XtraBars.BarSubItem();
            this.Opr_BarButtonItem01 = new DevExpress.XtraBars.BarButtonItem();
            this.Opr_BarButtonItem02 = new DevExpress.XtraBars.BarButtonItem();
            this.Opr_BarButtonItem03 = new DevExpress.XtraBars.BarButtonItem();
            this.Opr_BarButtonItem04 = new DevExpress.XtraBars.BarButtonItem();
            this.Config_BarSItm = new DevExpress.XtraBars.BarSubItem();
            this.Oth_BarButtonItem01 = new DevExpress.XtraBars.BarButtonItem();
            this.ExitPI_BarSItm = new DevExpress.XtraBars.BarSubItem();
            this.Status_Bar = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.Plugin_NavBar = new DevExpress.XtraNavBar.NavBarControl();
            this.Master_NavBarGrp = new DevExpress.XtraNavBar.NavBarGroup();
            this.Mas_NavBarItem01 = new DevExpress.XtraNavBar.NavBarItem();
            this.Mas_NavBarItem02 = new DevExpress.XtraNavBar.NavBarItem();
            this.Mas_NavBarItem03 = new DevExpress.XtraNavBar.NavBarItem();
            this.Operation_NavBarGrp = new DevExpress.XtraNavBar.NavBarGroup();
            this.Opr_NavBarItem01 = new DevExpress.XtraNavBar.NavBarItem();
            this.Opr_NavBarItem02 = new DevExpress.XtraNavBar.NavBarItem();
            this.Opr_NavBarItem03 = new DevExpress.XtraNavBar.NavBarItem();
            this.Opr_NavBarItem04 = new DevExpress.XtraNavBar.NavBarItem();
            this.Exit_NavBarGrp = new DevExpress.XtraNavBar.NavBarGroup();
            this.MainPIForm_XTabbedMdiMgr = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.Refresh_Tmr = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Plugin_BarMgr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Plugin_NavBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainPIForm_XTabbedMdiMgr)).BeginInit();
            this.SuspendLayout();
            // 
            // Plugin_BarMgr
            // 
            this.Plugin_BarMgr.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.Menu_Bar,
            this.Status_Bar});
            this.Plugin_BarMgr.DockControls.Add(this.barDockControlTop);
            this.Plugin_BarMgr.DockControls.Add(this.barDockControlBottom);
            this.Plugin_BarMgr.DockControls.Add(this.barDockControlLeft);
            this.Plugin_BarMgr.DockControls.Add(this.barDockControlRight);
            this.Plugin_BarMgr.Form = this;
            this.Plugin_BarMgr.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barStaticItem1,
            this.barStaticItem2,
            this.ExitPI_BarSItm,
            this.Config_BarSItm,
            this.Oth_BarButtonItem01,
            this.Master_BarSItm,
            this.Operations_BarSItm,
            this.Mas_BarButtonItem01,
            this.Mas_BarButtonItem02,
            this.Mas_BarButtonItem03,
            this.Opr_BarButtonItem01,
            this.Opr_BarButtonItem02,
            this.Opr_BarButtonItem03,
            this.Opr_BarButtonItem04});
            this.Plugin_BarMgr.MainMenu = this.Menu_Bar;
            this.Plugin_BarMgr.MaxItemId = 14;
            this.Plugin_BarMgr.StatusBar = this.Status_Bar;
            // 
            // Menu_Bar
            // 
            this.Menu_Bar.BarName = "Main menu";
            this.Menu_Bar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.Menu_Bar.DockCol = 0;
            this.Menu_Bar.DockRow = 0;
            this.Menu_Bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.Menu_Bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Master_BarSItm),
            new DevExpress.XtraBars.LinkPersistInfo(this.Operations_BarSItm),
            new DevExpress.XtraBars.LinkPersistInfo(this.Config_BarSItm),
            new DevExpress.XtraBars.LinkPersistInfo(this.ExitPI_BarSItm)});
            this.Menu_Bar.OptionsBar.AllowQuickCustomization = false;
            this.Menu_Bar.OptionsBar.DisableCustomization = true;
            this.Menu_Bar.OptionsBar.MultiLine = true;
            this.Menu_Bar.OptionsBar.UseWholeRow = true;
            this.Menu_Bar.Text = "Main menu";
            // 
            // Master_BarSItm
            // 
            this.Master_BarSItm.Caption = "Master";
            this.Master_BarSItm.Hint = "Master";
            this.Master_BarSItm.Id = 5;
            this.Master_BarSItm.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Mas_BarButtonItem01),
            new DevExpress.XtraBars.LinkPersistInfo(this.Mas_BarButtonItem02),
            new DevExpress.XtraBars.LinkPersistInfo(this.Mas_BarButtonItem03)});
            this.Master_BarSItm.Name = "Master_BarSItm";
            this.Master_BarSItm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.Master_BarSItm.VisibleInSearchMenu = false;
            // 
            // Mas_BarButtonItem01
            // 
            this.Mas_BarButtonItem01.Caption = "<Master 01>";
            this.Mas_BarButtonItem01.Id = 7;
            this.Mas_BarButtonItem01.Name = "Mas_BarButtonItem01";
            this.Mas_BarButtonItem01.Tag = "";
            this.Mas_BarButtonItem01.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Mas_BarButtonItem01_ItemClick);
            // 
            // Mas_BarButtonItem02
            // 
            this.Mas_BarButtonItem02.Caption = "<Master 02>";
            this.Mas_BarButtonItem02.Id = 8;
            this.Mas_BarButtonItem02.Name = "Mas_BarButtonItem02";
            this.Mas_BarButtonItem02.Tag = "";
            this.Mas_BarButtonItem02.VisibleInSearchMenu = false;
            this.Mas_BarButtonItem02.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Mas_BarButtonItem02_ItemClick);
            // 
            // Mas_BarButtonItem03
            // 
            this.Mas_BarButtonItem03.Caption = "<Master 03>";
            this.Mas_BarButtonItem03.Id = 9;
            this.Mas_BarButtonItem03.Name = "Mas_BarButtonItem03";
            this.Mas_BarButtonItem03.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.Mas_BarButtonItem03.VisibleInSearchMenu = false;
            this.Mas_BarButtonItem03.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Mas_BarButtonItem03_ItemClick);
            // 
            // Operations_BarSItm
            // 
            this.Operations_BarSItm.Caption = "Operations";
            this.Operations_BarSItm.Hint = "Operations";
            this.Operations_BarSItm.Id = 6;
            this.Operations_BarSItm.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Opr_BarButtonItem01),
            new DevExpress.XtraBars.LinkPersistInfo(this.Opr_BarButtonItem02),
            new DevExpress.XtraBars.LinkPersistInfo(this.Opr_BarButtonItem03),
            new DevExpress.XtraBars.LinkPersistInfo(this.Opr_BarButtonItem04)});
            this.Operations_BarSItm.Name = "Operations_BarSItm";
            // 
            // Opr_BarButtonItem01
            // 
            this.Opr_BarButtonItem01.Caption = "Installment Detail";
            this.Opr_BarButtonItem01.Id = 10;
            this.Opr_BarButtonItem01.ImageOptions.Image = global::VTACPluginBase.Properties.Resources.Installment_32;
            this.Opr_BarButtonItem01.Name = "Opr_BarButtonItem01";
            this.Opr_BarButtonItem01.Tag = "InstallmentLst_Form";
            this.Opr_BarButtonItem01.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Opr_BarButtonItem01_ItemClick);
            // 
            // Opr_BarButtonItem02
            // 
            this.Opr_BarButtonItem02.Caption = "Installment Summary Report";
            this.Opr_BarButtonItem02.Id = 11;
            this.Opr_BarButtonItem02.ImageOptions.Image = global::VTACPluginBase.Properties.Resources.SummaryReport_32;
            this.Opr_BarButtonItem02.Name = "Opr_BarButtonItem02";
            this.Opr_BarButtonItem02.Tag = "InstallmentSummLst_Form";
            this.Opr_BarButtonItem02.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Opr_BarButtonItem02_ItemClick);
            // 
            // Opr_BarButtonItem03
            // 
            this.Opr_BarButtonItem03.Caption = "Import Installment (From Excel)";
            this.Opr_BarButtonItem03.Id = 12;
            this.Opr_BarButtonItem03.ImageOptions.Image = global::VTACPluginBase.Properties.Resources.ImpXls_32;
            this.Opr_BarButtonItem03.Name = "Opr_BarButtonItem03";
            this.Opr_BarButtonItem03.Tag = "ImpXls2ITLst_Form";
            this.Opr_BarButtonItem03.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Opr_BarButtonItem03_ItemClick);
            // 
            // Opr_BarButtonItem04
            // 
            this.Opr_BarButtonItem04.Caption = "<Operation 04>";
            this.Opr_BarButtonItem04.Id = 13;
            this.Opr_BarButtonItem04.Name = "Opr_BarButtonItem04";
            this.Opr_BarButtonItem04.Tag = "";
            this.Opr_BarButtonItem04.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Opr_BarButtonItem04_ItemClick);
            // 
            // Config_BarSItm
            // 
            this.Config_BarSItm.Caption = "Configurations";
            this.Config_BarSItm.Hint = "Configuration Settings";
            this.Config_BarSItm.Id = 3;
            this.Config_BarSItm.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Oth_BarButtonItem01)});
            this.Config_BarSItm.Name = "Config_BarSItm";
            // 
            // Oth_BarButtonItem01
            // 
            this.Oth_BarButtonItem01.Caption = "Settings";
            this.Oth_BarButtonItem01.Hint = "Settings Configuration Form";
            this.Oth_BarButtonItem01.Id = 4;
            this.Oth_BarButtonItem01.ImageOptions.Image = global::VTACPluginBase.Properties.Resources.Control_Panel_1;
            this.Oth_BarButtonItem01.Name = "Oth_BarButtonItem01";
            this.Oth_BarButtonItem01.Tag = "Setting_Form";
            this.Oth_BarButtonItem01.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Oth_BarButtonItem01_ItemClick);
            // 
            // ExitPI_BarSItm
            // 
            this.ExitPI_BarSItm.Caption = "Exit Plugin";
            this.ExitPI_BarSItm.Hint = "Exit Plugin";
            this.ExitPI_BarSItm.Id = 2;
            this.ExitPI_BarSItm.ImageOptions.Image = global::VTACPluginBase.Properties.Resources.Close_16;
            this.ExitPI_BarSItm.Name = "ExitPI_BarSItm";
            this.ExitPI_BarSItm.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.ExitPI_BarSItm.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ExitPI_BarSItm_ItemClick);
            // 
            // Status_Bar
            // 
            this.Status_Bar.BarName = "Status bar";
            this.Status_Bar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.Status_Bar.DockCol = 0;
            this.Status_Bar.DockRow = 0;
            this.Status_Bar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.Status_Bar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2)});
            this.Status_Bar.OptionsBar.AllowQuickCustomization = false;
            this.Status_Bar.OptionsBar.DrawDragBorder = false;
            this.Status_Bar.OptionsBar.UseWholeRow = true;
            this.Status_Bar.Text = "Status bar";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barStaticItem1.Caption = "User ID:";
            this.barStaticItem1.Id = 0;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem2.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barStaticItem2.Caption = "00:00:00 AM/PM";
            this.barStaticItem2.Id = 1;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.Plugin_BarMgr;
            this.barDockControlTop.Size = new System.Drawing.Size(849, 25);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 699);
            this.barDockControlBottom.Manager = this.Plugin_BarMgr;
            this.barDockControlBottom.Size = new System.Drawing.Size(849, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 25);
            this.barDockControlLeft.Manager = this.Plugin_BarMgr;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 674);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(849, 25);
            this.barDockControlRight.Manager = this.Plugin_BarMgr;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 674);
            // 
            // Plugin_NavBar
            // 
            this.Plugin_NavBar.ActiveGroup = this.Master_NavBarGrp;
            this.Plugin_NavBar.Appearance.Item.Font = new System.Drawing.Font("Tahoma", 11F);
            this.Plugin_NavBar.Appearance.Item.Options.UseFont = true;
            this.Plugin_NavBar.Appearance.ItemHotTracked.Font = new System.Drawing.Font("Tahoma", 12F);
            this.Plugin_NavBar.Appearance.ItemHotTracked.Options.UseFont = true;
            this.Plugin_NavBar.Appearance.ItemPressed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Underline);
            this.Plugin_NavBar.Appearance.ItemPressed.Options.UseFont = true;
            this.Plugin_NavBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.Plugin_NavBar.ExplorerBarShowGroupButtons = false;
            this.Plugin_NavBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Plugin_NavBar.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.Master_NavBarGrp,
            this.Operation_NavBarGrp,
            this.Exit_NavBarGrp});
            this.Plugin_NavBar.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.Mas_NavBarItem01,
            this.Mas_NavBarItem02,
            this.Mas_NavBarItem03,
            this.Opr_NavBarItem01,
            this.Opr_NavBarItem02,
            this.Opr_NavBarItem03,
            this.Opr_NavBarItem04});
            this.Plugin_NavBar.Location = new System.Drawing.Point(0, 25);
            this.Plugin_NavBar.Name = "Plugin_NavBar";
            this.Plugin_NavBar.OptionsNavPane.ExpandedWidth = 214;
            this.Plugin_NavBar.OptionsNavPane.ShowExpandButton = false;
            this.Plugin_NavBar.OptionsNavPane.ShowOverflowPanel = false;
            this.Plugin_NavBar.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.Plugin_NavBar.Size = new System.Drawing.Size(214, 674);
            this.Plugin_NavBar.TabIndex = 5;
            this.Plugin_NavBar.Text = "Plugin Navigation Bar";
            this.Plugin_NavBar.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinExplorerBarViewInfoRegistrator("Visual Studio 2013 Light");
            this.Plugin_NavBar.ActiveGroupChanged += new DevExpress.XtraNavBar.NavBarGroupEventHandler(this.Plugin_NavBar_ActiveGroupChanged);
            // 
            // Master_NavBarGrp
            // 
            this.Master_NavBarGrp.Caption = "Master Menu";
            this.Master_NavBarGrp.DragDropFlags = DevExpress.XtraNavBar.NavBarDragDrop.None;
            this.Master_NavBarGrp.Expanded = true;
            this.Master_NavBarGrp.Hint = "Coil Work Order Master Menu";
            this.Master_NavBarGrp.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.Mas_NavBarItem01),
            new DevExpress.XtraNavBar.NavBarItemLink(this.Mas_NavBarItem02),
            new DevExpress.XtraNavBar.NavBarItemLink(this.Mas_NavBarItem03)});
            this.Master_NavBarGrp.Name = "Master_NavBarGrp";
            this.Master_NavBarGrp.Visible = false;
            // 
            // Mas_NavBarItem01
            // 
            this.Mas_NavBarItem01.Caption = "<Master 01>";
            this.Mas_NavBarItem01.Hint = "<Master 01> Listing";
            this.Mas_NavBarItem01.Name = "Mas_NavBarItem01";
            this.Mas_NavBarItem01.Tag = "MachineLst_Form";
            this.Mas_NavBarItem01.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Mas_NavBarItem01_LinkClicked);
            // 
            // Mas_NavBarItem02
            // 
            this.Mas_NavBarItem02.Caption = "<Master 02>";
            this.Mas_NavBarItem02.Hint = "<Master 02> Listing";
            this.Mas_NavBarItem02.Name = "Mas_NavBarItem02";
            this.Mas_NavBarItem02.Tag = "BulkItemLst_Form";
            this.Mas_NavBarItem02.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Mas_NavBarItem02_LinkClicked);
            // 
            // Mas_NavBarItem03
            // 
            this.Mas_NavBarItem03.Caption = "<Master 03>";
            this.Mas_NavBarItem03.Hint = "<Master 03> Listing";
            this.Mas_NavBarItem03.Name = "Mas_NavBarItem03";
            this.Mas_NavBarItem03.Tag = "Master03_Form";
            this.Mas_NavBarItem03.Visible = false;
            this.Mas_NavBarItem03.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Mas_NavBarItem03_LinkClicked);
            // 
            // Operation_NavBarGrp
            // 
            this.Operation_NavBarGrp.Caption = "Operation Menu";
            this.Operation_NavBarGrp.DragDropFlags = DevExpress.XtraNavBar.NavBarDragDrop.None;
            this.Operation_NavBarGrp.Expanded = true;
            this.Operation_NavBarGrp.Hint = "Coil Work Order Operation Menu";
            this.Operation_NavBarGrp.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.Opr_NavBarItem01),
            new DevExpress.XtraNavBar.NavBarItemLink(this.Opr_NavBarItem02),
            new DevExpress.XtraNavBar.NavBarItemLink(this.Opr_NavBarItem03),
            new DevExpress.XtraNavBar.NavBarItemLink(this.Opr_NavBarItem04)});
            this.Operation_NavBarGrp.Name = "Operation_NavBarGrp";
            // 
            // Opr_NavBarItem01
            // 
            this.Opr_NavBarItem01.Caption = "Installment Detail";
            this.Opr_NavBarItem01.Hint = "Installment Detail Listing";
            this.Opr_NavBarItem01.ImageOptions.SmallImage = global::VTACPluginBase.Properties.Resources.Installment_32;
            this.Opr_NavBarItem01.Name = "Opr_NavBarItem01";
            this.Opr_NavBarItem01.Tag = "InstallmentLst_Form";
            this.Opr_NavBarItem01.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Opr_NavBarItem01_LinkClicked);
            // 
            // Opr_NavBarItem02
            // 
            this.Opr_NavBarItem02.Caption = "Installment Summary Report";
            this.Opr_NavBarItem02.Hint = "Installment Summary Report";
            this.Opr_NavBarItem02.ImageOptions.SmallImage = global::VTACPluginBase.Properties.Resources.SummaryReport_32;
            this.Opr_NavBarItem02.Name = "Opr_NavBarItem02";
            this.Opr_NavBarItem02.Tag = "InstallmentSummLst_Form";
            this.Opr_NavBarItem02.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Opr_NavBarItem02_LinkClicked);
            // 
            // Opr_NavBarItem03
            // 
            this.Opr_NavBarItem03.Caption = "Import Installment (From Excel)";
            this.Opr_NavBarItem03.Hint = "Import Installment (From Excel) Listing";
            this.Opr_NavBarItem03.ImageOptions.SmallImage = global::VTACPluginBase.Properties.Resources.ImpXls_32;
            this.Opr_NavBarItem03.Name = "Opr_NavBarItem03";
            this.Opr_NavBarItem03.Tag = "ImpXls2ITLst_Form";
            this.Opr_NavBarItem03.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Opr_NavBarItem03_LinkClicked);
            // 
            // Opr_NavBarItem04
            // 
            this.Opr_NavBarItem04.Caption = "<Operation 04>";
            this.Opr_NavBarItem04.Hint = "<Operation 04> Listing";
            this.Opr_NavBarItem04.Name = "Opr_NavBarItem04";
            this.Opr_NavBarItem04.Tag = "";
            this.Opr_NavBarItem04.Visible = false;
            this.Opr_NavBarItem04.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(this.Opr_NavBarItem04_LinkClicked);
            // 
            // Exit_NavBarGrp
            // 
            this.Exit_NavBarGrp.Caption = "Exit Plugin Modules";
            this.Exit_NavBarGrp.Hint = "Exit this Plug Ins";
            this.Exit_NavBarGrp.Name = "Exit_NavBarGrp";
            this.Exit_NavBarGrp.Visible = false;
            // 
            // MainPIForm_XTabbedMdiMgr
            // 
            this.MainPIForm_XTabbedMdiMgr.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeaderAndOnMouseHover;
            this.MainPIForm_XTabbedMdiMgr.MdiParent = this;
            this.MainPIForm_XTabbedMdiMgr.PinPageButtonShowMode = DevExpress.XtraTab.PinPageButtonShowMode.InActiveTabPageHeader;
            // 
            // Refresh_Tmr
            // 
            this.Refresh_Tmr.Interval = 1000;
            this.Refresh_Tmr.Tick += new System.EventHandler(this.Refresh_Tmr_Tick);
            // 
            // MainPI_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 722);
            this.Controls.Add(this.Plugin_NavBar);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainPI_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installment Plug Ins Main Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainPI_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Plugin_BarMgr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Plugin_NavBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainPIForm_XTabbedMdiMgr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager Plugin_BarMgr;
        private DevExpress.XtraBars.Bar Menu_Bar;
        private DevExpress.XtraBars.Bar Status_Bar;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraNavBar.NavBarControl Plugin_NavBar;
        private DevExpress.XtraNavBar.NavBarGroup Master_NavBarGrp;
        private DevExpress.XtraNavBar.NavBarGroup Operation_NavBarGrp;
        private DevExpress.XtraNavBar.NavBarGroup Exit_NavBarGrp;
        private DevExpress.XtraNavBar.NavBarItem Opr_NavBarItem01;
        private DevExpress.XtraNavBar.NavBarItem Opr_NavBarItem02;
        private DevExpress.XtraNavBar.NavBarItem Opr_NavBarItem03;
        private DevExpress.XtraNavBar.NavBarItem Mas_NavBarItem01;
        private DevExpress.XtraNavBar.NavBarItem Mas_NavBarItem02;
        private DevExpress.XtraNavBar.NavBarItem Mas_NavBarItem03;
        internal DevExpress.XtraTabbedMdi.XtraTabbedMdiManager MainPIForm_XTabbedMdiMgr;
        internal System.Windows.Forms.Timer Refresh_Tmr;
        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarSubItem ExitPI_BarSItm;
        private DevExpress.XtraBars.BarSubItem Config_BarSItm;
        private DevExpress.XtraBars.BarSubItem Master_BarSItm;
        private DevExpress.XtraBars.BarSubItem Operations_BarSItm;
        private DevExpress.XtraBars.BarButtonItem Oth_BarButtonItem01;
        private DevExpress.XtraBars.BarButtonItem Mas_BarButtonItem01;
        private DevExpress.XtraBars.BarButtonItem Mas_BarButtonItem02;
        private DevExpress.XtraBars.BarButtonItem Mas_BarButtonItem03;
        private DevExpress.XtraBars.BarButtonItem Opr_BarButtonItem01;
        private DevExpress.XtraBars.BarButtonItem Opr_BarButtonItem02;
        private DevExpress.XtraBars.BarButtonItem Opr_BarButtonItem03;
        private DevExpress.XtraBars.BarButtonItem Opr_BarButtonItem04;
        private DevExpress.XtraNavBar.NavBarItem Opr_NavBarItem04;
    }
}

