CREATE TABLE [dbo].[Agendamento_TestData]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[IdAgendamento] [int] NOT NULL,
	[IdTestData] [int] NOT NULL,
    CONSTRAINT [PK_Agendamento_TestData] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Agendamento_TestData_TestData] FOREIGN KEY (IdTestData) REFERENCES [TestData]([Id]), 
    CONSTRAINT [FK_Agendamento_TestData_Encadeamento] FOREIGN KEY (IdAgendamento) REFERENCES [Agendamento]([Id]) 
)

GO

CREATE UNIQUE INDEX [IX_Agendamento_TestData_IdTestData] ON [dbo].[Agendamento_TestData] ([IdTestData])
