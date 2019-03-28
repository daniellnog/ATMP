CREATE PROCEDURE [dbo].[PR_CRIAR_INDICADORES]
	@IdAnalise INT
AS
BEGIN
	DECLARE
		@QtdProduto INT,
		@QtdProdutoDesativado INT = 0, 
		@QtdProdutoUnificado INT = 0, 
		@QtdProdutoPendenteUnificacao INT = 0, 
		
		@TotalSaida DECIMAL(18,2) = 0, 
		@TotalEntrada DECIMAL(18,2) = 0,
		
		@TotalInicial DECIMAL(18,2) = 0, 
		@TotalFinal DECIMAL(18,2) = 0

	SELECT
		@QtdProduto = COUNT(Prod.Id),
		@QtdProdutoDesativado = SUM(CASE WHEN Prod.Ativo = 0 THEN 1 ELSE 0 END),
		@QtdProdutoUnificado = SUM(CASE WHEN Prod.Ativo = 1 AND Prod.Unificado = 1 THEN 1 ELSE 0 END),
		@QtdProdutoPendenteUnificacao = SUM(CASE WHEN Prod.Ativo = 1 AND Prod.Unificado = 0 THEN 1 ELSE 0 END)
	FROM
		Produto Prod
	WHERE
		Prod.IdAnalise = @IdAnalise
		
	SELECT
		@TotalSaida = SUM(CASE WHEN Mov.Tipo = 'S' THEN Mov.ValorTotal ELSE 0 END),
		@TotalEntrada = SUM(CASE WHEN Mov.Tipo = 'E' THEN Mov.ValorTotal ELSE 0 END)
	FROM
		Movimentacao Mov
		INNER JOIN Produto Prod ON Prod.Id = Mov.IdProduto
	WHERE
		Prod.Ativo = 1 AND
		Prod.IdAnalise = @IdAnalise
		
	SELECT
		@TotalInicial = SUM(CASE WHEN Est.Tipo = 'I' THEN Est.ValorTotal ELSE 0 END),
		@TotalFinal = SUM(CASE WHEN Est.Tipo = 'F' THEN Est.ValorTotal ELSE 0 END)
	FROM
		Estoque Est
		INNER JOIN Produto Prod ON Prod.Id = Est.IdProduto
	WHERE
		Prod.Ativo = 1 AND
		Prod.IdAnalise = 1
	
	INSERT INTO Indicador 
	(
		IdAnalise, QtdProduto, QtdProdutoDesativado, QtdProdutoUnificado, QtdProdutoPendenteUnificacao,
		TotalSaida, TotalEntrada, TotalInicial, TotalFinal
	) VALUES
	(
		@IdAnalise, @QtdProduto, @QtdProdutoDesativado, @QtdProdutoUnificado, @QtdProdutoPendenteUnificacao,
		@TotalSaida, @TotalEntrada, @TotalInicial, @TotalFinal
	)
END