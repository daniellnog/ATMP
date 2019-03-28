CREATE PROCEDURE PR_LISTAR_ANALISES
	@DISPLAYLENGTH INT,
	@DISPLAYSTART INT,
	@SORTCOL INT,
	@SORTDIR NVARCHAR(10),
	@SEARCH NVARCHAR(255) = NULL
AS
BEGIN
	DECLARE @FIRSTREC INT, @LASTREC INT
	SET @FIRSTREC = @DISPLAYSTART;
	SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

	With CTE_Analises as
	(
		Select ROW_NUMBER() over 
		(
			order by 
				case when (@SortCol = 0 and @SortDir='asc') then Descricao end asc,
				case when (@SortCol = 0 and @SortDir='desc') then Descricao end desc
		) as RowNum,
		
		COUNT(*) over() as TotalCount,
		Id,
		Descricao
		FROM Analise
		WHERE 
		(
			@Search IS NULL 
			OR Descricao like '%' + @Search + '%'
		)
	)
	Select * from CTE_Analises where RowNum > @FirstRec and RowNum <= @LastRec
END