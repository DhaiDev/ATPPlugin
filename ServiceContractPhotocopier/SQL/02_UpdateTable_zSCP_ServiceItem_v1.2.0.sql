-- v1.2.0: Add PMNextServiceDate column to support Tab 1 "Next Service Date" field (V8 parity).
-- Guarded so re-runs are no-ops.
IF COL_LENGTH('dbo.zSCP_ServiceItem', 'PMNextServiceDate') IS NULL
    ALTER TABLE [dbo].[zSCP_ServiceItem] ADD PMNextServiceDate date NULL;
