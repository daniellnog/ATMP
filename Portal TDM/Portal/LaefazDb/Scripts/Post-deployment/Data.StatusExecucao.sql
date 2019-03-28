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

IF NOT EXISTS (SELECT * FROM StatusExecucao)
BEGIN
	INSERT INTO StatusExecucao (Descricao) VALUES ('Aguardando Processamento');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Em Processamento');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Processando Log Tosca');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Sucesso');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Falha');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Cancelamento');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Em Cancelamento');
	INSERT INTO StatusExecucao (Descricao) VALUES ('Agendada');
END