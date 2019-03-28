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

-- Cadastro de TDM_USUARIO
IF NOT EXISTS (SELECT * FROM TDM_Usuario)
BEGIN
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (1,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (2,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (3,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (4,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (5,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (6,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (7,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (8,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (9,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (10,1);
	INSERT INTO TDM_Usuario(IdUsuario, IdTDM) VALUES (11,1);
END