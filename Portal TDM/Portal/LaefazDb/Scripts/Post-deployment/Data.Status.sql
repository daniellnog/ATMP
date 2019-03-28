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

-- Cadastro de Status
-- Prezados, como essa carga trata-se de uma tabela de configuração, não podem ser alterados os IDs dos status.
-- Caso as ordens dos itens sejam alteradas, irá impactar diretamente em várias regras de negócio do Sistema.
IF NOT EXISTS (SELECT * FROM Status)
BEGIN
INSERT INTO STATUS(Descricao)VALUES('CADASTRADA');
INSERT INTO STATUS(Descricao)VALUES('EM GERAÇÃO');
INSERT INTO STATUS(Descricao)VALUES('DISPONÍVEL');
INSERT INTO STATUS(Descricao)VALUES('ERRO');
INSERT INTO STATUS(Descricao)VALUES('RESERVADA');
INSERT INTO STATUS(Descricao)VALUES('UTILIZADA');
END
