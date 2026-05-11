using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

using DevExpress.DataAccess.Excel;

using VTACPluginBase.Classes.TextLogger;

namespace VTACPluginBase.Classes.DevExpressExtensions
{
    public static class DXExcelDSEx_Cls
    {
        public static DataTable ToDataTableFromExcelDataSource(this ExcelDataSource excelDataSource)
        {
            IList list = ((IListSource)excelDataSource).GetList();
            DevExpress.DataAccess.Native.Excel.DataView dataView = (DevExpress.DataAccess.Native.Excel.DataView)list;
            List<PropertyDescriptor> props = dataView.Columns.ToList<PropertyDescriptor>();

            DataTable table = new DataTable();

            try
            {
                //to get the DataTable header columns
                //////////int int_CreatedonIdx = -1; //added by chang on 20211228: v2.0.1.3, to handle date if data type is double
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];

                    ////////////to handle date if data type is double //added by chang on 20211228: v2.0.1.3
                    //////////if (props[i].Name == "Created on")
                    //////////{
                    //////////    //to get the 'Created on' index for later use
                    //////////    int_CreatedonIdx = i;

                    //////////    table.Columns.Add(prop.Name, typeof(DateTime));
                    //////////}
                    //////////else 
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }

                //to get the UsedRange in Excel Data
                //////////int int_SalesDocTypeIdx = -1;
                object[] values = new object[props.Count];
                foreach (DevExpress.DataAccess.Native.Excel.ViewRow item in list)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);

                        ////////////to get the 'Sales Document Type' index for later use
                        //////////if (props[i].Name == "Sales Document Type") int_SalesDocTypeIdx = i;
                    }

                    //////////if (values[int_SalesDocTypeIdx].ToString().ToUpper().Trim() == "ZOR" ||
                    //////////    values[int_SalesDocTypeIdx].ToString().ToUpper().Trim() == "ZPRO" ||
                    //////////    values[int_SalesDocTypeIdx].ToString().ToUpper().Trim() == "ZSPN" ||
                    //////////    values[int_SalesDocTypeIdx].ToString().ToUpper().Trim() == "ZFNC")
                    //////////{
                    //////////    if (Information.IsNumeric(values[int_CreatedonIdx])) values[int_CreatedonIdx] = DateTime.FromOADate(System.Convert.ToDouble(values[int_CreatedonIdx])); //added by chang on 20211228: v2.0.1.3

                    table.Rows.Add(values);
                    //////////}
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write($"{nameof(DXExcelDSEx_Cls)}.{nameof(ToDataTableFromExcelDataSource)}()", ex);
            }

            return table;
        }
    }
}
