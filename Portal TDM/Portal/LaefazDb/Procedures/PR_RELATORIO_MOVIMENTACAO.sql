CREATE PROCEDURE [dbo].[PR_RELATORIO_MOVIMENTACAO]
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

	With CTE_RelatorioMovimentacao as
	(
		SELECT
			ROW_NUMBER() over 
			(
				order by 
					case when (@SortCol = 0 and @SortDir='asc') then Produto.Descricao end asc,
					case when (@SortCol = 0 and @SortDir='desc') then Produto.Descricao end desc,
					case when (@SortCol = 1 and @SortDir='asc') then ProdUnificado.Descricao end asc,
					case when (@SortCol = 1 and @SortDir='desc') then ProdUnificado.Descricao end desc,
					case when (@SortCol = 2 and @SortDir='asc') then Unidade.Descricao end asc,
					case when (@SortCol = 2 and @SortDir='desc') then Unidade.Descricao end desc,
					case when (@SortCol = 3 and @SortDir='asc') then Inicial end asc,
					case when (@SortCol = 3 and @SortDir='desc') then Inicial end desc,
					case when (@SortCol = 4 and @SortDir='asc') then Entrada end asc,
					case when (@SortCol = 4 and @SortDir='desc') then Entrada end desc,
					case when (@SortCol = 5 and @SortDir='asc') then Saida end asc,
					case when (@SortCol = 5 and @SortDir='desc') then Saida end desc,
					case when (@SortCol = 6 and @SortDir='asc') then Final end asc,
					case when (@SortCol = 6 and @SortDir='desc') then Final end desc,
					case when (@SortCol = 7 and @SortDir='asc') then ValorInicial end asc,
					case when (@SortCol = 7 and @SortDir='desc') then ValorInicial end desc,
					case when (@SortCol = 8 and @SortDir='asc') then ValorEntrada end asc,
					case when (@SortCol = 8 and @SortDir='desc') then ValorEntrada end desc,
					case when (@SortCol = 9 and @SortDir='asc') then ValorSaida end asc,
					case when (@SortCol = 9 and @SortDir='desc') then ValorSaida end desc,
					case when (@SortCol = 10 and @SortDir='asc') then ValorFinal end asc,
					case when (@SortCol = 10 and @SortDir='desc') then ValorFinal end desc
			) as RowNum,
		
			Produto.Descricao ProdutoDescricao,
			ProdUnificado.Descricao ProdutoUnificado,
			Unidade.Descricao UnidadeDescricao,
			Inicial QtdInicial,
			Entrada QtdEntrada,
			Saida QtdSaida,
			Final QtdFinal,
			ValorInicial,
			ValorEntrada,
			ValorSaida,
			ValorFinal
		FROM
			(
			Select 
				dados.Id Id,
				SUM(Estoque_Inicial) Inicial,
				SUM(Estoque_Final) Final,
				SUM(Estoque_ValorInicial) ValorInicial,
				SUM(Estoque_ValorFinal) ValorFinal,
				SUM(Movimentacao_Inicial) Entrada,
				SUM(Movimentacao_Final) Saida,
				SUM(Movimentacao_ValorInicial) ValorEntrada,
				SUM(Movimentacao_ValorFinal) ValorSaida
			FROM
			(
				SELECT 
					Prod.Id,
					SUM(ISNULL(CASE WHEN Est.Tipo = 'I' THEN Est.Quantidade ELSE 0 END,0)) AS Estoque_Inicial,
					SUM(ISNULL(CASE WHEN Est.Tipo = 'F' THEN Est.Quantidade ELSE 0 END,0)) AS Estoque_Final,
					MAX(CASE WHEN Est.Tipo = 'I' THEN Est.ValorUnitario ELSE NULL END) AS Estoque_ValorInicial,
					MAX(CASE WHEN Est.Tipo = 'F' THEN Est.ValorUnitario ELSE NULL END) AS Estoque_ValorFinal,
					0 AS Movimentacao_Inicial,
					0 AS Movimentacao_Final,
					0 AS Movimentacao_ValorInicial,
					0 AS Movimentacao_ValorFinal
				FROM   
					Produto Prod
					LEFT JOIN Estoque Est ON Prod.ID = Est.IdProduto
				GROUP BY 
					Prod.Id

				UNION ALL

				SELECT 
					Prod.Id,
					0,
					0,
					0,
					0,
					SUM(ISNULL(CASE WHEN Mov.Tipo = 'E' THEN Mov.Quantidade ELSE 0 END,0)),
					SUM(ISNULL(CASE WHEN Mov.Tipo = 'S' THEN Mov.Quantidade ELSE 0 END,0)),
					ISNULL(MAX(CASE WHEN Mov.Tipo = 'E' THEN Mov.ValorUnitario ELSE NULL END),0),
					ISNULL(MAX(CASE WHEN Mov.Tipo = 'S' THEN Mov.ValorUnitario ELSE NULL END),0) AS ValorFinal
				FROM   
					Produto Prod
					LEFT JOIN Movimentacao Mov ON Prod.ID = Mov.IdProduto
				GROUP BY 
					Prod.Id
			) dados
			GROUP BY 
				dados.Id
		) dados
		INNER JOIN Produto ON Produto.Id = dados.Id
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Produto.IdProdutoPai
		INNER JOIN Unidade ON Unidade.Id = Produto.IdUnidade
		WHERE 
		(
			Produto.Ativo = 1 AND
			Produto.IdAnalise = @IDANALISE AND 
			(
				@Search IS NULL 
				OR Produto.Descricao like '%' + @Search + '%'
				OR Unidade.Descricao like '%' + @Search + '%'
				OR ProdUnificado.Descricao like '%' + @Search + '%'
				OR Inicial like '%' + @Search + '%'
				OR Entrada like '%' + @Search + '%'
				OR Saida like '%' + @Search + '%'
				OR Final like '%' + @Search + '%'
				OR ValorInicial like '%' + @Search + '%'
				OR ValorEntrada like '%' + @Search + '%'
				OR ValorSaida like '%' + @Search + '%'
				OR ValorFinal like '%' + @Search + '%'
			)
		)
	)
	Select * from CTE_RelatorioMovimentacao where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END