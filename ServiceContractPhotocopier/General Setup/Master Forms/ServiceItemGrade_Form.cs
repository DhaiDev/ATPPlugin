using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceItemGrade_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_ServiceItemGrade"; } }
        protected override string KeyColumn   { get { return "ServiceItemGradeCode"; } }
        protected override string FormCaption { get { return "Edit Service Item Grade"; } }

        public ServiceItemGrade_Form() { InitializeComponent(); }
        public ServiceItemGrade_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
