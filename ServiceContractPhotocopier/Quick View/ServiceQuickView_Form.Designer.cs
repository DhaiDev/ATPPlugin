using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.QuickView
{
    partial class ServiceQuickView_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private SimpleButton BtnRefresh, BtnExit;
        private GroupControl GrpToday, GrpTomorrow, GrpOpenNotes, GrpOverdue;
        private LabelControl LblTodayCount, LblTomorrowCount, LblOpenNotesCount, LblOverdueCount;
        private GroupControl GrpNotes, GrpAppts, GrpTopStock;
        private GridControl GridNotes, GridAppts, GridTopStock;
        private GridView GridViewNotes, GridViewAppts, GridViewTopStock;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.BtnRefresh = new SimpleButton(); this.BtnExit = new SimpleButton();
            this.GrpToday = new GroupControl(); this.LblTodayCount = new LabelControl();
            this.GrpTomorrow = new GroupControl(); this.LblTomorrowCount = new LabelControl();
            this.GrpOpenNotes = new GroupControl(); this.LblOpenNotesCount = new LabelControl();
            this.GrpOverdue = new GroupControl(); this.LblOverdueCount = new LabelControl();
            this.GrpNotes = new GroupControl(); this.GridNotes = new GridControl(); this.GridViewNotes = new GridView();
            this.GrpAppts = new GroupControl(); this.GridAppts = new GridControl(); this.GridViewAppts = new GridView();
            this.GrpTopStock = new GroupControl(); this.GridTopStock = new GridControl(); this.GridViewTopStock = new GridView();

            this.SuspendLayout();
            this.Text = "Service - Quick View";
            this.ClientSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(1000, 650);

            this.LblTitle.Text = "Service - Quick View";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 8);

            this.BtnRefresh.Text = "Refresh (F5)"; this.BtnRefresh.Location = new Point(900, 12); this.BtnRefresh.Width = 100; this.BtnRefresh.Height = 28;
            this.BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BtnRefresh.Click += new System.EventHandler(this.OnRefresh);
            this.BtnExit.Text = "Exit (F2)"; this.BtnExit.Location = new Point(1005, 12); this.BtnExit.Width = 85; this.BtnExit.Height = 28;
            this.BtnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BtnExit.Click += new System.EventHandler(this.OnExit);

            // Stat cards
            SetupCard(this.GrpToday,    "Today's Appointments",    this.LblTodayCount,    new Point(14, 50),  Color.SteelBlue);
            SetupCard(this.GrpTomorrow, "Tomorrow's Appointments", this.LblTomorrowCount, new Point(280, 50), Color.DarkOrange);
            SetupCard(this.GrpOpenNotes,"Open Service Notes",      this.LblOpenNotesCount,new Point(546, 50), Color.SeaGreen);
            SetupCard(this.GrpOverdue,  "Overdue Notes",           this.LblOverdueCount,  new Point(812, 50), Color.Firebrick);

            // Notes grid
            this.GrpNotes.Text = "Top 10 Open Service Notes";
            this.GrpNotes.Location = new Point(14, 170);
            this.GrpNotes.Size = new Size(540, 250);
            this.GrpNotes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.GridNotes.Dock = DockStyle.Fill;
            this.GridViewNotes.GridControl = this.GridNotes;
            this.GridViewNotes.OptionsView.ShowGroupPanel = false;
            this.GridViewNotes.OptionsBehavior.Editable = false;
            this.GridNotes.MainView = this.GridViewNotes;
            this.GridNotes.ViewCollection.Add(this.GridViewNotes);
            AddCol(this.GridViewNotes, "ServiceNoteCode", "Note No", 110);
            AddCol(this.GridViewNotes, "ServiceNoteDate", "Date", 100);
            AddCol(this.GridViewNotes, "DebtorCode", "Debtor", 100);
            AddCol(this.GridViewNotes, "ServiceStatusCode", "Status", 100);
            AddCol(this.GridViewNotes, "Description", "Description", 200);
            this.GrpNotes.Controls.Add(this.GridNotes);

            // Appointments grid
            this.GrpAppts.Text = "Upcoming Appointments";
            this.GrpAppts.Location = new Point(560, 170);
            this.GrpAppts.Size = new Size(530, 250);
            this.GrpAppts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridAppts.Dock = DockStyle.Fill;
            this.GridViewAppts.GridControl = this.GridAppts;
            this.GridViewAppts.OptionsView.ShowGroupPanel = false;
            this.GridViewAppts.OptionsBehavior.Editable = false;
            this.GridAppts.MainView = this.GridViewAppts;
            this.GridAppts.ViewCollection.Add(this.GridViewAppts);
            AddCol(this.GridViewAppts, "StartTime", "Start", 130);
            AddCol(this.GridViewAppts, "Subject", "Subject", 180);
            AddCol(this.GridViewAppts, "DebtorCode", "Debtor", 100);
            AddCol(this.GridViewAppts, "ServicePersonCode", "Person", 100);
            AddCol(this.GridViewAppts, "AppointmentTypeCode", "Type", 90);
            this.GrpAppts.Controls.Add(this.GridAppts);

            // Top stock grid
            this.GrpTopStock.Text = "Top 10 Service Stock Codes";
            this.GrpTopStock.Location = new Point(14, 430);
            this.GrpTopStock.Size = new Size(1076, 235);
            this.GrpTopStock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridTopStock.Dock = DockStyle.Fill;
            this.GridViewTopStock.GridControl = this.GridTopStock;
            this.GridViewTopStock.OptionsView.ShowGroupPanel = false;
            this.GridViewTopStock.OptionsBehavior.Editable = false;
            this.GridTopStock.MainView = this.GridViewTopStock;
            this.GridTopStock.ViewCollection.Add(this.GridViewTopStock);
            AddCol(this.GridViewTopStock, "StockCode", "Stock Code", 200);
            AddCol(this.GridViewTopStock, "Count", "Count", 120);
            this.GrpTopStock.Controls.Add(this.GridTopStock);

            Control[] cs = new Control[] { this.LblTitle, this.BtnRefresh, this.BtnExit,
                this.GrpToday, this.GrpTomorrow, this.GrpOpenNotes, this.GrpOverdue,
                this.GrpNotes, this.GrpAppts, this.GrpTopStock };
            foreach (var c in cs) this.Controls.Add(c);
            this.ResumeLayout(false);
        }

        private static void SetupCard(GroupControl grp, string title, LabelControl countLbl, Point loc, Color color)
        {
            grp.Text = title;
            grp.Location = loc;
            grp.Size = new Size(256, 110);
            countLbl.Text = "0";
            countLbl.Appearance.Font = new Font("Tahoma", 28F, FontStyle.Bold);
            countLbl.Appearance.ForeColor = color;
            countLbl.Location = new Point(100, 40);
            grp.Controls.Add(countLbl);
        }

        private static void AddCol(GridView gv, string f, string c, int w)
        {
            var col = new GridColumn(); col.FieldName = f; col.Caption = c; col.Visible = true; col.Width = w;
            col.VisibleIndex = gv.Columns.Count; gv.Columns.Add(col);
        }
    }
}
