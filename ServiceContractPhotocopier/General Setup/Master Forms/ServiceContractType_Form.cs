using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceContractType_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_ServiceContractType"; } }
        protected override string KeyColumn   { get { return "ServiceContractTypeCode"; } }
        protected override string FormCaption { get { return "Edit Service Contract Type"; } }

        public ServiceContractType_Form() { InitializeComponent(); }
        public ServiceContractType_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
