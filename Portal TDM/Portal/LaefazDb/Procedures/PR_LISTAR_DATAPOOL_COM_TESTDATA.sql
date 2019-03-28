CREATE PROCEDURE PR_LISTAR_DATAPOOL_COM_TESTDATA
	@IDTDM NVARCHAR(255) = NULL
	
AS
BEGIN
	SELECT DISTINCT
		dp.Id,
		dp.Descricao as DescricaoDataPool
	FROM DataPool dp
		INNER JOIN TestData td on dp.Id = td.IdDataPool
	WHERE dp.IdTDM =  @IDTDM OR @IDTDM IS NULL
		AND td.IdExecucao IS NULL
	
								
	ORDER BY dp.Id
END