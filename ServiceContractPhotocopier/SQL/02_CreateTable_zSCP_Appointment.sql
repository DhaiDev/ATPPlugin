SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[zSCP_Appointment](
	[AppointmentKey]           [bigint] IDENTITY(1,1) NOT NULL,
	[ID]                       [nvarchar](40) NOT NULL DEFAULT(''),
	[ParentID]                 [nvarchar](40) NOT NULL DEFAULT(''),
	[Type]                     [int] NOT NULL DEFAULT(0),
	[StartTime]                [datetime2](0) NULL,
	[FinishTime]               [datetime2](0) NULL,
	[Options]                  [int] NOT NULL DEFAULT(0),
	[Subject]                  [nvarchar](200) NOT NULL DEFAULT(''),
	[RecurrenceIndex]          [int] NOT NULL DEFAULT(0),
	[RecurrenceInfo]           [varbinary](max) NULL,
	[ResourceID]               [varbinary](max) NULL,
	[AreaCode]                 [nvarchar](12) NOT NULL DEFAULT(''),
	[Message]                  [nvarchar](max) NULL,
	[ReminderDate]             [datetime2](0) NULL,
	[ReminderMinutes]          [int] NOT NULL DEFAULT(0),
	[State]                    [int] NOT NULL DEFAULT(0),
	[LabelColor]               [int] NOT NULL DEFAULT(0),
	[ActualStart]              [datetime2](0) NULL,
	[ActualFinish]             [datetime2](0) NULL,
	[AppointmentTypeCode]      [nvarchar](20) NOT NULL DEFAULT(''),
	[AppointmentPriorityCode]  [nvarchar](20) NOT NULL DEFAULT(''),
	[SalesLeadKey]             [bigint] NOT NULL DEFAULT(0),
	[ServiceNoteKey]           [bigint] NOT NULL DEFAULT(0),
	[DebtorCode]               [nvarchar](12) NOT NULL DEFAULT(''),
	[Address1]                 [nvarchar](200) NOT NULL DEFAULT(''),
	[Address2]                 [nvarchar](200) NOT NULL DEFAULT(''),
	[Address3]                 [nvarchar](200) NOT NULL DEFAULT(''),
	[Address4]                 [nvarchar](200) NOT NULL DEFAULT(''),
	[ContactPerson]            [nvarchar](200) NOT NULL DEFAULT(''),
	[StaffCode]                [nvarchar](20) NOT NULL DEFAULT(''),
	[ServicePersonCode]        [nvarchar](50) NOT NULL DEFAULT(''),
	[Description]              [nvarchar](200) NOT NULL DEFAULT(''),
	[SyncIDField]              [nvarchar](255) NOT NULL DEFAULT(''),
	[Module]                   [nvarchar](50) NOT NULL DEFAULT(''),
	[Done]                     [char](1) NOT NULL DEFAULT('N'),
	[VehicleCode]              [nvarchar](50) NOT NULL DEFAULT(''),
 CONSTRAINT [PK_zSCP_Appointment] PRIMARY KEY CLUSTERED ([AppointmentKey] ASC),
 CONSTRAINT [CK_zSCP_Appointment_Done] CHECK ([Done] IN ('Y','N'))
) ON [PRIMARY]
GO
CREATE INDEX [IX_zSCP_Appointment_DebtorCode] ON [dbo].[zSCP_Appointment]([DebtorCode])
GO
CREATE INDEX [IX_zSCP_Appointment_StaffCode] ON [dbo].[zSCP_Appointment]([StaffCode])
GO
CREATE INDEX [IX_zSCP_Appointment_TypeCode] ON [dbo].[zSCP_Appointment]([AppointmentTypeCode])
GO
CREATE INDEX [IX_zSCP_Appointment_Start] ON [dbo].[zSCP_Appointment]([StartTime])
GO
