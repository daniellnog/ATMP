CREATE TABLE [dbo].[Demanda]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descricao] VARCHAR(500) NOT NULL, 
    [Complemento] VARCHAR(500) NULL, 
    CONSTRAINT [AK_Demanda_Descricao] UNIQUE ([Descricao]),
)
