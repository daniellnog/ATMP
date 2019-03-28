CREATE PROCEDURE PR_LISTAR_ENCADEAMENTO
	@IPVDI NVARCHAR(255),
	@IDSTATUSEXECUCAO NVARCHAR(255) = 1
AS
BEGIN
	--
	SELECT DISTINCT
		en.IdEncadeamento as IdEncadeamento
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
	--
END