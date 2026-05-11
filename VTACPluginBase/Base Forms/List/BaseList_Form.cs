using System;
using System.Collections;
// edited by SCChang's Copilot on 20251123: v1.0.0.0, added for List<T>
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

// edited by SCChang's Copilot on 20251123: v1.0.0.0, removed dependency on Microsoft.VisualBasic
// using Microsoft.VisualBasic;

using VTACPluginBase.BaseForms.Editor;
using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.TextLogger;
using VTACPluginBase.CommonForms;

using static VTACPluginBase.BaseForms.FormHelper;
using static VTACPluginBase.Classes.Helpers.AutoCountHelper;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.BaseForms.List
{
    // edited by SCChang's Copilot on 20251124: removed abstract for Visual Inheritance in Designer
    // abstract class cannot be properly displayed in VS Designer
    //[TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<BaseList_Form, Form>))]
    public partial class BaseList_Form : Form
    {
        #region " Form Constructor "
        // edited by SCChang's Copilot on 20251124: uncommented InitializeComponent
        // for standard Visual Inheritance - base form initializes all controls
        // edited by SCChang's Copilot on 20251126: added Design-Time check
        // Skip full initialization in Designer to prevent NullReferenceException
        protected BaseList_Form()
        {
            // If in Design Time, only do minimal initialization
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                InitializeComponent();
                return; // Skip everything else in Design-Time
            }
            
            // Runtime: full initialization
            InitializeComponent();
        }
        #endregion " Form Constructor "

        #region " Variables/Constants declaration "
        // edited by SCChang's Copilot on 20251124: changed to virtual for non-abstract class
        // for editor form
        protected virtual string EditorFormClass { get; set; } = "";

        // for data
        protected virtual string TableName { get; set; } = "";
        protected virtual string TableQueryName { get; set; } = "";
        protected virtual string DetailTableName { get; set; } = "";
        protected virtual string DetailTableQueryName { get; set; } = "";
        #endregion " Variables/Constants declaration "

        #region " Form Properties "
        #region " Required Access Rights CMD "
        // edited by SCChang's Copilot on 20251124: changed to virtual for Visual Inheritance
        protected virtual string NewDocAccessRightsCMD { get; set; } = "";
        // edited by SCChang's Copilot on 20251126: added Design-Time check to prevent NullReferenceException
        // PropertyGrid/TypeDescriptor may access these properties in Designer, return false if in Design-Time
        protected bool NewDocPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(NewDocAccessRightsCMD); }
        protected virtual string EditDocAccessRightsCMD { get; set; } = "";
        protected bool EditDocPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(EditDocAccessRightsCMD); }
        protected virtual string ViewDocAccessRightsCMD { get; set; } = "";
        protected bool ViewDocPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(ViewDocAccessRightsCMD); }
        protected virtual string PrintPreviewAccessRightsCMD { get; set; } = "";
        protected bool PrintPreviewPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(PrintPreviewAccessRightsCMD); }
        protected virtual string PrintAccessRightsCMD { get; set; } = "";
        protected bool PrintPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(PrintAccessRightsCMD); }
        protected virtual string PrintDesignAccessRightsCMD { get; set; } = "";
        protected bool PrintDesignPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(PrintDesignAccessRightsCMD); }
        protected virtual string DeleteDocAccessRightsCMD { get; set; } = "";
        protected bool DeleteDocPermission { get => (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) ? false : CheckPermissions(DeleteDocAccessRightsCMD); }
        #endregion " Required Access Rights CMD "

        #region " Required Properties "
        // edited by SCChang's Copilot on 20251124: changed to virtual for Visual Inheritance
        protected virtual Type BusinessObjectType { get; set; } = null;
        #endregion " Required Properties "

        #region " Filtering Options "
        protected virtual string SQLStrLookUpEdtFilter01 { get; } = "select AccNo as DebtorCode, CompanyName as DebtorName from Debtor where IsActive='T' union select distinct DebtorCode, DebtorName from zINS_Installment order by DebtorCode ";
        protected virtual string SQLStrLookUpEdtFilter02 { get; } = "select DebtorType, Description from DebtorType where IsActive='T' union select U1DebtorType as DebtorType, isnull((select Description from DebtorType where DebtorType=inst.U1DebtorType),'') as Description from zINS_Installment inst order by DebtorType ";
        protected virtual string SQLStrLookUpEdtFilter03 { get; } = "select AreaCode, Description from Area union select AreaCode, isnull((select Description from Area where AreaCode=inst.AreaCode),'') as Description from zINS_Installment inst order by AreaCode ";

        protected virtual string SQLStrHyperLinkFilter01 { get; } = "select CONVERT(bit,0) AS Selected, AccNo as DebtorCode, CompanyName as DebtorName from Debtor where IsActive='T' union select distinct CONVERT(bit,0) AS Selected, DebtorCode, DebtorName from zINS_Installment order by DebtorCode ";
        protected virtual string SQLStrHyperLinkFilter01Key1 { get; } = "DebtorCode";
        protected virtual string SQLStrHyperLinkFilter02 { get; } = "select CONVERT(bit,0) AS Selected, DebtorType, Description from DebtorType where IsActive='T' union select CONVERT(bit,0) AS Selected, U1DebtorType as DebtorType, isnull((select Description from DebtorType where DebtorType=inst.U1DebtorType),'') as Description from zINS_Installment inst order by DebtorType ";
        protected virtual string SQLStrHyperLinkFilter02Key1 { get; } = "DebtorType";
        protected virtual string SQLStrHyperLinkFilter03 { get; } = "select CONVERT(bit,0) AS Selected, AreaCode, Description from Area union select CONVERT(bit,0) AS Selected, AreaCode, isnull((select Description from Area where AreaCode=inst.AreaCode),'') as Description from zINS_Installment inst order by AreaCode ";
        protected virtual string SQLStrHyperLinkFilter03Key1 { get; } = "AreaCode";
        #endregion " Filtering Options "
        #endregion " Form Properties "

        #region " Form Events "
        protected virtual void BaseList_Form_Load(System.Object sender, System.EventArgs e)
        {
            // edited by SCChang's Copilot on 20251126: added Design-Time check
            // Designer triggers Load event during InitializeComponent(), skip initialization in Design-Time
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;
                
            //to init form
            InitForm();
        }

        protected virtual void BaseList_Form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            // to save grid control layout
            SaveControlsLayout(this.Controls);

            // set refresh grid timer OFF
            this.RefreshGrid_Tmr.Enabled = false;
        }
        #endregion //" Form Events "

        #region " Timer Events "
        protected virtual void RefreshGrid_Tmr_Tick(System.Object sender, System.EventArgs e)
        {
            this.RefreshGrid_Tmr.Enabled = false;

            // edited by SCChang's Copilot on 20251123: v1.0.0.0, refactored to use List<T> and simplified logic
            /*
            if (this.Lstfrms_opened.Count > 0)
            {
                ArrayList arrl_frmremoveidx = new ArrayList();
                for (int int_idx = 0; int_idx <= Lstfrms_opened.Count - 1; int_idx++)
                {
                    BaseEditor_Form frm_edit = (BaseEditor_Form)Lstfrms_opened[int_idx];

                    // if edit form has been modified and saved, then refresh the grid
                    if (frm_edit.FormIsSaved) Refresh_SBtn_Click(this, new System.EventArgs());

                    if (frm_edit.IsFormClosing || frm_edit.IsDisposed) arrl_frmremoveidx.Add(int_idx);
                }

                if (arrl_frmremoveidx.Count > 0)
                {
                    for (int int_idx2 = arrl_frmremoveidx.Count - 1; int_idx2 >= 0; int_idx2--)
                        this.Lstfrms_opened.RemoveAt((int)arrl_frmremoveidx[int_idx2]);
                }

                // release memory
                arrl_frmremoveidx = null;
            }
            else if (this.Lstfrms_opened.Count == 0) this.Lstfrms_opened.Clear();
            */

            // edited by SCChang's Copilot on 20251123: v1.0.0.0, optimized to prevent multiple grid refreshes
            if (this.Lstfrms_opened.Count > 0)
            {
                // Check if any form has been saved (only refresh once)
                bool needRefresh = false;
                foreach (var frm_edit in this.Lstfrms_opened)
                {
                    if (frm_edit != null && !frm_edit.IsDisposed && frm_edit.FormIsSaved)
                    {
                        needRefresh = true;
                        break; // found one, no need to continue checking
                    }
                }

                // Refresh grid only once if needed
                if (needRefresh)
                {
                    Refresh_SBtn_Click(this, new System.EventArgs());
                }

                // Remove closed or disposed forms
                this.Lstfrms_opened.RemoveAll(frm => frm == null || frm.IsFormClosing || frm.IsDisposed);
            }

            this.RefreshGrid_Tmr.Enabled = true;
        }
        #endregion //" Timer Events "

        #region " DataGrid Events "
        protected virtual void DataList_DXGridVw_DoubleClick(System.Object sender, System.EventArgs e)
        {
            try
            {
                //OpenForm(Common_Cls.FormMethods.VIEW); // follow Autocount method, show as view mode
                OpenForm(FormMethods.VIEW); // follow Autocount method, show as view mode
            }

            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage(this, ex.Message);
                ErrorLogger_Cls.Write(this.Name + $".{nameof(DataList_DXGridVw_DoubleClick)}()", ex); //added by chang on 20211214: to log something in error log
            }
        }
        #endregion //" DataGrid Events "

        #region " Simple Buttons Events "
        protected virtual void Refresh_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // checking the filtering options validation, if not valid then do nothing
            if (CheckingDataFilling()) return;

            // here to load the data into the datagrid
            AssignGridControlDataSource();
        }

        protected virtual void Edit_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            //// open edit form base on the form's methods
            //OpenForm(Common_Cls.FormMethods.EDIT);
            // open edit form base on the form's methods
            OpenForm(FormMethods.EDIT);
        }

        protected virtual void View_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            //// open view form base on the form's methods
            //OpenForm(Common_Cls.FormMethods.VIEW);
            // open view form base on the form's methods
            OpenForm(FormMethods.VIEW);
        }

        protected virtual void Delete_SBtn_Click(object sender, System.EventArgs e)
        {
            //if (this.DataList_DXGrid.DataSource != null)
            //{
            //    if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
            //    {
            //        DataRow dr_focused = this.DataList_DXGridVw.GetDataRow(this.DataList_DXGridVw.FocusedRowHandle);

            //        Installment_Form frm_Doc = FindOpenedForm((long)dr_focused["DocKey"]);
            //        if (frm_Doc == null) frm_Doc = new Installment_Form(FormMethods.DELETE, (long)dr_focused["DocKey"]);

            //        frm_Doc.Delete_BarBtn_ItemClick(null, null);

            //        if (frm_Doc.IsDataSaved) Refresh_SBtn_Click(this, new System.EventArgs());

            //        if (!frm_Doc.ShowInTaskbar)
            //        {
            //            frm_Doc.Close();
            //            if (frm_Doc != null)
            //            {
            //                frm_Doc.Dispose();
            //                frm_Doc = null;
            //            }
            //        }
            //        else frm_Doc.BringToFront();
            //    }
            //}

            if (this.DataList_DXGrid.DataSource != null)
            {
                if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
                {
                    DataRow dr_focused = this.DataList_DXGridVw.GetDataRow(this.DataList_DXGridVw.FocusedRowHandle);
                    long docKey = (long)dr_focused["DocKey"];

                    BaseEditor_Form frm_Doc = FindOpenedForm(docKey);
                    if (frm_Doc == null)
                    {
                        object[] args = new object[] { Enum.ToObject(typeof(FormMethods), FormMethods.DELETE), docKey };
                        
                        // edited by SCChang's Gemini 3 Pro on 20260205: Fix cross-assembly instantiation
                        // frm_Doc = (BaseEditor_Form)System.Reflection.Assembly.GetCallingAssembly().CreateInstance(EditorFormClass, false, System.Reflection.BindingFlags.Default, null, args, null, null);
                        
                        frm_Doc = CreateEditorInstance(args);
                    }

                    if (frm_Doc != null)
                    {
                        frm_Doc.Delete_BarBtn_ItemClick(null, null);

                        if (frm_Doc.FormIsSaved) Refresh_SBtn_Click(this, new System.EventArgs());

                        if (!frm_Doc.ShowInTaskbar)
                        {
                            frm_Doc.Close();
                            if (frm_Doc != null)
                            {
                                frm_Doc.Dispose();
                                frm_Doc = null;
                            }
                        }
                        else frm_Doc.BringToFront();
                    }
                }
            }
        }

        // use to open 'NEW' Edit Form
        protected virtual void NewDoc_HyperLinkEdit_OpenLink(System.Object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            //OpenForm(Common_Cls.FormMethods.NEW);
            OpenForm(FormMethods.NEW);
        }
        #endregion //" Simple Buttons Events "

        #region " Export Tool Strip Menu Item Event "
        // export data grid to EXCEL format
        protected void ExportToEXCELToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xls";
            saveFileDialog.Filter = "(*.xls)|*.xls";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Trim() != "")
            {
                string str_Filename = saveFileDialog.FileName;

                str_Filename = str_Filename.Substring(str_Filename.IndexOf("."), str_Filename.Length - str_Filename.IndexOf("."));
                if (str_Filename.ToUpper() == ".XLS")
                {
                    DataList_DXGridVw.ExportToXls(saveFileDialog.FileName);

                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    if (Interaction.MsgBox("Do you want to open the file?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Open File") == MsgBoxResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                        // Proc.StartInfo.WorkingDirectory = "C:\"
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                    */
                    if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                }
                else
                {
                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    Interaction.MsgBox("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", MsgBoxStyle.OkOnly, "Abort Export operation");
                    */
                    MessageBox.Show("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", "Abort Export operation", MessageBoxButtons.OK);
                }
            }
        }

        // export data grid to PDF format
        protected void ExportToPDFToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".pdf";
            saveFileDialog.Filter = "(*.pdf)|*.pdf";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Trim() != "")
            {
                string str_Filename = saveFileDialog.FileName;

                str_Filename = str_Filename.Substring(str_Filename.IndexOf("."), str_Filename.Length - str_Filename.IndexOf("."));
                if (str_Filename.ToUpper() == ".PDF")
                {
                    DataList_DXGridVw.ExportToPdf(saveFileDialog.FileName);

                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    if (Interaction.MsgBox("Do you want to open the file?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Open File") == MsgBoxResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                        // Proc.StartInfo.WorkingDirectory = "C:\"
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                    */
                    if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                }
                else
                {
                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    Interaction.MsgBox("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", MsgBoxStyle.OkOnly, "Abort Export operation");
                    */
                    MessageBox.Show("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", "Abort Export operation", MessageBoxButtons.OK);
                }
            }
        }

        // export data grid to HTML format
        protected void ExportToHTMLToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".html";
            saveFileDialog.Filter = "HTML files (*.html)|*.html";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Trim() != "")
            {
                string str_Filename = saveFileDialog.FileName;

                str_Filename = str_Filename.Substring(str_Filename.IndexOf("."), str_Filename.Length - str_Filename.IndexOf("."));
                if (str_Filename.ToUpper() == ".HTML")
                {
                    DataList_DXGridVw.ExportToHtml(saveFileDialog.FileName);

                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    if (Interaction.MsgBox("Do you want to open the file?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Open File") == MsgBoxResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                    */
                    if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                }
                else
                {
                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    Interaction.MsgBox("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", MsgBoxStyle.OkOnly, "Abort Export operation");
                    */
                    MessageBox.Show("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", "Abort Export operation", MessageBoxButtons.OK);
                }
            }
        }

        // export data grid to MHT format
        protected void ExportToMHTToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".mht";
            saveFileDialog.Filter = "MHT files (*.mht)|*.mht";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Trim() != "")
            {
                string str_Filename = saveFileDialog.FileName;

                str_Filename = str_Filename.Substring(str_Filename.IndexOf("."), str_Filename.Length - str_Filename.IndexOf("."));
                if (str_Filename.ToUpper() == ".MHT")
                {
                    DataList_DXGridVw.ExportToMht(saveFileDialog.FileName);

                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    if (Interaction.MsgBox("Do you want to open the file?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Open File") == MsgBoxResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                    */
                    if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                }
                else
                {
                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    Interaction.MsgBox("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", MsgBoxStyle.OkOnly, "Abort Export operation");
                    */
                    MessageBox.Show("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", "Abort Export operation", MessageBoxButtons.OK);
                }
            }
        }

        // export data grid to RTF format
        protected void ExportToRTFToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".rtf";
            saveFileDialog.Filter = "RTF files (*.rtf)|*.rtf";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Trim() != "")
            {
                string str_Filename = saveFileDialog.FileName;

                str_Filename = str_Filename.Substring(str_Filename.IndexOf("."), str_Filename.Length - str_Filename.IndexOf("."));
                if (str_Filename.ToUpper() == ".RTF")
                {
                    DataList_DXGridVw.ExportToRtf(saveFileDialog.FileName);

                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    if (Interaction.MsgBox("Do you want to open the file?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Open File") == MsgBoxResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                    */
                    if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                }
                else
                {
                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    Interaction.MsgBox("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", MsgBoxStyle.OkOnly, "Abort Export operation");
                    */
                    MessageBox.Show("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", "Abort Export operation", MessageBoxButtons.OK);
                }
            }
        }

        // export data grid to TEXT format
        protected void ExportToTEXTToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName.Trim() != "")
            {
                string str_Filename = saveFileDialog.FileName;

                str_Filename = str_Filename.Substring(str_Filename.IndexOf("."), str_Filename.Length - str_Filename.IndexOf("."));
                if (str_Filename.ToUpper() == ".TXT")
                {
                    DataList_DXGridVw.ExportToText(saveFileDialog.FileName);

                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    if (Interaction.MsgBox("Do you want to open the file?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Open File") == MsgBoxResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();

                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                    */
                    if (MessageBox.Show("Do you want to open the file?", "Open File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process Proc = new System.Diagnostics.Process();
                        Proc.StartInfo.FileName = saveFileDialog.FileName;
                        Proc.Start();
                    }
                }
                else
                {
                    // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Interaction.MsgBox with MessageBox.Show
                    /*
                    Interaction.MsgBox("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", MsgBoxStyle.OkOnly, "Abort Export operation");
                    */
                    MessageBox.Show("The file you trying to open " + saveFileDialog.FileName + " , is in different format than specified by the file extension. Operation Export will be abort. ", "Abort Export operation", MessageBoxButtons.OK);
                }
            }
        }
        #endregion //" Export Tool Strip Menu Item Event "

        #region " Repository Item Control Events "
        #endregion " Repository Item Control Events "

        #region " Methods/Procedures/Functions "
        
        /// <summary>
        /// Configure Filter UI labels and visibility
        /// Override this method in derived forms to customize filter UI layout
        /// </summary>
        protected virtual void ConfigureFilterUI()
        {
            // Base implementation does nothing
            // Derived forms can override this to configure filter labels, positions, etc.
        }

        /// <summary>
        /// Configure GridView columns
        /// Override this method in derived forms to customize grid columns
        /// </summary>
        protected virtual void ConfigureGridColumns()
        {
            // Base implementation does nothing
            // Derived forms can override this to add/configure grid columns
        }

        /// <summary>
        /// Configure display formats for grid columns
        /// Override this method in derived forms to customize column formats (numeric, date, etc.)
        /// </summary>
        protected virtual void ConfigureColumnFormats()
        {
            // Base implementation does nothing
            // Derived forms can override this to set DisplayFormat for columns
        }

        /// <summary>
        /// Configure conditional formatting for the grid
        /// Override this method in derived forms to add conditional formatting rules
        /// </summary>
        protected virtual void ConfigureConditionalFormatting()
        {
            // Base implementation does nothing
            // Derived forms can override this to add FormatConditions
        }

        protected virtual void InitForm()
        {
            //to assign the BusinessObject for report use
            // edited by SCChang's Copilot on 20251125: support View-based forms without BusinessObject
            if (BusinessObjectType != null)
            {
                object tempBusinessObject = BusinessObjectType.Assembly.CreateInstance(BusinessObjectType.FullName);
                if (tempBusinessObject.GetType().BaseType == typeof(BusinessBase_Cls)) BusinessObject = (BusinessBase_Cls)tempBusinessObject;
            }

            // set refresh grid timer ON
            this.RefreshGrid_Tmr.Enabled = true;

            // ''''when form load, set default date range to load records
            // '''ReceivingDate_CBox.SelectedItem = "Filter by Range"

            // here to load the data into the datagrid
            AssignGridControlDataSource("FormLoad");

            // to load saved Grid Control layout
            LoadControlsLayout(this.Controls);

            SetManipulateDataPermissions(); // added by chang on 20110425

            // Configure filter UI (added by SCChang's Copilot on 20251203)
            ConfigureFilterUI();

            // Configure grid columns (added by SCChang's Copilot on 20251203)
            ConfigureGridColumns();

            // Configure column formats (added by SCChang's Copilot on 20251203)
            ConfigureColumnFormats();

            // Configure conditional formatting (added by SCChang's Copilot on 20251203)
            ConfigureConditionalFormatting();

            // to set the visibility of filter controls
            ControlVisibility("All", false);
        }

        // added by chang on 20110425: use to set the New/Edit/View/Delete permissions
        protected virtual void SetManipulateDataPermissions()
        {
            NewDoc_HyperLinkEdit.Enabled = NewDocPermission; // create new record permission
            Edit_SBtn.Enabled = EditDocPermission; // edit record permission
            View_SBtn.Enabled = ViewDocPermission; // view record permission
            PreviewListing_SBtn.Enabled = PrintPreviewPermission; // preview report permission
            PrintListing_SBtn.Enabled = PrintPermission; // print report permission
            DesignReportListing_SBtn.Enabled = PrintDesignPermission; // design report permission
            Delete_SBtn.Enabled = DeleteDocPermission; // delete record permission

            NewToolStripMenuItem.Enabled = NewDoc_HyperLinkEdit.Enabled; // create new record permission
            EditToolStripMenuItem.Enabled = Edit_SBtn.Enabled; // edit record permission
            ViewToolStripMenuItem.Enabled = View_SBtn.Enabled; // view record permission
            PreviewToolStripMenuItem.Enabled = PreviewListing_SBtn.Enabled; // preview report permission
            PrintToolStripMenuItem.Enabled = PrintListing_SBtn.Enabled; // print report permission
            DesignReportToolStripMenuItem.Enabled = DesignReportListing_SBtn.Enabled; // design report permission
            DeleteToolStripMenuItem.Enabled = Delete_SBtn.Enabled; // delete record permission
        }

        // edited by SCChang's Copilot on 20251123: v1.0.0.0, refactored to use List<T> instead of ArrayList
        /*
        protected ArrayList Lstfrms_opened = new ArrayList();
        */
        protected List<BaseEditor_Form> Lstfrms_opened = new List<BaseEditor_Form>();
        protected virtual void OpenForm(FormMethods enm_FormMethods)
        {
            //Installment_Form frm_edit = null;
            BaseEditor_Form frm_edit = null;

            switch (enm_FormMethods)
            {
                case FormMethods.NEW:
                    {
                        object[] args = new object[] { Enum.ToObject(typeof(FormMethods), enm_FormMethods) };

                        // edited by SCChang's Gemini 3 Pro on 20260205: Fix cross-assembly instantiation
                        // frm_edit = (BaseEditor_Form)System.Reflection.Assembly.GetCallingAssembly().CreateInstance(EditorFormClass, false, System.Reflection.BindingFlags.Default, null, args, null, null);
                        
                        frm_edit = CreateEditorInstance(args);

                        if (frm_edit != null)
                        {
                            frm_edit.WindowState = FormWindowState.Normal;
                            // [Updated] by scchang's Gemini 3 Pro on 20260205: Ensure modeless show
                            frm_edit.Show();
                        }
                        break;
                    }

                case FormMethods.EDIT:
                case FormMethods.VIEW:
                    {
                        if (this.DataList_DXGrid.DataSource != null)
                        {
                            if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
                            {
                                DataRow dr_focused = this.DataList_DXGridVw.GetDataRow(this.DataList_DXGridVw.FocusedRowHandle);

                                frm_edit = FindOpenedForm((long)dr_focused["DocKey"]);

                                if (frm_edit == null)
                                {
                                    object[] args = new object[] { Enum.ToObject(typeof(FormMethods), enm_FormMethods), (long)dr_focused["DocKey"] };
                                    
                                    // edited by SCChang's Gemini 3 Pro on 20260205: Fix cross-assembly instantiation
                                    // frm_edit = (BaseEditor_Form)System.Reflection.Assembly.GetCallingAssembly().CreateInstance(EditorFormClass, false, System.Reflection.BindingFlags.Default, null, args, null, null);
                                    
                                    frm_edit = CreateEditorInstance(args);

                                    if (frm_edit != null)
                                    {
                                        frm_edit.WindowState = FormWindowState.Normal;
                                        frm_edit.Show();
                                    }
                                }
                                else
                                {
                                    frm_edit.BringToFront();
                                }
                            }
                        }

                        break;
                    }

                case FormMethods.DELETE:
                    {
                        break;
                    }
            }

            if (frm_edit != null && !Lstfrms_opened.Contains(frm_edit)) Lstfrms_opened.Add(frm_edit);
        }

        protected virtual BaseEditor_Form FindOpenedForm(long DocKey)
        {
            BaseEditor_Form frm_rtn = null;

            // edited by SCChang's Copilot on 20251123: v1.0.0.0, refactored to use List<T> and fixed logic bug
            /*
            if (Lstfrms_opened.Count > 0)
            {
                foreach (BaseEditor_Form frm in Lstfrms_opened)
                {
                    //frm_rtn = (Installment_Form)frm;
                    if (frm_rtn.DocKey == DocKey) break;
                    frm_rtn = null;
                }
            }
            */

            if (Lstfrms_opened.Count > 0)
            {
                foreach (BaseEditor_Form frm in Lstfrms_opened)
                {
                    if (frm.DocKey == DocKey)
                    {
                        frm_rtn = frm;
                        break;
                    }
                }
            }

            return frm_rtn;
        }

        // [added] by scchang's Gemini 3 Pro on 20260205: Helper to create instance across assemblies
        private BaseEditor_Form CreateEditorInstance(object[] args)
        {
            try 
            {
                if (string.IsNullOrEmpty(EditorFormClass)) return null;

                Type type = Type.GetType(EditorFormClass);
                if (type == null)
                {
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = asm.GetType(EditorFormClass);
                        if (type != null) break;
                    }
                }

                if (type != null)
                {
                     return (BaseEditor_Form)Activator.CreateInstance(type, args);
                }
            }
            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage("Error creating form instance: " + ex.Message);
            }
            return null;
        }

        protected virtual bool CheckingDataFilling()
        {
            bool boo_rtn = false;
            string str_warning = "";
            string str_rtn = "";

            try
            {
            }
            catch (Exception ex)
            {
                // edited by SCChang's Copilot on 20251123: v1.0.0.0, replaced Strings.Chr(10) with Environment.NewLine
                /*
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: " + Strings.Chr(10) + ex.Message + Strings.Chr(10) + ex.StackTrace);
                */
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                ErrorLogger_Cls.Write(this.Name + $".{nameof(CheckingDataFilling)}()", ex);
            }

            return boo_rtn;
        }

        // [updated] by scchang's Gemini 3 Pro on 20260203: v2.2.1.0, Refactored to support dynamic Key names from derived classes
        protected virtual void ControlVisibility(string str_Control, bool boo_visible, bool boo_HyperLinkVisible = false)
        {
            string opKey = str_Control.ToUpper();

            if (opKey == "ALL")
            {
                // Filter01 From
                LabelFilter01From_LblCtrl.Visible = boo_visible;
                LookUpEdtFilter01From_LUEdit.Visible = boo_visible;
                // Filter01 To
                LabelFilter01To_LblCtrl.Visible = boo_visible;
                LookUpEdtFilter01To_LUEdit.Visible = boo_visible;
                // Filter01 HyperLink
                HyperLinkFilter01_HyperLinkEdit.Visible = boo_HyperLinkVisible;
                HyperLinkFilter01_HyperLinkEdit.Enabled = HyperLinkFilter01_HyperLinkEdit.Visible;

                // Filter02 From
                LabelFilter02From_LblCtrl.Visible = boo_visible;
                LookUpEdtFilter02From_LUEdit.Visible = boo_visible;
                // Filter02 To
                LabelFilter02To_LblCtrl.Visible = boo_visible;
                LookUpEdtFilter02To_LUEdit.Visible = boo_visible;
                // Filter02 HyperLink
                HyperLinkFilter02_HyperLinkEdit.Visible = boo_HyperLinkVisible;
                HyperLinkFilter02_HyperLinkEdit.Enabled = HyperLinkFilter02_HyperLinkEdit.Visible;

                // Filter03 From
                LabelFilter03From_LblCtrl.Visible = boo_visible;
                LookUpEdtFilter03From_LUEdit.Visible = boo_visible;
                // Filter03 To
                LabelFilter03To_LblCtrl.Visible = boo_visible;
                LookUpEdtFilter03To_LUEdit.Visible = boo_visible;
                // Filter03 HyperLink
                HyperLinkFilter03_HyperLinkEdit.Visible = boo_HyperLinkVisible;
                HyperLinkFilter03_HyperLinkEdit.Enabled = HyperLinkFilter03_HyperLinkEdit.Visible;
            }
            else if (opKey == SQLStrHyperLinkFilter01Key1.ToUpper())
            {
                // Filter01 From
                LabelFilter01From_LblCtrl.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter01From_LUEdit.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter01From_LUEdit.Properties.DisplayMember = SQLStrHyperLinkFilter01Key1;
                LookUpEdtFilter01From_LUEdit.Properties.ValueMember = SQLStrHyperLinkFilter01Key1;
                // Filter01 To
                LabelFilter01To_LblCtrl.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter01To_LUEdit.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter01To_LUEdit.Properties.DisplayMember = SQLStrHyperLinkFilter01Key1;
                LookUpEdtFilter01To_LUEdit.Properties.ValueMember = SQLStrHyperLinkFilter01Key1;
                // Filter01 HyperLink
                HyperLinkFilter01_HyperLinkEdit.Visible = boo_HyperLinkVisible;
                HyperLinkFilter01_HyperLinkEdit.Enabled = HyperLinkFilter01_HyperLinkEdit.Visible;
            }
            else if (opKey == SQLStrHyperLinkFilter02Key1.ToUpper())
            {
                // Filter02 From
                LabelFilter02From_LblCtrl.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter02From_LUEdit.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter02From_LUEdit.Properties.DisplayMember = SQLStrHyperLinkFilter02Key1;
                LookUpEdtFilter02From_LUEdit.Properties.ValueMember = SQLStrHyperLinkFilter02Key1;
                // Filter02 To
                LabelFilter02To_LblCtrl.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter02To_LUEdit.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter02To_LUEdit.Properties.DisplayMember = SQLStrHyperLinkFilter02Key1;
                LookUpEdtFilter02To_LUEdit.Properties.ValueMember = SQLStrHyperLinkFilter02Key1;
                // Filter02 HyperLink
                HyperLinkFilter02_HyperLinkEdit.Visible = boo_HyperLinkVisible;
                HyperLinkFilter02_HyperLinkEdit.Enabled = HyperLinkFilter02_HyperLinkEdit.Visible;
            }
            else if (opKey == SQLStrHyperLinkFilter03Key1.ToUpper())
            {
                // Filter03 From
                LabelFilter03From_LblCtrl.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter03From_LUEdit.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter03From_LUEdit.Properties.DisplayMember = SQLStrHyperLinkFilter03Key1;
                LookUpEdtFilter03From_LUEdit.Properties.ValueMember = SQLStrHyperLinkFilter03Key1;
                // Filter03 To
                LabelFilter03To_LblCtrl.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter03To_LUEdit.Visible = boo_visible && !boo_HyperLinkVisible;
                LookUpEdtFilter03To_LUEdit.Properties.DisplayMember = SQLStrHyperLinkFilter03Key1;
                LookUpEdtFilter03To_LUEdit.Properties.ValueMember = SQLStrHyperLinkFilter03Key1;
                // Filter03 HyperLink
                HyperLinkFilter03_HyperLinkEdit.Visible = boo_HyperLinkVisible;
                HyperLinkFilter03_HyperLinkEdit.Enabled = HyperLinkFilter03_HyperLinkEdit.Visible;
            }
        }
        #endregion //" Methods/Procedures/Functions "

        #region " For Filter Dock Panel "
        // to assigns the DataSource to the listing datagrid
        protected virtual void AssignGridControlDataSource(string str_Action = "")
        {
            // if str_Action has value then base on value to do special filtering
            string str_extrafiltersql = "";
            switch (str_Action)
            {
                case "FormLoad":
                    {
                        break;
                    }
            }

            try
            {
                // to assign extra filter

                // start getting main data to datatable
                DataTable dt_Source = myDBSetting.GetDataTable(GetRecGridControlList(str_extrafiltersql), false);

                // assign datasource into grid control
                this.DataList_DXGrid.DataSource = dt_Source;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + ".AssignGridControlDataSource()", ex);
                AutoCount.AppMessage.ShowErrorMessage(ex.Message);
            }
        }

        // Get the DataSource SQL Select string of DataGrid
        // edited by SCChang's Copilot on 20251125: added optional orderBy parameter, made filter logic dynamic based on combo box settings
        protected virtual string GetRecGridControlList(string str_ExtraFilter = "", string str_OrderBy = "")
        {
            string str_GetRecGridControlList = "Select * From " + TableQueryName + " ";
            string str_Where = "Where ";

            // Filter 01 - Dynamic based on combo box selection
            // ===================================================================================================
            if (CboFilter01_CBox.SelectedItem != null)
            {
                switch (CboFilter01_CBox.SelectedItem.ToString().ToUpper())
                {
                    case "FILTER BY RANGE":
                        {
                            if ((LookUpEdtFilter01From_LUEdit.EditValue != null && LookUpEdtFilter01From_LUEdit.EditValue.ToString() != "") && (LookUpEdtFilter01To_LUEdit.EditValue != null && LookUpEdtFilter01To_LUEdit.EditValue.ToString() != ""))
                                str_Where += SQLStrHyperLinkFilter01Key1 + " Between N'" + SQLString(LookUpEdtFilter01From_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter01To_LUEdit.EditValue.ToString()) + "' And ";
                            else if ((LookUpEdtFilter01From_LUEdit.EditValue != null && LookUpEdtFilter01From_LUEdit.EditValue.ToString() != "") && (LookUpEdtFilter01To_LUEdit.EditValue == null || LookUpEdtFilter01To_LUEdit.EditValue.ToString() == ""))
                                str_Where += SQLStrHyperLinkFilter01Key1 + " Between N'" + SQLString(LookUpEdtFilter01From_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter01From_LUEdit.EditValue.ToString()) + "' And ";
                            else if ((LookUpEdtFilter01From_LUEdit.EditValue == null || LookUpEdtFilter01From_LUEdit.EditValue.ToString() == "") && (LookUpEdtFilter01To_LUEdit.EditValue != null && LookUpEdtFilter01To_LUEdit.EditValue.ToString() != ""))
                                str_Where += SQLStrHyperLinkFilter01Key1 + " Between N'" + SQLString(LookUpEdtFilter01To_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter01To_LUEdit.EditValue.ToString()) + "' And ";
                            break;
                        }
                    case "FILTER BY MULTI-SELECT":
                        {
                            if (Lstr_Filter01s != "")
                                str_Where += Lstr_Filter01s;
                            break;
                        }
                }
            }
            // ===================================================================================================

            // Filter 02 - Dynamic based on combo box selection
            // ===================================================================================================
            if (CboFilter02_CBox.SelectedItem != null)
            {
                switch (CboFilter02_CBox.SelectedItem.ToString().ToUpper())
                {
                    case "FILTER BY RANGE":
                        {
                            if ((LookUpEdtFilter02From_LUEdit.EditValue != null && LookUpEdtFilter02From_LUEdit.EditValue.ToString() != "") && (LookUpEdtFilter02To_LUEdit.EditValue != null && LookUpEdtFilter02To_LUEdit.EditValue.ToString() != ""))
                                str_Where += SQLStrHyperLinkFilter02Key1 + " Between N'" + SQLString(LookUpEdtFilter02From_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter02To_LUEdit.EditValue.ToString()) + "' And ";
                            else if ((LookUpEdtFilter02From_LUEdit.EditValue != null && LookUpEdtFilter02From_LUEdit.EditValue.ToString() != "") && (LookUpEdtFilter02To_LUEdit.EditValue == null || LookUpEdtFilter02To_LUEdit.EditValue.ToString() == ""))
                                str_Where += SQLStrHyperLinkFilter02Key1 + " Between N'" + SQLString(LookUpEdtFilter02From_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter02From_LUEdit.EditValue.ToString()) + "' And ";
                            else if ((LookUpEdtFilter02From_LUEdit.EditValue == null || LookUpEdtFilter02From_LUEdit.EditValue.ToString() == "") && (LookUpEdtFilter02To_LUEdit.EditValue != null && LookUpEdtFilter02To_LUEdit.EditValue.ToString() != ""))
                                str_Where += SQLStrHyperLinkFilter02Key1 + " Between N'" + SQLString(LookUpEdtFilter02To_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter02To_LUEdit.EditValue.ToString()) + "' And ";
                            break;
                        }
                    case "FILTER BY MULTI-SELECT":
                        {
                            if (Lstr_Filter02s != "")
                                str_Where += Lstr_Filter02s;
                            break;
                        }
                }
            }
            // ===================================================================================================

            // Filter 03 - Dynamic based on combo box selection
            // ===================================================================================================
            if (CboFilter03_CBox.SelectedItem != null)
            {
                switch (CboFilter03_CBox.SelectedItem.ToString().ToUpper())
                {
                    case "FILTER BY RANGE":
                        {
                            if ((LookUpEdtFilter03From_LUEdit.EditValue != null && LookUpEdtFilter03From_LUEdit.EditValue.ToString() != "") && (LookUpEdtFilter03To_LUEdit.EditValue != null && LookUpEdtFilter03To_LUEdit.EditValue.ToString() != ""))
                                str_Where += SQLStrHyperLinkFilter03Key1 + " Between N'" + SQLString(LookUpEdtFilter03From_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter03To_LUEdit.EditValue.ToString()) + "' And ";
                            else if ((LookUpEdtFilter03From_LUEdit.EditValue != null && LookUpEdtFilter03From_LUEdit.EditValue.ToString() != "") && (LookUpEdtFilter03To_LUEdit.EditValue == null || LookUpEdtFilter03To_LUEdit.EditValue.ToString() == ""))
                                str_Where += SQLStrHyperLinkFilter03Key1 + " Between N'" + SQLString(LookUpEdtFilter03From_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter03From_LUEdit.EditValue.ToString()) + "' And ";
                            else if ((LookUpEdtFilter03From_LUEdit.EditValue == null || LookUpEdtFilter03From_LUEdit.EditValue.ToString() == "") && (LookUpEdtFilter03To_LUEdit.EditValue != null && LookUpEdtFilter03To_LUEdit.EditValue.ToString() != ""))
                                str_Where += SQLStrHyperLinkFilter03Key1 + " Between N'" + SQLString(LookUpEdtFilter03To_LUEdit.EditValue.ToString()) + "' and N'" + SQLString(LookUpEdtFilter03To_LUEdit.EditValue.ToString()) + "' And ";
                            break;
                        }
                    case "FILTER BY MULTI-SELECT":
                        {
                            if (Lstr_Filter03s != "")
                                str_Where += Lstr_Filter03s;
                            break;
                        }
                }
            }
            // ===================================================================================================

            // add in the extra filter string if got values
            if (str_ExtraFilter != "") str_Where += str_ExtraFilter;

            if (str_Where.Substring(0, str_Where.Length - 6) != "")
            {
                str_Where = str_Where.Substring(0, str_Where.Length - 4);
                str_GetRecGridControlList = str_GetRecGridControlList + str_Where;
            }

            // Add ORDER BY clause if provided
            if (!string.IsNullOrEmpty(str_OrderBy))
            {
                str_GetRecGridControlList += " ORDER BY " + str_OrderBy;
            }

            return str_GetRecGridControlList;
        }

        // hides or shows the Filter Dock Panel
        protected virtual void Find_HyperLinkEdit_Click(object sender, System.EventArgs e)
        {
            // hide and show Find_DockPanel
            if (this.Find_HyperLinkEdit.Text == "Hide " + this.Find_HyperLinkEdit.Tag.ToString() + " Filter Options")
            {
                this.Find_HyperLinkEdit.Text = "Show " + this.Find_HyperLinkEdit.Tag.ToString() + " Filter Options";
                this.FilterOptions_DockPanel.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            }
            else
            {
                this.Find_HyperLinkEdit.Text = "Hide " + this.Find_HyperLinkEdit.Tag.ToString() + " Filter Options";
                this.FilterOptions_DockPanel.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
            }
        }

        #region " Combo Box Events "
        protected virtual void CboFilter01_CBox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            switch (CboFilter01_CBox.SelectedItem.ToString().ToUpper())
            {
                case "NO FILTER":
                    {
                        ControlVisibility(SQLStrHyperLinkFilter01Key1, false);
                        LookUpEdtFilter01From_LUEdit.EditValue = null;
                        LookUpEdtFilter01To_LUEdit.EditValue = null;
                        break;
                    }

                case "FILTER BY RANGE":
                    {
                        ControlVisibility(SQLStrHyperLinkFilter01Key1, true);
                        break;
                    }

                case "FILTER BY MULTI-SELECT":
                    {
                        HyperLinkFilter01_HyperLinkEdit_OpenLink(null, null);
                        break;
                    }
            }
        }

        protected virtual void CboFilter02_CBox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            switch (CboFilter02_CBox.SelectedItem.ToString().ToUpper())
            {
                case "NO FILTER":
                    {
                        ControlVisibility(SQLStrHyperLinkFilter02Key1, false);
                        LookUpEdtFilter02From_LUEdit.EditValue = null;
                        LookUpEdtFilter02To_LUEdit.EditValue = null;
                        break;
                    }

                case "FILTER BY RANGE":
                    {
                        ControlVisibility(SQLStrHyperLinkFilter02Key1, true);
                        break;
                    }

                case "FILTER BY MULTI-SELECT":
                    {
                        HyperLinkFilter02_HyperLinkEdit_OpenLink(null, null);
                        break;
                    }
            }
        }

        protected virtual void CboFilter03_CBox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            switch (CboFilter03_CBox.SelectedItem.ToString().ToUpper())
            {
                case "NO FILTER":
                    {
                        ControlVisibility(SQLStrHyperLinkFilter03Key1, false);
                        LookUpEdtFilter03From_LUEdit.EditValue = null;
                        LookUpEdtFilter03To_LUEdit.EditValue = null;
                        break;
                    }

                case "FILTER BY RANGE":
                    {
                        ControlVisibility(SQLStrHyperLinkFilter03Key1, true);
                        break;
                    }

                case "FILTER BY MULTI-SELECT":
                    {
                        HyperLinkFilter03_HyperLinkEdit_OpenLink(null, null);
                        break;
                    }
            }
        }
        #endregion //" Combo Box Events "

        #region " LookUp Edit Events "
        protected virtual void LookUpEdtFilter01FromTo_LUEdit_Enter(object sender, System.EventArgs e)
        {
            DataTable dt_Filter01 = myDBSetting.GetDataTable(SQLStrLookUpEdtFilter01, false);

            LookUpEdtFilter01From_LUEdit.Properties.DataSource = dt_Filter01;
            LookUpEdtFilter01To_LUEdit.Properties.DataSource = dt_Filter01;
        }

        protected virtual void LookUpEdtFilter02FromTo_LUEdit_Enter(object sender, System.EventArgs e)
        {
            DataTable dt_Filter02 = myDBSetting.GetDataTable(SQLStrLookUpEdtFilter02, false);

            LookUpEdtFilter02From_LUEdit.Properties.DataSource = dt_Filter02;
            LookUpEdtFilter02To_LUEdit.Properties.DataSource = dt_Filter02;
        }

        protected virtual void LookUpEdtFilter03FromTo_LUEdit_Enter(object sender, System.EventArgs e)
        {
            DataTable dt_Filter03 = myDBSetting.GetDataTable(SQLStrLookUpEdtFilter03, false);

            LookUpEdtFilter03From_LUEdit.Properties.DataSource = dt_Filter03;
            LookUpEdtFilter03To_LUEdit.Properties.DataSource = dt_Filter03;
        }
        #endregion //" LookUp Edit Events "

        #region " HyperLink Events "
        protected string Lstr_Filter01s = "";
        protected DataTable Ldt_Filter01s = null; // added on 20110425
        protected virtual void HyperLinkFilter01_HyperLinkEdit_OpenLink(System.Object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            // Assign data visibility and clear for filter by range data
            // ================================================================
            ControlVisibility(CboFilter01_CBox.Tag.ToString(), false, true);
            LookUpEdtFilter01From_LUEdit.EditValue = null;
            LookUpEdtFilter01To_LUEdit.EditValue = null;
            // ================================================================

            OpenMultiSelectDialog(this, HyperLinkFilter01_HyperLinkEdit, "Multi-Select " + HyperLinkFilter01_HyperLinkEdit.Tag.ToString(), SQLStrHyperLinkFilter01, SQLStrHyperLinkFilter01Key1, ref Lstr_Filter01s, ref Ldt_Filter01s);
        }

        protected string Lstr_Filter02s = "";
        protected DataTable Ldt_Filter02s = null; // added on 20110425
        protected virtual void HyperLinkFilter02_HyperLinkEdit_OpenLink(System.Object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            // Assign data visibility and clear for filter by range data
            // ================================================================
            // [Updated] by scchang's Gemini 3 Pro on 20260203: Use key variable instead of Tag to ensure correct match
            ControlVisibility(SQLStrHyperLinkFilter02Key1, false, true);
            LookUpEdtFilter02From_LUEdit.EditValue = null;
            LookUpEdtFilter02To_LUEdit.EditValue = null;
            // ================================================================

            OpenMultiSelectDialog(this, HyperLinkFilter02_HyperLinkEdit, "Multi-Select " + HyperLinkFilter02_HyperLinkEdit.Tag.ToString(), SQLStrHyperLinkFilter02, SQLStrHyperLinkFilter02Key1, ref Lstr_Filter02s, ref Ldt_Filter02s);
        }

        protected string Lstr_Filter03s = "";
        protected DataTable Ldt_Filter03s = null; // added on 20110425
        protected virtual void HyperLinkFilter03_HyperLinkEdit_OpenLink(System.Object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            // Assign data visibility and clear for filter by range data
            // ================================================================
            // [Updated] by scchang's Gemini 3 Pro on 20260203: Use key variable instead of Tag to ensure correct match
            ControlVisibility(SQLStrHyperLinkFilter03Key1, false, true);
            LookUpEdtFilter03From_LUEdit.EditValue = null;
            LookUpEdtFilter03To_LUEdit.EditValue = null;
            // ================================================================

            OpenMultiSelectDialog(this, HyperLinkFilter03_HyperLinkEdit, "Multi-Select " + HyperLinkFilter03_HyperLinkEdit.Tag.ToString(), SQLStrHyperLinkFilter03, SQLStrHyperLinkFilter03Key1, ref Lstr_Filter03s, ref Ldt_Filter03s);
        }

        // [added] by scchang's Gemini 3 Pro on 20260205: Helper method for Multi-Select Dialog
        protected void OpenMultiSelectDialog(Form owner, DevExpress.XtraEditors.HyperLinkEdit hyperLinkEdit, string caption, string sql, string keyName, ref string selectedVal, ref DataTable prevResults)
        {
             // Clear Range Filters if they exist (This logic presumes generic filter controls, can be adjusted)
             // For now, we rely on the caller or ControlVisibility to handle specific UI control toggling.
            
            FilterImport_Form Filter_Frm = new FilterImport_Form();
            Filter_Frm.PrevResults = prevResults;
            Filter_Frm.TabPageHeaderName = caption; 
            Filter_Frm.ShowMe(sql, keyName);
            
            if (hyperLinkEdit != null)
                hyperLinkEdit.Text = Filter_Frm.HyperlinkText;

            if (Filter_Frm.DialogResult == DialogResult.OK)
            {
                prevResults = Filter_Frm.Results;
                selectedVal = Filter_Frm.SelectedVal;
            }
        }
        #endregion //" HyperLink Events "
        #endregion //" For Filter Dock Panel "

        #region " Report section "
        // edited by SCChang's Copilot on 20251124: changed to virtual for Visual Inheritance
        // for report
        //public const string ReportType = "Installment Document";
        protected virtual string ReportType { get; set; } = "";
        //public const string ReportListingType = "Installment Listing";
        protected virtual string ReportListingType { get; set; } = "";

        private AutoCount.Report.BasicReportOption myBasicReportOption;
        private AutoCount.Report.ReportInfo myBasicReportInfo;

        private DataTable tm = null;
        private DataTable td = null;
        private DataTable tb = null;

        #region " Document Report "
        BusinessBase_Cls BusinessObject;

        protected virtual void Preview_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            if (this.DataList_DXGrid.DataSource != null)
            {
                if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
                {
                    DataRow dr_focused = this.DataList_DXGridVw.GetDataRow(this.DataList_DXGridVw.FocusedRowHandle);
                    Report("Preview", (long)dr_focused["DocKey"]);
                }
                else
                    //Report("Preview", 0);
                    AutoCount.AppMessage.ShowInformationMessage(this, "There is NO Data to Preview !");
            }
            else
                //Report("Preview", 0);
                AutoCount.AppMessage.ShowInformationMessage(this, "There is NO Data to Preview !");
        }

        protected virtual void Print_SBtn_Click(object sender, EventArgs e)
        {
            if (this.DataList_DXGrid.DataSource != null)
            {
                if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
                {
                    DataRow dr_focused = this.DataList_DXGridVw.GetDataRow(this.DataList_DXGridVw.FocusedRowHandle);
                    Report("Print", (long)dr_focused["DocKey"]);
                }
                else
                    AutoCount.AppMessage.ShowInformationMessage(this, "There is NO Data to Print !");
            }
            else
                AutoCount.AppMessage.ShowInformationMessage(this, "There is NO Data to Print !");
        }

        protected virtual void PrintBarcode_SBtn_Click(object sender, EventArgs e)
        {
            if (this.DataList_DXGrid.DataSource != null)
            {
                if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
                {
                    int[] int_Selected = this.DataList_DXGridVw.GetSelectedRows();
                    if (int_Selected.Length > 0)
                    {
                        string str_Criteria = "";
                        DataRow dr_focused = null;
                        for (int int_idx = 0; int_idx <= int_Selected.Length - 1; int_idx++)
                        {
                            dr_focused = this.DataList_DXGridVw.GetDataRow(int_Selected[int_idx]);
                            str_Criteria += dr_focused["DocKey"].ToString() + ",";
                        }

                        if (str_Criteria != "" || str_Criteria.Length > 0)
                            str_Criteria = str_Criteria.Substring(0, str_Criteria.Length - 1);

                        Report("PrintBarcode", 0, str_Criteria);
                    }
                }
                else
                    Report("PrintBarcode", 0);
            }
            else
                Report("PrintBarcode", 0);
        }

        protected virtual void DesignReport_SBtn_Click(object sender, EventArgs e)
        {
            if (this.DataList_DXGrid.DataSource != null)
            {
                if (((DataTable)this.DataList_DXGrid.DataSource).Rows.Count > 0)
                {
                    DataRow dr_focused = this.DataList_DXGridVw.GetDataRow(this.DataList_DXGridVw.FocusedRowHandle);
                    Report("Design", (long)dr_focused["DocKey"]);
                }
                else
                    Report("Design", 0);
            }
            else
                Report("Design", 0);
        }

        protected virtual void Report(string str_ReportMode, long lng_DocKey, string str_Criteria = "", bool boo_ShowPrintDialog = false)
        {
            myBasicReportInfo = new AutoCount.Report.ReportInfo("", "", "", "");
            myBasicReportOption = new AutoCount.Report.BasicReportOption();
            myBasicReportOption.ShowPrintDialog = boo_ShowPrintDialog;

            // ''''If str_ReportMode = "PrintBarcode" Then
            // ''''    If str_Criteria <> "" Then
            // ''''        tb = myDBSetting.GetDataTable("Select BranchNo, LabNo, (BranchNo+LabNo) as TestSampleID From zLIS_TestRegistration where DocKey in (" & str_Criteria & ")", False)
            // ''''    Else
            // ''''        Autocount.AppMessage.ShowWarningMessage(Me, "No Record(s) Selected!")

            // ''''        Exit Sub
            // ''''    End If
            // ''''End If

            // edited by SCChang's Copilot on 20251125: support View-based forms without BusinessObject
            if (this.BusinessObject != null)
            {
                tm = myDBSetting.GetDataTable("Select * From " + SQLString(this.BusinessObject.MasterTableName) + " where DocKey=" + lng_DocKey.ToString() + "", false);
                td = myDBSetting.GetDataTable("Select * From " + SQLString(this.BusinessObject.Detail.DetailTableQryName) + " where DocKey=" + lng_DocKey.ToString() + "", false);
            }
            else
            {
                // For View-based forms, use TableName and DetailTableQueryName properties
                tm = myDBSetting.GetDataTable("Select * From " + SQLString(this.TableName) + " where DocKey=" + lng_DocKey.ToString() + "", false);
                if (!string.IsNullOrEmpty(this.DetailTableQueryName))
                    td = myDBSetting.GetDataTable("Select * From " + SQLString(this.DetailTableQueryName) + " where DocKey=" + lng_DocKey.ToString() + "", false);
            }

            DataTable dt_CompanyProfile = myDBSetting.GetDataTable("Select * From Profile", true);

            // for td_user
            DataTable td_user = new DataTable("TableParameter");
            td_user.Columns.Add("UserID");

            DataRow dr_newRow = td_user.NewRow();
            dr_newRow["UserID"] = AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID;
            td_user.Rows.Add(dr_newRow);

            switch (str_ReportMode)
            {
                case "Design":
                    {
                        AutoCount.Report.ReportTool.DesignReport(ReportType, GetReportDataSource(tm, td, dt_CompanyProfile, td_user, tb), AutoCount.Authentication.UserSession.CurrentUserSession);
                        break;
                    }

                case "Preview":
                    {
                        // ''''Autocount.Report.ReportTool.PreviewReportByName("Patient Registration", GetReportDataSource(tm, td, dt_CompanyProfile, td_user), myDBSetting, False, myBasicReportOption, myBasicReportInfo) 'added by chang on 20130502: Autocount changed the parameter list, added 'ReportInfo'
                        AutoCount.Report.ReportTool.PreviewReport(ReportType, GetReportDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession, true, false, myBasicReportOption, myBasicReportInfo); // added by chang on 20130502: Autocount changed the parameter list, added 'ReportInfo'
                        break;
                    }

                case "Print":
                    {
                        // ''''Autocount.Report.ReportTool.PrintReportByName("Patient Registration", GetReportDataSource(tm, td, dt_CompanyProfile, td_user), myDBSetting, myBasicReportOption, myBasicReportInfo)
                        AutoCount.Report.ReportTool.PrintReport(ReportType, GetReportDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession, true, myBasicReportOption, myBasicReportInfo);
                        break;
                    }

                case "PrintBarcode":
                    {
                        break;
                    }
            }

            myBasicReportInfo = null;
            myBasicReportOption = null;
            td_user = null;
        }

        protected virtual AutoCount.Report.DocumentReportDataSet GetReportDataSource(DataTable tm, DataTable td, DataTable td_CompanyProfile, DataTable td_user, DataTable tb = null)
        {
            AutoCount.Report.DocumentReportDataSet dsReport = new AutoCount.Report.DocumentReportDataSet(AutoCount.Authentication.UserSession.CurrentUserSession, ReportType, "Master", "Detail");

            tm.TableName = "Master";
            td.TableName = "Detail";
            td_CompanyProfile.TableName = "Company Profile";
            td_user.TableName = "TableUser";

            if (tb != null)
                tb.TableName = "BarcodeMaster";

            dsReport.Tables.Add(tm);
            dsReport.Tables.Add(td);
            dsReport.Tables.Add(td_CompanyProfile);
            dsReport.Tables.Add(td_user);
            if (tb != null) dsReport.Tables.Add(tb);

            DataRelation DataRelationMasterDetail = new DataRelation("MasterDetailRelation", tm.Columns["DocKey"], td.Columns["DocKey"]);
            dsReport.Relations.Add(DataRelationMasterDetail);

            return dsReport;
        }
        #endregion " Document Report "

        #region " Listing Report "
        protected virtual void PreviewListing_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            ReportListing("Preview");
        }

        protected virtual void PrintListing_SBtn_Click(object sender, EventArgs e)
        {
            ReportListing("Print");
        }

        protected virtual void DesignReportListing_SBtn_Click(object sender, EventArgs e)
        {
            ReportListing("Design");
        }

        protected virtual void ReportListing(string str_ReportMode, string str_Criteria = "", bool boo_ShowPrintDialog = false)
        {
            //initialize variables
            tm?.Dispose();
            tm = null;
            td?.Dispose();
            td = null;

            //try to get visible rows from gridcontrol
            if (DataList_DXGrid.DataSource != null && DataList_DXGridVw.RowCount > 0)
            {
                //clone the structure of the gridcontrol datasource 1st
                tm = ((DataTable)DataList_DXGrid.DataSource).Clone();

                //import the datarows into report's datatable
                for (int int_i = 0; int_i < DataList_DXGridVw.RowCount; int_i++)
                    tm.ImportRow(DataList_DXGridVw.GetDataRow(int_i));

                tm.AcceptChanges();
            }
            else
            {
                AutoCount.AppMessage.ShowInformationMessage(this, $"There is NO Data to {str_ReportMode} !");

                return;
            }

            myBasicReportInfo = new AutoCount.Report.ReportInfo("", "", "", "");
            myBasicReportOption = new AutoCount.Report.BasicReportOption();
            myBasicReportOption.ShowPrintDialog = boo_ShowPrintDialog;

            DataTable dt_CompanyProfile = myDBSetting.GetDataTable("Select * From Profile", true);

            // for td_user
            DataTable td_user = new DataTable("TableParameter");
            td_user.Columns.Add("UserID");

            DataRow dr_newRow = td_user.NewRow();
            dr_newRow["UserID"] = AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID;
            td_user.Rows.Add(dr_newRow);

            switch (str_ReportMode)
            {
                case "Design":
                    {
                        AutoCount.Report.ReportTool.DesignReport(ReportListingType, GetReportListingDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession);
                        break;
                    }

                case "Preview":
                    {
                        // ''''Autocount.Report.ReportTool.PreviewReportByName("Patient Registration", GetReportDataSource(tm, td, dt_CompanyProfile, td_user), myDBSetting, False, myBasicReportOption, myBasicReportInfo) 'added by chang on 20130502: Autocount changed the parameter list, added 'ReportInfo'
                        AutoCount.Report.ReportTool.PreviewReport(ReportListingType, GetReportListingDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession, true, false, myBasicReportOption, myBasicReportInfo); // added by chang on 20130502: Autocount changed the parameter list, added 'ReportInfo'
                        break;
                    }

                case "Print":
                    {
                        // ''''Autocount.Report.ReportTool.PrintReportByName("Patient Registration", GetReportDataSource(tm, td, dt_CompanyProfile, td_user), myDBSetting, myBasicReportOption, myBasicReportInfo)
                        AutoCount.Report.ReportTool.PrintReport(ReportListingType, GetReportListingDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession, true, myBasicReportOption, myBasicReportInfo);
                        break;
                    }
            }

            myBasicReportInfo = null;
            myBasicReportOption = null;
            td_user = null;
        }

        protected virtual AutoCount.Report.DocumentReportDataSet GetReportListingDataSource(DataTable tm, DataTable td, DataTable td_CompanyProfile, DataTable td_user)
        {
            AutoCount.Report.DocumentReportDataSet dsReport = new AutoCount.Report.DocumentReportDataSet(AutoCount.Authentication.UserSession.CurrentUserSession, ReportListingType, "Master");

            tm.TableName = "Master";
            //td.TableName = "Detail";
            td_CompanyProfile.TableName = "Company Profile";
            td_user.TableName = "TableUser";

            dsReport.Tables.Add(tm);
            //dsReport.Tables.Add(td);
            dsReport.Tables.Add(td_CompanyProfile);
            dsReport.Tables.Add(td_user);

            //DataRelation DataRelationMasterDetail = new DataRelation("MasterDetailRelation", tm.Columns["DocKey"], td.Columns["DocKey"]);
            //dsReport.Relations.Add(DataRelationMasterDetail);

            return dsReport;
        }
        #endregion " Listing Report "
        #endregion " Report section "
    }
}
