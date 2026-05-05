using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceSolution_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_ServiceSolution"; } }
        protected override string KeyColumn   { get { return "ServiceSolutionCode"; } }
        protected override string FormCaption { get { return "Edit Service Solution"; } }

        public ServiceSolution_Form() { InitializeComponent(); }
        public ServiceSolution_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
