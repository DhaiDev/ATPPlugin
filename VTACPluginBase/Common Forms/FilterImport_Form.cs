using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DevExpress.XtraGrid.Views.Base;

using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.CommonForms
{
    public partial class FilterImport_Form : Form
    {
        public FilterImport_Form()
        {
            InitializeComponent();
        }

        #region " Variables/Constants declaration "
        private String Lstr_sql = "";
        #endregion " Variables/Constants declaration "

        #region " Properties "
        // added by chang on 20191125: v1.8.2.9, to make this form more stardardize
        // ========================================================================
        public DataTable PrevResults { get; set; } = null;
        public DataTable PartialTfrPrevResults { get; set; } = null; // added by chang on 20191202: v1.8.2.9

        public DataTable Results { get; set; } = null;
        public DataTable PartialTfrResults { get; set; } = null; // added by chang on 20191202: v1.8.2.9

        public DataTable SelResults { get; set; } = null;
        public DataTable PartialTfrSelResults { get; set; } = null; // added by chang on 20191202: v1.8.2.9

        public string Type { get; set; } = "";

        public string FieldName { get; set; } = "";
        public string PartialTfrFieldName { get; set; } = ""; // added by chang on 20191202: v1.8.2.9

        public string TabPageHeaderName { get; set; } = "";
        public string PartialTfrTabPageHeaderName { get; set; } = ""; // added by chang on 20191202: v1.8.2.9

        // to put the field name those need to select but don't want to visible in filter form (put in string and split by semicolon), example: DocKey;DtlKey;ToDocKey
        public string InvisibleFieldNames { get; set; } = "";
        public string PartialTfrInvisibleFieldNames { get; set; } = ""; // added by chang on 20191202: v1.8.2.9

        // to put the field name those can be edited in filter form (put in string and split by semicolon), example: ReqQty;UnitCost;Price 'added by chang on 20191025: v1.8.2.7
        public string EditableFieldNames { get; set; } = "";
        public string PartialTfrEditableFieldNames { get; set; } = ""; // added by chang on 20191202: v1.8.2.9

        // to put the field name those can NOT be null in filter form (put in string and split by semicolon), example: ReqQty;UnitCost;Price 'added by chang on 20191030: v1.8.2.8
        public string NotNullFieldNames { get; set; } = "";
        public string PartialTfrNotNullFieldNames { get; set; } = ""; // added by chang on 20191202: v1.8.2.9

        public bool MultiSelect { get; set; } = true;
        public bool PartialTfrMultiSelect { get; set; } = true; // added by chang on 20191202: v1.8.2.9

        public string SelectedVal { get; set; } = ""; // added by chang on 20191203: v1.8.2.9
        public string HyperlinkText { get; set; } = ""; // added by chang on 20191203: v1.8.2.9
        #endregion " Properties "

        #region " Form Event "
        private void FilterImport_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ---------------------------------
            // START - for Full Transfer portion
            // =================================
            // ''''If Pdt_Results IsNot Nothing AndAlso Pdt_Results.Rows.Count > 0 Then 'removed by chang on 20191126: v1.8.2.9
            if (Results != null && Results.Rows.Count > 0)
            {
                SelectedVal = "";
                // ''''Dim Selected_dr() As DataRow = Pdt_Results.Select("Selected=1") 'removed by chang on 20191126: v1.8.2.9
                DataRow[] Selected_dr = Results.Select("Selected=1"); // added by chang on 20191126: v1.8.2.9

                // added by chang on 20191203: v1.8.2.9, to identify now which tab page selected and then do the related handling
                if (FilterForm_DXTbCtrl.SelectedTabPage != FilterFormFullTfr_DXTbPg)
                {
                    if (Selected_dr.Length > 0)
                    {
                        foreach (DataRow dr in Selected_dr)
                            dr["Selected"] = 0;

                        Results.AcceptChanges();

                        Selected_dr = Results.Select("Selected=1");
                    }
                }

                if (Selected_dr.Length > 0)
                {
                    // to reset to original state of Pdt_SelResults DataTable if it's not nothing
                    // removed by chang on 20191126: v1.8.2.9
                    // ''''If Pdt_SelResults Is Nothing Then
                    // ''''    Pdt_SelResults = Pdt_Results.Clone
                    // ''''Else
                    // ''''    Pdt_SelResults.Clear()
                    // ''''    Pdt_SelResults.Reset()
                    // ''''End If
                    // added by chang on 20191126: v1.8.2.9
                    if (SelResults == null)
                        SelResults = Results.Clone();
                    else
                    {
                        SelResults.Clear();
                        SelResults.Reset();
                    }

                    foreach (DataRow dr in Selected_dr)
                    {
                        // check for not null values of fields
                        // ''''If Pstr_NotNullFieldNames IsNot Nothing OrElse Pstr_NotNullFieldNames <> "" Then 'removed by chang on 20191126: v1.8.2.9
                        if (NotNullFieldNames != null || NotNullFieldNames != "")
                        {
                            // ''''Dim str_NotNullFields As String() = Pstr_NotNullFieldNames.Split(";") 'removed by chang on 20191126: v1.8.2.9
                            string[] str_NotNullFields = NotNullFieldNames.Split(Convert.ToChar(";")); // added by chang on 20191126: v1.8.2.9
                            if (str_NotNullFields.Length > 0)
                            {
                                for (int int_i = 0; int_i <= str_NotNullFields.Length - 1; int_i++)
                                {
                                    if (str_NotNullFields[int_i] == "")
                                        continue;

                                    if (dr[str_NotNullFields[int_i]] == DBNull.Value)
                                    {
                                        AutoCount.AppMessage.ShowInformationMessage(str_NotNullFields[int_i] + " is NULL, please kindly fill up the values");

                                        e.Cancel = true;

                                        return;
                                    }
                                }
                            }
                        }

                        // put into selected string
                        if (SelectedVal.Length > 0)
                            SelectedVal += ",";
                        // ''''Pstr_Select &= "'" & dr.Item(Pstr_FieldName) & "'" 'removed by chang on 20191126: v1.8.2.9
                        SelectedVal += "'" + dr[FieldName] + "'"; // added by chang on 20191126: v1.8.2.9

                        // import selected DataRow into Pdt_SelResults
                        // ''''Pdt_SelResults.ImportRow(dr) 'removed by chang on 20191126: v1.8.2.9
                        SelResults.ImportRow(dr); // added by chang on 20191126: v1.8.2.9
                    }

                    // ''''Pstr_Select = Pstr_FieldName & " In(" & Pstr_Select & ") AND " 'removed by chang on 20191126: v1.8.2.9
                    SelectedVal = FieldName + " In(" + SelectedVal + ") AND "; // added by chang on 20191126: v1.8.2.9
                }

                if (Selected_dr.Length == 1)
                    HyperlinkText = "1 was selected";
                else if (Selected_dr.Length > 1)
                    HyperlinkText = "" + Selected_dr.Length + " were selected";
                else
                    HyperlinkText = "None was selected";

                // just remove the 'Selected' column for return selected results
                // ''''If Pdt_SelResults IsNot Nothing AndAlso Pdt_SelResults.Columns.Contains("Selected") Then Pdt_SelResults.Columns.Remove("Selected") 'removed by chang on 20191126: v1.8.2.9
                if (SelResults != null && SelResults.Columns.Contains("Selected"))
                    SelResults.Columns.Remove("Selected"); // added by chang on 20191126: v1.8.2.9
            }
            else HyperlinkText = "None was selected";
            // =================================
            // END - for Full Transfer portion
            // ---------------------------------

            // ------------------------------------
            // START - for Partial Transfer portion
            // ====================================
            // added by chang on 20191203: v1.8.2.9
            if (PartialTfrResults != null && PartialTfrResults.Rows.Count > 0)
            {
                // ''''_SelectedVal = ""
                DataRow[] Selected_dr = PartialTfrResults.Select("Selected=1"); // added by chang on 20191126: v1.8.2.9

                if (FilterForm_DXTbCtrl.SelectedTabPage != FilterFormPartialTfr_DXTbPg)
                {
                    if (Selected_dr.Length > 0)
                    {
                        foreach (DataRow dr in Selected_dr)
                            dr["Selected"] = 0;

                        PartialTfrResults.AcceptChanges();

                        Selected_dr = PartialTfrResults.Select("Selected=1");
                    }
                }

                if (Selected_dr.Length > 0)
                {
                    // to reset to original state of Pdt_SelResults DataTable if it's not nothing
                    if (PartialTfrSelResults == null)
                        PartialTfrSelResults = PartialTfrResults.Clone();
                    else
                    {
                        PartialTfrSelResults.Clear();
                        PartialTfrSelResults.Reset();
                    }

                    foreach (DataRow dr in Selected_dr)
                    {
                        // check for not null values of fields
                        if (PartialTfrNotNullFieldNames != null || PartialTfrNotNullFieldNames != "")
                        {
                            string[] str_NotNullFields = PartialTfrNotNullFieldNames.Split(Convert.ToChar(";"));
                            if (str_NotNullFields.Length > 0)
                            {
                                for (int int_i = 0; int_i <= str_NotNullFields.Length - 1; int_i++)
                                {
                                    if (str_NotNullFields[int_i] == "")
                                        continue;

                                    if (dr[str_NotNullFields[int_i]] == DBNull.Value)
                                    {
                                        AutoCount.AppMessage.ShowInformationMessage(str_NotNullFields[int_i] + " is NULL, please kindly fill up the values");

                                        e.Cancel = true;

                                        return;
                                    }
                                }
                            }
                        }

                        // '''''put into selected string
                        // ''''If _SelectedVal.Length > 0 Then _SelectedVal &= ","
                        // ''''_SelectedVal &= "'" & dr.Item(_PartialTfrFieldName) & "'"

                        // import selected DataRow into Pdt_SelResults
                        PartialTfrSelResults.ImportRow(dr);
                    }
                }

                // ''''If Selected_dr.Length = 1 Then
                // ''''    _HyperlinkText = "1 was selected"
                // ''''ElseIf Selected_dr.Length > 1 Then
                // ''''    _HyperlinkText = "" & Selected_dr.Length & " were selected"
                // ''''Else
                // ''''    _HyperlinkText = "None was selected"
                // ''''End If

                // just remove the 'Selected' column for return selected results
                if (PartialTfrSelResults != null && PartialTfrSelResults.Columns.Contains("Selected"))
                    PartialTfrSelResults.Columns.Remove("Selected");
            }
        }
        #endregion " Form Event "

        #region " Simple Button Event "
        private void Cancel_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // ''''Pdt_Results = Pdt_PrevResults 'removed by chang on 20191126: v1.8.2.9
            Results = PrevResults; // added by chang on 20191126: v1.8.2.9
            PartialTfrResults = PartialTfrPrevResults; // added by chang on 20191203: v1.8.2.9
            this.Close(); // close form
        }

        private void CheckAllFullTfr_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // ''''If Pdt_Results Is Nothing Then Exit Sub 'removed by chang on 20191126: v1.8.2.9
            if (Results == null)
                return; // added by chang on 20191126: v1.8.2.9

            // removed by chang on 20191126: v1.8.2.9
            // ''''If Not Pdt_Results.Columns.Contains("Selected") Then
            // ''''    Pdt_Results.Columns.Add("Selected", GetType(Boolean))
            // ''''End If
            // added by chang on 20191126: v1.8.2.9
            if (!Results.Columns.Contains("Selected"))
                Results.Columns.Add("Selected", typeof(bool));

            // ''''For Each dr_iv As DataRow In Pdt_Results.Rows 'removed by chang on 20191126: v1.8.2.9
            foreach (DataRow dr_iv in Results.Rows) // added by chang on 20191126: v1.8.2.9
                dr_iv["Selected"] = true;

            // ''''Me.FilterItem_DXGrid.DataSource = Pdt_Results 'removed by chang on 20191126: v1.8.2.9
            this.FilterItemFullTfr_DXGrid.DataSource = Results; // added by chang on 20191126: v1.8.2.9
        }

        private void CheckAllPartialTfr_SBtn_Click(System.Object sender, System.EventArgs e) // added by chang on 20191203: v1.8.2.9
        {
            if (PartialTfrResults == null) return;

            if (!PartialTfrResults.Columns.Contains("Selected"))
                PartialTfrResults.Columns.Add("Selected", typeof(bool));

            foreach (DataRow dr_iv in PartialTfrResults.Rows)
                dr_iv["Selected"] = true;

            this.FilterItemPartialTfr_DXGrid.DataSource = PartialTfrResults;
        }

        private void UnCheckAllFullTfr_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // ''''If Pdt_Results Is Nothing Then Exit Sub 'removed by chang on 20191126: v1.8.2.9
            if (Results == null) return; // added by chang on 20191126: v1.8.2.9

            // removed by chang on 20191126: v1.8.2.9
            // ''''If Not Pdt_Results.Columns.Contains("Selected") Then
            // ''''    Pdt_Results.Columns.Add("Selected", GetType(Boolean))
            // ''''End If
            // added by chang on 20191126: v1.8.2.9
            if (!Results.Columns.Contains("Selected"))
                Results.Columns.Add("Selected", typeof(bool));

            // ''''For Each dr_iv As DataRow In Pdt_Results.Rows 'removed by chang on 20191126: v1.8.2.9
            foreach (DataRow dr_iv in Results.Rows) // added by chang on 20191126: v1.8.2.9
                dr_iv["Selected"] = false;

            // ''''Me.FilterItem_DXGrid.DataSource = Pdt_Results 'removed by chang on 20191126: v1.8.2.9
            this.FilterItemFullTfr_DXGrid.DataSource = Results; // added by chang on 20191126: v1.8.2.9
        }

        private void UnCheckAllPartialTfr_SBtn_Click(System.Object sender, System.EventArgs e) // added by chang on 20191203: v1.8.2.9
        {
            if (PartialTfrResults == null) return;

            if (!PartialTfrResults.Columns.Contains("Selected"))
                PartialTfrResults.Columns.Add("Selected", typeof(bool));

            foreach (DataRow dr_iv in PartialTfrResults.Rows)
                dr_iv["Selected"] = false;

            this.FilterItemPartialTfr_DXGrid.DataSource = PartialTfrResults;
        }

        private void OK_SBtn_Click(System.Object sender, System.EventArgs e)
        {
            // ''''If Pdt_Results.Select("Selected=1").Length <= 0 Then 'removed by chang on 20191126: v1.8.2.9
            // ''''If _Results.Select("Selected=1").Length <= 0 Then 'added by chang on 20191126: v1.8.2.9
            if ((Results.Select("Selected=1").Length <= 0 && PartialTfrResults == null) || (Results.Select("Selected=1").Length <= 0 && PartialTfrResults != null && PartialTfrResults.Select("Selected=1").Length <= 0))
                AutoCount.AppMessage.ShowInformationMessage("No Data is Selected ");
            else
            {
                // set dialog result to OK
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                // close form
                this.Close();
            }
        }
        #endregion " Simple Button Event "

        #region " GridControl Event "
        // added by chang on 20200522: v1.8.2.17, previously use CellValueChanged was wrong shd use CellValueChanging
        private void FilterItemFullTfr_DXGridVw_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.Name.ToUpper() == "SELECTED")
            {
                // ''''If Not Pboo_MultiSelect Then 'removed by chang on 20191126: v1.8.2.9
                if (!MultiSelect)
                {
                    if (System.Convert.ToBoolean(e.Value))
                    {
                        for (int int_i = 0; int_i <= FilterItemFullTfr_DXGridVw.RowCount - 1; int_i++)
                        {
                            if (int_i != e.RowHandle)
                                FilterItemFullTfr_DXGridVw.GetDataRow(int_i)["Selected"] = false;
                        }
                    }
                }
            }
        }

        private void FilterItemPartialTfr_DXGridVw_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.Name.ToUpper() == "SELECTED")
            {
                if (!PartialTfrMultiSelect)
                {
                    if (System.Convert.ToBoolean(e.Value))
                    {
                        for (int int_i = 0; int_i <= FilterItemPartialTfr_DXGridVw.RowCount - 1; int_i++)
                        {
                            if (int_i != e.RowHandle)
                                FilterItemPartialTfr_DXGridVw.GetDataRow(int_i)["Selected"] = false;
                        }
                    }
                }
            }
        }
        #endregion " GridControl Event "

        #region " Other Functions "
        // ''''Public Sub ShowMe(ByVal str_SQL As String, ByVal str_FieldName As String) 'removed by chang on 20191202: v1.8.2.9
        public void ShowMe(string str_SQL, string str_FieldName, string str_SQLPartialTfr = "", string str_FieldNamePartialTfr = "") // added by chang on 20191202: v1.8.2.9, add in new param list to handle partial import
        {
            try
            {
                // ''''Me.Text = If(Me.Text Is Nothing OrElse Me.Text = "", Pstr_TabPageHeaderName, Me.Text) 'removed by chang on 20191126: v1.8.2.9
                this.Text = this.Text == null || this.Text == "" ? TabPageHeaderName : this.Text; // added by chang on 20191126: v1.8.2.9
                // ''''FilterForm_DXTbPg.Text = Pstr_TabPageHeaderName 'removed by chang on 20191126: v1.8.2.9
                FilterFormFullTfr_DXTbPg.Text = TabPageHeaderName; // added by chang on 20191126: v1.8.2.9
                FilterFormPartialTfr_DXTbPg.Text = PartialTfrTabPageHeaderName; // added by chang on 20191202: v1.8.2.9

                // ''''Pstr_FieldName = str_FieldName 'removed by chang on 20191126: v1.8.2.9
                FieldName = str_FieldName; // added by chang on 20191126: v1.8.2.9
                PartialTfrFieldName = str_FieldNamePartialTfr; // added by chang on 20191202: v1.8.2.9

                // ---------------------------------
                // START - For Full Transfer TabPage
                // =================================
                // ''''If Pdt_Results Is Nothing Then Pdt_Results = myDBSetting.GetDataTable(str_SQL, False) 'will pre-assign before call this Sub, so just check first before using Autocount's GetDataTable function 'removed by chang on 20191126: v1.8.2.9
                if (Results == null) Results = myDBSetting.GetDataTable(str_SQL, false); // will pre-assign before call this Sub, so just check first before using Autocount's GetDataTable function 'added by chang on 20191126: v1.8.2.9
                
                // ''''If Pdt_Results IsNot Nothing Then 'removed by chang on 20191126: v1.8.2.9
                if (Results != null)
                {
                    // Create Grid Columns based on SQL query
                    // ''''CreateColumn(Pdt_Results, FilterItem_DXGridVw) 'removed by chang on 20191126: v1.8.2.9
                    CreateColumn(Results, ref FilterItemFullTfr_DXGridVw); // added by chang on 20191126: v1.8.2.9

                    // ''''If Pdt_PrevResults IsNot Nothing Then 'removed by chang on 20191126: v1.8.2.9
                    if (PrevResults != null)
                    {
                        // ''''Dim drs_sel As DataRow() = Pdt_PrevResults.Select("Selected=1") 'removed by chang on 20191126: v1.8.2.9
                        DataRow[] drs_sel = PrevResults.Select("Selected=1"); // added by chang on 20191126: v1.8.2.9
                        if (drs_sel.Length > 0)
                        {
                            DataRow[] drs_find = null;
                            foreach (DataRow dr_previmp in drs_sel)
                            {
                                // ''''drs_find = Pdt_Results.Select(str_FieldName & "= '" & dr_previmp(str_FieldName).ToString() & "' ") 'removed by chang on 20191126: v1.8.2.9
                                drs_find = Results.Select(str_FieldName + "= '" + dr_previmp[str_FieldName].ToString() + "' "); // added by chang on 20191126: v1.8.2.9
                                if (drs_find.Length > 0) drs_find[0]["Selected"] = true;
                            }
                        }
                        else foreach(DataRow dr in Results.Rows) dr["Selected"] = false;
                    }

                    // ''''FilterItem_DXGrid.DataSource = Pdt_Results 'removed by chang on 20191126: v1.8.2.9
                    FilterItemFullTfr_DXGrid.DataSource = Results; // added by chang on 20191126: v1.8.2.9
                }
                // =================================
                // END - For Full Transfer TabPage
                // ---------------------------------

                // ------------------------------------
                // START - For Partial Transfer TabPage
                // ====================================
                if (PartialTfrResults == null && str_SQLPartialTfr != "")
                    PartialTfrResults = myDBSetting.GetDataTable(str_SQLPartialTfr, false); // will pre-assign before call this Sub, so just check first before using Autocount's GetDataTable function 'added by chang on 20191126: v1.8.2.9
                
                if (PartialTfrResults != null)
                {
                    // Create Grid Columns based on SQL query
                    CreateColumn(PartialTfrResults, ref FilterItemPartialTfr_DXGridVw, true); // added by chang on 20191126: v1.8.2.9

                    if (PartialTfrPrevResults != null)
                    {
                        DataRow[] drs_sel = PartialTfrPrevResults.Select("Selected=1"); // added by chang on 20191126: v1.8.2.9
                        if (drs_sel.Length > 0)
                        {
                            DataRow[] drs_find = null;
                            foreach (DataRow dr_previmp in drs_sel)
                            {
                                drs_find = PartialTfrResults.Select(str_FieldNamePartialTfr + "= '" + dr_previmp[str_FieldNamePartialTfr].ToString() + "' "); // added by chang on 20191126: v1.8.2.9
                                if (drs_find.Length > 0) drs_find[0]["Selected"] = true;
                            }
                        }
                    }
                    else foreach (DataRow dr in PartialTfrResults.Rows) dr["Selected"] = false;

                    FilterItemPartialTfr_DXGrid.DataSource = PartialTfrResults; // added by chang on 20191126: v1.8.2.9
                }
                // ====================================
                // END - For Partial Transfer TabPage
                // ------------------------------------

                FilterFormPartialTfr_DXTbPg.PageVisible = (str_SQLPartialTfr != "");

                // to identify which tab page shd b selected 'added by chang on 20191204: v1.8.2.9
                if (str_SQLPartialTfr != "" && PartialTfrPrevResults != null && PartialTfrPrevResults.Rows.Count > 0 && PartialTfrPrevResults.Select("Selected=1").Length > 0)
                    FilterForm_DXTbCtrl.SelectedTabPage = FilterFormPartialTfr_DXTbPg;

                this.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write("FilterImport_Form.ShowMe", ex);
            }
        }

        // ''''Private Sub CreateColumn(ByVal dt As DataTable, ByRef MyGridView As DevExpress.XtraGrid.Views.Grid.GridView) 'removed by chang on 20191202: v1.8.2.9
        private void CreateColumn(DataTable dt, ref DevExpress.XtraGrid.Views.Grid.GridView MyGridView, bool IsPartialTfr = false) // added by chang on 20191202: v1.8.2.9
        {
            try
            {
                foreach (DataColumn col in dt.Columns)
                {
                    // Add a column
                    DevExpress.XtraGrid.Columns.GridColumn col_Added = MyGridView.Columns.Add();

                    // Set added column properties
                    col_Added.Caption = col.ColumnName;
                    col_Added.Width = 100;
                    col_Added.MinWidth = 50;
                    col_Added.FieldName = col.ColumnName;
                    col_Added.Name = col.ColumnName;
                    if (!IsPartialTfr)
                        // ''''col_Added.OptionsColumn.AllowEdit = Array.Find(Pstr_EditableFieldNames.Split(";"), Function(s) s = col.ColumnName) IsNot Nothing 'False 'edited by chang on 20191025: v1.8.2.7 'removed by chang on 20191126: v1.8.2.9
                        col_Added.OptionsColumn.AllowEdit = Array.Find(EditableFieldNames.Split(Convert.ToChar(";")), s => s == col.ColumnName) != null; // False 'edited by chang on 20191025: v1.8.2.7 'added by chang on 20191126: v1.8.2.9
                    else
                        col_Added.OptionsColumn.AllowEdit = Array.Find(PartialTfrEditableFieldNames.Split(Convert.ToChar(";")), s => s == col.ColumnName) != null;// False 'added by chang on 20191202: v1.8.2.9
                    
                    switch (col.DataType.FullName) // added by chang on 20191025: v1.8.2.7
                    {
                        case "System.Decimal":
                            {
                                col_Added.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                                break;
                            }
                    }
                    col_Added.OptionsColumn.AllowFocus = true;
                    col_Added.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
                    col_Added.OptionsColumn.AllowIncrementalSearch = true;
                    col_Added.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                    col_Added.OptionsColumn.AllowMove = true;
                    col_Added.OptionsColumn.AllowSize = true;
                    col_Added.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
                    if (!IsPartialTfr)
                        // ''''col_Added.Visible = Not Pstr_InvisibleFieldNames.Contains(col.ColumnName) 'True 'removed by chang on 20191126: v1.8.2.9
                        // ''''col_Added.Visible = Not _InvisibleFieldNames.Contains(col.ColumnName) 'True 'added by chang on 20191126: v1.8.2.9 'removed by chang on 20200524: v1.8.2.17, shd use split and contains
                        col_Added.Visible = !InvisibleFieldNames.Split(Convert.ToChar(";")).Contains(col.ColumnName); // True 'added by chang on 20200524: v1.8.2.17, shd use split and contains
                    else
                        // ''''col_Added.Visible = Not _PartialTfrInvisibleFieldNames.Contains(col.ColumnName) 'True 'added by chang on 20191202: v1.8.2.9 'removed by chang on 20200524: v1.8.2.17, shd use split and contains
                        col_Added.Visible = !PartialTfrInvisibleFieldNames.Split(Convert.ToChar(";")).Contains(col.ColumnName);// True 'added by chang on 20200524: v1.8.2.17, shd use split and contains
                    col_Added.AppearanceCell.Options.UseTextOptions = true;
                    col_Added.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    col_Added.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;

                    if (col.DataType.FullName == "System.Boolean")
                    {
                        col_Added.ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
                        col_Added.ColumnEdit.Name = col.ColumnName + "_RChkBox";
                        col_Added.ColumnEdit.AutoHeight = false;
                        col_Added.MinWidth = 30;
                        col_Added.Width = 30;

                        if (col.ColumnName.ToUpper() == "SELECTED") col_Added.OptionsColumn.AllowEdit = true;
                    }
                    else if (col.DataType.FullName == "System.DateTime")
                    {
                        col_Added.ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
                        col_Added.ColumnEdit.Name = col.ColumnName + "_RDteEdit";
                        col_Added.ColumnEdit.AutoHeight = false;
                        col_Added.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        col_Added.DisplayFormat.FormatString = "dd-MM-yyyy";
                    }
                }
            }
            // MyGridView.OptionsView.ShowAutoFilterRow = True

            catch (Exception ex)
            {
                ErrorLogger_Cls.Write("FilterImport_Form.CreateColumn", ex);
            }
        }
        #endregion " Other Functions "
    }
}
