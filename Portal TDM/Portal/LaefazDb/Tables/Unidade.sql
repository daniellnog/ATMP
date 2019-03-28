CREATE TABLE [dbo].[Unidade] (
    [Id]		INT IDENTITY (1, 1) NOT NULL,
    [Descricao] NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Unidade] PRIMARY KEY CLUSTERED ([Id] ASC)
);
/*
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Chave primária da tabela.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Unidade',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Descrição da unidade.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Unidade',
    @level2type = N'COLUMN',
    @level2name = N'Descricao'
*/