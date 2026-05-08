namespace ATPShadowMain
{
    partial class ShadowLauncherV2_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Chrome
        private DevExpress.XtraEditors.PanelControl PanelTop;
        private DevExpress.XtraEditors.LabelControl LblBreadcrumb;
        private DevExpress.XtraEditors.SimpleButton BtnRefresh;
        private DevExpress.XtraNavBar.NavBarControl NavLeft;
        private DevExpress.XtraTab.XtraTabControl TabsMain;
        private DevExpress.XtraEditors.LabelControl LblStatus;

        // Master tab dashboard scaffold
        private DevExpress.XtraTab.XtraTabPage TabPageMaster;
        private DevExpress.XtraEditors.PanelControl PanelDashboard;
        private DevExpress.XtraEditors.LabelControl LblTitle;
        private DevExpress.XtraEditors.LabelControl LblSubtitle;
        private DevExpress.XtraEditors.LabelControl LblQuickAccess;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelTop = new DevExpress.XtraEditors.PanelControl();
            this.LblBreadcrumb = new DevExpress.XtraEditors.LabelControl();
            this.BtnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.NavLeft = new DevExpress.XtraNavBar.NavBarControl();
            this.TabsMain = new DevExpress.XtraTab.XtraTabControl();
            this.TabPageMaster = new DevExpress.XtraTab.XtraTabPage();
            this.PanelDashboard = new DevExpress.XtraEditors.PanelControl();
            this.LblTitle = new DevExpress.XtraEditors.LabelControl();
            this.LblSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.LblQuickAccess = new DevExpress.XtraEditors.LabelControl();
            this.LblStatus = new DevExpress.XtraEditors.LabelControl();

            ((System.ComponentModel.ISupportInitialize)(this.PanelTop)).BeginInit();
            this.PanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NavLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabsMain)).BeginInit();
            this.TabsMain.SuspendLayout();
            this.TabPageMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelDashboard)).BeginInit();
            this.PanelDashboard.SuspendLayout();
            this.SuspendLayout();

            // PanelTop
            this.PanelTop.Appearance.BackColor = System.Drawing.Color.FromArgb(124, 179, 66);
            this.PanelTop.Appearance.Options.UseBackColor = true;
            this.PanelTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelTop.Controls.Add(this.LblBreadcrumb);
            this.PanelTop.Controls.Add(this.BtnRefresh);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(1280, 50);
            this.PanelTop.TabIndex = 0;

            // LblBreadcrumb
            this.LblBreadcrumb.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.LblBreadcrumb.Appearance.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.LblBreadcrumb.Appearance.ForeColor = System.Drawing.Color.White;
            this.LblBreadcrumb.Appearance.Options.UseBackColor = true;
            this.LblBreadcrumb.Appearance.Options.UseFont = true;
            this.LblBreadcrumb.Appearance.Options.UseForeColor = true;
            this.LblBreadcrumb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LblBreadcrumb.Location = new System.Drawing.Point(20, 14);
            this.LblBreadcrumb.Name = "LblBreadcrumb";
            this.LblBreadcrumb.Size = new System.Drawing.Size(900, 28);
            this.LblBreadcrumb.TabIndex = 0;
            this.LblBreadcrumb.Text = "ATP  /  Master Menu";

            // BtnRefresh
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRefresh.Location = new System.Drawing.Point(1170, 11);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(90, 28);
            this.BtnRefresh.TabIndex = 1;
            this.BtnRefresh.Text = "Refresh";

            // NavLeft
            this.NavLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.NavLeft.Location = new System.Drawing.Point(0, 50);
            this.NavLeft.Name = "NavLeft";
            this.NavLeft.OptionsNavPane.ExpandedWidth = 240;
            this.NavLeft.OptionsNavPane.ShowExpandButton = false;
            this.NavLeft.OptionsNavPane.ShowOverflowButton = false;
            this.NavLeft.PaintStyleKind = DevExpress.XtraNavBar.NavBarViewKind.NavigationPane;
            this.NavLeft.Size = new System.Drawing.Size(240, 728);
            this.NavLeft.TabIndex = 1;
            this.NavLeft.Text = "navBarControl";

            // TabsMain
            this.TabsMain.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.TabsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsMain.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Top;
            this.TabsMain.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.TabsMain.Location = new System.Drawing.Point(240, 50);
            this.TabsMain.Name = "TabsMain";
            this.TabsMain.SelectedTabPage = this.TabPageMaster;
            this.TabsMain.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.TabsMain.Size = new System.Drawing.Size(1040, 728);
            this.TabsMain.TabIndex = 2;
            this.TabsMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
                this.TabPageMaster});

            // TabPageMaster
            this.TabPageMaster.Controls.Add(this.PanelDashboard);
            this.TabPageMaster.Name = "TabPageMaster";
            this.TabPageMaster.ShowCloseButton = DevExpress.Utils.DefaultBoolean.False;
            this.TabPageMaster.Size = new System.Drawing.Size(1034, 700);
            this.TabPageMaster.Text = "Master Menu";

            // PanelDashboard
            this.PanelDashboard.Appearance.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.PanelDashboard.Appearance.Options.UseBackColor = true;
            this.PanelDashboard.AutoScroll = true;
            this.PanelDashboard.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelDashboard.Controls.Add(this.LblTitle);
            this.PanelDashboard.Controls.Add(this.LblSubtitle);
            this.PanelDashboard.Controls.Add(this.LblQuickAccess);
            this.PanelDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDashboard.Location = new System.Drawing.Point(0, 0);
            this.PanelDashboard.Name = "PanelDashboard";
            this.PanelDashboard.Size = new System.Drawing.Size(1034, 700);
            this.PanelDashboard.TabIndex = 0;

            // LblTitle
            this.LblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(40, 60, 110);
            this.LblTitle.Appearance.Options.UseFont = true;
            this.LblTitle.Appearance.Options.UseForeColor = true;
            this.LblTitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LblTitle.Location = new System.Drawing.Point(24, 18);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(800, 32);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Service & Contract Dashboard";

            // LblSubtitle
            this.LblSubtitle.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.LblSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(110, 120, 140);
            this.LblSubtitle.Appearance.Options.UseFont = true;
            this.LblSubtitle.Appearance.Options.UseForeColor = true;
            this.LblSubtitle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LblSubtitle.Location = new System.Drawing.Point(24, 52);
            this.LblSubtitle.Name = "LblSubtitle";
            this.LblSubtitle.Size = new System.Drawing.Size(800, 18);
            this.LblSubtitle.TabIndex = 1;
            this.LblSubtitle.Text = "Live counts from your AED_ATPLUGIN001 database. Click a tile below for one-click access.";

            // LblQuickAccess
            this.LblQuickAccess.Appearance.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.LblQuickAccess.Appearance.ForeColor = System.Drawing.Color.FromArgb(40, 60, 110);
            this.LblQuickAccess.Appearance.Options.UseFont = true;
            this.LblQuickAccess.Appearance.Options.UseForeColor = true;
            this.LblQuickAccess.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LblQuickAccess.Location = new System.Drawing.Point(24, 208);
            this.LblQuickAccess.Name = "LblQuickAccess";
            this.LblQuickAccess.Size = new System.Drawing.Size(400, 26);
            this.LblQuickAccess.TabIndex = 2;
            this.LblQuickAccess.Text = "Quick Access";

            // LblStatus
            this.LblStatus.Appearance.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.LblStatus.Appearance.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.LblStatus.Appearance.Options.UseBackColor = true;
            this.LblStatus.Appearance.Options.UseForeColor = true;
            this.LblStatus.Appearance.Options.UseTextOptions = true;
            this.LblStatus.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.LblStatus.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LblStatus.Location = new System.Drawing.Point(0, 778);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Padding = new System.Windows.Forms.Padding(8, 4, 0, 0);
            this.LblStatus.Size = new System.Drawing.Size(1280, 22);
            this.LblStatus.TabIndex = 3;
            this.LblStatus.Text = "User: —    DB: —    Open tabs: 0";

            // ShadowLauncherV2_Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.TabsMain);
            this.Controls.Add(this.NavLeft);
            this.Controls.Add(this.PanelTop);
            this.Controls.Add(this.LblStatus);
            this.MinimumSize = new System.Drawing.Size(1024, 640);
            this.Name = "ShadowLauncherV2_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ATP Shadow Launcher V2 (tabbed)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            this.PanelDashboard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelDashboard)).EndInit();
            this.TabPageMaster.ResumeLayout(false);
            this.TabsMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabsMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NavLeft)).EndInit();
            this.PanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelTop)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
