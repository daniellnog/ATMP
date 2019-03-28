CREATE PROCEDURE [dbo].[PR_REMOVER_PRODUTOS]
	@IdAnalise int
AS

BEGIN
	UPDATE Prod SET IdProdutoPai = NULL FROM Produto Prod WHERE Prod.IdAnalise = @IdAnalise;

	DELETE  
		Prod
	FROM
		Produto	Prod
	WHERE
		Prod.Id NOT IN
		(
			SELECT DISTINCT Prod.Id FROM Produto Prod INNER JOIN Movimentacao Mov ON Mov.IdProduto = Prod.Id WHERE Prod.IdAnalise = @IdAnalise
			UNION
			SELECT DISTINCT Prod.Id FROM Produto Prod INNER JOIN Estoque Est ON Est.IdProduto = Prod.Id WHERE Prod.IdAnalise = @IdAnalise
		) AND
		Prod.IdAnalise = @IdAnalise
END
GO
