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

-- Cadastro de Script
IF NOT EXISTS (SELECT * FROM Script)
BEGIN

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI' FROM AUT WHERE Descricao = 'SIEBEL 6.3');

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'CRIAR ANÁLISE DE CRÉDITO' FROM AUT WHERE Descricao = 'SIEBEL 8');

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'CRIAR PERFIL DE FATURAMENTO' FROM AUT WHERE Descricao = 'SIEBEL 8');

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'REALIZAR CRIAÇÃO DE CONTATO' FROM AUT WHERE Descricao = 'SIEBEL 8');

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA' FROM AUT WHERE Descricao = 'SIEBEL 8');

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'REALIZAR PEDIDO' FROM AUT WHERE Descricao = 'SIEBEL 8');

INSERT INTO Script (IdAUT, Descricao)
(SELECT Id, 'INCLUSÃO DE CLIENTE PF NO STC' FROM AUT WHERE Descricao = 'STC');
             
INSERT INTO Script (IdAUT, Descricao )
(SELECT Id, 'AJA - CADASTRA - ALTERAR CLIENTE' FROM AUT WHERE Descricao = 'SAC');

END