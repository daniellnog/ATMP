CREATE TABLE [dbo].[CondicaoScript]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descricao] VARCHAR(500) NOT NULL,
)

GO

CREATE INDEX [IX_CondicaoScript_Descricao] ON [dbo].[CondicaoScript] ([Descricao])
