-- v1.3.0: CRUD audit columns. Add CreatedBy / ModifiedBy + GETDATE() defaults
-- on Created / Modified so app omissions don't lose the audit trail.
-- Guarded for re-run.

IF COL_LENGTH('dbo.zSCP_ServiceItem', 'CreatedBy') IS NULL
    ALTER TABLE [dbo].[zSCP_ServiceItem] ADD CreatedBy nvarchar(40) NULL;

IF COL_LENGTH('dbo.zSCP_ServiceItem', 'ModifiedBy') IS NULL
    ALTER TABLE [dbo].[zSCP_ServiceItem] ADD ModifiedBy nvarchar(40) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_zSCP_ServiceItem_Created')
    ALTER TABLE [dbo].[zSCP_ServiceItem]
        ADD CONSTRAINT DF_zSCP_ServiceItem_Created DEFAULT (SYSUTCDATETIME()) FOR Created;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_zSCP_ServiceItem_Modified')
    ALTER TABLE [dbo].[zSCP_ServiceItem]
        ADD CONSTRAINT DF_zSCP_ServiceItem_Modified DEFAULT (SYSUTCDATETIME()) FOR Modified;
