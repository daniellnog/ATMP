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

-- Cadastro de DataPool
IF NOT EXISTS (SELECT * FROM DataPool)
BEGIN
INSERT INTO DataPool (IdTDM, IdAut, IdDemanda, IdScript_CondicaoScript, Descricao, QtdSolicitada, Observacao, DataSolicitacao, DataInicioExecucao)
(SELECT (SELECT Id FROM TDM WHERE Descricao = 'TDM PILOTO RELEASE MAIO'), (SELECT Id FROM AUT WHERE Descricao = 'SAC'), 
(SELECT Id FROM Demanda WHERE Complemento = 'CR URA'), 
(SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'),
'DATAPOOL TESTE', 10, 'OBSERVAÇÃO PARA DATAPOOL TESTE', GETDATE(), GETDATE());

END