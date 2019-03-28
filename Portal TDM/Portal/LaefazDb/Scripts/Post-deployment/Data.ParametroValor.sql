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

-- Cadastro de Parametro
IF NOT EXISTS (SELECT * FROM ParametroValor)
BEGIN
	

INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'CPF' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), '43970395003');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'USUARIO SISTEMA' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'TR487923');

INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'SENHA SISTEMA' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'CFT66TFC');	

INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'APLICACAO' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), '17');		
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'TRILHA' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), '02');	

INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'TIPO PESSOA' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'F');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: RESULTADO ESPERADO' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'Tela validada com sucesso');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: AMBIENTE' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'TI8');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: AUTOR' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'LEANDRO');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: DADOS DE ENTRADA' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'DADOS DE ENTRADA TESTE DO SCRIPT DO PORTAL');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: DADOS DE SAIDA' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'DADOS DE SAIDA TESTE DO SCRIPT DO PORTAL');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: FASE' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'TI');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: NOME DO CASO DE TESTE' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'AJA - CADASTRA - ALTERAR CLIENTE');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: TITULO' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'TITULO DA EVIDENCIA AJA - CADASTRA - ALTERAR CLIENTE');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: PRJ' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), 'PRJ0000001_ENT000001');	
	
INSERT INTO ParametroValor (IdTestData, IdParametroScript, Valor)
(SELECT (SELECT Id FROM TestData WHERE Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'), 
(SELECT parscr.Id FROM ParametroScript parscr 
	LEFT JOIN Parametro par ON parscr.IdParametro = par.Id 
	LEFT JOIN Script_CondicaoScript scrcon ON parscr.IdScript_CondicaoScript = scrcon.Id
	LEFT JOIN Script scr ON scrcon.IdScript = scr.Id
	WHERE par.Descricao = 'EVIDENCIA: NUMERO DO CASO DE TESTE' AND scr.Descricao='AJA - CADASTRA - ALTERAR CLIENTE'), '01.01');	
	
	
END