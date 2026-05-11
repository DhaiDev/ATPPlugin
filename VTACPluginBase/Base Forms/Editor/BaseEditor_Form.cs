using System;
using System.Collections;
// [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, required for focus highlight state tracking
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.TextLogger;
using VTACPluginBase.CommonForms;

using static VTACPluginBase.BaseForms.FormHelper;
using static VTACPluginBase.Classes.Helpers.AutoCountHelper;
using static VTACPluginBase.Classes.Helpers.GeneralHelper;
using static VTACPluginBase.Classes.Helpers.SystemHelper;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.BaseForms.Editor
{
    // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Visual inheritance in VS Designer works better with non-abstract base form
    // [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<BaseEditor_Form, Form>))]
    // [edited] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Make BaseEditor_Form non-abstract (align with BaseList_Form pattern)
    public partial class BaseEditor_Form : Form
    {
        #region " [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Focused control backcolor highlight (all editor forms) "
        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Guard to avoid double-hooking events
        private bool _focusHighlightInitialized = false;

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Preserve original BackColor for WinForms controls
        private readonly Dictionary<Control, System.Drawing.Color> _originalControlBackColors = new Dictionary<Control, System.Drawing.Color>();

        /// <summary>
        /// Enable/disable focus highlight for all editor forms.
        /// Derived forms can override to disable if they have special UI rules.
        /// </summary>
        protected virtual bool EnableFocusedControlBackColorHighlight
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Focused control background color.
        /// Uses SystemColors.Info (AutoCount-like light yellow) by default.
        /// </summary>
        protected virtual System.Drawing.Color FocusedControlBackColor
        {
            get
            {
                return System.Drawing.SystemColors.Info;
            }
        }

        /// <summary>
        /// Apply focused-control backcolor highlight across all input controls in this editor form.
        /// </summary>
        protected void EnableFocusedControlHighlighting()
        {
            try
            {
                if (_focusHighlightInitialized)
                {
                    return;
                }

                if (IsDesignTime)
                {
                    return;
                }

                _focusHighlightInitialized = true;

                ApplyFocusedControlHighlightingRecursive(this);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + "." + nameof(EnableFocusedControlHighlighting) + "()", ex);
            }
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
        private void ApplyFocusedControlHighlightingRecursive(Control container)
        {
            if (container == null)
            {
                return;
            }

            foreach (Control control in container.Controls)
            {
                ApplyFocusedControlHighlightingToControl(control);

                if (control != null && control.HasChildren)
                {
                    ApplyFocusedControlHighlightingRecursive(control);
                }
            }
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
        private void ApplyFocusedControlHighlightingToControl(Control control)
        {
            if (control == null)
            {
                return;
            }

            // DevExpress editors: prefer AppearanceFocused (no need to manually restore BackColor)
            DevExpress.XtraEditors.BaseEdit devExpressEditor = control as DevExpress.XtraEditors.BaseEdit;
            if (devExpressEditor != null)
            {
                try
                {
                    devExpressEditor.Properties.AppearanceFocused.BackColor = FocusedControlBackColor;
                    devExpressEditor.Properties.AppearanceFocused.Options.UseBackColor = true;
                }
                catch
                {
                    // swallow - some editors may not support focused appearance consistently
                }

                return;
            }

            // WinForms input controls: use Enter/Leave and restore original BackColor
            if (control is TextBoxBase || control is ComboBox || control is DateTimePicker || control is MaskedTextBox || control is NumericUpDown)
            {
                // Avoid double subscription
                control.Enter -= FocusHighlight_Control_Enter;
                control.Leave -= FocusHighlight_Control_Leave;
                control.Enter += FocusHighlight_Control_Enter;
                control.Leave += FocusHighlight_Control_Leave;
            }
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
        private void FocusHighlight_Control_Enter(object sender, EventArgs e)
        {
            try
            {
                Control control = sender as Control;
                if (control == null)
                {
                    return;
                }

                if (!_originalControlBackColors.ContainsKey(control))
                {
                    _originalControlBackColors[control] = control.BackColor;
                }

                control.BackColor = FocusedControlBackColor;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + "." + nameof(FocusHighlight_Control_Enter) + "()", ex);
            }
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
        private void FocusHighlight_Control_Leave(object sender, EventArgs e)
        {
            try
            {
                Control control = sender as Control;
                if (control == null)
                {
                    return;
                }

                if (_originalControlBackColors.ContainsKey(control))
                {
                    control.BackColor = _originalControlBackColors[control];
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + "." + nameof(FocusHighlight_Control_Leave) + "()", ex);
            }
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Focused control backcolor highlight (all editor forms) "

        #region " [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Validation error focus (all editor forms) "
        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Track the first validation error control for focus after message box
        private Control _validationErrorFocusControl = null;

        /// <summary>
        /// Register a validation error message and (optionally) the control to focus after the warning message is closed.
        /// Default behavior: first error control wins.
        /// </summary>
        /// <param name="currentErrors">Current aggregated error messages</param>
        /// <param name="message">New error message to append</param>
        /// <param name="focusControl">Control to focus after message box (optional)</param>
        /// <returns>Aggregated error messages</returns>
        protected string AddValidationError(string currentErrors, string message, Control focusControl = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return currentErrors;
            }

            try
            {
                if (_validationErrorFocusControl == null && focusControl != null)
                {
                    _validationErrorFocusControl = focusControl;
                }
            }
            catch
            {
                // swallow - focus tracking should never break validation
            }

            if (string.IsNullOrWhiteSpace(currentErrors))
            {
                return message.Trim();
            }

            return (currentErrors.TrimEnd('\n') + "\n" + message.Trim()).TrimEnd('\n');
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Clear validation focus state before running DataValidation()
        protected void ResetValidationErrorFocusControl()
        {
            _validationErrorFocusControl = null;
        }

        /// <summary>
        /// Gets the control that should be focused when validation failed.
        /// Derived forms may override to provide custom mapping.
        /// </summary>
        protected virtual Control GetValidationErrorFocusControl()
        {
            return _validationErrorFocusControl;
        }

        /// <summary>
        /// Try focusing the validation error control (after message box closed).
        /// Also tries to switch to the correct tab page if applicable.
        /// </summary>
        protected virtual bool TryFocusValidationErrorControl()
        {
            try
            {
                Control focusControl = GetValidationErrorFocusControl();
                if (focusControl == null)
                {
                    return false;
                }

                // Switch TabPage / XtraTabPage if the control is inside a tab
                DevExpress.XtraTab.XtraTabPage xtraTabPage = FindParentXtraTabPage(focusControl);
                if (xtraTabPage != null)
                {
                    DevExpress.XtraTab.XtraTabControl xtraTabControl = xtraTabPage.Parent as DevExpress.XtraTab.XtraTabControl;
                    if (xtraTabControl != null)
                    {
                        xtraTabControl.SelectedTabPage = xtraTabPage;
                    }
                }

                TabPage winTabPage = FindParentWinTabPage(focusControl);
                if (winTabPage != null)
                {
                    TabControl winTabControl = winTabPage.Parent as TabControl;
                    if (winTabControl != null)
                    {
                        winTabControl.SelectedTab = winTabPage;
                    }
                }

                // Focus + SelectAll where possible
                DevExpress.XtraEditors.BaseEdit devExpressEditor = focusControl as DevExpress.XtraEditors.BaseEdit;
                if (devExpressEditor != null)
                {
                    devExpressEditor.Focus();
                    try
                    {
                        devExpressEditor.SelectAll();
                    }
                    catch
                    {
                        // some editors may not support SelectAll
                    }

                    return true;
                }

                focusControl.Focus();

                TextBoxBase textBoxBase = focusControl as TextBoxBase;
                if (textBoxBase != null)
                {
                    textBoxBase.SelectAll();
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + "." + nameof(TryFocusValidationErrorControl) + "()", ex);
                return false;
            }
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
        private static DevExpress.XtraTab.XtraTabPage FindParentXtraTabPage(Control control)
        {
            Control current = control;
            while (current != null)
            {
                DevExpress.XtraTab.XtraTabPage xtraTabPage = current as DevExpress.XtraTab.XtraTabPage;
                if (xtraTabPage != null)
                {
                    return xtraTabPage;
                }

                current = current.Parent;
            }

            return null;
        }

        // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
        private static TabPage FindParentWinTabPage(Control control)
        {
            Control current = control;
            while (current != null)
            {
                TabPage tabPage = current as TabPage;
                if (tabPage != null)
                {
                    return tabPage;
                }

                current = current.Parent;
            }

            return null;
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Validation error focus (all editor forms) "

        #region " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, BaseEditor capabilities (Master + Doc universal) "
        [Flags]
        protected enum BaseEditorCapabilities
        {
            None = 0,
            DocFields = 1 << 0,
            DocStatusWorkflow = 1 << 1,
            DetailGrid = 1 << 2,
            AutoCountDocs = 1 << 3,
            Printing = 1 << 4,
            ProceedAfterSave = 1 << 5,
            DocNoAutoAssign = 1 << 6,

            DefaultDocEditor = DocFields | DocStatusWorkflow | DetailGrid | AutoCountDocs | Printing | ProceedAfterSave | DocNoAutoAssign
        }

        /// <summary>
        /// Capability/Policy switch for BaseEditor_Form.
        /// Derived master forms should override this to disable Doc-specific assumptions.
        /// </summary>
        protected virtual BaseEditorCapabilities EditorCapabilities
        {
            get
            {
                return BaseEditorCapabilities.DefaultDocEditor;
            }
        }

        protected bool HasCapability(BaseEditorCapabilities capability)
        {
            return (EditorCapabilities & capability) == capability;
        }

        protected virtual void ApplyEditorCapabilitiesToUI()
        {
            try
            {
                bool useDocFields = HasCapability(BaseEditorCapabilities.DocFields);
                bool useDetails = HasCapability(BaseEditorCapabilities.DetailGrid);
                bool useDocWorkflow = HasCapability(BaseEditorCapabilities.DocStatusWorkflow);
                bool useACDocs = HasCapability(BaseEditorCapabilities.AutoCountDocs);
                bool usePrinting = HasCapability(BaseEditorCapabilities.Printing);
                bool useProceed = HasCapability(BaseEditorCapabilities.ProceedAfterSave);

                if (this.DocNo_Lbl != null) this.DocNo_Lbl.Visible = useDocFields;
                if (this.DocNo_TxtEdit != null) this.DocNo_TxtEdit.Visible = useDocFields;
                if (this.DocDate_Lbl != null) this.DocDate_Lbl.Visible = useDocFields;
                if (this.DocDate_DteEdit != null) this.DocDate_DteEdit.Visible = useDocFields;

                if (this.Status_Lbl != null) this.Status_Lbl.Visible = useDocWorkflow;

                if (this.Details_Pnl != null) this.Details_Pnl.Visible = useDetails;
                if (this.OprButtons_Pnl != null) this.OprButtons_Pnl.Visible = useDetails;
                if (this.Details_TabCtrl != null) this.Details_TabCtrl.Visible = useDetails;

                if (this.AddDetail_SpBtn != null) this.AddDetail_SpBtn.Visible = useDetails;
                if (this.EditDetail_SpBtn != null) this.EditDetail_SpBtn.Visible = useDetails;
                if (this.DeleteDetail_SpBtn != null) this.DeleteDetail_SpBtn.Visible = useDetails;

                if (this.GenerateACDocs_BarBtn != null) this.GenerateACDocs_BarBtn.Visibility = useACDocs ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                if (this.OpenAutocountDoc_BarBtn != null) this.OpenAutocountDoc_BarBtn.Visibility = useACDocs ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;

                if (this.Print_BarBtn != null) this.Print_BarBtn.Visibility = usePrinting ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                if (this.PrintWithDefaultPrinter_BarBtn != null) this.PrintWithDefaultPrinter_BarBtn.Visibility = usePrinting ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                if (this.PrintWithPrinterDialog_BarBtn != null) this.PrintWithPrinterDialog_BarBtn.Visibility = usePrinting ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                if (this.Print_BarBtnGrp != null) this.Print_BarBtnGrp.Visibility = usePrinting ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;

                if (this.Cancel_BarBtn != null) this.Cancel_BarBtn.Visibility = useDocWorkflow ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                if (this.Complete_BarBtn != null) this.Complete_BarBtn.Visibility = useDocWorkflow ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                if (this.Terminate_BarBtn != null) this.Terminate_BarBtn.Visibility = useDocWorkflow ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;

                if (this.ProceedAfterSave_BarChkItem != null) this.ProceedAfterSave_BarChkItem.Visibility = useProceed ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(this.Name + "." + nameof(ApplyEditorCapabilitiesToUI) + "()", ex);
            }
        }

        protected virtual string GetBusinessObjectDisplayKeyText()
        {
            try
            {
                if (BusinessObject == null) return "";
                if (string.IsNullOrWhiteSpace(BusinessObject.DisplayKeyFieldName)) return "";

                var prop = BusinessObject.GetType().GetProperty(BusinessObject.DisplayKeyFieldName);
                if (prop == null) return "";

                object val = prop.GetValue(BusinessObject);
                return val == null ? "" : val.ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, BaseEditor capabilities (Master + Doc universal) "

        #region " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Exposed UI containers for derived forms "
        protected DevExpress.XtraTab.XtraTabPage HeaderInfoTabPage
        {
            get
            {
                return this.HeaderInfo_TabPg;
            }
        }

        protected DevExpress.XtraEditors.PanelControl DetailsPanel
        {
            get
            {
                return this.Details_Pnl;
            }
        }

        protected DevExpress.XtraGrid.GridControl DetailGridControl
        {
            get
            {
                return this.Detail_DXGrid;
            }
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Exposed UI containers for derived forms "

        #region " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Design-time constructor (inherited form designer support) "
        /// <summary>
        /// Parameterless constructor for WinForms designer/inherited designer support.
        /// Do not use this constructor at runtime.
        /// </summary>
        protected BaseEditor_Form()
        {
            // [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Ensure base designer layout is created in inherited form designer
            InitializeComponent();
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Design-time constructor (inherited form designer support) "

        #region " Form Constructor "
        public BaseEditor_Form(FormMethods enm_FormMethods, long docKey = -1)
        {

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.

            Lenm_FormMethods = enm_FormMethods;

            try
            {
                if (docKey > 0) Lng_DocKey = docKey;

                // here initialize the form
                // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Skip InitForm at design-time (designer should show base layout only)
                //InitForm();
                //if (!IsDesignTime)
                //{
                //    InitForm();
                //}
                // [moved] to OnLoad by Copilot to fix Virtual Method Call in Constructor issue
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errors due to: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(BaseEditor_Form) + "()", ex);
            }
        }
        #endregion " Form Constructor "

        #region " [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Safe InitForm call "
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            if (!IsDesignTime)
            {
                try
                {
                    // Ensure InitForm is called exactly once
                    // [commented] by scchang's GPT-5.2 on 20260123: v1.0.0.0, add focused control backcolor highlight after InitForm
                    // InitForm();

                    // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, call InitForm then apply common UI behaviors
                    InitForm();

                    // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, focused control highlight (AutoCount-like)
                    if (EnableFocusedControlBackColorHighlight)
                    {
                        EnableFocusedControlHighlighting();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errors during form loading: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ErrorLogger_Cls.Write(this.Name + "." + nameof(OnLoad) + "()", ex);
                }
            }
        }
        #endregion

        #region " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Design-time detection helper "
        // [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Guard for base designer initialization
        private bool _baseDesignerInitialized = false;

        protected bool IsDesignTime
        {
            get
            {
                try
                {
                    return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
                }
                catch
                {
                    return false;
                }
            }
        }

        // [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Allow inherited designers to force base InitializeComponent()
        protected void EnsureBaseDesignerInitialized()
        {
            try
            {
                if (_baseDesignerInitialized) return;

                // Base designer already ran.
                if (this.components != null && this.Header_GrpCtrl != null)
                {
                    _baseDesignerInitialized = true;
                    return;
                }

                // Force create base controls so inherited form designer can render the layout.
                InitializeComponent();
                _baseDesignerInitialized = true;
            }
            catch
            {
                // swallow for designer stability
            }
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Design-time detection helper "

        #region " Form Properties "
        protected FormMethods Lenm_FormMethods = FormMethods.NEW;
        protected FormMethods Method
        {
            get
            {
                return Lenm_FormMethods;
            }
        }

        private bool Lboo_Editable = false;
        protected bool Editable
        {
            get
            {
                return Lboo_Editable;
            }
            set
            {
                Lboo_Editable = value;
            }
        }

        public bool Modified
        {
            get
            {
                return FormIsModified;
            }
        }

        private bool Lboo_FormIsModified = false;
        protected bool FormIsModified
        {
            get
            {
                return Lboo_FormIsModified;
            }
            set
            {
                Lboo_FormIsModified = value;
                Lboo_FormIsSaved = !value;
            }
        }

        //public bool IsDataSaved
        //{
        //    get
        //    {
        //        return Lcls_common.FormIsSaved;
        //    }
        //}

        private bool Lboo_FormIsSaved = false;
        public bool FormIsSaved
        {
            get
            {
                return Lboo_FormIsSaved;
            }
            protected set
            {
                Lboo_FormIsSaved = value;
            }
        }

        protected bool Lboo_IsFormClosing = false;
        public bool IsFormClosing
        {
            get
            {
                return Lboo_IsFormClosing;
            }
        }

        //BusinessBase_Cls Lobj_BO;
        protected virtual BusinessBase_Cls BusinessObject { get; set; }

        protected long Lng_DocKey = -1;
        public long DocKey
        {
            get
            {
                return Lng_DocKey;
            }
        }

        //for access rights name assignment
        // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Abstract blocks Visual Inheritance in VS Designer
        //protected abstract string PrintDesignAccessRightsCMD { get; set; }
        // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Default to empty; derived forms should override
        protected virtual string PrintDesignAccessRightsCMD { get; set; } = "";

        // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, No design-time guard (Designer may trigger permission check)
        //protected bool PrintDesignPermission { get => CheckPermissions(PrintDesignAccessRightsCMD); }
        // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Design-time safe permission check
        protected bool PrintDesignPermission
        {
            get
            {
                return IsDesignTime ? false : CheckPermissions(PrintDesignAccessRightsCMD);
            }
        }

        // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Abstract blocks Visual Inheritance in VS Designer
        //protected abstract string ProceedNewAfterSaveSysConfigName { get; set; } // = "ProceedNewAfterSaveInstallment";
        // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Default to empty; derived forms should override
        protected virtual string ProceedNewAfterSaveSysConfigName { get; set; } = "";

        private bool Lboo_ProceedNewAfterSave = false;
        // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, ProceedNewAfterSave without design-time guard
        //protected bool ProceedNewAfterSave
        //{
        //    get
        //    {
        //        string str_ProceedNewAfterSave = GetSysConfigValue(ProceedNewAfterSaveSysConfigName);
        //        if (str_ProceedNewAfterSave == null)
        //        {
        //            Lboo_ProceedNewAfterSave = false;
        //            SetSysConfigValue(ProceedNewAfterSaveSysConfigName, Lboo_ProceedNewAfterSave, "Is Proceed New After Save " + ProceedAfterSave_BarChkItem.Tag.ToString());
        //        }
        //        else
        //            Lboo_ProceedNewAfterSave = System.Convert.ToBoolean(str_ProceedNewAfterSave);
        //
        //        return Lboo_ProceedNewAfterSave;
        //    }
        //    set
        //    {
        //        Lboo_ProceedNewAfterSave = value;
        //        SetSysConfigValue(ProceedNewAfterSaveSysConfigName, Lboo_ProceedNewAfterSave, "Is Proceed New After Save " + ProceedAfterSave_BarChkItem.Tag.ToString());
        //    }
        //}
        // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Design-time safe ProceedNewAfterSave
        protected bool ProceedNewAfterSave
        {
            get
            {
                if (IsDesignTime) return false;

                if (string.IsNullOrWhiteSpace(ProceedNewAfterSaveSysConfigName))
                    return false;

                string str_ProceedNewAfterSave = GetSysConfigValue(ProceedNewAfterSaveSysConfigName);
                if (str_ProceedNewAfterSave == null)
                {
                    Lboo_ProceedNewAfterSave = false;
                    SetSysConfigValue(ProceedNewAfterSaveSysConfigName, Lboo_ProceedNewAfterSave, "Is Proceed New After Save " + (ProceedAfterSave_BarChkItem?.Tag?.ToString() ?? ""));
                }
                else
                {
                    Lboo_ProceedNewAfterSave = System.Convert.ToBoolean(str_ProceedNewAfterSave);
                }

                return Lboo_ProceedNewAfterSave;
            }
            set
            {
                if (IsDesignTime) return;

                if (string.IsNullOrWhiteSpace(ProceedNewAfterSaveSysConfigName))
                    return;

                Lboo_ProceedNewAfterSave = value;
                SetSysConfigValue(ProceedNewAfterSaveSysConfigName, Lboo_ProceedNewAfterSave, "Is Proceed New After Save " + (ProceedAfterSave_BarChkItem?.Tag?.ToString() ?? ""));
            }
        }
        #endregion " Form Properties "

        #region " Form Handling Helpers "
        // use to register events (basically is for changed events)
        protected void StartupRegisterEvents(ref Control obj_DerivedForm)
        {
            // Register for the modified event
            InitControlsEventHandler(obj_DerivedForm);
        }

        protected void InitControlsEventHandler(Control obj_Container)
        {
            foreach (Control ctrl in obj_Container.Controls)
            {
                // If TypeOf ctrl Is MyTextBox_UCtrl Then
                // Dim obj_MyTBox As MyTextBox_UCtrl = ctrl
                // AddHandler obj_MyTBox.TextChanged, AddressOf Modified

                // Else

                if (ctrl is TextBox)
                {
                    TextBox obj_TextBox = (TextBox)ctrl;
                    obj_TextBox.TextChanged += FormModified;
                }
                else if (ctrl is CheckBox)
                {
                    CheckBox obj_CheckBox = (CheckBox)ctrl;
                    obj_CheckBox.CheckedChanged += FormModified;
                }
                else if (ctrl is DataGridView)
                {
                    DataGridView obj_DgView = (DataGridView)ctrl;
                    obj_DgView.CellValueChanged += FormModified;
                    obj_DgView.RowsRemoved += FormModified;
                }
                else if (ctrl is DevExpress.XtraEditors.BaseEdit)
                {
                    DevExpress.XtraEditors.BaseEdit obj_XtraBaseEdit = (DevExpress.XtraEditors.BaseEdit)ctrl;
                    obj_XtraBaseEdit.Modified += FormModified;
                }
                else if (ctrl is DevExpress.XtraRichEdit.RichEditControl)
                {
                    DevExpress.XtraRichEdit.RichEditControl obj_XtraBaseEdit = (DevExpress.XtraRichEdit.RichEditControl)ctrl;
                    obj_XtraBaseEdit.TextChanged += FormModified;
                }
                else if (ctrl is RichTextBox)
                {
                    RichTextBox obj_XtraBaseEdit = (RichTextBox)ctrl;
                    obj_XtraBaseEdit.TextChanged += FormModified;
                }
                else if (ctrl is DevExpress.XtraGrid.GridControl)
                {
                    DevExpress.XtraGrid.GridControl obj_XtrGridCtrl = (DevExpress.XtraGrid.GridControl)ctrl;
                    obj_XtrGridCtrl.DataSourceChanged += DataSourceChanged;
                    DataSourceChanged(obj_XtrGridCtrl, null/* TODO Change to default(_) if this is not a reference type */);
                }
                else if (ctrl is DevExpress.XtraVerticalGrid.VGridControl)
                {
                    DevExpress.XtraVerticalGrid.VGridControl obj_XtrVGridCtrl = (DevExpress.XtraVerticalGrid.VGridControl)ctrl;
                    obj_XtrVGridCtrl.DataSourceChanged += DataSourceChanged;
                    DataSourceChanged(obj_XtrVGridCtrl, null/* TODO Change to default(_) if this is not a reference type */);
                }
                else if (ctrl.Controls.Count > 0)
                    InitControlsEventHandler(ctrl);
            }
        }

        private Hashtable Lht_DataSource = new Hashtable();
        protected void DataSourceChanged(object sender, System.EventArgs e)
        {
            // Remove previously added handler, to prevent one event fire multiple event handler
            try
            {
                if (Lht_DataSource.ContainsKey(((Control)sender).Name))
                {
                    try
                    {
                        DataTable dt_Last = (DataTable)Lht_DataSource[((Control)sender).Name];

                        dt_Last.TableNewRow -= FormModified;
                        dt_Last.TableCleared -= FormModified;
                        dt_Last.RowChanged -= FormModified;
                        dt_Last.RowDeleted -= FormModified;
                        dt_Last.ColumnChanged -= FormModified;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }

            // Add event handler for the new datasource
            DataTable dt_Source = null/* TODO Change to default(_) if this is not a reference type */;

            if (sender is DevExpress.XtraGrid.GridControl)
            {
                DevExpress.XtraGrid.GridControl ctrl_XtraGridCtrl = (DevExpress.XtraGrid.GridControl)sender;

                if (ctrl_XtraGridCtrl.DataSource != null && ctrl_XtraGridCtrl.DataSource is DataTable)
                    dt_Source = (DataTable)ctrl_XtraGridCtrl.DataSource;
            }
            else if (sender is DevExpress.XtraVerticalGrid.VGridControl)
            {
                DevExpress.XtraVerticalGrid.VGridControl ctrl_XtraVGridCtrl = (DevExpress.XtraVerticalGrid.VGridControl)sender;

                if (ctrl_XtraVGridCtrl.DataSource != null && ctrl_XtraVGridCtrl.DataSource is DataTable)
                    dt_Source = (DataTable)ctrl_XtraVGridCtrl.DataSource;
            }

            if (dt_Source != null)
            {
                // Add again the event handler
                dt_Source.TableNewRow += FormModified;
                dt_Source.TableCleared += FormModified;
                dt_Source.RowChanged += FormModified;
                dt_Source.RowDeleted += FormModified;
                dt_Source.ColumnChanged += FormModified;

                // Add into hashtable for later events removal used.
                if (Lht_DataSource.ContainsKey(((Control)sender).Name))
                    Lht_DataSource[((Control)sender).Name] = dt_Source;
                else
                    Lht_DataSource.Add(((Control)sender).Name, dt_Source);
            }
        }

        protected void FormModified(object sender, System.EventArgs e)
        {
            this.FormIsModified = this.Editable;
            this.FormIsSaved = !this.FormIsModified;
        }

        protected void InitControlsDefaultValue(Control obj_SrcControl)
        {
            if (obj_SrcControl is DevExpress.XtraEditors.BaseEdit)
                ((DevExpress.XtraEditors.BaseEdit)obj_SrcControl).EditValue = "";
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseDateControl)
                ((DevExpress.XtraEditors.BaseDateControl)obj_SrcControl).DateTime = default; //null;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseImageListBoxControl)
                ((DevExpress.XtraEditors.BaseImageListBoxControl)obj_SrcControl).DataSource = null;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseListBoxControl)
                ((DevExpress.XtraEditors.BaseListBoxControl)obj_SrcControl).DataSource = null;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseSpinEdit)
                ((DevExpress.XtraEditors.BaseSpinEdit)obj_SrcControl).EditValue = 0;
            else if (obj_SrcControl is TextBox)
                ((TextBox)obj_SrcControl).Text = "";
            else if (obj_SrcControl is RadioButton)
                ((RadioButton)obj_SrcControl).Checked = false;
            else if (obj_SrcControl is CheckBox)
                ((CheckBox)obj_SrcControl).Checked = false;
            else if (obj_SrcControl is System.Windows.Forms.ComboBox)
                ((System.Windows.Forms.ComboBox)obj_SrcControl).DataSource = null;
            else if (obj_SrcControl is DataGridView)
                ((DataGridView)obj_SrcControl).DataSource = null;
            else if (obj_SrcControl is DevExpress.XtraGrid.GridControl)
                ((DevExpress.XtraGrid.GridControl)obj_SrcControl).DataSource = null;
            else if (obj_SrcControl is DevExpress.XtraVerticalGrid.VGridControl)
                ((DevExpress.XtraVerticalGrid.VGridControl)obj_SrcControl).DataSource = null;

            // Loop for inner controls
            foreach (Control obj_Control in obj_SrcControl.Controls)
                InitControlsDefaultValue(obj_Control);
        }

        // use to set control's enabled
        protected virtual void EnableControls(Control obj_SrcControl, bool boo_Enable)
        {
            if (obj_SrcControl is DevExpress.XtraEditors.BaseEdit)
                ((DevExpress.XtraEditors.BaseEdit)obj_SrcControl).Properties.ReadOnly = !boo_Enable;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseButton)
                ((DevExpress.XtraEditors.BaseButton)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseDateControl)
                ((DevExpress.XtraEditors.BaseDateControl)obj_SrcControl).ReadOnly = !boo_Enable;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseImageListBoxControl)
                ((DevExpress.XtraEditors.BaseImageListBoxControl)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseListBoxControl)
                ((DevExpress.XtraEditors.BaseListBoxControl)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is DevExpress.XtraEditors.BaseSpinEdit)
                ((DevExpress.XtraEditors.BaseSpinEdit)obj_SrcControl).Properties.ReadOnly = !boo_Enable;
            else if (obj_SrcControl is DevExpress.XtraRichEdit.RichEditControl)
                ((DevExpress.XtraRichEdit.RichEditControl)obj_SrcControl).ReadOnly = !boo_Enable;
            else if (obj_SrcControl is TextBox)
                ((TextBox)obj_SrcControl).ReadOnly = boo_Enable;
            else if (obj_SrcControl is Button)
                ((Button)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is RadioButton)
                ((RadioButton)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is CheckBox)
                ((CheckBox)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is System.Windows.Forms.ComboBox)
                ((System.Windows.Forms.ComboBox)obj_SrcControl).Enabled = boo_Enable;
            else if (obj_SrcControl is DataGridView)
            {
                ((DataGridView)obj_SrcControl).ReadOnly = !boo_Enable;

                // added by Danny 2008-12-19 don't allow the user to delete rows or use the newrow when disabled
                if (boo_Enable == false)
                    ((DataGridView)obj_SrcControl).AllowUserToAddRows = boo_Enable;
                if (boo_Enable == false)
                    ((DataGridView)obj_SrcControl).AllowUserToDeleteRows = boo_Enable;
            }
            else if (obj_SrcControl is DevExpress.XtraGrid.GridControl)
            {
                DevExpress.XtraGrid.GridControl ctrl_Target = (DevExpress.XtraGrid.GridControl)obj_SrcControl;

                for (int int_Index = 0; int_Index <= ctrl_Target.Views.Count - 1; int_Index++)
                    ((DevExpress.XtraGrid.Views.Grid.GridView)ctrl_Target.Views[int_Index]).OptionsBehavior.Editable = boo_Enable;
            }
            else if (obj_SrcControl is DevExpress.XtraVerticalGrid.VGridControl)
            {
                DevExpress.XtraVerticalGrid.VGridControl ctrl_Target = (DevExpress.XtraVerticalGrid.VGridControl)obj_SrcControl;

                ctrl_Target.OptionsBehavior.Editable = boo_Enable;
            }

            // Loop for inner controls
            foreach (Control obj_Control in obj_SrcControl.Controls)
                EnableControls(obj_Control, boo_Enable);
        }
        #endregion " Form Handling Helpers "

        #region " Form Events "
        private void BaseEditor_Form_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                //// here initialize the form
                //InitForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errors due to: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(BaseEditor_Form_Load) + "()", ex);
            }
        }

        private void BaseEditor_Form_FormClosing(System.Object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            MsgBoxResult obj_MsgBoxResult;
            if (BusinessObject == null) return;

            if (this.Modified)
            {
                // obj_MsgBoxResult = MessageBox.Show(Me, "Changes " & str_DisplayKey & " is not save, continue to close?", CType(sender, Form).Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                obj_MsgBoxResult = (MsgBoxResult)AutoCount.AppMessage.ShowConfirmSaveChangesMessage(this);
                if (obj_MsgBoxResult == MsgBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (obj_MsgBoxResult == MsgBoxResult.Yes)
                {
                    // here may need to put save process
                    Save_BarBtn_ItemClick(sender, null);

                    if (!this.FormIsSaved)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            // to save grid control layout
            SaveControlsLayout(this.Controls);

            this.Lboo_IsFormClosing = true;
        }
        #endregion " Form Events "

        #region " GridControl Events "
        protected virtual void Detail_DXGrid_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            //if (e.Button.ButtonType == DevExpress.XtraEditors.NavigatorButtonType.Remove)
            //{
            //    if (MessageBox.Show("Are you sure you want to delete this record ?", "Delete Detail", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            //        this.BusinessObject.DeleteDetail(Detail_DXGridVw.GetDataRow(Detail_DXGridVw.FocusedRowHandle), false);
            //    else
            //        e.Handled = true;
            //}
        }

        protected virtual void Detail_DXGridVw_DoubleClick(object sender, EventArgs e)
        {
            //if (Detail_DXGridVw.GetFocusedDataRow() == null) return;

            //switch (Detail_DXGridVw.FocusedColumn.Name)
            //{
            //    case "DG_RPDocNo_GCol":
            //        {
            //            DataRow dr_focused = Detail_DXGridVw.GetFocusedDataRow();
            //            if (dr_focused[nameof(InstallmentDTL_Cls.RPDocKey)] != DBNull.Value && (long)dr_focused[nameof(InstallmentDTL_Cls.RPDocKey)] > 0)
            //            {
            //                try
            //                {
            //                    AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting).GetARPayment((long)dr_focused[nameof(InstallmentDTL_Cls.RPDocKey)]);
            //                    if (doc_RP != null) AutoCount.ARAP.ARPayment.FormARPaymentCmd.ViewDocument(AutoCount.Authentication.UserSession.CurrentUserSession, doc_RP.DocKey);
            //                }
            //                catch (Exception ex)
            //                {
            //                    ErrorLogger_Cls.Write(this.Name + "." + nameof(Detail_DXGridVw_DoubleClick) + "()", ex);
            //                }
            //            }

            //            break;
            //        }
            //}
        }

        protected virtual void Detail_DXGridVw_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (this.Method == FormMethods.VIEW) return;

            //DataRow dr_focused = Detail_DXGridVw.GetFocusedDataRow();
            //if (dr_focused == null) return;

            //if (e.FocusedRowHandle == 0)
            //{
            //    DG_RealPaymentAmt_GCol.OptionsColumn.AllowEdit = (!this.InitPayment_GrpCtrl.CustomHeaderButtons["Initial Amount ?"].Properties.Checked);
            //    DG_RealPaymentDate_GCol.OptionsColumn.AllowEdit = (!this.InitPayment_GrpCtrl.CustomHeaderButtons["Initial Amount ?"].Properties.Checked);
            //}
            //else
            //{
            //    DG_RealPaymentAmt_GCol.OptionsColumn.AllowEdit = true;
            //    DG_RealPaymentDate_GCol.OptionsColumn.AllowEdit = true;
            //}

            //DataRow dr_PrevFocused = e.FocusedRowHandle > 0 ? Detail_DXGridVw.GetDataRow(e.FocusedRowHandle - 1) : null;
            //if (dr_PrevFocused == null ||
            //   GetDBValue(dr_PrevFocused, nameof(InstallmentDTL_Cls.RPDocKey), -1) > 0 ||
            //   GetDBValue(dr_PrevFocused, nameof(InstallmentDTL_Cls.RPDocNo), "") != "" ||
            //   GetDBValue(dr_PrevFocused, nameof(InstallmentDTL_Cls.IsARCreated), false))
            //    Detail_DXGridVw.OptionsBehavior.Editable = true;
            //else Detail_DXGridVw.OptionsBehavior.Editable = false;

            ////if (dr_focused[nameof(InstallmentDTL_Cls.DtlStatus)].ToString().ToUpper() == this.BusinessObject.GetBODTL().GetDtlStatusString(BusinessBaseDTL_Cls.DetailStatus.ARPYMTGENERATED).ToUpper() ||
            ////    GetDBValue(dr_focused, nameof(InstallmentDTL_Cls.RPDocKey), -1) > 0 ||
            ////    GetDBValue(dr_focused, nameof(InstallmentDTL_Cls.RPDocNo), "") != "" ||
            ////    GetDBValue(dr_focused, nameof(InstallmentDTL_Cls.IsARCreated), false) ||
            ////    System.Convert.ToDateTime(GetDBValue(dr_focused, nameof(InstallmentDTL_Cls.PlannedInstallmentDate), DateTime.MinValue)) < DateTime.Today)
            ////{
            ////    Detail_DXGridVw.OptionsBehavior.Editable = false;
            ////}
            ////else Detail_DXGridVw.OptionsBehavior.Editable = true;
        }
        #endregion " GridControl Events "

        #region " Ribbon BarButton Events "
        protected virtual void New_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // set the form method to 'EDIT' 1st
            this.Lenm_FormMethods = FormMethods.NEW;

            // reset the primary key value of this form
            Lng_DocKey = -1;

            // then just do InitForm() again shd b OK 
            InitForm();

            // set focus to main field
            //////////this.DebtorCode_MyGLEdit.Focus();
        }

        protected virtual void Edit_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // set the form method to 'EDIT' 1st
            this.Lenm_FormMethods = FormMethods.EDIT;

            // then just do InitForm() again shd b OK 
            InitForm();
        }

        protected virtual void Save_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Support Master + Doc editor by capability policy
            //if ((int)this.BusinessObject.DocStatusValue <= 1)
            //{
            //    // set Document Status to 'PROCESSING'
            //    this.BusinessObject.DocStatusValue = BusinessBase_Cls.DocumentStatus.PROCESSING;
            //    this.BusinessObject.DocStatus = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);
            //}
            //
            //// save data
            //string str_savedata = this.Save();
            //if (str_savedata == "")
            //{
            //    // generate AutoCount Invoice Doc directly here
            //    str_savedata = this.BusinessObject.SaveAutoCountIVRec();
            //    if (str_savedata == "")
            //    {
            //        this.BusinessObject.Save();
            //
            //        AutoCount.AppMessage.ShowInformationMessage(this, "Save Successful!");
            //    }
            //    else AutoCount.AppMessage.ShowWarningMessage(this, "Save Successful! But generate AutoCount Invoice Doc has following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
            //
            //    this.FormIsModified = false;
            //
            //    if (this.ProceedAfterSave_BarChkItem.Checked)
            //    {
            //        this.Lenm_FormMethods = FormMethods.NEW;
            //
            //        Lng_DocKey = -1;
            //
            //        InitForm();
            //    }
            //    else this.Close();
            //}
            //else AutoCount.AppMessage.ShowWarningMessage(this, "Save Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

            if (this.BusinessObject == null)
            {
                AutoCount.AppMessage.ShowWarningMessage(this, "No Business Object, cannot save.");
                return;
            }

            if (HasCapability(BaseEditorCapabilities.DocStatusWorkflow))
            {
                if ((int)this.BusinessObject.DocStatusValue <= 1)
                {
                    this.BusinessObject.DocStatusValue = BusinessBase_Cls.DocumentStatus.PROCESSING;
                    this.BusinessObject.DocStatus = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);
                }
            }

            // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Ensure validation focus state is clean for each save attempt
            ResetValidationErrorFocusControl();

            string str_savedata = this.Save();
            if (str_savedata == "")
            {
                if (HasCapability(BaseEditorCapabilities.AutoCountDocs))
                {
                    str_savedata = this.BusinessObject.SaveAutoCountIVRec();
                    if (str_savedata == "")
                    {
                        str_savedata = this.BusinessObject.Save();
                        if (str_savedata == "")
                            AutoCount.AppMessage.ShowInformationMessage(this, "Save Successful!");
                        else
                            AutoCount.AppMessage.ShowWarningMessage(this, "Save Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
                    }
                    else
                    {
                        AutoCount.AppMessage.ShowWarningMessage(this, "Save Successful! But generate AutoCount Invoice Doc has following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
                    }
                }
                else
                {
                    // str_savedata = this.BusinessObject.Save();
                    // if (str_savedata == "")
                    //     AutoCount.AppMessage.ShowInformationMessage(this, "Save Successful!");
                    // else
                    //     AutoCount.AppMessage.ShowWarningMessage(this, "Save Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

                    // [edited] by scchang's GPT-5.2 on 20260121: v1.0.0.0, Prevent double-save (this.Save() already called BusinessObject.Save())
                    str_savedata = "";
                    AutoCount.AppMessage.ShowInformationMessage(this, "Save Successful!");
                }

                if (str_savedata == "")
                {
                    this.FormIsModified = false;

                    if (HasCapability(BaseEditorCapabilities.ProceedAfterSave) && this.ProceedAfterSave_BarChkItem != null && this.ProceedAfterSave_BarChkItem.Checked)
                    {
                        this.Lenm_FormMethods = FormMethods.NEW;
                        Lng_DocKey = -1;
                        InitForm();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                AutoCount.AppMessage.ShowWarningMessage(this, "Save Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

                // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Focus back to invalid control after user closes warning message
                TryFocusValidationErrorControl();
            }
        }

        public virtual void Delete_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!HasDocKey("Delete")) return;

            if (!CheckingDocStatusValue("Delete")) return;

            DeleteDlg_Form frm_del = new DeleteDlg_Form(FormMethods.DELETE, BusinessObject, this.DocKey);
            //frm_del.DocKey = this.DocKey;
            frm_del.ShowDialog();

            if (frm_del.DialogResult == DialogResult.Yes)
            {
                if (frm_del.FormIsSaved)
                {
                    this.FormIsSaved = true;

                    //if (this.BusinessObject.IVDocKey > 0)
                    //{
                    //    string str_DeleteIV = this.BusinessObject.DeleteAutoCountIVRec();
                    //    if (str_DeleteIV != "")
                    //    {
                    //        AutoCount.AppMessage.ShowWarningMessage(this, "Delete AutoCount Invoice [" + this.BusinessObject.IVDocNo + "] FAILED!\n\n(NOTE: Please kindly refer to your developer.)");

                    //        return;
                    //    }
                    //}

                    AutoCount.AppMessage.ShowInformationMessage(this, "Delete Successful!");

                    this.FormIsSaved = true;

                    // if dialogresult is YES and rec deleted, then close the form
                    this.Close();
                }
            }
        }

        // added by chang on 20200106: v1.8.2.11, added 'CANCEL' feature
        protected virtual void Cancel_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!HasDocKey("Cancel")) return;

            if (!CheckingDocStatusValue("Cancel")) return;

            if (AutoCount.AppMessage.ShowConfirmMessage(this, "Cancel", "Are you sure you want to Cancel this Document?" + Constants.vbCrLf + "(Note: This action might CANCEL also the related Invoice Document(s) transferred)"))
            {
                // set Document Status to 'CANCELLED'
                this.BusinessObject.DocStatusValue = BusinessBase_Cls.DocumentStatus.CANCELLED;
                this.BusinessObject.DocStatus = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);

                //////////if (Detail_DXGrid.DataSource != null)
                //////////{
                //////////    DataTable dt_Detail = (DataTable)Detail_DXGrid.DataSource;
                //////////    foreach (DataRow dr in dt_Detail.Rows)
                //////////    {
                //////////        dr[nameof(BusinessBaseDTL_Cls.DtlStatusValue)] = BusinessBaseDTL_Cls.DetailStatus.CANCELLED;
                //////////        dr[nameof(BusinessBaseDTL_Cls.DtlStatus)] = this.BusinessObject.Detail.GetDtlStatusString(BusinessBaseDTL_Cls.DetailStatus.CANCELLED);
                //////////    }
                //////////}

                string str_savedata = this.Save();
                if (str_savedata == "")
                {
                    // cancel Autocount Invoice Doc directly here
                    str_savedata = this.BusinessObject.CancelAutoCountIVRec();
                    if (str_savedata == "")
                        AutoCount.AppMessage.ShowInformationMessage(this, "Cancel Successful!");
                    else
                        AutoCount.AppMessage.ShowWarningMessage(this, "Cancel AutoCount Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

                    this.FormIsModified = false;

                    this.Close();
                }
                else
                {
                    AutoCount.AppMessage.ShowWarningMessage(this, "Cancel Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

                    // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0
                    TryFocusValidationErrorControl();
                }
            }
        }

        protected virtual void Terminate_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!HasDocKey("Terminate")) return;

            if (!CheckingDocStatusValue("Terminated")) return;

            if (AutoCount.AppMessage.ShowConfirmMessage(this, "Terminate", "Are you sure you want to Terminate this Document?" + Constants.vbCrLf + "(Note: This action might affected also the related Invoice Document(s) transferred)"))
            {
                // set Document Status to 'TERMINATED'
                this.BusinessObject.DocStatusValue = BusinessBase_Cls.DocumentStatus.TERMINATED;
                this.BusinessObject.DocStatus = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);

                #region " [commented] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Broken if/else structure (syntax error) "
                /*
                string str_savedata = this.Save();
                if (str_savedata == "")
                {
                    //////////// Terminate Autocount Invoice Doc directly here
                    //////////str_savedata = this.BusinessObject.CancelAutoCountIVRec();
                    //////////if (str_savedata == "")
                    //////////    AutoCount.AppMessage.ShowInformationMessage(this, "Terminate Successful!");
                    //////////else
                    //////////    AutoCount.AppMessage.ShowWarningMessage(this, "Terminate AutoCount Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

                else
                {
                    AutoCount.AppMessage.ShowWarningMessage(this, "Terminate Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
                    TryFocusValidationErrorControl();
                }
                    this.FormIsModified = false;

                    this.Close();
                }
                else
                    AutoCount.AppMessage.ShowWarningMessage(this, "Terminate Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
                */
                #endregion " [commented] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Broken if/else structure (syntax error) "

                // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Correct if/else structure + focus-back UX
                string str_savedata = this.Save();
                if (str_savedata == "")
                {
                    //////////// Terminate Autocount Invoice Doc directly here
                    //////////str_savedata = this.BusinessObject.CancelAutoCountIVRec();
                    //////////if (str_savedata == "")
                    //////////    AutoCount.AppMessage.ShowInformationMessage(this, "Terminate Successful!");
                    //////////else
                    //////////    AutoCount.AppMessage.ShowWarningMessage(this, "Terminate AutoCount Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);

                    this.FormIsModified = false;
                    this.Close();
                }
                else
                {
                    AutoCount.AppMessage.ShowWarningMessage(this, "Terminate Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
                    TryFocusValidationErrorControl();
                }
            }
        }

        protected virtual void Complete_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Guard for Master editor
            if (!HasCapability(BaseEditorCapabilities.DocStatusWorkflow))
            {
                AutoCount.AppMessage.ShowWarningMessage(this, "This function is not applicable.");
                return;
            }

            if (!HasDocKey("Complete")) return;

            // if no details then don't perform complete action
            if (!HasCapability(BaseEditorCapabilities.DetailGrid) || this.Detail_DXGrid == null || this.Detail_DXGrid.DataSource == null)
            {
                AutoCount.AppMessage.ShowWarningMessage(this, "No any Detail yet, cannot perform Complete!");

                return;
            }

            if (((DataTable)this.Detail_DXGrid.DataSource).Rows.Count <= 0)
            {
                AutoCount.AppMessage.ShowWarningMessage(this, "No any Detail yet, cannot perform Complete!");

                return;
            }

            //if header status <> "ENDCOLLECTIONS" then cannot perform COMPLETE action
            if (this.BusinessObject.DocStatusValue != BusinessBase_Cls.DocumentStatus.FINISHCOLLECTIONS)
            {
                AutoCount.AppMessage.ShowWarningMessage(this, "The Status was NOT [" + this.BusinessObject.GetDocStatusString(BusinessBase_Cls.DocumentStatus.FINISHCOLLECTIONS) + "] yet, cannot perform Complete!");

                return;
            }

            if (AutoCount.AppMessage.ShowConfirmMessage(this, "Complete", "Are you sure you want to Complete this Document?"))
            {
                // set Document Status to 'COMPLETED'
                this.BusinessObject.DocStatusValue = BusinessBase_Cls.DocumentStatus.COMPLETED;
                this.BusinessObject.DocStatus = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);

                string str_savedata = "";
                //if (this.BusinessObject.IVDocKey <= 0) str_savedata = this.BusinessObject.SaveAutoCountIVRec();
                if (str_savedata == "")
                {
                    str_savedata = this.BusinessObject.Save();
                    if (str_savedata == "")
                    {
                        AutoCount.AppMessage.ShowInformationMessage(this, "Complete Successful!");

                        this.FormIsModified = false;

                        this.Close();
                    }
                }
                else AutoCount.AppMessage.ShowWarningMessage(this, "Complete Document Failed! Due to following error: " + Constants.vbCrLf + Constants.vbCrLf + str_savedata);
            }
        }

        protected virtual void Print_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // ''''ReportType = "Pharmis AddOn Tax Goods Received Note"
            // ''''ReportType = "Goods Received Note Document"
            // ''''myBasicReportInfo = New Autocount.Report.ReportInfo("", "", "", "")
            // ''''Me.BusinessObject.PrintReport(ReportType, myBasicReportInfo)

            // ''''Return

            // call report preview form
            Report("Preview");
        }

        protected virtual void PrintWithDefaultPrinter_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // call print report and directly print on default printer
            Report("Print");
        }

        protected virtual void PrintWithPrinterDialog_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // call print report and show the printer dialog 1st
            Report("Print", true);
        }

        protected virtual void PrintWithDesigner_BarBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Report("Design");
        }

        protected virtual void Close_BarBtn_ItemClick(System.Object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Close form
            this.Close();
        }

        #region " BarButton Processes "
        protected virtual bool HasDocKey(string str_Action, bool boo_ShowWarningMsg = true)
        {
            bool boo_rtn = true;

            try
            {
                if (this.DocKey <= 0)
                {
                    if (boo_ShowWarningMsg) AutoCount.AppMessage.ShowWarningMessage(this, "The data NOT yet SAVED, cannot perform [" + str_Action + "]!");

                    boo_rtn = false; ;
                }
            }
            catch (Exception ex)
            {
                boo_rtn = false; ;

                ErrorLogger_Cls.Write(this.Name + "." + nameof(HasDocKey) + "()", ex);
                AutoCount.AppMessage.ShowErrorMessage(this, "Failed to [" + str_Action + "] due to: " + ex.Message);
            }

            return boo_rtn;
        }

        protected virtual bool CheckingDocStatusValue(string str_Action)
        {
            bool boo_rtn = true;

            try
            {
                // [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Master editor does not enforce DocStatus workflow
                if (!HasCapability(BaseEditorCapabilities.DocStatusWorkflow)) return true;

                if ((int)this.BusinessObject.DocStatusValue > 1)
                {
                    if (str_Action.ToUpper() != BusinessBase_Cls.DocumentStatus.TERMINATED.ToString().ToUpper() && this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.STARTCOLLECTIONS) AutoCount.AppMessage.ShowWarningMessage(this, "Cannot perform [" + str_Action + "] due to AutoCount INVOICE already started RECEIVED PAYMENT!");
                    if (this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.COMPLETED || this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.CANCELLED || this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.FINISHCOLLECTIONS) AutoCount.AppMessage.ShowWarningMessage(this, "Cannot perform [" + str_Action + "] due to this document has been [" + this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue).ToUpper() + "]!");

                    boo_rtn = false;
                }
            }
            catch (Exception ex)
            {
                boo_rtn = false; ;

                ErrorLogger_Cls.Write(this.Name + "." + nameof(CheckingDocStatusValue) + "()", ex);
                AutoCount.AppMessage.ShowErrorMessage(this, "Failed to [" + str_Action + "] due to: " + ex.Message);
            }

            return boo_rtn;
        }
        #endregion " BarButton Processes "
        #endregion " Ribbon BarButton Events "

        #region " SimpleButton Events "
        protected virtual void AddDetail_SpBtn_Click(object sender, EventArgs e)
        {
            //InstallmentDetail_Form frm_WODetail = new InstallmentDetail_Form(FormMethods.NEW);
            //frm_WODetail.ParentBusinessObject = this.BusinessObject;
            //frm_WODetail.TempDtlKey = this.BusinessObject.TempDtlKey;
            ////frm_WODetail.BusinessObject = BusinessObject;
            ////DataRow dr_new = BusinessObject.GetDetailTable().NewRow();
            ////dr_new["DtlKey"] = -1;
            ////frm_WODetail.WorkOrderDTLDataRows.Add(dr_new);
            //frm_WODetail.ShowDialog();

            //if (frm_WODetail.IsDataSaved)
            //{
            //    UpdateDataFromWODTLForm(frm_WODetail.BusinessObject);
            //}
        }

        protected virtual void EditDetail_SpBtn_Click(object sender, EventArgs e)
        {
            //if (Detail_DXGridVw.FocusedRowHandle < 0) return;

            //InstallmentDetail_Form frm_WODetail = new InstallmentDetail_Form(FormMethods.EDIT);
            //frm_WODetail.ParentBusinessObject = this.BusinessObject;
            //frm_WODetail.ItemCode = this.Detail_DXGridVw.GetFocusedDataRow()["ItemCode"].ToString();
            //frm_WODetail.TempDtlKey = this.BusinessObject.TempDtlKey;
            ////////////frm_WODetail.BusinessObject = BusinessObject;
            ////////////frm_WODetail.WorkOrderDTLDataRow.Add(Detail_DXGridVw.GetFocusedDataRow());
            //frm_WODetail.ShowDialog();

            //if (frm_WODetail.IsDataSaved)
            //{
            //    UpdateDataFromWODTLForm(frm_WODetail.BusinessObject);
            //}
        }

        protected virtual void DeleteDetail_SpBtn_Click(object sender, EventArgs e)
        {
            //if (Detail_DXGridVw.FocusedRowHandle < 0) return;

            //DialogResult diag_Delete = MessageBox.Show("Do you want to DELETE this ItemCode [" + Detail_DXGridVw.GetFocusedDataRow()["ItemCode"].ToString() + "] data ?", "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (diag_Delete == DialogResult.Yes)
            //{
            //    //need to delete the previous MotherCoil & Baby Coil record(s)
            //    DataRow[] drs_del = BusinessObject.GetDetailTable().Select("DtlKey=" + Detail_DXGridVw.GetFocusedDataRow()["DtlKey"].ToString());
            //    for (int int_i = 0; int_i < drs_del.Length; int_i++)
            //    {
            //        BusinessObject.DeleteDetail(drs_del[int_i], false);
            //    }

            //    ((DataTable)Detail_DXGrid.DataSource).AcceptChanges();
            //}

            //this.BusinessObject.DeleteDetail(Detail_DXGridVw.GetFocusedDataRow());
        }
        #endregion " SimpleButton Events "

        #region " SearchLookupEdit Events "
        #endregion " SearchLookupEdit Events "

        #region " TextEdit Events "
        #endregion " TextEdit Events "

        #region " SpinEdit Events "
        #endregion " SpinEdit Events "

        #region " GroupControl Events "
        #endregion " GroupControl Events "

        #region " DateEdit Events "
        #endregion " DateEdit Events "

        #region " Bar Check Item Events "
        protected virtual void ProceedAfterSave_BarChkItem_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ProceedNewAfterSave = this.ProceedAfterSave_BarChkItem.Checked;
        }
        #endregion " Bar Check Item Events "

        #region " [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Runtime configuration validation (Doc-mode safety net) "
        /// <summary>
        /// Runtime safety net for BaseEditor virtual configuration.
        /// Only runs at runtime (not in Designer) to avoid silent misconfiguration when derived forms forget to override required properties.
        /// </summary>
        protected virtual void ValidateRuntimeRequirements()
        {
            if (IsDesignTime) return;

            ArrayList arr_issues = new ArrayList();

            if (this.BusinessObject == null)
            {
                arr_issues.Add("BusinessObject is not assigned. Derived form should set BusinessObject before calling base.InitForm().");
            }

            if (HasCapability(BaseEditorCapabilities.ProceedAfterSave) && string.IsNullOrWhiteSpace(this.ProceedNewAfterSaveSysConfigName))
            {
                arr_issues.Add("ProceedNewAfterSaveSysConfigName is empty (ProceedAfterSave enabled). Please override and provide a sys config key.");
            }

            if (HasCapability(BaseEditorCapabilities.Printing) && string.IsNullOrWhiteSpace(this.ReportType))
            {
                arr_issues.Add("ReportType is empty (Printing enabled). Please override and provide a report type.");
            }

            if (HasCapability(BaseEditorCapabilities.Printing) && string.IsNullOrWhiteSpace(this.PrintDesignAccessRightsCMD))
            {
                arr_issues.Add("PrintDesignAccessRightsCMD is empty (Printing enabled). Print design permission will be disabled unless overridden.");
            }

            if (HasCapability(BaseEditorCapabilities.DetailGrid) && this.BusinessObject != null && this.BusinessObject.Detail == null)
            {
                arr_issues.Add("BusinessObject.Detail is null (DetailGrid enabled). Please ensure BO.Detail is initialized.");
            }

            if (arr_issues.Count > 0)
            {
                string str_msg = "BaseEditor configuration error:" + Constants.vbCrLf + Constants.vbCrLf;
                for (int int_i = 0; int_i <= arr_issues.Count - 1; int_i++)
                {
                    str_msg += "- " + arr_issues[int_i].ToString() + Constants.vbCrLf;
                }

                str_msg += Constants.vbCrLf + "Please contact developer.";

                AutoCount.AppMessage.ShowErrorMessage(this, str_msg);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(ValidateRuntimeRequirements) + "()", new InvalidOperationException(str_msg));
                throw new InvalidOperationException(str_msg);
            }
        }
        #endregion " [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Runtime configuration validation (Doc-mode safety net) "

        #region " Repository Item Control Events "
        #endregion " Repository Item Control Events "

        #region " Form Refresh/Display "
        private bool Lboo_FormLoading = true;

        // use to initialize required info 1st
        protected virtual void DoInit(FormMethods enm_FormMethods, long docKey = -1)
        {

        }

        // use to initialize form
        protected virtual void InitForm()
        {
            Lboo_FormLoading = true;

            // [added] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Apply capability policy early
            ApplyEditorCapabilitiesToUI();

            // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Runtime safety net for virtual configuration
            ValidateRuntimeRequirements();

            // by default form's editable is True
            this.Editable = true;

            // to load saved Grid Control layout
            LoadControlsLayout(this.Controls);

            // to load data base on DocKey
            if (this.DocKey > 0)
                // if form method NOT 'NEW' then need to load the business obj 1st
                if (this.Method != FormMethods.NEW) this.BusinessObject.Load(this.DocKey.ToString());

            #region " DocStatus assignment "
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, DocStatus workflow is optional (Master editors)
            //if (this.Method != FormMethods.DELETE &&
            //    (this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.CANCELLED ||
            //    this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.COMPLETED ||
            //    this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.FINISHCOLLECTIONS ||
            //    this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.TERMINATED))
            //{
            //    AutoCount.AppMessage.ShowInformationMessage(this, "This Document has been [" + this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue).ToUpper() + "], you are NOT ABLE to EDIT this document.");
            //
            //    this.Lenm_FormMethods = FormMethods.VIEW;
            //}

            if (HasCapability(BaseEditorCapabilities.DocStatusWorkflow))
            {
                if (this.Method != FormMethods.DELETE &&
                    (this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.CANCELLED ||
                    this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.COMPLETED ||
                    this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.FINISHCOLLECTIONS ||
                    this.BusinessObject.DocStatusValue == BusinessBase_Cls.DocumentStatus.TERMINATED))
                {
                    AutoCount.AppMessage.ShowInformationMessage(this, "This Document has been [" + this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue).ToUpper() + "], you are NOT ABLE to EDIT this document.");

                    this.Lenm_FormMethods = FormMethods.VIEW;
                }
            }
            #endregion " DocStatus assignment "

            switch (this.Method)
            {
                case FormMethods.NEW:
                    {
                        // enable controls
                        EnableControls(this, true);

                        InitControlsDefaultValue(this);
                        break;
                    }

                case FormMethods.EDIT:
                    {
                        // enable controls
                        EnableControls(this, true);
                        break;
                    }

                case FormMethods.DELETE:
                    {
                        // enable controls
                        EnableControls(this, false);

                        this.Editable = false;
                        break;
                    }

                case FormMethods.VIEW:
                    {
                        // enable controls
                        EnableControls(this, false);

                        this.Editable = false;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            // start to load data
            InitSimpleButtons();
            LoadData();
            InitSpecificDisplay();

            // to set readonly for those special fields
            if (this.Lenm_FormMethods == FormMethods.EDIT || this.Lenm_FormMethods == FormMethods.NEW)
            {
                //EnableControls(this.SODocNo_TxtEdit, false);
                //EnableControls(this.IVDocNo_TxtEdit, false);
            }
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Guard DocFields/DocStatusWorkflow
            //if ((int)this.BusinessObject.DocStatusValue > 1)
            //{
            //    EnableControls(this.DocNo_TxtEdit, false);
            //    EnableControls(this.DocDate_DteEdit, false);
            //}
            if (HasCapability(BaseEditorCapabilities.DocStatusWorkflow) && HasCapability(BaseEditorCapabilities.DocFields) && (int)this.BusinessObject.DocStatusValue > 1)
            {
                if (this.DocNo_TxtEdit != null) EnableControls(this.DocNo_TxtEdit, false);
                if (this.DocDate_DteEdit != null) EnableControls(this.DocDate_DteEdit, false);
            }

            // to register controls' modified events
            Control thisForm = (Control)this;
            StartupRegisterEvents(ref thisForm);

            Lboo_FormLoading = false;
        }

        // use to initialize Simple Buttons Controls
        protected virtual void InitSimpleButtons()
        {
            // form manipulate buttons
            // ''''Me.Edit_BarBtn.Enabled = (Me.Method = FormMethods.VIEW AndAlso Me.BusinessObject.Status.ToUpper() <> "COMPLETED") 'boo_enabled 'removed by chang on 20200105: v1.8.2.11, to add in NEW status 'CANCEL'
            //this.Edit_BarBtn.Enabled = (this.Method == FormMethods.VIEW && (this.BusinessObject.Status.ToUpper() != "COMPLETED" || this.BusinessObject.Status.ToUpper() != "CANCELLED")); // boo_enabled 'added by chang on 20200105: v1.8.2.11, to add in NEW status 'CANCEL'
            this.Edit_BarBtn.Enabled = (this.Method == FormMethods.VIEW);
            this.Save_BarBtn.Enabled = (this.Method == FormMethods.NEW || this.Method == FormMethods.EDIT); // boo_enabled
            // ''''Me.SaveAndClose_BarBtn.Enabled = (Me.Method = FormMethods.NEW OrElse Me.Method = FormMethods.EDIT) 'boo_enabled
            // ''''Me.Delete_BarBtn.Enabled = ((Me.Method = FormMethods.EDIT OrElse Me.Method = FormMethods.VIEW) AndAlso Me.BusinessObject.Status.ToUpper <> "COMPLETED") 'boo_enabled 'removed by chang on 20200105: v1.8.2.11, to add in NEW status 'CANCEL'
            //this.Delete_BarBtn.Enabled = ((this.Method == FormMethods.EDIT || this.Method == FormMethods.VIEW) && (this.BusinessObject.Status.ToUpper() != "COMPLETED" || this.BusinessObject.Status.ToUpper() != "CANCELLED")); // boo_enabled 'added by chang on 20200105: v1.8.2.11, to add in NEW status 'CANCEL'
            this.Delete_BarBtn.Enabled = ((this.Method == FormMethods.EDIT || this.Method == FormMethods.VIEW)); // boo_enabled
            this.Terminate_BarBtn.Enabled = ((this.Method == FormMethods.EDIT || this.Method == FormMethods.VIEW)); // boo_enabled

            // special handling buttons
            this.GenerateACDocs_BarBtn.Enabled = (this.Method == FormMethods.NEW || this.Method == FormMethods.EDIT); // boo_enabled

            // print handling buttons
            // ''''If Me.Method = FormMethods.EDIT OrElse Me.Method = FormMethods.VIEW Then Me.Print_BarBtnGrp.Visibility = BarItemVisibility.Always
            this.Print_BarBtn.Enabled = (this.Method == FormMethods.EDIT || this.Method == FormMethods.VIEW); // boo_enabled
                                                                                                              // ''''Me.PrintWithDefaultPrinter_BarBtn.Enabled = (Me.Method = FormMethods.EDIT OrElse Me.Method = FormMethods.VIEW) 'boo_enabled
            this.PrintWithPrinterDialog_BarBtn.Enabled = (this.Method == FormMethods.EDIT || this.Method == FormMethods.VIEW); // boo_enabled

            // status buttons
            // '' ''Me.Activate_BarBtn.Enabled = boo_enabled
            // '' ''Me.Deactivate_BarBtn.Enabled = boo_enabled
            this.Cancel_BarBtn.Enabled = (this.Method == FormMethods.EDIT); // boo_enabled 'added by chang on 20200105: v1.8.2.11, to add in NEW status 'CANCEL'
            this.Complete_BarBtn.Enabled = (this.Method == FormMethods.EDIT); // boo_enabled

            // print designs button
            if (PrintDesignPermission)
                this.PrintWithDesigner_BarBtn.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            else
                this.PrintWithDesigner_BarBtn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            this.Close_BarBtn.Enabled = true; // this button suppose always enabled
        }

        // use to load data for this form's document
        protected virtual void LoadData()
        {
            #region " load data for HEADER "
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Master editor may not use DocNo/DocDate
            //this.DocNo_TxtEdit.EditValue = this.BusinessObject.DocNo;
            //this.DocDate_DteEdit.EditValue = this.BusinessObject.DocDate;
            if (HasCapability(BaseEditorCapabilities.DocFields))
            {
                if (this.DocNo_TxtEdit != null) this.DocNo_TxtEdit.EditValue = this.BusinessObject.DocNo;
                if (this.DocDate_DteEdit != null) this.DocDate_DteEdit.EditValue = this.BusinessObject.DocDate;
            }

            if (Lboo_FormLoading)
            {
                //this.AreaName_Lbl.Text = "";
                //this.ItemName_Lbl.Text = "";
                //this.DebtorTypeDesc_Lbl.Text = "";
            }

            //this.InstallmentCount_MyGLEdit.EditValue = this.BusinessObject.InstallmentCount;

            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Status is DocWorkflow-only
            //this.Status_Lbl.Text = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);
            if (HasCapability(BaseEditorCapabilities.DocStatusWorkflow))
            {
                if (this.Status_Lbl != null) this.Status_Lbl.Text = this.BusinessObject.GetDocStatusString(this.BusinessObject.DocStatusValue);
            }
            #endregion " load data for HEADER "

            // load datagrid data
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Detail may be null
            //this.Detail_DXGrid.DataSource = this.BusinessObject.Detail.DetailDataTable;
            if (HasCapability(BaseEditorCapabilities.DetailGrid) && this.Detail_DXGrid != null && this.BusinessObject.Detail != null)
            {
                this.Detail_DXGrid.DataSource = this.BusinessObject.Detail.DetailDataTable;
            }

            // get value for Continue New after Save
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, ProceedAfterSave is optional
            //this.ProceedAfterSave_BarChkItem.Checked = ProceedNewAfterSave;
            if (HasCapability(BaseEditorCapabilities.ProceedAfterSave) && this.ProceedAfterSave_BarChkItem != null)
                this.ProceedAfterSave_BarChkItem.Checked = ProceedNewAfterSave;
        }

        // use to initialize specific display
        protected virtual void InitSpecificDisplay()
        {
            #region " load data source for the grid lookup edit "
            //// Debtor
            //DataTable dt_Debtor = myDBSetting.GetDataTable("Select AccNo as DebtorCode, CompanyName as DebtorName, Address1, Address2, Address3, Address4, SalesAgent, DebtorType, AreaCode, Phone1, Mobile from Debtor where IsActive='T' union select DebtorCode, DebtorName, Debtor.Address1, Debtor.Address2, Debtor.Address3, Debtor.Address4, Debtor.SalesAgent, U1DebtorType as DebtorType, zINS_Installment.AreaCode, PhoneNo as Phone1, Debtor.Mobile from zINS_Installment inner join Debtor on zINS_Installment.DebtorCode=Debtor.AccNo where DocKey=" + this.Lng_DocKey.ToString() + " Order By DebtorCode", false);
            //this.DebtorCode_MyGLEdit.Properties.DataSource = dt_Debtor;

            //if (dt_Debtor != null) dt_Debtor.Dispose();
            //dt_Debtor = null;
            #endregion " load data source for the grid lookup edit "

            // assigns form's title
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Use DisplayKey for Master editors
            //this.Text = this.Tag.ToString() + " Form [" + this.Method.ToString("G") + "] - " + this.DocNo_TxtEdit.Text;
            string str_displayKey = HasCapability(BaseEditorCapabilities.DocFields) && this.DocNo_TxtEdit != null ? this.DocNo_TxtEdit.Text : GetBusinessObjectDisplayKeyText();
            this.Text = this.Tag.ToString() + " Form [" + this.Method.ToString("G") + "]" + (string.IsNullOrWhiteSpace(str_displayKey) ? "" : " - " + str_displayKey);

            // assign ProceedAfterSave_BarChkItem text
            // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, ProceedAfterSave is optional
            //ProceedAfterSave_BarChkItem.Caption = "After save, proceed with new " + ProceedAfterSave_BarChkItem.Tag.ToString();
            if (HasCapability(BaseEditorCapabilities.ProceedAfterSave) && this.ProceedAfterSave_BarChkItem != null)
                ProceedAfterSave_BarChkItem.Caption = "After save, proceed with new " + ProceedAfterSave_BarChkItem.Tag.ToString();

            //// make Debtor GridLookUpEdit 1st focus 'added by chang on 20191030: v1.8.2.8
            //this.ActiveControl = DebtorCode_MyGLEdit;
        }
        #endregion " Form Refresh/Display "

        #region " Data Manipulation section "
        // use to Save data
        protected virtual string Save()
        {
            string str_rtn = "";

            try
            {
                // [added] by scchang's GPT-5.2 on 20260123: v1.0.0.0, Reset focus target before validation
                ResetValidationErrorFocusControl();

                // here need to check the fields that need to be validate b4 save
                string str_chkvalid = DataValidation();
                if (str_chkvalid != "")
                    return str_chkvalid;

                switch (this.Method)
                {
                    case FormMethods.NEW:
                        {
                            break;
                        }

                    case FormMethods.EDIT:
                        {
                            break;
                        }
                }

                // here will collect all the information that need to be save
                if (CollectBusinessObjectValues())
                    str_rtn = this.BusinessObject.Save();
                else
                    str_rtn = "Some information were not enough to proceed to save.";

                // if successful saved then need to be initialize again the specific display
                if (str_rtn == "")
                {
                    if (this.Lenm_FormMethods == FormMethods.NEW)
                    {
                        this.Lenm_FormMethods = FormMethods.EDIT;
                        this.Lng_DocKey = this.BusinessObject.PrimaryKey;
                    }

                    // just do InitForm() again shd b OK 
                    InitForm();
                }
            }
            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(Save) + "()", ex);

                // return the error messages
                str_rtn = ex.Message;
            }

            return str_rtn;
        }

        // use to Check Data Validation
        protected virtual string DataValidation()
        {
            string str_rtn = "";

            try
            {
                #region " DocNo assignment "
                // ------------------------------------------------------------------------------------------------------------------------------------
                // START - if DocNo_TxtEdit is '<<New>>' then try to auto assign a value for it
                // P.S. we hard code for the name then fixed it to get the doc no format
                // ==================================================================================
                // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, DocNo auto-assign is capability based
                //if (DocNo_TxtEdit.Text == AutoCount.Const.AppConst.NewDocumentNo || DocNo_TxtEdit.Text == "")
                if (HasCapability(BaseEditorCapabilities.DocNoAutoAssign) && HasCapability(BaseEditorCapabilities.DocFields) && DocNo_TxtEdit != null && (DocNo_TxtEdit.Text == AutoCount.Const.AppConst.NewDocumentNo || DocNo_TxtEdit.Text == ""))
                {
                    #region " remarked area "
                    // ''''Dim dt_docnofmt As DataTable = myDBSetting.GetDataTable("SELECT * FROM DocNoFormat WHERE Name='Receiving'", False)
                    // ''''If dt_docnofmt.Rows.Count <= 0 Then
                    // ''''    str_sqlcmd = "INSERT INTO DocNoFormat (Name, DocType, NextNumber, Format, Sample, IsDefault, OneMonthOneSet) values ('Receiving', 'GR', 2, 'RCV-{yyyyMM}-<000000>', 'RCV-" & Now.ToString("yyyyMM") & "-000001', 'F', 'F')"
                    // ''''    myDBSetting.ExecuteNonQuery(str_sqlcmd)

                    // ''''    str_newDocNo = "RCV-" & Now.ToString("yyyyMM") & "-000001"
                    // ''''Else
                    // ''''    'get the doc no format
                    // ''''    str_format = dt_docnofmt.Rows(0)("Format")

                    // ''''    'get the length of the auto number
                    // ''''    str_nextno = str_format.Substring(str_format.IndexOf("<") + 1, str_format.Length - str_format.IndexOf("<") - 2)

                    // ''''    'set the auto number
                    // ''''    If dt_docnofmt.Rows(0)("NextNumber").ToString().Length > str_nextno.Length Then
                    // ''''        str_newnextno = "1".PadLeft(str_nextno.Length, Convert.ToChar("0"))
                    // ''''        dt_docnofmt.Rows(0)("NextNumber") = 1
                    // ''''    Else
                    // ''''        str_newnextno = dt_docnofmt.Rows(0)("NextNumber").ToString().PadLeft(str_nextno.Length, Convert.ToChar("0"))
                    // ''''    End If

                    // ''''    'assign new doc no according to the selected format
                    // ''''    str_newDocNo = str_format.Replace("{yyyyMM}", Now.ToString("yyyyMM")).Replace("<000000>", str_newnextno)

                    // ''''    'update back the next auto number for the selected doc no format
                    // ''''    str_sqlcmd = "UPDATE DocNoFormat set NextNumber=" & dt_docnofmt.Rows(0)("NextNumber") + 1 & " WHERE Name='Receiving'"
                    // ''''    myDBSetting.ExecuteNonQuery(str_sqlcmd)
                    // ''''End If
                    #endregion " remarked area "

                    DocNo_TxtEdit.Text = this.BusinessObject.GetNewDocNo();
                }
                // ==================================================================================
                // END - if DocNo_TxtEdit is '<<New>>' then try to auto assign a value for it
                // ------------------------------------------------------------------------------------------------------------------------------------
                #endregion " DocNo assignment "

                #region " DocStatus assignment "
                // ----------------------------------------------------------------------------------------------------------------------------------------------
                // START - according to Ricky at 20190830, this customer need to receive the goods 1st and do some investigation then only decide the ClaimStatus
                // ==============================================================================================================================================
                // to check ClaimStatus, MUST HAVE Value otherwise when import to StockIssue Doc some items may not know need to generate CN or not (WHEN COMPLETE action ONLY)
                //////////if (this.BusinessObject.Status.ToUpper() == "COMPLETED")
                //////////{
                //////////    ((DataTable)this.Detail_DXGrid.DataSource).AcceptChanges();

                //////////    bool boo_NoClaimStatus = false;
                //////////    for (int int_i = 0; int_i <= ((DataTable)Detail_DXGrid.DataSource).Rows.Count - 1; int_i++)
                //////////    {
                //////////        if (((DataTable)Detail_DXGrid.DataSource).Rows[int_i]["ClaimStatus"] == DBNull.Value || ((DataTable)Detail_DXGrid.DataSource).Rows[int_i]["ClaimStatus"].ToString() == "")
                //////////        {
                //////////            boo_NoClaimStatus = true;

                //////////            break;
                //////////        }
                //////////    }
                //////////    if (boo_NoClaimStatus)
                //////////    {
                //////////        return str_rtn += "Some detail info of 'Claim Status' still not filled in, it MUST HAVE Value";
                //////////    }
                //////////}
                // ==============================================================================================================================================
                // END - according to Ricky at 20190830, this customer need to receive the goods 1st and do some investigation then only decide the ClaimStatus
                // ----------------------------------------------------------------------------------------------------------------------------------------------
                #endregion " DocStatus assignment "

                #region " Data Validation "
                //if (DebtorCode_MyGLEdit.EditValue == null || DebtorCode_MyGLEdit.EditValue.ToString() == "") str_rtn += "Please select a [Debtor/Customer] to proceed...\n";
                //if (AreaCode_MyGLEdit.EditValue == null || AreaCode_MyGLEdit.EditValue.ToString() == "") str_rtn += "Please select a [Area] to proceed...\n";
                //if (ItemCode_MyGLEdit.EditValue == null || ItemCode_MyGLEdit.EditValue.ToString() == "") str_rtn += "Please select a [Item/Product] to proceed...\n";
                //if (Price_SpEdit.EditValue == null || Price_SpEdit.Value <= 0) str_rtn += "Please fill up the [Bill Amt] to proceed...\n";
                //if (U1DebtorType_MyGLEdit.EditValue == null || U1DebtorType_MyGLEdit.EditValue.ToString() == "") str_rtn += "Please select a [U1/Debtor Type] to proceed...\n";
                //if (U2PayOffDay_MyGLEdit.EditValue == null || U2PayOffDay_MyGLEdit.EditValue.ToString() == "") str_rtn += "Please select a [U2/Pay Off Day] to proceed...\n";
                //if (InstallmentCount_MyGLEdit.EditValue == null || InstallmentCount_MyGLEdit.EditValue.ToString() == "") str_rtn += "Please select a [Installment Terms] to proceed...\n";

                //if (InitPayment_GrpCtrl.CustomHeaderButtons["Initial Amount ?"].Properties.Checked)
                //{
                //    if (InitPaymentAmt_SpEdit.EditValue == null || (decimal)InitPaymentAmt_SpEdit.EditValue <= 0) str_rtn += "Please assign [Initial Payment Amount] to proceed...\n";
                //    //if ((DateTime)InitPaymentDate_DteEdit.EditValue <DateTime.Now) str_rtn += "Please assign a valid 'Initial Payment Date' to proceed...";
                //}

                if (str_rtn != "") str_rtn = str_rtn.Trim('\n');
                #endregion " Data Validation "
            }

            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(DataValidation) + "()", ex);
            }

            return str_rtn;
        }

        // use to collect business obj values for save data
        protected virtual bool CollectBusinessObjectValues()
        {
            bool boo_rtn = true;

            try
            {
                #region " HEADER "
                // load values from HEADER
                // =======================
                // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Master editor may not use DocNo/DocDate
                //this.BusinessObject.DocNo = this.DocNo_TxtEdit.EditValue.ToString();
                //this.BusinessObject.DocDate = this.DocDate_DteEdit.DateTime;
                if (HasCapability(BaseEditorCapabilities.DocFields))
                {
                    if (this.DocNo_TxtEdit != null && this.DocNo_TxtEdit.EditValue != null)
                        this.BusinessObject.DocNo = this.DocNo_TxtEdit.EditValue.ToString();

                    if (this.DocDate_DteEdit != null)
                        this.BusinessObject.DocDate = this.DocDate_DteEdit.DateTime;
                }
                //this.BusinessObject.DebtorCode = this.DebtorCode_MyGLEdit.EditValue.ToString();
                //this.BusinessObject.DebtorName = this.DebtorName_Lbl.Text;
                //this.BusinessObject.AreaCode = this.AreaCode_MyGLEdit.EditValue.ToString();
                //this.BusinessObject.ItemCode = this.ItemCode_MyGLEdit.EditValue.ToString();
                //this.BusinessObject.Qty = this.Qty_SpEdit.Value;
                //this.BusinessObject.UOM = this.UOM_Lbl.Text;
                //this.BusinessObject.ProductPrice = this.Price_SpEdit.Value;
                //this.BusinessObject.U1DebtorType = this.U1DebtorType_MyGLEdit.EditValue.ToString();
                //this.BusinessObject.U2PayOffDay = this.U2PayOffDay_MyGLEdit.EditValue.ToString();
                //this.BusinessObject.PhoneNo = this.PhoneNo_TxtEdit.Text;
                //this.BusinessObject.Remarks = this.Remarks_MyMemoEdit.Text;
                //this.BusinessObject.InstallmentCount = System.Convert.ToInt32(this.InstallmentCount_MyGLEdit.EditValue);

                ////Init Payment Portion
                //this.BusinessObject.HasInitPayment = this.InitPayment_GrpCtrl.CustomHeaderButtons["Initial Amount ?"].Properties.Checked;
                //if (this.InitPayment_GrpCtrl.CustomHeaderButtons["Initial Amount ?"].Properties.Checked)
                //{
                //    this.BusinessObject.InitPayment = InitPaymentAmt_SpEdit.Value;
                //    this.BusinessObject.InitPaymentDate = InitPaymentDate_DteEdit.DateTime;
                //}
                //else
                //{
                //    this.BusinessObject.InitPayment = 0;
                //    this.BusinessObject.InitPaymentDate = DateTime.MinValue;
                //}
                #endregion " HEADER "

                // load values from datagrid data (no need to collect datagrid obj values, cos it's oredy linked with the business obj)
                // [edited] by scchang's GPT-5.2 on 20260112: v1.0.0.0, Detail is optional
                //if (this.Detail_DXGrid.DataSource != null)
                if (HasCapability(BaseEditorCapabilities.DetailGrid) && this.Detail_DXGrid != null && this.Detail_DXGrid.DataSource != null)
                {
                    ((DataTable)this.Detail_DXGrid.DataSource).AcceptChanges();

                    if (this.BusinessObject.Detail != null)
                        boo_rtn = this.BusinessObject.SetDetailTable((DataTable)this.Detail_DXGrid.DataSource);
                }
            }
            catch (Exception ex)
            {
                AutoCount.AppMessage.ShowErrorMessage(this, "Errors due to: " + Strings.Chr(13) + ex.Message + Strings.Chr(13) + ex.StackTrace);
                ErrorLogger_Cls.Write(this.Name + "." + nameof(CollectBusinessObjectValues) + "()", ex);

                boo_rtn = false;
            }

            return boo_rtn;
        }
        #endregion " Data Manipulation section "

        #region " Method/Sub/Procedure/Function "
        #endregion " Method/Sub/Procedure/Function "

        #region " Reports "
        // [removed] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Abstract blocks Visual Inheritance in VS Designer
        //protected abstract string ReportType { get; set; }
        // [added] by scchang's GPT-5.2 on 20260113: v1.0.0.0, Default to empty; derived forms should override
        protected virtual string ReportType { get; set; } = "";
        //protected abstract string ReportType = "Installment Document";
        private AutoCount.Report.BasicReportOption myBasicReportOption;
        private AutoCount.Report.ReportInfo myBasicReportInfo;
        private DataTable tm = null;
        private DataTable td = null;

        // use to preview the report
        protected virtual void Report(string str_ReportMode, bool boo_ShowPrintDialog = false)
        {
            myBasicReportInfo = new AutoCount.Report.ReportInfo("", "", "", "");
            myBasicReportOption = new AutoCount.Report.BasicReportOption();
            myBasicReportOption.ShowPrintDialog = boo_ShowPrintDialog;

            tm = myDBSetting.GetDataTable("Select * From " + SQLString(this.BusinessObject.MasterTableQryName) + " where DocKey=" + this.DocKey.ToString() + "", false);
            td = myDBSetting.GetDataTable("Select * From " + SQLString(this.BusinessObject.Detail.DetailTableQryName) + " where DocKey=" + this.DocKey.ToString() + "", false);

            DataTable dt_CompanyProfile = myDBSetting.GetDataTable("Select * From Profile", false);

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
                        AutoCount.Report.ReportTool.DesignReport(ReportType, GetReportDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession);
                        break;
                    }

                case "Preview":
                    {
                        AutoCount.Report.ReportTool.PreviewReport(ReportType, GetReportDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession, true, false, myBasicReportOption, myBasicReportInfo); // added by chang on 20130502: Autocount changed the parameter list, added 'ReportInfo'
                        break;
                    }

                case "Print":
                    {
                        AutoCount.Report.ReportTool.PrintReport(ReportType, GetReportDataSource(tm, td, dt_CompanyProfile, td_user), AutoCount.Authentication.UserSession.CurrentUserSession, true, myBasicReportOption, myBasicReportInfo);
                        break;
                    }
            }

            myBasicReportInfo = null;
            myBasicReportOption = null;
            td_user = null;
        }

        // for report
        protected virtual AutoCount.Report.DocumentReportDataSet GetReportDataSource(DataTable tm, DataTable td, DataTable td_Companyprofile, DataTable td_user)
        {
            AutoCount.Report.DocumentReportDataSet dsReport = new AutoCount.Report.DocumentReportDataSet(AutoCount.Authentication.UserSession.CurrentUserSession, ReportType, "Master", "Detail");

            tm.TableName = "Master";
            td.TableName = "Detail";
            td_Companyprofile.TableName = "Company Profile";
            td_user.TableName = "TableUser";

            dsReport.Tables.Add(tm);
            dsReport.Tables.Add(td);
            dsReport.Tables.Add(td_Companyprofile);
            dsReport.Tables.Add(td_user);

            DataRelation DataRelationMasterDetail = new DataRelation("MasterDetailRelation", tm.Columns[nameof(DocKey)], td.Columns[nameof(DocKey)]);
            dsReport.Relations.Add(DataRelationMasterDetail);

            return dsReport;
        }
        #endregion " Reports "
    }
}
