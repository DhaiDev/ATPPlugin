using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AutoCount.Data;
using DevExpress.XtraEditors;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;

namespace ServiceContractPhotocopier.Classes.BaseForms
{
    /// <summary>
    /// Shared editor for zSCP_ServicePerson, zSCP_ServiceAdvisor, zSCP_Mechanic.
    /// All three tables share the same schema (Code, Name, Name2, Gender, Phone, Email,
    /// ICNo, Occupation, DateJoined, DateLeft, Inactive) so one editor handles all.
    /// </summary>
    public class ScpPersonEdt_Form : XtraForm
    {
        protected LabelControl LblCode, LblName, LblName2, LblGender, LblPhone, LblEmail, LblICNo, LblOccupation, LblDateJoined, LblInactive;
        protected TextEdit TxtCode, TxtName, TxtName2, TxtPhone, TxtEmail, TxtICNo, TxtOccupation;
        protected ComboBoxEdit CmbGender;
        protected DateEdit DtJoined;
        protected CheckEdit ChkInactive;
        protected SimpleButton BtnSave, BtnCancel;

        protected DBSetting _dbSetting;
        protected DataRow _existing;

        protected virtual string TableName   { get { return "zSCP_ServicePerson"; } }
        protected virtual string KeyColumn   { get { return "ServicePersonCode"; } }
        protected virtual string FormCaption { get { return "Edit Service Person"; } }

        public ScpPersonEdt_Form() { InitializeBaseLayout(); }

        public ScpPersonEdt_Form(DBSetting dbSetting, DataRow existing) : this()
        {
            _dbSetting = dbSetting;
            _existing = existing;
            this.Load += delegate { PopulateFromRow(); };
        }

        private void InitializeBaseLayout()
        {
            this.Text = FormCaption;
            this.Width = 620;
            this.Height = 460;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            int lblX = 20;
            int edX  = 150;
            int edW  = 420;
            int y    = 20;
            int gap  = 32;

            LblCode       = MakeLabel("Code",        lblX, y);           TxtCode       = MakeText(edX, y, edW);           y += gap;
            LblName       = MakeLabel("Name",        lblX, y);           TxtName       = MakeText(edX, y, edW);           y += gap;
            LblName2      = MakeLabel("Name 2",      lblX, y);           TxtName2      = MakeText(edX, y, edW);           y += gap;
            LblGender     = MakeLabel("Gender",      lblX, y);           CmbGender     = new ComboBoxEdit();
            CmbGender.Location = new Point(edX, y);  CmbGender.Width = 180;
            CmbGender.Properties.Items.AddRange(new object[] { "UNKNOWN", "MALE", "FEMALE" });
            CmbGender.SelectedIndex = 0;
            y += gap;
            LblPhone      = MakeLabel("Phone",       lblX, y);           TxtPhone      = MakeText(edX, y, edW);           y += gap;
            LblEmail      = MakeLabel("Email",       lblX, y);           TxtEmail      = MakeText(edX, y, edW);           y += gap;
            LblICNo       = MakeLabel("IC / NRIC",   lblX, y);           TxtICNo       = MakeText(edX, y, edW);           y += gap;
            LblOccupation = MakeLabel("Occupation",  lblX, y);           TxtOccupation = MakeText(edX, y, edW);           y += gap;
            LblDateJoined = MakeLabel("Date Joined", lblX, y);           DtJoined      = new DateEdit(); DtJoined.Location = new Point(edX, y); DtJoined.Width = 180; y += gap;
            LblInactive   = MakeLabel("Inactive",    lblX, y);           ChkInactive   = new CheckEdit(); ChkInactive.Location = new Point(edX - 2, y - 2); ChkInactive.Properties.Caption = "";

            BtnSave   = new SimpleButton(); BtnSave.Text = "&Save";   BtnSave.Location = new Point(410, 385); BtnSave.Width = 75;  BtnSave.Height = 28;
            BtnCancel = new SimpleButton(); BtnCancel.Text = "&Cancel"; BtnCancel.Location = new Point(495, 385); BtnCancel.Width = 75; BtnCancel.Height = 28;
            BtnSave.Click   += new EventHandler(OnSave);
            BtnCancel.Click += delegate { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(LblCode);   this.Controls.Add(TxtCode);
            this.Controls.Add(LblName);   this.Controls.Add(TxtName);
            this.Controls.Add(LblName2);  this.Controls.Add(TxtName2);
            this.Controls.Add(LblGender); this.Controls.Add(CmbGender);
            this.Controls.Add(LblPhone);  this.Controls.Add(TxtPhone);
            this.Controls.Add(LblEmail);  this.Controls.Add(TxtEmail);
            this.Controls.Add(LblICNo);   this.Controls.Add(TxtICNo);
            this.Controls.Add(LblOccupation); this.Controls.Add(TxtOccupation);
            this.Controls.Add(LblDateJoined); this.Controls.Add(DtJoined);
            this.Controls.Add(LblInactive);   this.Controls.Add(ChkInactive);
            this.Controls.Add(BtnSave);       this.Controls.Add(BtnCancel);

            this.AcceptButton = BtnSave;
            this.CancelButton = BtnCancel;
        }

        private LabelControl MakeLabel(string text, int x, int y)
        {
            var l = new LabelControl();
            l.Text = text;
            l.Location = new Point(x, y + 3);
            return l;
        }

        private TextEdit MakeText(int x, int y, int w)
        {
            var t = new TextEdit();
            t.Location = new Point(x, y);
            t.Width = w;
            return t;
        }

        protected virtual void PopulateFromRow()
        {
            if (_existing == null) return;
            TxtCode.Text = Get("", KeyColumn);
            TxtCode.Properties.ReadOnly = true;
            TxtName.Text = Get("", "Name");
            TxtName2.Text = Get("", "Name2");
            CmbGender.Text = Get("UNKNOWN", "Gender");
            TxtPhone.Text = Get("", "Phone");
            TxtEmail.Text = Get("", "Email");
            TxtICNo.Text = Get("", "ICNo");
            TxtOccupation.Text = Get("", "Occupation");
            if (_existing.Table.Columns.Contains("DateJoined") && _existing["DateJoined"] != DBNull.Value)
                DtJoined.DateTime = Convert.ToDateTime(_existing["DateJoined"]);
            ChkInactive.Checked = Get("N", "Inactive") == "Y";
        }

        private string Get(string def, string col)
        {
            if (_existing == null) return def;
            if (!_existing.Table.Columns.Contains(col)) return def;
            var v = _existing[col];
            return (v == null || v == DBNull.Value) ? def : v.ToString();
        }

        protected virtual void OnSave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCode.Text))
            {
                XtraMessageBox.Show("Code is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string code = SQLString(TxtCode.Text.Trim());
                string name = SQLString(TxtName.Text ?? "");
                string name2 = SQLString(TxtName2.Text ?? "");
                string gender = SQLString(CmbGender.Text ?? "UNKNOWN");
                string phone = SQLString(TxtPhone.Text ?? "");
                string email = SQLString(TxtEmail.Text ?? "");
                string icno = SQLString(TxtICNo.Text ?? "");
                string occ = SQLString(TxtOccupation.Text ?? "");
                string joined = DtJoined.DateTime == DateTime.MinValue ? "NULL" : "'" + DtJoined.DateTime.ToString("yyyy-MM-dd") + "'";
                string inac = ChkInactive.Checked ? "Y" : "N";

                string sql;
                if (_existing == null)
                {
                    sql = "INSERT INTO [dbo].[" + TableName + "] " +
                          "([" + KeyColumn + "],[Name],[Name2],[Gender],[Phone],[Email],[ICNo],[Occupation],[DateJoined],[Inactive]) " +
                          "VALUES (N'" + code + "', N'" + name + "', N'" + name2 + "', N'" + gender + "', N'" + phone + "', N'" +
                          email + "', N'" + icno + "', N'" + occ + "', " + joined + ", '" + inac + "')";
                }
                else
                {
                    sql = "UPDATE [dbo].[" + TableName + "] SET " +
                          "[Name]=N'" + name + "',[Name2]=N'" + name2 + "',[Gender]=N'" + gender + "'," +
                          "[Phone]=N'" + phone + "',[Email]=N'" + email + "',[ICNo]=N'" + icno + "'," +
                          "[Occupation]=N'" + occ + "',[DateJoined]=" + joined + ",[Inactive]='" + inac + "'," +
                          "[LastModified]=GETDATE() " +
                          "WHERE [" + KeyColumn + "]=N'" + code + "'";
                }
                _dbSetting.ExecuteNonQuery(sql);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Save failed:\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
