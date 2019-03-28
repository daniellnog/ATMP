CREATE TABLE [dbo].[Movimentacao]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdProduto] [int] NOT NULL,
	[SEF_NFE_ECF] [nvarchar](50) NULL,
	[NUM_NF] NVARCHAR(10) NULL,
	[DataEntrada] [smalldatetime] NULL,
	[Quantidade] DECIMAL(18, 2) NULL,
	[ValorUnitario] DECIMAL(18, 2) NULL,
	[ValorTotal] DECIMAL(18, 2) NULL,
	[CFOP] [int] NULL,
	[Tipo] [char](1) NOT NULL,
	CONSTRAINT [PK_Movimentacao] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Movimentacao_Produto] FOREIGN KEY ([IdProduto]) REFERENCES [dbo].[Produto] ([Id])
);
/*
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Chave primária da tabela.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FK do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'IdProduto'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Nota fiscal do estoque.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'NUM_NF'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Tipo de Registro.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'SEF_NFE_ECF'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Tipo de planilha importada. E = Entrada, S = Saída.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'Tipo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Data de entrada do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'DataEntrada'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Quantidade do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'Quantidade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Valor unitário do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'ValorUnitario'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Valor total do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'ValorTotal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CFOP do produto.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Movimentacao',
    @level2type = N'COLUMN',
    @level2name = N'CFOP'
*/
GO

CREATE INDEX [IX_Movimentacao_IdProduto] ON [dbo].[Movimentacao] (IdProduto)
