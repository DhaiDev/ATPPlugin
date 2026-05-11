namespace VTACPluginBase.CommonForms
{
    partial class EditFurtherDescription_Form
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
            this.PanelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.MemoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.Cancel_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.OK_SBtn = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.PanelControl1)).BeginInit();
            this.PanelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelControl1
            // 
            this.PanelControl1.Controls.Add(this.MemoEdit1);
            this.PanelControl1.Controls.Add(this.Cancel_SBtn);
            this.PanelControl1.Controls.Add(this.OK_SBtn);
            this.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelControl1.Location = new System.Drawing.Point(0, 0);
            this.PanelControl1.Name = "PanelControl1";
            this.PanelControl1.Size = new System.Drawing.Size(772, 520);
            this.PanelControl1.TabIndex = 22;
            // 
            // MemoEdit1
            // 
            this.MemoEdit1.Location = new System.Drawing.Point(5, 12);
            this.MemoEdit1.Name = "MemoEdit1";
            this.MemoEdit1.Size = new System.Drawing.Size(762, 467);
            this.MemoEdit1.TabIndex = 1;
            // 
            // Cancel_SBtn
            // 
            this.Cancel_SBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_SBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_SBtn.Location = new System.Drawing.Point(688, 485);
            this.Cancel_SBtn.Name = "Cancel_SBtn";
            this.Cancel_SBtn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_SBtn.TabIndex = 3;
            this.Cancel_SBtn.Text = "Cancel";
            // 
            // OK_SBtn
            // 
            this.OK_SBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_SBtn.Location = new System.Drawing.Point(607, 485);
            this.OK_SBtn.Name = "OK_SBtn";
            this.OK_SBtn.Size = new System.Drawing.Size(75, 23);
            this.OK_SBtn.TabIndex = 2;
            this.OK_SBtn.Text = "OK";
            // 
            // EditFurtherDescription_Form
            // 
            this.AcceptButton = this.OK_SBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_SBtn;
            this.ClientSize = new System.Drawing.Size(772, 520);
            this.Controls.Add(this.PanelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EditFurtherDescription_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Further Description";
            ((System.ComponentModel.ISupportInitialize)(this.PanelControl1)).EndInit();
            this.PanelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MemoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.PanelControl PanelControl1;
        internal DevExpress.XtraEditors.MemoEdit MemoEdit1;
        internal DevExpress.XtraEditors.SimpleButton Cancel_SBtn;
        internal DevExpress.XtraEditors.SimpleButton OK_SBtn;
    }
}