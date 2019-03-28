CREATE PROCEDURE [dbo].[PR_PLANILHA_PRODUTO]
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

	With CTE_PlanilhaProduto as
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
					case when (@SortCol = 2 and @SortDir='desc') then Unid.Descricao end desc
			) as RowNum,
		
			COUNT(*) over() as TotalCount,
		
			Prod.Descricao DescricaoOriginal,
			ProdUnificado.Descricao DescricaoUnificada,
			Unid.Descricao DescricaoUnidade
		FROM
			Produto Prod
			INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Prod.IdProdutoPai
			INNER JOIN Unidade Unid ON Unid.ID = Prod.IdUnidade
		WHERE 
		(
			Prod.Ativo = 1 AND
			Prod.IdAnalise = @IDANALISE AND 
			(
				@Search IS NULL 
				OR Prod.Descricao like '%' + @Search + '%'
				OR Unid.Descricao like '%' + @Search + '%'
				OR ProdUnificado.Descricao like '%' + @Search + '%'
			)
		)
	)
	Select * from CTE_PlanilhaProduto where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END