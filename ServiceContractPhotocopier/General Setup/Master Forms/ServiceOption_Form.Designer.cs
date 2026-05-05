using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    partial class ServiceOption_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private XtraTabControl TabMain;
        private XtraTabPage PageMain, PageNoteControl, PageDefaults;
        private CheckEdit ChkShowStockPicture, ChkUseAlternativeItem, ChkNegativeStockChecking;
        private CheckEdit ChkAutoGenSalesInvoice, ChkAutoCloseNote, ChkAllowEditClosed;
        private LabelControl LblDefaultStatus, LblDefaultPriority;
        private TextEdit TxtDefaultServiceStatus, TxtDefaultAppointmentPriority;
        private SimpleButton BtnSave, BtnCancel;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.TabMain = new XtraTabControl();
            this.PageMain = new XtraTabPage();
            this.PageNoteControl = new XtraTabPage();
            this.PageDefaults = new XtraTabPage();
            this.ChkShowStockPicture = new CheckEdit();
            this.ChkUseAlternativeItem = new CheckEdit();
            this.ChkNegativeStockChecking = new CheckEdit();
            this.ChkAutoGenSalesInvoice = new CheckEdit();
            this.ChkAutoCloseNote = new CheckEdit();
            this.ChkAllowEditClosed = new CheckEdit();
            this.LblDefaultStatus = new LabelControl(); this.TxtDefaultServiceStatus = new TextEdit();
            this.LblDefaultPriority = new LabelControl(); this.TxtDefaultAppointmentPriority = new TextEdit();
            this.BtnSave = new SimpleButton(); this.BtnCancel = new SimpleButton();

            this.SuspendLayout();
            this.Text = "Service Option";
            this.ClientSize = new Size(640, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;

            this.LblTitle.Text = "Service Option";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            this.TabMain.Location = new Point(12, 45);
            this.TabMain.Size = new Size(616, 385);
            this.PageMain.Text = "1. Main";
            this.PageNoteControl.Text = "2. Service Note Control";
            this.PageDefaults.Text = "3. Defaults";
            this.TabMain.TabPages.AddRange(new XtraTabPage[] { this.PageMain, this.PageNoteControl, this.PageDefaults });

            // Main tab
            this.ChkShowStockPicture.Properties.Caption = "Show Stock Picture";
            this.ChkShowStockPicture.Location = new Point(20, 20);
            this.ChkUseAlternativeItem.Properties.Caption = "Use Customer / Alternative Item";
            this.ChkUseAlternativeItem.Location = new Point(20, 50);
            this.ChkNegativeStockChecking.Properties.Caption = "Negative Stock Balance Checking";
            this.ChkNegativeStockChecking.Location = new Point(20, 80);
            this.PageMain.Controls.Add(this.ChkShowStockPicture);
            this.PageMain.Controls.Add(this.ChkUseAlternativeItem);
            this.PageMain.Controls.Add(this.ChkNegativeStockChecking);

            // Service Note Control tab
            this.ChkAutoGenSalesInvoice.Properties.Caption = "Auto Generate Sales Invoice on Close";
            this.ChkAutoGenSalesInvoice.Location = new Point(20, 20);
            this.ChkAutoCloseNote.Properties.Caption = "Auto Close Service Note on Solution Save";
            this.ChkAutoCloseNote.Location = new Point(20, 50);
            this.ChkAllowEditClosed.Properties.Caption = "Allow Modify Closed Service Note";
            this.ChkAllowEditClosed.Location = new Point(20, 80);
            this.PageNoteControl.Controls.Add(this.ChkAutoGenSalesInvoice);
            this.PageNoteControl.Controls.Add(this.ChkAutoCloseNote);
            this.PageNoteControl.Controls.Add(this.ChkAllowEditClosed);

            // Defaults tab
            Lbl(this.LblDefaultStatus, "Default Service Status", 20, 22);
            this.TxtDefaultServiceStatus.Location = new Point(180, 20); this.TxtDefaultServiceStatus.Width = 200;
            Lbl(this.LblDefaultPriority, "Default Appt. Priority", 20, 52);
            this.TxtDefaultAppointmentPriority.Location = new Point(180, 50); this.TxtDefaultAppointmentPriority.Width = 200;
            this.PageDefaults.Controls.Add(this.LblDefaultStatus); this.PageDefaults.Controls.Add(this.TxtDefaultServiceStatus);
            this.PageDefaults.Controls.Add(this.LblDefaultPriority); this.PageDefaults.Controls.Add(this.TxtDefaultAppointmentPriority);

            this.BtnSave.Text = "&Save"; this.BtnSave.Location = new Point(440, 440); this.BtnSave.Width = 85; this.BtnSave.Height = 30;
            this.BtnSave.Click += new System.EventHandler(this.OnSave);
            this.BtnCancel.Text = "&Cancel"; this.BtnCancel.Location = new Point(535, 440); this.BtnCancel.Width = 85; this.BtnCancel.Height = 30;
            this.BtnCancel.Click += new System.EventHandler(this.OnCancel);

            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.TabMain);
            this.Controls.Add(this.BtnSave); this.Controls.Add(this.BtnCancel);
            this.AcceptButton = this.BtnSave;
            this.CancelButton = this.BtnCancel;
            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y); }
    }
}
