CREATE TABLE [dbo].[Script]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[IdScriptPai] INT,
	[IdAUT] INT,
	[Descricao] VARCHAR(500) NOT NULL, 
	CONSTRAINT [FK_Script_AUT] FOREIGN KEY ([IdAUT]) REFERENCES [AUT]([id]),
	CONSTRAINT [FK_Script_ScriptPai] FOREIGN KEY ([IdScriptPai]) REFERENCES [Script]([id]) 
)

GO

CREATE UNIQUE INDEX [IX_Script_Descricao] ON [dbo].[Script] ([Descricao])
