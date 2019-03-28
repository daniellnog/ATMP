CREATE TABLE [dbo].[TelaMapaCalor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descricao] VARCHAR(500) NOT NULL, 
    [Imagem] IMAGE NULL, 
    [Caminho] VARCHAR(1000) NOT NULL, 
    [Resolucao] VARCHAR(500) NULL 
)
