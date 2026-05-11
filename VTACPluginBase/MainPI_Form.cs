using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using AutoCount.Authentication;
using AutoCount.Data;

//using static VTACPluginBase.Classes.AccessRightsConsts_Cls;
//using static VTACPluginBase.Classes.Common_Cls;
using static VTACPluginBase.PlugIn_Cls;

using VTACPluginBase.Classes;
using VTACPluginBase.Classes.TextLogger;
//using VTACPluginBase.OperationForms.Installment;
//using VTACPluginBase.SystemForms;

namespace VTACPluginBase
{
    public partial class MainPI_Form : IContainerControl
    {
        #region " Variables/Constants declaration section "
        private const string str_myTips = "Installment Plug-In Modules Main Form";
        #endregion //" Variables/Constants declaration section "

        #region " Constructor "
        public MainPI_Form(UserSession userSession)
        {
            //// here just pass in the autocount's dbsetting for our own DBSetting object
            //if (myDBSetting == null) myDBSetting = userSession.DBSetting;

            // 'Report
            // myBasicReportOption = New Autocount.BasicReportOption()

            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }
        #endregion //" Constructor "

        #region " Form Events "
        private void MainPI_Form_Load(System.Object sender, System.EventArgs e)
        {
            // start timer
            this.Refresh_Tmr.Enabled = true;

            // set the default explorer group to 1st group
            this.Plugin_NavBar.ActiveGroup = this.Master_NavBarGrp;

            // load user id in status bar
            this.barStaticItem1.Caption = "User ID: " + AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID;

            // to create and display the standard panel header using autocount's obj
            string str_version = System.Windows.Forms.Application.ProductVersion; // Add by LF on 20110420
            bool boo_Is64BitsApps = Environment.Is64BitProcess; // added by chang on 20141231
            string str_BitsVersion = (boo_Is64BitsApps ? " (64bits)" : " (32bits)"); // added by chang on 20141231

            str_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            AutoCount.Controls.PanelHeader ctrl_panelhdr = new AutoCount.Controls.PanelHeader(this, str_myTips + " " + str_version + str_BitsVersion, str_myTips); // added by chang on 20141231

            SetBarTreeView(); // added by chang on 20110425
            SetMenuBarView(); // added by chang on 20110425
        }

        private void MainPI_Form_Disposed(object sender, System.EventArgs e)
        {
            // stop timer
            this.Refresh_Tmr.Enabled = false;
        }
        #endregion //" Form Events "

        #region " Timer Events "
        // use to refresh the times displayed at the status bar
        private void Refresh_Tmr_Tick(System.Object sender, System.EventArgs e)
        {
            this.barStaticItem2.Caption = DateTime.Now.ToString("dddd, yyyy-MMM-dd hh:mm:ss tt");
        }
        #endregion //" Timer Events "

        #region " Menu Bar Sub Item Events "
        // use to exit this plugin main form
        private void ExitPI_BarSItm_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // close this form
            this.Close();
        }
        #endregion //" Menu Bar Sub Item Events "

        #region " Navigation Bar Control Events "
        // use to handle the event when active group changed
        private void Plugin_NavBar_ActiveGroupChanged(System.Object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            // MessageBox.Show(e.Group.Caption)

            // exit form if selected 'Exit PlugIn' group
            if (e.Group.Caption == "Exit PlugIn") this.Close();
        }

        #region " Master Portion "
        // use to handle <Mas_NavBarItem01> List Module
        private void Mas_NavBarItem01_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_MCHMAS); // edited by chang on 20110425
        }

        // use to handle <Mas_NavBarItem02> List Module
        private void Mas_NavBarItem02_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_BITMAS); // edited by chang on 20110425
        }

        // use to handle <Mas_NavBarItem03> List Module
        private void Mas_NavBarItem03_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), "STK_ITEM_SHOW");
        }
        #endregion //" Master Portion "

        #region " Operation Portion "
        // use to handle <Opr_NavBarItem01> List Module
        private void Opr_NavBarItem01_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_INSTMT); // edited by chang on 20110425
        }

        // use to handle <Opr_NavBarItem02> List Module
        private void Opr_NavBarItem02_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_INSTMT);
        }

        // use to handle <Opr_NavBarItem03> List Module
        private void Opr_NavBarItem03_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_INSTMT);
        }

        // use to handle <Opr_NavBarItem04> List Module
        private void Opr_NavBarItem04_LinkClicked(System.Object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_BULKORD);
        }
        #endregion //" Operation Portion "
        #endregion //" Navigation Bar Control Events "

        #region " Menu Bar Buttons Events "
        #region " Master Portion "
        // use to handle <Mas_BarButtonItem01> Module
        private void Mas_BarButtonItem01_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_MCHMAS); // edited by chang on 20110425
        }

        // use to handle <Mas_BarButtonItem02> Module
        private void Mas_BarButtonItem02_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_BITMAS); // edited by chang on 20110425
        }

        // use to handle <Mas_BarButtonItem03> Module
        private void Mas_BarButtonItem03_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }
        #endregion //" Master Portion "

        #region " Operation Portion "
        // use to handle <Opr_BarButtonItem01> List Module
        private void Opr_BarButtonItem01_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_INSTMT); // edited by chang on 20110425
        }

        // use to handle <Opr_BarButtonItem02> List Module
        private void Opr_BarButtonItem02_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_INSTMT); // edited by chang on 20110425
        }

        // use to handle <Opr_BarButtonItem03> List Module
        private void Opr_BarButtonItem03_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_INSTMT); // edited by chang on 20110425
        }

        // use to handle <Opr_BarButtonItem04> List Module
        private void Opr_BarButtonItem04_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_SHOW_BULKORD); // edited by chang on 20110425
        }
        #endregion //" Operation Portion "

        #region " Other Portion "
        private void Oth_BarButtonItem01_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //LoadForm(e.Link.Item.Tag.ToString(), CMD_OPEN_INSTMT); // edited by chang on 20110425
        }
        #endregion //" Other Portion "
        #endregion //" Menu Bar Buttons Events "

        #region " Methods/Procedures/Functions "
        // ---------------------------------------------------------------------------------------------------------------------------------------
        // added by chang on 20110425
        // use to set the bar tree view base on the access rights
        private void SetBarTreeView()
        {
            try
            {
                // for reference, if wanna know Autocount's AccessRights CmdID then just un-remark below code
                // '' ''Dim dt_AccessRights As DataTable = Autocount.Authentication.AccessRightMap.GetAccessRightMapTable()

                //Master Portion
                //==============
                ////check user has <Master 01> List SHOW rights
                //Mas_NavBarItem01.Visible = CheckPermissions(CMD_SHOW_MCHMAS, myDBSetting);
                ////check user has <Master 02> List SHOW rights
                //Mas_NavBarItem02.Visible = CheckPermissions(CMD_SHOW_BITMAS, myDBSetting);
                ////check user has <Master 03> List SHOW rights
                //Mas_NavBarItem03.Visible = CheckPermissions("CMD_OPEN_ONE", myDBSetting)

                // Operation Portion
                // =================
                //// check user has <Operation 01> List SHOW rights
                //Opr_NavBarItem01.Visible = CheckPermissions(CMD_OPEN_INSTMT, myDBSetting);

                //// check user has <Operation 02> List SHOW rights
                //Opr_NavBarItem02.Visible = CheckPermissions(CMD_OPEN_INSTMT, myDBSetting);

                //// check user has <Operation 03> List SHOW rights
                //Opr_NavBarItem03.Visible = CheckPermissions(CMD_OPEN_INSTMT, myDBSetting);

                //// check user has <Operation 04> List SHOW rights
                //Opr_NavBarItem04.Visible = CheckPermissions(CMD_DUMMY, myDBSetting);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + ".SetBarTreeView()", ex);
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------

        // ---------------------------------------------------------------------------------------------------------------------------------------
        // added by chang on 20110425
        // use to set the bar tree view base on the access rights
        private void SetMenuBarView()
        {
            try
            {
                //Master Portion
                //==============
                ////check user has <Master 01> List SHOW rights
                //Mas_BarButtonItem01.Visibility = (DevExpress.XtraBars.BarItemVisibility)(CheckPermissions(CMD_SHOW_MCHMAS, myDBSetting) ? 0 : 1);
                ////check user has <Master 02> List SHOW rights
                //Mas_BarButtonItem02.Visibility = (DevExpress.XtraBars.BarItemVisibility)(CheckPermissions(CMD_SHOW_BITMAS, myDBSetting) ? 0 : 1);
                ////check user has <Master 03> List SHOW rights
                //Mas_BarButtonItem03.Visibility = IIf(CheckPermissions("STK_ITEM_SHOW", myDBSetting), 0, 1)

                // Operation Portion
                // =================
                //// check user has <Operation 01> List SHOW rights
                //Opr_BarButtonItem01.Visibility = (DevExpress.XtraBars.BarItemVisibility)(CheckPermissions(CMD_OPEN_INSTMT, myDBSetting) ? 0 : 1);

                //// check user has <Operation 02> List SHOW rights
                //Opr_BarButtonItem02.Visibility = (DevExpress.XtraBars.BarItemVisibility)(CheckPermissions(CMD_OPEN_INSTMT, myDBSetting) ? 0 : 1);

                //// check user has <Operation 03> List SHOW rights
                //Opr_BarButtonItem03.Visibility = (DevExpress.XtraBars.BarItemVisibility)(CheckPermissions(CMD_OPEN_INSTMT, myDBSetting) ? 0 : 1);

                //// check user has <Operation 04> List SHOW rights
                //Opr_BarButtonItem04.Visibility = (DevExpress.XtraBars.BarItemVisibility)(CheckPermissions(CMD_DUMMY, myDBSetting) ? 0 : 1);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + ".SetMenuBarView()", ex);
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------

        // use to load called form(s)
        private void LoadForm(string str_FormName, string str_CmdID = "") // edited by chang on 20110425: added str_CmdID param list
        {
            // check for the user permissions
            if (str_CmdID != "")
            {
                //if (!CheckPermissions(str_CmdID, myDBSetting))
                //{
                //    AutoCount.AppMessage.ShowInformationMessage(this, "You don't have the Access Rights for this module.");

                //    return;
                //}
            }

            // checking if found active form then just exit sub
            if (ActivateFoundForm(str_FormName)) return;

            // declare a form obj
            System.Windows.Forms.Form frm = null;

            // base on the form name to assigns the form obj
            switch (str_FormName)
            {
                case "InstallmentLst_Form":
                    {
                        // to open the Installment Detail List Form
                        //frm = new InstallmentLst_Form();
                        break;
                    }

                case "InstallmentSummLst_Form":
                    {
                        // to open the Installment Summary Report Form
                        //frm = new InstallmentSummLst_Form();
                        break;
                    }

                case "ImpXls2ITLst_Form":
                    {
                        // to open the Installment Summary Report Form
                        //frm = new ImpXls2ITLst_Form();
                        break;
                    }

                case "Setting_Form":
                    {
                        // to open the Setting Form
                        //frm = new Setting_Form();
                        break;
                    }

                default:
                    {
                        // to open Dummy Form if got any mistaken tag infor..
                        frm = new System.Windows.Forms.Form();
                        break;
                    }
            }

            // set mdi parent form (this form) of the called form and show form(s)
            frm.MdiParent = this;
            frm.Show();
        }

        // use to find out the form opened and activate it back
        private bool ActivateFoundForm(string str_FormName)
        {
            bool boo_rtn = false;

            Form frm_chk = FindDocument(str_FormName);
            if (frm_chk != null)
            {
                frm_chk.Activate();

                boo_rtn = true;
            }

            return boo_rtn;
        }

        // use for find the form opened base on the form name
        private Form FindDocument(string str_WindowName)
        {
            str_WindowName = str_WindowName.ToUpper();

            foreach (Form form in MdiChildren)
            {
                if (form.Name.ToUpper() == str_WindowName) return form;
            }

            return null;
        }
        #endregion //" Methods/Procedures/Functions "
    }
}
