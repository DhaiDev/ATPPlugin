using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceSeverity_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_ServiceSeverity"; } }
        protected override string KeyColumn   { get { return "ServiceSeverityCode"; } }
        protected override string FormCaption { get { return "Edit Service Severity"; } }

        public ServiceSeverity_Form() { InitializeComponent(); }
        public ServiceSeverity_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
