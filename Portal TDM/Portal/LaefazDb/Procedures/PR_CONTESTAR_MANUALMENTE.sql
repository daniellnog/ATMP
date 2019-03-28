CREATE PROCEDURE [dbo].[PR_CONTESTAR_MANUALMENTE]
	@IdAnalise int,
	@IdProdutoPai VARCHAR(MAX)
AS
BEGIN
	DECLARE @T TABLE
	(
		IdProdutoPai	INT,
		Descricao		VARCHAR(MAX),
		Valor			DECIMAL(18,2),
		Id				INT
	)

	INSERT INTO @T
	SELECT 
		ProdUnificado.Id IdProdutoPai, ProdUnificado.Descricao, MovSaida.ValorUnitario, MovSaida.Id
	FROM
		Produto Prod
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Prod.IdProdutoPai
		INNER JOIN Movimentacao MovSaida ON MovSaida.IdProduto = Prod.Id AND Tipo = 'S'
	WHERE
		Prod.Ativo = 1 AND
		Prod.IdAnalise = @IdAnalise AND
		Prod.IdProdutoPai IN (SELECT SPLITVALUE FROM FN_SPLIT_STRING(@IdProdutoPai, ','))
	GROUP BY 
		Prod.Id, ProdUnificado.Id, ProdUnificado.Descricao, MovSaida.ValorUnitario, MovSaida.Id
	
	
	DECLARE @Agrupado TABLE
	(
		IdProdutoPai	int,
		Descricao		varchar(max),
		Valor			decimal(18,2),
		Quantidade		int,
		IdMovSaida		int
	)
	
	INSERT INTO @Agrupado
	SELECT 
		IdProdutoPai, Descricao, MAX(Valor), COUNT(Valor), MAX(Id)
	FROM 
		@T
	GROUP BY 
		IdProdutoPai, Descricao, Valor
		
	select 
		IdProdutoPai, Descricao, CAST(Valor AS VARCHAR(MAX)) + ' (' + CAST(Quantidade AS VARCHAR(MAX)) + ')' ValorUnitario, IdMovSaida Id
	from @Agrupado 
	order by Quantidade Desc
END