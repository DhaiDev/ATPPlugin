using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.ServiceNote.OperationForms
{
    partial class ServiceNoteClosing_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblNoteCode, LblClosedDate, LblActualDate, LblSolution, LblRemark;
        private TextEdit TxtNoteCode;
        private DateEdit DtClosedDate, DtActualServiceDate;
        private ComboBoxEdit CmbSolution;
        private MemoEdit TxtSolutionRemark;
        private SimpleButton BtnClose, BtnCancel;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblNoteCode = new LabelControl(); this.TxtNoteCode = new TextEdit();
            this.LblClosedDate = new LabelControl(); this.DtClosedDate = new DateEdit();
            this.LblActualDate = new LabelControl(); this.DtActualServiceDate = new DateEdit();
            this.LblSolution = new LabelControl(); this.CmbSolution = new ComboBoxEdit();
            this.LblRemark = new LabelControl(); this.TxtSolutionRemark = new MemoEdit();
            this.BtnClose = new SimpleButton(); this.BtnCancel = new SimpleButton();

            this.SuspendLayout();
            this.Text = "Service Note Closing";
            this.ClientSize = new Size(560, 380);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;

            this.LblTitle.Text = "Service Note Closing";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            int lX = 14, eX = 150, eW = 380;
            int y = 50, gap = 32;
            Lbl(this.LblNoteCode, "Service Note No", lX, y);
            this.TxtNoteCode.Location = new Point(eX, y); this.TxtNoteCode.Width = 260;
            y += gap;
            Lbl(this.LblClosedDate, "Closed Date", lX, y);
            this.DtClosedDate.Location = new Point(eX, y); this.DtClosedDate.Width = 200;
            y += gap;
            Lbl(this.LblActualDate, "Actual Service Date", lX, y);
            this.DtActualServiceDate.Location = new Point(eX, y); this.DtActualServiceDate.Width = 200;
            y += gap;
            Lbl(this.LblSolution, "Solution Code", lX, y);
            this.CmbSolution.Location = new Point(eX, y); this.CmbSolution.Width = 260;
            y += gap;
            Lbl(this.LblRemark, "Solution Remark", lX, y);
            this.TxtSolutionRemark.Location = new Point(eX, y); this.TxtSolutionRemark.Size = new Size(eW, 90);

            this.BtnClose.Text = "&Close Note"; this.BtnClose.Location = new Point(340, 335); this.BtnClose.Width = 110; this.BtnClose.Height = 30;
            this.BtnClose.Click += new System.EventHandler(this.OnClose);
            this.BtnCancel.Text = "&Cancel"; this.BtnCancel.Location = new Point(455, 335); this.BtnCancel.Width = 95; this.BtnCancel.Height = 30;
            this.BtnCancel.Click += new System.EventHandler(this.OnCancel);

            Control[] cs = new Control[] {
                this.LblTitle, this.LblNoteCode, this.TxtNoteCode,
                this.LblClosedDate, this.DtClosedDate, this.LblActualDate, this.DtActualServiceDate,
                this.LblSolution, this.CmbSolution, this.LblRemark, this.TxtSolutionRemark,
                this.BtnClose, this.BtnCancel };
            foreach (var c in cs) this.Controls.Add(c);
            this.AcceptButton = this.BtnClose;
            this.CancelButton = this.BtnCancel;
            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
    }
}
