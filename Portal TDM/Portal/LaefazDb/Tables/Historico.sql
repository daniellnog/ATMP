CREATE TABLE [dbo].[Historico]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[IdUsuario] INT NOT NULL,
	[IdDataPool] INT NULL, 
    [IdTestData] INT NULL, 
    [IdExecucao] INT NULL, 
    [Acao] VARCHAR(500) NOT NULL, 
    [Valores] VARCHAR(MAX) NULL,     
	[Entidade] VARCHAR(500) NOT NULL, 
	[Date] DATETIME NOT NULL, 
    CONSTRAINT [FK_Historico_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario]([Id])
    
)
