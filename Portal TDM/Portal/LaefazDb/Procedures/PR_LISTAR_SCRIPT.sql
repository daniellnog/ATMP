CREATE PROCEDURE PR_LISTAR_SCRIPT 
		@IDAUT        NVARCHAR(255), 
        @IDFASESCRIPT NVARCHAR(255) = NULL, 
        @IDPLATAFORMA NVARCHAR(255) = NULL 
AS 
  BEGIN 
      SELECT DISTINCT scs.id Id, 
                      s.descricao + ISNULL(cs.descricao, '') Nome, 
                      p.id                                   IdPlataforma, 
                      p.descricao                            DescricaoPlataforma, 
                      ae.id IdFaseScript, 
                      ae.descricao                           DescricaoFaseScript, 
                      aut.descricao Sistema
      FROM   script s 
             INNER JOIN script_condicaoscript scs ON s.id = scs.idscript 
             LEFT OUTER JOIN script_condicaoscript_ambiente sca ON scs.id = sca.idscript_condicaoscript 
             LEFT OUTER JOIN condicaoscript cs ON scs.idcondicaoscript = cs.id 
             INNER JOIN aut aut ON s.idaut = aut.id 
             INNER JOIN plataforma p ON scs.idplataforma = p.id 
             LEFT OUTER JOIN ambienteexecucao ae ON sca.idambienteexecucao = ae.id 
      WHERE  
				 ( aut.id = @IDAUT OR @IDAUT IS NULL ) 
             AND ( sca.idambienteexecucao = @IDFASESCRIPT OR @IDFASESCRIPT IS NULL ) 
             AND ( scs.idplataforma = @IDPLATAFORMA OR @IDPLATAFORMA IS NULL ) 
  END 