SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceItemMeterType](
	[ServiceItemMeterTypeKey] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceItemKey]          [bigint] NOT NULL,
	[MeterTypeCode]           [nvarchar](20) NOT NULL,
	[MinimumCharges]          [decimal](20,2) NOT NULL DEFAULT(0),
	[ChargesRate]             [decimal](20,6) NOT NULL DEFAULT(0),
	[MeterMultiPriceCode]     [nvarchar](20) NOT NULL DEFAULT(''),
	[RebateQtyInPercent]      [decimal](20,2) NOT NULL DEFAULT(0),
	[FOCQty]                  [decimal](20,2) NOT NULL DEFAULT(0),
	[InitialReading]          [decimal](20,2) NOT NULL DEFAULT(0),
 CONSTRAINT [PK_zSCP_ServiceItemMeterType] PRIMARY KEY CLUSTERED ([ServiceItemMeterTypeKey] ASC),
 CONSTRAINT [UQ_zSCP_ServiceItemMeterType] UNIQUE NONCLUSTERED ([ServiceItemKey] ASC, [MeterTypeCode] ASC),
 CONSTRAINT [FK_zSCP_ServiceItemMeterType_zSCP_ServiceItem] FOREIGN KEY ([ServiceItemKey]) REFERENCES [dbo].[zSCP_ServiceItem]([ServiceItemKey]) ON DELETE CASCADE,
 CONSTRAINT [FK_zSCP_ServiceItemMeterType_zSCP_MeterType] FOREIGN KEY ([MeterTypeCode]) REFERENCES [dbo].[zSCP_MeterType]([MeterTypeCode])
) ON [PRIMARY]
GO
