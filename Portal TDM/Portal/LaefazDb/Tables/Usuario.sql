CREATE TABLE [dbo].[Usuario] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Login] NVARCHAR (500)  NOT NULL,
    [Senha] NVARCHAR (32)  NOT NULL,
    CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([Id] ASC)
);