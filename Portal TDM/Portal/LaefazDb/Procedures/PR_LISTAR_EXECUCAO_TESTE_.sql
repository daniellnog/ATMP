/****** Object:  StoredProcedure [dbo].[PR_LISTAR_EXECUCAO_TESTE]    Script Date: 02/15/2019 10:45:16 ******/
CREATE PROCEDURE PR_LISTAR_EXECUCAO_TESTE
       @DISPLAYLENGTH INT,
       @DISPLAYSTART INT,
       @SORTCOL INT,
       @SORTDIR NVARCHAR(10),
       @SEARCH NVARCHAR(255) = NULL,
       @LISTARTODOS INT = 1,
       @IDPROJETO INT = NULL,
       @IDSISTEMA INT = NULL,
       @IDPASTA INT = NULL,
       @IDPLANOTESTE INT = NULL
AS
BEGIN

	DECLARE @PROJETO VARCHAR(MAX), @SISTEMA VARCHAR(MAX), @PASTA VARCHAR(MAX), @PLANOTESTE VARCHAR(MAX);
	
	SELECT @PROJETO = PROJETO FROM PlanoTeste_TI WHERE ID = @IDPROJETO;
	SELECT @SISTEMA = SISTEMA FROM PlanoTeste_TI WHERE ID = @IDSISTEMA;
	SELECT @PASTA = PASTA_LM FROM PlanoTeste_TI WHERE ID = @IDPASTA;
	SELECT @PLANOTESTE = PLANO_TESTE FROM PlanoTeste_TI WHERE ID = @IDPLANOTESTE;

       DECLARE @FIRSTREC INT, @LASTREC INT
       SET @FIRSTREC = @DISPLAYSTART;
       SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

       With CTE_PlanoTeste as
       (
              SELECT 
                     ROW_NUMBER() over 
                     (
                           order by
                                  -- COLUNA DESCRIÇÃO DO PROJETO --
                                  case when (@SortCol = 0 and @SortDir='asc') then PT_TI1.PROJETO end asc,
                                  case when (@SortCol = 0 and @SortDir='desc') then PT_TI1.PROJETO end desc,
                           
                                  -- COLUNA DESCRIÇÃO DO SISTEMA --
                                  case when (@SortCol = 1 and @SortDir='asc') then PT_TI1.SISTEMA end asc,
                                  case when (@SortCol = 1 and @SortDir='desc') then PT_TI1.SISTEMA end desc
                     ) as RowNum,

                     COUNT(*) over() as TotalCount,
     --               ExecucaoAutomatizada_ATMP,
					--MotivoExecucao_ATMP,
					--Ofensor_ATMP,
					NRO_CT,
					NRO_CENARIO,
					NOME,
					Plano_Teste,
					PT_TI1.Id IdPlanoTeste_TI,

					
					--CASE 
					--	WHEN PT_TI1.SCRIPT_TOSCA_ATMP IS NOT NULL THEN PT_TI1.SCRIPT_TOSCA_ATMP
					--	WHEN PT_TI1.SCRIPT_RFT_ATMP  IS NOT NULL THEN PT_TI1.SCRIPT_RFT_ATMP
					--END SCRIPT_SELECIONADO, 


					LISTA_SCRIPTS =
					STUFF((
						SELECT DISTINCT ' ; ' + PLA.DESCRICAO + ' | ' + convert(varchar,SCS.ID) + ' | ' + SCS.NomeTecnicoScript
						FROM 
						PlanoTeste_TI  P
						INNER JOIN
						CT
						ON	P.NOME = CT.NOME 
						AND P.SISTEMA = CT.SISTEMA 
						AND P.CICLO  = CT.FASE 
						INNER JOIN
						SCRIPT_CONDICAOSCRIPT_CT SCSCT
						ON SCSCT.IDCT = CT.ID
						INNER JOIN Script_CondicaoScript SCS
						ON SCSCT.IDSCRIPT_CONDICAOSCRIPT = SCS.ID
						INNER JOIN
						PLATAFORMA PLA
						ON SCS.IDPLATAFORMA = PLA.ID
					WHERE ct.nome = PT_TI1.nome and ct.sistema = PT_TI1.sistema and PT_TI1.Ciclo = ct.Fase
					FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '')

            FROM 
				PlanoTeste_TI PT_TI1
				--LEFT JOIN CT ON PlanoTeste_TI.Sistema = CT.Sistema AND PlanoTeste_TI.Ciclo = CT.Fase AND PlanoTeste_TI.Nome = CT.Nome 
            WHERE 
            (
				(@IDPROJETO IS NULL OR PROJETO = @PROJETO) AND
				(@IDSISTEMA IS NULL OR PT_TI1.SISTEMA = @SISTEMA) AND
				(@IDPASTA IS NULL OR PASTA_LM = @PASTA) AND
				(@IDPLANOTESTE IS NULL OR PLANO_TESTE = @PLANOTESTE)
            ) 
       )
       
       Select * from CTE_PlanoTeste where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END