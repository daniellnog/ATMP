CREATE TABLE [dbo].[Script_CondicaoScript_Ambiente]
(
    [Id] INT NOT NULL IDENTITY, 
    [IdScript_CondicaoScript] INT NOT NULL, 
    [IdAmbienteVirtual] INT NULL, 
    [IdAmbienteExecucao] INT NULL, 
    CONSTRAINT [FK_Script_CondicaoScript_Ambiente_AmbienteVirtual] FOREIGN KEY (IdAmbienteVirtual) REFERENCES AmbienteVirtual(Id),
    CONSTRAINT [FK_Script_CondicaoScript_Ambiente_AmbienteExeucao] FOREIGN KEY (IdAmbienteExecucao) REFERENCES AmbienteExecucao(Id),
	CONSTRAINT [FK_Script_CondicaoScript_Ambiente_CondicaoScript] FOREIGN KEY (IdScript_CondicaoScript) REFERENCES Script_CondicaoScript(Id), 
    CONSTRAINT [PK_Script_CondicaoScript_Ambiente] PRIMARY KEY ([Id])
)