CREATE PROCEDURE PR_LISTAR_TESTDATA_CT 
		@IDCT NVARCHAR(255) 
AS 
  BEGIN 
      SELECT 
			 td.id                                        IdTestData, 
             td.caminhoevidencia, 
             s.descricao + ' ' + Isnull(cs.descricao, '') MassaDeDados, 
             dm.descricao                                 Demanda, 
             td.casotesterelativo                         NumeroCasoDeTeste, 
             st.descricao                                 Status, 
             td.geradopor, 
             td.datageracao 
      FROM   testdata td 
             INNER JOIN TestData_CT tdCt
                     ON td.id = tdCt.idtestdata 
             INNER JOIN CT ct 
                     ON tdCt.idct = ct.id 
             INNER JOIN DataPool dtp 
                     ON td.iddatapool = dtp.id 
             LEFT JOIN Demanda dm 
                    ON dtp.iddemanda = dm.id 
             INNER JOIN Status st 
                     ON td.idstatus = st.id 
             INNER JOIN Script_CondicaoScript scs 
                     ON dtp.idscript_condicaoscript = scs.id 
             INNER JOIN Script s 
                     ON scs.idscript = s.id 
             LEFT JOIN CondicaoScript cs 
                    ON scs.idcondicaoscript = cs.id 
      WHERE  
			 ct.id = @IDCT 
  END 