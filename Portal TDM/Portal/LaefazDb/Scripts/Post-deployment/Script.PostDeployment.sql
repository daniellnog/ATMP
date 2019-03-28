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


-- Carga Inicial


:r Data.AUT.sql
:r Data.Usuario.sql
:r Data.TipoFaseTeste.sql
:r Data.TipoParametro.sql
:r Data.Status.sql
:r Data.Script.sql
:r Data.Parametro.sql
:r Data.CondicaoScript.sql
:r Data.Demanda.sql
:r Data.Plataforma.sql
:r Data.Script_CondicaoScript.sql
:r Data.ParametroScript.sql
:r Data.AmbienteExecucao.sql
:r Data.AmbienteVirtual.sql
:r Data.TDM.sql
:r Data.DataPool.sql
:r Data.Script_CondicaoScript_Ambiente.sql
--:r Data.TestData.sql
--:r Data.ParametroValor.sql
:r Data.StatusExecucao.sql
:r Data.TDM_Usuario.sql
:r Data.TelaMapaCalor.sql
:r Data.TipoDadoParametro.sql
:r Data.Encadeamento.sql
--:r Data.Encadeamento_TestData.sql



