using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ServiceContractPhotocopier.ServiceItem.MasterForms
{
    partial class GenerateServiceItemFromSerial_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblDebtor, LblStock, LblPrefix, LblCount;
        private TextEdit TxtDebtorCode, TxtStockCode, TxtSerialPrefix, TxtCount;
        private SimpleButton BtnGenerate, BtnCancel;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblDebtor = new LabelControl(); this.TxtDebtorCode = new TextEdit();
            this.LblStock = new LabelControl(); this.TxtStockCode = new TextEdit();
            this.LblPrefix = new LabelControl(); this.TxtSerialPrefix = new TextEdit();
            this.LblCount = new LabelControl(); this.TxtCount = new TextEdit();
            this.BtnGenerate = new SimpleButton(); this.BtnCancel = new SimpleButton();

            this.SuspendLayout();
            this.Text = "Generate Service Item From Serial No";
            this.ClientSize = new Size(520, 280);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;

            this.LblTitle.Text = "Generate Service Item From Serial No";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            int lX = 14, eX = 150, eW = 340;
            int y = 50, gap = 32;
            Lbl(this.LblDebtor, "Debtor Code", lX, y);
            this.TxtDebtorCode.Location = new Point(eX, y); this.TxtDebtorCode.Width = eW;
            y += gap;
            Lbl(this.LblStock, "Stock Code", lX, y);
            this.TxtStockCode.Location = new Point(eX, y); this.TxtStockCode.Width = eW;
            y += gap;
            Lbl(this.LblPrefix, "Serial Prefix", lX, y);
            this.TxtSerialPrefix.Location = new Point(eX, y); this.TxtSerialPrefix.Width = eW;
            y += gap;
            Lbl(this.LblCount, "Count", lX, y);
            this.TxtCount.Location = new Point(eX, y); this.TxtCount.Width = 120; this.TxtCount.Text = "1";

            this.BtnGenerate.Text = "Generate"; this.BtnGenerate.Location = new Point(310, 235); this.BtnGenerate.Width = 100; this.BtnGenerate.Height = 30;
            this.BtnGenerate.Click += new System.EventHandler(this.OnGenerate);
            this.BtnCancel.Text = "Cancel"; this.BtnCancel.Location = new Point(415, 235); this.BtnCancel.Width = 95; this.BtnCancel.Height = 30;
            this.BtnCancel.Click += new System.EventHandler(this.OnCancel);

            Control[] cs = new Control[] { this.LblTitle, this.LblDebtor, this.TxtDebtorCode, this.LblStock, this.TxtStockCode,
                this.LblPrefix, this.TxtSerialPrefix, this.LblCount, this.TxtCount, this.BtnGenerate, this.BtnCancel };
            foreach (var c in cs) this.Controls.Add(c);
            this.AcceptButton = this.BtnGenerate;
            this.CancelButton = this.BtnCancel;
            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
    }
}
