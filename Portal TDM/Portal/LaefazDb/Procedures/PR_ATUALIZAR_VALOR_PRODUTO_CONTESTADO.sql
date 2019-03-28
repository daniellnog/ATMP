CREATE PROCEDURE [dbo].[PR_ATUALIZAR_VALOR_PRODUTO_CONTESTADO]
	@IdProdutoPai	VARCHAR(MAX)
AS
BEGIN
	DECLARE @ProdutosContestar TABLE
	(
		IdProdutoPai			INT,
		IdProdContesPlaMovEnt   INT,
		IdProdContesPlaFinal	INT,
		IdProdContesPlaInicial	INT
	)

	INSERT INTO @ProdutosContestar
	SELECT
		Prod.IdProdutoPai, 

		CASE	
			WHEN MAX(MovEntrada.ValorUnitario) IS NOT NULL THEN MAX(MovEntrada.Id) ELSE NULL
		END MovEntradaId,
		
		CASE
			WHEN MAX(MovEntrada.ValorUnitario) IS NULL AND MAX(EstFinal.ValorUnitario) IS NOT NULL THEN MAX(EstFinal.Id) ELSE NULL
		END EstFinalId,
		
		CASE
			WHEN MAX(MovEntrada.ValorUnitario) IS NULL AND MAX(EstFinal.ValorUnitario) IS NULL  AND MAX(EstInicial.ValorUnitario) IS NOT NULL THEN MAX(EstInicial.Id) ELSE NULL
		END EstInicialId
	FROM 
		Produto Prod 
		LEFT JOIN Movimentacao MovEntrada ON MovEntrada.IdProduto = Prod.Id AND MovEntrada.Tipo = 'E'
		LEFT JOIN Estoque EstFinal ON EstFinal.IdProduto = Prod.Id AND EstFinal.Tipo = 'F'
		LEFT JOIN Estoque EstInicial ON EstInicial.IdProduto = Prod.Id AND EstInicial.Tipo = 'I'
	WHERE 
		Prod.Ativo = 1 AND 
		Prod.IdProdutoPai IN (SELECT SPLITVALUE FROM FN_SPLIT_STRING(@IdProdutoPai, ','))
	GROUP BY
		Prod.IdProdutoPai

	-- Atualizar Status Contestado
	-- Atualizar Id Contestação caso tenha encontrado em Entrada, Final ou Inicial.
	UPDATE 
		Prod
	SET
		Prod.Contestado = 1,
		Prod.IdProdContesPlaMovEnt = ProdContestar.IdProdContesPlaMovEnt,
		Prod.IdProdContesPlaFinal = ProdContestar.IdProdContesPlaFinal,
		Prod.IdProdContesPlaInicial = ProdContestar.IdProdContesPlaInicial,
		prod.IdProdContesPlaMovSai = NULL
	FROM
		Produto Prod
		INNER JOIN @ProdutosContestar ProdContestar ON ProdContestar.IdProdutoPai = Prod.Id


	-- Produtos que não foram contestados por não constarem nas respectivas planilhas
	SELECT
		Prod.Descricao
	FROM
		@ProdutosContestar ProdContestar
		INNER JOIN Produto Prod ON Prod.Id = ProdContestar.IdProdutoPai
	WHERE
		Prod.IdProdContesPlaMovEnt IS NULL AND
		Prod.IdProdContesPlaFinal IS NULL AND
		Prod.IdProdContesPlaInicial IS NULL
END