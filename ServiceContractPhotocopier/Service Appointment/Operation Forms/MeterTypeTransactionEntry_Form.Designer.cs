namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    partial class MeterTypeTransactionEntry_Form
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

        #region Windows Form Designer generated code

        private DevExpress.XtraEditors.LabelControl LblTitle;
        private DevExpress.XtraEditors.SimpleButton BtnSaveInvoice;
        private DevExpress.XtraEditors.SimpleButton BtnExit;
        private DevExpress.XtraEditors.PanelControl PanelHeader;
        private DevExpress.XtraEditors.LabelControl LblServiceTag;
        private DevExpress.XtraEditors.SearchLookUpEdit LkServiceTag;
        private DevExpress.XtraEditors.LabelControl LblReadingDate;
        private DevExpress.XtraEditors.DateEdit DtReadingDate;
        private DevExpress.XtraEditors.LabelControl LblStockCode;
        private DevExpress.XtraEditors.SearchLookUpEdit LkStockCode;
        private DevExpress.XtraEditors.LabelControl LblStockCodeDesc;
        private DevExpress.XtraEditors.LabelControl LblDebtorCode;
        private DevExpress.XtraEditors.SearchLookUpEdit LkDebtorCode;
        private DevExpress.XtraEditors.LabelControl LblDebtorCodeDesc;
        private DevExpress.XtraEditors.LabelControl LblDepartment;
        private DevExpress.XtraEditors.SearchLookUpEdit LkDepartment;
        private DevExpress.XtraEditors.LabelControl LblDeptDesc;
        private DevExpress.XtraEditors.LabelControl LblJob;
        private DevExpress.XtraEditors.SearchLookUpEdit LkJob;
        private DevExpress.XtraEditors.LabelControl LblJobDesc;
        private DevExpress.XtraEditors.LabelControl LblLocation;
        private DevExpress.XtraEditors.SearchLookUpEdit LkLocation;
        private DevExpress.XtraEditors.LabelControl LblLocationDesc;
        private DevExpress.XtraEditors.GroupControl GrpSalesInvoice;
        private DevExpress.XtraEditors.LabelControl LblInvoiceDate;
        private DevExpress.XtraEditors.DateEdit DtInvoiceDate;
        private DevExpress.XtraEditors.LabelControl LblInvoiceNoFmt;
        private DevExpress.XtraEditors.ComboBoxEdit CmbInvoiceNoFmt;
        private DevExpress.XtraEditors.LabelControl LblDescription;
        private DevExpress.XtraEditors.ComboBoxEdit CmbDescription;
        private DevExpress.XtraEditors.CheckEdit ChkTaxInclusive;
        private DevExpress.XtraEditors.CheckEdit ChkUseRoundingAdj;
        private DevExpress.XtraEditors.SimpleButton BtnTickSelection;
        private DevExpress.XtraEditors.SimpleButton BtnSelectAll;
        private DevExpress.XtraEditors.SimpleButton BtnDeselectAll;
        private DevExpress.XtraEditors.SimpleButton BtnFetchReading;
        private DevExpress.XtraEditors.CheckEdit ChkSimulateFailure;
        private DevExpress.XtraGrid.GridControl GridMeter;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewMeter;
        private DevExpress.XtraGrid.Columns.GridColumn ColMeterType;
        private DevExpress.XtraGrid.Columns.GridColumn ColMeterTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn ColMinCharges;
        private DevExpress.XtraGrid.Columns.GridColumn ColUnitPrice;
        private DevExpress.XtraGrid.Columns.GridColumn ColFOCQty;
        private DevExpress.XtraGrid.Columns.GridColumn ColRebateQty;
        private DevExpress.XtraGrid.Columns.GridColumn ColLastReadDate;
        private DevExpress.XtraGrid.Columns.GridColumn ColLastReading;
        private DevExpress.XtraGrid.Columns.GridColumn ColMeterUsage;
        private DevExpress.XtraGrid.Columns.GridColumn ColTotalCharges;
        private DevExpress.XtraGrid.Columns.GridColumn ColSelected;
        private DevExpress.XtraGrid.Columns.GridColumn ColCurrentReading;
        private DevExpress.XtraGrid.Columns.GridColumn ColUseMinCharges;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit RepoChkSelected;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit RepoChkUseMinCharges;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit RepoSpinReading;
        private DevExpress.XtraEditors.PanelControl PanelStatus;
        private DevExpress.XtraEditors.LabelControl LblRowCount;
        private DevExpress.XtraEditors.LabelControl LblTotal;

        private void InitializeComponent()
        {
            this.LblTitle = new DevExpress.XtraEditors.LabelControl();
            this.BtnSaveInvoice = new DevExpress.XtraEditors.SimpleButton();
            this.BtnExit = new DevExpress.XtraEditors.SimpleButton();
            this.PanelHeader = new DevExpress.XtraEditors.PanelControl();
            this.LblServiceTag = new DevExpress.XtraEditors.LabelControl();
            this.LkServiceTag = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.LblReadingDate = new DevExpress.XtraEditors.LabelControl();
            this.DtReadingDate = new DevExpress.XtraEditors.DateEdit();
            this.LblStockCode = new DevExpress.XtraEditors.LabelControl();
            this.LkStockCode = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.LblStockCodeDesc = new DevExpress.XtraEditors.LabelControl();
            this.LblDebtorCode = new DevExpress.XtraEditors.LabelControl();
            this.LkDebtorCode = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.LblDebtorCodeDesc = new DevExpress.XtraEditors.LabelControl();
            this.LblDepartment = new DevExpress.XtraEditors.LabelControl();
            this.LkDepartment = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.LblDeptDesc = new DevExpress.XtraEditors.LabelControl();
            this.LblJob = new DevExpress.XtraEditors.LabelControl();
            this.LkJob = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.LblJobDesc = new DevExpress.XtraEditors.LabelControl();
            this.LblLocation = new DevExpress.XtraEditors.LabelControl();
            this.LkLocation = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.LblLocationDesc = new DevExpress.XtraEditors.LabelControl();
            this.GrpSalesInvoice = new DevExpress.XtraEditors.GroupControl();
            this.LblInvoiceDate = new DevExpress.XtraEditors.LabelControl();
            this.DtInvoiceDate = new DevExpress.XtraEditors.DateEdit();
            this.LblInvoiceNoFmt = new DevExpress.XtraEditors.LabelControl();
            this.CmbInvoiceNoFmt = new DevExpress.XtraEditors.ComboBoxEdit();
            this.LblDescription = new DevExpress.XtraEditors.LabelControl();
            this.CmbDescription = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ChkTaxInclusive = new DevExpress.XtraEditors.CheckEdit();
            this.ChkUseRoundingAdj = new DevExpress.XtraEditors.CheckEdit();
            this.BtnTickSelection = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSelectAll = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDeselectAll = new DevExpress.XtraEditors.SimpleButton();
            this.BtnFetchReading = new DevExpress.XtraEditors.SimpleButton();
            this.ChkSimulateFailure = new DevExpress.XtraEditors.CheckEdit();
            this.GridMeter = new DevExpress.XtraGrid.GridControl();
            this.GridViewMeter = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColMeterType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColMeterTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColMinCharges = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColUnitPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColFOCQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColRebateQty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColLastReadDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColLastReading = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColMeterUsage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColTotalCharges = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColSelected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColCurrentReading = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColUseMinCharges = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepoChkSelected = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.RepoChkUseMinCharges = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.RepoSpinReading = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.PanelStatus = new DevExpress.XtraEditors.PanelControl();
            this.LblRowCount = new DevExpress.XtraEditors.LabelControl();
            this.LblTotal = new DevExpress.XtraEditors.LabelControl();
            //
            // BeginInit
            //
            ((System.ComponentModel.ISupportInitialize)(this.PanelHeader)).BeginInit();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LkServiceTag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtReadingDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtReadingDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkStockCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkDebtorCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpSalesInvoice)).BeginInit();
            this.GrpSalesInvoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DtInvoiceDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtInvoiceDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbInvoiceNoFmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkTaxInclusive.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkUseRoundingAdj.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridMeter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMeter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoChkSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoChkUseMinCharges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoSpinReading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelStatus)).BeginInit();
            this.PanelStatus.SuspendLayout();
            this.SuspendLayout();
            //
            // LblTitle
            //
            this.LblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.LblTitle.Appearance.Options.UseFont = true;
            this.LblTitle.Appearance.Options.UseForeColor = true;
            this.LblTitle.Location = new System.Drawing.Point(14, 8);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(310, 25);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Meter Type Transaction Entry";
            //
            // BtnSaveInvoice
            //
            this.BtnSaveInvoice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.BtnSaveInvoice.Location = new System.Drawing.Point(1100, 8);
            this.BtnSaveInvoice.Name = "BtnSaveInvoice";
            this.BtnSaveInvoice.Size = new System.Drawing.Size(160, 28);
            this.BtnSaveInvoice.TabIndex = 1;
            this.BtnSaveInvoice.Text = "Save && Generate Invoice";
            this.BtnSaveInvoice.Click += new System.EventHandler(this.OnSaveAndGenerateInvoice);
            //
            // BtnExit
            //
            this.BtnExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.BtnExit.Location = new System.Drawing.Point(1270, 8);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(80, 28);
            this.BtnExit.TabIndex = 2;
            this.BtnExit.Text = "Exit (F2)";
            this.BtnExit.Click += new System.EventHandler(this.OnExit);
            //
            // PanelHeader
            //
            this.PanelHeader.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.PanelHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelHeader.Location = new System.Drawing.Point(0, 40);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(1364, 140);
            this.PanelHeader.TabIndex = 3;
            this.PanelHeader.Controls.Add(this.LblServiceTag);
            this.PanelHeader.Controls.Add(this.LkServiceTag);
            this.PanelHeader.Controls.Add(this.LblReadingDate);
            this.PanelHeader.Controls.Add(this.DtReadingDate);
            this.PanelHeader.Controls.Add(this.LblStockCode);
            this.PanelHeader.Controls.Add(this.LkStockCode);
            this.PanelHeader.Controls.Add(this.LblStockCodeDesc);
            this.PanelHeader.Controls.Add(this.LblDebtorCode);
            this.PanelHeader.Controls.Add(this.LkDebtorCode);
            this.PanelHeader.Controls.Add(this.LblDebtorCodeDesc);
            this.PanelHeader.Controls.Add(this.LblDepartment);
            this.PanelHeader.Controls.Add(this.LkDepartment);
            this.PanelHeader.Controls.Add(this.LblDeptDesc);
            this.PanelHeader.Controls.Add(this.LblJob);
            this.PanelHeader.Controls.Add(this.LkJob);
            this.PanelHeader.Controls.Add(this.LblJobDesc);
            this.PanelHeader.Controls.Add(this.LblLocation);
            this.PanelHeader.Controls.Add(this.LkLocation);
            this.PanelHeader.Controls.Add(this.LblLocationDesc);
            //
            // LblServiceTag
            //
            this.LblServiceTag.Location = new System.Drawing.Point(14, 10);
            this.LblServiceTag.Name = "LblServiceTag";
            this.LblServiceTag.Size = new System.Drawing.Size(65, 13);
            this.LblServiceTag.TabIndex = 0;
            this.LblServiceTag.Text = "Service Tag :";
            //
            // LkServiceTag
            //
            this.LkServiceTag.Location = new System.Drawing.Point(130, 7);
            this.LkServiceTag.Name = "LkServiceTag";
            this.LkServiceTag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.LkServiceTag.Properties.PopupFormWidth = 520;
            this.LkServiceTag.Size = new System.Drawing.Size(250, 20);
            this.LkServiceTag.TabIndex = 1;
            this.LkServiceTag.EditValueChanged += new System.EventHandler(this.LkServiceTag_EditValueChanged);
            //
            // LblReadingDate
            //
            this.LblReadingDate.Location = new System.Drawing.Point(14, 42);
            this.LblReadingDate.Name = "LblReadingDate";
            this.LblReadingDate.Size = new System.Drawing.Size(72, 13);
            this.LblReadingDate.TabIndex = 2;
            this.LblReadingDate.Text = "Reading Date :";
            //
            // DtReadingDate
            //
            this.DtReadingDate.EditValue = null;
            this.DtReadingDate.Location = new System.Drawing.Point(130, 39);
            this.DtReadingDate.Name = "DtReadingDate";
            this.DtReadingDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtReadingDate.Properties.DisplayFormat.FormatString = "d/M/yyyy h:mm:ss tt";
            this.DtReadingDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DtReadingDate.Properties.EditFormat.FormatString = "d/M/yyyy h:mm:ss tt";
            this.DtReadingDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DtReadingDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtReadingDate.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
            this.DtReadingDate.Size = new System.Drawing.Size(250, 20);
            this.DtReadingDate.TabIndex = 3;
            //
            // LblStockCode
            //
            this.LblStockCode.Location = new System.Drawing.Point(14, 74);
            this.LblStockCode.Name = "LblStockCode";
            this.LblStockCode.Size = new System.Drawing.Size(64, 13);
            this.LblStockCode.TabIndex = 4;
            this.LblStockCode.Text = "Stock Code :";
            //
            // LkStockCode
            //
            this.LkStockCode.Location = new System.Drawing.Point(130, 71);
            this.LkStockCode.Name = "LkStockCode";
            this.LkStockCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.LkStockCode.Properties.PopupFormWidth = 520;
            this.LkStockCode.Size = new System.Drawing.Size(250, 20);
            this.LkStockCode.TabIndex = 5;
            //
            // LblStockCodeDesc
            //
            this.LblStockCodeDesc.Location = new System.Drawing.Point(390, 74);
            this.LblStockCodeDesc.Name = "LblStockCodeDesc";
            this.LblStockCodeDesc.Size = new System.Drawing.Size(5, 13);
            this.LblStockCodeDesc.TabIndex = 6;
            this.LblStockCodeDesc.Text = "";
            //
            // LblDebtorCode
            //
            this.LblDebtorCode.Location = new System.Drawing.Point(14, 106);
            this.LblDebtorCode.Name = "LblDebtorCode";
            this.LblDebtorCode.Size = new System.Drawing.Size(70, 13);
            this.LblDebtorCode.TabIndex = 7;
            this.LblDebtorCode.Text = "Debtor Code :";
            //
            // LkDebtorCode
            //
            this.LkDebtorCode.Location = new System.Drawing.Point(130, 103);
            this.LkDebtorCode.Name = "LkDebtorCode";
            this.LkDebtorCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.LkDebtorCode.Properties.PopupFormWidth = 520;
            this.LkDebtorCode.Size = new System.Drawing.Size(250, 20);
            this.LkDebtorCode.TabIndex = 8;
            //
            // LblDebtorCodeDesc
            //
            this.LblDebtorCodeDesc.Location = new System.Drawing.Point(390, 106);
            this.LblDebtorCodeDesc.Name = "LblDebtorCodeDesc";
            this.LblDebtorCodeDesc.Size = new System.Drawing.Size(5, 13);
            this.LblDebtorCodeDesc.TabIndex = 9;
            this.LblDebtorCodeDesc.Text = "";
            //
            // LblDepartment — right column
            //
            this.LblDepartment.Location = new System.Drawing.Point(640, 10);
            this.LblDepartment.Name = "LblDepartment";
            this.LblDepartment.Size = new System.Drawing.Size(66, 13);
            this.LblDepartment.TabIndex = 10;
            this.LblDepartment.Text = "Department :";
            //
            // LkDepartment
            //
            this.LkDepartment.Location = new System.Drawing.Point(740, 7);
            this.LkDepartment.Name = "LkDepartment";
            this.LkDepartment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.LkDepartment.Properties.PopupFormWidth = 420;
            this.LkDepartment.Size = new System.Drawing.Size(200, 20);
            this.LkDepartment.TabIndex = 11;
            //
            // LblDeptDesc
            //
            this.LblDeptDesc.Location = new System.Drawing.Point(950, 10);
            this.LblDeptDesc.Name = "LblDeptDesc";
            this.LblDeptDesc.Size = new System.Drawing.Size(5, 13);
            this.LblDeptDesc.TabIndex = 12;
            this.LblDeptDesc.Text = "";
            //
            // LblJob
            //
            this.LblJob.Location = new System.Drawing.Point(640, 42);
            this.LblJob.Name = "LblJob";
            this.LblJob.Size = new System.Drawing.Size(24, 13);
            this.LblJob.TabIndex = 13;
            this.LblJob.Text = "Job :";
            //
            // LkJob
            //
            this.LkJob.Location = new System.Drawing.Point(740, 39);
            this.LkJob.Name = "LkJob";
            this.LkJob.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.LkJob.Properties.PopupFormWidth = 420;
            this.LkJob.Size = new System.Drawing.Size(200, 20);
            this.LkJob.TabIndex = 14;
            //
            // LblJobDesc
            //
            this.LblJobDesc.Location = new System.Drawing.Point(950, 42);
            this.LblJobDesc.Name = "LblJobDesc";
            this.LblJobDesc.Size = new System.Drawing.Size(5, 13);
            this.LblJobDesc.TabIndex = 15;
            this.LblJobDesc.Text = "";
            //
            // LblLocation
            //
            this.LblLocation.Location = new System.Drawing.Point(640, 74);
            this.LblLocation.Name = "LblLocation";
            this.LblLocation.Size = new System.Drawing.Size(48, 13);
            this.LblLocation.TabIndex = 16;
            this.LblLocation.Text = "Location :";
            //
            // LkLocation
            //
            this.LkLocation.Location = new System.Drawing.Point(740, 71);
            this.LkLocation.Name = "LkLocation";
            this.LkLocation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.LkLocation.Properties.PopupFormWidth = 420;
            this.LkLocation.Size = new System.Drawing.Size(200, 20);
            this.LkLocation.TabIndex = 17;
            //
            // LblLocationDesc
            //
            this.LblLocationDesc.Location = new System.Drawing.Point(950, 74);
            this.LblLocationDesc.Name = "LblLocationDesc";
            this.LblLocationDesc.Size = new System.Drawing.Size(5, 13);
            this.LblLocationDesc.TabIndex = 18;
            this.LblLocationDesc.Text = "";
            //
            // GrpSalesInvoice
            //
            this.GrpSalesInvoice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left;
            this.GrpSalesInvoice.Location = new System.Drawing.Point(14, 184);
            this.GrpSalesInvoice.Name = "GrpSalesInvoice";
            this.GrpSalesInvoice.Size = new System.Drawing.Size(610, 110);
            this.GrpSalesInvoice.TabIndex = 4;
            this.GrpSalesInvoice.Text = "Sales Invoice Settings";
            this.GrpSalesInvoice.Controls.Add(this.LblInvoiceDate);
            this.GrpSalesInvoice.Controls.Add(this.DtInvoiceDate);
            this.GrpSalesInvoice.Controls.Add(this.LblInvoiceNoFmt);
            this.GrpSalesInvoice.Controls.Add(this.CmbInvoiceNoFmt);
            this.GrpSalesInvoice.Controls.Add(this.LblDescription);
            this.GrpSalesInvoice.Controls.Add(this.CmbDescription);
            this.GrpSalesInvoice.Controls.Add(this.ChkTaxInclusive);
            //
            // LblInvoiceDate
            //
            this.LblInvoiceDate.Location = new System.Drawing.Point(12, 30);
            this.LblInvoiceDate.Name = "LblInvoiceDate";
            this.LblInvoiceDate.Size = new System.Drawing.Size(68, 13);
            this.LblInvoiceDate.TabIndex = 0;
            this.LblInvoiceDate.Text = "Invoice Date :";
            //
            // DtInvoiceDate
            //
            this.DtInvoiceDate.EditValue = null;
            this.DtInvoiceDate.Location = new System.Drawing.Point(110, 27);
            this.DtInvoiceDate.Name = "DtInvoiceDate";
            this.DtInvoiceDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtInvoiceDate.Properties.DisplayFormat.FormatString = "d/M/yyyy";
            this.DtInvoiceDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DtInvoiceDate.Properties.EditFormat.FormatString = "d/M/yyyy";
            this.DtInvoiceDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.DtInvoiceDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtInvoiceDate.Size = new System.Drawing.Size(160, 20);
            this.DtInvoiceDate.TabIndex = 1;
            //
            // LblInvoiceNoFmt
            //
            this.LblInvoiceNoFmt.Location = new System.Drawing.Point(300, 30);
            this.LblInvoiceNoFmt.Name = "LblInvoiceNoFmt";
            this.LblInvoiceNoFmt.Size = new System.Drawing.Size(86, 13);
            this.LblInvoiceNoFmt.TabIndex = 2;
            this.LblInvoiceNoFmt.Text = "Invoice No Format :";
            //
            // CmbInvoiceNoFmt
            //
            this.CmbInvoiceNoFmt.Location = new System.Drawing.Point(410, 27);
            this.CmbInvoiceNoFmt.Name = "CmbInvoiceNoFmt";
            this.CmbInvoiceNoFmt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)});
            this.CmbInvoiceNoFmt.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbInvoiceNoFmt.Size = new System.Drawing.Size(185, 20);
            this.CmbInvoiceNoFmt.TabIndex = 3;
            this.CmbInvoiceNoFmt.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbInvoiceNoFmt_ButtonClick);
            //
            // LblDescription
            //
            this.LblDescription.Location = new System.Drawing.Point(12, 56);
            this.LblDescription.Name = "LblDescription";
            this.LblDescription.Size = new System.Drawing.Size(60, 13);
            this.LblDescription.TabIndex = 4;
            this.LblDescription.Text = "Description :";
            //
            // CmbDescription
            //
            this.CmbDescription.Location = new System.Drawing.Point(110, 53);
            this.CmbDescription.Name = "CmbDescription";
            this.CmbDescription.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbDescription.Size = new System.Drawing.Size(485, 20);
            this.CmbDescription.TabIndex = 5;
            //
            // ChkTaxInclusive
            //
            this.ChkTaxInclusive.Location = new System.Drawing.Point(12, 80);
            this.ChkTaxInclusive.Name = "ChkTaxInclusive";
            this.ChkTaxInclusive.Properties.Caption = "Tax Inclusive";
            this.ChkTaxInclusive.Size = new System.Drawing.Size(120, 19);
            this.ChkTaxInclusive.TabIndex = 6;
            //
            // ChkUseRoundingAdj — outside group, right column
            //
            this.ChkUseRoundingAdj.Location = new System.Drawing.Point(640, 200);
            this.ChkUseRoundingAdj.Name = "ChkUseRoundingAdj";
            this.ChkUseRoundingAdj.Properties.Caption = "Use Rounding Adjustment";
            this.ChkUseRoundingAdj.Size = new System.Drawing.Size(190, 19);
            this.ChkUseRoundingAdj.TabIndex = 5;
            //
            // BtnTickSelection
            //
            this.BtnTickSelection.Location = new System.Drawing.Point(750, 260);
            this.BtnTickSelection.Name = "BtnTickSelection";
            this.BtnTickSelection.Size = new System.Drawing.Size(100, 26);
            this.BtnTickSelection.TabIndex = 6;
            this.BtnTickSelection.Text = "Tick Selection";
            this.BtnTickSelection.Click += new System.EventHandler(this.OnTickSelection);
            //
            // BtnSelectAll
            //
            this.BtnSelectAll.Location = new System.Drawing.Point(858, 260);
            this.BtnSelectAll.Name = "BtnSelectAll";
            this.BtnSelectAll.Size = new System.Drawing.Size(100, 26);
            this.BtnSelectAll.TabIndex = 7;
            this.BtnSelectAll.Text = "Select All";
            this.BtnSelectAll.Click += new System.EventHandler(this.OnSelectAll);
            //
            // BtnDeselectAll
            //
            this.BtnDeselectAll.Location = new System.Drawing.Point(966, 260);
            this.BtnDeselectAll.Name = "BtnDeselectAll";
            this.BtnDeselectAll.Size = new System.Drawing.Size(100, 26);
            this.BtnDeselectAll.TabIndex = 8;
            this.BtnDeselectAll.Text = "Deselect All";
            this.BtnDeselectAll.Click += new System.EventHandler(this.OnDeselectAll);
            //
            // BtnFetchReading
            //
            this.BtnFetchReading.Appearance.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
            this.BtnFetchReading.Appearance.ForeColor = System.Drawing.Color.White;
            this.BtnFetchReading.Appearance.Options.UseBackColor = true;
            this.BtnFetchReading.Appearance.Options.UseForeColor = true;
            this.BtnFetchReading.Location = new System.Drawing.Point(1074, 260);
            this.BtnFetchReading.Name = "BtnFetchReading";
            this.BtnFetchReading.Size = new System.Drawing.Size(110, 26);
            this.BtnFetchReading.TabIndex = 9;
            this.BtnFetchReading.Text = "Fetch Reading";
            this.BtnFetchReading.Click += new System.EventHandler(this.OnFetchReading);
            //
            // ChkSimulateFailure (demo toggle — empty caption, tiny, near the Fetch button)
            //
            ((System.ComponentModel.ISupportInitialize)(this.ChkSimulateFailure.Properties)).BeginInit();
            this.ChkSimulateFailure.Location = new System.Drawing.Point(1190, 264);
            this.ChkSimulateFailure.Name = "ChkSimulateFailure";
            this.ChkSimulateFailure.Properties.Caption = "";
            this.ChkSimulateFailure.Size = new System.Drawing.Size(18, 19);
            this.ChkSimulateFailure.TabIndex = 10;
            this.ChkSimulateFailure.TabStop = false;
            ((System.ComponentModel.ISupportInitialize)(this.ChkSimulateFailure.Properties)).EndInit();
            //
            // GridMeter
            //
            this.GridMeter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.GridMeter.Location = new System.Drawing.Point(14, 296);
            this.GridMeter.MainView = this.GridViewMeter;
            this.GridMeter.Name = "GridMeter";
            this.GridMeter.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
                this.RepoChkSelected,
                this.RepoChkUseMinCharges,
                this.RepoSpinReading});
            this.GridMeter.Size = new System.Drawing.Size(1336, 340);
            this.GridMeter.TabIndex = 9;
            this.GridMeter.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { this.GridViewMeter });
            //
            // GridViewMeter
            //
            this.GridViewMeter.GridControl = this.GridMeter;
            this.GridViewMeter.Name = "GridViewMeter";
            this.GridViewMeter.OptionsView.ShowGroupPanel = false;
            this.GridViewMeter.OptionsView.ColumnAutoWidth = true;
            this.GridViewMeter.OptionsView.ShowIndicator = false;
            this.GridViewMeter.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
                this.ColMeterType,
                this.ColMeterTypeName,
                this.ColMinCharges,
                this.ColUnitPrice,
                this.ColFOCQty,
                this.ColRebateQty,
                this.ColLastReadDate,
                this.ColLastReading,
                this.ColMeterUsage,
                this.ColTotalCharges,
                this.ColSelected,
                this.ColCurrentReading,
                this.ColUseMinCharges});
            //
            // ColMeterType
            //
            this.ColMeterType.Caption = "Meter Type";
            this.ColMeterType.FieldName = "MeterTypeCode";
            this.ColMeterType.Name = "ColMeterType";
            this.ColMeterType.VisibleIndex = 0;
            this.ColMeterType.Width = 90;
            this.ColMeterType.MinWidth = 60;
            this.ColMeterType.OptionsColumn.AllowEdit = false;
            //
            // ColMeterTypeName
            //
            this.ColMeterTypeName.Caption = "Meter Type Name";
            this.ColMeterTypeName.FieldName = "MeterTypeName";
            this.ColMeterTypeName.Name = "ColMeterTypeName";
            this.ColMeterTypeName.VisibleIndex = 1;
            this.ColMeterTypeName.Width = 130;
            this.ColMeterTypeName.MinWidth = 80;
            this.ColMeterTypeName.OptionsColumn.AllowEdit = false;
            //
            // ColMinCharges
            //
            this.ColMinCharges.Caption = "Min. Charges";
            this.ColMinCharges.FieldName = "MinCharges";
            this.ColMinCharges.Name = "ColMinCharges";
            this.ColMinCharges.VisibleIndex = 2;
            this.ColMinCharges.Width = 80;
            this.ColMinCharges.MinWidth = 60;
            this.ColMinCharges.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColMinCharges.DisplayFormat.FormatString = "n2";
            this.ColMinCharges.OptionsColumn.AllowEdit = false;
            //
            // ColUnitPrice
            //
            this.ColUnitPrice.Caption = "Unit Price";
            this.ColUnitPrice.FieldName = "UnitPrice";
            this.ColUnitPrice.Name = "ColUnitPrice";
            this.ColUnitPrice.VisibleIndex = 3;
            this.ColUnitPrice.Width = 70;
            this.ColUnitPrice.MinWidth = 50;
            this.ColUnitPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColUnitPrice.DisplayFormat.FormatString = "n3";
            this.ColUnitPrice.OptionsColumn.AllowEdit = false;
            //
            // ColFOCQty
            //
            this.ColFOCQty.Caption = "FOC Qty";
            this.ColFOCQty.FieldName = "FOCQty";
            this.ColFOCQty.Name = "ColFOCQty";
            this.ColFOCQty.VisibleIndex = 4;
            this.ColFOCQty.Width = 60;
            this.ColFOCQty.MinWidth = 40;
            this.ColFOCQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColFOCQty.DisplayFormat.FormatString = "n0";
            this.ColFOCQty.OptionsColumn.AllowEdit = false;
            //
            // ColRebateQty
            //
            this.ColRebateQty.Caption = "Rebate Qty (%)";
            this.ColRebateQty.FieldName = "RebateQtyPercent";
            this.ColRebateQty.Name = "ColRebateQty";
            this.ColRebateQty.VisibleIndex = 5;
            this.ColRebateQty.Width = 85;
            this.ColRebateQty.MinWidth = 60;
            this.ColRebateQty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColRebateQty.DisplayFormat.FormatString = "n2";
            this.ColRebateQty.OptionsColumn.AllowEdit = false;
            //
            // ColLastReadDate
            //
            this.ColLastReadDate.Caption = "Last Read Date";
            this.ColLastReadDate.FieldName = "LastReadDate";
            this.ColLastReadDate.Name = "ColLastReadDate";
            this.ColLastReadDate.VisibleIndex = 6;
            this.ColLastReadDate.Width = 120;
            this.ColLastReadDate.MinWidth = 80;
            this.ColLastReadDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ColLastReadDate.DisplayFormat.FormatString = "d/M/yyyy h:mm:ss tt";
            this.ColLastReadDate.OptionsColumn.AllowEdit = false;
            //
            // ColLastReading
            //
            this.ColLastReading.Caption = "Last Reading";
            this.ColLastReading.FieldName = "LastReading";
            this.ColLastReading.Name = "ColLastReading";
            this.ColLastReading.VisibleIndex = 7;
            this.ColLastReading.Width = 80;
            this.ColLastReading.MinWidth = 60;
            this.ColLastReading.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColLastReading.DisplayFormat.FormatString = "n0";
            this.ColLastReading.OptionsColumn.AllowEdit = false;
            //
            // ColMeterUsage
            //
            this.ColMeterUsage.Caption = "Meter Usage";
            this.ColMeterUsage.FieldName = "MeterUsage";
            this.ColMeterUsage.Name = "ColMeterUsage";
            this.ColMeterUsage.VisibleIndex = 8;
            this.ColMeterUsage.Width = 80;
            this.ColMeterUsage.MinWidth = 50;
            this.ColMeterUsage.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColMeterUsage.DisplayFormat.FormatString = "n0";
            this.ColMeterUsage.OptionsColumn.AllowEdit = false;
            //
            // ColTotalCharges
            //
            this.ColTotalCharges.Caption = "Total Charges";
            this.ColTotalCharges.FieldName = "TotalCharges";
            this.ColTotalCharges.Name = "ColTotalCharges";
            this.ColTotalCharges.VisibleIndex = 9;
            this.ColTotalCharges.Width = 85;
            this.ColTotalCharges.MinWidth = 60;
            this.ColTotalCharges.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColTotalCharges.DisplayFormat.FormatString = "n2";
            this.ColTotalCharges.OptionsColumn.AllowEdit = false;
            //
            // ColSelected
            //
            this.ColSelected.Caption = "Selected";
            this.ColSelected.FieldName = "Selected";
            this.ColSelected.Name = "ColSelected";
            this.ColSelected.VisibleIndex = 10;
            this.ColSelected.Width = 60;
            this.ColSelected.MinWidth = 50;
            this.ColSelected.ColumnEdit = this.RepoChkSelected;
            //
            // ColCurrentReading
            //
            this.ColCurrentReading.Caption = "Current Reading";
            this.ColCurrentReading.FieldName = "CurrentReading";
            this.ColCurrentReading.Name = "ColCurrentReading";
            this.ColCurrentReading.VisibleIndex = 11;
            this.ColCurrentReading.Width = 90;
            this.ColCurrentReading.MinWidth = 60;
            this.ColCurrentReading.ColumnEdit = this.RepoSpinReading;
            //
            // ColUseMinCharges
            //
            this.ColUseMinCharges.Caption = "Use Min. Charges";
            this.ColUseMinCharges.FieldName = "UseMinCharges";
            this.ColUseMinCharges.Name = "ColUseMinCharges";
            this.ColUseMinCharges.VisibleIndex = 12;
            this.ColUseMinCharges.Width = 90;
            this.ColUseMinCharges.MinWidth = 60;
            this.ColUseMinCharges.ColumnEdit = this.RepoChkUseMinCharges;
            //
            // RepoChkSelected
            //
            this.RepoChkSelected.Name = "RepoChkSelected";
            this.RepoChkSelected.ValueChecked = true;
            this.RepoChkSelected.ValueUnchecked = false;
            //
            // RepoChkUseMinCharges
            //
            this.RepoChkUseMinCharges.Name = "RepoChkUseMinCharges";
            this.RepoChkUseMinCharges.ValueChecked = true;
            this.RepoChkUseMinCharges.ValueUnchecked = false;
            //
            // RepoSpinReading
            //
            this.RepoSpinReading.Name = "RepoSpinReading";
            this.RepoSpinReading.IsFloatValue = false;
            this.RepoSpinReading.MinValue = 0;
            this.RepoSpinReading.MaxValue = 999999999;
            this.RepoSpinReading.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            //
            // PanelStatus
            //
            this.PanelStatus.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.PanelStatus.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PanelStatus.Location = new System.Drawing.Point(0, 640);
            this.PanelStatus.Name = "PanelStatus";
            this.PanelStatus.Size = new System.Drawing.Size(1364, 28);
            this.PanelStatus.TabIndex = 10;
            this.PanelStatus.Controls.Add(this.LblRowCount);
            this.PanelStatus.Controls.Add(this.LblTotal);
            //
            // LblRowCount
            //
            this.LblRowCount.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.LblRowCount.Appearance.Options.UseFont = true;
            this.LblRowCount.Location = new System.Drawing.Point(14, 6);
            this.LblRowCount.Name = "LblRowCount";
            this.LblRowCount.Size = new System.Drawing.Size(7, 14);
            this.LblRowCount.TabIndex = 0;
            this.LblRowCount.Text = "0";
            //
            // LblTotal
            //
            this.LblTotal.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.LblTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.LblTotal.Appearance.Options.UseFont = true;
            this.LblTotal.Location = new System.Drawing.Point(1250, 6);
            this.LblTotal.Name = "LblTotal";
            this.LblTotal.Size = new System.Drawing.Size(24, 14);
            this.LblTotal.TabIndex = 1;
            this.LblTotal.Text = "0.00";
            //
            // MeterTypeTransactionEntry_Form
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 670);
            this.Name = "MeterTypeTransactionEntry_Form";
            this.Text = "Meter Type Transaction Entry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.BtnSaveInvoice);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.PanelHeader);
            this.Controls.Add(this.GrpSalesInvoice);
            this.Controls.Add(this.ChkUseRoundingAdj);
            this.Controls.Add(this.BtnTickSelection);
            this.Controls.Add(this.BtnSelectAll);
            this.Controls.Add(this.BtnDeselectAll);
            this.Controls.Add(this.BtnFetchReading);
            this.Controls.Add(this.ChkSimulateFailure);
            this.Controls.Add(this.GridMeter);
            this.Controls.Add(this.PanelStatus);
            //
            // EndInit
            //
            ((System.ComponentModel.ISupportInitialize)(this.PanelHeader)).EndInit();
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LkServiceTag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtReadingDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtReadingDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkStockCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkDebtorCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LkLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpSalesInvoice)).EndInit();
            this.GrpSalesInvoice.ResumeLayout(false);
            this.GrpSalesInvoice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DtInvoiceDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtInvoiceDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbInvoiceNoFmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkTaxInclusive.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkUseRoundingAdj.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewMeter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridMeter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoChkSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoChkUseMinCharges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepoSpinReading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelStatus)).EndInit();
            this.PanelStatus.ResumeLayout(false);
            this.PanelStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
