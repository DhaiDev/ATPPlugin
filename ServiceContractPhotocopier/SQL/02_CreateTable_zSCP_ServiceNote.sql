SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceNote](
	[ServiceNoteKey]             [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceNoteCode]            [nvarchar](50) NOT NULL,
	[ServiceNoteDate]            [datetime2](0) NULL,
	[ServiceStatusCode]          [nvarchar](20) NOT NULL DEFAULT(''),
	[ServiceItemCode]            [nvarchar](50) NOT NULL DEFAULT(''),
	[ServiceStartDate]           [date] NULL,
	[ServiceExpiryDate]          [date] NULL,
	[ContractNo]                 [nvarchar](50) NOT NULL DEFAULT(''),
	[ServiceTypeCode]            [nvarchar](20) NOT NULL DEFAULT(''),
	[ServiceSeverityCode]        [nvarchar](20) NOT NULL DEFAULT(''),
	[ServiceProblemCode]         [nvarchar](20) NOT NULL DEFAULT(''),
	[ServiceProblemRemark]       [nvarchar](max) NULL,
	[ServiceSolutionCode]        [nvarchar](20) NOT NULL DEFAULT(''),
	[ServiceSolutionRemark]      [nvarchar](max) NULL,
	[AttendedServicePersonCode]  [nvarchar](50) NOT NULL DEFAULT(''),
	[AssignToServicePersonCode]  [nvarchar](50) NOT NULL DEFAULT(''),
	[DebtorCode]                 [nvarchar](12) NOT NULL DEFAULT(''),
	[DebtorName]                 [nvarchar](200) NOT NULL DEFAULT(''),
	[TermCode]                   [nvarchar](30) NOT NULL DEFAULT(''),
	[StaffCode]                  [nvarchar](20) NOT NULL DEFAULT(''),
	[AreaCode]                   [nvarchar](12) NOT NULL DEFAULT(''),
	[Description]                [nvarchar](200) NOT NULL DEFAULT(''),
	[Validity]                   [nvarchar](100) NOT NULL DEFAULT(''),
	[DeliveryTerm]               [nvarchar](100) NOT NULL DEFAULT(''),
	[RefNo]                      [nvarchar](50) NOT NULL DEFAULT(''),
	[Address1]                   [nvarchar](200) NOT NULL DEFAULT(''),
	[Address2]                   [nvarchar](200) NOT NULL DEFAULT(''),
	[Address3]                   [nvarchar](200) NOT NULL DEFAULT(''),
	[Address4]                   [nvarchar](200) NOT NULL DEFAULT(''),
	[City]                       [nvarchar](50) NOT NULL DEFAULT(''),
	[PostalCode]                 [nvarchar](10) NOT NULL DEFAULT(''),
	[State]                      [nvarchar](50) NOT NULL DEFAULT(''),
	[CountryCode]                [nvarchar](50) NOT NULL DEFAULT(''),
	[Attention]                  [nvarchar](200) NOT NULL DEFAULT(''),
	[Phone]                      [nvarchar](30) NOT NULL DEFAULT(''),
	[Fax]                        [nvarchar](30) NOT NULL DEFAULT(''),
	[DiscountAmount]             [decimal](20,2) NOT NULL DEFAULT(0),
	[AmountBeforeDiscount]       [decimal](20,2) NOT NULL DEFAULT(0),
	[Amount]                     [decimal](20,2) NOT NULL DEFAULT(0),
	[TaxAmount]                  [decimal](20,2) NOT NULL DEFAULT(0),
	[AmountAfterTax]             [decimal](20,2) NOT NULL DEFAULT(0),
	[Note]                       [nvarchar](max) NULL,
	[Remark1]                    [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark2]                    [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark3]                    [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark4]                    [nvarchar](100) NOT NULL DEFAULT(''),
	[GlobalDepartment]           [char](1) NOT NULL DEFAULT('Y'),
	[DepartmentCode]             [nvarchar](10) NOT NULL DEFAULT(''),
	[GlobalJob]                  [char](1) NOT NULL DEFAULT('N'),
	[JobCode]                    [nvarchar](20) NOT NULL DEFAULT(''),
	[GlobalStockLocation]        [char](1) NOT NULL DEFAULT('Y'),
	[StockLocationCode]          [nvarchar](20) NOT NULL DEFAULT(''),
	[AppointmentDate]            [datetime2](0) NULL,
	[ActualServiceDate]          [datetime2](0) NULL,
	[CollectionDate]             [datetime2](0) NULL,
	[Closed]                     [char](1) NOT NULL DEFAULT('N'),
	[ClosedDate]                 [datetime2](0) NULL,
	[Created]                    [datetime2](0) NULL,
	[Modified]                   [datetime2](0) NULL,
	[Done]                       [char](1) NOT NULL DEFAULT('N'),
	[BranchCode]                 [nvarchar](20) NOT NULL DEFAULT(''),
	[CurrencyCode]               [nvarchar](5) NOT NULL DEFAULT('MYR'),
	[CurrencyRate]               [decimal](20,8) NOT NULL DEFAULT(1),
	[SalesInvoiceDocKey]         [bigint] NULL,
	[LastModified]               [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_ServiceNote] PRIMARY KEY CLUSTERED ([ServiceNoteKey] ASC),
 CONSTRAINT [UQ_zSCP_ServiceNote_Code] UNIQUE NONCLUSTERED ([ServiceNoteCode] ASC),
 CONSTRAINT [CK_zSCP_ServiceNote_Closed] CHECK ([Closed] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceNote_Done] CHECK ([Done] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceNote_GlobalDepartment] CHECK ([GlobalDepartment] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceNote_GlobalJob] CHECK ([GlobalJob] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceNote_GlobalStockLocation] CHECK ([GlobalStockLocation] IN ('Y','N'))
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServiceNote_Date] ON [dbo].[zSCP_ServiceNote]([ServiceNoteDate])
GO
CREATE INDEX [IX_zSCP_ServiceNote_DebtorCode] ON [dbo].[zSCP_ServiceNote]([DebtorCode])
GO
CREATE INDEX [IX_zSCP_ServiceNote_ServiceItemCode] ON [dbo].[zSCP_ServiceNote]([ServiceItemCode])
GO
CREATE INDEX [IX_zSCP_ServiceNote_StatusCode] ON [dbo].[zSCP_ServiceNote]([ServiceStatusCode])
GO
