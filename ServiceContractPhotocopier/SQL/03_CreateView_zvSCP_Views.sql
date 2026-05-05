SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ServiceContract list view (for ServiceContractLst_Form)
CREATE VIEW [dbo].[zvSCP_ServiceContractList] AS
SELECT
	sc.[ServiceContractKey],
	sc.[ServiceContractCode],
	sc.[ServiceContractTypeCode],
	ct.[Description] AS [ContractTypeDescription],
	sc.[ServiceContractDate],
	sc.[ServiceStartDate],
	sc.[ServiceExpiryDate],
	sc.[DebtorCode],
	d.[CompanyName] AS [DebtorName],
	sc.[ServiceContractValue],
	sc.[CurrencyCode],
	sc.[AreaCode],
	sc.[BranchCode],
	sc.[StaffCode],
	sc.[Inactive],
	sc.[Done],
	sc.[LastModified]
FROM [dbo].[zSCP_ServiceContract] sc
LEFT JOIN [dbo].[zSCP_LK_ServiceContractType] ct ON ct.[ServiceContractTypeCode] = sc.[ServiceContractTypeCode]
LEFT JOIN [dbo].[Debtor] d ON d.[AccNo] = sc.[DebtorCode]
GO

-- ServiceNote list view
CREATE VIEW [dbo].[zvSCP_ServiceNoteList] AS
SELECT
	sn.[ServiceNoteKey],
	sn.[ServiceNoteCode],
	sn.[ServiceNoteDate],
	sn.[ServiceStatusCode],
	ss.[Description] AS [StatusDescription],
	sn.[ServiceItemCode],
	sn.[ContractNo],
	sn.[ServiceTypeCode],
	sn.[ServiceSeverityCode],
	sn.[DebtorCode],
	sn.[DebtorName],
	sn.[AttendedServicePersonCode],
	sn.[AssignToServicePersonCode],
	sn.[AppointmentDate],
	sn.[ActualServiceDate],
	sn.[Closed],
	sn.[ClosedDate],
	sn.[Amount],
	sn.[AmountAfterTax],
	sn.[CurrencyCode],
	sn.[BranchCode],
	sn.[LastModified]
FROM [dbo].[zSCP_ServiceNote] sn
LEFT JOIN [dbo].[zSCP_LK_ServiceStatus] ss ON ss.[ServiceStatusCode] = sn.[ServiceStatusCode]
GO

-- ServiceItem list view
CREATE VIEW [dbo].[zvSCP_ServiceItemList] AS
SELECT
	si.[ServiceItemKey],
	si.[ServiceItemCode],
	si.[StockCode],
	si.[Description],
	si.[ServiceItemGradeCode],
	sig.[Description] AS [GradeDescription],
	si.[PurchaseDate],
	si.[UnitPrice],
	si.[ContractNo],
	si.[DebtorCode],
	d.[CompanyName] AS [DebtorName],
	si.[ServiceStartDate],
	si.[ServiceExpiryDate],
	si.[AreaCode],
	si.[BranchCode],
	si.[PMActive],
	si.[PMIntervalType],
	si.[PMLastServiceDate],
	si.[Inactive],
	si.[LastModified]
FROM [dbo].[zSCP_ServiceItem] si
LEFT JOIN [dbo].[zSCP_LK_ServiceItemGrade] sig ON sig.[ServiceItemGradeCode] = si.[ServiceItemGradeCode]
LEFT JOIN [dbo].[Debtor] d ON d.[AccNo] = si.[DebtorCode]
GO

-- Appointment calendar view
CREATE VIEW [dbo].[zvSCP_AppointmentCalendar] AS
SELECT
	a.[AppointmentKey],
	a.[Subject],
	a.[StartTime],
	a.[FinishTime],
	a.[AppointmentTypeCode],
	at.[Description] AS [TypeDescription],
	a.[AppointmentPriorityCode],
	ap.[Description] AS [PriorityDescription],
	a.[DebtorCode],
	d.[CompanyName] AS [DebtorName],
	a.[ServicePersonCode],
	sp.[Name] AS [ServicePersonName],
	a.[AreaCode],
	a.[ServiceNoteKey],
	a.[LabelColor],
	a.[State],
	a.[Done]
FROM [dbo].[zSCP_Appointment] a
LEFT JOIN [dbo].[zSCP_LK_AppointmentType] at ON at.[AppointmentTypeCode] = a.[AppointmentTypeCode]
LEFT JOIN [dbo].[zSCP_LK_AppointmentPriority] ap ON ap.[AppointmentPriorityCode] = a.[AppointmentPriorityCode]
LEFT JOIN [dbo].[zSCP_ServicePerson] sp ON sp.[ServicePersonCode] = a.[ServicePersonCode]
LEFT JOIN [dbo].[Debtor] d ON d.[AccNo] = a.[DebtorCode]
GO

-- Outstanding Service Contract Item view (items on open contracts)
CREATE VIEW [dbo].[zvSCP_OutstandingServiceContractItem] AS
SELECT
	sc.[ServiceContractKey],
	sc.[ServiceContractCode],
	sc.[DebtorCode],
	d.[CompanyName] AS [DebtorName],
	sc.[ServiceStartDate],
	sc.[ServiceExpiryDate],
	svi.[Pos],
	svi.[ServiceItemCode],
	si.[StockCode],
	si.[Description] AS [ServiceItemDescription],
	sc.[Inactive],
	sc.[Done]
FROM [dbo].[zSCP_ServiceContract] sc
INNER JOIN [dbo].[zSCP_ServiceContractSVI] svi ON svi.[ServiceContractKey] = sc.[ServiceContractKey]
LEFT JOIN [dbo].[zSCP_ServiceItem] si ON si.[ServiceItemCode] = svi.[ServiceItemCode]
LEFT JOIN [dbo].[Debtor] d ON d.[AccNo] = sc.[DebtorCode]
WHERE sc.[Done] = 'N' AND sc.[Inactive] = 'N'
GO

-- Outstanding Service Note Assignment view
CREATE VIEW [dbo].[zvSCP_OutstandingServiceNoteAssignment] AS
SELECT
	sn.[ServiceNoteKey],
	sn.[ServiceNoteCode],
	sn.[ServiceNoteDate],
	sn.[ServiceStatusCode],
	sn.[DebtorCode],
	sn.[DebtorName],
	sn.[AssignToServicePersonCode],
	sp.[Name] AS [AssignedPersonName],
	sn.[AppointmentDate],
	sn.[ServiceItemCode],
	sn.[ContractNo],
	sn.[Closed]
FROM [dbo].[zSCP_ServiceNote] sn
LEFT JOIN [dbo].[zSCP_ServicePerson] sp ON sp.[ServicePersonCode] = sn.[AssignToServicePersonCode]
WHERE sn.[Closed] = 'N'
GO
