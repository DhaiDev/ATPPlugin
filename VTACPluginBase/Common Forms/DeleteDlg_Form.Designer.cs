namespace VTACPluginBase.CommonForms
{
    partial class DeleteDlg_Form
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
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.DeleteMsg_Lbl = new DevExpress.XtraEditors.LabelControl();
            this.Yes_SBtn = new DevExpress.XtraEditors.SimpleButton();
            this.No_SBtn = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox1
            // 
            this.PictureBox1.Location = new System.Drawing.Point(14, 10);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(48, 48);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureBox1.TabIndex = 111;
            this.PictureBox1.TabStop = false;
            // 
            // DeleteMsg_Lbl
            // 
            this.DeleteMsg_Lbl.Appearance.Options.UseTextOptions = true;
            this.DeleteMsg_Lbl.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.DeleteMsg_Lbl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.DeleteMsg_Lbl.Location = new System.Drawing.Point(68, 10);
            this.DeleteMsg_Lbl.Name = "DeleteMsg_Lbl";
            this.DeleteMsg_Lbl.Size = new System.Drawing.Size(293, 48);
            this.DeleteMsg_Lbl.TabIndex = 112;
            this.DeleteMsg_Lbl.Text = "Do you really want to delete ";
            // 
            // Yes_SBtn
            // 
            this.Yes_SBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Yes_SBtn.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Yes_SBtn.Location = new System.Drawing.Point(105, 70);
            this.Yes_SBtn.Name = "Yes_SBtn";
            this.Yes_SBtn.Size = new System.Drawing.Size(75, 23);
            this.Yes_SBtn.TabIndex = 113;
            this.Yes_SBtn.Text = "&Yes";
            this.Yes_SBtn.Click += new System.EventHandler(this.Yes_SBtn_Click);
            // 
            // No_SBtn
            // 
            this.No_SBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.No_SBtn.DialogResult = System.Windows.Forms.DialogResult.No;
            this.No_SBtn.Location = new System.Drawing.Point(186, 70);
            this.No_SBtn.Name = "No_SBtn";
            this.No_SBtn.Size = new System.Drawing.Size(75, 23);
            this.No_SBtn.TabIndex = 114;
            this.No_SBtn.Text = "&No";
            this.No_SBtn.Click += new System.EventHandler(this.No_SBtn_Click);
            // 
            // DeleteDlg_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 105);
            this.Controls.Add(this.Yes_SBtn);
            this.Controls.Add(this.No_SBtn);
            this.Controls.Add(this.DeleteMsg_Lbl);
            this.Controls.Add(this.PictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteDlg_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Coil Work Order PlugIns";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeleteDlg_Form_FormClosing);
            this.Load += new System.EventHandler(this.DeleteDlg_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PictureBox PictureBox1;
        internal DevExpress.XtraEditors.LabelControl DeleteMsg_Lbl;
        internal DevExpress.XtraEditors.SimpleButton Yes_SBtn;
        internal DevExpress.XtraEditors.SimpleButton No_SBtn;
    }
}