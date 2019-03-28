CREATE TABLE [dbo].[Encadeamento_TestData]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[IdTestData] [int] NOT NULL,
	[IdEncadeamento] [int] NOT NULL,
	[IdAmbienteExecucao] [int] NOT NULL, 
	[Ordem] [int] NOT NULL,
    CONSTRAINT [PK_Encadeamento_TestData] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Encadeamento_TestData_TestData] FOREIGN KEY (IdTestData) REFERENCES [TestData]([Id]), 
    CONSTRAINT [FK_Encadeamento_TestData_Encadeamento] FOREIGN KEY (IdEncadeamento) REFERENCES [Encadeamento]([Id]),
	CONSTRAINT [FK_AmbienteExecucao_TestData_Encadeamento] FOREIGN KEY (IdAmbienteExecucao) REFERENCES [AmbienteExecucao]([Id])  
)

GO

CREATE UNIQUE INDEX [IX_Encadeamento_TestData_IdTestData] ON [dbo].[Encadeamento_TestData] ([IdTestData], [IdEncadeamento])
