﻿CREATE TABLE [dbo].[TDM]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descricao] VARCHAR(500) NOT NULL,
	CONSTRAINT [AK_TDM_Descricao] UNIQUE ([Descricao]),
	[TdmPublico] BIT NOT NULL DEFAULT 0 
    
)
