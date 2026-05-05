SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceItemDebtorHistory](
	[ServiceItemDebtorHistoryKey] [bigint] IDENTITY(1,1) NOT NULL,
	[ServiceItemKey]              [bigint] NOT NULL,
	[ServiceItemCode]             [nvarchar](50) NOT NULL DEFAULT(''),
	[DebtorCode]                  [nvarchar](12) NOT NULL DEFAULT(''),
	[CurrencyCode]                [nvarchar](5) NOT NULL DEFAULT('MYR'),
	[CurrencyRate]                [decimal](20,8) NOT NULL DEFAULT(1),
	[StartDate]                   [date] NULL,
	[EndDate]                     [date] NULL,
	[Remark]                      [nvarchar](500) NOT NULL DEFAULT(''),
	[LastModified]                [datetime2](0) NOT NULL DEFAULT(GETDATE()),
 CONSTRAINT [PK_zSCP_ServiceItemDebtorHistory] PRIMARY KEY CLUSTERED ([ServiceItemDebtorHistoryKey] ASC),
 CONSTRAINT [FK_zSCP_ServiceItemDebtorHistory_zSCP_ServiceItem] FOREIGN KEY ([ServiceItemKey]) REFERENCES [dbo].[zSCP_ServiceItem]([ServiceItemKey]) ON DELETE CASCADE
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServiceItemDebtorHistory_DebtorCode] ON [dbo].[zSCP_ServiceItemDebtorHistory]([DebtorCode])
GO
