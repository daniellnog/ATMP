CREATE TABLE [dbo].[Indicador]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdAnalise] INT NOT NULL, 
	[QtdProduto] INT NOT NULL, 
	[QtdProdutoDesativado] INT NOT NULL, 
	[QtdProdutoUnificado] INT NOT NULL, 
	[QtdProdutoPendenteUnificacao] INT NOT NULL, 
    [TotalSaida] DECIMAL(18, 2) NOT NULL,
    [TotalEntrada] DECIMAL (18, 2) NOT NULL,
    [TotalInicial] DECIMAL(18, 2) NOT NULL,
    [TotalFinal] DECIMAL(18, 2) NOT NULL, 
    CONSTRAINT [FK_Indicador_Analise] FOREIGN KEY ([IdAnalise]) REFERENCES [Analise]([Id])
)
