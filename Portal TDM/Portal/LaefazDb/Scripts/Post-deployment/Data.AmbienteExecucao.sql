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

-- Cadastro de Ambiente Execução 'TI8'
IF NOT EXISTS (SELECT * FROM AmbienteExecucao WHERE Descricao = 'TI8')
BEGIN
	INSERT INTO AmbienteExecucao (Descricao) VALUES ('TI8');
END

-- Cadastro de Ambiente Execução 'TI1'
IF NOT EXISTS (SELECT * FROM AmbienteExecucao WHERE Descricao = 'TI1')
BEGIN
	INSERT INTO AmbienteExecucao (Descricao) VALUES ('TI1');
END