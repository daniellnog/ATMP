CREATE PROCEDURE [dbo].[PR_PLANILHA_ESTOQUE_FINAL]
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

	With CTE_PlanilhaEstoqueFinal as
	(
		Select 
			ROW_NUMBER() over 
			(
				order by 
					case when (@SortCol = 0 and @SortDir='asc') then Prod.Descricao end asc,
					case when (@SortCol = 0 and @SortDir='desc') then Prod.Descricao end desc,
					case when (@SortCol = 1 and @SortDir='asc') then ProdUnificado.Descricao end asc,
					case when (@SortCol = 1 and @SortDir='desc') then ProdUnificado.Descricao end desc,
					case when (@SortCol = 2 and @SortDir='asc') then Unid.Descricao end asc,
					case when (@SortCol = 2 and @SortDir='desc') then Unid.Descricao end desc,
					case when (@SortCol = 3 and @SortDir='asc') then Quantidade end asc,
					case when (@SortCol = 3 and @SortDir='desc') then Quantidade end desc,
					case when (@SortCol = 4 and @SortDir='asc') then ValorUnitario end asc,
					case when (@SortCol = 4 and @SortDir='desc') then ValorUnitario end desc,
					case when (@SortCol = 5 and @SortDir='asc') then ValorTotal end asc,
					case when (@SortCol = 5 and @SortDir='desc') then ValorTotal end desc
			) as RowNum,
		
			COUNT(*) over() as TotalCount,

			Prod.Descricao ProdutoDescricao,
			ProdUnificado.Descricao ProdutoUnificado,
			Unid.Descricao UnidadeDescricao,
			Quantidade,
			ValorUnitario,
			ValorTotal
		FROM
			Produto Prod
			INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Prod.IdProdutoPai
			INNER JOIN Unidade Unid ON Unid.ID = Prod.IdUnidade
			INNER JOIN Estoque ON Estoque.IdProduto = Prod.Id AND Estoque.Tipo = 'F'
		WHERE 
		(
			Prod.Ativo = 1 AND
			Prod.IdAnalise = @IDANALISE AND 
			(
				@Search IS NULL 
				OR Prod.Descricao like '%' + @Search + '%'
				OR Unid.Descricao like '%' + @Search + '%'
				OR ProdUnificado.Descricao like '%' + @Search + '%'
				OR Quantidade like '%' + @Search + '%'
				OR ValorUnitario like '%' + @Search + '%'
				OR ValorTotal like '%' + @Search + '%'
			)
		)
	)
	Select * from CTE_PlanilhaEstoqueFinal where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END