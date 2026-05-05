using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

using AutoCount.Data;

using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ServiceContractPhotocopier.ServiceAppointment.OperationForms
{
    /// <summary>
    /// Read-only history view of all meter readings recorded against ONE
    /// ServiceItemMeterType row. Opened from Maintain Service Item → Tab 7
    /// "Meter Type" → Transaction button. Mirrors the V8 "Meter Type Transaction"
    /// popup: Meter Type / Meter Type Name / Reading Date / Meter Reading /
    /// Sales Invoice Code, with Delete (F9) / Delete All / Exit (F2) buttons.
    /// </summary>
    public partial class MeterTypeTransactionList_Form : XtraForm
    {
        private readonly DBSetting _db;
        private readonly long _serviceItemMeterTypeKey;
        private readonly string _meterTypeCode;
        private readonly string _meterTypeName;

        private LabelControl _lblTitle;
        private SimpleButton _btnDelete;
        private SimpleButton _btnDeleteAll;
        private SimpleButton _btnExit;
        private GridControl _grid;
        private GridView _view;
        private LabelControl _lblCount;

        public MeterTypeTransactionList_Form() { InitializeComponent(); }

        public MeterTypeTransactionList_Form(DBSetting db, long serviceItemMeterTypeKey,
                                             string meterTypeCode, string meterTypeName)
            : this()
        {
            _db = db;
            _serviceItemMeterTypeKey = serviceItemMeterTypeKey;
            _meterTypeCode = meterTypeCode ?? "";
            _meterTypeName = meterTypeName ?? "";
            BuildLayout();
            this.Load += new EventHandler(OnFormLoad);
        }

        private void BuildLayout()
        {
            this.Text = "Meter Type Transaction";
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(900, 460);
            this.MinimumSize = new Size(700, 300);

            this._lblTitle = new LabelControl();
            this._lblTitle.Text = "Meter Type Transaction";
            this._lblTitle.Appearance.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            this._lblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            this._lblTitle.Location = new Point(14, 10);
            this._lblTitle.AutoSizeMode = LabelAutoSizeMode.None;
            this._lblTitle.Size = new Size(420, 32);

            this._btnExit = new SimpleButton();
            this._btnExit.Text = "Exit (F2)";
            this._btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this._btnExit.Size = new Size(90, 28);
            this._btnExit.Location = new Point(this.ClientSize.Width - 100, 10);
            this._btnExit.Click += new EventHandler(OnExit);

            this._btnDeleteAll = new SimpleButton();
            this._btnDeleteAll.Text = "Delete All";
            this._btnDeleteAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this._btnDeleteAll.Size = new Size(90, 28);
            this._btnDeleteAll.Location = new Point(this.ClientSize.Width - 196, 10);
            this._btnDeleteAll.Click += new EventHandler(OnDeleteAll);

            this._btnDelete = new SimpleButton();
            this._btnDelete.Text = "Delete (F9)";
            this._btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this._btnDelete.Size = new Size(90, 28);
            this._btnDelete.Location = new Point(this.ClientSize.Width - 292, 10);
            this._btnDelete.Click += new EventHandler(OnDelete);

            this._grid = new GridControl();
            this._grid.Location = new Point(0, 50);
            this._grid.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 78);
            this._grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this._view = new GridView();
            this._view.GridControl = this._grid;
            this._view.OptionsView.ShowGroupPanel = false;
            this._view.OptionsView.ColumnAutoWidth = true;
            this._view.OptionsBehavior.Editable = false;
            this._view.OptionsBehavior.ReadOnly = true;
            this._view.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this._grid.MainView = this._view;

            GridColumn cMT = new GridColumn();
            cMT.FieldName = "MeterType"; cMT.Caption = "Meter Type";
            cMT.Visible = true; cMT.VisibleIndex = 0; cMT.Width = 110;

            GridColumn cMTN = new GridColumn();
            cMTN.FieldName = "MeterTypeName"; cMTN.Caption = "Meter Type Name";
            cMTN.Visible = true; cMTN.VisibleIndex = 1; cMTN.Width = 240;

            GridColumn cRD = new GridColumn();
            cRD.FieldName = "ReadingDate"; cRD.Caption = "Reading Date";
            cRD.Visible = true; cRD.VisibleIndex = 2; cRD.Width = 160;
            cRD.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            cRD.DisplayFormat.FormatString = "d/M/yyyy h:mm:ss tt";

            GridColumn cMR = new GridColumn();
            cMR.FieldName = "MeterReading"; cMR.Caption = "Meter Reading";
            cMR.Visible = true; cMR.VisibleIndex = 3; cMR.Width = 110;
            cMR.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            cMR.DisplayFormat.FormatString = "n0";

            GridColumn cInv = new GridColumn();
            cInv.FieldName = "SalesInvoiceCode"; cInv.Caption = "Sales Invoice Code";
            cInv.Visible = true; cInv.VisibleIndex = 4; cInv.Width = 130;

            this._view.Columns.Add(cMT);
            this._view.Columns.Add(cMTN);
            this._view.Columns.Add(cRD);
            this._view.Columns.Add(cMR);
            this._view.Columns.Add(cInv);

            this._lblCount = new LabelControl();
            this._lblCount.Text = "0 transaction(s)";
            this._lblCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this._lblCount.Location = new Point(14, this.ClientSize.Height - 22);
            this._lblCount.Appearance.Font = new Font("Tahoma", 9F, FontStyle.Bold);

            this.Controls.Add(this._lblTitle);
            this.Controls.Add(this._btnExit);
            this.Controls.Add(this._btnDeleteAll);
            this.Controls.Add(this._btnDelete);
            this.Controls.Add(this._grid);
            this.Controls.Add(this._lblCount);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void ReloadGrid()
        {
            if (_db == null) return;
            string sql =
                "SELECT mt.MeterTransKey, " +
                "       mt.MeterTypeCode AS MeterType, " +
                "       ISNULL(lk.[Description],'') AS MeterTypeName, " +
                "       mt.MeterTransDate AS ReadingDate, " +
                "       mt.MeterTransReading AS MeterReading, " +
                "       ISNULL(iv.DocNo,'') AS SalesInvoiceCode " +
                "FROM   [dbo].[zSCP_MeterTrans] mt " +
                "LEFT JOIN [dbo].[zSCP_MeterType] lk ON lk.MeterTypeCode = mt.MeterTypeCode " +
                "LEFT JOIN [dbo].[IV] iv             ON iv.DocKey       = mt.SalesInvoiceDocKey " +
                "WHERE  mt.ServiceItemMeterTypeKey = " + _serviceItemMeterTypeKey + " " +
                "ORDER BY mt.MeterTransDate, mt.MeterTransKey";

            DataTable dt = _db.GetDataTable(sql, false);
            this._grid.DataSource = dt;
            this._lblCount.Text = dt.Rows.Count + " transaction(s)";
        }

        private long FocusedTransKey()
        {
            int rh = this._view.FocusedRowHandle;
            if (rh < 0) return 0;
            DataRow row = this._view.GetDataRow(rh);
            if (row == null) return 0;
            return Convert.ToInt64(row["MeterTransKey"]);
        }

        private void OnDelete(object sender, EventArgs e)
        {
            long key = FocusedTransKey();
            if (key <= 0)
            {
                XtraMessageBox.Show("Pick a row to delete first.", "Delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult ok = XtraMessageBox.Show(
                "Delete this meter reading?\r\n\r\nThe linked Sales Invoice (if any) is NOT removed.",
                "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ok != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(_db.ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM [dbo].[zSCP_MeterTrans] WHERE MeterTransKey=@k", cn);
                    cmd.Parameters.AddWithValue("@k", key);
                    cmd.ExecuteNonQuery();
                }
                ReloadGrid();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Delete failed:\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnDeleteAll(object sender, EventArgs e)
        {
            DialogResult ok = XtraMessageBox.Show(
                "Delete ALL meter readings for " + _meterTypeCode + "?\r\n\r\n" +
                "This is permanent. Linked Sales Invoices are NOT removed.",
                "Confirm Delete All",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ok != DialogResult.Yes) return;

            try
            {
                using (SqlConnection cn = new SqlConnection(_db.ConnectionString))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM [dbo].[zSCP_MeterTrans] WHERE ServiceItemMeterTypeKey=@k", cn);
                    cmd.Parameters.AddWithValue("@k", _serviceItemMeterTypeKey);
                    cmd.ExecuteNonQuery();
                }
                ReloadGrid();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Delete All failed:\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2) { OnExit(this, EventArgs.Empty); return true; }
            if (keyData == Keys.F9) { OnDelete(this, EventArgs.Empty); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
