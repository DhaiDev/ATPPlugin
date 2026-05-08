namespace ServiceContractPhotocopier.StockRequest.OperationForms
{
    partial class StockRequestIntegration_Form
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

        // ---- Title bar ----
        private DevExpress.XtraEditors.PanelControl PanelTitle;
        private DevExpress.XtraEditors.LabelControl LblTitle;
        private DevExpress.XtraEditors.SimpleButton BtnRefresh;
        private DevExpress.XtraEditors.SimpleButton BtnExportCsv;

        // ---- Filter panel ----
        private DevExpress.XtraEditors.PanelControl PanelFilter;
        private DevExpress.XtraEditors.LabelControl LblDateFrom;
        private DevExpress.XtraEditors.DateEdit DtDateFrom;
        private DevExpress.XtraEditors.LabelControl LblDateTo;
        private DevExpress.XtraEditors.DateEdit DtDateTo;
        private DevExpress.XtraEditors.LabelControl LblTechnician;
        private DevExpress.XtraEditors.ComboBoxEdit CmbTechnician;
        private DevExpress.XtraEditors.LabelControl LblPartsSearch;
        private DevExpress.XtraEditors.TextEdit TxtPartsSearch;
        private DevExpress.XtraEditors.LabelControl LblTicketSearch;
        private DevExpress.XtraEditors.TextEdit TxtTicketSearch;
        private DevExpress.XtraEditors.LabelControl LblIsCollected;
        private DevExpress.XtraEditors.CheckEdit ChkCollWaiting;
        private DevExpress.XtraEditors.CheckEdit ChkCollYes;
        private DevExpress.XtraEditors.CheckEdit ChkCollNo;
        private DevExpress.XtraEditors.CheckEdit ChkCollAll;
        private DevExpress.XtraEditors.LabelControl LblApproval;
        private DevExpress.XtraEditors.CheckEdit ChkApprWaiting;
        private DevExpress.XtraEditors.CheckEdit ChkApprYes;
        private DevExpress.XtraEditors.CheckEdit ChkApprNo;
        private DevExpress.XtraEditors.CheckEdit ChkApprBackorder;
        private DevExpress.XtraEditors.SimpleButton BtnApplyFilter;
        private DevExpress.XtraEditors.SimpleButton BtnResetAll;

        // ---- Grid ----
        private DevExpress.XtraGrid.GridControl Grid;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit RepoSelCheck;

        private DevExpress.XtraGrid.Columns.GridColumn ColSel;
        private DevExpress.XtraGrid.Columns.GridColumn ColID;
        private DevExpress.XtraGrid.Columns.GridColumn ColPID;
        private DevExpress.XtraGrid.Columns.GridColumn ColDate;
        private DevExpress.XtraGrid.Columns.GridColumn ColTech;
        private DevExpress.XtraGrid.Columns.GridColumn ColPartsSupplies;
        private DevExpress.XtraGrid.Columns.GridColumn ColReqQty;
        private DevExpress.XtraGrid.Columns.GridColumn ColQty;
        private DevExpress.XtraGrid.Columns.GridColumn ColBalance;
        private DevExpress.XtraGrid.Columns.GridColumn ColType;
        private DevExpress.XtraGrid.Columns.GridColumn ColUnit;
        private DevExpress.XtraGrid.Columns.GridColumn ColColor;
        private DevExpress.XtraGrid.Columns.GridColumn ColIsCollected;
        private DevExpress.XtraGrid.Columns.GridColumn ColCollectedAt;
        private DevExpress.XtraGrid.Columns.GridColumn ColApproval;
        private DevExpress.XtraGrid.Columns.GridColumn ColRemark;
        private DevExpress.XtraGrid.Columns.GridColumn ColTicketID;

        // ---- Bottom action bar ----
        private DevExpress.XtraEditors.PanelControl PanelAction;
        private DevExpress.XtraEditors.LabelControl LblSelectionCount;
        private DevExpress.XtraEditors.SimpleButton BtnPerformMovement;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelTitle = new DevExpress.XtraEditors.PanelControl();
            this.LblTitle = new DevExpress.XtraEditors.LabelControl();
            this.BtnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.BtnExportCsv = new DevExpress.XtraEditors.SimpleButton();

            this.PanelFilter = new DevExpress.XtraEditors.PanelControl();
            this.LblDateFrom = new DevExpress.XtraEditors.LabelControl();
            this.DtDateFrom = new DevExpress.XtraEditors.DateEdit();
            this.LblDateTo = new DevExpress.XtraEditors.LabelControl();
            this.DtDateTo = new DevExpress.XtraEditors.DateEdit();
            this.LblTechnician = new DevExpress.XtraEditors.LabelControl();
            this.CmbTechnician = new DevExpress.XtraEditors.ComboBoxEdit();
            this.LblPartsSearch = new DevExpress.XtraEditors.LabelControl();
            this.TxtPartsSearch = new DevExpress.XtraEditors.TextEdit();
            this.LblTicketSearch = new DevExpress.XtraEditors.LabelControl();
            this.TxtTicketSearch = new DevExpress.XtraEditors.TextEdit();
            this.LblIsCollected = new DevExpress.XtraEditors.LabelControl();
            this.ChkCollWaiting = new DevExpress.XtraEditors.CheckEdit();
            this.ChkCollYes = new DevExpress.XtraEditors.CheckEdit();
            this.ChkCollNo = new DevExpress.XtraEditors.CheckEdit();
            this.ChkCollAll = new DevExpress.XtraEditors.CheckEdit();
            this.LblApproval = new DevExpress.XtraEditors.LabelControl();
            this.ChkApprWaiting = new DevExpress.XtraEditors.CheckEdit();
            this.ChkApprYes = new DevExpress.XtraEditors.CheckEdit();
            this.ChkApprNo = new DevExpress.XtraEditors.CheckEdit();
            this.ChkApprBackorder = new DevExpress.XtraEditors.CheckEdit();
            this.BtnApplyFilter = new DevExpress.XtraEditors.SimpleButton();
            this.BtnResetAll = new DevExpress.XtraEditors.SimpleButton();

            this.Grid = new DevExpress.XtraGrid.GridControl();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepoSelCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.ColSel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColPID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColTech = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColPartsSupplies = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColReqQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColIsCollected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColCollectedAt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColApproval = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColRemark = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColTicketID = new DevExpress.XtraGrid.Columns.GridColumn();

            this.PanelAction = new DevExpress.XtraEditors.PanelControl();
            this.LblSelectionCount = new DevExpress.XtraEditors.LabelControl();
            this.BtnPerformMovement = new DevExpress.XtraEditors.SimpleButton();

            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).BeginInit();
            this.PanelTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelFilter)).BeginInit();
            this.PanelFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbTechnician.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPartsSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTicketSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollWaiting.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollYes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprWaiting.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprYes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprBackorder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoSelCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelAction)).BeginInit();
            this.PanelAction.SuspendLayout();
            this.SuspendLayout();
            //
            // PanelTitle
            //
            this.PanelTitle.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(179)))), ((int)(((byte)(66)))));
            this.PanelTitle.Appearance.Options.UseBackColor = true;
            this.PanelTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelTitle.Controls.Add(this.BtnExportCsv);
            this.PanelTitle.Controls.Add(this.BtnRefresh);
            this.PanelTitle.Controls.Add(this.LblTitle);
            this.PanelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTitle.Location = new System.Drawing.Point(0, 0);
            this.PanelTitle.Name = "PanelTitle";
            this.PanelTitle.Size = new System.Drawing.Size(1380, 50);
            this.PanelTitle.TabIndex = 0;
            //
            // LblTitle
            //
            this.LblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = System.Drawing.Color.White;
            this.LblTitle.Appearance.Options.UseFont = true;
            this.LblTitle.Appearance.Options.UseForeColor = true;
            this.LblTitle.Location = new System.Drawing.Point(16, 10);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(330, 29);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Stock Request Integration";
            //
            // BtnRefresh
            //
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRefresh.Location = new System.Drawing.Point(1180, 11);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(90, 28);
            this.BtnRefresh.TabIndex = 1;
            this.BtnRefresh.Text = "Refresh";
            //
            // BtnExportCsv
            //
            this.BtnExportCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExportCsv.Location = new System.Drawing.Point(1275, 11);
            this.BtnExportCsv.Name = "BtnExportCsv";
            this.BtnExportCsv.Size = new System.Drawing.Size(90, 28);
            this.BtnExportCsv.TabIndex = 2;
            this.BtnExportCsv.Text = "Export CSV";
            //
            // PanelFilter
            //
            this.PanelFilter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.PanelFilter.Appearance.Options.UseBackColor = true;
            this.PanelFilter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelFilter.Controls.Add(this.BtnResetAll);
            this.PanelFilter.Controls.Add(this.BtnApplyFilter);
            this.PanelFilter.Controls.Add(this.ChkApprBackorder);
            this.PanelFilter.Controls.Add(this.ChkApprNo);
            this.PanelFilter.Controls.Add(this.ChkApprYes);
            this.PanelFilter.Controls.Add(this.ChkApprWaiting);
            this.PanelFilter.Controls.Add(this.LblApproval);
            this.PanelFilter.Controls.Add(this.ChkCollAll);
            this.PanelFilter.Controls.Add(this.ChkCollNo);
            this.PanelFilter.Controls.Add(this.ChkCollYes);
            this.PanelFilter.Controls.Add(this.ChkCollWaiting);
            this.PanelFilter.Controls.Add(this.LblIsCollected);
            this.PanelFilter.Controls.Add(this.TxtTicketSearch);
            this.PanelFilter.Controls.Add(this.LblTicketSearch);
            this.PanelFilter.Controls.Add(this.TxtPartsSearch);
            this.PanelFilter.Controls.Add(this.LblPartsSearch);
            this.PanelFilter.Controls.Add(this.CmbTechnician);
            this.PanelFilter.Controls.Add(this.LblTechnician);
            this.PanelFilter.Controls.Add(this.DtDateTo);
            this.PanelFilter.Controls.Add(this.LblDateTo);
            this.PanelFilter.Controls.Add(this.DtDateFrom);
            this.PanelFilter.Controls.Add(this.LblDateFrom);
            this.PanelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelFilter.Location = new System.Drawing.Point(0, 50);
            this.PanelFilter.Name = "PanelFilter";
            this.PanelFilter.Size = new System.Drawing.Size(1380, 110);
            this.PanelFilter.TabIndex = 1;
            //
            // LblDateFrom
            //
            this.LblDateFrom.Location = new System.Drawing.Point(14, 14);
            this.LblDateFrom.Name = "LblDateFrom";
            this.LblDateFrom.Size = new System.Drawing.Size(54, 13);
            this.LblDateFrom.TabIndex = 0;
            this.LblDateFrom.Text = "Date From:";
            //
            // DtDateFrom
            //
            this.DtDateFrom.EditValue = null;
            this.DtDateFrom.Location = new System.Drawing.Point(80, 11);
            this.DtDateFrom.Name = "DtDateFrom";
            this.DtDateFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtDateFrom.Size = new System.Drawing.Size(120, 20);
            this.DtDateFrom.TabIndex = 1;
            //
            // LblDateTo
            //
            this.LblDateTo.Location = new System.Drawing.Point(212, 14);
            this.LblDateTo.Name = "LblDateTo";
            this.LblDateTo.Size = new System.Drawing.Size(42, 13);
            this.LblDateTo.TabIndex = 2;
            this.LblDateTo.Text = "Date To:";
            //
            // DtDateTo
            //
            this.DtDateTo.EditValue = null;
            this.DtDateTo.Location = new System.Drawing.Point(264, 11);
            this.DtDateTo.Name = "DtDateTo";
            this.DtDateTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtDateTo.Size = new System.Drawing.Size(120, 20);
            this.DtDateTo.TabIndex = 3;
            //
            // LblTechnician
            //
            this.LblTechnician.Location = new System.Drawing.Point(396, 14);
            this.LblTechnician.Name = "LblTechnician";
            this.LblTechnician.Size = new System.Drawing.Size(58, 13);
            this.LblTechnician.TabIndex = 4;
            this.LblTechnician.Text = "Technician:";
            //
            // CmbTechnician
            //
            this.CmbTechnician.Location = new System.Drawing.Point(464, 11);
            this.CmbTechnician.Name = "CmbTechnician";
            this.CmbTechnician.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbTechnician.Size = new System.Drawing.Size(220, 20);
            this.CmbTechnician.TabIndex = 5;
            //
            // LblPartsSearch
            //
            this.LblPartsSearch.Location = new System.Drawing.Point(696, 14);
            this.LblPartsSearch.Name = "LblPartsSearch";
            this.LblPartsSearch.Size = new System.Drawing.Size(78, 13);
            this.LblPartsSearch.TabIndex = 6;
            this.LblPartsSearch.Text = "Parts/Supplies:";
            //
            // TxtPartsSearch
            //
            this.TxtPartsSearch.Location = new System.Drawing.Point(782, 11);
            this.TxtPartsSearch.Name = "TxtPartsSearch";
            this.TxtPartsSearch.Size = new System.Drawing.Size(180, 20);
            this.TxtPartsSearch.TabIndex = 7;
            //
            // LblTicketSearch
            //
            this.LblTicketSearch.Location = new System.Drawing.Point(976, 14);
            this.LblTicketSearch.Name = "LblTicketSearch";
            this.LblTicketSearch.Size = new System.Drawing.Size(50, 13);
            this.LblTicketSearch.TabIndex = 8;
            this.LblTicketSearch.Text = "Ticket ID:";
            //
            // TxtTicketSearch
            //
            this.TxtTicketSearch.Location = new System.Drawing.Point(1034, 11);
            this.TxtTicketSearch.Name = "TxtTicketSearch";
            this.TxtTicketSearch.Size = new System.Drawing.Size(160, 20);
            this.TxtTicketSearch.TabIndex = 9;
            //
            // LblIsCollected
            //
            this.LblIsCollected.Location = new System.Drawing.Point(14, 47);
            this.LblIsCollected.Name = "LblIsCollected";
            this.LblIsCollected.Size = new System.Drawing.Size(64, 13);
            this.LblIsCollected.TabIndex = 10;
            this.LblIsCollected.Text = "Is Collected:";
            //
            // ChkCollWaiting
            //
            this.ChkCollWaiting.Location = new System.Drawing.Point(80, 44);
            this.ChkCollWaiting.Name = "ChkCollWaiting";
            this.ChkCollWaiting.Properties.Caption = "Waiting for Collect";
            this.ChkCollWaiting.Size = new System.Drawing.Size(130, 19);
            this.ChkCollWaiting.TabIndex = 11;
            //
            // ChkCollYes
            //
            this.ChkCollYes.Location = new System.Drawing.Point(214, 44);
            this.ChkCollYes.Name = "ChkCollYes";
            this.ChkCollYes.Properties.Caption = "Yes";
            this.ChkCollYes.Size = new System.Drawing.Size(60, 19);
            this.ChkCollYes.TabIndex = 12;
            //
            // ChkCollNo
            //
            this.ChkCollNo.Location = new System.Drawing.Point(278, 44);
            this.ChkCollNo.Name = "ChkCollNo";
            this.ChkCollNo.Properties.Caption = "No";
            this.ChkCollNo.Size = new System.Drawing.Size(60, 19);
            this.ChkCollNo.TabIndex = 13;
            //
            // ChkCollAll
            //
            this.ChkCollAll.EditValue = true;
            this.ChkCollAll.Location = new System.Drawing.Point(342, 44);
            this.ChkCollAll.Name = "ChkCollAll";
            this.ChkCollAll.Properties.Caption = "All";
            this.ChkCollAll.Size = new System.Drawing.Size(60, 19);
            this.ChkCollAll.TabIndex = 14;
            //
            // LblApproval
            //
            this.LblApproval.Location = new System.Drawing.Point(14, 76);
            this.LblApproval.Name = "LblApproval";
            this.LblApproval.Size = new System.Drawing.Size(48, 13);
            this.LblApproval.TabIndex = 15;
            this.LblApproval.Text = "Approval:";
            //
            // ChkApprWaiting
            //
            this.ChkApprWaiting.Location = new System.Drawing.Point(80, 73);
            this.ChkApprWaiting.Name = "ChkApprWaiting";
            this.ChkApprWaiting.Properties.Caption = "Waiting Approval";
            this.ChkApprWaiting.Size = new System.Drawing.Size(130, 19);
            this.ChkApprWaiting.TabIndex = 16;
            //
            // ChkApprYes
            //
            this.ChkApprYes.Location = new System.Drawing.Point(214, 73);
            this.ChkApprYes.Name = "ChkApprYes";
            this.ChkApprYes.Properties.Caption = "Yes";
            this.ChkApprYes.Size = new System.Drawing.Size(60, 19);
            this.ChkApprYes.TabIndex = 17;
            //
            // ChkApprNo
            //
            this.ChkApprNo.Location = new System.Drawing.Point(278, 73);
            this.ChkApprNo.Name = "ChkApprNo";
            this.ChkApprNo.Properties.Caption = "No";
            this.ChkApprNo.Size = new System.Drawing.Size(60, 19);
            this.ChkApprNo.TabIndex = 18;
            //
            // ChkApprBackorder
            //
            this.ChkApprBackorder.Location = new System.Drawing.Point(342, 73);
            this.ChkApprBackorder.Name = "ChkApprBackorder";
            this.ChkApprBackorder.Properties.Caption = "Backorder";
            this.ChkApprBackorder.Size = new System.Drawing.Size(90, 19);
            this.ChkApprBackorder.TabIndex = 19;
            //
            // BtnApplyFilter
            //
            this.BtnApplyFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnApplyFilter.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(179)))), ((int)(((byte)(66)))));
            this.BtnApplyFilter.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnApplyFilter.Appearance.Options.UseBackColor = true;
            this.BtnApplyFilter.Appearance.Options.UseForeColor = true;
            this.BtnApplyFilter.Location = new System.Drawing.Point(1180, 70);
            this.BtnApplyFilter.Name = "BtnApplyFilter";
            this.BtnApplyFilter.Size = new System.Drawing.Size(90, 28);
            this.BtnApplyFilter.TabIndex = 20;
            this.BtnApplyFilter.Text = "Apply Filter";
            //
            // BtnResetAll
            //
            this.BtnResetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnResetAll.Location = new System.Drawing.Point(1275, 70);
            this.BtnResetAll.Name = "BtnResetAll";
            this.BtnResetAll.Size = new System.Drawing.Size(90, 28);
            this.BtnResetAll.TabIndex = 21;
            this.BtnResetAll.Text = "Reset All";
            //
            // Grid
            //
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 160);
            this.Grid.MainView = this.GridView;
            this.Grid.Name = "Grid";
            this.Grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
                this.RepoSelCheck});
            this.Grid.Size = new System.Drawing.Size(1380, 530);
            this.Grid.TabIndex = 2;
            this.Grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
                this.GridView});
            //
            // GridView
            //
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
                this.ColSel,
                this.ColID,
                this.ColPID,
                this.ColDate,
                this.ColTech,
                this.ColPartsSupplies,
                this.ColReqQty,
                this.ColQty,
                this.ColBalance,
                this.ColType,
                this.ColUnit,
                this.ColColor,
                this.ColIsCollected,
                this.ColCollectedAt,
                this.ColApproval,
                this.ColRemark,
                this.ColTicketID});
            this.GridView.GridControl = this.Grid;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.Editable = true;
            this.GridView.OptionsSelection.EnableAppearanceFocusedRow = true;
            this.GridView.OptionsView.ColumnAutoWidth = false;
            this.GridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.GridView.OptionsView.ShowAutoFilterRow = true;
            this.GridView.OptionsView.ShowGroupPanel = false;
            //
            // RepoSelCheck
            //
            this.RepoSelCheck.AutoHeight = false;
            this.RepoSelCheck.Name = "RepoSelCheck";
            //
            // ColSel
            //
            this.ColSel.Caption = " ";
            this.ColSel.ColumnEdit = this.RepoSelCheck;
            this.ColSel.FieldName = "Sel";
            this.ColSel.Name = "ColSel";
            this.ColSel.Visible = true;
            this.ColSel.VisibleIndex = 0;
            this.ColSel.Width = 36;
            //
            // ColID
            //
            this.ColID.Caption = "ID";
            this.ColID.FieldName = "ID";
            this.ColID.Name = "ColID";
            this.ColID.OptionsColumn.ReadOnly = true;
            this.ColID.Visible = true;
            this.ColID.VisibleIndex = 1;
            this.ColID.Width = 60;
            //
            // ColPID
            //
            this.ColPID.Caption = "P. ID";
            this.ColPID.FieldName = "PID";
            this.ColPID.Name = "ColPID";
            this.ColPID.OptionsColumn.ReadOnly = true;
            this.ColPID.Visible = true;
            this.ColPID.VisibleIndex = 2;
            this.ColPID.Width = 60;
            //
            // ColDate
            //
            this.ColDate.Caption = "Date";
            this.ColDate.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.ColDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ColDate.FieldName = "Date";
            this.ColDate.Name = "ColDate";
            this.ColDate.OptionsColumn.ReadOnly = true;
            this.ColDate.Visible = true;
            this.ColDate.VisibleIndex = 3;
            this.ColDate.Width = 130;
            //
            // ColTech
            //
            this.ColTech.Caption = "Tech";
            this.ColTech.FieldName = "Tech";
            this.ColTech.Name = "ColTech";
            this.ColTech.OptionsColumn.ReadOnly = true;
            this.ColTech.Visible = true;
            this.ColTech.VisibleIndex = 4;
            this.ColTech.Width = 100;
            //
            // ColPartsSupplies
            //
            this.ColPartsSupplies.Caption = "Parts / Supplies";
            this.ColPartsSupplies.FieldName = "PartsSupplies";
            this.ColPartsSupplies.Name = "ColPartsSupplies";
            this.ColPartsSupplies.OptionsColumn.ReadOnly = true;
            this.ColPartsSupplies.Visible = true;
            this.ColPartsSupplies.VisibleIndex = 5;
            this.ColPartsSupplies.Width = 320;
            //
            // ColReqQty
            //
            this.ColReqQty.Caption = "Req. Qty";
            this.ColReqQty.DisplayFormat.FormatString = "0.0000";
            this.ColReqQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColReqQty.FieldName = "ReqQty";
            this.ColReqQty.Name = "ColReqQty";
            this.ColReqQty.OptionsColumn.ReadOnly = true;
            this.ColReqQty.Visible = true;
            this.ColReqQty.VisibleIndex = 6;
            this.ColReqQty.Width = 70;
            //
            // ColQty
            //
            this.ColQty.Caption = "Qty";
            this.ColQty.DisplayFormat.FormatString = "0.0000";
            this.ColQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColQty.FieldName = "Qty";
            this.ColQty.Name = "ColQty";
            this.ColQty.OptionsColumn.ReadOnly = true;
            this.ColQty.Visible = true;
            this.ColQty.VisibleIndex = 7;
            this.ColQty.Width = 70;
            //
            // ColBalance
            //
            this.ColBalance.Caption = "Balance";
            this.ColBalance.DisplayFormat.FormatString = "0.0000";
            this.ColBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColBalance.FieldName = "Balance";
            this.ColBalance.Name = "ColBalance";
            this.ColBalance.OptionsColumn.ReadOnly = true;
            this.ColBalance.Visible = true;
            this.ColBalance.VisibleIndex = 8;
            this.ColBalance.Width = 70;
            //
            // ColType
            //
            this.ColType.Caption = "Type";
            this.ColType.FieldName = "Type";
            this.ColType.Name = "ColType";
            this.ColType.OptionsColumn.ReadOnly = true;
            this.ColType.Visible = true;
            this.ColType.VisibleIndex = 9;
            this.ColType.Width = 50;
            //
            // ColUnit
            //
            this.ColUnit.Caption = "Unit";
            this.ColUnit.FieldName = "Unit";
            this.ColUnit.Name = "ColUnit";
            this.ColUnit.OptionsColumn.ReadOnly = true;
            this.ColUnit.Visible = true;
            this.ColUnit.VisibleIndex = 10;
            this.ColUnit.Width = 60;
            //
            // ColColor
            //
            this.ColColor.Caption = "Color";
            this.ColColor.FieldName = "Color";
            this.ColColor.Name = "ColColor";
            this.ColColor.OptionsColumn.ReadOnly = true;
            this.ColColor.Visible = true;
            this.ColColor.VisibleIndex = 11;
            this.ColColor.Width = 50;
            //
            // ColIsCollected
            //
            this.ColIsCollected.Caption = "Is Collected";
            this.ColIsCollected.FieldName = "IsCollected";
            this.ColIsCollected.Name = "ColIsCollected";
            this.ColIsCollected.OptionsColumn.ReadOnly = true;
            this.ColIsCollected.Visible = true;
            this.ColIsCollected.VisibleIndex = 12;
            this.ColIsCollected.Width = 130;
            //
            // ColCollectedAt
            //
            this.ColCollectedAt.Caption = "Collected At";
            this.ColCollectedAt.FieldName = "CollectedAt";
            this.ColCollectedAt.Name = "ColCollectedAt";
            this.ColCollectedAt.OptionsColumn.ReadOnly = true;
            this.ColCollectedAt.Visible = true;
            this.ColCollectedAt.VisibleIndex = 13;
            this.ColCollectedAt.Width = 110;
            //
            // ColApproval
            //
            this.ColApproval.Caption = "Approval";
            this.ColApproval.FieldName = "Approval";
            this.ColApproval.Name = "ColApproval";
            this.ColApproval.OptionsColumn.ReadOnly = true;
            this.ColApproval.Visible = true;
            this.ColApproval.VisibleIndex = 14;
            this.ColApproval.Width = 120;
            //
            // ColRemark
            //
            this.ColRemark.Caption = "Remark";
            this.ColRemark.FieldName = "Remark";
            this.ColRemark.Name = "ColRemark";
            this.ColRemark.OptionsColumn.ReadOnly = true;
            this.ColRemark.Visible = true;
            this.ColRemark.VisibleIndex = 15;
            this.ColRemark.Width = 220;
            //
            // ColTicketID
            //
            this.ColTicketID.Caption = "Ticket ID";
            this.ColTicketID.FieldName = "TicketID";
            this.ColTicketID.Name = "ColTicketID";
            this.ColTicketID.OptionsColumn.ReadOnly = true;
            this.ColTicketID.Visible = true;
            this.ColTicketID.VisibleIndex = 16;
            this.ColTicketID.Width = 130;
            //
            // PanelAction
            //
            this.PanelAction.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.PanelAction.Appearance.Options.UseBackColor = true;
            this.PanelAction.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelAction.Controls.Add(this.BtnPerformMovement);
            this.PanelAction.Controls.Add(this.LblSelectionCount);
            this.PanelAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelAction.Location = new System.Drawing.Point(0, 690);
            this.PanelAction.Name = "PanelAction";
            this.PanelAction.Size = new System.Drawing.Size(1380, 50);
            this.PanelAction.TabIndex = 3;
            //
            // LblSelectionCount
            //
            this.LblSelectionCount.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.LblSelectionCount.Appearance.Options.UseFont = true;
            this.LblSelectionCount.Location = new System.Drawing.Point(16, 16);
            this.LblSelectionCount.Name = "LblSelectionCount";
            this.LblSelectionCount.Size = new System.Drawing.Size(120, 19);
            this.LblSelectionCount.TabIndex = 0;
            this.LblSelectionCount.Text = "Selected: 0 of 0";
            //
            // BtnPerformMovement
            //
            this.BtnPerformMovement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPerformMovement.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.BtnPerformMovement.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BtnPerformMovement.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnPerformMovement.Appearance.Options.UseBackColor = true;
            this.BtnPerformMovement.Appearance.Options.UseFont = true;
            this.BtnPerformMovement.Appearance.Options.UseForeColor = true;
            this.BtnPerformMovement.Location = new System.Drawing.Point(1130, 8);
            this.BtnPerformMovement.Name = "BtnPerformMovement";
            this.BtnPerformMovement.Size = new System.Drawing.Size(235, 35);
            this.BtnPerformMovement.TabIndex = 1;
            this.BtnPerformMovement.Text = "Perform Stock Movement";
            //
            // StockRequestIntegration_Form
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 740);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.PanelAction);
            this.Controls.Add(this.PanelFilter);
            this.Controls.Add(this.PanelTitle);
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "StockRequestIntegration_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stock Request Integration";
            ((System.ComponentModel.ISupportInitialize)(this.PanelTitle)).EndInit();
            this.PanelTitle.ResumeLayout(false);
            this.PanelTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelFilter)).EndInit();
            this.PanelFilter.ResumeLayout(false);
            this.PanelFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtDateTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbTechnician.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPartsSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTicketSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollWaiting.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollYes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCollAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprWaiting.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprYes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkApprBackorder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoSelCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelAction)).EndInit();
            this.PanelAction.ResumeLayout(false);
            this.PanelAction.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
