using System;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using VTACPluginBase.BaseForms;
//using VTACPluginBase.BaseForms.Editor;
using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.BaseForms.FormHelper;

namespace VTACPluginBase.CommonForms
{
    public partial class DeleteDlg_Form : Form // BaseEditor_Form
    {
        #region " Variables/Constants Declaration section "
        #region " Classes declaration "
        //////////Private Lcls_TestClass As New TestClass_Cls()

        //////////Private LitemDA As Autocount.Stock.Item.ItemDataAccess = Autocount.Stock.Item.ItemDataAccess.Create(myDBSetting)
        //////////Private LBO_TestItem As Autocount.Stock.Item.ItemEntity
        #endregion " Classes declaration "
        #endregion " Variables/Constants Declaration section "

        #region " Form Properties "
        private FormMethods Lenm_FormMethods = FormMethods.NEW;
        public FormMethods Method
        {
            get
            {
                return Lenm_FormMethods;
            }
        }

        protected bool Editable
        {
            get
            {
                return Lcls_formHelper.Editable;
            }
            set
            {
                Lcls_formHelper.Editable = value;
            }
        }

        public bool Modified
        {
            get
            {
                return Lcls_formHelper.FormIsModified;
            }
        }

        public bool FormIsSaved
        {
            get
            {
                return Lcls_formHelper.FormIsSaved;
            }
        }

        // here represents the primary key of the main class (main table, exp: RentalXFER() has RentalXFER.Code)
        private string Lstr_Code = "";
        public string Code
        {
            get
            {
                return Lstr_Code;
            }
            set
            {
                Lstr_Code = value;
            }
        }

        private BusinessBase_Cls BusinessObject;

        private long Li64_DocKey = -1;
        public long DocKey
        {
            get
            {
                return Li64_DocKey;
            }
        }

        //////////protected override string ProceedNewAfterSaveSysConfigName { get; set; }

        //////////protected override string ReportType { get; set; }

        //////////protected override string PrintDesignAccessRightsCMD { get; set; }

        #endregion " Form Properties "

        #region " Form Constructor "
        private FormHelper Lcls_formHelper = null;

        //here need to pass in 2 param list
        //1. Form's Method: suppose always is 'DELETE'
        //2. Class that wanna to delete the record, exp: RentalXFER class
        public DeleteDlg_Form(FormMethods enm_FormMethods, BusinessBase_Cls cls_BO, long DocKey = -1) //: base(enm_FormMethods, DocKey)
        {
            InitializeComponent();

            //here assigns the form method
            Lenm_FormMethods = enm_FormMethods;

            Lcls_formHelper = new FormHelper();

            Li64_DocKey = DocKey;

            //here assigns the obj class to business obj
            this.BusinessObject = cls_BO;
        }
        #endregion " Form Constructor "

        #region " Form Events "
        private void DeleteDlg_Form_Load(object sender, EventArgs e)
        {
            try
            {
                //here initialize the form
                InitForm();
            }
            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: \n" + ex.Message + "\n" + ex.StackTrace);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(DeleteDlg_Form_Load) + "()", ex);
            }
        }

        private void DeleteDlg_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            //////////Dim typ_obj As Type = LBO_obj.GetType()

            //////////Select Case typ_obj.Name
            //////////    Case "RentalXFER_Cls"
            //////////        '----------------------------------------------------------------------------------------------------------------------------
            //////////        'Rental Stock Transfer section
            //////////        Ldoc_XFER = Lcmd_XFER.Edit(Lcls_RentalXFER.AutoCountDocKey)
            //////////        If Ldoc_XFER IsNot Nothing Then
            //////////            Ldoc_XFER.Note = "I@SRentalPlugins####"

            //////////            Ldoc_XFER.Save(Autocount.Authentication.UserAuthentication.GetOrCreate(Autocount.Application.DBSetting).LoginUserID)
            //////////        End If
            //////////        '----------------------------------------------------------------------------------------------------------------------------

            //////////    Case "RentalWOFF_Cls"
            //////////        '----------------------------------------------------------------------------------------------------------------------------
            //////////        'Rental Stock Write Off section
            //////////        Ldoc_WOFF = Lcmd_WOFF.Edit(Lcls_RentalWOFF.AutoCountDocKey)
            //////////        If Ldoc_WOFF IsNot Nothing Then
            //////////            Ldoc_WOFF.Note = "I@SRentalPlugins####"

            //////////            Ldoc_WOFF.Save(Autocount.Authentication.UserAuthentication.GetOrCreate(Autocount.Application.DBSetting).LoginUserID)
            //////////        End If
            //////////        '----------------------------------------------------------------------------------------------------------------------------

            //////////    Case "RentalGR_Cls"
            //////////        '----------------------------------------------------------------------------------------------------------------------------
            //////////        'Rental Goods Receiving section
            //////////        Ldoc_GR = Lcmd_GR.Edit(Lcls_RentalGR.AutoCountDocKey)
            //////////        If Ldoc_GR IsNot Nothing Then
            //////////            Ldoc_GR.Note = "I@SRentalPlugins####"

            //////////            Ldoc_GR.Save(Autocount.Authentication.UserAuthentication.GetOrCreate(Autocount.Application.DBSetting).LoginUserID)
            //////////        End If
            //////////        '----------------------------------------------------------------------------------------------------------------------------

            //////////End Select
        }
        #endregion " Form Events "

        #region " Simple Button Events "
        // use to close this form
        private void No_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // Close form
            this.Close();
        }

        // use to delete data process
        private void Yes_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // here for delete data process
            string str_deldata = this.Delete();

            if (str_deldata == "")
            {
                this.Lcls_formHelper.FormIsModified = false;

                this.Close();
            }
            else AutoCount.AppMessage.ShowErrorMessage(this, "Delete Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_deldata);
        }
        #endregion " Simple Button Events "

        #region " Methods/Procedures/Functions "
        #region " Form Refresh/Display "
        // use to initialize form
        private void InitForm()
        {
            // ------------------------------------------------------------------------------------------------------------------------------------
            // here to put image for picturebox1
            // =================================
            this.PictureBox1.Image = System.Drawing.SystemIcons.Question.ToBitmap();
            // ------------------------------------------------------------------------------------------------------------------------------------

            // by default form's editable is True
            this.Editable = true;

            switch (this.Method)
            {
                case FormMethods.DELETE:
                    {
                        // 'enable controls
                        // EnableControls(Me, False)
                        // InitSimpleButtons(False)
                        // InitSpecificDisplay()

                        this.Editable = false;

                        // start to load data
                        LoadData();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            // to register controls' modified events
            Control thisFormCtrl = (Control)this;
            Lcls_formHelper.StartupRegisterEvents(ref thisFormCtrl);
        }

        // use to initialize Simple Buttons Controls
        private void InitSimpleButtons(bool boo_enabled)
        {
            // common functions buttons
            this.Yes_SBtn.Enabled = boo_enabled;
            this.No_SBtn.Enabled = true; // this button suppose always enabled
        }

        // use to initialize specific display
        private void InitSpecificDisplay()
        {
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------
        // use to load data for this form's document
        // =========================================
        private void LoadData()
        {
            // [edited] by scchang's GPT-5.2 on 20260121: v1.0.0.0, Only load if not already loaded (check PrimaryKey)
            if (BusinessObject.PrimaryKey <= 0 || BusinessObject.PrimaryKey != this.DocKey)
            {
                BusinessObject.Load(this.DocKey.ToString());
            }

            // here to set the delete message
            // ==============================
            // [edited] by scchang's GPT-5.2 on 20260121: v1.0.0.0, Use DisplayKeyFieldName instead of hardcoded DocNo
            string displayKeyValue = GetDisplayKeyValue();
            this.DeleteMsg_Lbl.Text = "Do you really want to delete this record [" + displayKeyValue + "] ?";
        }

        // [added] by scchang's GPT-5.2 on 20260121: v1.0.0.0, Get display key value dynamically
        private string GetDisplayKeyValue()
        {
            try
            {
                if (BusinessObject == null) return "";
                
                string displayKeyFieldName = BusinessObject.DisplayKeyFieldName;
                if (string.IsNullOrWhiteSpace(displayKeyFieldName)) return BusinessObject.DocNo;
                
                var prop = BusinessObject.GetType().GetProperty(displayKeyFieldName);
                if (prop == null) return BusinessObject.DocNo;
                
                object val = prop.GetValue(BusinessObject);
                return val == null ? "" : val.ToString();
            }
            catch
            {
                return BusinessObject.DocNo;
            }

            //Type typ_obj = LBO_obj.GetType();
            //switch (typ_obj.Name)
            //{
            //    case nameof(Installment_Cls):
            //        {
            //            ((Installment_Cls)LBO_obj).Load(this.DocKey.ToString());

            //            // here to set the delete message
            //            // ==============================
            //            this.DeleteMsg_Lbl.Text = "Do you really want to delete this Document [" + ((Installment_Cls)LBO_obj).DocNo + "] ?";
            //            break;
            //        }
            //}
        }
        #endregion " Form Refresh/Display "

        #region " Data Manipulation section "
        // use to Save data
        private new string Save()
        {
            string str_rtn = "";

            try
            {
            }
            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(Save) + "()", ex);
            }

            return str_rtn;
        }

        // ----------------------------------------------------------------------------------------------------------------------------------------
        // use to Delete data
        // ==================
        // here declare for the autocount's classes for delete purposes
        private string Delete()
        {
            string str_rtn = "";

            try
            {
                str_rtn = BusinessObject.Delete();

                //Type typ_obj = LBO_obj.GetType();
                //switch (typ_obj.Name)
                //{
                //    case nameof(Installment_Cls):
                //        {
                //            str_rtn = ((Installment_Cls)LBO_obj).Delete();
                //            break;
                //        }
                //}
            }
            catch (Exception ex)
            {
                // '' ''Autocount.AppMessage.ShowErrorMessage(Me, "Errors due to: " + Chr(13) + ex.Message + Chr(13) + ex.StackTrace)
                ErrorLogger_Cls.Write(this.Name + "." + nameof(Delete) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " Data Manipulation section "
        #endregion " Methods/Procedures/Functions "
    }
}
