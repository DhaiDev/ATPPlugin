using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    partial class AppointmentCalendar_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblFrom, LblTo, LblPerson;
        private DateEdit DtFrom, DtTo;
        private ComboBoxEdit CmbServicePerson;
        private SimpleButton BtnNew, BtnEdit, BtnRefresh, BtnExit;
        private GridControl GridAppointments;
        private GridView GridViewAppts;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblFrom = new LabelControl(); this.DtFrom = new DateEdit();
            this.LblTo = new LabelControl(); this.DtTo = new DateEdit();
            this.LblPerson = new LabelControl(); this.CmbServicePerson = new ComboBoxEdit();
            this.BtnNew = new SimpleButton(); this.BtnEdit = new SimpleButton();
            this.BtnRefresh = new SimpleButton(); this.BtnExit = new SimpleButton();
            this.GridAppointments = new GridControl(); this.GridViewAppts = new GridView();

            this.SuspendLayout();
            this.Text = "Service Appointments";
            this.ClientSize = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(900, 560);

            this.LblTitle.Text = "Service Appointments";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            Lbl(this.LblFrom, "From", 14, 52);
            this.DtFrom.Location = new Point(60, 49); this.DtFrom.Width = 130;
            Lbl(this.LblTo, "To", 200, 52);
            this.DtTo.Location = new Point(230, 49); this.DtTo.Width = 130;
            Lbl(this.LblPerson, "Service Person", 380, 52);
            this.CmbServicePerson.Location = new Point(480, 49); this.CmbServicePerson.Width = 200;

            this.BtnNew.Text = "New"; this.BtnNew.Location = new Point(780, 49); this.BtnNew.Width = 75; this.BtnNew.Height = 28;
            this.BtnNew.Click += new System.EventHandler(this.OnNew);
            this.BtnEdit.Text = "Edit"; this.BtnEdit.Location = new Point(858, 49); this.BtnEdit.Width = 75; this.BtnEdit.Height = 28;
            this.BtnEdit.Click += new System.EventHandler(this.OnEdit);
            this.BtnRefresh.Text = "Refresh"; this.BtnRefresh.Location = new Point(936, 49); this.BtnRefresh.Width = 75; this.BtnRefresh.Height = 28;
            this.BtnRefresh.Click += new System.EventHandler(this.OnRefresh);
            this.BtnExit.Text = "Exit"; this.BtnExit.Location = new Point(1014, 49); this.BtnExit.Width = 75; this.BtnExit.Height = 28;
            this.BtnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BtnExit.Click += new System.EventHandler(this.OnExit);

            this.GridAppointments.Location = new Point(14, 88);
            this.GridAppointments.Size = new Size(1075, 545);
            this.GridAppointments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewAppts.GridControl = this.GridAppointments;
            this.GridViewAppts.OptionsView.ShowGroupPanel = true;
            this.GridViewAppts.OptionsBehavior.Editable = false;
            this.GridAppointments.MainView = this.GridViewAppts;
            this.GridAppointments.ViewCollection.Add(this.GridViewAppts);
            AddCol(this.GridViewAppts, "StartTime", "Start", 130);
            AddCol(this.GridViewAppts, "FinishTime", "Finish", 130);
            AddCol(this.GridViewAppts, "Subject", "Subject", 200);
            AddCol(this.GridViewAppts, "DebtorCode", "Debtor", 100);
            AddCol(this.GridViewAppts, "DebtorName", "Debtor Name", 180);
            AddCol(this.GridViewAppts, "ServicePersonCode", "Person", 100);
            AddCol(this.GridViewAppts, "ServicePersonName", "Name", 140);
            AddCol(this.GridViewAppts, "TypeDescription", "Type", 100);
            AddCol(this.GridViewAppts, "PriorityDescription", "Priority", 90);
            AddCol(this.GridViewAppts, "Done", "Done", 60);

            Control[] cs = new Control[] { this.LblTitle, this.LblFrom, this.DtFrom, this.LblTo, this.DtTo,
                this.LblPerson, this.CmbServicePerson, this.BtnNew, this.BtnEdit, this.BtnRefresh, this.BtnExit,
                this.GridAppointments };
            foreach (var c in cs) this.Controls.Add(c);
            this.ResumeLayout(false);
        }

        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
        private static void AddCol(GridView gv, string f, string c, int w)
        {
            var col = new GridColumn(); col.FieldName = f; col.Caption = c; col.Visible = true; col.Width = w;
            col.VisibleIndex = gv.Columns.Count; gv.Columns.Add(col);
        }
    }
}
