CREATE TABLE [dbo].[ImportacaoProdutos]
(
    [Id] INT IDENTITY(1,1) NOT NULL, 
	[IdProduto] INT NOT NULL, 
    [IdProdutoPai] INT NULL, 
    [Chave] BIGINT NOT NULL, 
    CONSTRAINT [PK_ImportacaoProdutos] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_ImportacaoProdutos_Produto] FOREIGN KEY ([IdProduto]) REFERENCES Produto([id])
)

GO

CREATE INDEX [IX_ImportacaoProdutos_Chave] ON [dbo].[ImportacaoProdutos] ([Chave])
