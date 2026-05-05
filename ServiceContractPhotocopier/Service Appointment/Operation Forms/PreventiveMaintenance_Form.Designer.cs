using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    partial class PreventiveMaintenance_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private LabelControl LblFrom, LblTo, LblStatus, LblType, LblSeverity, LblProblem, LblProblemRemark, LblAttendedBy, LblAssignTo, LblDesc;
        private DateEdit DtFrom, DtTo;
        private CheckEdit ChkTaxInclusive;
        private ComboBoxEdit CmbStatus, CmbType, CmbSeverity, CmbProblem, CmbAttendedBy, CmbAssignTo;
        private MemoEdit TxtProblemRemark;
        private TextEdit TxtDescription;
        private GroupControl GrpDefault;
        private SimpleButton BtnGenerate, BtnExit;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblFrom = new LabelControl(); this.DtFrom = new DateEdit();
            this.LblTo = new LabelControl(); this.DtTo = new DateEdit();
            this.ChkTaxInclusive = new CheckEdit();
            this.GrpDefault = new GroupControl();
            this.LblStatus = new LabelControl(); this.CmbStatus = new ComboBoxEdit();
            this.LblType = new LabelControl(); this.CmbType = new ComboBoxEdit();
            this.LblSeverity = new LabelControl(); this.CmbSeverity = new ComboBoxEdit();
            this.LblProblem = new LabelControl(); this.CmbProblem = new ComboBoxEdit();
            this.LblProblemRemark = new LabelControl(); this.TxtProblemRemark = new MemoEdit();
            this.LblAttendedBy = new LabelControl(); this.CmbAttendedBy = new ComboBoxEdit();
            this.LblAssignTo = new LabelControl(); this.CmbAssignTo = new ComboBoxEdit();
            this.LblDesc = new LabelControl(); this.TxtDescription = new TextEdit();
            this.BtnGenerate = new SimpleButton(); this.BtnExit = new SimpleButton();

            this.SuspendLayout();

            this.Text = "Preventive Maintenance - Service Note";
            this.ClientSize = new Size(620, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;

            this.LblTitle.Text = "Preventive Maintenance - Service Note";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            Lbl(this.LblFrom, "Service Date From", 14, 44);
            this.DtFrom.Location = new Point(140, 41); this.DtFrom.Width = 150;
            Lbl(this.LblTo, "To", 300, 44);
            this.DtTo.Location = new Point(330, 41); this.DtTo.Width = 150;
            this.ChkTaxInclusive.Properties.Caption = "Tax Inclusive";
            this.ChkTaxInclusive.Location = new Point(500, 41);

            this.GrpDefault.Text = "Default Settings";
            this.GrpDefault.Location = new Point(14, 80);
            this.GrpDefault.Size = new Size(590, 380);

            int lX = 14, eX = 150, eW = 260, gap = 30;
            int y = 30;
            Lbl(this.LblStatus, "Status Code", lX, y);
            this.CmbStatus.Location = new Point(eX, y); this.CmbStatus.Width = eW;
            y += gap;
            Lbl(this.LblType, "Type Code", lX, y);
            this.CmbType.Location = new Point(eX, y); this.CmbType.Width = eW;
            y += gap;
            Lbl(this.LblSeverity, "Severity Code", lX, y);
            this.CmbSeverity.Location = new Point(eX, y); this.CmbSeverity.Width = eW;
            y += gap;
            Lbl(this.LblProblem, "Problem Code", lX, y);
            this.CmbProblem.Location = new Point(eX, y); this.CmbProblem.Width = eW;
            y += gap;
            Lbl(this.LblProblemRemark, "Problem Remark", lX, y);
            this.TxtProblemRemark.Location = new Point(eX, y); this.TxtProblemRemark.Size = new Size(420, 60);
            y += 68;
            Lbl(this.LblAttendedBy, "Attended By", lX, y);
            this.CmbAttendedBy.Location = new Point(eX, y); this.CmbAttendedBy.Width = eW;
            y += gap;
            Lbl(this.LblAssignTo, "Assign To", lX, y);
            this.CmbAssignTo.Location = new Point(eX, y); this.CmbAssignTo.Width = eW;
            y += gap;
            Lbl(this.LblDesc, "Description", lX, y);
            this.TxtDescription.Location = new Point(eX, y); this.TxtDescription.Width = 420;

            this.GrpDefault.Controls.Add(this.LblStatus); this.GrpDefault.Controls.Add(this.CmbStatus);
            this.GrpDefault.Controls.Add(this.LblType); this.GrpDefault.Controls.Add(this.CmbType);
            this.GrpDefault.Controls.Add(this.LblSeverity); this.GrpDefault.Controls.Add(this.CmbSeverity);
            this.GrpDefault.Controls.Add(this.LblProblem); this.GrpDefault.Controls.Add(this.CmbProblem);
            this.GrpDefault.Controls.Add(this.LblProblemRemark); this.GrpDefault.Controls.Add(this.TxtProblemRemark);
            this.GrpDefault.Controls.Add(this.LblAttendedBy); this.GrpDefault.Controls.Add(this.CmbAttendedBy);
            this.GrpDefault.Controls.Add(this.LblAssignTo); this.GrpDefault.Controls.Add(this.CmbAssignTo);
            this.GrpDefault.Controls.Add(this.LblDesc); this.GrpDefault.Controls.Add(this.TxtDescription);

            this.BtnGenerate.Text = "Generate Service Note";
            this.BtnGenerate.Location = new Point(320, 475); this.BtnGenerate.Width = 180; this.BtnGenerate.Height = 30;
            this.BtnGenerate.Click += new System.EventHandler(this.OnGenerate);
            this.BtnExit.Text = "Exit (F2)";
            this.BtnExit.Location = new Point(510, 475); this.BtnExit.Width = 95; this.BtnExit.Height = 30;
            this.BtnExit.Click += new System.EventHandler(this.OnExit);

            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.LblFrom); this.Controls.Add(this.DtFrom);
            this.Controls.Add(this.LblTo); this.Controls.Add(this.DtTo);
            this.Controls.Add(this.ChkTaxInclusive);
            this.Controls.Add(this.GrpDefault);
            this.Controls.Add(this.BtnGenerate); this.Controls.Add(this.BtnExit);
            this.AcceptButton = this.BtnGenerate;
            this.CancelButton = this.BtnExit;

            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
    }
}
