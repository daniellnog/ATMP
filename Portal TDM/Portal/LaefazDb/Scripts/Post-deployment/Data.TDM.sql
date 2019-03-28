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

-- Cadastro de TDM
IF NOT EXISTS (SELECT * FROM TDM)
BEGIN

INSERT INTO TDM (Descricao, TdmPublico) VALUES ('TDM PILOTO RELEASE MAIO', 0);
INSERT INTO TDM (Descricao, TdmPublico) VALUES ('TDM PÚBLICO', 1);

END