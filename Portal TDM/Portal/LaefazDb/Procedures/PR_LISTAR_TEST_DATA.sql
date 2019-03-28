CREATE PROCEDURE PR_LISTAR_TEST_DATA
	@IDDATAPOOL NVARCHAR(255) = NULL,
	@IDTESTDATA NVARCHAR(255) = NULL
AS
BEGIN
	SELECT DISTINCT
		td.Id IdTestData,
		td.Descricao,
		td.CasoTesteRelativo,
		td.Observacao,
		td.DataGeracao,
		td.GeradoPor,
		td.IdStatus,
		td.IdDataPool,
		td.GerarMigracao,
		td.CaminhoEvidencia,
		td.TempoEstimadoExecucao,
		td.ClassificacaoMassa,
		st.Descricao DescricaoStatus,
		dtp.Descricao DescricaoDataPool,
		dtp.IdScript_CondicaoScript,
		dtp.IdAut,
		AUT.Descricao DescricaoAut,
		encadTD.Ordem
	FROM 
		TestData td
			INNER JOIN Status st on td.IdStatus = st.Id
			INNER JOIN DataPool dtp on td.IdDataPool = dtp.Id
			INNER JOIN AUT on dtp.IdAut = AUT.Id
			INNER JOIN Encadeamento_TestData encadTD on td.Id = encadTD.IdTestData
	WHERE 
			   (dtp.Id = @IDDATAPOOL OR @IDDATAPOOL IS NULL)
			AND(td.Id = @IDTESTDATA OR @IDTESTDATA IS NULL)
	ORDER BY td.Id
END