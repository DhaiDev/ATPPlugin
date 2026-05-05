using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.ServiceNote.OperationForms
{
    partial class ServiceNoteQuickEntry_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblDate, LblNoteCode, LblDebtor, LblItem, LblStatus, LblSeverity, LblProblem, LblAssignTo, LblDesc;
        private DateEdit DtNoteDate;
        private TextEdit TxtNoteCode, TxtDebtorCode, TxtServiceItemCode;
        private MemoEdit TxtDescription;
        private ComboBoxEdit CmbStatus, CmbSeverity, CmbProblem, CmbAssignTo;
        private SimpleButton BtnSaveNew, BtnSaveClose, BtnCancel;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblDate = new LabelControl(); this.DtNoteDate = new DateEdit();
            this.LblNoteCode = new LabelControl(); this.TxtNoteCode = new TextEdit();
            this.LblDebtor = new LabelControl(); this.TxtDebtorCode = new TextEdit();
            this.LblItem = new LabelControl(); this.TxtServiceItemCode = new TextEdit();
            this.LblStatus = new LabelControl(); this.CmbStatus = new ComboBoxEdit();
            this.LblSeverity = new LabelControl(); this.CmbSeverity = new ComboBoxEdit();
            this.LblProblem = new LabelControl(); this.CmbProblem = new ComboBoxEdit();
            this.LblAssignTo = new LabelControl(); this.CmbAssignTo = new ComboBoxEdit();
            this.LblDesc = new LabelControl(); this.TxtDescription = new MemoEdit();
            this.BtnSaveNew = new SimpleButton(); this.BtnSaveClose = new SimpleButton(); this.BtnCancel = new SimpleButton();

            this.SuspendLayout();
            this.Text = "Service Note - Quick Entry";
            this.ClientSize = new Size(600, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;

            this.LblTitle.Text = "Service Note - Quick Entry";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            int lX = 14, eX = 150, eW = 420;
            int y = 50, gap = 30;
            Lbl(this.LblDate, "Date", lX, y);
            this.DtNoteDate.Location = new Point(eX, y); this.DtNoteDate.Width = 200;
            y += gap;
            Lbl(this.LblNoteCode, "Service Note No", lX, y);
            this.TxtNoteCode.Location = new Point(eX, y); this.TxtNoteCode.Width = 280;
            y += gap;
            Lbl(this.LblDebtor, "Debtor Code", lX, y);
            this.TxtDebtorCode.Location = new Point(eX, y); this.TxtDebtorCode.Width = 280;
            y += gap;
            Lbl(this.LblItem, "Service Item Code", lX, y);
            this.TxtServiceItemCode.Location = new Point(eX, y); this.TxtServiceItemCode.Width = 280;
            y += gap;
            Lbl(this.LblStatus, "Status", lX, y);
            this.CmbStatus.Location = new Point(eX, y); this.CmbStatus.Width = 200;
            y += gap;
            Lbl(this.LblSeverity, "Severity", lX, y);
            this.CmbSeverity.Location = new Point(eX, y); this.CmbSeverity.Width = 200;
            y += gap;
            Lbl(this.LblProblem, "Problem", lX, y);
            this.CmbProblem.Location = new Point(eX, y); this.CmbProblem.Width = 200;
            y += gap;
            Lbl(this.LblAssignTo, "Assign To", lX, y);
            this.CmbAssignTo.Location = new Point(eX, y); this.CmbAssignTo.Width = 200;
            y += gap;
            Lbl(this.LblDesc, "Description", lX, y);
            this.TxtDescription.Location = new Point(eX, y); this.TxtDescription.Size = new Size(eW, 60);

            this.BtnSaveNew.Text = "Save && &New"; this.BtnSaveNew.Location = new Point(240, 435); this.BtnSaveNew.Width = 105; this.BtnSaveNew.Height = 30;
            this.BtnSaveNew.Click += new System.EventHandler(this.OnSaveAndNew);
            this.BtnSaveClose.Text = "&Save && Close"; this.BtnSaveClose.Location = new Point(355, 435); this.BtnSaveClose.Width = 115; this.BtnSaveClose.Height = 30;
            this.BtnSaveClose.Click += new System.EventHandler(this.OnSaveAndClose);
            this.BtnCancel.Text = "&Cancel"; this.BtnCancel.Location = new Point(480, 435); this.BtnCancel.Width = 95; this.BtnCancel.Height = 30;
            this.BtnCancel.Click += new System.EventHandler(this.OnCancel);

            Control[] cs = new Control[] {
                this.LblTitle, this.LblDate, this.DtNoteDate, this.LblNoteCode, this.TxtNoteCode,
                this.LblDebtor, this.TxtDebtorCode, this.LblItem, this.TxtServiceItemCode,
                this.LblStatus, this.CmbStatus, this.LblSeverity, this.CmbSeverity,
                this.LblProblem, this.CmbProblem, this.LblAssignTo, this.CmbAssignTo,
                this.LblDesc, this.TxtDescription,
                this.BtnSaveNew, this.BtnSaveClose, this.BtnCancel };
            foreach (var c in cs) this.Controls.Add(c);
            this.CancelButton = this.BtnCancel;
            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
    }
}
