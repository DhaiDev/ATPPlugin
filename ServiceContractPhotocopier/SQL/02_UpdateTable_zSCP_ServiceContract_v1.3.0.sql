-- v1.3.0: CRUD audit columns. Add CreatedBy / ModifiedBy + GETDATE() defaults
-- on Created / Modified. Guarded for re-run.

IF COL_LENGTH('dbo.zSCP_ServiceContract', 'CreatedBy') IS NULL
    ALTER TABLE [dbo].[zSCP_ServiceContract] ADD CreatedBy nvarchar(40) NULL;

IF COL_LENGTH('dbo.zSCP_ServiceContract', 'ModifiedBy') IS NULL
    ALTER TABLE [dbo].[zSCP_ServiceContract] ADD ModifiedBy nvarchar(40) NULL;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_zSCP_ServiceContract_Created')
    ALTER TABLE [dbo].[zSCP_ServiceContract]
        ADD CONSTRAINT DF_zSCP_ServiceContract_Created DEFAULT (SYSUTCDATETIME()) FOR Created;

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name='DF_zSCP_ServiceContract_Modified')
    ALTER TABLE [dbo].[zSCP_ServiceContract]
        ADD CONSTRAINT DF_zSCP_ServiceContract_Modified DEFAULT (SYSUTCDATETIME()) FOR Modified;
