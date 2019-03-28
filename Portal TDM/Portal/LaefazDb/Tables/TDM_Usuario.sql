CREATE TABLE [dbo].[TDM_Usuario]
(
    [IdUsuario] INT, 
    [IdTDM] INT, 
    [Descricao] VARCHAR(500) NULL, 
    CONSTRAINT [FK_TDM_Usuario_Usuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuario]([Id]), 
    CONSTRAINT [FK_TDM_Usuario_TDM] FOREIGN KEY ([IdTDM]) REFERENCES [TDM]([Id]), 
    CONSTRAINT [PK_TDM_Usuario] PRIMARY KEY ([IdTDM], [IdUsuario]), 
)