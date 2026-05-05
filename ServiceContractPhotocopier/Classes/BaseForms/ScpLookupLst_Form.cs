using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AutoCount.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.Classes.BaseForms
{
    /// <summary>
    /// Standardized list+inline-edit form for all zSCP_LK_* lookup tables.
    /// Matches the real AutoCount UI pattern from screenshots:
    ///   - Red title at top left
    ///   - Refresh + Exit (F2) buttons at top right
    ///   - Grid filling most of the form (columns: Code | Description | Inactive)
    ///   - Bottom edit panel (Code + Inactive checkbox on line 1, Description on line 2)
    ///   - Bottom toolbar: Add (F5) | Edit (F6) | Save (F7) | Cancel (F8) | Delete (F9)
    ///   - Status bar at very bottom
    /// No separate popup editor dialog — editing is inline.
    /// </summary>
    public class ScpLookupLst_Form : XtraForm
    {
        protected GridControl GridCtl;
        protected GridView GridVw;
        protected SimpleButton BtnAdd, BtnEdit, BtnSave, BtnCancel, BtnDelete, BtnRefresh, BtnExit;
        protected LabelControl LblTitle;
        protected PanelControl PanelEdit;
        protected PanelControl PanelToolbar;
        protected PanelControl PanelStatus;
        protected LabelControl LblCode, LblDesc, LblStatus;
        protected TextEdit TxtCode, TxtDesc;
        protected CheckEdit ChkInactive;

        protected DBSetting _dbSetting;
        private long _selectedKey = 0;
        private bool _isNewRow = false;
        private bool _editMode = false;

        protected virtual string TableName   { get { return "zSCP_LK_Unknown"; } }
        protected virtual string ViewName    { get { return null; } }
        protected virtual string KeyColumn   { get { return "Code"; } }
        protected virtual string KeyField    { get { return "Key"; } }
        protected virtual string FormCaption { get { return "Lookup"; } }
        protected virtual string StatusText  { get { return "Lookup"; } }

        public ScpLookupLst_Form() { InitBaseLayout(); }

        public ScpLookupLst_Form(DBSetting dbSetting) : this()
        {
            _dbSetting = dbSetting;
            this.Load += delegate { LoadData(); SetEditMode(false); };
        }

        private void InitBaseLayout()
        {
            this.Text = FormCaption;
            this.ClientSize = new Size(920, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(750, 500);

            // Title
            LblTitle = new LabelControl();
            LblTitle.Text = FormCaption;
            LblTitle.Appearance.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            LblTitle.Appearance.ForeColor = Color.FromArgb(180, 20, 40);
            LblTitle.Location = new Point(14, 10);

            // Refresh + Exit buttons
            BtnRefresh = new SimpleButton();
            BtnRefresh.Text = "Refresh";
            BtnRefresh.Location = new Point(770, 10);
            BtnRefresh.Width = 65;
            BtnRefresh.Height = 26;
            BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnRefresh.Click += delegate { LoadData(); };

            BtnExit = new SimpleButton();
            BtnExit.Text = "Exit (F2)";
            BtnExit.Location = new Point(845, 10);
            BtnExit.Width = 65;
            BtnExit.Height = 26;
            BtnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnExit.Click += delegate { this.Close(); };

            // Grid — fills center
            GridCtl = new GridControl();
            GridCtl.Location = new Point(14, 44);
            GridCtl.Size = new Size(892, 380);
            GridCtl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            GridVw = new GridView(GridCtl);
            GridVw.OptionsBehavior.Editable = false;
            GridVw.OptionsView.ShowGroupPanel = false;
            GridVw.OptionsView.ColumnAutoWidth = true;
            GridCtl.MainView = GridVw;
            GridCtl.ViewCollection.Add(GridVw);
            // Columns set by subclass or default
            var colCode = new GridColumn(); colCode.FieldName = "Col_Code"; colCode.Caption = "Code"; colCode.Visible = true; colCode.Width = 150; colCode.VisibleIndex = 0;
            var colDesc = new GridColumn(); colDesc.FieldName = "Col_Desc"; colDesc.Caption = "Description"; colDesc.Visible = true; colDesc.Width = 500; colDesc.VisibleIndex = 1;
            var colInac = new GridColumn(); colInac.FieldName = "Inactive"; colInac.Caption = "Inactive"; colInac.Visible = true; colInac.Width = 70; colInac.VisibleIndex = 2;
            GridVw.Columns.Add(colCode);
            GridVw.Columns.Add(colDesc);
            GridVw.Columns.Add(colInac);
            GridVw.FocusedRowChanged += delegate { OnGridRowChanged(); };

            // Bottom edit panel
            PanelEdit = new PanelControl();
            PanelEdit.Location = new Point(14, 430);
            PanelEdit.Size = new Size(892, 65);
            PanelEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PanelEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            LblCode = new LabelControl();
            LblCode.Text = "Code :";
            LblCode.Location = new Point(6, 8);

            TxtCode = new TextEdit();
            TxtCode.Location = new Point(140, 5);
            TxtCode.Width = 180;

            ChkInactive = new CheckEdit();
            ChkInactive.Properties.Caption = "Inactive";
            ChkInactive.Location = new Point(340, 5);

            LblDesc = new LabelControl();
            LblDesc.Text = "Description :";
            LblDesc.Location = new Point(6, 36);

            TxtDesc = new TextEdit();
            TxtDesc.Location = new Point(140, 33);
            TxtDesc.Width = 740;
            TxtDesc.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            PanelEdit.Controls.Add(LblCode);
            PanelEdit.Controls.Add(TxtCode);
            PanelEdit.Controls.Add(ChkInactive);
            PanelEdit.Controls.Add(LblDesc);
            PanelEdit.Controls.Add(TxtDesc);

            // Bottom toolbar
            PanelToolbar = new PanelControl();
            PanelToolbar.Location = new Point(14, 500);
            PanelToolbar.Size = new Size(892, 38);
            PanelToolbar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PanelToolbar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            BtnAdd    = MakeToolBtn("Add (F5)",    6,   delegate { OnAdd(); });
            BtnEdit   = MakeToolBtn("Edit (F6)",   96,  delegate { OnEditBtn(); });
            BtnSave   = MakeToolBtn("Save (F7)",   186, delegate { OnSave(); });
            BtnCancel = MakeToolBtn("Cancel (F8)", 276, delegate { OnCancelEdit(); });
            BtnDelete = MakeToolBtn("Delete (F9)", 366, delegate { OnDelete(); });

            PanelToolbar.Controls.Add(BtnAdd);
            PanelToolbar.Controls.Add(BtnEdit);
            PanelToolbar.Controls.Add(BtnSave);
            PanelToolbar.Controls.Add(BtnCancel);
            PanelToolbar.Controls.Add(BtnDelete);

            // Status bar
            PanelStatus = new PanelControl();
            PanelStatus.Dock = DockStyle.Bottom;
            PanelStatus.Height = 24;
            LblStatus = new LabelControl();
            LblStatus.Text = StatusText;
            LblStatus.Location = new Point(6, 5);
            PanelStatus.Controls.Add(LblStatus);

            this.Controls.Add(LblTitle);
            this.Controls.Add(BtnRefresh);
            this.Controls.Add(BtnExit);
            this.Controls.Add(GridCtl);
            this.Controls.Add(PanelEdit);
            this.Controls.Add(PanelToolbar);
            this.Controls.Add(PanelStatus);

            // F-key shortcuts
            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
        }

        private SimpleButton MakeToolBtn(string text, int x, EventHandler onClick)
        {
            var b = new SimpleButton();
            b.Text = text;
            b.Location = new Point(x, 4);
            b.Width = 85;
            b.Height = 28;
            b.Click += onClick;
            return b;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2) { this.Close(); e.Handled = true; }
            else if (e.KeyCode == Keys.F5) { OnAdd(); e.Handled = true; }
            else if (e.KeyCode == Keys.F6) { OnEditBtn(); e.Handled = true; }
            else if (e.KeyCode == Keys.F7) { OnSave(); e.Handled = true; }
            else if (e.KeyCode == Keys.F8) { OnCancelEdit(); e.Handled = true; }
            else if (e.KeyCode == Keys.F9) { OnDelete(); e.Handled = true; }
        }

        protected virtual void LoadData()
        {
            if (_dbSetting == null) return;
            try
            {
                string source = string.IsNullOrEmpty(ViewName) ? TableName : ViewName;
                string sql = "SELECT * FROM [dbo].[" + source + "] ORDER BY [" + KeyColumn + "]";
                var dt = _dbSetting.GetDataTable(sql, false);
                if (dt == null) dt = new DataTable();

                // Point grid columns at the actual table columns (idempotent — look up by FieldName or Caption).
                var colCode = FindGridColumn("Col_Code", "Code");
                var colDesc = FindGridColumn("Col_Desc", "Description");
                if (colCode != null && dt.Columns.Contains(KeyColumn))
                {
                    colCode.FieldName = KeyColumn;
                    colCode.Caption  = KeyColumn.Replace("Code", " Code").Replace("  ", " ").Trim();
                }
                if (colDesc != null && dt.Columns.Contains("Description"))
                {
                    colDesc.FieldName = "Description";
                }
                GridCtl.DataSource = dt;
                if (LblCode   != null) LblCode.Text   = KeyColumn.Replace("Code", " Code").Replace("  ", " ").Trim() + " :";
                if (LblStatus != null) LblStatus.Text = StatusText;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Failed to load " + TableName + ":\r\n" + ex.Message,
                    "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private GridColumn FindGridColumn(params string[] fieldNamesOrCaptions)
        {
            if (GridVw == null) return null;
            foreach (var key in fieldNamesOrCaptions)
            {
                foreach (GridColumn col in GridVw.Columns)
                {
                    if (col == null) continue;
                    if (string.Equals(col.FieldName, key, StringComparison.OrdinalIgnoreCase)) return col;
                    if (string.Equals(col.Caption,   key, StringComparison.OrdinalIgnoreCase)) return col;
                    if (string.Equals(col.Name,      key, StringComparison.OrdinalIgnoreCase)) return col;
                }
            }
            return null;
        }

        private void OnGridRowChanged()
        {
            int rh = GridVw.FocusedRowHandle;
            if (rh < 0) { ClearEdit(); return; }
            var row = GridVw.GetDataRow(rh);
            if (row == null) { ClearEdit(); return; }
            _selectedKey = row.Table.Columns.Contains(KeyField)
                ? Convert.ToInt64(row[KeyField])
                : 0;
            _isNewRow = false;
            TxtCode.Text = row.Table.Columns.Contains(KeyColumn) ? row[KeyColumn].ToString() : "";
            TxtDesc.Text = row.Table.Columns.Contains("Description") && row["Description"] != DBNull.Value ? row["Description"].ToString() : "";
            ChkInactive.Checked = row.Table.Columns.Contains("Inactive") && row["Inactive"] != DBNull.Value && row["Inactive"].ToString() == "Y";
        }

        private void ClearEdit()
        {
            _selectedKey = 0;
            _isNewRow = false;
            TxtCode.Text = "";
            TxtDesc.Text = "";
            ChkInactive.Checked = false;
        }

        private void SetEditMode(bool editing)
        {
            _editMode = editing;
            TxtCode.Properties.ReadOnly = !editing || !_isNewRow;
            TxtDesc.Properties.ReadOnly = !editing;
            ChkInactive.Properties.ReadOnly = !editing;
            BtnSave.Enabled = editing;
            BtnCancel.Enabled = editing;
            BtnAdd.Enabled = !editing;
            BtnEdit.Enabled = !editing;
            BtnDelete.Enabled = !editing;
        }

        private void OnAdd()
        {
            ClearEdit();
            _isNewRow = true;
            SetEditMode(true);
            TxtCode.Focus();
        }

        private void OnEditBtn()
        {
            if (string.IsNullOrEmpty(TxtCode.Text)) return;
            _isNewRow = false;
            SetEditMode(true);
            TxtDesc.Focus();
        }

        private void OnCancelEdit()
        {
            OnGridRowChanged();
            SetEditMode(false);
        }

        private void OnSave()
        {
            if (string.IsNullOrWhiteSpace(TxtCode.Text))
            {
                XtraMessageBox.Show("Code is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string code = SQLString(TxtCode.Text.Trim());
                string desc = SQLString(TxtDesc.Text ?? "");
                string inac = ChkInactive.Checked ? "Y" : "N";

                if (_isNewRow)
                {
                    _dbSetting.ExecuteNonQuery(
                        "INSERT INTO [dbo].[" + TableName + "] ([" + KeyColumn + "], [Description], [Inactive]) " +
                        "VALUES (N'" + code + "', N'" + desc + "', '" + inac + "')");
                }
                else
                {
                    _dbSetting.ExecuteNonQuery(
                        "UPDATE [dbo].[" + TableName + "] SET [Description] = N'" + desc + "', " +
                        "[Inactive] = '" + inac + "', [LastModified] = GETDATE() " +
                        "WHERE [" + KeyColumn + "] = N'" + code + "'");
                }

                SetEditMode(false);
                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Save failed:\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OnDelete()
        {
            if (string.IsNullOrEmpty(TxtCode.Text)) return;
            if (XtraMessageBox.Show("Delete '" + TxtCode.Text + "'?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                _dbSetting.ExecuteNonQuery(
                    "DELETE FROM [dbo].[" + TableName + "] WHERE [" + KeyColumn + "] = N'" + SQLString(TxtCode.Text.Trim()) + "'");
                ClearEdit();
                SetEditMode(false);
                LoadData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Delete failed:\r\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OpenEditor(DataRow existingRow) { }
    }
}
