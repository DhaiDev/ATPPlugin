SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_ServiceContractSVI](
	[ServiceContractKey] [bigint] NOT NULL,
	[Pos]                [int] NOT NULL,
	[ServiceItemCode]    [nvarchar](50) NOT NULL DEFAULT(''),
 CONSTRAINT [PK_zSCP_ServiceContractSVI] PRIMARY KEY CLUSTERED ([ServiceContractKey] ASC, [Pos] ASC),
 CONSTRAINT [FK_zSCP_ServiceContractSVI_zSCP_ServiceContract] FOREIGN KEY ([ServiceContractKey]) REFERENCES [dbo].[zSCP_ServiceContract]([ServiceContractKey]) ON DELETE CASCADE
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_ServiceContractSVI_ItemCode] ON [dbo].[zSCP_ServiceContractSVI]([ServiceItemCode])
GO
