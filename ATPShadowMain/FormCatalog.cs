using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AutoCount.Data;

using ServiceContractPhotocopier.GeneralSetup.MasterForms;
using ServiceContractPhotocopier.QuickView;
using ServiceContractPhotocopier.Reports;
using ServiceContractPhotocopier.ServiceAppointment.OperationForms;
using ServiceContractPhotocopier.ServiceContract.OperationForms;
using ServiceContractPhotocopier.ServiceItem.MasterForms;
using ServiceContractPhotocopier.ServiceNote.OperationForms;
using ServiceContractPhotocopier.StockRequest.OperationForms;

namespace ATPShadowMain
{
    /// <summary>
    /// One catalog of every plugin form the dev launchers (V1 modal-windows /
    /// V2 tabbed-shell) can open. Single source of truth so adding a form
    /// shows up in both UIs after one edit.
    /// </summary>
    internal sealed class CatalogEntry
    {
        public string Group;
        public string Title;
        public Func<DBSetting, Form> Create;

        public CatalogEntry(string group, string title, Func<DBSetting, Form> create)
        {
            Group = group; Title = title; Create = create;
        }
    }

    internal static class FormCatalog
    {
        public static List<CatalogEntry> All()
        {
            List<CatalogEntry> list = new List<CatalogEntry>();

            // --- Service Contract ---
            list.Add(new CatalogEntry("Service Contract", "Maintain Service Contract", db => new ServiceContractLst_Form(db)));
            list.Add(new CatalogEntry("Service Contract", "New Service Contract",      db => new ServiceContract_Form(db)));
            list.Add(new CatalogEntry("Service Contract", "Service Contract Inquiry",  db => new ServiceContractInquiry_Form(db)));
            list.Add(new CatalogEntry("Service Contract", "Outstanding Contract Item", db => new OutstandingServiceContractItem_Form(db)));

            // --- Service Item ---
            list.Add(new CatalogEntry("Service Item", "Maintain Service Item",        db => new ServiceItemLst_Form(db)));
            list.Add(new CatalogEntry("Service Item", "New Service Item",             db => new ServiceItem_Form(db)));
            list.Add(new CatalogEntry("Service Item", "Service Item Inquiry",         db => new ServiceItemInquiry_Form(db)));
            list.Add(new CatalogEntry("Service Item", "Service Item Tag Search",      db => new ServiceItemTagSearch_Form(db)));
            list.Add(new CatalogEntry("Service Item", "Generate Item From Serial",    db => new GenerateServiceItemFromSerial_Form(db)));
            list.Add(new CatalogEntry("Service Item", "Reset Item Debtor Ownership",  db => new ResetServiceItemDebtorOwnership_Form(db)));

            // --- Service Note ---
            list.Add(new CatalogEntry("Service Note", "Service Note List",            db => new ServiceNoteLst_Form(db)));
            list.Add(new CatalogEntry("Service Note", "New Service Note",             db => new ServiceNote_Form(db)));
            list.Add(new CatalogEntry("Service Note", "Service Note Quick Entry",     db => new ServiceNoteQuickEntry_Form(db)));
            list.Add(new CatalogEntry("Service Note", "Service Note Closing",         db => new ServiceNoteClosing_Form(db)));
            list.Add(new CatalogEntry("Service Note", "Service Note Assignment",      db => new ServiceNoteAssignment_Form(db)));
            list.Add(new CatalogEntry("Service Note", "Service Note Inquiry",         db => new ServiceNoteInquiry_Form(db)));
            list.Add(new CatalogEntry("Service Note", "Outstanding Note Assignment",  db => new OutstandingServiceNoteAssignment_Form(db)));

            // --- Service Appointment ---
            list.Add(new CatalogEntry("Service Appointment", "Appointment List",       db => new AppointmentLst_Form(db)));
            list.Add(new CatalogEntry("Service Appointment", "New Appointment",        db => new Appointment_Form(db)));
            list.Add(new CatalogEntry("Service Appointment", "Appointment Calendar",   db => new AppointmentCalendar_Form(db)));
            list.Add(new CatalogEntry("Service Appointment", "Appointment Inquiry",    db => new AppointmentInquiry_Form(db)));
            list.Add(new CatalogEntry("Service Appointment", "Preventive Maintenance", db => new PreventiveMaintenance_Form(db)));
            list.Add(new CatalogEntry("Service Appointment", "Meter Type Trans Entry", db => new MeterTypeTransactionEntry_Form(db)));

            // --- Quick View ---
            list.Add(new CatalogEntry("Quick View", "Service Quick View", db => new ServiceQuickView_Form(db)));

            // --- General Setup: People ---
            list.Add(new CatalogEntry("Setup - People", "Service Person",  db => new ServicePersonLst_Form(db)));
            list.Add(new CatalogEntry("Setup - People", "Service Advisor", db => new ServiceAdvisorLst_Form(db)));
            list.Add(new CatalogEntry("Setup - People", "Mechanic",        db => new MechanicLst_Form(db)));

            // --- General Setup: Lookups ---
            list.Add(new CatalogEntry("Setup - Lookups", "Service Status",        db => new ServiceStatusLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Service Severity",      db => new ServiceSeverityLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Service Solution",      db => new ServiceSolutionLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Service Problem",       db => new ServiceProblemLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Service Type",          db => new ServiceTypeLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Service Contract Type", db => new ServiceContractTypeLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Service Item Grade",    db => new ServiceItemGradeLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Appointment Type",      db => new AppointmentTypeLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Lookups", "Appointment Priority",  db => new AppointmentPriorityLst_Form(db)));

            // --- General Setup: Meter ---
            list.Add(new CatalogEntry("Setup - Meter", "Meter Type",          db => new MeterTypeLst_Form(db)));
            list.Add(new CatalogEntry("Setup - Meter", "Meter Multi Pricing", db => new MeterMultiPricingLst_Form(db)));

            // --- Stock Request ---
            list.Add(new CatalogEntry("Stock Request", "Stock Request Integration", db => new StockRequestIntegration_Form(db)));

            // --- Service Option ---
            list.Add(new CatalogEntry("Service Option", "Service Option", db => new ServiceOption_Form(db)));

            // --- Reports ---
            list.Add(new CatalogEntry("Reports", "Service Status Listing",        db => new ServiceStatusListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Severity Listing",      db => new ServiceSeverityListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Solution Listing",      db => new ServiceSolutionListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Problem Listing",       db => new ServiceProblemListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Type Listing",          db => new ServiceTypeListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Contract Type Listing", db => new ServiceContractTypeListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Item Grade Listing",    db => new ServiceItemGradeListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Appointment Type Listing",      db => new AppointmentTypeListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Appointment Priority Listing",  db => new AppointmentPriorityListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Person Listing",        db => new ServicePersonListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Advisor Listing",       db => new ServiceAdvisorListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Mechanic Listing",              db => new MechanicListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Meter Type Listing",            db => new MeterTypeListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Note Listing",          db => new ServiceNoteListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Contract Listing",      db => new ServiceContractListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Service Item Listing",          db => new ServiceItemListingReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Top Service Stock Code",        db => new TopServiceStockCodeReport_Form(db)));
            list.Add(new CatalogEntry("Reports", "Top Service Stock Code By Dept", db => new TopServiceStockCodeByDeptReport_Form(db)));

            return list;
        }
    }
}
