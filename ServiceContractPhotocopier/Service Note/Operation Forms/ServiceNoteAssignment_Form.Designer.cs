using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.ServiceNote.OperationForms
{
    partial class ServiceNoteAssignment_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle, LblPerson;
        private ComboBoxEdit CmbServicePerson;
        private SimpleButton BtnAssign, BtnClose;
        private GridControl GridNotes;
        private GridView GridViewNotes;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.LblPerson = new LabelControl(); this.CmbServicePerson = new ComboBoxEdit();
            this.BtnAssign = new SimpleButton(); this.BtnClose = new SimpleButton();
            this.GridNotes = new GridControl(); this.GridViewNotes = new GridView();

            this.SuspendLayout();
            this.Text = "Service Note Assignment";
            this.ClientSize = new Size(960, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(800, 500);

            this.LblTitle.Text = "Service Note Assignment";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 10);

            Lbl(this.LblPerson, "Assign To", 14, 50);
            this.CmbServicePerson.Location = new Point(100, 47); this.CmbServicePerson.Width = 240;

            this.BtnAssign.Text = "Assign Selected";
            this.BtnAssign.Location = new Point(360, 47); this.BtnAssign.Width = 130; this.BtnAssign.Height = 28;
            this.BtnAssign.Click += new System.EventHandler(this.OnAssign);

            this.BtnClose.Text = "Close";
            this.BtnClose.Location = new Point(860, 47); this.BtnClose.Width = 85; this.BtnClose.Height = 28;
            this.BtnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BtnClose.Click += new System.EventHandler(this.OnClose);

            this.GridNotes.Location = new Point(14, 85);
            this.GridNotes.Size = new Size(930, 460);
            this.GridNotes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.GridViewNotes.GridControl = this.GridNotes;
            this.GridViewNotes.OptionsView.ShowGroupPanel = false;
            this.GridViewNotes.OptionsBehavior.Editable = false;
            this.GridViewNotes.OptionsSelection.MultiSelect = true;
            this.GridNotes.MainView = this.GridViewNotes;
            this.GridNotes.ViewCollection.Add(this.GridViewNotes);
            AddCol(this.GridViewNotes, "ServiceNoteCode", "Note No", 120);
            AddCol(this.GridViewNotes, "ServiceNoteDate", "Date", 120);
            AddCol(this.GridViewNotes, "DebtorCode", "Debtor Code", 100);
            AddCol(this.GridViewNotes, "DebtorName", "Debtor Name", 220);
            AddCol(this.GridViewNotes, "ServiceItemCode", "Service Item", 120);
            AddCol(this.GridViewNotes, "AssignToServicePersonCode", "Assign To", 120);
            AddCol(this.GridViewNotes, "ServiceStatusCode", "Status", 100);

            Control[] cs = new Control[] { this.LblTitle, this.LblPerson, this.CmbServicePerson, this.BtnAssign, this.BtnClose, this.GridNotes };
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
