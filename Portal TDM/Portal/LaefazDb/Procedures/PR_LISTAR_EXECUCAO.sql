CREATE PROCEDURE PR_LISTAR_EXECUCAO
	@IPVDI NVARCHAR(255),
	@IDENCADEAMENTO NVARCHAR(255) = NULL,
	@IDSTATUSEXECUCAO NVARCHAR(255) = 1
AS
BEGIN
	SELECT 
		ex.Id,
		ex.IdScript_CondicaoScript_Ambiente,
		ex.IdTestData,
		ex.IdStatusExecucao,
		sca.IdAmbienteVirtual,
		av.IP AmbienteVirtual, 
		scs.CaminhoArquivoTCS,
		scs.DiretorioRelatorio,
		en.IdEncadeamento,
		ex.EnvioTelegram,
		scs.ListaExecucaoTosca ListaExecucaoTosca, 
		scs.ListaExecucaoTosca NomeQueryTosca, 
		ex.ToscaInput QueryTosca,
	CASE	
		WHEN en.IdEncadeamento IS NULL THEN CAST(0 AS BIT) -- return False
		ELSE CAST(1 AS BIT) -- return True
	END AS Encadeado
	FROM 
		Execucao ex
		INNER JOIN Script_CondicaoScript_Ambiente sca ON sca.Id = ex.IdScript_CondicaoScript_Ambiente
		INNER JOIN AmbienteVirtual av ON av.Id = sca.IdAmbienteVirtual
		INNER JOIN Script_CondicaoScript scs ON scs.Id = sca.IdScript_CondicaoScript
		INNER JOIN TestData td ON td.Id = ex.IdTestData
		LEFT JOIN Encadeamento_TestData en ON en.IdTestData = td.Id
	WHERE 
			(av.IP = @IPVDI OR @IPVDI IS NULL)
		AND	(ex.IdStatusExecucao = @IDSTATUSEXECUCAO)
		AND (en.IdEncadeamento = @IDENCADEAMENTO OR  @IDENCADEAMENTO IS NULL)
		Order by en.Ordem
END