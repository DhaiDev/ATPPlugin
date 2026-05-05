SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[z_SysConfig](
	[ConfigName] [nvarchar](100) NOT NULL,
	[ConfigDesc] [nvarchar](250) NULL,
	[ConfigDataType] [nvarchar](25) NULL,
	[ConfigValue] [nvarchar](300) NULL,
	[ConfigValueMax] [nvarchar](50) NULL,
	[ConfigValueMin] [nvarchar](50) NULL,
 CONSTRAINT [PK_z_SysConfig] PRIMARY KEY CLUSTERED
(
	[ConfigName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
