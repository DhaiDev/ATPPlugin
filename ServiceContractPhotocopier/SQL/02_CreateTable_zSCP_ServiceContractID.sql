SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- zSCP_ServiceContract must be created BEFORE this table (migration runner orders it that way).

CREATE TABLE [dbo].[zSCP_ServiceContractID](
	[ServiceContractKey] [bigint] NOT NULL,
	[ID]                 [bigint] NOT NULL,
 CONSTRAINT [PK_zSCP_ServiceContractID] PRIMARY KEY CLUSTERED ([ServiceContractKey] ASC, [ID] ASC),
 CONSTRAINT [FK_zSCP_ServiceContractID_zSCP_ServiceContract] FOREIGN KEY ([ServiceContractKey]) REFERENCES [dbo].[zSCP_ServiceContract]([ServiceContractKey]) ON DELETE CASCADE
) ON [PRIMARY]
GO
