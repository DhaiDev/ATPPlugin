namespace VTACPluginBase.CommonForms
{
    partial class FilterImport_Form
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
            this.FilterForm_Pnl = new DevExpress.XtraEditors.PanelControl();
            this.Cancel_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.OK_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.FilterForm_DXTbCtrl = new DevExpress.XtraTab.XtraTabControl();
            this.FilterFormFullTfr_DXTbPg = new DevExpress.XtraTab.XtraTabPage();
            this.FilterItemFullTfr_DXGrid = new DevExpress.XtraGrid.GridControl();
            this.FilterItemFullTfr_DXGridVw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Selected_RChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.UncheckAllFullTfr_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.CheckAllFullTfr_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.FilterFormPartialTfr_DXTbPg = new DevExpress.XtraTab.XtraTabPage();
            this.FilterItemPartialTfr_DXGrid = new DevExpress.XtraGrid.GridControl();
            this.FilterItemPartialTfr_DXGridVw = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.UncheckAllPartialTfr_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.CheckAllPartialTfr_SBtn = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.FilterForm_Pnl)).BeginInit();
            this.FilterForm_Pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterForm_DXTbCtrl)).BeginInit();
            this.FilterForm_DXTbCtrl.SuspendLayout();
            this.FilterFormFullTfr_DXTbPg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemFullTfr_DXGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemFullTfr_DXGridVw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Selected_RChkBox)).BeginInit();
            this.FilterFormPartialTfr_DXTbPg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemPartialTfr_DXGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemPartialTfr_DXGridVw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // FilterForm_Pnl
            // 
            this.FilterForm_Pnl.Controls.Add(this.Cancel_SBtn);
            this.FilterForm_Pnl.Controls.Add(this.OK_SBtn);
            this.FilterForm_Pnl.Controls.Add(this.FilterForm_DXTbCtrl);
            this.FilterForm_Pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterForm_Pnl.Location = new System.Drawing.Point(0, 0);
            this.FilterForm_Pnl.Name = "FilterForm_Pnl";
            this.FilterForm_Pnl.Size = new System.Drawing.Size(904, 461);
            this.FilterForm_Pnl.TabIndex = 1;
            // 
            // Cancel_SBtn
            // 
            this.Cancel_SBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_SBtn.Location = new System.Drawing.Point(817, 421);
            this.Cancel_SBtn.Name = "Cancel_SBtn";
            this.Cancel_SBtn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_SBtn.TabIndex = 2;
            this.Cancel_SBtn.Text = "Cancel";
            this.Cancel_SBtn.Click += new System.EventHandler(this.Cancel_SBtn_Click);
            // 
            // OK_SBtn
            // 
            this.OK_SBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_SBtn.Location = new System.Drawing.Point(736, 421);
            this.OK_SBtn.Name = "OK_SBtn";
            this.OK_SBtn.Size = new System.Drawing.Size(75, 23);
            this.OK_SBtn.TabIndex = 1;
            this.OK_SBtn.Text = "OK";
            this.OK_SBtn.Click += new System.EventHandler(this.OK_SBtn_Click);
            // 
            // FilterForm_DXTbCtrl
            // 
            this.FilterForm_DXTbCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterForm_DXTbCtrl.Location = new System.Drawing.Point(10, 12);
            this.FilterForm_DXTbCtrl.Name = "FilterForm_DXTbCtrl";
            this.FilterForm_DXTbCtrl.SelectedTabPage = this.FilterFormFullTfr_DXTbPg;
            this.FilterForm_DXTbCtrl.Size = new System.Drawing.Size(882, 396);
            this.FilterForm_DXTbCtrl.TabIndex = 0;
            this.FilterForm_DXTbCtrl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.FilterFormFullTfr_DXTbPg,
            this.FilterFormPartialTfr_DXTbPg});
            // 
            // FilterFormFullTfr_DXTbPg
            // 
            this.FilterFormFullTfr_DXTbPg.Controls.Add(this.FilterItemFullTfr_DXGrid);
            this.FilterFormFullTfr_DXTbPg.Controls.Add(this.UncheckAllFullTfr_SBtn);
            this.FilterFormFullTfr_DXTbPg.Controls.Add(this.CheckAllFullTfr_SBtn);
            this.FilterFormFullTfr_DXTbPg.Name = "FilterFormFullTfr_DXTbPg";
            this.FilterFormFullTfr_DXTbPg.Size = new System.Drawing.Size(880, 382);
            // 
            // FilterItemFullTfr_DXGrid
            // 
            this.FilterItemFullTfr_DXGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterItemFullTfr_DXGrid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.FilterItemFullTfr_DXGrid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.FilterItemFullTfr_DXGrid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.FilterItemFullTfr_DXGrid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.FilterItemFullTfr_DXGrid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.FilterItemFullTfr_DXGrid.Location = new System.Drawing.Point(3, 32);
            this.FilterItemFullTfr_DXGrid.MainView = this.FilterItemFullTfr_DXGridVw;
            this.FilterItemFullTfr_DXGrid.Name = "FilterItemFullTfr_DXGrid";
            this.FilterItemFullTfr_DXGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Selected_RChkBox});
            this.FilterItemFullTfr_DXGrid.Size = new System.Drawing.Size(867, 334);
            this.FilterItemFullTfr_DXGrid.TabIndex = 2;
            this.FilterItemFullTfr_DXGrid.UseEmbeddedNavigator = true;
            this.FilterItemFullTfr_DXGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.FilterItemFullTfr_DXGridVw});
            // 
            // FilterItemFullTfr_DXGridVw
            // 
            this.FilterItemFullTfr_DXGridVw.GridControl = this.FilterItemFullTfr_DXGrid;
            this.FilterItemFullTfr_DXGridVw.Name = "FilterItemFullTfr_DXGridVw";
            this.FilterItemFullTfr_DXGridVw.OptionsView.ShowAutoFilterRow = true;
            this.FilterItemFullTfr_DXGridVw.OptionsView.ShowGroupPanel = false;
            this.FilterItemFullTfr_DXGridVw.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.FilterItemFullTfr_DXGridVw_CellValueChanging);
            // 
            // Selected_RChkBox
            // 
            this.Selected_RChkBox.AutoHeight = false;
            this.Selected_RChkBox.Caption = "Check";
            this.Selected_RChkBox.Name = "Selected_RChkBox";
            // 
            // UncheckAllFullTfr_SBtn
            // 
            this.UncheckAllFullTfr_SBtn.Location = new System.Drawing.Point(84, 3);
            this.UncheckAllFullTfr_SBtn.Name = "UncheckAllFullTfr_SBtn";
            this.UncheckAllFullTfr_SBtn.Size = new System.Drawing.Size(75, 23);
            this.UncheckAllFullTfr_SBtn.TabIndex = 1;
            this.UncheckAllFullTfr_SBtn.Text = "Uncheck All";
            this.UncheckAllFullTfr_SBtn.Click += new System.EventHandler(this.UnCheckAllFullTfr_SBtn_Click);
            // 
            // CheckAllFullTfr_SBtn
            // 
            this.CheckAllFullTfr_SBtn.Location = new System.Drawing.Point(3, 3);
            this.CheckAllFullTfr_SBtn.Name = "CheckAllFullTfr_SBtn";
            this.CheckAllFullTfr_SBtn.Size = new System.Drawing.Size(75, 23);
            this.CheckAllFullTfr_SBtn.TabIndex = 0;
            this.CheckAllFullTfr_SBtn.Text = "Check All";
            this.CheckAllFullTfr_SBtn.Click += new System.EventHandler(this.CheckAllFullTfr_SBtn_Click);
            // 
            // FilterFormPartialTfr_DXTbPg
            // 
            this.FilterFormPartialTfr_DXTbPg.Controls.Add(this.FilterItemPartialTfr_DXGrid);
            this.FilterFormPartialTfr_DXTbPg.Controls.Add(this.UncheckAllPartialTfr_SBtn);
            this.FilterFormPartialTfr_DXTbPg.Controls.Add(this.CheckAllPartialTfr_SBtn);
            this.FilterFormPartialTfr_DXTbPg.Name = "FilterFormPartialTfr_DXTbPg";
            this.FilterFormPartialTfr_DXTbPg.Size = new System.Drawing.Size(880, 382);
            // 
            // FilterItemPartialTfr_DXGrid
            // 
            this.FilterItemPartialTfr_DXGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterItemPartialTfr_DXGrid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.FilterItemPartialTfr_DXGrid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.FilterItemPartialTfr_DXGrid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.FilterItemPartialTfr_DXGrid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.FilterItemPartialTfr_DXGrid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.FilterItemPartialTfr_DXGrid.Location = new System.Drawing.Point(3, 32);
            this.FilterItemPartialTfr_DXGrid.MainView = this.FilterItemPartialTfr_DXGridVw;
            this.FilterItemPartialTfr_DXGrid.Name = "FilterItemPartialTfr_DXGrid";
            this.FilterItemPartialTfr_DXGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepositoryItemCheckEdit1});
            this.FilterItemPartialTfr_DXGrid.Size = new System.Drawing.Size(867, 334);
            this.FilterItemPartialTfr_DXGrid.TabIndex = 5;
            this.FilterItemPartialTfr_DXGrid.UseEmbeddedNavigator = true;
            this.FilterItemPartialTfr_DXGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.FilterItemPartialTfr_DXGridVw});
            // 
            // FilterItemPartialTfr_DXGridVw
            // 
            this.FilterItemPartialTfr_DXGridVw.GridControl = this.FilterItemPartialTfr_DXGrid;
            this.FilterItemPartialTfr_DXGridVw.Name = "FilterItemPartialTfr_DXGridVw";
            this.FilterItemPartialTfr_DXGridVw.OptionsView.ShowAutoFilterRow = true;
            this.FilterItemPartialTfr_DXGridVw.OptionsView.ShowGroupPanel = false;
            this.FilterItemPartialTfr_DXGridVw.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.FilterItemPartialTfr_DXGridVw_CellValueChanging);
            // 
            // RepositoryItemCheckEdit1
            // 
            this.RepositoryItemCheckEdit1.AutoHeight = false;
            this.RepositoryItemCheckEdit1.Caption = "Check";
            this.RepositoryItemCheckEdit1.Name = "RepositoryItemCheckEdit1";
            // 
            // UncheckAllPartialTfr_SBtn
            // 
            this.UncheckAllPartialTfr_SBtn.Location = new System.Drawing.Point(84, 3);
            this.UncheckAllPartialTfr_SBtn.Name = "UncheckAllPartialTfr_SBtn";
            this.UncheckAllPartialTfr_SBtn.Size = new System.Drawing.Size(75, 23);
            this.UncheckAllPartialTfr_SBtn.TabIndex = 4;
            this.UncheckAllPartialTfr_SBtn.Text = "Uncheck All";
            this.UncheckAllPartialTfr_SBtn.Click += new System.EventHandler(this.UnCheckAllPartialTfr_SBtn_Click);
            // 
            // CheckAllPartialTfr_SBtn
            // 
            this.CheckAllPartialTfr_SBtn.Location = new System.Drawing.Point(3, 3);
            this.CheckAllPartialTfr_SBtn.Name = "CheckAllPartialTfr_SBtn";
            this.CheckAllPartialTfr_SBtn.Size = new System.Drawing.Size(75, 23);
            this.CheckAllPartialTfr_SBtn.TabIndex = 3;
            this.CheckAllPartialTfr_SBtn.Text = "Check All";
            this.CheckAllPartialTfr_SBtn.Click += new System.EventHandler(this.CheckAllPartialTfr_SBtn_Click);
            // 
            // FilterImport_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 461);
            this.Controls.Add(this.FilterForm_Pnl);
            this.Name = "FilterImport_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FilterImport_Form_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.FilterForm_Pnl)).EndInit();
            this.FilterForm_Pnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilterForm_DXTbCtrl)).EndInit();
            this.FilterForm_DXTbCtrl.ResumeLayout(false);
            this.FilterFormFullTfr_DXTbPg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemFullTfr_DXGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemFullTfr_DXGridVw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Selected_RChkBox)).EndInit();
            this.FilterFormPartialTfr_DXTbPg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemPartialTfr_DXGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterItemPartialTfr_DXGridVw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.PanelControl FilterForm_Pnl;
        internal DevExpress.XtraEditors.SimpleButton Cancel_SBtn;
        internal DevExpress.XtraEditors.SimpleButton OK_SBtn;
        internal DevExpress.XtraTab.XtraTabControl FilterForm_DXTbCtrl;
        internal DevExpress.XtraTab.XtraTabPage FilterFormFullTfr_DXTbPg;
        internal DevExpress.XtraGrid.GridControl FilterItemFullTfr_DXGrid;
        internal DevExpress.XtraGrid.Views.Grid.GridView FilterItemFullTfr_DXGridVw;
        internal DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit Selected_RChkBox;
        internal DevExpress.XtraEditors.SimpleButton UncheckAllFullTfr_SBtn;
        internal DevExpress.XtraEditors.SimpleButton CheckAllFullTfr_SBtn;
        internal DevExpress.XtraTab.XtraTabPage FilterFormPartialTfr_DXTbPg;
        internal DevExpress.XtraGrid.GridControl FilterItemPartialTfr_DXGrid;
        internal DevExpress.XtraGrid.Views.Grid.GridView FilterItemPartialTfr_DXGridVw;
        internal DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit RepositoryItemCheckEdit1;
        internal DevExpress.XtraEditors.SimpleButton UncheckAllPartialTfr_SBtn;
        internal DevExpress.XtraEditors.SimpleButton CheckAllPartialTfr_SBtn;
    }
}