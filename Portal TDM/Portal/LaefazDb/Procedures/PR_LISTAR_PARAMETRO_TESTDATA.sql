CREATE PROCEDURE PR_LISTAR_PARAMETRO_TESTDATA
	   @DISPLAYLENGTH INT,
	   @DISPLAYSTART INT,
	   @SORTCOL INT,
	   @SORTDIR NVARCHAR(10),
	   @SEARCH NVARCHAR(255) = NULL,
	   @LISTARTODOS INT = 1,
	   @IDENCADEAMENTO INT = NULL,
	   @IDTESTDATA INT = NULL
AS
BEGIN

	   DECLARE @FIRSTREC INT, @LASTREC INT
	   SET @FIRSTREC = @DISPLAYSTART;
	   SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;
	   
	   With CTE_ParametroTestData as
	   (
			  SELECT DISTINCT
					 ROW_NUMBER() over 
					 (
						   order by
								  -- COLUNA DESCRIÇÃO DO TESTDATA --
								  case when (@SortCol = 0 and @SortDir='asc') then td.Descricao end asc,
								  case when (@SortCol = 0 and @SortDir='desc') then td.Descricao end desc,
						   
								  -- COLUNA DESCRIÇÃO DO PARAMETRO --
								  case when (@SortCol = 1 and @SortDir='asc') then p.Descricao end asc,
								  case when (@SortCol = 1 and @SortDir='desc') then p.Descricao end desc,
									 
								  -- COLUNA DESCRICAO DO TIPO DE PARAMETRO --
								  case when (@SortCol = 2 and @SortDir='asc') then tp.Descricao end asc,
								  case when (@SortCol = 2 and @SortDir='desc') then tp.Descricao end desc,
									 
								  -- COLUNA DO VALOR DO PARAMETRO --
								  case when (@SortCol = 4 and @SortDir='asc') then pv.Valor end asc,
								  case when (@SortCol = 4 and @SortDir='desc') then pv.Valor end desc   
					 ) as RowNum,

					 COUNT(*) over() as TotalCount,
					 td.Id IdTestData,
					 td.Descricao DescricaoTestData,
					 ps.IdParametro,
					 p.Descricao DescricaoParametro,
					 ps.IdTipoParametro IdTipoParametro,
					 tp.Descricao DescricaoTipoParametro,
					 pv.IdParametroValor_Origem,
					 pv.Valor,
					 pv.Id Id  
			  FROM 
					 TestData td
					 INNER JOIN ParametroValor pv on td.Id =pv.IdTestData
					 INNER JOIN Encadeamento_TestData encadTd on td.Id = encadTd.IdTestData
					 INNER JOIN DataPool dtp on td.IdDataPool = dtp.Id
					 INNER JOIN ParametroScript ps on pv.IdParametroScript = ps.Id
					 INNER JOIN Parametro p on ps.IdParametro = p.Id
					 INNER JOIN TipoParametro tp on ps.IdTipoParametro = tp.Id
					 --INNER JOIN TipoDadoParametro tdp on ps.IdTipoParametro = tdp.Id
			  WHERE 
			  (
						(encadTd.IdEncadeamento = @IDENCADEAMENTO OR @IDENCADEAMENTO IS NULL)
					AND (td.Id = @IDTESTDATA OR @IDTESTDATA IS NULL)
					AND (
                           @SEARCH IS NULL 
                           OR td.Descricao like '%' + @Search + '%'
                           OR pv.Valor like '%' + @Search + '%'
                           OR p.Descricao like '%' + @Search + '%'
                           OR tp.Descricao like '%' + @Search + '%'
                        )
              )
       )
       
	   Select * from CTE_ParametroTestData where RowNum > @FirstRec and RowNum <= @LastRec
END