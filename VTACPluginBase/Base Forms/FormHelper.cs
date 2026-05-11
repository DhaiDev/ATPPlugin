using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using VTACPluginBase.BaseForms.Editor;
using VTACPluginBase.BaseForms.List;

namespace VTACPluginBase.BaseForms
{
    public class FormHelper
    {
        #region " Enums "
        /// <summary>
        /// Use to identified the form's method, this enum shd declared in every editor form(s)
        /// </summary>
        /// <remarks></remarks>
        public enum FormMethods
        {
            NEW,    // New document for the form
            EDIT,   // Edit document for the form
            DELETE, // Delete document for the form
            VIEW    // View document for the form
        }
        #endregion " Enums "

        #region " Classes "
        public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
        {
            public AbstractControlDescriptionProvider()
                : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
            {
            }

            public override Type GetReflectionType(Type objectType, object instance)
            {
                //if (objectType == typeof(TAbstract)) //ori version
                if (objectType.FullName == typeof(TAbstract).FullName) //modified version
                    return typeof(TBase);

                return base.GetReflectionType(objectType, instance);
            }

            public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
            {
                //if (objectType == typeof(TAbstract)) //ori version
                if (objectType.FullName == typeof(TAbstract).FullName) //modified version
                    objectType = typeof(TBase);

                return base.CreateInstance(provider, objectType, argTypes, args);
            }
        }
        #endregion " Classes "

        #region " Common Form Handling "
        #region " Default Properties "
        private bool Lboo_Editable = false;
        public bool Editable
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
        public bool FormIsModified
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

        private bool Lboo_FormIsSaved = false;
        public bool FormIsSaved
        {
            get
            {
                return Lboo_FormIsSaved;
            }
            set
            {
                Lboo_FormIsSaved = value;
            }
        }
        #endregion " Default Properties "

        #region " Form Handling Helpers "
        // use to register events (basically is for changed events)
        public void StartupRegisterEvents(ref Control obj_DerivedForm)
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

        public static void InitControlsDefaultValue(Control obj_SrcControl)
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
        public static void EnableControls(Control obj_SrcControl, bool boo_Enable)
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
        #endregion " Common Form Handling "
    }

    public class BaseListFormMiddle1 : BaseList_Form
    {
        public BaseListFormMiddle1() { }

        protected override string EditorFormClass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string TableName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string TableQueryName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string DetailTableName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string DetailTableQueryName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string NewDocAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string EditDocAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string ViewDocAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string PrintPreviewAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string PrintAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string PrintDesignAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string DeleteDocAccessRightsCMD { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override Type BusinessObjectType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string ReportType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override string ReportListingType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    class BaseListFormMiddle2 : BaseListFormMiddle1  // empty class, just to make the VS designer working
    { }
}
