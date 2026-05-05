using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    partial class Appointment_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private LabelControl LblSubject, LblStart, LblFinish, LblType, LblPriority, LblServicePerson, LblDebtor, LblContact, LblStaff, LblArea, LblDesc, LblMsg, LblAddress;
        private TextEdit TxtSubject, TxtDebtorCode, TxtContactPerson, TxtStaffCode, TxtAreaCode, TxtDescription;
        private DateEdit DtStart, DtFinish;
        private ComboBoxEdit CmbType, CmbPriority, CmbServicePerson;
        private MemoEdit TxtMessage, TxtAddress;
        private CheckEdit ChkDone;
        private SimpleButton BtnSave, BtnCancel;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblSubject = new LabelControl(); this.TxtSubject = new TextEdit();
            this.LblStart = new LabelControl(); this.DtStart = new DateEdit();
            this.LblFinish = new LabelControl(); this.DtFinish = new DateEdit();
            this.LblType = new LabelControl(); this.CmbType = new ComboBoxEdit();
            this.LblPriority = new LabelControl(); this.CmbPriority = new ComboBoxEdit();
            this.LblServicePerson = new LabelControl(); this.CmbServicePerson = new ComboBoxEdit();
            this.LblDebtor = new LabelControl(); this.TxtDebtorCode = new TextEdit();
            this.LblContact = new LabelControl(); this.TxtContactPerson = new TextEdit();
            this.LblStaff = new LabelControl(); this.TxtStaffCode = new TextEdit();
            this.LblArea = new LabelControl(); this.TxtAreaCode = new TextEdit();
            this.LblDesc = new LabelControl(); this.TxtDescription = new TextEdit();
            this.LblMsg = new LabelControl(); this.TxtMessage = new MemoEdit();
            this.LblAddress = new LabelControl(); this.TxtAddress = new MemoEdit();
            this.ChkDone = new CheckEdit();
            this.BtnSave = new SimpleButton(); this.BtnCancel = new SimpleButton();

            this.SuspendLayout();

            this.Text = "Appointment";
            this.ClientSize = new Size(700, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;

            this.LblTitle.Text = "Appointment";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            int lX = 14, eX = 140, eW = 280;
            int y = 50, gap = 30;
            Lbl(this.LblSubject, "Subject", lX, y);
            this.TxtSubject.Location = new Point(eX, y); this.TxtSubject.Width = eW + 200;
            y += gap;
            Lbl(this.LblStart, "Start", lX, y);
            this.DtStart.Location = new Point(eX, y); this.DtStart.Width = 200;
            Lbl(this.LblFinish, "Finish", 370, y);
            this.DtFinish.Location = new Point(440, y); this.DtFinish.Width = 200;
            y += gap;
            Lbl(this.LblType, "Type", lX, y);
            this.CmbType.Location = new Point(eX, y); this.CmbType.Width = 200;
            Lbl(this.LblPriority, "Priority", 370, y);
            this.CmbPriority.Location = new Point(440, y); this.CmbPriority.Width = 200;
            y += gap;
            Lbl(this.LblServicePerson, "Service Person", lX, y);
            this.CmbServicePerson.Location = new Point(eX, y); this.CmbServicePerson.Width = 200;
            this.ChkDone.Properties.Caption = "Done";
            this.ChkDone.Location = new Point(440, y);
            y += gap;
            Lbl(this.LblDebtor, "Debtor Code", lX, y);
            this.TxtDebtorCode.Location = new Point(eX, y); this.TxtDebtorCode.Width = eW;
            y += gap;
            Lbl(this.LblContact, "Contact Person", lX, y);
            this.TxtContactPerson.Location = new Point(eX, y); this.TxtContactPerson.Width = eW;
            y += gap;
            Lbl(this.LblStaff, "Staff Code", lX, y);
            this.TxtStaffCode.Location = new Point(eX, y); this.TxtStaffCode.Width = eW;
            y += gap;
            Lbl(this.LblArea, "Area Code", lX, y);
            this.TxtAreaCode.Location = new Point(eX, y); this.TxtAreaCode.Width = eW;
            y += gap;
            Lbl(this.LblDesc, "Description", lX, y);
            this.TxtDescription.Location = new Point(eX, y); this.TxtDescription.Width = eW + 200;
            y += gap;
            Lbl(this.LblAddress, "Address", lX, y);
            this.TxtAddress.Location = new Point(eX, y); this.TxtAddress.Size = new Size(eW + 200, 55);
            y += 60;
            Lbl(this.LblMsg, "Message", lX, y);
            this.TxtMessage.Location = new Point(eX, y); this.TxtMessage.Size = new Size(eW + 200, 55);

            this.BtnSave.Text = "&Save"; this.BtnSave.Location = new Point(500, 515); this.BtnSave.Width = 85; this.BtnSave.Height = 30;
            this.BtnSave.Click += new System.EventHandler(this.OnSave);
            this.BtnCancel.Text = "&Cancel"; this.BtnCancel.Location = new Point(595, 515); this.BtnCancel.Width = 85; this.BtnCancel.Height = 30;
            this.BtnCancel.Click += new System.EventHandler(this.OnCancel);

            Control[] ctrls = new Control[] {
                this.LblTitle,
                this.LblSubject, this.TxtSubject, this.LblStart, this.DtStart, this.LblFinish, this.DtFinish,
                this.LblType, this.CmbType, this.LblPriority, this.CmbPriority,
                this.LblServicePerson, this.CmbServicePerson, this.ChkDone,
                this.LblDebtor, this.TxtDebtorCode, this.LblContact, this.TxtContactPerson,
                this.LblStaff, this.TxtStaffCode, this.LblArea, this.TxtAreaCode,
                this.LblDesc, this.TxtDescription, this.LblAddress, this.TxtAddress,
                this.LblMsg, this.TxtMessage, this.BtnSave, this.BtnCancel
            };
            foreach (var c in ctrls) this.Controls.Add(c);
            this.AcceptButton = this.BtnSave;
            this.CancelButton = this.BtnCancel;

            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
    }
}
