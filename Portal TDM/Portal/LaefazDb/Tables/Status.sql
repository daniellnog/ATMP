CREATE TABLE [dbo].[Status]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Descricao] VARCHAR(500) NOT NULL,
    CONSTRAINT [AK_Status_Column] UNIQUE ([Descricao])
)

GO
