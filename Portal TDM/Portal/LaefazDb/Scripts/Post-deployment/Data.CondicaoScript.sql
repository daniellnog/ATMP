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

-- Cadastro de Condicao Script
IF NOT EXISTS (SELECT * FROM CondicaoScript)
BEGIN
	INSERT INTO CondicaoScript (Descricao) VALUES ('SEM ENVIO AO CDI');
	INSERT INTO CondicaoScript (Descricao) VALUES ('ATÉ O PONTO DE ENVIO DA ORDEM');
	INSERT INTO CondicaoScript (Descricao) VALUES ('SEM PRODUTO DE TV');
	INSERT INTO CondicaoScript (Descricao) VALUES ('SEM DEPENDENTE');
	INSERT INTO CondicaoScript (Descricao) VALUES ('COM DEPEMDEMTE');
	INSERT INTO CondicaoScript (Descricao) VALUES ('SIMPLES');
	INSERT INTO CondicaoScript (Descricao) VALUES ('COMBO');
	INSERT INTO CondicaoScript (Descricao) VALUES ('COM PRODUTO DE TV');
	INSERT INTO CondicaoScript (Descricao) VALUES ('COM ENVIO AO CDI');
	INSERT INTO CondicaoScript (Descricao) VALUES ('GERENTE TELECON');
	INSERT INTO CondicaoScript (Descricao) VALUES ('COM CAMPANHA');
	INSERT INTO CondicaoScript (Descricao) VALUES ('OFC');
END