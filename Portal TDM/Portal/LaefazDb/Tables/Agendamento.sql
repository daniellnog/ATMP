CREATE TABLE [dbo].[Agendamento]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[RotinaDiaria] bit NULL,
	[InicioAgendamento] DATETIME NULL,
    [TerminoAgendamento] DATETIME NULL,
	[Output] VARCHAR(MAX) null, 
     
)
