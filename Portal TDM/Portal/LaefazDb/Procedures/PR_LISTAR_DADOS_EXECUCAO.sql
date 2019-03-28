CREATE PROCEDURE PR_LISTAR_DADOS_EXECUCAO 
	@IPVDI NVARCHAR(255),
	@IDENCADEAMENTO INT = NULL,
	@IDSTATUSEXECUCAO NVARCHAR(255)
AS
BEGIN
	SELECT
		ex.IdTestData IdTestData, ex.Id IdExecucao, dtp.Descricao DatapoolName,
		dmd.Descricao DemandaName, ambv.Descricao AmbienteVirtual
	FROM 
		Execucao ex
		INNER JOIN Script_CondicaoScript_Ambiente sca on ex.IdScript_CondicaoScript_Ambiente = sca.Id
		INNER JOIN AmbienteVirtual ambv on sca.IdAmbienteVirtual = ambv.Id
		INNER JOIN TestData td on ex.IdTestData = td.Id
		INNER JOIN DataPool dtp on td.IdDataPool = dtp.Id
		INNER JOIN Demanda dmd on dtp.IdDemanda = dmd.Id
		INNER JOIN Script_CondicaoScript scs on sca.IdScript_CondicaoScript = scs.Id
		INNER JOIN Script scr on scs.IdScript = scr.Id
	WHERE 
				(ex.SituacaoAmbiente = 2)
			AND (ambv.IP = @IPVDI)
			AND	(ex.IdStatusExecucao = @IDSTATUSEXECUCAO)
			AND (	
					ex.Id IN (
								SELECT 
									EXECUCAO.Id 
								FROM 
									Execucao 
									LEFT JOIN Encadeamento_TestData E_T ON E_T.IdTestData = Execucao.IdTestData
								WHERE
									E_T.IdEncadeamento = @IDENCADEAMENTO OR @IDENCADEAMENTO IS NULL
							  )
				)
END