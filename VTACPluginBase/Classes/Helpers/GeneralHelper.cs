using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.Helpers
{
    public class GeneralHelper
    {
        /// <summary>
        /// Use to format SQL Stmt's string (if SQL Stmt got "'")
        /// </summary>
        /// <remarks></remarks>
        public static string SQLString(string str_SQL)
        {
            return Strings.Replace(str_SQL, "'", "''");
        }

        /// <summary>
        /// Use to get datarow value
        /// </summary>
        /// <remarks></remarks>
        public static dynamic GetDBValue(DataRow dr, string FieldName, object DefaultValue)
        {
            if (dr.Table.Columns.Contains(FieldName))
            {
                if (dr[FieldName] == DBNull.Value) return DefaultValue;

                switch (dr.Table.Columns[FieldName].DataType.Name)
                {
                    case "string":
                    case "String":
                        return System.Convert.ToString(dr[FieldName]);
                    case "bool":
                    case "Boolean":
                        return System.Convert.ToBoolean(dr[FieldName]);
                    case "int":
                    case "Int32":
                        return System.Convert.ToInt32(dr[FieldName]);
                    case "Int64":
                    case "long":
                        return System.Convert.ToInt64(dr[FieldName]);
                    case "decimal":
                    case "Decimal":
                        return System.Convert.ToDecimal(dr[FieldName]);
                    case "double":
                    case "Double":
                        return System.Convert.ToDouble(dr[FieldName]);
                    case "DateTime":
                        return System.Convert.ToDateTime(dr[FieldName]);
                    default:
                        return (dr[FieldName] == DBNull.Value ? DefaultValue : dr[FieldName].ToString());
                }
            }
            else return DefaultValue;
        }

        /// <summary>
        /// Use to save Default DevExpress GridView Layout (in Memory Stream Only)
        /// </summary>
        /// <remarks></remarks>
        public static void SaveDefaultDataGridControlLayout(DevExpress.XtraGrid.Views.Grid.GridView DXGridVw, ref System.IO.Stream dg_stream)
        {
            DXGridVw.SaveLayoutToStream(dg_stream, DevExpress.Utils.OptionsLayoutBase.FullLayout);
            dg_stream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        /// <summary>
        /// Use to save Default DevExpress Pivot GridView Layout (in Memory Stream Only)
        /// </summary>
        /// <remarks></remarks>
        public static void SaveDefaultDataGridControlLayout(DevExpress.XtraPivotGrid.PivotGridControl DXPvtGridVw, ref System.IO.Stream dg_stream)
        {
            DXPvtGridVw.SaveLayoutToStream(dg_stream, DevExpress.Utils.OptionsLayoutBase.FullLayout);
            dg_stream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        /// <summary>
        /// Use to save UI Layout
        /// </summary>
        /// <remarks></remarks>
        public static void SaveControlsLayout(System.Windows.Forms.Control.ControlCollection obj_Controls)
        {
            //added by chang on 20230131: need to check folder existence here else will hits error
            if (!System.IO.Directory.Exists(AutoCountProfile()))
                System.IO.Directory.CreateDirectory(AutoCountProfile());

            foreach (System.Windows.Forms.Control obj_Control in obj_Controls)
            {
                ////////added by chang on 20230131: need to check also the file existence //removed by chang on 20230210: seems no need this check process
                //////if (!System.IO.File.Exists(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml"))
                //////    continue;

                #region " GridControl/PivotGridControl "
                //////////if (obj_Control is DevExpress.XtraGrid.GridControl) //removed by chang on 20210429: v2.0.1.0
                if (obj_Control is DevExpress.XtraGrid.GridControl || obj_Control is DevExpress.XtraPivotGrid.PivotGridControl) //added by chang on 20210429: v2.0.1.0, add in also the PivotGridControl
                {
                    object obj = myDBSetting.ExecuteScalar("select count(*) from dbo.sysobjects where id = object_id(N'[dbo].[zPOS_SysUserProfile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1");
                    if ((obj != null && obj != DBNull.Value && (int)obj > 0))
                    {
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        ((DevExpress.XtraGrid.GridControl)obj_Control).Views[0].SaveLayoutToStream(stream, DevExpress.Utils.OptionsLayoutBase.FullLayout);
                        DataTable dt = myDBSetting.GetDataTable("SELECT ProfileID, UserName, Profile, ProfileName, ModDate FROM zPOS_SysUserProfile WHERE UserName = '" + SQLString(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID) + "' AND ProfileName = '" + SQLString(obj_Control.FindForm().Name + "." + obj_Control.Name) + "'", true);
                        if (dt.Rows.Count > 0)
                            dt.Rows[0]["Profile"] = stream.ToArray();
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr["UserName"] = AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID;
                            dr["Profile"] = stream.ToArray();
                            dr["ProfileName"] = SQLString(obj_Control.FindForm().Name + "." + obj_Control.Name);
                            dr["ModDate"] = DateTime.Now;
                            dt.Rows.Add(dr);
                        }
                        myDBSetting.SimpleSaveDataTable(dt, "SELECT ProfileID, UserName, Profile, ProfileName, ModDate FROM zPOS_SysUserProfile WHERE UserName = '" + SQLString(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID) + "' AND ProfileName = '" + SQLString(obj_Control.FindForm().Name + "." + obj_Control.Name) + "'");
                    }
                    else
                    {
                        if (obj_Control is DevExpress.XtraGrid.GridControl)
                            ((DevExpress.XtraGrid.GridControl)obj_Control).Views[0].SaveLayoutToXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml", DevExpress.Utils.OptionsLayoutBase.FullLayout);

                        //added by chang on 20210430: v2.0.1.0, to handle also the PivotGridControl
                        if (obj_Control is DevExpress.XtraPivotGrid.PivotGridControl)
                            ((DevExpress.XtraPivotGrid.PivotGridControl)obj_Control).SaveLayoutToXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml", DevExpress.Utils.OptionsLayoutBase.FullLayout);
                    }
                }
                #endregion " GridControl/PivotGridControl "

                #region " CheckedListBoxControl "
                //-----------------------------------------------------------------------------------------------------------------------
                //START - to handle also others controls type 'CheckedListBoxControl' //added by chang on 20210601: v2.0.1.0
                //==========================================================================================================
                if (obj_Control is DevExpress.XtraEditors.CheckedListBoxControl)
                {
                    if (((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).ItemCount > 0)
                    {
                        //declare the datatable 1st for saving 'CheckedListBoxControl' items and values into datatable and then save it into xml file
                        DataTable dt_CheckedListBoxControl = new DataTable(obj_Control.Name);

                        //declare the required columns
                        if (!dt_CheckedListBoxControl.Columns.Contains("ItemName")) dt_CheckedListBoxControl.Columns.Add("ItemName", typeof(string));
                        if (!dt_CheckedListBoxControl.Columns.Contains("ItemValue")) dt_CheckedListBoxControl.Columns.Add("ItemValue", typeof(bool));

                        DataRow dr_new = null;

                        //start to loop into object 'CheckedListBoxControl' items
                        for (int int_i = 0; int_i < ((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).ItemCount; int_i++)
                        {
                            dr_new = dt_CheckedListBoxControl.NewRow();
                            dr_new["ItemName"] = ((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).Items[int_i].Description;
                            dr_new["ItemValue"] = (((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).Items[int_i].CheckState == CheckState.Checked);

                            dt_CheckedListBoxControl.Rows.Add(dr_new);
                        }

                        dt_CheckedListBoxControl.AcceptChanges();

                        //save datatable into xml
                        dt_CheckedListBoxControl.WriteXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml", XmlWriteMode.WriteSchema);

                        //clean resources of datatable
                        if (dt_CheckedListBoxControl != null) dt_CheckedListBoxControl.Dispose();
                        dt_CheckedListBoxControl = null;
                    }
                }
                //==========================================================================================================
                //END - to handle also others controls type 'CheckedListBoxControl' //added by chang on 20210601: v2.0.1.0
                //-----------------------------------------------------------------------------------------------------------------------
                #endregion " CheckedListBoxControl "

                #region " AutoCount Controls "
                if (obj_Control is AutoCount.Controls.FilterUI.UCDebtorSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCDebtorTypeSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCSalesAgentSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCCurrencySelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCAreaSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCLocationSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemGroupSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemTypeSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCProjectSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCDepartmentSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemBrandSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemCategorySelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemClassSelector)
                {
                    AutoCount.SearchFilter.Filter filter = null;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCDebtorSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCDebtorSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCDebtorTypeSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCDebtorTypeSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCSalesAgentSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCSalesAgentSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCCurrencySelector)
                        filter = ((AutoCount.Controls.FilterUI.UCCurrencySelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCAreaSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCAreaSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCLocationSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCLocationSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCItemSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemGroupSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCItemGroupSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemTypeSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCItemTypeSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCProjectSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCProjectSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCDepartmentSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCDepartmentSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemBrandSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCItemBrandSelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemCategorySelector)
                        filter = ((AutoCount.Controls.FilterUI.UCItemCategorySelector)obj_Control).Filter;

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemClassSelector)
                        filter = ((AutoCount.Controls.FilterUI.UCItemClassSelector)obj_Control).Filter;

                    if (filter == null) filter = new AutoCount.SearchFilter.Filter("", "");

                    //declare the datatable 1st for saving AutoCount SelectorControl
                    DataTable dt_ACUCSelector = new DataTable(obj_Control.Name);

                    //declare the required columns
                    if (!dt_ACUCSelector.Columns.Contains("FilterProperty")) dt_ACUCSelector.Columns.Add("FilterProperty", typeof(string));
                    if (!dt_ACUCSelector.Columns.Contains("FilterPropertyValue")) dt_ACUCSelector.Columns.Add("FilterPropertyValue", typeof(string));

                    DataRow dr_new = null;

                    //start to assign the properties values into datatable
                    //----------------------------------------------------
                    // Property of 'From'
                    dr_new = dt_ACUCSelector.NewRow();
                    dr_new["FilterProperty"] = "From";
                    dr_new["FilterPropertyValue"] = filter.From == null ? "" : filter.From.ToString();
                    dt_ACUCSelector.Rows.Add(dr_new);

                    // Property of 'To'
                    dr_new = dt_ACUCSelector.NewRow();
                    dr_new["FilterProperty"] = "To";
                    dr_new["FilterPropertyValue"] = filter.To == null ? "" : filter.To.ToString();
                    dt_ACUCSelector.Rows.Add(dr_new);

                    // Property of 'Type'
                    dr_new = dt_ACUCSelector.NewRow();
                    dr_new["FilterProperty"] = "Type";
                    dr_new["FilterPropertyValue"] = filter.Type.ToString();
                    dt_ACUCSelector.Rows.Add(dr_new);

                    // Property of 'Prefix'
                    dr_new = dt_ACUCSelector.NewRow();
                    dr_new["FilterProperty"] = "Prefix";
                    dr_new["FilterPropertyValue"] = filter.Prefix.ToString();
                    dt_ACUCSelector.Rows.Add(dr_new);

                    // Property of 'Filter'
                    if (filter.Count > 0)
                    {
                        string str_Debtors = "";
                        for (int int_i = 0; int_i < filter.Count; int_i++)
                        {
                            if (str_Debtors.Length > 0) str_Debtors += ",";
                            str_Debtors += filter[int_i].ToString();
                        }

                        dr_new = dt_ACUCSelector.NewRow();
                        dr_new["FilterProperty"] = "Filter";
                        dr_new["FilterPropertyValue"] = str_Debtors;
                        dt_ACUCSelector.Rows.Add(dr_new);
                    }

                    dt_ACUCSelector.AcceptChanges();

                    //save datatable into xml
                    dt_ACUCSelector.WriteXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml", XmlWriteMode.WriteSchema);

                    //clean resources of datatable
                    if (dt_ACUCSelector != null) dt_ACUCSelector.Dispose();
                    dt_ACUCSelector = null;
                }
                #endregion " AutoCount Controls "

                //////////else if (obj_Control.HasChildren) //removed by chang on 20210531: v2.0.1.0
                if (obj_Control.HasChildren) //added by chang on 20210531: v2.0.1.0
                    SaveControlsLayout(obj_Control.Controls);
            }
        }

        /// <summary>
        /// Use to load UI Layout
        /// </summary>
        /// <remarks></remarks>
        public static void LoadControlsLayout(System.Windows.Forms.Control.ControlCollection obj_Controls)
        {
            //added by chang on 20210602: v2.0.1.0, moved to here
            if (!System.IO.Directory.Exists(AutoCountProfile()))
                System.IO.Directory.CreateDirectory(AutoCountProfile());

            foreach (System.Windows.Forms.Control obj_Control in obj_Controls)
            {
                #region " GridControl/PivotGridControl "
                //////////if (obj_Control is DevExpress.XtraGrid.GridControl) //removed by chang on 20210520: v2.0.1.0
                if (obj_Control is DevExpress.XtraGrid.GridControl || obj_Control is DevExpress.XtraPivotGrid.PivotGridControl) //added by chang on 20210520: v2.0.1.0, add in also the PivotGridControl
                {
                    object obj = myDBSetting.ExecuteScalar("select count(*) from dbo.sysobjects where id = object_id(N'[dbo].[zPOS_SysUserProfile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1");
                    if ((obj != null & obj != DBNull.Value & (int)obj > 0))
                    {
                        DataTable dt = myDBSetting.GetDataTable("SELECT ProfileID, UserName, Profile, ProfileName, ModDate FROM zPOS_SysUserProfile WHERE UserName = '" + SQLString(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID) + "' AND ProfileName = '" + SQLString(obj_Control.FindForm().Name + "." + obj_Control.Name) + "'", true);
                        if (dt.Rows.Count > 0 && dt.Rows[0]["Profile"] != DBNull.Value)
                        {
                            System.IO.MemoryStream stream = new System.IO.MemoryStream((byte[])dt.Rows[0]["Profile"]);
                            ((DevExpress.XtraGrid.GridControl)obj_Control).Views[0].RestoreLayoutFromStream(stream, DevExpress.Utils.OptionsLayoutBase.FullLayout);
                        }
                        else if (dt.Rows.Count == 0 || dt.Rows[0]["Profile"] != DBNull.Value)
                        {
                            DataTable dt_Default = myDBSetting.GetDataTable("SELECT ProfileID, UserName, Profile, ProfileName, ModDate FROM zPOS_SysUserProfile WHERE IsDefault = 1 AND ProfileName = '" + SQLString(obj_Control.FindForm().Name + "." + obj_Control.Name) + "'", true);
                            if (dt_Default.Rows.Count > 0 && dt_Default.Rows[0]["Profile"] != DBNull.Value)
                            {
                                System.IO.MemoryStream stream = new System.IO.MemoryStream((byte[])dt_Default.Rows[0]["Profile"]);
                                ((DevExpress.XtraGrid.GridControl)obj_Control).Views[0].RestoreLayoutFromStream(stream, DevExpress.Utils.OptionsLayoutBase.FullLayout);
                            }
                        }
                    }
                    else
                    {
                        //removed by chang on 20210602: v2.0.1.0, moved to above
                        //////////if (!System.IO.Directory.Exists(@"C:\AutoCountProfile"))
                        //////////    System.IO.Directory.CreateDirectory(@"C:\AutoCountProfile");

                        //added by chang on 20210520: v2.0.1.0, to handle also the PivotGridControl
                        if (System.IO.File.Exists(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml"))
                        {
                            if (obj_Control is DevExpress.XtraGrid.GridControl)
                                ((DevExpress.XtraGrid.GridControl)obj_Control).Views[0].RestoreLayoutFromXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml", DevExpress.Utils.OptionsLayoutBase.FullLayout);

                            if (obj_Control is DevExpress.XtraPivotGrid.PivotGridControl)
                                ((DevExpress.XtraPivotGrid.PivotGridControl)obj_Control).RestoreLayoutFromXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml", DevExpress.Utils.OptionsLayoutBase.FullLayout);
                        }
                    }
                }
                #endregion //" GridControl/PivotGridControl "

                #region " CheckedListBoxControl "
                //-----------------------------------------------------------------------------------------------------------------------
                //START - to handle also others controls type 'CheckedListBoxControl' //added by chang on 20210602: v2.0.1.0
                //==========================================================================================================
                if (obj_Control is DevExpress.XtraEditors.CheckedListBoxControl)
                {
                    if (System.IO.File.Exists(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml"))
                    {
                        //declare the datatable 1st for loading 'CheckedListBoxControl' items and values into CheckedListBoxControl
                        DataTable dt_CheckedListBoxControl = new DataTable(obj_Control.Name);
                        dt_CheckedListBoxControl.ReadXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml");

                        if (dt_CheckedListBoxControl.Rows.Count > 0)
                        {
                            for (int int_i = 0; int_i < dt_CheckedListBoxControl.Rows.Count; int_i++)
                            {
                                for (int int_j = 0; int_j < ((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).ItemCount; int_j++)
                                {
                                    if (((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).Items[int_j].Description == dt_CheckedListBoxControl.Rows[int_i]["ItemName"].ToString())
                                    {
                                        ((DevExpress.XtraEditors.CheckedListBoxControl)obj_Control).Items[int_j].CheckState = System.Convert.ToBoolean(dt_CheckedListBoxControl.Rows[int_i]["ItemValue"]) ? CheckState.Checked : CheckState.Unchecked;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                //==========================================================================================================
                //END - to handle also others controls type 'CheckedListBoxControl' //added by chang on 20210602: v2.0.1.0
                //-----------------------------------------------------------------------------------------------------------------------
                #endregion //" CheckedListBoxControl "

                #region " AutoCount Controls "
                if (obj_Control is AutoCount.Controls.FilterUI.UCDebtorSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCDebtorTypeSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCSalesAgentSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCCurrencySelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCAreaSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCLocationSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemGroupSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemTypeSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCProjectSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCDepartmentSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemBrandSelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemCategorySelector ||
                    obj_Control is AutoCount.Controls.FilterUI.UCItemClassSelector)
                {
                    AutoCount.SearchFilter.Filter filter = new AutoCount.SearchFilter.Filter("", "");

                    if (System.IO.File.Exists(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml"))
                    {
                        //declare the datatable 1st for laoding AutoCount SelectorControl
                        DataTable dt_ACUCSelector = new DataTable(obj_Control.Name);
                        dt_ACUCSelector.ReadXml(AutoCountProfile() + @"\" + obj_Control.FindForm().Name + "." + obj_Control.Name + ".xml");

                        if (dt_ACUCSelector.Rows.Count > 0)
                        {
                            for (int int_i = 0; int_i < dt_ACUCSelector.Rows.Count; int_i++)
                            {
                                if (dt_ACUCSelector.Rows[int_i]["FilterProperty"].ToString() == "From")
                                    filter.From = dt_ACUCSelector.Rows[int_i]["FilterPropertyValue"].ToString();

                                if (dt_ACUCSelector.Rows[int_i]["FilterProperty"].ToString() == "To")
                                    filter.To = dt_ACUCSelector.Rows[int_i]["FilterPropertyValue"].ToString();

                                if (dt_ACUCSelector.Rows[int_i]["FilterProperty"].ToString() == "Type")
                                {
                                    AutoCount.SearchFilter.FilterType filterType = AutoCount.SearchFilter.FilterType.None;
                                    switch (dt_ACUCSelector.Rows[int_i]["FilterPropertyValue"].ToString())
                                    {
                                        case "None":
                                            filterType = AutoCount.SearchFilter.FilterType.None;
                                            break;

                                        case "ByRange":
                                            filterType = AutoCount.SearchFilter.FilterType.ByRange;
                                            break;

                                        case "ByIndividual":
                                            filterType = AutoCount.SearchFilter.FilterType.ByIndividual;
                                            break;

                                        default:
                                            break;

                                    }
                                    filter.Type = filterType;
                                }

                                if (dt_ACUCSelector.Rows[int_i]["FilterProperty"].ToString() == "Prefix")
                                    filter.Prefix = dt_ACUCSelector.Rows[int_i]["FilterPropertyValue"].ToString();

                                if (dt_ACUCSelector.Rows[int_i]["FilterProperty"].ToString() == "Filter")
                                {
                                    if (dt_ACUCSelector.Rows[int_i]["FilterPropertyValue"].ToString().Length > 0)
                                    {
                                        string[] str_ACUCSelectors = dt_ACUCSelector.Rows[int_i]["FilterPropertyValue"].ToString().Split(new string[] { "," }, StringSplitOptions.None);
                                        if (str_ACUCSelectors.Length > 0)
                                        {
                                            for (int int_j = 0; int_j < str_ACUCSelectors.Length; int_j++)
                                            {
                                                filter.Add(str_ACUCSelectors[int_j]);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //clean resources of datatable
                        if (dt_ACUCSelector != null) dt_ACUCSelector.Dispose();
                        dt_ACUCSelector = null;
                    }

                    if (obj_Control is AutoCount.Controls.FilterUI.UCDebtorSelector)
                        ((AutoCount.Controls.FilterUI.UCDebtorSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCDebtorTypeSelector)
                        ((AutoCount.Controls.FilterUI.UCDebtorTypeSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCSalesAgentSelector)
                        ((AutoCount.Controls.FilterUI.UCSalesAgentSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCCurrencySelector)
                        ((AutoCount.Controls.FilterUI.UCCurrencySelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCAreaSelector)
                        ((AutoCount.Controls.FilterUI.UCAreaSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCLocationSelector)
                        ((AutoCount.Controls.FilterUI.UCLocationSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemSelector)
                        ((AutoCount.Controls.FilterUI.UCItemSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemGroupSelector)
                        ((AutoCount.Controls.FilterUI.UCItemGroupSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemTypeSelector)
                        ((AutoCount.Controls.FilterUI.UCItemTypeSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCProjectSelector)
                        ((AutoCount.Controls.FilterUI.UCProjectSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCDepartmentSelector)
                        ((AutoCount.Controls.FilterUI.UCDepartmentSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemBrandSelector)
                        ((AutoCount.Controls.FilterUI.UCItemBrandSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemCategorySelector)
                        ((AutoCount.Controls.FilterUI.UCItemCategorySelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);

                    if (obj_Control is AutoCount.Controls.FilterUI.UCItemClassSelector)
                        ((AutoCount.Controls.FilterUI.UCItemClassSelector)obj_Control).Initialize(AutoCount.Authentication.UserSession.CurrentUserSession, filter);
                }
                #endregion //" AutoCount Controls "

                //////////else if (obj_Control.HasChildren) //removed by chang on 20210602: v2.0.1.0
                if (obj_Control.HasChildren) //added by chang on 20210602: v2.0.1.0
                    LoadControlsLayout(obj_Control.Controls);
            }
        }

        /// <summary>
        /// Use to convert the CamelCase string to Normal Readable Pattern
        /// </summary>
        /// <remarks></remarks>
        public static string ConvertCamelCase2NormalPattern(string str_Source)
        {
            string str_rtn = "";

            try
            {
                //try to remove SPACE in between of source string 1st
                str_Source = str_Source.Replace(" ", "");

                var r = new Regex(@"
                    (?<=[A-Z])(?=[A-Z][a-z]) |
                    (?<=[^A-Z])(?=[A-Z]) |
                    (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
                str_rtn = r.Replace(str_Source, " ");

                //below codes r sample
                //string s = "Today12.3ILiveInTheUSAWithSimon";
                //Console.WriteLine("YYY{0}ZZZ", r.Replace(s, " "));
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(GeneralHelper)}.{nameof(ConvertCamelCase2NormalPattern)}()", ex);
            }

            return str_rtn;
        }
    }
}
