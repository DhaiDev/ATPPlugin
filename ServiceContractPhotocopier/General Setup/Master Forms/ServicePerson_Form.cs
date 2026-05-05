using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class ServicePerson_Form : ScpPersonEdt_Form
    {
        protected override string TableName   { get { return "zSCP_ServicePerson"; } }
        protected override string KeyColumn   { get { return "ServicePersonCode"; } }
        protected override string FormCaption { get { return "Edit Service Person"; } }
        public ServicePerson_Form() { InitializeComponent(); }
        public ServicePerson_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
