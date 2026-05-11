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
    public static class CategoryGLARAP_Cls
    {
        #region " Fields "
        public const string Name = nameof(VTACPluginBase) + "." + nameof(Classes) + "." + nameof(AutoCountGenerator) + "." + nameof(CategoryGLARAP_Cls);
        #endregion " Fields "

        #region " V2.0.0 Support Methods "
        // Generate Autocount Documents (AR Payment) - V2.0.0 Support
        public static string SaveAutoCountRPRec(BusinessBaseV2_0_0_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoRPDocKey = null;
            PropertyInfo propertyInfoRPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.RPDocKey, ref propertyInfoRPDocKey, ref propertyInfoRPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            //for AR Payment Document
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = null;
            AutoCount.ARAP.ARPayment.ARPaymentDTLEntity doc_RPDTL = null;
            #endregion " declaration "

            try
            {
                #region " to determine CREATE or EDIT doc "
                long lng_RPDocKey = (long)propertyInfoRPDocKey.GetValue(BO);
                if (lng_RPDocKey > 0)
                    doc_RP = cmd_RP.GetARPayment(lng_RPDocKey);
                else
                    doc_RP = cmd_RP.NewARPayment();
                #endregion " to determine CREATE or EDIT doc "

                #region " start assign values into document "
                if (doc_RP != null)
                {
                    // assigns for header 1st
                    doc_RP.Description = BO.RPDocDescription + BO.DocNo;
                    doc_RP.DocDate = BO.DocDate;
                    doc_RP.DocNo2 = BO.DocNo; // assign as our DocNo 1st

                    doc_RP.DebtorCode = BO.GetBOFieldValue("DebtorCode");

                    #region " assigns for details data "
                    // directly delete all rec(s) 1st
                    if (doc_RP.ARPaymentDTLTable.Rows.Count > 0) doc_RP.ClearDetails();

                    // delete also the knockoff data
                    if (doc_RP.ARPaymentKnockOffTable.Rows.Count > 0) doc_RP.ClearKnockOff();

                    // then check for detail rec(s)
                    if (BO.Detail.DetailDataTable.Rows.Count > 0)
                    {
                        #region " assign detail data "
                        string str_DocTblName = AutoCount.Document.DocumentType.ARPayment;

                        for (int int_idx = 0; int_idx <= BO.Detail.DetailDataTable.Rows.Count - 1; int_idx++)
                        {
                            doc_RPDTL = doc_RP.NewDetail();

                            //detail data
                            doc_RPDTL.PaymentMethod = BO.Detail.GetBODTLDTFieldValue("PaymentMethod", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_RPDTL.PaymentAmt = BO.Detail.GetBODTLDTFieldValue("PaymentAmt", BO.Detail.DetailDataTable.Rows[int_idx]);

                            //knockoff data
                            doc_RP.KnockOff(AutoCount.Document.DocumentType.Invoice, BO.GetBOFieldValue(AutoCount.Document.DocumentType.Invoice + "DocKey"), doc_RPDTL.PaymentAmt, BO.Detail.GetBODTLDTFieldValue("PaymentDate", BO.Detail.DetailDataTable.Rows[int_idx]));

                            // assigns AutoCountDtlKey for detail rec
                            BO.Detail.SetBODTLDTFieldValue(str_DocTblName + "DtlKey", doc_RPDTL.DtlKey, BO.Detail.DetailDataTable.Rows[int_idx]);
                        }
                        #endregion " assign detail data "
                    }
                    #endregion " assigns for details data "

                    // here will save the autocount's AR Payment doc
                    cmd_RP.SaveARPayment(doc_RP, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);

                    // assigns AutoCount Doc Info
                    if (propertyInfoRPDocKey != null) propertyInfoRPDocKey.SetValue(BO, doc_RP.DocKey);
                    if (propertyInfoRPDocNo != null) propertyInfoRPDocNo.SetValue(BO, doc_RP.DocNo);
                }
                #endregion " start assign values into document "
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(SaveAutoCountRPRec) + "(V2)", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }
        // Generate Autocount Documents (AR Payment) - V2.0.0 Support
        public static string SaveAutoCountRPRec(BusinessBaseV2_0_0_Cls BO, ref DataRow dr_BODTL)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            object obj_RPDocKey = null;
            object obj_RPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(dr_BODTL, DocKeyTypes.RPDocKey, ref obj_RPDocKey, ref obj_RPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            //for AR Payment Document
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = null;
            AutoCount.ARAP.ARPayment.ARPaymentDTLEntity doc_RPDTL = null;
            #endregion " declaration "

            try
            {
                #region " to determine CREATE or EDIT doc "
                try
                {
                    doc_RP = cmd_RP.GetARPayment((long)obj_RPDocKey);
                }
                catch (Exception ex)
                {
                }

                // to check whether the Document has been created or not
                if (doc_RP == null) doc_RP = cmd_RP.NewARPayment();
                #endregion " to determine CREATE or EDIT doc "

                #region " start assign values into document "
                if (doc_RP != null)
                {
                    // assigns for header 1st
                    // ======================
                    // ''''doc_RP.DocNo = Me.DocNo
                    doc_RP.Description = BO.RPDocDescription + BO.DocNo;
                    doc_RP.DocDate = BO.DocDate;
                    doc_RP.DocNo2 = BO.DocNo; // assign as our DocNo 1st

                    doc_RP.DebtorCode = BO.GetBOFieldValue("DebtorCode");

                    #region " assigns for details data "
                    // directly delete all rec(s) 1st
                    if (doc_RP.ARPaymentDTLTable.Rows.Count > 0) doc_RP.ClearDetails();

                    // delete also the knockoff data
                    if (doc_RP.ARPaymentKnockOffTable.Rows.Count > 0) doc_RP.ClearKnockOff();

                    // then check for detail rec(s)
                    string str_DocTblName = AutoCount.Document.DocumentType.ARPayment;

                    doc_RPDTL = doc_RP.NewDetail();

                    //detail data
                    doc_RPDTL.PaymentMethod = dr_BODTL["PaymentMethod"].ToString();
                    doc_RPDTL.PaymentAmt = System.Convert.ToDecimal(dr_BODTL["PaymentAmt"]);

                    //knockoff data
                    doc_RP.KnockOff(AutoCount.Document.DocumentType.Invoice, BO.GetBOFieldValue(AutoCount.Document.DocumentType.Invoice + "DocKey"), doc_RPDTL.PaymentAmt, System.Convert.ToDateTime(dr_BODTL["PaymentDate"]));

                    // assigns AutoCountDtlKey for detail rec
                    dr_BODTL[str_DocTblName + "DtlKey"] = doc_RPDTL.DtlKey;
                    #endregion " assigns for details data "

                    // here will save the autocount's stock receive doc
                    cmd_RP.SaveARPayment(doc_RP, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);

                    // assigns AutoCount Doc Info
                    if (obj_RPDocKey != null) obj_RPDocKey = doc_RP.DocKey;
                    if (obj_RPDocNo != null) obj_RPDocNo = doc_RP.DocNo;
                }
                #endregion " start assign values into document "
            }
            catch (Exception ex)
            {
                //// if saved not successful then shd go back the default values
                //BO.RollBackDetailInfo();

                ErrorLogger_Cls.Write(Name + "." + nameof(SaveAutoCountRPRec) + "(V2)", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }

        // Cancel Autocount Documents (AR Payment) - V2.0.0 Support
        public static string CancelAutoCountRPRec(BusinessBaseV2_0_0_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoRPDocKey = null;
            PropertyInfo propertyInfoRPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.RPDocKey, ref propertyInfoRPDocKey, ref propertyInfoRPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = cmd_RP.GetARPayment((long)propertyInfoRPDocKey.GetValue(BO));
            #endregion " declaration "

            try
            {
                // Cancel AutoCount DOC "
                if (doc_RP != null) cmd_RP.CancelARPayment(doc_RP.DocKey, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CancelAutoCountRPRec) + "(V2)", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }
        #endregion " V2.0.0 Support Methods "

        #region " AR Payment (RP) "
        // Generate Autocount Documents (AR Payment)
        public static string SaveAutoCountRPRec(BusinessBase_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoRPDocKey = null;
            PropertyInfo propertyInfoRPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.RPDocKey, ref propertyInfoRPDocKey, ref propertyInfoRPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            //for AR Payment Document
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = null;
            AutoCount.ARAP.ARPayment.ARPaymentDTLEntity doc_RPDTL = null;
            #endregion " declaration "

            try
            {
                #region " to determine CREATE or EDIT doc "
                try
                {
                    doc_RP = cmd_RP.GetARPayment((long)propertyInfoRPDocKey.GetValue(BO));
                }
                catch (Exception ex)
                {
                }

                // to check whether the Document has been created or not
                if (doc_RP == null) doc_RP = cmd_RP.NewARPayment();
                #endregion " to determine CREATE or EDIT doc "

                #region " start assign values into document "
                if (doc_RP != null)
                {
                    // assigns for header 1st
                    // ======================
                    // ''''doc_RP.DocNo = Me.DocNo
                    doc_RP.Description = BO.RPDocDescription + BO.DocNo;
                    doc_RP.DocDate = BO.DocDate;
                    doc_RP.DocNo2 = BO.DocNo; // assign as our DocNo 1st

                    doc_RP.DebtorCode = BO.GetBOFieldValue("DebtorCode");

                    #region " assigns for details data "
                    // directly delete all rec(s) 1st
                    if (doc_RP.ARPaymentDTLTable.Rows.Count > 0) doc_RP.ClearDetails();

                    // delete also the knockoff data
                    if (doc_RP.ARPaymentKnockOffTable.Rows.Count > 0) doc_RP.ClearKnockOff();

                    // then check for detail rec(s)
                    if (BO.Detail.DetailDataTable.Rows.Count > 0)
                    {
                        #region " assign detail data "
                        string str_DocTblName = AutoCount.Document.DocumentType.ARPayment;

                        for (int int_idx = 0; int_idx <= BO.Detail.DetailDataTable.Rows.Count - 1; int_idx++)
                        {
                            doc_RPDTL = doc_RP.NewDetail();

                            //detail data
                            doc_RPDTL.PaymentMethod = BO.Detail.GetBODTLDTFieldValue("PaymentMethod", BO.Detail.DetailDataTable.Rows[int_idx]);
                            doc_RPDTL.PaymentAmt = BO.Detail.GetBODTLDTFieldValue("PaymentAmt", BO.Detail.DetailDataTable.Rows[int_idx]);

                            //knockoff data
                            doc_RP.KnockOff(AutoCount.Document.DocumentType.Invoice, BO.GetBOFieldValue(AutoCount.Document.DocumentType.Invoice + "DocKey"), doc_RPDTL.PaymentAmt, BO.Detail.GetBODTLDTFieldValue("PaymentDate", BO.Detail.DetailDataTable.Rows[int_idx]));

                            // assigns AutoCountDtlKey for detail rec
                            BO.Detail.SetBODTLDTFieldValue(str_DocTblName + "DtlKey", doc_RPDTL.DtlKey, BO.Detail.DetailDataTable.Rows[int_idx]);
                        }
                        #endregion " assign detail data "
                    }
                    #endregion " assigns for details data "

                    // here will save the autocount's stock receive doc
                    cmd_RP.SaveARPayment(doc_RP, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);

                    // assigns AutoCount Doc Info
                    if (propertyInfoRPDocKey != null) propertyInfoRPDocKey.SetValue(BO, doc_RP.DocKey);
                    if (propertyInfoRPDocNo != null) propertyInfoRPDocNo.SetValue(BO, doc_RP.DocNo);
                }
                #endregion " start assign values into document "
            }
            catch (Exception ex)
            {
                // if saved not successful then shd go back the default values
                BO.RollBackDetailInfo();

                ErrorLogger_Cls.Write(Name + "." + nameof(SaveAutoCountRPRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }
        public static string SaveAutoCountRPRec(BusinessBase_Cls BO, ref DataRow dr_BODTL)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            object obj_RPDocKey = null;
            object obj_RPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(dr_BODTL, DocKeyTypes.RPDocKey, ref obj_RPDocKey, ref obj_RPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            //for AR Payment Document
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = null;
            AutoCount.ARAP.ARPayment.ARPaymentDTLEntity doc_RPDTL = null;
            #endregion " declaration "

            try
            {
                #region " to determine CREATE or EDIT doc "
                try
                {
                    doc_RP = cmd_RP.GetARPayment((long)obj_RPDocKey);
                }
                catch (Exception ex)
                {
                }

                // to check whether the Document has been created or not
                if (doc_RP == null) doc_RP = cmd_RP.NewARPayment();
                #endregion " to determine CREATE or EDIT doc "

                #region " start assign values into document "
                if (doc_RP != null)
                {
                    // assigns for header 1st
                    // ======================
                    // ''''doc_RP.DocNo = Me.DocNo
                    doc_RP.Description = BO.RPDocDescription + BO.DocNo;
                    doc_RP.DocDate = BO.DocDate;
                    doc_RP.DocNo2 = BO.DocNo; // assign as our DocNo 1st

                    doc_RP.DebtorCode = BO.GetBOFieldValue("DebtorCode");

                    #region " assigns for details data "
                    // directly delete all rec(s) 1st
                    if (doc_RP.ARPaymentDTLTable.Rows.Count > 0) doc_RP.ClearDetails();

                    // delete also the knockoff data
                    if (doc_RP.ARPaymentKnockOffTable.Rows.Count > 0) doc_RP.ClearKnockOff();

                    // then check for detail rec(s)
                    string str_DocTblName = AutoCount.Document.DocumentType.ARPayment;

                    doc_RPDTL = doc_RP.NewDetail();

                    //detail data
                    doc_RPDTL.PaymentMethod = dr_BODTL["PaymentMethod"].ToString();
                    doc_RPDTL.PaymentAmt = System.Convert.ToDecimal(dr_BODTL["PaymentAmt"]);

                    //knockoff data
                    doc_RP.KnockOff(AutoCount.Document.DocumentType.Invoice, BO.GetBOFieldValue(AutoCount.Document.DocumentType.Invoice + "DocKey"), doc_RPDTL.PaymentAmt, System.Convert.ToDateTime(dr_BODTL["PaymentDate"]));

                    // assigns AutoCountDtlKey for detail rec
                    dr_BODTL[str_DocTblName + "DtlKey"] = doc_RPDTL.DtlKey;
                    #endregion " assigns for details data "

                    // here will save the autocount's stock receive doc
                    cmd_RP.SaveARPayment(doc_RP, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);

                    // assigns AutoCount Doc Info
                    if (obj_RPDocKey != null) obj_RPDocKey = doc_RP.DocKey;
                    if (obj_RPDocNo != null) obj_RPDocNo = doc_RP.DocNo;
                }
                #endregion " start assign values into document "
            }
            catch (Exception ex)
            {
                //// if saved not successful then shd go back the default values
                //BO.RollBackDetailInfo();

                ErrorLogger_Cls.Write(Name + "." + nameof(SaveAutoCountRPRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }

        // Cancel Autocount Documents (AR Payment)
        public static string CancelAutoCountRPRec(BusinessBase_Cls BO)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            PropertyInfo propertyInfoRPDocKey = null;
            PropertyInfo propertyInfoRPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(BO, DocKeyTypes.RPDocKey, ref propertyInfoRPDocKey, ref propertyInfoRPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = cmd_RP.GetARPayment((long)propertyInfoRPDocKey.GetValue(BO));
            #endregion " declaration "

            try
            {
                // Cancel AutoCount DOC "
                if (doc_RP != null) cmd_RP.CancelARPayment(doc_RP.DocKey, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CancelAutoCountRPRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }
        public static string CancelAutoCountRPRec(ref DataRow dr_BODTL)
        {
            string str_rtn = "";

            #region " to identify the AutoCount DOC TYPE correctness "
            object obj_RPDocKey = null;
            object obj_RPDocNo = null;

            str_rtn = CheckDocTypeCorrectness(dr_BODTL, DocKeyTypes.RPDocKey, ref obj_RPDocKey, ref obj_RPDocNo);
            if (str_rtn != "") return str_rtn;
            #endregion " to identify the AutoCount DOC TYPE correctness "

            #region " declaration "
            AutoCount.ARAP.ARPayment.ARPaymentDataAccess cmd_RP = AutoCount.ARAP.ARPayment.ARPaymentDataAccess.Create(AutoCount.Authentication.UserSession.CurrentUserSession, myDBSetting);
            AutoCount.ARAP.ARPayment.ARPaymentEntity doc_RP = cmd_RP.GetARPayment((long)obj_RPDocKey);
            #endregion " declaration "

            try
            {
                // Cancel AutoCount DOC "
                if (doc_RP != null) cmd_RP.CancelARPayment(doc_RP.DocKey, AutoCount.Authentication.UserSession.CurrentUserSession.LoginUserID);
            }
            catch (Exception ex)
            {
                ErrorLogger_Cls.Write(Name + "." + nameof(CancelAutoCountRPRec) + "()", ex);
                str_rtn = ex.Message;
            }
            finally
            {
                doc_RP = null;
                cmd_RP = null;
            }

            return str_rtn;
        }
        #endregion " AR Payment (RP) "
    }
}
