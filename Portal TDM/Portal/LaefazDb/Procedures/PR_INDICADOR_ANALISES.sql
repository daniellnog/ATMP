
CREATE PROCEDURE [dbo].[PR_INDICADOR_ANALISES]
AS
BEGIN
	IF OBJECT_ID('tempdb..#Movimentacao') IS NOT NULL DROP TABLE #Movimentacao

	DECLARE @Produto TABLE
	(
		IdAnalise			INT,
		QtdProduto			DECIMAL(18,2),
		QtdProdutoUnificados DECIMAL(18,2)
	)
	INSERT INTO @Produto
	SELECT
		Analise.Id, 
		CAST(SUM(CASE WHEN Prod.Unificar  = 1 THEN 1 ELSE 0 END) AS DECIMAL(18,2)) QtdProduto,
		CAST(SUM(CASE WHEN Prod.Unificado = 1 AND PROD.IdProdutoPai IS NOT NULL THEN 1 ELSE 0 END) AS DECIMAL(18,2)) QtdProdutoUnificados 
	FROM
		Analise 
		INNER JOIN Produto Prod ON Prod.IdAnalise = Analise.Id
	GROUP BY
		Analise.Id


	--DECLARE @Movimentacao TABLE
	--(
	--	IdAnalise		INT,
	--	TotalEntrada	DECIMAL(18,2),
	--	TotalSaida		DECIMAL(18,2)
	--)
	--INSERT INTO @Movimentacao
	SELECT
		Analise.Id IdAnalise, 
		CAST(0 AS DECIMAL(18,2)) TotalEntrada,
		CAST(0 AS DECIMAL(18,2)) TotalSaida
	INTO
		#Movimentacao
	FROM Analise
		
	DECLARE @Levantamento TABLE
	(
		IdAnalise		INT,
		TotalInicial	DECIMAL(18,2),
		TotalFinal		DECIMAL(18,2)
	)
	INSERT INTO @Levantamento
	SELECT
		Prod.IdAnalise,
		ISNULL(SUM(CASE WHEN Est.Tipo = 'I' THEN Est.ValorTotal ELSE 0 END),0) AS Inicial,
		ISNULL(SUM(CASE WHEN Est.Tipo = 'F' THEN Est.ValorTotal ELSE 0 END),0) AS Final
	FROM
		Produto Prod
		LEFT JOIN Estoque Est ON Est.IdProduto = Prod.Id
	GROUP BY
		Prod.IdAnalise

	SELECT 
		Descricao,
		ISNULL(QtdProduto,0) QtdProduto,
		ISNULL(QtdProdutoUnificados,0) QtdProdutoUnificados,
		ISNULL(TotalEntrada,0) TotalEntrada,
		ISNULL(TotalSaida,0) TotalSaida,
		ISNULL(TotalInicial,0) TotalInicial,
		ISNULL(TotalFinal,0) TotalFinal
	FROM
		Analise 
		LEFT JOIN @Produto P ON P.IdAnalise = Analise.Id
		LEFT JOIN @Levantamento L ON L.IdAnalise = Analise.Id
		LEFT JOIN #Movimentacao M ON M.IdAnalise = Analise.Id
END