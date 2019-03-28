CREATE TABLE [dbo].[ParametroScript_Valor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdParametroScript] INT NOT NULL, 
    [ValorSugerido] VARCHAR(MAX) NOT NULL, 
    [Usada] BIT NOT NULL, 
    CONSTRAINT [FK_ParametroScript_Valor_ParametroScript] FOREIGN KEY ([IdParametroScript]) REFERENCES [ParametroScript]([Id])
)
