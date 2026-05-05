SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- zSCP_ServiceNote must be created BEFORE this table (migration runner orders it that way).

CREATE TABLE [dbo].[zSCP_ServiceNoteID](
	[ServiceNoteKey] [bigint] NOT NULL,
	[ID]             [bigint] NOT NULL,
 CONSTRAINT [PK_zSCP_ServiceNoteID] PRIMARY KEY CLUSTERED ([ServiceNoteKey] ASC, [ID] ASC),
 CONSTRAINT [FK_zSCP_ServiceNoteID_zSCP_ServiceNote] FOREIGN KEY ([ServiceNoteKey]) REFERENCES [dbo].[zSCP_ServiceNote]([ServiceNoteKey]) ON DELETE CASCADE
) ON [PRIMARY]
GO
