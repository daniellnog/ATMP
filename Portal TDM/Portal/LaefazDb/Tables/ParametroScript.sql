CREATE TABLE [dbo].[ParametroScript]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdParametro] INT NOT NULL, 
    [IdScript_CondicaoScript] INT NOT NULL, 
    [IdTipoParametro] INT NOT NULL, 
    [Obrigatorio] BIT NOT NULL, 
    [ValorDefault] VARCHAR(5000) NULL, 
    [VisivelEmTela] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_ParametroScript_Script_CondicaoScript] FOREIGN KEY ([IdScript_CondicaoScript]) REFERENCES [Script_CondicaoScript]([Id]),
    CONSTRAINT [FK_ParametroScript_Parametro] FOREIGN KEY ([IdParametro]) REFERENCES [Parametro]([Id]),
    CONSTRAINT [FK_ParametroScript_TipoParametro] FOREIGN KEY ([IdTipoParametro]) REFERENCES [TipoParametro]([Id])
)
