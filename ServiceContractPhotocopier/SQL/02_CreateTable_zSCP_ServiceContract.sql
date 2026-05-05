SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceContract](
	[ServiceContractKey]      [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceContractCode]     [nvarchar](50) NOT NULL,
	[ServiceContractTypeCode] [nvarchar](20) NOT NULL DEFAULT(''),
	[ServiceContractDate]     [date] NULL,
	[ServiceContractValue]    [decimal](20,2) NOT NULL DEFAULT(0),
	[RefNo]                   [nvarchar](50) NOT NULL DEFAULT(''),
	[Description]             [nvarchar](200) NOT NULL DEFAULT(''),
	[ServiceStartDate]        [date] NULL,
	[ServiceExpiryDate]       [date] NULL,
	[DebtorCode]              [nvarchar](12) NOT NULL DEFAULT(''),
	[TermCode]                [nvarchar](30) NOT NULL DEFAULT(''),
	[StaffCode]               [nvarchar](20) NOT NULL DEFAULT(''),
	[AreaCode]                [nvarchar](12) NOT NULL DEFAULT(''),
	[Address1]                [nvarchar](200) NOT NULL DEFAULT(''),
	[Address2]                [nvarchar](200) NOT NULL DEFAULT(''),
	[Address3]                [nvarchar](200) NOT NULL DEFAULT(''),
	[Address4]                [nvarchar](200) NOT NULL DEFAULT(''),
	[City]                    [nvarchar](50) NOT NULL DEFAULT(''),
	[PostalCode]              [nvarchar](10) NOT NULL DEFAULT(''),
	[State]                   [nvarchar](50) NOT NULL DEFAULT(''),
	[CountryCode]             [nvarchar](50) NOT NULL DEFAULT(''),
	[Attention]               [nvarchar](200) NOT NULL DEFAULT(''),
	[Phone]                   [nvarchar](30) NOT NULL DEFAULT(''),
	[Fax]                     [nvarchar](30) NOT NULL DEFAULT(''),
	[Note]                    [nvarchar](max) NULL,
	[Remark1]                 [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark2]                 [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark3]                 [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark4]                 [nvarchar](100) NOT NULL DEFAULT(''),
	[Inactive]                [char](1) NOT NULL DEFAULT('N'),
	[Done]                    [char](1) NOT NULL DEFAULT('N'),
	[Created]                 [datetime2](0) NULL,
	[Modified]                [datetime2](0) NULL,
	[BranchCode]              [nvarchar](20) NOT NULL DEFAULT(''),
	[BranchName]              [nvarchar](200) NOT NULL DEFAULT(''),
	[DOContactPerson]         [nvarchar](200) NOT NULL DEFAULT(''),
	[DOPhone]                 [nvarchar](30) NOT NULL DEFAULT(''),
	[DOFax]                   [nvarchar](30) NOT NULL DEFAULT(''),
	[DOEmail]                 [nvarchar](200) NOT NULL DEFAULT(''),
	[DOAddress1]              [nvarchar](200) NOT NULL DEFAULT(''),
	[DOAddress2]              [nvarchar](200) NOT NULL DEFAULT(''),
	[DOAddress3]              [nvarchar](200) NOT NULL DEFAULT(''),
	[DOAddress4]              [nvarchar](200) NOT NULL DEFAULT(''),
	[DOCity]                  [nvarchar](50) NOT NULL DEFAULT(''),
	[DOPostalCode]            [nvarchar](10) NOT NULL DEFAULT(''),
	[DOState]                 [nvarchar](50) NOT NULL DEFAULT(''),
	[DOCountryCode]           [nvarchar](50) NOT NULL DEFAULT(''),
	[CurrencyCode]            [nvarchar](5) NOT NULL DEFAULT('MYR'),
	[CurrencyRate]            [decimal](20,8) NOT NULL DEFAULT(1),
	[Ref1]                    [nvarchar](30) NOT NULL DEFAULT(''),
	[Ref2]                    [nvarchar](30) NOT NULL DEFAULT(''),
	[Ref3]                    [nvarchar](30) NOT NULL DEFAULT(''),
	[Ref4]                    [nvarchar](30) NOT NULL DEFAULT(''),
	[LastModified]            [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_ServiceContract] PRIMARY KEY CLUSTERED ([ServiceContractKey] ASC),
 CONSTRAINT [UQ_zSCP_ServiceContract_Code] UNIQUE NONCLUSTERED ([ServiceContractCode] ASC),
 CONSTRAINT [CK_zSCP_ServiceContract_Inactive] CHECK ([Inactive] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceContract_Done] CHECK ([Done] IN ('Y','N'))
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServiceContract_Date] ON [dbo].[zSCP_ServiceContract]([ServiceContractDate])
GO
CREATE INDEX [IX_zSCP_ServiceContract_DebtorCode] ON [dbo].[zSCP_ServiceContract]([DebtorCode])
GO
CREATE INDEX [IX_zSCP_ServiceContract_TypeCode] ON [dbo].[zSCP_ServiceContract]([ServiceContractTypeCode])
GO
