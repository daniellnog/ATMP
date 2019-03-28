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

-- Cadastro de Usuário Administrador
IF NOT EXISTS (SELECT * FROM Usuario)
BEGIN
INSERT INTO Usuario(Login, Senha)VALUES('administrador','senha');
INSERT INTO Usuario(Login, Senha)VALUES('walter','senha');
INSERT INTO Usuario(Login, Senha)VALUES('bruce','senha');
INSERT INTO Usuario(Login, Senha)VALUES('rapha','senha');
INSERT INTO Usuario(Login, Senha)VALUES('thiago','senha');
INSERT INTO Usuario(Login, Senha)VALUES('guga','senha');
INSERT INTO Usuario(Login, Senha)VALUES('daniel','senha');
INSERT INTO Usuario(Login, Senha)VALUES('leandro','senha');
INSERT INTO Usuario(Login, Senha)VALUES('gugaNobre','senha');
INSERT INTO Usuario(Login, Senha)VALUES('priscila.maria.silva','senha');
INSERT INTO Usuario(Login, Senha)VALUES('daniel.de.melo.moura','senha');
INSERT INTO Usuario(Login, Senha)VALUES('tiago.silvestre','senha');

END
