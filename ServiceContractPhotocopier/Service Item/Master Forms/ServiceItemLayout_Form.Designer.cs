namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    partial class ServiceItemLayout_Form
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

        // Top button bar
        private DevExpress.XtraEditors.PanelControl PanelTop;
        private DevExpress.XtraEditors.LabelControl LblTitle;
        private DevExpress.XtraEditors.SimpleButton BtnFillTest;
        private DevExpress.XtraEditors.SimpleButton BtnGenerate;
        private DevExpress.XtraEditors.SimpleButton BtnEdit;
        private DevExpress.XtraEditors.SimpleButton BtnSave;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
        private DevExpress.XtraEditors.SimpleButton BtnDelete;
        private DevExpress.XtraEditors.SimpleButton BtnPrint;
        private DevExpress.XtraEditors.SimpleButton BtnAttachments;
        private DevExpress.XtraEditors.SimpleButton BtnCopyFrom;
        private DevExpress.XtraEditors.SimpleButton BtnExit;
        private DevExpress.XtraEditors.SimpleButton BtnNavFirst;
        private DevExpress.XtraEditors.SimpleButton BtnNavPrev;
        private DevExpress.XtraEditors.SimpleButton BtnNavNext;
        private DevExpress.XtraEditors.SimpleButton BtnNavLast;

        // Header LayoutControl + items
        private DevExpress.XtraLayout.LayoutControl LytHeader;
        private DevExpress.XtraLayout.LayoutControlGroup LytHeaderRoot;

        // Header form controls (left column)
        private DevExpress.XtraEditors.DateEdit DtPurchaseDate;
        private DevExpress.XtraEditors.CheckEdit ChkInactive;
        private DevExpress.XtraEditors.TextEdit TxtServiceTag;
        private DevExpress.XtraEditors.SimpleButton BtnAutoTag;
        private DevExpress.XtraEditors.ComboBoxEdit CmbStockCode;
        private DevExpress.XtraEditors.ComboBoxEdit CmbDebtorCode;
        private DevExpress.XtraEditors.ComboBoxEdit CmbAgentCode;
        private DevExpress.XtraEditors.ComboBoxEdit CmbTerm;
        private DevExpress.XtraEditors.TextEdit TxtRefNo;
        private DevExpress.XtraEditors.ComboBoxEdit CmbArea;
        private DevExpress.XtraEditors.ComboBoxEdit CmbGradeCode;
        private DevExpress.XtraEditors.SpinEdit SpnUnitPrice;
        private DevExpress.XtraEditors.TextEdit TxtDescription;

        // Header form controls (right column)
        private DevExpress.XtraEditors.ComboBoxEdit CmbContractNo;
        private DevExpress.XtraEditors.SimpleButton BtnResetContract;
        private DevExpress.XtraEditors.DateEdit DtServiceStartDate;
        private DevExpress.XtraEditors.DateEdit DtServiceEndDate;
        private DevExpress.XtraEditors.MemoEdit TxtAddress;
        private DevExpress.XtraEditors.SimpleButton BtnResetDebtorOwnership;
        private DevExpress.XtraEditors.TextEdit TxtAttention;

        // Header LayoutControlItems (left)
        private DevExpress.XtraLayout.LayoutControlItem LytItemPurchaseDate;
        private DevExpress.XtraLayout.LayoutControlItem LytItemInactive;
        private DevExpress.XtraLayout.LayoutControlItem LytItemServiceTag;
        private DevExpress.XtraLayout.LayoutControlItem LytItemAutoTag;
        private DevExpress.XtraLayout.LayoutControlItem LytItemStockCode;
        private DevExpress.XtraLayout.LayoutControlItem LytItemDebtorCode;
        private DevExpress.XtraLayout.LayoutControlItem LytItemAgentCode;
        private DevExpress.XtraLayout.LayoutControlItem LytItemTerm;
        private DevExpress.XtraLayout.LayoutControlItem LytItemRefNo;
        private DevExpress.XtraLayout.LayoutControlItem LytItemArea;
        private DevExpress.XtraLayout.LayoutControlItem LytItemGradeCode;
        private DevExpress.XtraLayout.LayoutControlItem LytItemUnitPrice;
        private DevExpress.XtraLayout.LayoutControlItem LytItemDescription;

        // Header LayoutControlItems (right)
        private DevExpress.XtraLayout.LayoutControlItem LytItemContractNo;
        private DevExpress.XtraLayout.LayoutControlItem LytItemResetContract;
        private DevExpress.XtraLayout.LayoutControlItem LytItemServiceStartDate;
        private DevExpress.XtraLayout.LayoutControlItem LytItemServiceEndDate;
        private DevExpress.XtraLayout.LayoutControlItem LytItemAddress;
        private DevExpress.XtraLayout.LayoutControlItem LytItemResetDebtorOwnership;
        private DevExpress.XtraLayout.LayoutControlItem LytItemAttention;

        // Tab control + pages
        private DevExpress.XtraTab.XtraTabControl TabMain;
        private DevExpress.XtraTab.XtraTabPage PageMain;
        private DevExpress.XtraTab.XtraTabPage PageMoreHeader;
        private DevExpress.XtraTab.XtraTabPage PageNote;
        private DevExpress.XtraTab.XtraTabPage PageRemarks;
        private DevExpress.XtraTab.XtraTabPage PageServiceNoteHistory;
        private DevExpress.XtraTab.XtraTabPage PageDebtorsOwnershipHistory;
        private DevExpress.XtraTab.XtraTabPage PageMeterType;

        // Tab 1 (Main) — Preventive Maintenance via nested LayoutControl
        private DevExpress.XtraEditors.GroupControl GrpPM;
        private DevExpress.XtraLayout.LayoutControl LytPM;
        private DevExpress.XtraLayout.LayoutControlGroup LytPMRoot;
        private DevExpress.XtraEditors.CheckEdit ChkPMActive;
        private DevExpress.XtraEditors.ComboBoxEdit CmbPMIntervalType;
        private DevExpress.XtraEditors.SpinEdit SpnPMIntervalValue;
        private DevExpress.XtraEditors.DateEdit DtPMStartDate;
        private DevExpress.XtraEditors.DateEdit DtPMLastServiceDate;
        private DevExpress.XtraEditors.DateEdit DtPMNextServiceDate;
        private DevExpress.XtraEditors.ComboBoxEdit CmbDepartment;
        private DevExpress.XtraEditors.ComboBoxEdit CmbJob;
        private DevExpress.XtraEditors.ComboBoxEdit CmbLocation;
        private DevExpress.XtraLayout.LayoutControlItem LytItemPMActive;
        private DevExpress.XtraLayout.LayoutControlItem LytItemPMIntervalType;
        private DevExpress.XtraLayout.LayoutControlItem LytItemPMIntervalValue;
        private DevExpress.XtraLayout.LayoutControlItem LytItemPMStartDate;
        private DevExpress.XtraLayout.LayoutControlItem LytItemPMLastServiceDate;
        private DevExpress.XtraLayout.LayoutControlItem LytItemPMNextServiceDate;
        private DevExpress.XtraLayout.LayoutControlItem LytItemDepartment;
        private DevExpress.XtraLayout.LayoutControlItem LytItemJob;
        private DevExpress.XtraLayout.LayoutControlItem LytItemLocation;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PanelTop = new DevExpress.XtraEditors.PanelControl();
            this.LblTitle = new DevExpress.XtraEditors.LabelControl();
            this.BtnFillTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGenerate = new DevExpress.XtraEditors.SimpleButton();
            this.BtnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSave = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.BtnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.BtnAttachments = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCopyFrom = new DevExpress.XtraEditors.SimpleButton();
            this.BtnExit = new DevExpress.XtraEditors.SimpleButton();
            this.BtnNavFirst = new DevExpress.XtraEditors.SimpleButton();
            this.BtnNavPrev = new DevExpress.XtraEditors.SimpleButton();
            this.BtnNavNext = new DevExpress.XtraEditors.SimpleButton();
            this.BtnNavLast = new DevExpress.XtraEditors.SimpleButton();

            this.LytHeader = new DevExpress.XtraLayout.LayoutControl();
            this.LytHeaderRoot = new DevExpress.XtraLayout.LayoutControlGroup();

            this.DtPurchaseDate = new DevExpress.XtraEditors.DateEdit();
            this.ChkInactive = new DevExpress.XtraEditors.CheckEdit();
            this.TxtServiceTag = new DevExpress.XtraEditors.TextEdit();
            this.BtnAutoTag = new DevExpress.XtraEditors.SimpleButton();
            this.CmbStockCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbDebtorCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbAgentCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbTerm = new DevExpress.XtraEditors.ComboBoxEdit();
            this.TxtRefNo = new DevExpress.XtraEditors.TextEdit();
            this.CmbArea = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbGradeCode = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SpnUnitPrice = new DevExpress.XtraEditors.SpinEdit();
            this.TxtDescription = new DevExpress.XtraEditors.TextEdit();
            this.CmbContractNo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BtnResetContract = new DevExpress.XtraEditors.SimpleButton();
            this.DtServiceStartDate = new DevExpress.XtraEditors.DateEdit();
            this.DtServiceEndDate = new DevExpress.XtraEditors.DateEdit();
            this.TxtAddress = new DevExpress.XtraEditors.MemoEdit();
            this.BtnResetDebtorOwnership = new DevExpress.XtraEditors.SimpleButton();
            this.TxtAttention = new DevExpress.XtraEditors.TextEdit();

            this.LytItemPurchaseDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemInactive = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemServiceTag = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemAutoTag = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemStockCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemDebtorCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemAgentCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemTerm = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemRefNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemArea = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemGradeCode = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemUnitPrice = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemContractNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemResetContract = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemServiceStartDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemServiceEndDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemAddress = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemResetDebtorOwnership = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemAttention = new DevExpress.XtraLayout.LayoutControlItem();

            this.TabMain = new DevExpress.XtraTab.XtraTabControl();
            this.PageMain = new DevExpress.XtraTab.XtraTabPage();
            this.PageMoreHeader = new DevExpress.XtraTab.XtraTabPage();
            this.PageNote = new DevExpress.XtraTab.XtraTabPage();
            this.PageRemarks = new DevExpress.XtraTab.XtraTabPage();
            this.PageServiceNoteHistory = new DevExpress.XtraTab.XtraTabPage();
            this.PageDebtorsOwnershipHistory = new DevExpress.XtraTab.XtraTabPage();
            this.PageMeterType = new DevExpress.XtraTab.XtraTabPage();

            this.GrpPM = new DevExpress.XtraEditors.GroupControl();
            this.LytPM = new DevExpress.XtraLayout.LayoutControl();
            this.LytPMRoot = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ChkPMActive = new DevExpress.XtraEditors.CheckEdit();
            this.CmbPMIntervalType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SpnPMIntervalValue = new DevExpress.XtraEditors.SpinEdit();
            this.DtPMStartDate = new DevExpress.XtraEditors.DateEdit();
            this.DtPMLastServiceDate = new DevExpress.XtraEditors.DateEdit();
            this.DtPMNextServiceDate = new DevExpress.XtraEditors.DateEdit();
            this.CmbDepartment = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbJob = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbLocation = new DevExpress.XtraEditors.ComboBoxEdit();
            this.LytItemPMActive = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemPMIntervalType = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemPMIntervalValue = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemPMStartDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemPMLastServiceDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemPMNextServiceDate = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemDepartment = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemJob = new DevExpress.XtraLayout.LayoutControlItem();
            this.LytItemLocation = new DevExpress.XtraLayout.LayoutControlItem();

            ((System.ComponentModel.ISupportInitialize)(this.PanelTop)).BeginInit();
            this.PanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LytHeader)).BeginInit();
            this.LytHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LytHeaderRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPurchaseDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPurchaseDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkInactive.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtServiceTag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbStockCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDebtorCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbAgentCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbTerm.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtRefNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbGradeCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpnUnitPrice.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbContractNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAttention.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPurchaseDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemInactive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemServiceTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAutoTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemStockCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemDebtorCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAgentCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemTerm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemRefNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemGradeCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemUnitPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemContractNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemResetContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemServiceStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemServiceEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemResetDebtorOwnership)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAttention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabMain)).BeginInit();
            this.TabMain.SuspendLayout();
            this.PageMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpPM)).BeginInit();
            this.GrpPM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LytPM)).BeginInit();
            this.LytPM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LytPMRoot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkPMActive.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbPMIntervalType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpnPMIntervalValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMLastServiceDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMLastServiceDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMNextServiceDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMNextServiceDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbJob.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbLocation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMIntervalType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMIntervalValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMLastServiceDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMNextServiceDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemJob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemLocation)).BeginInit();
            this.SuspendLayout();

            // PanelTop
            this.PanelTop.Controls.Add(this.LblTitle);
            this.PanelTop.Controls.Add(this.BtnFillTest);
            this.PanelTop.Controls.Add(this.BtnGenerate);
            this.PanelTop.Controls.Add(this.BtnEdit);
            this.PanelTop.Controls.Add(this.BtnSave);
            this.PanelTop.Controls.Add(this.BtnCancel);
            this.PanelTop.Controls.Add(this.BtnDelete);
            this.PanelTop.Controls.Add(this.BtnPrint);
            this.PanelTop.Controls.Add(this.BtnAttachments);
            this.PanelTop.Controls.Add(this.BtnCopyFrom);
            this.PanelTop.Controls.Add(this.BtnExit);
            this.PanelTop.Controls.Add(this.BtnNavFirst);
            this.PanelTop.Controls.Add(this.BtnNavPrev);
            this.PanelTop.Controls.Add(this.BtnNavNext);
            this.PanelTop.Controls.Add(this.BtnNavLast);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(1180, 80);
            this.PanelTop.TabIndex = 0;

            // LblTitle
            this.LblTitle.Appearance.FontSizeDelta = 4;
            this.LblTitle.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.LblTitle.Appearance.ForeColor = System.Drawing.Color.Firebrick;
            this.LblTitle.Appearance.Options.UseFont = true;
            this.LblTitle.Appearance.Options.UseForeColor = true;
            this.LblTitle.Location = new System.Drawing.Point(12, 10);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(280, 24);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Maintain Service Item (Layout)";

            // BtnFillTest
            this.BtnFillTest.Appearance.BackColor = System.Drawing.Color.Gold;
            this.BtnFillTest.Appearance.Options.UseBackColor = true;
            this.BtnFillTest.Location = new System.Drawing.Point(1060, 8);
            this.BtnFillTest.Name = "BtnFillTest";
            this.BtnFillTest.Size = new System.Drawing.Size(108, 26);
            this.BtnFillTest.TabIndex = 1;
            this.BtnFillTest.Text = "Fill Test Data";

            // Action buttons row
            this.BtnGenerate.Location = new System.Drawing.Point(12, 44);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(132, 26);
            this.BtnGenerate.TabIndex = 2;
            this.BtnGenerate.Text = "Generate Service Item";

            this.BtnEdit.Location = new System.Drawing.Point(150, 44);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(74, 26);
            this.BtnEdit.TabIndex = 3;
            this.BtnEdit.Text = "Edit (F6)";

            this.BtnSave.Location = new System.Drawing.Point(228, 44);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(74, 26);
            this.BtnSave.TabIndex = 4;
            this.BtnSave.Text = "Save (F7)";

            this.BtnCancel.Location = new System.Drawing.Point(306, 44);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(82, 26);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "Cancel (F8)";

            this.BtnDelete.Location = new System.Drawing.Point(392, 44);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(82, 26);
            this.BtnDelete.TabIndex = 6;
            this.BtnDelete.Text = "Delete (F9)";

            this.BtnPrint.Location = new System.Drawing.Point(478, 44);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(82, 26);
            this.BtnPrint.TabIndex = 7;
            this.BtnPrint.Text = "Print (F11)";

            this.BtnAttachments.Location = new System.Drawing.Point(720, 44);
            this.BtnAttachments.Name = "BtnAttachments";
            this.BtnAttachments.Size = new System.Drawing.Size(86, 26);
            this.BtnAttachments.TabIndex = 8;
            this.BtnAttachments.Text = "Attachments";

            this.BtnCopyFrom.Location = new System.Drawing.Point(810, 44);
            this.BtnCopyFrom.Name = "BtnCopyFrom";
            this.BtnCopyFrom.Size = new System.Drawing.Size(86, 26);
            this.BtnCopyFrom.TabIndex = 9;
            this.BtnCopyFrom.Text = "Copy From...";

            this.BtnExit.Location = new System.Drawing.Point(900, 44);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(74, 26);
            this.BtnExit.TabIndex = 10;
            this.BtnExit.Text = "Exit (F2)";

            this.BtnNavFirst.Location = new System.Drawing.Point(984, 44);
            this.BtnNavFirst.Name = "BtnNavFirst";
            this.BtnNavFirst.Size = new System.Drawing.Size(40, 26);
            this.BtnNavFirst.TabIndex = 11;
            this.BtnNavFirst.Text = "|<<";

            this.BtnNavPrev.Location = new System.Drawing.Point(1028, 44);
            this.BtnNavPrev.Name = "BtnNavPrev";
            this.BtnNavPrev.Size = new System.Drawing.Size(40, 26);
            this.BtnNavPrev.TabIndex = 12;
            this.BtnNavPrev.Text = "<";

            this.BtnNavNext.Location = new System.Drawing.Point(1072, 44);
            this.BtnNavNext.Name = "BtnNavNext";
            this.BtnNavNext.Size = new System.Drawing.Size(40, 26);
            this.BtnNavNext.TabIndex = 13;
            this.BtnNavNext.Text = ">";

            this.BtnNavLast.Location = new System.Drawing.Point(1116, 44);
            this.BtnNavLast.Name = "BtnNavLast";
            this.BtnNavLast.Size = new System.Drawing.Size(48, 26);
            this.BtnNavLast.TabIndex = 14;
            this.BtnNavLast.Text = ">>|";

            // LytHeader (LayoutControl filling top portion)
            this.LytHeader.Controls.Add(this.DtPurchaseDate);
            this.LytHeader.Controls.Add(this.ChkInactive);
            this.LytHeader.Controls.Add(this.TxtServiceTag);
            this.LytHeader.Controls.Add(this.BtnAutoTag);
            this.LytHeader.Controls.Add(this.CmbStockCode);
            this.LytHeader.Controls.Add(this.CmbDebtorCode);
            this.LytHeader.Controls.Add(this.CmbAgentCode);
            this.LytHeader.Controls.Add(this.CmbTerm);
            this.LytHeader.Controls.Add(this.TxtRefNo);
            this.LytHeader.Controls.Add(this.CmbArea);
            this.LytHeader.Controls.Add(this.CmbGradeCode);
            this.LytHeader.Controls.Add(this.SpnUnitPrice);
            this.LytHeader.Controls.Add(this.TxtDescription);
            this.LytHeader.Controls.Add(this.CmbContractNo);
            this.LytHeader.Controls.Add(this.BtnResetContract);
            this.LytHeader.Controls.Add(this.DtServiceStartDate);
            this.LytHeader.Controls.Add(this.DtServiceEndDate);
            this.LytHeader.Controls.Add(this.TxtAddress);
            this.LytHeader.Controls.Add(this.BtnResetDebtorOwnership);
            this.LytHeader.Controls.Add(this.TxtAttention);
            this.LytHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.LytHeader.Location = new System.Drawing.Point(0, 80);
            this.LytHeader.Name = "LytHeader";
            this.LytHeader.Root = this.LytHeaderRoot;
            this.LytHeader.Size = new System.Drawing.Size(1180, 280);
            this.LytHeader.TabIndex = 1;

            // Header form controls — basic config, layout managed by items
            this.DtPurchaseDate.EditValue = null;
            this.DtPurchaseDate.Name = "DtPurchaseDate";
            this.DtPurchaseDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtPurchaseDate.Size = new System.Drawing.Size(170, 22);
            this.DtPurchaseDate.StyleController = this.LytHeader;
            this.DtPurchaseDate.TabIndex = 0;

            this.ChkInactive.Name = "ChkInactive";
            this.ChkInactive.Properties.Caption = "Inactive";
            this.ChkInactive.Size = new System.Drawing.Size(80, 19);
            this.ChkInactive.StyleController = this.LytHeader;
            this.ChkInactive.TabIndex = 1;

            this.TxtServiceTag.Name = "TxtServiceTag";
            this.TxtServiceTag.Size = new System.Drawing.Size(170, 22);
            this.TxtServiceTag.StyleController = this.LytHeader;
            this.TxtServiceTag.TabIndex = 2;

            this.BtnAutoTag.Name = "BtnAutoTag";
            this.BtnAutoTag.Size = new System.Drawing.Size(80, 22);
            this.BtnAutoTag.StyleController = this.LytHeader;
            this.BtnAutoTag.TabIndex = 3;
            this.BtnAutoTag.Text = "Auto (F12)";

            this.CmbStockCode.Name = "CmbStockCode";
            this.CmbStockCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbStockCode.Size = new System.Drawing.Size(170, 22);
            this.CmbStockCode.StyleController = this.LytHeader;
            this.CmbStockCode.TabIndex = 4;

            this.CmbDebtorCode.Name = "CmbDebtorCode";
            this.CmbDebtorCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbDebtorCode.Size = new System.Drawing.Size(170, 22);
            this.CmbDebtorCode.StyleController = this.LytHeader;
            this.CmbDebtorCode.TabIndex = 5;

            this.CmbAgentCode.Name = "CmbAgentCode";
            this.CmbAgentCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbAgentCode.Size = new System.Drawing.Size(170, 22);
            this.CmbAgentCode.StyleController = this.LytHeader;
            this.CmbAgentCode.TabIndex = 6;

            this.CmbTerm.Name = "CmbTerm";
            this.CmbTerm.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbTerm.Size = new System.Drawing.Size(170, 22);
            this.CmbTerm.StyleController = this.LytHeader;
            this.CmbTerm.TabIndex = 7;

            this.TxtRefNo.Name = "TxtRefNo";
            this.TxtRefNo.Size = new System.Drawing.Size(170, 22);
            this.TxtRefNo.StyleController = this.LytHeader;
            this.TxtRefNo.TabIndex = 8;

            this.CmbArea.Name = "CmbArea";
            this.CmbArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbArea.Size = new System.Drawing.Size(170, 22);
            this.CmbArea.StyleController = this.LytHeader;
            this.CmbArea.TabIndex = 9;

            this.CmbGradeCode.Name = "CmbGradeCode";
            this.CmbGradeCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbGradeCode.Size = new System.Drawing.Size(170, 22);
            this.CmbGradeCode.StyleController = this.LytHeader;
            this.CmbGradeCode.TabIndex = 10;

            this.SpnUnitPrice.Name = "SpnUnitPrice";
            this.SpnUnitPrice.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpnUnitPrice.Size = new System.Drawing.Size(170, 22);
            this.SpnUnitPrice.StyleController = this.LytHeader;
            this.SpnUnitPrice.TabIndex = 11;

            this.TxtDescription.Name = "TxtDescription";
            this.TxtDescription.Size = new System.Drawing.Size(440, 22);
            this.TxtDescription.StyleController = this.LytHeader;
            this.TxtDescription.TabIndex = 12;

            this.CmbContractNo.Name = "CmbContractNo";
            this.CmbContractNo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbContractNo.Size = new System.Drawing.Size(220, 22);
            this.CmbContractNo.StyleController = this.LytHeader;
            this.CmbContractNo.TabIndex = 13;

            this.BtnResetContract.Name = "BtnResetContract";
            this.BtnResetContract.Size = new System.Drawing.Size(110, 22);
            this.BtnResetContract.StyleController = this.LytHeader;
            this.BtnResetContract.TabIndex = 14;
            this.BtnResetContract.Text = "Reset Contract";

            this.DtServiceStartDate.EditValue = null;
            this.DtServiceStartDate.Name = "DtServiceStartDate";
            this.DtServiceStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtServiceStartDate.Size = new System.Drawing.Size(140, 22);
            this.DtServiceStartDate.StyleController = this.LytHeader;
            this.DtServiceStartDate.TabIndex = 15;

            this.DtServiceEndDate.EditValue = null;
            this.DtServiceEndDate.Name = "DtServiceEndDate";
            this.DtServiceEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtServiceEndDate.Size = new System.Drawing.Size(140, 22);
            this.DtServiceEndDate.StyleController = this.LytHeader;
            this.DtServiceEndDate.TabIndex = 16;

            this.TxtAddress.Name = "TxtAddress";
            this.TxtAddress.Size = new System.Drawing.Size(220, 80);
            this.TxtAddress.StyleController = this.LytHeader;
            this.TxtAddress.TabIndex = 17;

            this.BtnResetDebtorOwnership.Name = "BtnResetDebtorOwnership";
            this.BtnResetDebtorOwnership.Size = new System.Drawing.Size(110, 56);
            this.BtnResetDebtorOwnership.StyleController = this.LytHeader;
            this.BtnResetDebtorOwnership.TabIndex = 18;
            this.BtnResetDebtorOwnership.Text = "Reset Debtor\r\nOwnership";

            this.TxtAttention.Name = "TxtAttention";
            this.TxtAttention.Size = new System.Drawing.Size(340, 22);
            this.TxtAttention.StyleController = this.LytHeader;
            this.TxtAttention.TabIndex = 19;

            // LytHeaderRoot
            this.LytHeaderRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.LytHeaderRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
                this.LytItemPurchaseDate,
                this.LytItemInactive,
                this.LytItemServiceTag,
                this.LytItemAutoTag,
                this.LytItemStockCode,
                this.LytItemDebtorCode,
                this.LytItemAgentCode,
                this.LytItemTerm,
                this.LytItemRefNo,
                this.LytItemArea,
                this.LytItemGradeCode,
                this.LytItemUnitPrice,
                this.LytItemDescription,
                this.LytItemContractNo,
                this.LytItemResetContract,
                this.LytItemServiceStartDate,
                this.LytItemServiceEndDate,
                this.LytItemAddress,
                this.LytItemResetDebtorOwnership,
                this.LytItemAttention});
            this.LytHeaderRoot.Name = "LytHeaderRoot";
            this.LytHeaderRoot.Size = new System.Drawing.Size(1180, 280);
            this.LytHeaderRoot.TextVisible = false;

            // Items — left column (X = 0..540)
            this.LytItemPurchaseDate.Control = this.DtPurchaseDate;
            this.LytItemPurchaseDate.Location = new System.Drawing.Point(0, 0);
            this.LytItemPurchaseDate.Name = "LytItemPurchaseDate";
            this.LytItemPurchaseDate.Size = new System.Drawing.Size(290, 28);
            this.LytItemPurchaseDate.Text = "Purchases Date";
            this.LytItemPurchaseDate.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemInactive.Control = this.ChkInactive;
            this.LytItemInactive.Location = new System.Drawing.Point(290, 0);
            this.LytItemInactive.Name = "LytItemInactive";
            this.LytItemInactive.Size = new System.Drawing.Size(290, 28);
            this.LytItemInactive.TextSize = new System.Drawing.Size(0, 0);
            this.LytItemInactive.TextVisible = false;

            this.LytItemServiceTag.Control = this.TxtServiceTag;
            this.LytItemServiceTag.Location = new System.Drawing.Point(0, 28);
            this.LytItemServiceTag.Name = "LytItemServiceTag";
            this.LytItemServiceTag.Size = new System.Drawing.Size(290, 28);
            this.LytItemServiceTag.Text = "Service Tag";
            this.LytItemServiceTag.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemAutoTag.Control = this.BtnAutoTag;
            this.LytItemAutoTag.Location = new System.Drawing.Point(290, 28);
            this.LytItemAutoTag.Name = "LytItemAutoTag";
            this.LytItemAutoTag.Size = new System.Drawing.Size(290, 28);
            this.LytItemAutoTag.TextSize = new System.Drawing.Size(0, 0);
            this.LytItemAutoTag.TextVisible = false;

            this.LytItemStockCode.Control = this.CmbStockCode;
            this.LytItemStockCode.Location = new System.Drawing.Point(0, 56);
            this.LytItemStockCode.Name = "LytItemStockCode";
            this.LytItemStockCode.Size = new System.Drawing.Size(580, 28);
            this.LytItemStockCode.Text = "Stock Code";
            this.LytItemStockCode.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemDebtorCode.Control = this.CmbDebtorCode;
            this.LytItemDebtorCode.Location = new System.Drawing.Point(0, 84);
            this.LytItemDebtorCode.Name = "LytItemDebtorCode";
            this.LytItemDebtorCode.Size = new System.Drawing.Size(580, 28);
            this.LytItemDebtorCode.Text = "Debtor Code";
            this.LytItemDebtorCode.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemAgentCode.Control = this.CmbAgentCode;
            this.LytItemAgentCode.Location = new System.Drawing.Point(0, 112);
            this.LytItemAgentCode.Name = "LytItemAgentCode";
            this.LytItemAgentCode.Size = new System.Drawing.Size(290, 28);
            this.LytItemAgentCode.Text = "Agent Code";
            this.LytItemAgentCode.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemTerm.Control = this.CmbTerm;
            this.LytItemTerm.Location = new System.Drawing.Point(290, 112);
            this.LytItemTerm.Name = "LytItemTerm";
            this.LytItemTerm.Size = new System.Drawing.Size(290, 28);
            this.LytItemTerm.Text = "Term";
            this.LytItemTerm.TextSize = new System.Drawing.Size(50, 13);

            this.LytItemRefNo.Control = this.TxtRefNo;
            this.LytItemRefNo.Location = new System.Drawing.Point(0, 140);
            this.LytItemRefNo.Name = "LytItemRefNo";
            this.LytItemRefNo.Size = new System.Drawing.Size(290, 28);
            this.LytItemRefNo.Text = "Reference No";
            this.LytItemRefNo.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemArea.Control = this.CmbArea;
            this.LytItemArea.Location = new System.Drawing.Point(290, 140);
            this.LytItemArea.Name = "LytItemArea";
            this.LytItemArea.Size = new System.Drawing.Size(290, 28);
            this.LytItemArea.Text = "Area";
            this.LytItemArea.TextSize = new System.Drawing.Size(50, 13);

            this.LytItemGradeCode.Control = this.CmbGradeCode;
            this.LytItemGradeCode.Location = new System.Drawing.Point(0, 168);
            this.LytItemGradeCode.Name = "LytItemGradeCode";
            this.LytItemGradeCode.Size = new System.Drawing.Size(290, 28);
            this.LytItemGradeCode.Text = "Grade Code";
            this.LytItemGradeCode.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemUnitPrice.Control = this.SpnUnitPrice;
            this.LytItemUnitPrice.Location = new System.Drawing.Point(290, 168);
            this.LytItemUnitPrice.Name = "LytItemUnitPrice";
            this.LytItemUnitPrice.Size = new System.Drawing.Size(290, 28);
            this.LytItemUnitPrice.Text = "Unit Price";
            this.LytItemUnitPrice.TextSize = new System.Drawing.Size(50, 13);

            this.LytItemDescription.Control = this.TxtDescription;
            this.LytItemDescription.Location = new System.Drawing.Point(0, 196);
            this.LytItemDescription.Name = "LytItemDescription";
            this.LytItemDescription.Size = new System.Drawing.Size(580, 28);
            this.LytItemDescription.Text = "Description";
            this.LytItemDescription.TextSize = new System.Drawing.Size(95, 13);

            // Items — right column (X = 580..1180)
            this.LytItemContractNo.Control = this.CmbContractNo;
            this.LytItemContractNo.Location = new System.Drawing.Point(580, 0);
            this.LytItemContractNo.Name = "LytItemContractNo";
            this.LytItemContractNo.Size = new System.Drawing.Size(450, 28);
            this.LytItemContractNo.Text = "Contract No";
            this.LytItemContractNo.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemResetContract.Control = this.BtnResetContract;
            this.LytItemResetContract.Location = new System.Drawing.Point(1030, 0);
            this.LytItemResetContract.Name = "LytItemResetContract";
            this.LytItemResetContract.Size = new System.Drawing.Size(150, 28);
            this.LytItemResetContract.TextSize = new System.Drawing.Size(0, 0);
            this.LytItemResetContract.TextVisible = false;

            this.LytItemServiceStartDate.Control = this.DtServiceStartDate;
            this.LytItemServiceStartDate.Location = new System.Drawing.Point(580, 28);
            this.LytItemServiceStartDate.Name = "LytItemServiceStartDate";
            this.LytItemServiceStartDate.Size = new System.Drawing.Size(300, 28);
            this.LytItemServiceStartDate.Text = "Service Start Date";
            this.LytItemServiceStartDate.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemServiceEndDate.Control = this.DtServiceEndDate;
            this.LytItemServiceEndDate.Location = new System.Drawing.Point(880, 28);
            this.LytItemServiceEndDate.Name = "LytItemServiceEndDate";
            this.LytItemServiceEndDate.Size = new System.Drawing.Size(150, 28);
            this.LytItemServiceEndDate.Text = "To";
            this.LytItemServiceEndDate.TextSize = new System.Drawing.Size(20, 13);

            this.LytItemAddress.Control = this.TxtAddress;
            this.LytItemAddress.Location = new System.Drawing.Point(580, 56);
            this.LytItemAddress.Name = "LytItemAddress";
            this.LytItemAddress.Size = new System.Drawing.Size(450, 84);
            this.LytItemAddress.Text = "Address";
            this.LytItemAddress.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemResetDebtorOwnership.Control = this.BtnResetDebtorOwnership;
            this.LytItemResetDebtorOwnership.Location = new System.Drawing.Point(1030, 56);
            this.LytItemResetDebtorOwnership.Name = "LytItemResetDebtorOwnership";
            this.LytItemResetDebtorOwnership.Size = new System.Drawing.Size(150, 84);
            this.LytItemResetDebtorOwnership.TextSize = new System.Drawing.Size(0, 0);
            this.LytItemResetDebtorOwnership.TextVisible = false;

            this.LytItemAttention.Control = this.TxtAttention;
            this.LytItemAttention.Location = new System.Drawing.Point(580, 140);
            this.LytItemAttention.Name = "LytItemAttention";
            this.LytItemAttention.Size = new System.Drawing.Size(600, 28);
            this.LytItemAttention.Text = "Attention";
            this.LytItemAttention.TextSize = new System.Drawing.Size(95, 13);

            // TabMain
            this.TabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabMain.Location = new System.Drawing.Point(0, 360);
            this.TabMain.Name = "TabMain";
            this.TabMain.SelectedTabPage = this.PageMain;
            this.TabMain.Size = new System.Drawing.Size(1180, 340);
            this.TabMain.TabIndex = 2;
            this.TabMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
                this.PageMain,
                this.PageMoreHeader,
                this.PageNote,
                this.PageRemarks,
                this.PageServiceNoteHistory,
                this.PageDebtorsOwnershipHistory,
                this.PageMeterType});

            this.PageMain.Controls.Add(this.GrpPM);
            this.PageMain.Name = "PageMain";
            this.PageMain.Size = new System.Drawing.Size(1174, 312);
            this.PageMain.Text = "1.Main";

            this.PageMoreHeader.Name = "PageMoreHeader";
            this.PageMoreHeader.Size = new System.Drawing.Size(1174, 312);
            this.PageMoreHeader.Text = "2.More Header";

            this.PageNote.Name = "PageNote";
            this.PageNote.Size = new System.Drawing.Size(1174, 312);
            this.PageNote.Text = "3.Note";

            this.PageRemarks.Name = "PageRemarks";
            this.PageRemarks.Size = new System.Drawing.Size(1174, 312);
            this.PageRemarks.Text = "4.Remarks";

            this.PageServiceNoteHistory.Name = "PageServiceNoteHistory";
            this.PageServiceNoteHistory.Size = new System.Drawing.Size(1174, 312);
            this.PageServiceNoteHistory.Text = "5.Service Note History";

            this.PageDebtorsOwnershipHistory.Name = "PageDebtorsOwnershipHistory";
            this.PageDebtorsOwnershipHistory.Size = new System.Drawing.Size(1174, 312);
            this.PageDebtorsOwnershipHistory.Text = "6.Debtors Ownership History";

            this.PageMeterType.Name = "PageMeterType";
            this.PageMeterType.Size = new System.Drawing.Size(1174, 312);
            this.PageMeterType.Text = "7.Meter Type";

            // GrpPM (Preventive Maintenance group, on PageMain)
            this.GrpPM.Controls.Add(this.LytPM);
            this.GrpPM.Location = new System.Drawing.Point(8, 8);
            this.GrpPM.Name = "GrpPM";
            this.GrpPM.Size = new System.Drawing.Size(720, 296);
            this.GrpPM.TabIndex = 0;
            this.GrpPM.Text = "Preventive Maintenance";

            // LytPM (nested LayoutControl for the PM group)
            this.LytPM.Controls.Add(this.ChkPMActive);
            this.LytPM.Controls.Add(this.CmbPMIntervalType);
            this.LytPM.Controls.Add(this.SpnPMIntervalValue);
            this.LytPM.Controls.Add(this.DtPMStartDate);
            this.LytPM.Controls.Add(this.DtPMLastServiceDate);
            this.LytPM.Controls.Add(this.DtPMNextServiceDate);
            this.LytPM.Controls.Add(this.CmbDepartment);
            this.LytPM.Controls.Add(this.CmbJob);
            this.LytPM.Controls.Add(this.CmbLocation);
            this.LytPM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LytPM.Location = new System.Drawing.Point(2, 22);
            this.LytPM.Name = "LytPM";
            this.LytPM.Root = this.LytPMRoot;
            this.LytPM.Size = new System.Drawing.Size(716, 272);
            this.LytPM.TabIndex = 0;

            this.ChkPMActive.Name = "ChkPMActive";
            this.ChkPMActive.Properties.Caption = "Active Preventive Maintenance";
            this.ChkPMActive.Size = new System.Drawing.Size(220, 19);
            this.ChkPMActive.StyleController = this.LytPM;
            this.ChkPMActive.TabIndex = 0;

            this.CmbPMIntervalType.Name = "CmbPMIntervalType";
            this.CmbPMIntervalType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbPMIntervalType.Size = new System.Drawing.Size(180, 22);
            this.CmbPMIntervalType.StyleController = this.LytPM;
            this.CmbPMIntervalType.TabIndex = 1;

            this.SpnPMIntervalValue.Name = "SpnPMIntervalValue";
            this.SpnPMIntervalValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpnPMIntervalValue.Size = new System.Drawing.Size(180, 22);
            this.SpnPMIntervalValue.StyleController = this.LytPM;
            this.SpnPMIntervalValue.TabIndex = 2;

            this.DtPMStartDate.EditValue = null;
            this.DtPMStartDate.Name = "DtPMStartDate";
            this.DtPMStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtPMStartDate.Size = new System.Drawing.Size(180, 22);
            this.DtPMStartDate.StyleController = this.LytPM;
            this.DtPMStartDate.TabIndex = 3;

            this.DtPMLastServiceDate.EditValue = null;
            this.DtPMLastServiceDate.Name = "DtPMLastServiceDate";
            this.DtPMLastServiceDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtPMLastServiceDate.Size = new System.Drawing.Size(180, 22);
            this.DtPMLastServiceDate.StyleController = this.LytPM;
            this.DtPMLastServiceDate.TabIndex = 4;

            this.DtPMNextServiceDate.EditValue = null;
            this.DtPMNextServiceDate.Name = "DtPMNextServiceDate";
            this.DtPMNextServiceDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DtPMNextServiceDate.Size = new System.Drawing.Size(180, 22);
            this.DtPMNextServiceDate.StyleController = this.LytPM;
            this.DtPMNextServiceDate.TabIndex = 5;

            this.CmbDepartment.Name = "CmbDepartment";
            this.CmbDepartment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbDepartment.Size = new System.Drawing.Size(220, 22);
            this.CmbDepartment.StyleController = this.LytPM;
            this.CmbDepartment.TabIndex = 6;

            this.CmbJob.Name = "CmbJob";
            this.CmbJob.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbJob.Size = new System.Drawing.Size(220, 22);
            this.CmbJob.StyleController = this.LytPM;
            this.CmbJob.TabIndex = 7;

            this.CmbLocation.Name = "CmbLocation";
            this.CmbLocation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbLocation.Size = new System.Drawing.Size(220, 22);
            this.CmbLocation.StyleController = this.LytPM;
            this.CmbLocation.TabIndex = 8;

            // LytPMRoot
            this.LytPMRoot.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.LytPMRoot.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
                this.LytItemPMActive,
                this.LytItemPMIntervalType,
                this.LytItemPMIntervalValue,
                this.LytItemPMStartDate,
                this.LytItemPMLastServiceDate,
                this.LytItemPMNextServiceDate,
                this.LytItemDepartment,
                this.LytItemJob,
                this.LytItemLocation});
            this.LytPMRoot.Name = "LytPMRoot";
            this.LytPMRoot.Size = new System.Drawing.Size(716, 272);
            this.LytPMRoot.TextVisible = false;

            this.LytItemPMActive.Control = this.ChkPMActive;
            this.LytItemPMActive.Location = new System.Drawing.Point(0, 0);
            this.LytItemPMActive.Name = "LytItemPMActive";
            this.LytItemPMActive.Size = new System.Drawing.Size(360, 28);
            this.LytItemPMActive.TextSize = new System.Drawing.Size(0, 0);
            this.LytItemPMActive.TextVisible = false;

            this.LytItemPMIntervalType.Control = this.CmbPMIntervalType;
            this.LytItemPMIntervalType.Location = new System.Drawing.Point(0, 28);
            this.LytItemPMIntervalType.Name = "LytItemPMIntervalType";
            this.LytItemPMIntervalType.Size = new System.Drawing.Size(360, 28);
            this.LytItemPMIntervalType.Text = "Interval Type";
            this.LytItemPMIntervalType.TextSize = new System.Drawing.Size(110, 13);

            this.LytItemPMIntervalValue.Control = this.SpnPMIntervalValue;
            this.LytItemPMIntervalValue.Location = new System.Drawing.Point(0, 56);
            this.LytItemPMIntervalValue.Name = "LytItemPMIntervalValue";
            this.LytItemPMIntervalValue.Size = new System.Drawing.Size(360, 28);
            this.LytItemPMIntervalValue.Text = "Interval Value";
            this.LytItemPMIntervalValue.TextSize = new System.Drawing.Size(110, 13);

            this.LytItemPMStartDate.Control = this.DtPMStartDate;
            this.LytItemPMStartDate.Location = new System.Drawing.Point(0, 84);
            this.LytItemPMStartDate.Name = "LytItemPMStartDate";
            this.LytItemPMStartDate.Size = new System.Drawing.Size(360, 28);
            this.LytItemPMStartDate.Text = "Start Date";
            this.LytItemPMStartDate.TextSize = new System.Drawing.Size(110, 13);

            this.LytItemPMLastServiceDate.Control = this.DtPMLastServiceDate;
            this.LytItemPMLastServiceDate.Location = new System.Drawing.Point(0, 112);
            this.LytItemPMLastServiceDate.Name = "LytItemPMLastServiceDate";
            this.LytItemPMLastServiceDate.Size = new System.Drawing.Size(360, 28);
            this.LytItemPMLastServiceDate.Text = "Last Service Date";
            this.LytItemPMLastServiceDate.TextSize = new System.Drawing.Size(110, 13);

            this.LytItemPMNextServiceDate.Control = this.DtPMNextServiceDate;
            this.LytItemPMNextServiceDate.Location = new System.Drawing.Point(0, 140);
            this.LytItemPMNextServiceDate.Name = "LytItemPMNextServiceDate";
            this.LytItemPMNextServiceDate.Size = new System.Drawing.Size(360, 28);
            this.LytItemPMNextServiceDate.Text = "Next Service Date";
            this.LytItemPMNextServiceDate.TextSize = new System.Drawing.Size(110, 13);

            this.LytItemDepartment.Control = this.CmbDepartment;
            this.LytItemDepartment.Location = new System.Drawing.Point(360, 0);
            this.LytItemDepartment.Name = "LytItemDepartment";
            this.LytItemDepartment.Size = new System.Drawing.Size(356, 28);
            this.LytItemDepartment.Text = "Department :";
            this.LytItemDepartment.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemJob.Control = this.CmbJob;
            this.LytItemJob.Location = new System.Drawing.Point(360, 28);
            this.LytItemJob.Name = "LytItemJob";
            this.LytItemJob.Size = new System.Drawing.Size(356, 28);
            this.LytItemJob.Text = "Job :";
            this.LytItemJob.TextSize = new System.Drawing.Size(95, 13);

            this.LytItemLocation.Control = this.CmbLocation;
            this.LytItemLocation.Location = new System.Drawing.Point(360, 56);
            this.LytItemLocation.Name = "LytItemLocation";
            this.LytItemLocation.Size = new System.Drawing.Size(356, 28);
            this.LytItemLocation.Text = "Location :";
            this.LytItemLocation.TextSize = new System.Drawing.Size(95, 13);

            // ServiceItemLayout_Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 700);
            this.Controls.Add(this.TabMain);
            this.Controls.Add(this.LytHeader);
            this.Controls.Add(this.PanelTop);
            this.Name = "ServiceItemLayout_Form";
            this.Text = "Maintain Service Item (Layout)";

            ((System.ComponentModel.ISupportInitialize)(this.LytItemLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemJob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMNextServiceDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMLastServiceDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMIntervalValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMIntervalType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPMActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbLocation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbJob.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMNextServiceDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMNextServiceDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMLastServiceDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMLastServiceDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPMStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpnPMIntervalValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbPMIntervalType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkPMActive.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytPMRoot)).EndInit();
            this.LytPM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LytPM)).EndInit();
            this.GrpPM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpPM)).EndInit();
            this.PageMain.ResumeLayout(false);
            this.TabMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAttention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemResetDebtorOwnership)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemServiceEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemServiceStartDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemResetContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemContractNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemUnitPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemGradeCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemRefNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemTerm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAgentCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemDebtorCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemStockCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemAutoTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemServiceTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemInactive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytItemPurchaseDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAttention.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtServiceStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbContractNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpnUnitPrice.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbGradeCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtRefNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbTerm.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbAgentCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDebtorCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbStockCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtServiceTag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkInactive.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPurchaseDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DtPurchaseDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LytHeaderRoot)).EndInit();
            this.LytHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LytHeader)).EndInit();
            this.PanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelTop)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}
