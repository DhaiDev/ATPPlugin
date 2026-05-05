using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServiceProblem_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_ServiceProblem"; } }
        protected override string KeyColumn   { get { return "ServiceProblemCode"; } }
        protected override string FormCaption { get { return "Edit Service Problem"; } }

        public ServiceProblem_Form() { InitializeComponent(); }
        public ServiceProblem_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
