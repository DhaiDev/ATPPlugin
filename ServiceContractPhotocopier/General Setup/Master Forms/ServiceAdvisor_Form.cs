using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceAdvisor_Form : ScpPersonEdt_Form
    {
        protected override string TableName   { get { return "zSCP_ServiceAdvisor"; } }
        protected override string KeyColumn   { get { return "ServiceAdvisorCode"; } }
        protected override string FormCaption { get { return "Edit Service Advisor"; } }
        public ServiceAdvisor_Form() { InitializeComponent(); }
        public ServiceAdvisor_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
