SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServicePerson](
	[ServicePersonKey]  [bigint] IDENTITY(1,1) NOT NULL,
	[ServicePersonCode] [nvarchar](50) NOT NULL,
	[Name]              [nvarchar](200) NOT NULL DEFAULT(''),
	[Name2]             [nvarchar](200) NOT NULL DEFAULT(''),
	[Gender]            [nvarchar](10) NOT NULL DEFAULT('UNKNOWN'),
	[DOB]               [date] NULL,
	[Photo]             [varbinary](max) NULL,
	[Address1]          [nvarchar](200) NOT NULL DEFAULT(''),
	[Address2]          [nvarchar](200) NOT NULL DEFAULT(''),
	[Address3]          [nvarchar](200) NOT NULL DEFAULT(''),
	[Address4]          [nvarchar](200) NOT NULL DEFAULT(''),
	[Phone]             [nvarchar](30) NOT NULL DEFAULT(''),
	[Phone2]            [nvarchar](30) NOT NULL DEFAULT(''),
	[Email]             [nvarchar](200) NOT NULL DEFAULT(''),
	[ICNo]              [nvarchar](30) NOT NULL DEFAULT(''),
	[Occupation]        [nvarchar](50) NOT NULL DEFAULT(''),
	[DateJoined]        [date] NULL,
	[DateLeft]          [date] NULL,
	[Note]              [nvarchar](max) NULL,
	[Inactive]          [char](1) NOT NULL DEFAULT('N'),
	[LastModified]      [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_ServicePerson] PRIMARY KEY CLUSTERED ([ServicePersonKey] ASC),
 CONSTRAINT [UQ_zSCP_ServicePerson_Code] UNIQUE NONCLUSTERED ([ServicePersonCode] ASC),
 CONSTRAINT [CK_zSCP_ServicePerson_Gender] CHECK ([Gender] IN ('MALE','FEMALE','UNKNOWN')),
 CONSTRAINT [CK_zSCP_ServicePerson_Inactive] CHECK ([Inactive] IN ('Y','N'))
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServicePerson_ICNo] ON [dbo].[zSCP_ServicePerson]([ICNo])
GO
