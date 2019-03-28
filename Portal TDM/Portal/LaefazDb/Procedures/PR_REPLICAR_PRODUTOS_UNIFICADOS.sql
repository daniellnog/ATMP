CREATE PROCEDURE [dbo].[PR_REPLICAR_PRODUTOS_UNIFICADOS]
(
	@IdAnaliseCriada		INT,
	@IdAnaliseReferencia	INT
)
AS      
BEGIN
	IF OBJECT_ID('tempdb..#LISTAPRODUTOSNOVO') IS NOT NULL DROP TABLE #LISTAPRODUTOSNOVO
	IF OBJECT_ID('tempdb..#LISTAPRODUTOSREFERENCIA') IS NOT NULL DROP TABLE #LISTAPRODUTOSREFERENCIA

	-- Preencher tabela com produtos da análise que foi criada
	SELECT IDANALISE, ID AS IDPRODUTO, NULL AS IDPRODUTOPAI, DESCRICAO, IDUNIDADE
	INTO #LISTAPRODUTOSNOVO
	FROM Produto WHERE IdAnalise = @IdAnaliseCriada

	-- Preencher tabela com produtos da análise de referência, com o produto pai (unificado) que o respectivo produto filho também consta na tabela da análise que foi criada
	SELECT 
		distinct Produto.IdProdutoPai, Produto.Descricao, ProdutoPai.Descricao AS DESCRICAO_PAI, ProdutoNovos.IdUnidade
	INTO
		#LISTAPRODUTOSREFERENCIA
	FROM 
		Produto 
		INNER JOIN #LISTAPRODUTOSNOVO AS ProdutoNovos ON ProdutoNovos.DESCRICAO = Produto.Descricao
		INNER JOIN Produto ProdutoPai ON Produto.IdProdutoPai = ProdutoPai.Id
	WHERE 
		Produto.IdAnalise = @IdAnaliseReferencia

	-- Criar produtos novos associando a análise que foi criada. São esses produtos que serão unificados automaticamente na análise que foi criada.
	INSERT INTO Produto (idprodutopai, descricao, idanalise, ativo, IdUnidade)
	SELECT 
		distinct null, Lista.DESCRICAO_PAI, @IdAnaliseCriada, 1, Lista.IdUnidade 
	FROM 
		#LISTAPRODUTOSREFERENCIA Lista 
	WHERE 
		Lista.DESCRICAO_PAI NOT IN (SELECT Descricao FROM Produto WHERE IdAnalise = @IdAnaliseCriada)


	-- Atualizar lista de referência com os respectivos Id's que foram criados para a análise criada com a mesma descrição
	UPDATE 
		Lista
	SET
		Lista.IDPRODUTOPAI = P.Id
	FROM
		#LISTAPRODUTOSREFERENCIA Lista
		INNER JOIN Produto P ON P.Descricao = Lista.DESCRICAO_PAI
	WHERE
		P.IdAnalise = @IdAnaliseCriada

	-- Atualizar produtos unificados para os respectivos produtos que constavam na análise de referência
	UPDATE
		P
	SET
		P.IdProdutoPai = Lista.IdProdutoPai,
		P.Unificado = 1
	FROM
		Produto P
		INNER JOIN #LISTAPRODUTOSREFERENCIA Lista ON P.Descricao = Lista.Descricao
	WHERE
		P.IdAnalise = @IdAnaliseCriada
END