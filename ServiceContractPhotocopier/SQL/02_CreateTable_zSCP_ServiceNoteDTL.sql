SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceNoteDTL](
	[ServiceNoteKey]         [bigint] NOT NULL,
	[ServiceNoteCode]        [nvarchar](50) NOT NULL DEFAULT(''),
	[ID]                     [int] NOT NULL DEFAULT(0),
	[Pos]                    [int] NOT NULL,
	[ItemNo]                 [nvarchar](10) NOT NULL DEFAULT(''),
	[StockCode]              [nvarchar](30) NOT NULL DEFAULT(''),
	[Description]            [nvarchar](200) NOT NULL DEFAULT(''),
	[MoreDescr]              [nvarchar](max) NULL,
	[Qty]                    [decimal](20,6) NULL,
	[UnitPrice]              [decimal](20,6) NULL,
	[UOM]                    [nvarchar](8) NOT NULL DEFAULT(''),
	[UOMRate]                [decimal](20,8) NOT NULL DEFAULT(1),
	[QtyBaseUOM]             [decimal](20,6) NOT NULL DEFAULT(0),
	[UnitPriceBaseUOM]       [decimal](20,6) NOT NULL DEFAULT(0),
	[DiscountFormat]         [nvarchar](100) NOT NULL DEFAULT(''),
	[DiscountAmount]         [decimal](20,2) NULL,
	[AmountBeforeDiscount]   [decimal](20,2) NULL,
	[Amount]                 [decimal](20,2) NULL,
	[SalesTaxPercentage]     [decimal](20,2) NULL,
	[SalesTaxAmount]         [decimal](20,2) NULL,
	[AmountAfterTax]         [decimal](20,2) NULL,
	[DepartmentCode]         [nvarchar](10) NOT NULL DEFAULT(''),
	[JobCode]                [nvarchar](20) NOT NULL DEFAULT(''),
	[StockLocationCode]      [nvarchar](20) NOT NULL DEFAULT(''),
	[DebtorItemCode]         [nvarchar](30) NOT NULL DEFAULT(''),
	[BarCode]                [nvarchar](30) NOT NULL DEFAULT(''),
	[Remark1]                [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark2]                [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark3]                [nvarchar](100) NOT NULL DEFAULT(''),
	[Remark4]                [nvarchar](100) NOT NULL DEFAULT(''),
	[RowType]                [nvarchar](30) NOT NULL DEFAULT(''),
	[FOC]                    [char](1) NOT NULL DEFAULT('N'),
	[CurrencyCode]           [nvarchar](5) NOT NULL DEFAULT('MYR'),
	[CurPrice]               [decimal](20,6) NULL,
	[CurrencyRate]           [decimal](20,8) NULL,
	[ContractItemID]         [int] NULL,
	[Contract]               [char](1) NOT NULL DEFAULT('N'),
	[GstTypeCode]            [nvarchar](20) NOT NULL DEFAULT(''),
	[TaxSubtract]            [char](1) NOT NULL DEFAULT('N'),
 CONSTRAINT [PK_zSCP_ServiceNoteDTL] PRIMARY KEY CLUSTERED ([ServiceNoteKey] ASC, [Pos] ASC),
 CONSTRAINT [FK_zSCP_ServiceNoteDTL_zSCP_ServiceNote] FOREIGN KEY ([ServiceNoteKey]) REFERENCES [dbo].[zSCP_ServiceNote]([ServiceNoteKey]) ON DELETE CASCADE,
 CONSTRAINT [CK_zSCP_ServiceNoteDTL_FOC] CHECK ([FOC] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceNoteDTL_Contract] CHECK ([Contract] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceNoteDTL_TaxSubtract] CHECK ([TaxSubtract] IN ('Y','N'))
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServiceNoteDTL_StockCode] ON [dbo].[zSCP_ServiceNoteDTL]([StockCode])
GO
