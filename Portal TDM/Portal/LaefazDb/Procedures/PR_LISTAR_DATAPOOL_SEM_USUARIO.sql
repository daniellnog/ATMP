CREATE PROCEDURE PR_LISTAR_DATAPOOL_SEM_USUARIO
       @DISPLAYLENGTH INT,
       @DISPLAYSTART INT,
       @SORTCOL INT,
       @SORTDIR NVARCHAR(10),
       @SEARCH NVARCHAR(255) = NULL,
       @LISTARTODOS INT = 1,
       @IDTDM NVARCHAR(255) = NULL,
	   @ROTINADIARIA INT = NULL
AS
BEGIN
       DECLARE @FIRSTREC INT, @LASTREC INT
       SET @FIRSTREC = @DISPLAYSTART;
       SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

       With CTE_DataPool as
       (
              SELECT 
                     ROW_NUMBER() over 
                     (
                           order by
                                  -- COLUNA DESCRIÇÃO DO TDM --
                                  case when (@SortCol = 0 and @SortDir='asc') then TDM.Descricao end asc,
                                  case when (@SortCol = 0 and @SortDir='desc') then TDM.Descricao end desc,
                           
                                  -- COLUNA DESCRIÇÃO DA DEMANDA --
                                  case when (@SortCol = 1 and @SortDir='asc') then Demanda.Descricao end asc,
                                  case when (@SortCol = 1 and @SortDir='desc') then Demanda.Descricao end desc,
                                  
                                  -- COLUNA DESCRIÇÃO DO SISTEMA --
                                  case when (@SortCol = 2 and @SortDir='asc') then AUT.Descricao end asc,
                                  case when (@SortCol = 2 and @SortDir='desc') then AUT.Descricao end desc,
                                  
                                  -- COLUNA DESCRIÇÃO DO SCRIPT --
                                  case when (@SortCol = 3 and @SortDir='asc') then ISNULL(Script.Descricao,'') + ' ' + ISNULL(CondicaoScript.Descricao,'') end asc,
                                  case when (@SortCol = 3 and @SortDir='desc') then ISNULL(Script.Descricao,'') + ' ' + ISNULL(CondicaoScript.Descricao,'') end desc,
                                  
                                  -- COLUNA QTD CADASTRADA --
                                  case when (@SortCol = 4 and @SortDir='asc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=1) end asc,
                                  case when (@SortCol = 4 and @SortDir='desc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=1) end desc,
                                  
                                  -- COLUNA QTD SOLICITADA --
                                  case when (@SortCol = 5 and @SortDir='asc') then DataPool.QtdSolicitada end asc,
                                  case when (@SortCol = 5 and @SortDir='desc') then DataPool.QtdSolicitada end desc,
                                  
                                  -- COLUNA QTD DISPONÍVEL --
                                  case when (@SortCol = 6 and @SortDir='asc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) end asc,
                                  case when (@SortCol = 6 and @SortDir='desc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) end desc,
                                  
                                  -- COLUNA QTD RESERVADA --
                                  case when (@SortCol = 7 and @SortDir='asc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=5) end asc,
                                  case when (@SortCol = 7 and @SortDir='desc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=5) end desc,
                                  
                                  -- COLUNA QTD UTILIZADA --
                                  case when (@SortCol = 8 and @SortDir='asc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6) end asc,
                                  case when (@SortCol = 8 and @SortDir='desc') then (SELECT CAST(COUNT(TestData.IdStatus) AS INT) FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6) end desc
                                  
                                  --case when (@SortCol = 7 and @SortDir='asc') then Farol end asc,
                                  --case when (@SortCol = 7 and @SortDir='desc') then Farol end desc,
                                  
                                  --case when (@SortCol = 8 and @SortDir='asc') then DataPool.Descricao end asc,
                                  --case when (@SortCol = 8 and @SortDir='desc') then DataPool.Descricao end desc
                     ) as RowNum,

                     COUNT(*) over() as TotalCount,
                     DataPool.Id,
                     DataPool.Descricao DescricaoDataPool,
                     DataPool.DataTermino DataTermino,
                     Demanda.Descricao DescricaoDemanda,
                     AUT.Descricao DescricaoSistema,
                     'Tipo' TipoMassa,
                     (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=1) QtdCadastrada,
                     DataPool.QtdSolicitada,
                     (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) QtdDisponivel,
                     (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=5) QtdReservada,
                     (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6) QtdUtilizada,
                     DataPool.DataSolicitacao,
                     CondicaoScript.Id IdCondicaoScript,
                     Script.Id IdScript,
                     TDM.Descricao DescricaoTDM,
                     ISNULL(Script.Descricao,'') + ' ' + ISNULL(CondicaoScript.Descricao,'') as DescricaoCondicaoScript    
              FROM 
                     DataPool
                     LEFT JOIN Demanda ON Demanda.Id = DataPool.IdDemanda
                     LEFT JOIN AUT ON AUT.Id = DataPool.IdAut
                     LEFT JOIN Script_CondicaoScript ON Script_CondicaoScript.Id = DataPool.IdScript_CondicaoScript
                     LEFT JOIN Script ON Script.Id = Script_CondicaoScript.IdScript
                     LEFT JOIN CondicaoScript ON CondicaoScript.Id = Script_CondicaoScript.IdCondicaoScript
                     LEFT JOIN TDM ON TDM.Id = DataPool.IdTDM
                     LEFT JOIN TDM_Usuario ON TDM_Usuario.IdTDM = TDM.Id
              WHERE 
              (
                     (DataPool.IdTDM = @IDTDM OR @IDTDM IS NULL) AND
					 (DataPool.ConsiderarRotinaDiaria = @ROTINADIARIA OR @ROTINADIARIA IS NULL) AND
                     (
                           @SEARCH IS NULL 
                           OR DataPool.Descricao like '%' + @Search + '%'
                           OR Demanda.Descricao like '%' + @Search + '%'
                           OR AUT.Descricao like '%' + @Search + '%'
                           OR DataPool.QtdSolicitada like '%' + @Search + '%'
                     )
              )
       )
       
       Select * from CTE_DataPool where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END