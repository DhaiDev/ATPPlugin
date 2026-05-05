SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_LK_ServiceSolution](
	[ServiceSolutionKey]  [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceSolutionCode] [nvarchar](20) NOT NULL,
	[Description]         [nvarchar](200) NOT NULL DEFAULT(''),
	[Inactive]            [char](1) NOT NULL DEFAULT('N'),
	[LastModified]        [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_LK_ServiceSolution] PRIMARY KEY CLUSTERED ([ServiceSolutionKey] ASC),
 CONSTRAINT [UQ_zSCP_LK_ServiceSolution_Code] UNIQUE NONCLUSTERED ([ServiceSolutionCode] ASC),
 CONSTRAINT [CK_zSCP_LK_ServiceSolution_Inactive] CHECK ([Inactive] IN ('Y','N'))
) ON [PRIMARY]
GO
