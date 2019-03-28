CREATE TABLE [dbo].[Produto]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProdutoPai] [int] NULL,
	[IdUnidade] [int] NOT NULL,
	[IdAnalise] [int] NOT NULL,
	[Descricao] [nvarchar](max) NOT NULL,
	[Ativo] [bit] NOT NULL DEFAULT 1,
    [Unificar] BIT NULL DEFAULT 0, 
    [Unificado] BIT NULL DEFAULT 0, 
    [Contestado] BIT NULL, 
    [IdProdContesPlaMovSai] INT NULL, 
    [IdProdContesPlaMovEnt] INT NULL, 
    [IdProdContesPlaFinal] INT NULL, 
    [IdProdContesPlaInicial] INT NULL, 
    CONSTRAINT [PK_Produto] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Produto_ProdutoPai] FOREIGN KEY ([IdProdutoPai]) REFERENCES [dbo].[Produto] ([Id]),
	CONSTRAINT [FK_Analise] FOREIGN KEY ([IdAnalise]) REFERENCES [dbo].[Analise] ([Id]),
	CONSTRAINT [FK_Unidade] FOREIGN KEY ([IdUnidade]) REFERENCES [dbo].[Unidade] ([Id]),
	CONSTRAINT [FK_PlanilhaSaida] FOREIGN KEY ([IdProdContesPlaMovSai]) REFERENCES [dbo].[Movimentacao] ([Id]),
	CONSTRAINT [FK_PlanilhaEntrada] FOREIGN KEY ([IdProdContesPlaMovEnt]) REFERENCES [dbo].[Movimentacao] ([Id]),
	CONSTRAINT [FK_PlanilhaFinal] FOREIGN KEY ([IdProdContesPlaFinal]) REFERENCES [dbo].[Estoque] ([Id]),
	CONSTRAINT [FK_PlanilhaInicial] FOREIGN KEY ([IdProdContesPlaInicial]) REFERENCES [dbo].[Estoque] ([Id])
);
/*
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Chave primária da tabela.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FK do produto pai.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'IdProdutoPai'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FK da unidade.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'IdUnidade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FK da análise.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'IdAnalise'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Descrição do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'Descricao'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Produto ativo ou inativo.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'Ativo'
GO
	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Se esse produto é pra ser unificado ou não 0 = não 1 = sim.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Produto',
    @level2type = N'COLUMN',
    @level2name = N'Unificar'
*/



GO

CREATE INDEX [IX_Produto_IdProdutoPai] ON [dbo].[Produto] ([IdProdutoPai])

GO

CREATE INDEX [IX_Produto_IdAnalise] ON [dbo].[Produto] ([IdAnalise])

GO

CREATE INDEX [IX_Produto_IdUnidade] ON [dbo].[Produto] ([IdUnidade])
