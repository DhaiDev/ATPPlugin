using System.Data;
using AutoCount.Data;
using ServiceContractPhotocopier.Classes.BaseForms;

namespace ServiceContractPhotocopier.GeneralSetup.MasterForms
{
    public partial class Mechanic_Form : ScpPersonEdt_Form
    {
        protected override string TableName   { get { return "zSCP_Mechanic"; } }
        protected override string KeyColumn   { get { return "MechanicCode"; } }
        protected override string FormCaption { get { return "Edit Mechanic"; } }
        public Mechanic_Form() { InitializeComponent(); }
        public Mechanic_Form(DBSetting dbSetting, DataRow existing) : base(dbSetting, existing) { InitializeComponent(); }
    }
}
