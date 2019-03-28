











IF NOT EXISTS (SELECT * FROM Controle_Ambiente)
BEGIN
	INSERT INTO [dbo].[Controle_Ambiente] ([Descricao], [Script], [Usuario], [Situacao]) VALUES (N'10.43.6.160', N'validar_home.tce', N'thiago.teixeira', 2)
END	