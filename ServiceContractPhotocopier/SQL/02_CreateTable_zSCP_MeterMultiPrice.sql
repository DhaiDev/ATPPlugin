SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_MeterMultiPrice](
	[MeterMultiPriceKey]  [bigint] IDENTITY(1,1) NOT NULL,
	[MeterMultiPriceCode] [nvarchar](20) NOT NULL,
	[Description]         [nvarchar](200) NOT NULL DEFAULT(''),
	[LastModified]        [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_MeterMultiPrice] PRIMARY KEY CLUSTERED ([MeterMultiPriceKey] ASC),
 CONSTRAINT [UQ_zSCP_MeterMultiPrice_Code] UNIQUE NONCLUSTERED ([MeterMultiPriceCode] ASC)
) ON [PRIMARY]
GO
