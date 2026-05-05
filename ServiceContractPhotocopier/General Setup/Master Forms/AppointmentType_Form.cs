using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class AppointmentType_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_AppointmentType"; } }
        protected override string KeyColumn   { get { return "AppointmentTypeCode"; } }
        protected override string FormCaption { get { return "Edit Appointment Type"; } }

        public AppointmentType_Form() { InitializeComponent(); }
        public AppointmentType_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
