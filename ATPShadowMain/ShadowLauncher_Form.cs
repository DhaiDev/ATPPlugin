using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using AutoCount.Data;

using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;

namespace ATPShadowMain
{
    /// <summary>
    /// Dev launcher window: AutoCount-style NavBar on the left lists every plugin form
    /// grouped by module; clicking opens the form non-modally with the active DBSetting.
    /// Keeps Program.cs static — no need to edit code to switch which form to test.
    /// </summary>
    public partial class ShadowLauncher_Form : XtraForm
    {
        private readonly DBSetting _db;
        private NavBarControl _nav;
        private PanelControl _canvas;
        private LabelControl _lblTitle;
        private LabelControl _lblHint;
        private LabelControl _lblStatus;

        public ShadowLauncher_Form() { InitializeComponent(); }

        public ShadowLauncher_Form(DBSetting db) : this()
        {
            _db = db;
            BuildLayout();
            BuildNav();
        }

        private void BuildLayout()
        {
            this.Text = "ATP Shadow Launcher — Service & Contract (Photocopier)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1100, 720);
            this.MinimumSize = new Size(900, 600);
            this.IsMdiContainer = false;

            this._nav = new NavBarControl();
            this._nav.Dock = DockStyle.Left;
            this._nav.Width = 320;
            this._nav.OptionsNavPane.ExpandedWidth = 320;
            this._nav.PaintStyleKind = NavBarViewKind.NavigationPane;
            this._nav.OptionsNavPane.ShowExpandButton = false;
            this._nav.OptionsNavPane.ShowOverflowButton = false;
            this._nav.LinkClicked += new NavBarLinkEventHandler(this.OnNavLinkClicked);

            this._canvas = new PanelControl();
            this._canvas.Dock = DockStyle.Fill;
            this._canvas.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            this._lblTitle = new LabelControl();
            this._lblTitle.Text = "ATP Shadow Launcher";
            this._lblTitle.Appearance.Font = new Font("Tahoma", 18F, FontStyle.Bold);
            this._lblTitle.Appearance.ForeColor = Color.FromArgb(40, 60, 110);
            this._lblTitle.Location = new Point(40, 40);
            this._lblTitle.AutoSizeMode = LabelAutoSizeMode.None;
            this._lblTitle.Size = new Size(700, 40);

            this._lblHint = new LabelControl();
            this._lblHint.Text = "Pick a module from the navigation bar on the left to open it.\r\n" +
                                 "Forms open as non-modal windows — close them to return here. Close this window to exit.";
            this._lblHint.Appearance.Font = new Font("Tahoma", 10F);
            this._lblHint.Appearance.ForeColor = Color.FromArgb(80, 80, 80);
            this._lblHint.Location = new Point(40, 90);
            this._lblHint.AutoSizeMode = LabelAutoSizeMode.None;
            this._lblHint.Size = new Size(700, 60);

            this._lblStatus = new LabelControl();
            this._lblStatus.Text = BuildStatusText();
            this._lblStatus.Appearance.Font = new Font("Consolas", 9F);
            this._lblStatus.Appearance.ForeColor = Color.FromArgb(60, 100, 60);
            this._lblStatus.Location = new Point(40, 170);
            this._lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
            this._lblStatus.Size = new Size(700, 80);

            this._canvas.Controls.Add(this._lblTitle);
            this._canvas.Controls.Add(this._lblHint);
            this._canvas.Controls.Add(this._lblStatus);

            this.Controls.Add(this._canvas);
            this.Controls.Add(this._nav);
        }

        private string BuildStatusText()
        {
            string user = AutoCount.Authentication.UserSession.CurrentUserSession != null
                ? AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID
                : "(none)";
            string srv = _db != null ? _db.ServerName : "(none)";
            string dbName = _db != null ? _db.DBName : "(none)";
            return "Logged in as : " + user + "\r\n" +
                   "Server       : " + srv + "\r\n" +
                   "Database     : " + dbName;
        }

        private void BuildNav()
        {
            List<CatalogEntry> entries = FormCatalog.All();

            Dictionary<string, NavBarGroup> groups = new Dictionary<string, NavBarGroup>();

            foreach (CatalogEntry entry in entries)
            {
                NavBarGroup grp;
                if (!groups.TryGetValue(entry.Group, out grp))
                {
                    grp = new NavBarGroup(entry.Group);
                    grp.GroupStyle = NavBarGroupStyle.SmallIconsText;
                    grp.Expanded = true;
                    this._nav.Groups.Add(grp);
                    groups[entry.Group] = grp;
                }

                NavBarItem item = new NavBarItem(entry.Title);
                item.Tag = entry;
                this._nav.Items.Add(item);

                NavBarItemLink link = new NavBarItemLink(item);
                grp.ItemLinks.Add(link);
            }

            if (this._nav.Groups.Count > 0) this._nav.ActiveGroup = this._nav.Groups[0];
        }

        private void OnNavLinkClicked(object sender, NavBarLinkEventArgs e)
        {
            CatalogEntry entry = e.Link.Item.Tag as CatalogEntry;
            if (entry == null) return;

            try
            {
                Form frm = entry.Create(_db);
                if (frm == null) return;
                frm.Owner = this;
                frm.StartPosition = frm.StartPosition == FormStartPosition.WindowsDefaultLocation
                    ? FormStartPosition.CenterScreen
                    : frm.StartPosition;
                frm.Show();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    "Failed to open '" + entry.Title + "':\r\n\r\n" + ex.Message,
                    "ATP Shadow Launcher",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
