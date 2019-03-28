	/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
			   SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Cadastro de Script
IF NOT EXISTS (SELECT * FROM Script_CondicaoScript)
BEGIN

INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)   
(SELECT scr.Id, NULL,'CRIAR_PEDIDO_SBL8_REALIZAR_PEDIDO', '2001-01-01 00:30:00.000', '/Execution/ExecutionLists/CRIAR_PEDIDO_SBL8_REALIZAR_PEDIDO','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\criar-pedido-sbl8-realizar_pedido.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt', 

'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''URL''
	 AND parvAux.IdTestData = tdt.Id) AS URL,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF,
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''PDV''
	 AND parvAux.IdTestData = tdt.Id) AS PDV,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''LOGIN VENDEDOR''
	 AND parvAux.IdTestData = tdt.Id) AS LOGIN_VENDEDOR, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''TIPO DE USO''
	 AND parvAux.IdTestData = tdt.Id) AS TIPO_DE_USO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''CLASSIFICACAO''
	 AND parvAux.IdTestData = tdt.Id) AS CLASSIFICACAO, 
	 
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''CAMPANHA''
	 AND parvAux.IdTestData = tdt.Id) AS CAMPANHA, 
	 
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NOME PRODUTO''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_PRODUTO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''DATA ENTREGA''
	 AND parvAux.IdTestData = tdt.Id) AS DATA_ENTREGA, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''QTD PONTOS TV''
	 AND parvAux.IdTestData = tdt.Id) AS QTD_PONTOS_TV, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''DESCRICAO PEDIDO''
	 AND parvAux.IdTestData = tdt.Id) AS DESCRICAO_PEDIDO, 
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'REALIZAR PEDIDO');

INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)   
(SELECT scr.Id, NULL, 'CONTATO_E_CONTA_PF_SEM_ENVIO_CDI', '2001-01-01 00:30:00.000', '/Execution/ExecutionLists/CONTATO_E_CONTA_PF_SEM_ENVIO_CDI','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\contato-e-conta-pf-sem-envio-cdi.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt', 

'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''NOME CONTATO''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_CONTATO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''DDD''
	 AND parvAux.IdTestData = tdt.Id) AS DDD,
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''DATA NASCIMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS DATA_NASCIMENTO,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''LOCAL''
	 AND parvAux.IdTestData = tdt.Id) AS LOCAL, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''SEXO''
	 AND parvAux.IdTestData = tdt.Id) AS SEXO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''PDV''
	 AND parvAux.IdTestData = tdt.Id) AS PDV, 
	 
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''CEP''
	 AND parvAux.IdTestData = tdt.Id) AS CEP, 
	 
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NUMERO RESIDENCIA''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_RESIDENCIA, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''TIPO RESIDENCIA''
	 AND parvAux.IdTestData = tdt.Id) AS TIPO_RESIDENCIA, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''PONTO RESIDENCIA''
	 AND parvAux.IdTestData = tdt.Id) AS PONTO_RESIDENCIA, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NUMERO COMPLEMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_COMPLEMENTO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF, 	 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''PLANO CONVERGENTE''
	 AND parvAux.IdTestData = tdt.Id) AS PLANO_CONVERGENTE,	 
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI');


INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)   
(SELECT scr.Id, NULL, 'INCLUSAO_DE_CLIENTE_PF_NO_STC','2001-01-01 00:30:00.000','/Execution/ExecutionLists/INCLUSAO_DE_CLIENTE_PF_NO_STC','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\inclusao-de-cliente-pf-no-stc.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt', 
'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''TRILHA''
	 AND parvAux.IdTestData = tdt.Id) AS TRILHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''AMBIENTE SISTEMA''
	 AND parvAux.IdTestData = tdt.Id) AS AMBIENTE_SISTEMA,
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''ESTADO''
	 AND parvAux.IdTestData = tdt.Id) AS ESTADO,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''LOCAL''
	 AND parvAux.IdTestData = tdt.Id) AS LOCAL, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''TIPO PESSOA''
	 AND parvAux.IdTestData = tdt.Id) AS TIPO_PESSOA, 
	 
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NOME CLIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_CLIENTE, 
	 
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NOME MAE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_MAE, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NOME SOLICITANTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_SOLICITANTE, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''SEXO PESSOA''
	 AND parvAux.IdTestData = tdt.Id) AS SEXO_PESSOA, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''DIA NASCIMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS DIA_NASCIMENTO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''MES NASCIMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS MES_NASCIMENTO, 	 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''ANO NASCIMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS ANO_NASCIMENTO,	 
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC');

INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)  
(SELECT scr.Id, NULL, 'CRIAR_PEDIDO_SBL8_CRIAR_ENDERECO', '2001-01-01 00:30:00.000', '/Execution/ExecutionLists/CRIAR_PEDIDO_SBL8_CRIAR_ENDERECO','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\criar-pedido-sbl8-criar-endereco.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt',

'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''URL''
	 AND parvAux.IdTestData = tdt.Id) AS URL,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''NUMERO''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO,
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''ESTADO''
	 AND parvAux.IdTestData = tdt.Id) AS ESTADO,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''CEP''
	 AND parvAux.IdTestData = tdt.Id) AS CEP, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''TIPO COMPLEMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS TIPO_COMPLEMENTO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''N COMPLEMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS N_COMPLEMENTO, 
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA');


INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)  
(SELECT scr.Id, NULL,'CRIAR_PEDIDO_SBL8_CRIAR_CONTATO', '2001-01-01 00:30:00.000', '/Execution/ExecutionLists/CRIAR_PEDIDO_SBL8_CRIAR_CONTATO','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\criar-pedido-sbl8-criar-contato.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt',

'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''URL''
	 AND parvAux.IdTestData = tdt.Id) AS URL,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''TEL CONTATO''
	 AND parvAux.IdTestData = tdt.Id) AS TEL_CONTATO,
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''TIPO ID''
	 AND parvAux.IdTestData = tdt.Id) AS TIPO_ID, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''N IDENTIDADE''
	 AND parvAux.IdTestData = tdt.Id) AS N_IDENTIDADE, 
  
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO');

INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)  
(SELECT scr.Id, NULL,'CRIAR_PEDIDO_SBL8_CRIAR_PERFIL_FATURAMENTO', '2001-01-01 00:30:00.000', '/Execution/ExecutionLists/CRIAR_PEDIDO_SBL8_CRIAR_PERFIL_FATURAMENTO','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\criar-pedido-sbl8-criar-perfil-faturamento.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt',

'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''URL''
	 AND parvAux.IdTestData = tdt.Id) AS URL,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''NATUREZA PRODUTO''
	 AND parvAux.IdTestData = tdt.Id) AS NATUREZA_PRODUTO, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''MIDIA''
	 AND parvAux.IdTestData = tdt.Id) AS MIDIA, 
  
  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''DIA VENCIMENTO''
	 AND parvAux.IdTestData = tdt.Id) AS DIA_VENCIMENTO, 	 
  
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO');

INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)  
(SELECT scr.Id, NULL, 'CRIAR_PEDIDO_SBL8_REALIZAR_ANALISE_DE_CREDITO','2001-01-01 00:30:00.000', '/Execution/ExecutionLists/CRIAR_PEDIDO_SBL8_REALIZAR_ANALISE_DE_CREDITO','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\criar-pedido-sbl8-realizar-analise-de-credito.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt',
 'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''URL''
	 AND parvAux.IdTestData = tdt.Id) AS URL,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''PDV''
	 AND parvAux.IdTestData = tdt.Id) AS PDV, 

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
	INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
	INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
	WHERE parAux.Descricao = ''NOME PRODUTO''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_PRODUTO, 
	 
  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id' FROM Script scr WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO');

INSERT INTO Script_CondicaoScript (IdScript, IdCondicaoScript, NomeTecnicoScript, TempoEstimadoExecucao, ListaExecucaoTosca, CaminhoArquivoTCS, DiretorioRelatorio, QueryTosca)  
(SELECT scr.Id, NULL,'AJA_CADASTRA_ALTERAR_CLIENTE', '2001-01-01 00:30:00.000', '/Execution/ExecutionLists/AJA_CADASTRA_ALTERAR_CLIENTE','C:\Tosca_Projects\Tosca_Workspaces\TDM\remote_exec\aja-cadastra-altera-cliente.tcs','C:/Tosca_Projects/Tosca_Workspaces/TDM/remote_exec/Reports/report.txt',

'SELECT DISTINCT top 10000 tdt.Id AS ID_TEST_DATA,
					scr.Descricao AS SCRIPT,
					dem.Descricao AS DEMANDA,
					tdt.CasoTesteRelativo AS CASO_TESTE,
					aut.Descricao AS SISTEMA,
					ambexec.descricao as AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''CPF''
	 AND parvAux.IdTestData = tdt.Id) AS CPF,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''USUARIO SISTEMA''
	 AND parvAux.IdTestData = tdt.Id) AS USUARIO_SISTEMA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''SENHA SISTEMA''
	 AND parvAux.IdTestData = tdt.Id) AS SENHA_SISTEMA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''APLICACAO''
	 AND parvAux.IdTestData = tdt.Id) AS APLICACAO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''TRILHA''
	 AND parvAux.IdTestData = tdt.Id) AS TRILHA,

  (SELECT parvAux.valor 
	FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id  
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id  
   WHERE parAux.Descricao = ''TIPO PESSOA''
	 AND parvAux.IdTestData = tdt.Id) AS TIPO_PESSOA, 

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: RESULTADO ESPERADO''
	 AND parvAux.IdTestData = tdt.Id) AS RESULTADO_ESPERADO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AMBIENTE''
	 AND parvAux.IdTestData = tdt.Id) AS EVIDENCIA_AMBIENTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: AUTOR''
	 AND parvAux.IdTestData = tdt.Id) AS AUTOR,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE ENTRADA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_ENTRADA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: DADOS DE SAIDA''
	 AND parvAux.IdTestData = tdt.Id) AS DADOS_DE_SAIDA,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: FASE''
	 AND parvAux.IdTestData = tdt.Id) AS FASE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NOME DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NOME_DO_CASO_DE_TESTE,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: TITULO''
	 AND parvAux.IdTestData = tdt.Id) AS TITULO,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: PRJ''
	 AND parvAux.IdTestData = tdt.Id) AS PRJ,

  (SELECT parvAux.valor
   FROM ParametroValor parvAux
   INNER JOIN ParametroScript parsAux ON parvAux.IdParametroScript = parsAux.Id
   INNER JOIN Parametro parAux ON parsAux.IdParametro = parAux.Id
   WHERE parAux.Descricao = ''EVIDENCIA: NUMERO DO CASO DE TESTE''
	 AND parvAux.IdTestData = tdt.Id) AS NUMERO_DO_CASO_DE_TESTE
FROM TestData tdt
INNER JOIN DataPool dtp ON tdt.IdDataPool = dtp.Id
LEFT JOIN Script_CondicaoScript scrcon ON dtp.IdScript_CondicaoScript = scrcon.Id
LEFT JOIN Script_CondicaoScript_Ambiente scrconamb ON scrconamb.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
INNER JOIN ParametroScript parscr ON parscr.IdScript_CondicaoScript = scrcon.Id
INNER JOIN Parametro par ON parscr.IdParametro = par.Id
INNER JOIN AUT aut ON dtp.IdAut = aut.Id
LEFT JOIN AmbienteExecucao ambexec ON scrconamb.IdAmbienteExecucao = ambexec.Id
INNER JOIN AmbienteVirtual ambvir ON scrconamb.IdAmbienteVirtual = ambvir.Id
LEFT JOIN Demanda dem ON dtp.IdDemanda = dem.Id
INNER JOIN Usuario us ON tdt.IdUsuario = us.Id
WHERE tdt.Id IN (ptdTosca)
ORDER BY tdt.id'  FROM Script scr WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE');


END