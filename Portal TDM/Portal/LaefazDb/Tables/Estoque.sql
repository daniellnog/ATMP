CREATE TABLE [dbo].[Estoque]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProduto] [int] NOT NULL,
	[Quantidade] DECIMAL(18, 2) NULL,
	[ValorUnitario] DECIMAL(18, 2) NULL,
	[ValorTotal] DECIMAL(18, 2) NULL,
	[Tipo] CHAR NOT NULL, 
    CONSTRAINT [PK_Estoque] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Estoque_Produto] FOREIGN KEY ([IdProduto]) REFERENCES [dbo].[Produto] ([Id])
);

/*
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Tipo de planilha importada. I = Estoque Inicial, F = Estoque Final.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Estoque',
    @level2type = N'COLUMN',
    @level2name = N'Tipo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Valor total do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Estoque',
    @level2type = N'COLUMN',
    @level2name = N'ValorTotal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Valor unitário do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Estoque',
    @level2type = N'COLUMN',
    @level2name = N'ValorUnitario'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Quantidade do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Estoque',
    @level2type = N'COLUMN',
    @level2name = N'Quantidade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FK do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Estoque',
    @level2type = N'COLUMN',
    @level2name = N'IdProduto'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Chave primária da tabela.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Estoque',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
*/
GO

CREATE INDEX [IX_Estoque_IdProduto] ON [dbo].[Estoque] ([IdProduto])
