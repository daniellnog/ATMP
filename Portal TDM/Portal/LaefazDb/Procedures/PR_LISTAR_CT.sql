CREATE PROCEDURE PR_LISTAR_CT 
		@IDAUT NVARCHAR(255) = NULL, 
        @IDCT  NVARCHAR(255) = NULL,
        @FASECT  NVARCHAR(255) = NULL  
AS 
  BEGIN 
      SELECT ct.id, 
             ct.nome Descricao, 
             ct.sistema, 
             ct.fase
      FROM   ct ct 
             INNER JOIN aut aut 
                     ON ct.sistema COLLATE latin1_general_ci_as = aut.descricao 
      WHERE  ( aut.id = @IDAUT OR @IDAUT IS NULL) 
             AND ( ct.id = @IDCT 
                    OR @IDCT IS NULL ) 
             AND (ct.Fase = @FASECT OR @FASECT IS NULL)
  END 