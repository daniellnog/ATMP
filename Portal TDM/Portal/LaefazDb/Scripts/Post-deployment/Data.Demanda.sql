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

-- Cadastro de Demanda
IF NOT EXISTS (SELECT * FROM Demanda)
BEGIN
INSERT INTO DEMANDA(Descricao, Complemento)VALUES('PRJ00017516_ENT00006232', 'CR URA');
INSERT INTO DEMANDA(Descricao, Complemento)VALUES('PRJ00021768_ENT00008364', 'Adequação de portfólio da Móvel-Varejo–Móvel-F3');
END