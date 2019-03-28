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


IF NOT EXISTS (SELECT * FROM TipoDadoParametro)
BEGIN
INSERT INTO TipoDadoParametro(Descricao) VALUES ('STRING');
INSERT INTO TipoDadoParametro(Descricao) VALUES ('NUMBER');
INSERT INTO TipoDadoParametro(Descricao) VALUES ('DATE');
INSERT INTO TipoDadoParametro(Descricao) VALUES ('BOLEAN');
END