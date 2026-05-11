using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.VisualBasic;

using AutoCount.Data;

using VTACPluginBase.Classes.BusinessBase;
using VTACPluginBase.Classes.TextLogger;

using static VTACPluginBase.Classes.AutoCountGenerator.ACHelper;
//using static VTACPluginBase.Classes.Common_Cls;
using static VTACPluginBase.PlugIn_Cls;

namespace VTACPluginBase.Classes.AutoCountGenerator
{
    public static class CategorySales_Cls
    {
        #region " Fields "
        public const string Name = nameof(VTACPluginBase) + "." + nameof(Classes) + "." + nameof(AutoCountGenerator) + "." + nameof(CategorySales_Cls);
        #endregion " Fields "

        #region " V2.0.0 Support Methods "
        // Generate Autocount Documents (Invoice) - V2.0.0 Support
        public static string SaveAutoCountIVRec(BusinessBaseV2_0_0_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoIVDocKey = null;
            PropertyInfo propertyInfoIVDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.IVDocKey, ref propertyInfoIVDocKey, ref propertyInfoIVDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            //for Invoice Document
            AutoCount.Invoicing.Sales.Invoice.InvoiceCommand cmd_IV = AutoCount.Invoicing.Sales.Invoice.InvoiceCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.Invoicing.Sales.Invoice.Invoice doc_IV = cmd_IV.Edit((long)propertyInfoIVDocKey.GetValue(BO));
            AutoCount.Invoicing.Sales.Invoice.InvoiceDetail doc_IVDTL = null;
            #endregion " declaration "

            try
            {
                #region " to determine CREATE or EDIT doc "
                // to check whether the Document has been created or not
                if (doc_IV == null) doc_IV = cmd_IV.AddNew();
                #endregion " to determine CREATE or EDIT doc "

                #region " start assign values into document "
                if (doc_IV != null)
                {
                    // assigns for header 1st
                    // ======================
                    doc_IV.Description = BO.IVDocDescription + BO.DocNo;
                    doc_IV.DocDate = BO.DocDate;
                    doc_IV.RefDocNo = BO.DocNo;

                    doc_IV.DebtorCode = BO.GetBOFieldValue("DebtorCode");

                    #region " assigns for details data "
                    // directly delete all rec(s) 1st
                    if (doc_IV.DetailCount > 0) doc_IV.ClearDetails();

                    // then check for detail rec(s)
                    if (BO.Detail.DetailDataTable.Rows.Count > 0)
                    {
                        #region " assign detail data "
                        string str_DocTblName = AutoCount.Document.DocumentType.Invoice;

                        for (int int_idx = 0; int_idx <= BO.Detail.DetailDataTable.Rows.Count - 1; int_idx++)
                        {
                            doc_IVDTL = doc_IV.AddDetail();

                            doc_IVDTL.ItemCode = BO.Detail.GetBODTLDTFieldValue("ItemCode", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.Description = BO.Detail.GetBODTLDTFieldValue("Description", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.UOM = BO.Detail.GetBODTLDTFieldValue("UOM", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.Qty = BO.Detail.GetBODTLDTFieldValue("Qty", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.UnitPrice = BO.Detail.GetBODTLDTFieldValue("UnitPrice", BO.Detail.DetailDataTable.Rows[int_idx]);

                            // assigns AutoCountDtlKey for detail rec
                            BO.Detail.SetBODTLDTFieldValue(str_DocTblName + "DtlKey", doc_IVDTL.DtlKey, BO.Detail.DetailDataTable.Rows[int_idx]);
                        }
                        #endregion " assign detail data "
                    }
                    #endregion " assigns for details data "

                    // here will save the autocount's stock receive doc
                    doc_IV.Save();

                    // assigns AutoCount Doc Info
                    if (propertyInfoIVDocKey != null) propertyInfoIVDocKey.SetValue(BO, doc_IV.DocKey);
                    if (propertyInfoIVDocNo != null) propertyInfoIVDocNo.SetValue(BO, doc_IV.DocNo);
                }
                #endregion " start assign values into document "
            }
            catch (Exception ex)
            {
                // if saved not successful then shd go back the default values
                BO.RollBackDetailInfo();

                ErrorLogger_Cls.Write(Name + "." + nameof(SaveAutoCountIVRec) + "(V2)", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_IV = null;
                cmd_IV = null;
            }

            return str_rtn;
        }

        // Cancel Autocount Documents (Invoice) - V2.0.0 Support
        public static string CancelAutoCountIVRec(BusinessBaseV2_0_0_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoIVDocKey = null;
            PropertyInfo propertyInfoIVDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.IVDocKey, ref propertyInfoIVDocKey, ref propertyInfoIVDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.Invoicing.Sales.Invoice.InvoiceCommand cmd_IV = AutoCount.Invoicing.Sales.Invoice.InvoiceCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.Invoicing.Sales.Invoice.Invoice doc_IV = cmd_IV.View((long)propertyInfoIVDocKey.GetValue(BO));
            #endregion " declaration "

            try
            {
                #region " Cancel AutoCount DOC "
                if (doc_IV != null) doc_IV.CancelDocument(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);
                #endregion " Cancel AutoCount DOC "
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CancelAutoCountIVRec) + "(V2)", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_IV = null;
                cmd_IV = null;
            }

            return str_rtn;
        }

        // Delete Autocount Documents (Invoice) - V2.0.0 Support
        public static string DeleteAutoCountIVRec(BusinessBaseV2_0_0_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoIVDocKey = null;
            PropertyInfo propertyInfoIVDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.IVDocKey, ref propertyInfoIVDocKey, ref propertyInfoIVDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.Invoicing.Sales.Invoice.InvoiceCommand cmd_IV = AutoCount.Invoicing.Sales.Invoice.InvoiceCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.Invoicing.Sales.Invoice.Invoice doc_IV = cmd_IV.View((long)propertyInfoIVDocKey.GetValue(BO));
            #endregion " declaration "

            try
            {
                #region " Delete AutoCount DOC "
                if (doc_IV != null) cmd_IV.Delete(doc_IV.DocKey);
                #endregion " Delete AutoCount DOC "
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(DeleteAutoCountIVRec) + "(V2)", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_IV = null;
                cmd_IV = null;
            }

            return str_rtn;
        }
        #endregion " V2.0.0 Support Methods "

        #region " INVOICE (IV) "
        // Generate Autocount Documents (Invoice)
        public static string SaveAutoCountIVRec(BusinessBase_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoIVDocKey = null;
            PropertyInfo propertyInfoIVDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.IVDocKey, ref propertyInfoIVDocKey, ref propertyInfoIVDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            //for Invoice Document
            AutoCount.Invoicing.Sales.Invoice.InvoiceCommand cmd_IV = AutoCount.Invoicing.Sales.Invoice.InvoiceCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.Invoicing.Sales.Invoice.Invoice doc_IV = cmd_IV.Edit((long)propertyInfoIVDocKey.GetValue(BO));
            AutoCount.Invoicing.Sales.Invoice.InvoiceDetail doc_IVDTL = null;
            #endregion " declaration "

            try
            {
                #region " to determine CREATE or EDIT doc "
                // to check whether the Document has been created or not
                if (doc_IV == null) doc_IV = cmd_IV.AddNew();
                #endregion " to determine CREATE or EDIT doc "

                #region " start assign values into document "
                if (doc_IV != null)
                {
                    // assigns for header 1st
                    // ======================
                    // ''''doc_IV.DocNo = Me.DocNo
                    doc_IV.Description = BO.IVDocDescription + BO.DocNo; // "Installment Plugin [INSTALLMENT SALES] - " + this.DocNo;
                    doc_IV.DocDate = BO.DocDate;
                    doc_IV.RefDocNo = BO.DocNo; // assign as our DocNo 1st

                    doc_IV.DebtorCode = BO.GetBOFieldValue("DebtorCode");

                    #region " assigns for details data "
                    // directly delete all rec(s) 1st
                    if (doc_IV.DetailCount > 0) doc_IV.ClearDetails();

                    // then check for detail rec(s)
                    if (BO.Detail.DetailDataTable.Rows.Count > 0)
                    {
                        #region " assign detail data "
                        string str_DocTblName = AutoCount.Document.DocumentType.Invoice;

                        for (int int_idx = 0; int_idx <= BO.Detail.DetailDataTable.Rows.Count - 1; int_idx++)
                        {
                            doc_IVDTL = doc_IV.AddDetail();

                            doc_IVDTL.ItemCode = BO.Detail.GetBODTLDTFieldValue("ItemCode", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.Description = BO.Detail.GetBODTLDTFieldValue("Description", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.UOM = BO.Detail.GetBODTLDTFieldValue("UOM", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.Qty = BO.Detail.GetBODTLDTFieldValue("Qty", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_IVDTL.UnitPrice = BO.Detail.GetBODTLDTFieldValue("UnitPrice", BO.Detail.DetailDataTable.Rows[int_idx]);

                            // assigns AutoCountDtlKey for detail rec
                            BO.Detail.SetBODTLDTFieldValue(str_DocTblName + "DtlKey", doc_IVDTL.DtlKey, BO.Detail.DetailDataTable.Rows[int_idx]);
                        }
                        #endregion " assign detail data "
                    }
                    #endregion " assigns for details data "

                    // here will save the autocount's stock receive doc
                    doc_IV.Save();

                    // assigns AutoCount Doc Info
                    if (propertyInfoIVDocKey != null) propertyInfoIVDocKey.SetValue(BO, doc_IV.DocKey);
                    if (propertyInfoIVDocNo != null) propertyInfoIVDocNo.SetValue(BO, doc_IV.DocNo);
                }
                #endregion " start assign values into document "
            }
            catch (Exception ex)
            {
                // if saved not successful then shd go back the default values
                BO.RollBackDetailInfo();

                ErrorLogger_Cls.Write(Name + "." + nameof(SaveAutoCountIVRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_IV = null;
                cmd_IV = null;
            }

            return str_rtn;
        }

        // Cancel Autocount Documents (Invoice)
        public static string CancelAutoCountIVRec(BusinessBase_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoIVDocKey = null;
            PropertyInfo propertyInfoIVDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.IVDocKey, ref propertyInfoIVDocKey, ref propertyInfoIVDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.Invoicing.Sales.Invoice.InvoiceCommand cmd_IV = AutoCount.Invoicing.Sales.Invoice.InvoiceCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.Invoicing.Sales.Invoice.Invoice doc_IV = cmd_IV.View((long)propertyInfoIVDocKey.GetValue(BO));
            #endregion " declaration "

            try
            {
                #region " Cancel AutoCount DOC "
                if (doc_IV != null) doc_IV.CancelDocument(AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);
                #endregion " Cancel AutoCount DOC "
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CancelAutoCountIVRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_IV = null;
                cmd_IV = null;
            }

            return str_rtn;
        }

        // Delete Autocount Documents (Invoice)
        public static string DeleteAutoCountIVRec(BusinessBase_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoIVDocKey = null;
            PropertyInfo propertyInfoIVDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.IVDocKey, ref propertyInfoIVDocKey, ref propertyInfoIVDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.Invoicing.Sales.Invoice.InvoiceCommand cmd_IV = AutoCount.Invoicing.Sales.Invoice.InvoiceCommand.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.Invoicing.Sales.Invoice.Invoice doc_IV = cmd_IV.View((long)propertyInfoIVDocKey.GetValue(BO));
            #endregion " declaration "

            try
            {
                #region " Delete AutoCount DOC "
                if (doc_IV != null) cmd_IV.Delete(doc_IV.DocKey);
                #endregion " Delete AutoCount DOC "
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(DeleteAutoCountIVRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_IV = null;
                cmd_IV = null;
            }

            return str_rtn;
        }
        #endregion " INVOICE (IV) "
    }
}
