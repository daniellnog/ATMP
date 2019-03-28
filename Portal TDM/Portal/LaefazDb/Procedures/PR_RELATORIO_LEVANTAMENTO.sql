CREATE PROCEDURE [dbo].[PR_RELATORIO_LEVANTAMENTO]
	@DISPLAYLENGTH INT,
	@DISPLAYSTART INT,
	@SORTCOL INT,
	@SORTDIR NVARCHAR(10),
	@SEARCH NVARCHAR(255) = NULL,
	@LISTARTODOS INT,
	@CORLINHA INT = NULL,
	@IDANALISE NVARCHAR(255)
AS
BEGIN
	DECLARE @FIRSTREC INT, @LASTREC INT
	SET @FIRSTREC = @DISPLAYSTART;
	SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

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
	
	SELECT 
		RANK() OVER (PARTITION BY M.IdProdutoPai ORDER BY M.IdProdutoPai ASC, MIN(M.Tipo) ASC) ordem,
		M.IdProdutoPai, M.ValorUnitario, CAST(NULL AS DECIMAL(18,2)) ValorMedio
	INTO
		#ValorMaiorUnitarioComOrdem
	FROM 
		#MaiorValorUnitario M
	GROUP BY
		M.IdProdutoPai, M.ValorUnitario

	;WITH Valores AS
	(
		SELECT
			IdProdutoPai, AVG(ValorUnitario) ValorMedio
		FROM
			#ValorMaiorUnitarioComOrdem ValorMaior
		GROUP BY
			IdProdutoPai
	)
	UPDATE T
	SET
		ValorMedio = Valores.ValorMedio
	FROM
		#ValorMaiorUnitarioComOrdem T
		INNER JOIN Valores ON Valores.IdProdutoPai = T.IdProdutoPai
	
	DELETE FROM #ValorMaiorUnitarioComOrdem WHERE Ordem > 1	
	
	DECLARE @Relatorio TABLE
	(
		IdProdutoPai		INT,
		ProdutoDescricao	NVARCHAR(MAX),
		UnidadeDescricao	NVARCHAR(MAX),
		QtdInicial			DECIMAL(18,2),
		QtdEntrada			DECIMAL(18,2),
		QtdSaida			DECIMAL(18,2),
		QtdFinal			DECIMAL(18,2),
		QtdFinalCalculado	DECIMAL(18,2),
		QtdDiferenca		DECIMAL(18,2),
		OMISSAO				NVARCHAR(MAX),
		ValorUnitario		DECIMAL(18,2),
		ValorMedio			DECIMAL(18,2),
		BaseCalculo			DECIMAL(18,2),
		CorLinha			VARCHAR(MAX)
	)

	IF OBJECT_ID('tempdb..#Levantamento') IS NOT NULL DROP TABLE #Levantamento

	SELECT SumProdutos.IdProdutoPai, SUM(SumProdutos.Inicial) Inicial, SUM(SumProdutos.Entrada) Entrada, SUM(SumProdutos.Saida) Saida, SUM(SumProdutos.Final) Final 
	INTO #LEVANTAMENTO
	FROM 
	(
		SELECT
			Prod.IdProdutoPai,
			ISNULL(SUM(CASE WHEN Est.Tipo = 'I' THEN Est.Quantidade ELSE 0 END),0) AS Inicial,
			0 Entrada,
			0 Saida,
			ISNULL(SUM(CASE WHEN Est.Tipo = 'F' THEN Est.Quantidade ELSE 0 END),0) AS Final
		FROM   
			Produto Prod
			LEFT JOIN Estoque Est ON Prod.ID = Est.IdProduto
		WHERE	
			Prod.IdAnalise = @IdAnalise AND
			Prod.Ativo = 1
		GROUP BY 
			Prod.IdProdutoPai
	
		UNION ALL

		SELECT 
			Prod.IdProdutoPai,
			0 AS Inicial,
			ISNULL(SUM(CASE WHEN Mov.Tipo = 'E' THEN Mov.Quantidade ELSE 0 END),0) AS Entrada,
			ISNULL(SUM(CASE WHEN Mov.Tipo = 'S' THEN Mov.Quantidade ELSE 0 END),0) AS Saida,
			0 AS Final
		FROM   
			Produto Prod
			LEFT JOIN Movimentacao Mov ON Prod.ID = Mov.IdProduto
		WHERE	
			Prod.IdAnalise = @IdAnalise AND
			Prod.Ativo = 1
		GROUP BY 
			Prod.IdProdutoPai
	) AS SumProdutos
	GROUP BY SumProdutos.IdProdutoPai

	UPDATE P
	SET 
		ValorUnitario = Mov.Valorunitario
	FROM 
		#ValorMaiorUnitarioComOrdem p 
		INNER JOIN Produto Prod ON prod.Id = p.IdProdutoPai
		INNER JOIN Movimentacao Mov ON Mov.Id = Prod.IdProdContesPlaMovSai

	UPDATE P
	SET 
		ValorUnitario = Mov.Valorunitario
	FROM 
		#ValorMaiorUnitarioComOrdem p 
		INNER JOIN Produto Prod ON prod.Id = p.IdProdutoPai
		INNER JOIN Movimentacao Mov ON Mov.Id = Prod.IdProdContesPlaMovEnt

	UPDATE P
	SET 
		ValorUnitario = Est.Valorunitario
	FROM 
		#ValorMaiorUnitarioComOrdem p 
		INNER JOIN Produto Prod ON prod.Id = p.IdProdutoPai
		INNER JOIN Estoque Est ON Est.Id = Prod.IdProdContesPlaInicial
	
	UPDATE P
	SET 
		ValorUnitario = Est.Valorunitario
	FROM 
		#ValorMaiorUnitarioComOrdem p 
		INNER JOIN Produto Prod ON prod.Id = p.IdProdutoPai
		INNER JOIN Estoque Est ON Est.Id = Prod.IdProdContesPlaFinal

	INSERT INTO @Relatorio
	-- Relatório
	SELECT 
		Levantamento.IdProdutoPai IdProdutoPai,
		ProdUnificado.Descricao ProdutoDescricao,
		Unid.Descricao UnidadeDescricao,
		Inicial QtdInicial,
		Entrada QtdEntrada,
		Saida QtdSaida,
		Final QtdFinal,
		(Inicial + Entrada - Saida) QtdFinalCalculado,
		(Final - (Inicial + Entrada - Saida)) QtdDiferenca,
		case
			when (Inicial + Entrada - Saida) < Final then 'Entrada'
			when (Inicial + Entrada - Saida) > Final then 'Saída'
			else ''
		end OMISSAO,
		MaiorValor.ValorUnitario,
		MaiorValor.ValorMedio,
		CONVERT(DECIMAL(18,2),ABS(((Final - (Inicial + Entrada - Saida)) * MaiorValor.ValorUnitario))) BaseCalculo,
		CASE 
			-- Linha Laranja
			WHEN ProdUnificado.IdProdContesPlaMovSai IS NULL AND ProdUnificado.IdProdContesPlaMovEnt IS NULL AND ProdUnificado.IdProdContesPlaFinal IS NULL AND ProdUnificado.IdProdContesPlaInicial IS NULL AND ProdUnificado.Contestado = 1 THEN 'laranja'
			
			-- Linha Verde
			WHEN (MaiorValor.ValorUnitario > (ValorMedio*3)) AND (ProdUnificado.Contestado = 0 OR ProdUnificado.Contestado IS NULL) THEN 'verde' 
			
			ELSE
			''
		END CorLinha
	FROM
		#Levantamento Levantamento
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Levantamento.IdProdutoPai
		INNER JOIN Unidade Unid ON Unid.ID = ProdUnificado.IdUnidade
		LEFT JOIN #ValorMaiorUnitarioComOrdem MaiorValor ON MaiorValor.IdProdutoPai = Levantamento.IdProdutoPai
	WHERE
		ProdUnificado.Ativo = 1;

	With CTE_RelatorioLevantamento as
	(
		Select 
			ROW_NUMBER() over 
			(
				order by 
					case when (@SortCol = 0 and @SortDir='asc') then ProdutoDescricao end asc,
					case when (@SortCol = 0 and @SortDir='desc') then ProdutoDescricao end desc,
					case when (@SortCol = 1 and @SortDir='asc') then UnidadeDescricao end asc,
					case when (@SortCol = 1 and @SortDir='desc') then UnidadeDescricao end desc,
					case when (@SortCol = 2 and @SortDir='asc') then QtdInicial end asc,
					case when (@SortCol = 2 and @SortDir='desc') then QtdInicial end desc,
					case when (@SortCol = 3 and @SortDir='asc') then QtdEntrada end asc,
					case when (@SortCol = 3 and @SortDir='desc') then QtdEntrada end desc,
					case when (@SortCol = 4 and @SortDir='asc') then QtdSaida end asc,
					case when (@SortCol = 4 and @SortDir='desc') then QtdSaida end desc,
					case when (@SortCol = 5 and @SortDir='asc') then QtdFinal end asc,
					case when (@SortCol = 5 and @SortDir='desc') then QtdFinal end desc,
					case when (@SortCol = 6 and @SortDir='asc') then QtdFinalCalculado end asc,
					case when (@SortCol = 6 and @SortDir='desc') then QtdFinalCalculado end desc,
					case when (@SortCol = 7 and @SortDir='asc') then QtdDiferenca end asc,
					case when (@SortCol = 7 and @SortDir='desc') then QtdDiferenca end desc,
					case when (@SortCol = 8 and @SortDir='asc') then OMISSAO end asc,
					case when (@SortCol = 8 and @SortDir='desc') then OMISSAO end desc,
					case when (@SortCol = 9 and @SortDir='asc') then ValorUnitario end asc,
					case when (@SortCol = 9 and @SortDir='desc') then ValorUnitario end desc,
					case when (@SortCol = 10 and @SortDir='asc') then BaseCalculo end asc,
					case when (@SortCol = 10 and @SortDir='desc') then BaseCalculo end desc
			) as RowNum,

			COUNT(*) over() as TotalCount,
			
			IdProdutoPai,
			ProdutoDescricao,	
			UnidadeDescricao,	
			QtdInicial,		
			QtdEntrada,			
			QtdSaida,			
			QtdFinal,			
			QtdFinalCalculado,	
			QtdDiferenca,		
			OMISSAO,				
			ValorUnitario,		
			BaseCalculo,
			ValorMedio,
			CorLinha
		FROM
			@Relatorio
		WHERE
		(
			(
				@CorLinha IS NULL OR 
				CorLinha = 
						CASE
							WHEN @CorLinha = 1 THEN 'verde'
							WHEN @CorLinha = 2 THEN 'laranja'
						END
			) AND
			(
				@Search IS NULL 
				OR ProdutoDescricao like '%' + @Search + '%'
				OR UnidadeDescricao like '%' + @Search + '%'
				OR QtdInicial like '%' + @Search + '%'
				OR QtdEntrada like '%' + @Search + '%'
				OR QtdSaida like '%' + @Search + '%'
				OR QtdFinal like '%' + @Search + '%'
				OR QtdFinalCalculado like '%' + @Search + '%'
				OR QtdDiferenca like '%' + @Search + '%'
				OR ValorUnitario like '%' + @Search + '%'
				OR OMISSAO like '%' + @Search + '%'
				OR BaseCalculo like '%' + @Search + '%'
			)
		)
	)

	Select
		RowNum, 
		TotalCount, 
		IdProdutoPai,
		ProdutoDescricao,	
		UnidadeDescricao,	
		ISNULL(QtdInicial,0) QtdInicial,		
		ISNULL(QtdEntrada,0) QtdEntrada,			
		ISNULL(QtdSaida,0) QtdSaida,			
		ISNULL(QtdFinal,0) QtdFinal,			
		ISNULL(QtdFinalCalculado,0) QtdFinalCalculado,	
		ISNULL(QtdDiferenca,0) QtdDiferenca,		
		OMISSAO,				
		ISNULL(ValorUnitario,0) ValorUnitario,		
		ISNULL(BaseCalculo,0) BaseCalculo,
		CorLinha
	from CTE_RelatorioLevantamento where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END