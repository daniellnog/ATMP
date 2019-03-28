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
INSERT INTO AUT(Descricao) VALUES ('SIEBEL 6.3');
INSERT INTO AUT(Descricao) VALUES ('STC');
INSERT INTO AUT(Descricao) VALUES ('SAC');
INSERT INTO AUT(Descricao) VALUES ('SIEBEL 8');
END