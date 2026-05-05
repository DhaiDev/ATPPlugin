namespace ServiceContractPhotocopier.ServiceContract.OperationForms
{
    partial class ServiceContractLst_Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private DevExpress.XtraEditors.LabelControl LblTitle;
        private DevExpress.XtraEditors.SimpleButton BtnNew;
        private DevExpress.XtraEditors.SimpleButton BtnEdit;
        private DevExpress.XtraEditors.SimpleButton BtnDelete;
        private DevExpress.XtraEditors.SimpleButton BtnRefresh;
        private DevExpress.XtraEditors.SimpleButton BtnExit;
        private DevExpress.XtraGrid.GridControl Grid;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraGrid.Columns.GridColumn ColContractCode;
        private DevExpress.XtraGrid.Columns.GridColumn ColContractType;
        private DevExpress.XtraGrid.Columns.GridColumn ColContractDate;
        private DevExpress.XtraGrid.Columns.GridColumn ColStartDate;
        private DevExpress.XtraGrid.Columns.GridColumn ColExpiryDate;
        private DevExpress.XtraGrid.Columns.GridColumn ColDebtorCode;
        private DevExpress.XtraGrid.Columns.GridColumn ColDebtorName;
        private DevExpress.XtraGrid.Columns.GridColumn ColAmount;
        private DevExpress.XtraGrid.Columns.GridColumn ColCurrency;
        private DevExpress.XtraGrid.Columns.GridColumn ColAgent;
        private DevExpress.XtraGrid.Columns.GridColumn ColInactive;

        private void InitializeComponent()
        {
            this.LblTitle = new DevExpress.XtraEditors.LabelControl();
            this.BtnNew = new DevExpress.XtraEditors.SimpleButton();
            this.BtnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.BtnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.BtnExit = new DevExpress.XtraEditors.SimpleButton();
            this.Grid = new DevExpress.XtraGrid.GridControl();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColContractCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColContractType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColContractDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColStartDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColExpiryDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColDebtorCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColDebtorName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColCurrency = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColAgent = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColInactive = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            this.SuspendLayout();
            //
            // LblTitle
            //
            this.LblTitle.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.LblTitle.Location = new System.Drawing.Point(14, 8);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(260, 29);
            this.LblTitle.TabIndex = 0;
            this.LblTitle.Text = "Maintain Service Contract";
            //
            // BtnNew
            //
            this.BtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNew.Location = new System.Drawing.Point(766, 14);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(80, 28);
            this.BtnNew.TabIndex = 1;
            this.BtnNew.Text = "+ New";
            this.BtnNew.Click += new System.EventHandler(this.OnNew);
            //
            // BtnEdit
            //
            this.BtnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnEdit.Location = new System.Drawing.Point(851, 14);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(80, 28);
            this.BtnEdit.TabIndex = 2;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.Click += new System.EventHandler(this.OnEdit);
            //
            // BtnDelete
            //
            this.BtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDelete.Location = new System.Drawing.Point(936, 14);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(80, 28);
            this.BtnDelete.TabIndex = 3;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.Click += new System.EventHandler(this.OnDelete);
            //
            // BtnRefresh
            //
            this.BtnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRefresh.Location = new System.Drawing.Point(1021, 14);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(80, 28);
            this.BtnRefresh.TabIndex = 4;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.Click += new System.EventHandler(this.OnRefresh);
            //
            // BtnExit
            //
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(1106, 14);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(80, 28);
            this.BtnExit.TabIndex = 5;
            this.BtnExit.Text = "Exit (F2)";
            this.BtnExit.Click += new System.EventHandler(this.OnExit);
            //
            // Grid
            //
            this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid.Location = new System.Drawing.Point(14, 50);
            this.Grid.MainView = this.GridView;
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(1172, 640);
            this.Grid.TabIndex = 6;
            this.Grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
                this.GridView});
            //
            // GridView
            //
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
                this.ColContractCode,
                this.ColContractType,
                this.ColContractDate,
                this.ColStartDate,
                this.ColExpiryDate,
                this.ColDebtorCode,
                this.ColDebtorName,
                this.ColAmount,
                this.ColCurrency,
                this.ColAgent,
                this.ColInactive});
            this.GridView.GridControl = this.Grid;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.Editable = false;
            this.GridView.OptionsSelection.EnableAppearanceFocusedRow = true;
            this.GridView.OptionsView.ShowGroupPanel = false;
            //
            // ColContractCode
            //
            this.ColContractCode.Caption = "Contract No.";
            this.ColContractCode.FieldName = "ServiceContractCode";
            this.ColContractCode.Name = "ColContractCode";
            this.ColContractCode.Visible = true;
            this.ColContractCode.VisibleIndex = 0;
            this.ColContractCode.Width = 130;
            //
            // ColContractType
            //
            this.ColContractType.Caption = "Contract Type";
            this.ColContractType.FieldName = "ContractTypeDescription";
            this.ColContractType.Name = "ColContractType";
            this.ColContractType.Visible = true;
            this.ColContractType.VisibleIndex = 1;
            this.ColContractType.Width = 160;
            //
            // ColContractDate
            //
            this.ColContractDate.Caption = "Contract Date";
            this.ColContractDate.FieldName = "ServiceContractDate";
            this.ColContractDate.Name = "ColContractDate";
            this.ColContractDate.Visible = true;
            this.ColContractDate.VisibleIndex = 2;
            this.ColContractDate.Width = 100;
            //
            // ColStartDate
            //
            this.ColStartDate.Caption = "Start Date";
            this.ColStartDate.FieldName = "ServiceStartDate";
            this.ColStartDate.Name = "ColStartDate";
            this.ColStartDate.Visible = true;
            this.ColStartDate.VisibleIndex = 3;
            this.ColStartDate.Width = 100;
            //
            // ColExpiryDate
            //
            this.ColExpiryDate.Caption = "Expiry Date";
            this.ColExpiryDate.FieldName = "ServiceExpiryDate";
            this.ColExpiryDate.Name = "ColExpiryDate";
            this.ColExpiryDate.Visible = true;
            this.ColExpiryDate.VisibleIndex = 4;
            this.ColExpiryDate.Width = 100;
            //
            // ColDebtorCode
            //
            this.ColDebtorCode.Caption = "Debtor Code";
            this.ColDebtorCode.FieldName = "DebtorCode";
            this.ColDebtorCode.Name = "ColDebtorCode";
            this.ColDebtorCode.Visible = true;
            this.ColDebtorCode.VisibleIndex = 5;
            this.ColDebtorCode.Width = 110;
            //
            // ColDebtorName
            //
            this.ColDebtorName.Caption = "Debtor Name";
            this.ColDebtorName.FieldName = "DebtorName";
            this.ColDebtorName.Name = "ColDebtorName";
            this.ColDebtorName.Visible = true;
            this.ColDebtorName.VisibleIndex = 6;
            this.ColDebtorName.Width = 220;
            //
            // ColAmount
            //
            this.ColAmount.Caption = "Amount";
            this.ColAmount.FieldName = "ServiceContractValue";
            this.ColAmount.Name = "ColAmount";
            this.ColAmount.Visible = true;
            this.ColAmount.VisibleIndex = 7;
            this.ColAmount.Width = 110;
            //
            // ColCurrency
            //
            this.ColCurrency.Caption = "Currency";
            this.ColCurrency.FieldName = "CurrencyCode";
            this.ColCurrency.Name = "ColCurrency";
            this.ColCurrency.Visible = true;
            this.ColCurrency.VisibleIndex = 8;
            this.ColCurrency.Width = 70;
            //
            // ColAgent
            //
            this.ColAgent.Caption = "Agent";
            this.ColAgent.FieldName = "StaffCode";
            this.ColAgent.Name = "ColAgent";
            this.ColAgent.Visible = true;
            this.ColAgent.VisibleIndex = 9;
            this.ColAgent.Width = 90;
            //
            // ColInactive
            //
            this.ColInactive.Caption = "Inactive";
            this.ColInactive.FieldName = "Inactive";
            this.ColInactive.Name = "ColInactive";
            this.ColInactive.Visible = true;
            this.ColInactive.VisibleIndex = 10;
            this.ColInactive.Width = 60;
            //
            // ServiceContractLst_Form
            //
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.BtnDelete);
            this.Controls.Add(this.BtnEdit);
            this.Controls.Add(this.BtnNew);
            this.Controls.Add(this.LblTitle);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "ServiceContractLst_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Maintain Service Contract";
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
