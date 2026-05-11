using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.TextLogger;

//using static VTACPluginBase.Classes.Common_Cls;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.AutoCountGenerator
{
    public static class ACHelper
    {
        #region " Fields "
        public const string Name = nameof(VTACPluginBase) + "." + nameof(Classes) + "." + nameof(AutoCountGenerator) + "." + nameof(ACHelper);
        #endregion " Fields "

        #region " Enum Portion "
        public enum DocKeyTypes
        {
            None,
            QTDocKey,
            SODocKey,
            DODocKey,
            IVDocKey,
            CNDocKey,
            DNDocKey,
            CSDocKey,
            XSDocKey,
            PODocKey,
            GRDocKey,
            XPDocKey,
            ADJDocKey,
            ISSDocKey,
            RCVDocKey,
            WOFFDocKey,
            XFERDocKey,
            RPDocKey
        }
        public enum DocNoTypes
        {
            None,
            QTDocNo,
            SODocNo,
            DODocNo,
            IVDocNo,
            CNDocNo,
            DNDocNo,
            CSDocNo,
            XSDocNo,
            PODocNo,
            GRDocNo,
            XPDocNo,
            ADJDocNo,
            ISSDocNo,
            RCVDocNo,
            WOFFDocNo,
            XFERDocNo,
            RPDocNo
        }
        #endregion " Enum Portion "

        #region " Methods/Functions/Procedures "
        
        #region " V2.0.0 Support Methods "
        public static string CheckDocTypeCorrectness(BusinessBaseV2_0_0_Cls BO, DocKeyTypes docKeyTypes, ref PropertyInfo propertyInfoDocKey, ref PropertyInfo propertyInfoDocNo)
        {
            string str_rtn = "";

            Type typ_BO = BO.GetType();

            try
            {
                string str_Msg = "";
                DocNoTypes docNoTypes = DocNoTypes.None;

                switch (docKeyTypes)
                {
                    case DocKeyTypes.QTDocKey:
                        str_Msg = "Quotation";
                        docNoTypes = DocNoTypes.QTDocNo;
                        break;
                    case DocKeyTypes.SODocKey:
                        str_Msg = "Sales Order";
                        docNoTypes = DocNoTypes.SODocNo;
                        break;
                    case DocKeyTypes.DODocKey:
                        str_Msg = "Delivery Order";
                        docNoTypes = DocNoTypes.DODocNo;
                        break;
                    case DocKeyTypes.IVDocKey:
                        str_Msg = "Invoice";
                        docNoTypes = DocNoTypes.IVDocNo;
                        break;
                    case DocKeyTypes.CNDocKey:
                        str_Msg = "Credit Note";
                        docNoTypes = DocNoTypes.CNDocNo;
                        break;
                    case DocKeyTypes.DNDocKey:
                        str_Msg = "Debit Note";
                        docNoTypes = DocNoTypes.DNDocNo;
                        break;
                    case DocKeyTypes.CSDocKey:
                        str_Msg = "Cash Sales";
                        docNoTypes = DocNoTypes.CSDocNo;
                        break;
                    case DocKeyTypes.XSDocKey:
                        str_Msg = "Cancel SO";
                        docNoTypes = DocNoTypes.XSDocNo;
                        break;
                    case DocKeyTypes.PODocKey:
                        str_Msg = "Purchase Order";
                        docNoTypes = DocNoTypes.PODocNo;
                        break;
                    case DocKeyTypes.GRDocKey:
                        str_Msg = "Goods Received";
                        docNoTypes = DocNoTypes.GRDocNo;
                        break;
                    case DocKeyTypes.XPDocKey:
                        str_Msg = "Cancel PO";
                        docNoTypes = DocNoTypes.XPDocNo;
                        break;
                    case DocKeyTypes.ADJDocKey:
                        str_Msg = "Stock Adjustment";
                        docNoTypes = DocNoTypes.ADJDocNo;
                        break;
                    case DocKeyTypes.ISSDocKey:
                        str_Msg = "Stock Issue";
                        docNoTypes = DocNoTypes.ISSDocNo;
                        break;
                    case DocKeyTypes.RCVDocKey:
                        str_Msg = "Stock Receive";
                        docNoTypes = DocNoTypes.RCVDocNo;
                        break;
                    case DocKeyTypes.WOFFDocKey:
                        str_Msg = "Stock Write Off";
                        docNoTypes = DocNoTypes.WOFFDocNo;
                        break;
                    case DocKeyTypes.XFERDocKey:
                        str_Msg = "Stock Transfer";
                        docNoTypes = DocNoTypes.XFERDocNo;
                        break;
                    case DocKeyTypes.RPDocKey:
                        str_Msg = "AR Payment";
                        docNoTypes = DocNoTypes.RPDocNo;
                        break;
                    default:
                        str_rtn = "Unknown AutoCount Document Type";
                        return str_rtn;
                }

                // Check for DocKey property
                propertyInfoDocKey = typ_BO.GetProperty(docKeyTypes.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfoDocKey == null)
                {
                    str_rtn = $"Missing Property Field [{docKeyTypes}] for generate [{str_Msg}] Document in Business Object [{BO.Name}]";
                    return str_rtn;
                }

                // Check for DocNo property
                propertyInfoDocNo = typ_BO.GetProperty(docNoTypes.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfoDocNo == null)
                {
                    str_rtn = $"Missing Property Field [{docNoTypes}] for generate [{str_Msg}] Document in Business Object [{BO.Name}]";
                    return str_rtn;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CheckDocTypeCorrectness) + "(V2)", ex);
                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " V2.0.0 Support Methods "
        
        public static string CheckDocTypeCorrectness(BusinessBase_Cls BO, DocKeyTypes docKeyTypes, ref PropertyInfo propertyInfoDocKey, ref PropertyInfo propertyInfoDocNo)
        {
            string str_rtn = "";

            Type typ_BO = BO.GetType();

            try
            {
                string str_Msg = "";
                DocNoTypes docNoTypes = DocNoTypes.None;

                switch (docKeyTypes)
                {
                    case DocKeyTypes.QTDocKey:
                        str_Msg = "Quotation";
                        docNoTypes = DocNoTypes.QTDocNo;

                        break;
                    case DocKeyTypes.SODocKey:
                        str_Msg = "Sales Order";
                        docNoTypes = DocNoTypes.SODocNo;

                        break;
                    case DocKeyTypes.DODocKey:
                        str_Msg = "Delivery Order";
                        docNoTypes = DocNoTypes.DODocNo;

                        break;
                    case DocKeyTypes.IVDocKey:
                        str_Msg = "Invoice";
                        docNoTypes = DocNoTypes.IVDocNo;

                        break;
                    case DocKeyTypes.CNDocKey:
                        str_Msg = "Credit Note";
                        docNoTypes = DocNoTypes.CNDocNo;

                        break;
                    case DocKeyTypes.DNDocKey:
                        str_Msg = "Debit Note";
                        docNoTypes = DocNoTypes.DNDocNo;

                        break;
                    case DocKeyTypes.CSDocKey:
                        str_Msg = "Cash Sales";
                        docNoTypes = DocNoTypes.CSDocNo;

                        break;
                    case DocKeyTypes.XSDocKey:
                        str_Msg = "Cancel SO";
                        docNoTypes = DocNoTypes.XSDocNo;

                        break;
                    case DocKeyTypes.PODocKey:
                        str_Msg = "Purchase Order";
                        docNoTypes = DocNoTypes.PODocNo;

                        break;
                    case DocKeyTypes.GRDocKey:
                        str_Msg = "Goods Receive Notes";
                        docNoTypes = DocNoTypes.GRDocNo;

                        break;
                    case DocKeyTypes.XPDocKey:
                        str_Msg = "Cancel PO";
                        docNoTypes = DocNoTypes.XPDocNo;

                        break;
                    case DocKeyTypes.ADJDocKey:
                        str_Msg = "Stock Adjustment";
                        docNoTypes = DocNoTypes.ADJDocNo;

                        break;
                    case DocKeyTypes.ISSDocKey:
                        str_Msg = "Stock Issue";
                        docNoTypes = DocNoTypes.ISSDocNo;

                        break;
                    case DocKeyTypes.RCVDocKey:
                        str_Msg = "Stock Receive";
                        docNoTypes = DocNoTypes.RCVDocNo;

                        break;
                    case DocKeyTypes.WOFFDocKey:
                        str_Msg = "Stock Write Off";
                        docNoTypes = DocNoTypes.WOFFDocNo;

                        break;
                    case DocKeyTypes.XFERDocKey:
                        str_Msg = "Stock Transfer";
                        docNoTypes = DocNoTypes.XFERDocNo;

                        break;
                    case DocKeyTypes.RPDocKey:
                        str_Msg = "AR Payment";
                        docNoTypes = DocNoTypes.RPDocNo;

                        break;
                    default:
                        docKeyTypes = DocKeyTypes.None;

                        break;
                }

                propertyInfoDocKey = typ_BO.GetProperty(docKeyTypes.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (propertyInfoDocKey == null) return str_rtn = "NO AutoCount [" + str_Msg + "] Info kept in this Document, kindly refer back to your developer.";

                propertyInfoDocNo = typ_BO.GetProperty(docNoTypes.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CheckDocTypeCorrectness) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        public static string CheckDocTypeCorrectness(DataRow dr_BODTL, DocKeyTypes docKeyTypes, ref object obj_DocKey, ref object obj_DocNo)
        {
            string str_rtn = "";

            try
            {
                string str_Msg = "";
                DocNoTypes docNoTypes = DocNoTypes.None;

                switch (docKeyTypes)
                {
                    case DocKeyTypes.QTDocKey:
                        str_Msg = "Quotation";
                        docNoTypes = DocNoTypes.QTDocNo;

                        break;
                    case DocKeyTypes.SODocKey:
                        str_Msg = "Sales Order";
                        docNoTypes = DocNoTypes.SODocNo;

                        break;
                    case DocKeyTypes.DODocKey:
                        str_Msg = "Delivery Order";
                        docNoTypes = DocNoTypes.DODocNo;

                        break;
                    case DocKeyTypes.IVDocKey:
                        str_Msg = "Invoice";
                        docNoTypes = DocNoTypes.IVDocNo;

                        break;
                    case DocKeyTypes.CNDocKey:
                        str_Msg = "Credit Note";
                        docNoTypes = DocNoTypes.CNDocNo;

                        break;
                    case DocKeyTypes.DNDocKey:
                        str_Msg = "Debit Note";
                        docNoTypes = DocNoTypes.DNDocNo;

                        break;
                    case DocKeyTypes.CSDocKey:
                        str_Msg = "Cash Sales";
                        docNoTypes = DocNoTypes.CSDocNo;

                        break;
                    case DocKeyTypes.XSDocKey:
                        str_Msg = "Cancel SO";
                        docNoTypes = DocNoTypes.XSDocNo;

                        break;
                    case DocKeyTypes.PODocKey:
                        str_Msg = "Purchase Order";
                        docNoTypes = DocNoTypes.PODocNo;

                        break;
                    case DocKeyTypes.GRDocKey:
                        str_Msg = "Goods Receive Notes";
                        docNoTypes = DocNoTypes.GRDocNo;

                        break;
                    case DocKeyTypes.XPDocKey:
                        str_Msg = "Cancel PO";
                        docNoTypes = DocNoTypes.XPDocNo;

                        break;
                    case DocKeyTypes.ADJDocKey:
                        str_Msg = "Stock Adjustment";
                        docNoTypes = DocNoTypes.ADJDocNo;

                        break;
                    case DocKeyTypes.ISSDocKey:
                        str_Msg = "Stock Issue";
                        docNoTypes = DocNoTypes.ISSDocNo;

                        break;
                    case DocKeyTypes.RCVDocKey:
                        str_Msg = "Stock Receive";
                        docNoTypes = DocNoTypes.RCVDocNo;

                        break;
                    case DocKeyTypes.WOFFDocKey:
                        str_Msg = "Stock Write Off";
                        docNoTypes = DocNoTypes.WOFFDocNo;

                        break;
                    case DocKeyTypes.XFERDocKey:
                        str_Msg = "Stock Transfer";
                        docNoTypes = DocNoTypes.XFERDocNo;

                        break;
                    case DocKeyTypes.RPDocKey:
                        str_Msg = "AR Payment";
                        docNoTypes = DocNoTypes.RPDocNo;

                        break;
                    default:
                        docKeyTypes = DocKeyTypes.None;

                        break;
                }

                if (!dr_BODTL.Table.Columns.Contains(docKeyTypes.ToString())) return str_rtn = "NO AutoCount [" + str_Msg + "] Info kept in this Document, kindly refer back to your developer.";

                obj_DocKey = dr_BODTL[docKeyTypes.ToString()];
                obj_DocNo = dr_BODTL[docNoTypes.ToString()];
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CheckDocTypeCorrectness) + "()", ex);

                str_rtn = ex.Message;
            }

            return str_rtn;
        }
        #endregion " Methods/Functions/Procedures "
    }
}
