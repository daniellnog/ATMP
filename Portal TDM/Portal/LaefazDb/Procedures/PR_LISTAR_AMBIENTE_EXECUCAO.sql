CREATE PROCEDURE Pr_listar_ambiente_execucao @IDDATAPOOL NVARCHAR(255) = NULL 
AS 
  BEGIN 
      SELECT DISTINCT( ae.id ), 
                     ae.descricao 
      FROM   ambienteexecucao ae 
             INNER JOIN script_condicaoscript_ambiente sca 
                     ON ae.id = sca.idambienteexecucao 
             INNER JOIN datapool dtp 
                     ON sca.idscript_condicaoscript = 
                        dtp.idscript_condicaoscript 
      WHERE  dtp.id = @IDDATAPOOL 
              OR @IDDATAPOOL IS NULL 
  END 