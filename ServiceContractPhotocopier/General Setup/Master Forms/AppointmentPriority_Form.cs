using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class AppointmentPriority_Form : ScpLookupEdt_Form
    {
        protected override string TableName   { get { return "zSCP_LK_AppointmentPriority"; } }
        protected override string KeyColumn   { get { return "AppointmentPriorityCode"; } }
        protected override string FormCaption { get { return "Edit Appointment Priority"; } }

        public AppointmentPriority_Form() { InitializeComponent(); }
        public AppointmentPriority_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
