-- Seed a small set of Service Item Grade codes so the Grade Code dropdown
-- has usable defaults on first install. Idempotent — skips rows that exist.
-- To add your own grades later: Master > zSCP_LK_ServiceItemGrade via the plugin UI.

IF NOT EXISTS (SELECT 1 FROM [dbo].[zSCP_LK_ServiceItemGrade] WHERE ServiceItemGradeCode='A')
    INSERT [dbo].[zSCP_LK_ServiceItemGrade] (ServiceItemGradeCode, [Description], Inactive)
    VALUES ('A', 'Grade A (Premium)', 'N');

IF NOT EXISTS (SELECT 1 FROM [dbo].[zSCP_LK_ServiceItemGrade] WHERE ServiceItemGradeCode='B')
    INSERT [dbo].[zSCP_LK_ServiceItemGrade] (ServiceItemGradeCode, [Description], Inactive)
    VALUES ('B', 'Grade B (Standard)', 'N');

IF NOT EXISTS (SELECT 1 FROM [dbo].[zSCP_LK_ServiceItemGrade] WHERE ServiceItemGradeCode='C')
    INSERT [dbo].[zSCP_LK_ServiceItemGrade] (ServiceItemGradeCode, [Description], Inactive)
    VALUES ('C', 'Grade C (Economy)', 'N');

IF NOT EXISTS (SELECT 1 FROM [dbo].[zSCP_LK_ServiceItemGrade] WHERE ServiceItemGradeCode='REFURB')
    INSERT [dbo].[zSCP_LK_ServiceItemGrade] (ServiceItemGradeCode, [Description], Inactive)
    VALUES ('REFURB', 'Refurbished', 'N');
