using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class MeterMultiPricing_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_MeterMultiPrice"; } }
        protected override string KeyColumn   { get { return "MeterMultiPriceCode"; } }
        protected override string FormCaption { get { return "Edit Meter Multi Pricing"; } }
        public MeterMultiPricing_Form() { InitializeComponent(); }
        public MeterMultiPricing_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
