CREATE TABLE [dbo].[Script_CondicaoScript_CT]
(
	[Id] INT NOT NULL IDENTITY, 
	[IdScript_CondicaoScript] INT NOT NULL, 
	[IdCT] INT NOT NULL, 
    CONSTRAINT [FK_Script_CondicaoScript_CT_Script_CondicaoScript] FOREIGN KEY (IdScript_CondicaoScript) REFERENCES Script_CondicaoScript(Id),
    CONSTRAINT [FK_Script_CondicaoScript_CT_CT] FOREIGN KEY (IdCT) REFERENCES CT(ID),
    CONSTRAINT [PK_Script_CondicaoScript_CT] PRIMARY KEY ([Id])
)
