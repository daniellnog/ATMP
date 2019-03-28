CREATE PROCEDURE PR_CARREGAR_ENCADEAMENTOS 
    @DISPLAYLENGTH INT,
    @DISPLAYSTART INT,
    @SORTCOL INT,
    @SORTDIR NVARCHAR(10),
    @SEARCH NVARCHAR(255) = NULL,
    @LISTARTODOS INT = 1,
	@IDSTATUSCADASTRADA NVARCHAR(1) = 1, 
	@IDSTATUSEMGERACAO  NVARCHAR(1) = 2, 
	@IDSTATUSDISPONIVEL NVARCHAR(1) = 3, 
	@IDSTATUSERRO       NVARCHAR(1) = 4, 
	@IDSTATUSRESERVADA  NVARCHAR(1) = 5, 
	@IDSTATUSUTILIZADA  NVARCHAR(1) = 6 
AS 
BEGIN 	
	   DECLARE @FIRSTREC INT, @LASTREC INT
       SET @FIRSTREC = @DISPLAYSTART;
       SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

	   With CTE_Encadeamento as
	   (
	   SELECT 
              ROW_NUMBER() over
              (
                      order by
								 -- COLUNA DESCRIÇÃO DO ENCADEAMENTO --
                                  case when (@SortCol = 1 and @SortDir='asc') then en.Descricao end asc,
                                  case when (@SortCol = 1 and @SortDir='desc') then en.Descricao end desc,
                           
                                  -- COLUNA QTD TESTEDATA TOTAL --
                                  case when (@SortCol = 2 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE  encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 2 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE  encadeamento_testdata.idencadeamento = en.id) end desc,
                                  
                                  -- COLUNA QTD TESTDATA CADASTRADA --
                                  case when (@SortCol = 3 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSCADASTRADA AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 3 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSCADASTRADA AND encadeamento_testdata.idencadeamento = en.id) end desc,
                                  
                                  -- COLUNA QTD TESTDATA EM GERAÇÃO --
                                  case when (@SortCol = 4 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT)FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE  testdata.idstatus = @IDSTATUSEMGERACAO AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 4 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT)FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE  testdata.idstatus = @IDSTATUSEMGERACAO AND encadeamento_testdata.idencadeamento = en.id) end desc,
                                  
                                  -- COLUNA QTD TESTDATA DISPONÍVEL --
                                  case when (@SortCol = 5 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSDISPONIVEL AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 5 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSDISPONIVEL AND encadeamento_testdata.idencadeamento = en.id) end desc,
                                  
                                  -- COLUNA QTD TESTDATA ERRO--
                                  case when (@SortCol = 6 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSERRO AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 6 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSERRO AND encadeamento_testdata.idencadeamento = en.id) end desc,
                                  
                                  -- COLUNA QTD TESTDATA RESERVADA --
                                  case when (@SortCol = 7 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSRESERVADA AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 7 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSRESERVADA AND encadeamento_testdata.idencadeamento = en.id) end desc,
                                 
                                  -- COLUNA QTD TESTDATA UTILIZADA --
                                  case when (@SortCol = 8 and @SortDir='asc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSUTILIZADA AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 8 and @SortDir='desc') then (SELECT CAST(COUNT(encadeamento_testdata.id) AS INT) FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE testdata.idstatus = @IDSTATUSUTILIZADA AND encadeamento_testdata.idencadeamento = en.id) end desc,
                                 
                                  -- COLUNA GERADO POR --
                                  case when (@SortCol = 9 and @SortDir='asc') then (SELECT DISTINCT testdata.geradopor FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE  testdata.geradopor IS NOT NULL AND encadeamento_testdata.idencadeamento = en.id) end asc,
                                  case when (@SortCol = 9 and @SortDir='desc') then (SELECT DISTINCT testdata.geradopor FROM encadeamento_testdata INNER JOIN testdata ON encadeamento_testdata.idtestdata = testdata.id WHERE  testdata.geradopor IS NOT NULL AND encadeamento_testdata.idencadeamento = en.id) end desc
                    ) as RowNum,

              COUNT(*) over() as TotalCount,
			 en.id IdEncadeamento, 
             en.descricao Descricao, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  encadeamento_testdata.idencadeamento = en.id) qtdTds, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.idstatus = @IDSTATUSCADASTRADA 
                     AND encadeamento_testdata.idencadeamento = en.id) 
             QtdCadastrada, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.idstatus = @IDSTATUSEMGERACAO 
                     AND encadeamento_testdata.idencadeamento = en.id) 
             QtdEmGeracao, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.idstatus = @IDSTATUSDISPONIVEL 
                     AND encadeamento_testdata.idencadeamento = en.id) 
             QtdDisponivel, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.idstatus = @IDSTATUSERRO 
                     AND encadeamento_testdata.idencadeamento = en.id) 
			 QtdErro, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.idstatus = @IDSTATUSRESERVADA 
                     AND encadeamento_testdata.idencadeamento = en.id) 
             QtdReservada, 
             (SELECT Count(encadeamento_testdata.id) 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.idstatus = @IDSTATUSUTILIZADA 
                     AND encadeamento_testdata.idencadeamento = en.id) 
             QtdUtilizada, 
             (SELECT DISTINCT testdata.geradopor 
              FROM   encadeamento_testdata 
                     INNER JOIN testdata 
                             ON encadeamento_testdata.idtestdata = testdata.id 
              WHERE  testdata.geradopor IS NOT NULL 
                     AND encadeamento_testdata.idencadeamento = en.id) GeradoPor 
      FROM   encadeamento en 
         
	  WHERE 
              (
                    
                     (
                           @SEARCH IS NULL 
                           OR en.descricao like '%' + @Search + '%'
                           --OR qtdTds like '%' + @Search + '%'
                           --OR QtdCadastrada like '%' + @Search + '%'
                           --OR QtdEmGeracao like '%' + @Search + '%'
                           --OR QtdDisponivel like '%' + @Search + '%'
                           --OR QtdErro like '%' + @Search + '%'
                           --OR QtdReservada like '%' + @Search + '%'
                           --OR QtdUtilizada like '%' + @Search + '%'
                           --OR GeradoPor like '%' + @Search + '%'
                     )
              )
			)

			SELECT DISTINCT * FROM CTE_Encadeamento where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
  END