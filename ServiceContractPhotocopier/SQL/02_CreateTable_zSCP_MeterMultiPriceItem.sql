SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_MeterMultiPriceItem](
	[MeterMultiPriceItemKey] [bigint] IDENTITY(1,1) NOT NULL,
	[MeterMultiPriceCode]    [nvarchar](20) NOT NULL,
	[MeterReading]           [decimal](20,2) NOT NULL DEFAULT(0),
	[UnitPrice]              [decimal](20,6) NOT NULL DEFAULT(0),
 CONSTRAINT [PK_zSCP_MeterMultiPriceItem] PRIMARY KEY CLUSTERED ([MeterMultiPriceItemKey] ASC),
 CONSTRAINT [UQ_zSCP_MeterMultiPriceItem_CodeReading] UNIQUE NONCLUSTERED ([MeterMultiPriceCode] ASC, [MeterReading] ASC),
 CONSTRAINT [FK_zSCP_MeterMultiPriceItem_zSCP_MeterMultiPrice] FOREIGN KEY ([MeterMultiPriceCode]) REFERENCES [dbo].[zSCP_MeterMultiPrice]([MeterMultiPriceCode])
) ON [PRIMARY]
GO
