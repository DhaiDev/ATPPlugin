-- v1.3.0: Add ACItemCode column to zSCP_MeterType
-- Maps a meter type to an AutoCount Item Code for invoice generation.
-- When set, invoice lines use this as dtl.ItemCode (stock line).
-- When empty, invoice lines are non-stock (description-only, same as V8).
-- Guarded for idempotent re-run.

IF COL_LENGTH('dbo.zSCP_MeterType', 'ACItemCode') IS NULL
    ALTER TABLE [dbo].[zSCP_MeterType] ADD ACItemCode nvarchar(30) NOT NULL DEFAULT('');
GO
