CREATE PROCEDURE PR_LISTAR_PRODUTOS
	@DISPLAYLENGTH INT,
	@DISPLAYSTART INT,
	@SORTCOL INT,
	@SORTDIR NVARCHAR(10),
	@SEARCH NVARCHAR(255) = NULL,
	@ATIVO INT,
	@LISTARTODOS INT = 0,
	@TIPOUNIFICACAO INT = NULL,
	@IDANALISE NVARCHAR(255)
AS
BEGIN
	DECLARE @FIRSTREC INT, @LASTREC INT
	SET @FIRSTREC = @DISPLAYSTART;
	SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

	With CTE_Produtos as
	(
		Select 
			ROW_NUMBER() over 
			(
				order by 
					case when (@SortCol = 0 and @SortDir='asc') then Prod.Descricao end asc,
					case when (@SortCol = 0 and @SortDir='desc') then Prod.Descricao end desc,
					case when (@SortCol = 1 and @SortDir='asc') then ProdUnificado.Descricao end asc,
					case when (@SortCol = 1 and @SortDir='desc') then ProdUnificado.Descricao end desc,
					case when (@SortCol = 2 and @SortDir='asc') then Und.Descricao end asc,
					case when (@SortCol = 2 and @SortDir='desc') then Und.Descricao end desc
			) as RowNum,
		
			COUNT(*) over() as TotalCount,
			Prod.Id,
			Prod.IdUnidade,
			Prod.Descricao DescricaoOriginal,
			ISNULL(ProdUnificado.Descricao, '') DescricaoUnificada,
			Und.Descricao DescricaoUnidade,
			Prod.Unificado
		FROM 
			Produto Prod
			LEFT JOIN Produto ProdUnificado ON ProdUnificado.Id = Prod.IdProdutoPai
			LEFT JOIN Unidade Und ON Und.Id = Prod.IdUnidade
		WHERE 
		(
			(@TIPOUNIFICACAO IS NULL OR Prod.Unificado = @TIPOUNIFICACAO) AND
			Prod.Ativo = @ATIVO AND
			Prod.IdAnalise = @IDANALISE AND 
			Prod.Unificar = 1 AND
			(
				@Search IS NULL 
				OR Prod.Descricao like '%' + @Search + '%'
				OR Und.Descricao like '%' + @Search + '%'
				OR ProdUnificado.Descricao like '%' + @Search + '%'
			)
		)
	)
	Select * from CTE_Produtos where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END