using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceType_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_ServiceType"; } }
        protected override string KeyColumn   { get { return "ServiceTypeCode"; } }
        protected override string FormCaption { get { return "Edit Service Type"; } }

        public ServiceType_Form() { InitializeComponent(); }
        public ServiceType_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
