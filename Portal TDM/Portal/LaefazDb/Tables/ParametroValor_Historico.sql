CREATE TABLE [dbo].[ParametroValor_Historico]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdParametroValor] INT NOT NULL, 
    [TempoInicio] DATETIME NOT NULL, 
    [TempoTermino] DATETIME NOT NULL, 
    [Valor] VARCHAR(5000) NOT NULL, 
    CONSTRAINT [FK_ParametroValor_Historico_ParametroValor] FOREIGN KEY (IdParametroValor) REFERENCES [ParametroValor]([Id])
)
