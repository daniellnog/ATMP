CREATE TABLE [dbo].[Parametro]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descricao] VARCHAR(500) NOT NULL,
    [Tipo] VARCHAR(500) NOT NULL,
    [ColunaTecnicaTosca] VARCHAR(500) NOT NULL, 
    CONSTRAINT [AK_Parametro_Descricao] UNIQUE ([Descricao]),
	CONSTRAINT [AK_Parametro_ColunaTecnicaTosca] UNIQUE ([ColunaTecnicaTosca]),
)
