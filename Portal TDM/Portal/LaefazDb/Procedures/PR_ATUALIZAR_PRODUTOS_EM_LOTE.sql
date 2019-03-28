CREATE PROCEDURE [dbo].[PR_ATUALIZAR_PRODUTOS_EM_LOTE]
	@IdAnalise	INT,
	@Chave		VARCHAR(50)
AS
BEGIN

	UPDATE	
		Prod
	SET
		Prod.IdProdutoPai = Import.IdProdutoPai,
		Prod.Unificado = 1
	FROM
		ImportacaoProdutos Import
		INNER JOIN Produto Prod ON Prod.Id = Import.IdProduto
	WHERE
		Prod.Ativo = 1 AND
		Import.Chave = @Chave AND
		Prod.IdAnalise = @IdAnalise

	
	DELETE FROM ImportacaoProdutos WHERE Chave = @Chave

END
