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

-- Cadastro de ParametroScript
IF NOT EXISTS (SELECT * FROM ParametroScript)
BEGIN

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1  
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'USUARIO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SENHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NOME CONTATO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'DDD'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'DATA NASCIMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SEXO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PDV'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CEP'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NUMERO RESIDENCIA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO RESIDENCIA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PONTO REFERENCIA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NUMERO COMPLEMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CPF'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PLANO CONVERGENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
		
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');	


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO RESIDENCIA'
AND tpar.Descricao = 'SAÍDA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PONTO REFERENCIA'
AND tpar.Descricao = 'SAÍDA');

---------------------------------------------------------------------------------------------------------


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'USUARIO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SENHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TRILHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'AMBIENTE SISTEMA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'LOCAL'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CPF'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO PESSOA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NOME CLIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NOME MAE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NOME SOLICITANTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SEXO PESSOA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'DIA NASCIMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'MES NASCIMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'ANO NASCIMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');


-----------------------------------------------------------------------------------------------------------


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CPF'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'USUARIO SISTEMA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SENHA SISTEMA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'APLICACAO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TRILHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO PESSOA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');	


---------------------------------------------------------------------------------------------------------------

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'USUARIO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SENHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'URL'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CPF'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PDV'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NOME PRODUTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');	


-----------------------------------------------------------------------------------------------------------


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NATUREZA PRODUTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'MIDIA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'DIA VENCIMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');	


----------------------------------------------------------------------------------------------------------


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'USUARIO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SENHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'URL'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TEL CONTATO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CPF'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO ID'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'N IDENTIDADE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');


---------------------------------------------------------------------------------------------------------


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NUMERO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'ESTADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CEP'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO COMPLEMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'N COMPLEMENTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');


-----------------------------------------------------------------------------------------------------------


INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'USUARIO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'SENHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'URL'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CPF'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PDV'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'LOGIN VENDEDOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'TIPO DE USO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CLASSIFICACAO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'CAMPANHA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'NOME PRODUTO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'DATA ENTREGA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'QTD PONTOS TV'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro, Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'PDV'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AMBIENTE'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: AUTOR'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: FASE'
AND tpar.Descricao = 'ENTRADA');
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');
	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: TITULO'
AND tpar.Descricao = 'ENTRADA');

INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: PRJ'
AND tpar.Descricao = 'ENTRADA');	
	
INSERT INTO ParametroScript (IdParametro, IdScript_CondicaoScript, IdTipoParametro,Obrigatorio) 
(SELECT par.Id,
(SELECT scrcon.Id 
FROM Script_CondicaoScript scrcon 
LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
LEFT JOIN CondicaoScript con ON scrcon.IdCondicaoScript = con.Id
WHERE scr.Descricao = 'REALIZAR PEDIDO'), tpar.Id, 1 
FROM Parametro par, TipoParametro tpar
WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE'
AND tpar.Descricao = 'ENTRADA');

END