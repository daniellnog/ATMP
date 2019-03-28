CREATE TABLE [dbo].[DataPool]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[IdTDM] INT NOT NULL, 
	[IdAut] INT NOT NULL, 
	[IdDemanda] INT NULL, 
	[IdScript_CondicaoScript] INT NOT NULL,
    [Descricao] VARCHAR(500) NOT NULL, 
    [QtdSolicitada] INT NULL, 
    [Observacao] VARCHAR(5000) NULL, 
	[DataSolicitacao] DATETIME NOT NULL, 
    [DataInicioExecucao] DATETIME NULL,
    [DataTermino] DATETIME NULL, 
    [ConsiderarRotinaDiaria] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_DataPool_TDM] FOREIGN KEY ([IdTDM]) REFERENCES [TDM]([Id]), 
    CONSTRAINT [FK_DataPool_Aut] FOREIGN KEY ([IdAut]) REFERENCES [AUT]([Id]), 
    CONSTRAINT [FK_DataPool_Demanda] FOREIGN KEY ([IdDemanda]) REFERENCES [Demanda]([Id]),
	CONSTRAINT [FK_DataPool_Script_CondicaoScript] FOREIGN KEY ([IdScript_CondicaoScript]) REFERENCES [Script_CondicaoScript]([Id])

)
