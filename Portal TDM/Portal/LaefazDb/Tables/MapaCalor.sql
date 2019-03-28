CREATE TABLE [dbo].[MapaCalor]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdTelaMapaCalor] INT NOT NULL, 
    [IdUsuario] INT NOT NULL, 
    [Data] DATETIME NOT NULL, 
    [PosX] INT NOT NULL, 
    [PosY] INT NOT NULL, 
    [Resolucao] VARCHAR(500) NULL, 
    CONSTRAINT [FK_MapaCalor_Monitoramento_MapaCalor] FOREIGN KEY ([IdTelaMapaCalor]) REFERENCES [TelaMapaCalor]([Id]), 
    CONSTRAINT [FK_MapaCalor_Monitoramento_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario]([Id])
)
