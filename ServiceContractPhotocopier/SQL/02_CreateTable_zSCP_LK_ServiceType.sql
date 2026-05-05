SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_LK_ServiceType](
	[ServiceTypeKey]  [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceTypeCode] [nvarchar](20) NOT NULL,
	[Description]     [nvarchar](200) NOT NULL DEFAULT(''),
	[Inactive]        [char](1) NOT NULL DEFAULT('N'),
	[LastModified]    [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_LK_ServiceType] PRIMARY KEY CLUSTERED ([ServiceTypeKey] ASC),
 CONSTRAINT [UQ_zSCP_LK_ServiceType_Code] UNIQUE NONCLUSTERED ([ServiceTypeCode] ASC),
 CONSTRAINT [CK_zSCP_LK_ServiceType_Inactive] CHECK ([Inactive] IN ('Y','N'))
) ON [PRIMARY]
GO
