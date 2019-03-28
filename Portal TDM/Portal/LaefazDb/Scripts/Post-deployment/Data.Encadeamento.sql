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

-- Cadastro de AUT
IF NOT EXISTS (SELECT * FROM AUT)
BEGIN
INSERT INTO Encadeamento(Descricao) VALUES ('POC - Execução Encadeada 01');
INSERT INTO Encadeamento(Descricao) VALUES ('POC - Execução Encadeada 02');
END