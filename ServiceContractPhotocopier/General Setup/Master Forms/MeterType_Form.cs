using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class MeterType_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_MeterType"; } }
        protected override string KeyColumn   { get { return "MeterTypeCode"; } }
        protected override string FormCaption { get { return "Edit Meter Type"; } }
        public MeterType_Form() { InitializeComponent(); }
        public MeterType_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
