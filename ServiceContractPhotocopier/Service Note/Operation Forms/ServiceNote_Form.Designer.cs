using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace ServiceContractPhotocopier.ServiceNote.OperationForms
{
    partial class ServiceNote_Form
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private LabelControl LblTitle;
        private SimpleButton BtnEdit, BtnSave, BtnCancel, BtnPreview, BtnDelete, BtnPrint, BtnAttachments, BtnCopyFrom, BtnSearch, BtnExit;
        private PanelControl PanelHeader;
        private LabelControl LblDate, LblNoteNo, LblDebtorCode, LblDebtorName, LblAgentCode, LblRefNo, LblDescription;
        private DateEdit DtNoteDate;
        private TextEdit TxtServiceNoteNo, TxtDebtorCode, TxtDebtorName, TxtAgentCode, TxtRefNo, TxtDescription;
        private LabelControl LblAddress, LblAttention, LblAreaCode, LblTermCode, LblValidity, LblDeliveryTerm;
        private MemoEdit TxtAddress;
        private TextEdit TxtAttention, TxtAreaCode, TxtTermCode, TxtValidity, TxtDeliveryTerm;
        private GroupControl GrpServiceTag;
        private LabelControl LblSvcTagCode, LblStockCode, LblBrandModel, LblContractNo, LblServiceStartDate, LblServiceEndDate;
        private TextEdit TxtServiceItemCode, TxtStockCode, TxtBrandModel, TxtContractNo;
        private DateEdit DtServiceStartDate, DtServiceEndDate;
        private LabelControl LblStatusSvc, LblSeverity, LblServiceType, LblProblem, LblSolution, LblAttendedBy, LblAssignTo, LblClosed;
        private ComboBoxEdit CmbServiceStatus, CmbSeverity, CmbServiceType, CmbProblem, CmbSolution, CmbAttendedBy, CmbAssignTo;
        private CheckEdit ChkClosed;
        private MemoEdit TxtProblemRemark, TxtSolutionRemark;
        private XtraTabControl TabMain;
        private XtraTabPage PageMain, PageAppointments, PageChargesItem, PageMoreHeader, PageNotes, PageRemarks;
        private GridControl GridChargesItems;
        private GridView GridViewCI;
        private MemoEdit TxtNote;
        private TextEdit TxtRemark1, TxtRemark2, TxtRemark3, TxtRemark4;
        private LabelControl LblR1, LblR2, LblR3, LblR4;

        private void InitializeComponent()
        {
            this.LblTitle = new LabelControl();
            this.BtnEdit = new SimpleButton(); this.BtnSave = new SimpleButton(); this.BtnCancel = new SimpleButton();
            this.BtnPreview = new SimpleButton(); this.BtnDelete = new SimpleButton(); this.BtnPrint = new SimpleButton();
            this.BtnAttachments = new SimpleButton(); this.BtnCopyFrom = new SimpleButton();
            this.BtnSearch = new SimpleButton(); this.BtnExit = new SimpleButton();
            this.PanelHeader = new PanelControl();
            this.LblDate = new LabelControl(); this.DtNoteDate = new DateEdit();
            this.LblNoteNo = new LabelControl(); this.TxtServiceNoteNo = new TextEdit();
            this.LblDebtorCode = new LabelControl(); this.TxtDebtorCode = new TextEdit();
            this.LblDebtorName = new LabelControl(); this.TxtDebtorName = new TextEdit();
            this.LblAgentCode = new LabelControl(); this.TxtAgentCode = new TextEdit();
            this.LblRefNo = new LabelControl(); this.TxtRefNo = new TextEdit();
            this.LblDescription = new LabelControl(); this.TxtDescription = new TextEdit();
            this.LblAddress = new LabelControl(); this.TxtAddress = new MemoEdit();
            this.LblAttention = new LabelControl(); this.TxtAttention = new TextEdit();
            this.LblAreaCode = new LabelControl(); this.TxtAreaCode = new TextEdit();
            this.LblTermCode = new LabelControl(); this.TxtTermCode = new TextEdit();
            this.LblValidity = new LabelControl(); this.TxtValidity = new TextEdit();
            this.LblDeliveryTerm = new LabelControl(); this.TxtDeliveryTerm = new TextEdit();
            this.GrpServiceTag = new GroupControl();
            this.LblSvcTagCode = new LabelControl(); this.TxtServiceItemCode = new TextEdit();
            this.LblStockCode = new LabelControl(); this.TxtStockCode = new TextEdit();
            this.LblBrandModel = new LabelControl(); this.TxtBrandModel = new TextEdit();
            this.LblContractNo = new LabelControl(); this.TxtContractNo = new TextEdit();
            this.LblServiceStartDate = new LabelControl(); this.DtServiceStartDate = new DateEdit();
            this.LblServiceEndDate = new LabelControl(); this.DtServiceEndDate = new DateEdit();
            this.LblStatusSvc = new LabelControl(); this.CmbServiceStatus = new ComboBoxEdit();
            this.LblSeverity = new LabelControl(); this.CmbSeverity = new ComboBoxEdit();
            this.LblServiceType = new LabelControl(); this.CmbServiceType = new ComboBoxEdit();
            this.LblProblem = new LabelControl(); this.CmbProblem = new ComboBoxEdit();
            this.LblSolution = new LabelControl(); this.CmbSolution = new ComboBoxEdit();
            this.LblAttendedBy = new LabelControl(); this.CmbAttendedBy = new ComboBoxEdit();
            this.LblAssignTo = new LabelControl(); this.CmbAssignTo = new ComboBoxEdit();
            this.LblClosed = new LabelControl(); this.ChkClosed = new CheckEdit();
            this.TxtProblemRemark = new MemoEdit(); this.TxtSolutionRemark = new MemoEdit();
            this.TabMain = new XtraTabControl();
            this.PageMain = new XtraTabPage(); this.PageAppointments = new XtraTabPage();
            this.PageChargesItem = new XtraTabPage(); this.PageMoreHeader = new XtraTabPage();
            this.PageNotes = new XtraTabPage(); this.PageRemarks = new XtraTabPage();
            this.GridChargesItems = new GridControl(); this.GridViewCI = new GridView();
            this.TxtNote = new MemoEdit();
            this.TxtRemark1 = new TextEdit(); this.TxtRemark2 = new TextEdit();
            this.TxtRemark3 = new TextEdit(); this.TxtRemark4 = new TextEdit();
            this.LblR1 = new LabelControl(); this.LblR2 = new LabelControl();
            this.LblR3 = new LabelControl(); this.LblR4 = new LabelControl();

            this.SuspendLayout();

            this.Text = "Service Note";
            this.ClientSize = new Size(1150, 760);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(1050, 700);

            this.LblTitle.Text = "Service Note";
            this.LblTitle.Appearance.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            this.LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this.LblTitle.Location = new Point(14, 8);

            int tbY = 40;
            Tb(this.BtnEdit, "Edit (F5)", 14, tbY, 85);
            Tb(this.BtnSave, "Save (F7)", 102, tbY, 85); this.BtnSave.Click += new System.EventHandler(this.OnSave);
            Tb(this.BtnCancel, "Cancel (F8)", 190, tbY, 85); this.BtnCancel.Click += new System.EventHandler(this.OnCancel);
            Tb(this.BtnPreview, "Preview", 278, tbY, 75);
            Tb(this.BtnDelete, "Delete (F6)", 356, tbY, 85);
            Tb(this.BtnPrint, "Print (F3)", 444, tbY, 85);
            Tb(this.BtnAttachments, "Attachments", 850, tbY, 90);
            Tb(this.BtnCopyFrom, "Copy From", 943, tbY, 85);
            Tb(this.BtnSearch, "Search (F2)", 1031, tbY, 85);
            Tb(this.BtnExit, "Exit (F2)", 1119, tbY, 75); this.BtnExit.Click += new System.EventHandler(this.OnExit);

            this.PanelHeader.Location = new Point(8, 78);
            this.PanelHeader.Size = new Size(1128, 170);
            this.PanelHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            int lblX = 14, edX = 130, edW = 200;
            int y = 12, gap = 28;
            Lbl(this.LblDate, "Date", lblX, y);
            this.DtNoteDate.Location = new Point(edX, y); this.DtNoteDate.Width = 150;
            y += gap;
            Lbl(this.LblNoteNo, "Service Note No", lblX, y);
            this.TxtServiceNoteNo.Location = new Point(edX, y); this.TxtServiceNoteNo.Width = edW;
            y += gap;
            Lbl(this.LblDebtorCode, "Debtor Code", lblX, y);
            this.TxtDebtorCode.Location = new Point(edX, y); this.TxtDebtorCode.Width = edW;
            y += gap;
            Lbl(this.LblAgentCode, "Agent Code", lblX, y);
            this.TxtAgentCode.Location = new Point(edX, y); this.TxtAgentCode.Width = edW;
            y += gap;
            Lbl(this.LblRefNo, "Reference No", lblX, y);
            this.TxtRefNo.Location = new Point(edX, y); this.TxtRefNo.Width = edW;
            y += gap;
            Lbl(this.LblDescription, "Description", lblX, y);
            this.TxtDescription.Location = new Point(edX, y); this.TxtDescription.Width = edW;

            int mLbl = 360, mEd = 460, mW = 180;
            int my = 12;
            Lbl(this.LblDebtorName, "Debtor Name", mLbl, my);
            this.TxtDebtorName.Location = new Point(mEd, my); this.TxtDebtorName.Width = mW;
            my += gap;
            Lbl(this.LblTermCode, "Term", mLbl, my);
            this.TxtTermCode.Location = new Point(mEd, my); this.TxtTermCode.Width = mW;
            my += gap;
            Lbl(this.LblAreaCode, "Area", mLbl, my);
            this.TxtAreaCode.Location = new Point(mEd, my); this.TxtAreaCode.Width = mW;
            my += gap;
            Lbl(this.LblValidity, "Validity", mLbl, my);
            this.TxtValidity.Location = new Point(mEd, my); this.TxtValidity.Width = mW;
            my += gap;
            Lbl(this.LblDeliveryTerm, "Delivery Term", mLbl, my);
            this.TxtDeliveryTerm.Location = new Point(mEd, my); this.TxtDeliveryTerm.Width = mW;

            int rLbl = 680, rEd = 770, rW = 340;
            int ry = 12;
            Lbl(this.LblAddress, "Address", rLbl, ry);
            this.TxtAddress.Location = new Point(rEd, ry); this.TxtAddress.Size = new Size(rW, 80);
            ry += 86;
            Lbl(this.LblAttention, "Attention", rLbl, ry);
            this.TxtAttention.Location = new Point(rEd, ry); this.TxtAttention.Width = rW;

            this.PanelHeader.Controls.Add(this.LblDate); this.PanelHeader.Controls.Add(this.DtNoteDate);
            this.PanelHeader.Controls.Add(this.LblNoteNo); this.PanelHeader.Controls.Add(this.TxtServiceNoteNo);
            this.PanelHeader.Controls.Add(this.LblDebtorCode); this.PanelHeader.Controls.Add(this.TxtDebtorCode);
            this.PanelHeader.Controls.Add(this.LblDebtorName); this.PanelHeader.Controls.Add(this.TxtDebtorName);
            this.PanelHeader.Controls.Add(this.LblAgentCode); this.PanelHeader.Controls.Add(this.TxtAgentCode);
            this.PanelHeader.Controls.Add(this.LblRefNo); this.PanelHeader.Controls.Add(this.TxtRefNo);
            this.PanelHeader.Controls.Add(this.LblDescription); this.PanelHeader.Controls.Add(this.TxtDescription);
            this.PanelHeader.Controls.Add(this.LblTermCode); this.PanelHeader.Controls.Add(this.TxtTermCode);
            this.PanelHeader.Controls.Add(this.LblAreaCode); this.PanelHeader.Controls.Add(this.TxtAreaCode);
            this.PanelHeader.Controls.Add(this.LblValidity); this.PanelHeader.Controls.Add(this.TxtValidity);
            this.PanelHeader.Controls.Add(this.LblDeliveryTerm); this.PanelHeader.Controls.Add(this.TxtDeliveryTerm);
            this.PanelHeader.Controls.Add(this.LblAddress); this.PanelHeader.Controls.Add(this.TxtAddress);
            this.PanelHeader.Controls.Add(this.LblAttention); this.PanelHeader.Controls.Add(this.TxtAttention);

            this.GrpServiceTag.Text = "Service Tag Information";
            this.GrpServiceTag.Location = new Point(8, 252);
            this.GrpServiceTag.Size = new Size(1128, 130);
            int gL = 14, gE = 130, gW = 200;
            int gY = 28, gGap = 28;
            Lbl(this.LblSvcTagCode, "Service Tag", gL, gY);
            this.TxtServiceItemCode.Location = new Point(gE, gY); this.TxtServiceItemCode.Width = gW;
            gY += gGap;
            Lbl(this.LblStockCode, "Stock Code", gL, gY);
            this.TxtStockCode.Location = new Point(gE, gY); this.TxtStockCode.Width = gW;
            gY += gGap;
            Lbl(this.LblBrandModel, "Brand / Model", gL, gY);
            this.TxtBrandModel.Location = new Point(gE, gY); this.TxtBrandModel.Width = gW;

            int gL2 = 360, gE2 = 480, gW2 = 200;
            int gY2 = 28;
            Lbl(this.LblContractNo, "Contract No", gL2, gY2);
            this.TxtContractNo.Location = new Point(gE2, gY2); this.TxtContractNo.Width = gW2;
            gY2 += gGap;
            Lbl(this.LblServiceStartDate, "Service Start Date", gL2, gY2);
            this.DtServiceStartDate.Location = new Point(gE2, gY2); this.DtServiceStartDate.Width = gW2;
            gY2 += gGap;
            Lbl(this.LblServiceEndDate, "Service End Date", gL2, gY2);
            this.DtServiceEndDate.Location = new Point(gE2, gY2); this.DtServiceEndDate.Width = gW2;

            int gL3 = 710, gE3 = 820, gW3 = 200;
            int gY3 = 28;
            Lbl(this.LblStatusSvc, "Status", gL3, gY3);
            this.CmbServiceStatus.Location = new Point(gE3, gY3); this.CmbServiceStatus.Width = gW3;
            gY3 += gGap;
            Lbl(this.LblServiceType, "Type", gL3, gY3);
            this.CmbServiceType.Location = new Point(gE3, gY3); this.CmbServiceType.Width = gW3;
            gY3 += gGap;
            Lbl(this.LblSeverity, "Severity", gL3, gY3);
            this.CmbSeverity.Location = new Point(gE3, gY3); this.CmbSeverity.Width = gW3;

            this.GrpServiceTag.Controls.Add(this.LblSvcTagCode); this.GrpServiceTag.Controls.Add(this.TxtServiceItemCode);
            this.GrpServiceTag.Controls.Add(this.LblStockCode); this.GrpServiceTag.Controls.Add(this.TxtStockCode);
            this.GrpServiceTag.Controls.Add(this.LblBrandModel); this.GrpServiceTag.Controls.Add(this.TxtBrandModel);
            this.GrpServiceTag.Controls.Add(this.LblContractNo); this.GrpServiceTag.Controls.Add(this.TxtContractNo);
            this.GrpServiceTag.Controls.Add(this.LblServiceStartDate); this.GrpServiceTag.Controls.Add(this.DtServiceStartDate);
            this.GrpServiceTag.Controls.Add(this.LblServiceEndDate); this.GrpServiceTag.Controls.Add(this.DtServiceEndDate);
            this.GrpServiceTag.Controls.Add(this.LblStatusSvc); this.GrpServiceTag.Controls.Add(this.CmbServiceStatus);
            this.GrpServiceTag.Controls.Add(this.LblServiceType); this.GrpServiceTag.Controls.Add(this.CmbServiceType);
            this.GrpServiceTag.Controls.Add(this.LblSeverity); this.GrpServiceTag.Controls.Add(this.CmbSeverity);

            this.TabMain.Location = new Point(8, 386);
            this.TabMain.Size = new Size(1128, 360);
            this.TabMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.PageMain.Text = "1. Main";
            this.PageAppointments.Text = "2. Appointments / Activities";
            this.PageChargesItem.Text = "3. Charges Item";
            this.PageMoreHeader.Text = "4. More Header";
            this.PageNotes.Text = "5. Notes";
            this.PageRemarks.Text = "6. Remarks";
            this.TabMain.TabPages.AddRange(new XtraTabPage[] {
                this.PageMain, this.PageAppointments, this.PageChargesItem, this.PageMoreHeader, this.PageNotes, this.PageRemarks });

            int pX = 14, pL = 130, pEW = 480;
            int pY = 14;
            Lbl(this.LblProblem, "Problem Code", pX, pY);
            this.CmbProblem.Location = new Point(pL, pY); this.CmbProblem.Width = 200;
            pY += 28;
            var lblPR = new LabelControl { Text = "Problem Remark", Location = new Point(pX, pY + 3) };
            this.PageMain.Controls.Add(lblPR);
            this.TxtProblemRemark.Location = new Point(pL, pY); this.TxtProblemRemark.Size = new Size(pEW, 60);
            pY += 68;
            Lbl(this.LblSolution, "Solution Code", pX, pY);
            this.CmbSolution.Location = new Point(pL, pY); this.CmbSolution.Width = 200;
            pY += 28;
            var lblSR = new LabelControl { Text = "Solution Remark", Location = new Point(pX, pY + 3) };
            this.PageMain.Controls.Add(lblSR);
            this.TxtSolutionRemark.Location = new Point(pL, pY); this.TxtSolutionRemark.Size = new Size(pEW, 60);
            pY += 68;
            Lbl(this.LblAttendedBy, "Attended By", pX, pY);
            this.CmbAttendedBy.Location = new Point(pL, pY); this.CmbAttendedBy.Width = 200;
            pY += 28;
            Lbl(this.LblAssignTo, "Assign To", pX, pY);
            this.CmbAssignTo.Location = new Point(pL, pY); this.CmbAssignTo.Width = 200;
            pY += 28;
            this.ChkClosed.Properties.Caption = "Closed";
            this.ChkClosed.Location = new Point(pL, pY);

            this.PageMain.Controls.Add(this.LblProblem); this.PageMain.Controls.Add(this.CmbProblem);
            this.PageMain.Controls.Add(this.TxtProblemRemark);
            this.PageMain.Controls.Add(this.LblSolution); this.PageMain.Controls.Add(this.CmbSolution);
            this.PageMain.Controls.Add(this.TxtSolutionRemark);
            this.PageMain.Controls.Add(this.LblAttendedBy); this.PageMain.Controls.Add(this.CmbAttendedBy);
            this.PageMain.Controls.Add(this.LblAssignTo); this.PageMain.Controls.Add(this.CmbAssignTo);
            this.PageMain.Controls.Add(this.ChkClosed);

            this.GridChargesItems.Dock = DockStyle.Fill;
            this.GridViewCI.GridControl = this.GridChargesItems;
            this.GridViewCI.OptionsView.ShowGroupPanel = false;
            this.GridChargesItems.MainView = this.GridViewCI;
            this.GridChargesItems.ViewCollection.Add(this.GridViewCI);
            AddCol(this.GridViewCI, "No", "No", 40);
            AddCol(this.GridViewCI, "StockCode", "Item Code", 120);
            AddCol(this.GridViewCI, "Description", "Description", 280);
            AddCol(this.GridViewCI, "UOM", "UOM", 60);
            AddCol(this.GridViewCI, "Qty", "Quantity", 90);
            AddCol(this.GridViewCI, "UnitPrice", "Unit Price", 110);
            AddCol(this.GridViewCI, "Amount", "Amount", 120);
            this.PageChargesItem.Controls.Add(this.GridChargesItems);

            this.TxtNote.Dock = DockStyle.Fill;
            this.PageNotes.Controls.Add(this.TxtNote);

            Lbl(this.LblR1, "Remark 1", 14, 20); this.TxtRemark1.Location = new Point(110, 17); this.TxtRemark1.Width = 700;
            Lbl(this.LblR2, "Remark 2", 14, 50); this.TxtRemark2.Location = new Point(110, 47); this.TxtRemark2.Width = 700;
            Lbl(this.LblR3, "Remark 3", 14, 80); this.TxtRemark3.Location = new Point(110, 77); this.TxtRemark3.Width = 700;
            Lbl(this.LblR4, "Remark 4", 14, 110); this.TxtRemark4.Location = new Point(110, 107); this.TxtRemark4.Width = 700;
            this.PageRemarks.Controls.Add(this.LblR1); this.PageRemarks.Controls.Add(this.TxtRemark1);
            this.PageRemarks.Controls.Add(this.LblR2); this.PageRemarks.Controls.Add(this.TxtRemark2);
            this.PageRemarks.Controls.Add(this.LblR3); this.PageRemarks.Controls.Add(this.TxtRemark3);
            this.PageRemarks.Controls.Add(this.LblR4); this.PageRemarks.Controls.Add(this.TxtRemark4);

            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.BtnEdit); this.Controls.Add(this.BtnSave); this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnPreview); this.Controls.Add(this.BtnDelete); this.Controls.Add(this.BtnPrint);
            this.Controls.Add(this.BtnAttachments); this.Controls.Add(this.BtnCopyFrom);
            this.Controls.Add(this.BtnSearch); this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.PanelHeader);
            this.Controls.Add(this.GrpServiceTag);
            this.Controls.Add(this.TabMain);

            this.ResumeLayout(false);
        }

        private static void Tb(SimpleButton b, string t, int x, int y, int w) { b.Text = t; b.Location = new Point(x, y); b.Width = w; b.Height = 28; }
        private static void Lbl(LabelControl l, string t, int x, int y) { l.Text = t; l.Location = new Point(x, y + 3); }
        private static void AddCol(GridView gv, string f, string c, int w)
        {
            var col = new GridColumn(); col.FieldName = f; col.Caption = c; col.Visible = true; col.Width = w;
            col.VisibleIndex = gv.Columns.Count; gv.Columns.Add(col);
        }
    }
}
