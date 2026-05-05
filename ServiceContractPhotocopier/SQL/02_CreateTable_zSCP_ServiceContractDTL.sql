SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceContractDTL](
	[ServiceContractKey]     [bigint] NOT NULL,
	[ServiceContractCode]    [nvarchar](50) NOT NULL DEFAULT(''),
	[Pos]                    [int] NOT NULL,
	[ID]                     [int] NOT NULL DEFAULT(0),
	[ItemNo]                 [nvarchar](10) NOT NULL DEFAULT(''),
	[ItemStockCode]          [nvarchar](30) NOT NULL DEFAULT(''),
	[Description]            [nvarchar](200) NOT NULL DEFAULT(''),
	[MoreDescr]              [nvarchar](max) NULL,
	[Unlimited]              [char](1) NOT NULL DEFAULT('N'),
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
	[GstTypeCode]            [nvarchar](20) NOT NULL DEFAULT(''),
	[TaxSubtract]            [char](1) NOT NULL DEFAULT('N'),
 CONSTRAINT [PK_zSCP_ServiceContractDTL] PRIMARY KEY CLUSTERED ([ServiceContractKey] ASC, [Pos] ASC),
 CONSTRAINT [FK_zSCP_ServiceContractDTL_zSCP_ServiceContract] FOREIGN KEY ([ServiceContractKey]) REFERENCES [dbo].[zSCP_ServiceContract]([ServiceContractKey]) ON DELETE CASCADE,
 CONSTRAINT [CK_zSCP_ServiceContractDTL_Unlimited] CHECK ([Unlimited] IN ('Y','N')),
 CONSTRAINT [CK_zSCP_ServiceContractDTL_TaxSubtract] CHECK ([TaxSubtract] IN ('Y','N'))
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServiceContractDTL_ItemStockCode] ON [dbo].[zSCP_ServiceContractDTL]([ItemStockCode])
GO
