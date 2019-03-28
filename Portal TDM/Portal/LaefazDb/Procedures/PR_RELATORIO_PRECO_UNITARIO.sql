CREATE PROCEDURE [dbo].[PR_RELATORIO_PRECO_UNITARIO]
	@DISPLAYLENGTH INT,
	@DISPLAYSTART INT,
	@SORTCOL INT,
	@SORTDIR NVARCHAR(10),
	@SEARCH NVARCHAR(255) = NULL,
	@LISTARTODOS INT = 0,
	@IDANALISE NVARCHAR(255)
AS
BEGIN
	DECLARE @FIRSTREC INT, @LASTREC INT
	SET @FIRSTREC = @DISPLAYSTART;
	SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;


	IF OBJECT_ID('tempdb..#DadosMovimentacaoEntComOrdem') IS NOT NULL DROP TABLE #DadosMovimentacaoEntComOrdem
	SELECT 
		ROW_NUMBER() OVER (PARTITION BY Prod.IdProdutoPai, Mov.Tipo ORDER BY Prod.IdProdutoPai, ValorUnitario DESC, Mov.NUM_NF DESC) ordem,
		Prod.IdProdutoPai, Prod.Id IdProduto, Mov.Id, Mov.ValorUnitario, Mov.NUM_NF, Mov.DataEntrada, Mov.Tipo
	INTO
		#DadosMovimentacaoEntComOrdem 
	FROM 
		Movimentacao Mov 
		INNER JOIN Produto Prod ON Prod.Id = Mov.IdProduto 
	WHERE 
		Prod.IdAnalise = @IDANALISE AND
		Mov.Tipo = 'E'
	
	IF OBJECT_ID('tempdb..#DadosMovimentacaoContestadoComOrdem') IS NOT NULL DROP TABLE #DadosMovimentacaoContestadoComOrdem
	SELECT 
		ROW_NUMBER() OVER (PARTITION BY Prod.IdProdutoPai, Mov.Tipo ORDER BY Prod.IdProdutoPai, ValorUnitario DESC, Mov.NUM_NF DESC) ordem,
		Prod.IdProdutoPai, Prod.Id IdProduto, Mov.Id, Mov.ValorUnitario, Mov.NUM_NF, Mov.DataEntrada, Mov.Tipo
	INTO
		#DadosMovimentacaoContestadoComOrdem
	FROM 
		Movimentacao Mov 
		INNER JOIN Produto Prod ON Prod.Id = Mov.IdProduto 
	WHERE 
		Prod.IdAnalise = @IDANALISE
		
	-- Recuperar Maior valor unitário do produto unificado em Movimentação Saída. Caso não tenha em Saída, deverá buscar o Maior valor em Movimentação entrada.
	IF OBJECT_ID('tempdb..#MaiorValorUnitario') IS NOT NULL DROP TABLE #MaiorValorUnitario
	
	-- Inserir o maior valor de entrada e saída do respectivo produto unificado
	SELECT 
		COALESCE(MAX(Prod.IdProdutoPai), MAX(Prod.Id)) IdProdutoPai,
		
		COALESCE
		(
			MAX(MovSaidaContest.ValorUnitario), 
			MAX(MovEntradaContest.ValorUnitario), 
			MAX(EstFinalContest.ValorUnitario), 
			MAX(EstInicialContest.ValorUnitario),
			MAX(MovSaida.ValorUnitario),
			MAX(MovEntrada.ValorUnitario), 
			MAX(EstFinal.ValorUnitario),
			MAX(EstInicial.ValorUnitario)
		) ValorUnitario,
		
		CASE	
			WHEN (MAX(MovSaidaContest.ValorUnitario) IS NOT NULL)	THEN '1-SC'
			WHEN (MAX(MovEntradaContest.ValorUnitario) IS NOT NULL) THEN '2-EC'
			WHEN (MAX(EstFinalContest.ValorUnitario) IS NOT NULL)	THEN '3-FC'
			WHEN (MAX(EstInicialContest.ValorUnitario) IS NOT NULL) THEN '4-IC'
			
			WHEN (MAX(MovSaidaContest.ValorUnitario) IS NULL) AND (MAX(MovEntradaContest.ValorUnitario) IS NULL) AND (MAX(EstFinalContest.ValorUnitario) IS NULL) AND (MAX(EstInicialContest.ValorUnitario) IS NULL) THEN
				CASE
					WHEN (MAX(MovSaida.ValorUnitario) IS NOT NULL) THEN '5-S'
					WHEN (MAX(MovSaida.ValorUnitario) IS NULL) AND (MAX(MovEntrada.ValorUnitario) IS NOT NULL) THEN '6-E'
					WHEN (MAX(MovSaida.ValorUnitario) IS NULL) AND (MAX(MovEntrada.ValorUnitario) IS NULL) AND (MAX(EstFinal.ValorUnitario) IS NOT NULL) THEN '7-F'
					WHEN (MAX(MovSaida.ValorUnitario) IS NULL) AND (MAX(MovEntrada.ValorUnitario) IS NULL) AND (MAX(EstFinal.ValorUnitario) IS NULL)  AND (MAX(EstInicial.ValorUnitario) IS NOT NULL) THEN '8-I'
				END
		END Tipo
	INTO
		#MaiorValorUnitario
	FROM 
		Produto Prod
		LEFT JOIN Movimentacao MovSaida ON MovSaida.IdProduto = Prod.Id AND MovSaida.Tipo = 'S'
		LEFT JOIN Movimentacao MovEntrada ON MovEntrada.IdProduto = Prod.Id AND MovEntrada.Tipo = 'E'
		LEFT JOIN Estoque EstInicial ON EstInicial.IdProduto = Prod.Id AND EstInicial.Tipo = 'I'
		LEFT JOIN Estoque EstFinal ON EstFinal.IdProduto = Prod.Id AND EstFinal.Tipo = 'F'
		LEFT JOIN Movimentacao MovSaidaContest ON MovSaidaContest.Id = Prod.IdProdContesPlaMovSai AND MovSaidaContest.Tipo = 'S'
		LEFT JOIN Movimentacao MovEntradaContest ON MovEntradaContest.Id = Prod.IdProdContesPlaMovEnt AND MovEntradaContest.Tipo = 'E'
		LEFT JOIN Estoque EstInicialContest ON EstInicialContest.Id = Prod.IdProdContesPlaInicial AND EstInicialContest.Tipo = 'I'
		LEFT JOIN Estoque EstFinalContest ON EstFinalContest.Id = Prod.IdProdContesPlaFinal AND EstFinalContest.Tipo = 'F'
	WHERE 
		Prod.Ativo = 1 AND
		Prod.IdAnalise = @IdAnalise AND
		Prod.IdProdutoPai IS NOT NULL
	GROUP BY
		Prod.IdProdutoPai

	IF OBJECT_ID('tempdb..#ValorMaiorUnitarioComOrdem') IS NOT NULL 
		DROP TABLE #ValorMaiorUnitarioComOrdem
	
	CREATE TABLE #ValorMaiorUnitarioComOrdem
	(
		Ordem			INT,
		IdProdutoPai	INT,
		ValorUnitario	DECIMAL(18,2),
		Tipo			VARCHAR(MAX),
		NUM_NF			INT NULL,
		DataEntrada		DATE NULL
	)
	
	INSERT INTO #ValorMaiorUnitarioComOrdem (Ordem, IdProdutoPai, ValorUnitario, Tipo, NUM_NF, DataEntrada)
	SELECT 
		RANK() OVER (PARTITION BY M.IdProdutoPai ORDER BY M.IdProdutoPai ASC, MIN(M.Tipo) ASC) ordem,
		M.IdProdutoPai, M.ValorUnitario, MIN(M.Tipo) AS Tipo, null, null
	FROM 
		#MaiorValorUnitario M
	GROUP BY
		M.IdProdutoPai, M.ValorUnitario
		
	DELETE FROM #ValorMaiorUnitarioComOrdem WHERE Ordem > 1	
	
	/*SELECT
		Produto.Id IdProduto,
		ProdUnificado.Descricao DescricaoUnificada, 
		
		Valor.ValorUnitario, 
		
		--CASE
		--	WHEN (Valor.Tipo = '1-SC' OR Valor.Tipo = '5-S') THEN 'Saída'
		--	WHEN (Valor.Tipo = '2-EC' OR Valor.Tipo = '6-E') THEN 'Entrada'
		--	WHEN (Valor.Tipo = '3-FC' OR Valor.Tipo = '7-F') THEN 'Final'
		--	WHEN (Valor.Tipo = '4-IC' OR Valor.Tipo = '8-I') THEN 'Inicial'
		--END Origem,
		
		CASE
			WHEN (Valor.Tipo = '1-SC') THEN 'Saída Contest'
			WHEN (Valor.Tipo = '5-S') THEN 'Saída'
			WHEN (Valor.Tipo = '2-EC') THEN 'Entrada Contest'
			WHEN (Valor.Tipo = '6-E') THEN 'Entrada'
			WHEN (Valor.Tipo = '3-FC') THEN 'Final Contest'
			WHEN (Valor.Tipo = '7-F') THEN 'Final'
			WHEN (Valor.Tipo = '4-IC') THEN 'Inicial Contest'
			WHEN (Valor.Tipo = '8-I') THEN 'Inicial'
		END Origem
		
		,COALESCE(MovSaiContest.NUM_NF, MovEntContest.NUM_NF, MovSai.NUM_NF, MovEnt.NUM_NF) NotaFiscal
	FROM 
		#ValorMaiorUnitarioComOrdem Valor
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Valor.IdProdutoPai
		INNER JOIN Produto Produto ON Produto.IdProdutoPai = ProdUnificado.Id*/
		
	
	IF OBJECT_ID('tempdb..#movimentacaoSaida') IS NOT NULL 
		DROP TABLE #movimentacaoSaida
	
	select 
		ROW_NUMBER() OVER (PARTITION BY Prod.IdProdutoPai ORDER BY Prod.IdProdutoPai, ValorUnitario DESC, Mov.NUM_NF DESC) ordem,
		Prod.IdProdutoPai, mov.* 
	into
		#movimentacaoSaida
	from 
		Movimentacao mov 
		inner join produto prod on prod.id = mov.idproduto
	where
		mov.Tipo = 'S' and
		prod.IdAnalise = @IdAnalise
	
	delete from #movimentacaoSaida where ordem > 1	
	
	IF OBJECT_ID('tempdb..#movimentacaoEntrada') IS NOT NULL 
		DROP TABLE #movimentacaoEntrada
	
	select 
		ROW_NUMBER() OVER (PARTITION BY Prod.IdProdutoPai ORDER BY Prod.IdProdutoPai, ValorUnitario DESC, Mov.NUM_NF DESC) ordem,
		Prod.IdProdutoPai, mov.* 
	into
		#movimentacaoEntrada
	from 
		Movimentacao mov 
		inner join produto prod on prod.id = mov.idproduto
	where
		mov.Tipo = 'E' and
		prod.IdAnalise = @IdAnalise
	
	delete from #movimentacaoEntrada where ordem > 1		
	
	UPDATE
		ValorMaior
	SET
		NUM_NF = MovSaiContest.NUM_NF,
		DataEntrada = MovSaiContest.DataEntrada
	FROM
		#ValorMaiorUnitarioComOrdem ValorMaior
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = ValorMaior.IdProdutoPai
		INNER JOIN Produto Produto ON Produto.IdProdutoPai = ProdUnificado.Id
		INNER JOIN #DadosMovimentacaoContestadoComOrdem MovSaiContest ON (MovSaiContest.Tipo = 'S' AND MovSaiContest.Id = ProdUnificado.IdProdContesPlaMovSai)
	WHERE
		ValorMaior.Tipo = '1-SC'
			
	UPDATE
		ValorMaior
	SET
		NUM_NF = MovEntContest.NUM_NF,
		DataEntrada = MovEntContest.DataEntrada
	FROM
		#ValorMaiorUnitarioComOrdem ValorMaior
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = ValorMaior.IdProdutoPai
		INNER JOIN Produto Produto ON Produto.IdProdutoPai = ProdUnificado.Id
		INNER JOIN #DadosMovimentacaoContestadoComOrdem MovEntContest ON (MovEntContest.Tipo = 'E' AND MovEntContest.Id = ProdUnificado.IdProdContesPlaMovEnt)
	WHERE
		ValorMaior.Tipo = '2-EC'

	UPDATE
		ValorMaior
	SET
		NUM_NF = MovSai.NUM_NF,
		DataEntrada = MovSai.DataEntrada
	FROM
		#ValorMaiorUnitarioComOrdem ValorMaior
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = ValorMaior.IdProdutoPai
		INNER JOIN Produto Produto ON Produto.IdProdutoPai = ProdUnificado.Id
		INNER JOIN #movimentacaoSaida MovSai ON (MovSai.IdProdutoPai = ValorMaior.IdProdutoPai)
	WHERE
		ValorMaior.Tipo = '5-S'
		
	UPDATE
		ValorMaior
	SET
		NUM_NF = MovEnt.NUM_NF,
		DataEntrada = MovEnt.DataEntrada
	FROM
		#ValorMaiorUnitarioComOrdem ValorMaior
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = ValorMaior.IdProdutoPai
		INNER JOIN Produto Produto ON Produto.IdProdutoPai = ProdUnificado.Id
		INNER JOIN #movimentacaoEntrada MovEnt ON (MovEnt.IdProdutoPai = ValorMaior.IdProdutoPai)
	WHERE
		ValorMaior.Tipo = '6-E'		
		
	select distinct
		ProdUnificado.Descricao Descricao, 
		Valor.ValorUnitario,
		CASE
			WHEN (Valor.Tipo = '1-SC' OR Valor.Tipo = '5-S') THEN 'Saída'
			WHEN (Valor.Tipo = '2-EC' OR Valor.Tipo = '6-E') THEN 'Entrada'
			WHEN (Valor.Tipo = '3-FC' OR Valor.Tipo = '7-F') THEN 'Final'
			WHEN (Valor.Tipo = '4-IC' OR Valor.Tipo = '8-I') THEN 'Inicial'
		END PlanilhaOrigem,
		Valor.NUM_NF NotaFiscal,
		Valor.DataEntrada
	into
		#relatorio
	from 
		#ValorMaiorUnitarioComOrdem Valor
		inner join produto ProdUnificado on produnificado.id = Valor.IdProdutoPai
		inner join produto Prod on prod.IdProdutoPai = Valor.IdProdutoPai
	where
		Prod.Ativo = 1;

	With CTE_RelatorioPreco as
	(
		Select 
			ROW_NUMBER() over 
			(
				order by 
					case when (@SortCol = 0 and @SortDir='asc') then Descricao end asc,
					case when (@SortCol = 0 and @SortDir='desc') then Descricao end desc,
					case when (@SortCol = 1 and @SortDir='asc') then ValorUnitario end asc,
					case when (@SortCol = 1 and @SortDir='desc') then ValorUnitario end desc,
					case when (@SortCol = 2 and @SortDir='asc') then PlanilhaOrigem end asc,
					case when (@SortCol = 2 and @SortDir='desc') then PlanilhaOrigem end desc,
					case when (@SortCol = 3 and @SortDir='asc') then NotaFiscal end asc,
					case when (@SortCol = 3 and @SortDir='desc') then NotaFiscal end desc,
					case when (@SortCol = 4 and @SortDir='asc') then DataEntrada end asc,
					case when (@SortCol = 4 and @SortDir='desc') then DataEntrada end desc
			) as RowNum,

			COUNT(*) over() as TotalCount,
			
			Descricao,
			ValorUnitario,
			PlanilhaOrigem,
			NotaFiscal,
			DataEntrada
		FROM
			#relatorio
		WHERE
		(
			@Search IS NULL 
			OR Descricao like '%' + @Search + '%'
			OR ValorUnitario like '%' + @Search + '%'
			OR PlanilhaOrigem like '%' + @Search + '%'
			OR NotaFiscal like '%' + @Search + '%'
			OR DataEntrada like '%' + @Search + '%'
		)
	)

	Select
		RowNum, TotalCount, Descricao, ValorUnitario, PlanilhaOrigem, NotaFiscal NUM_NF, '' + CONVERT(NVARCHAR(MAX), DataEntrada, 103) + '' DataEntrada
	from CTE_RelatorioPreco where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END