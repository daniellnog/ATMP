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

-- Cadastro de Parametro
IF NOT EXISTS (SELECT * FROM Parametro)
BEGIN
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('CPF', 'NUMBER', 'CPF');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('DDD', 'NUMBER', 'DDD');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('CONTA FATURA', 'STRING', 'CONTA_FATURA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('ENDERECO', 'STRING', 'ENDERECO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('SENHA GERENTE', 'STRING', 'SENHA_GERENTE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('LOCALIDADE', 'STRING', 'LOCALIDADE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('LOCAL', 'STRING', 'LOCAL');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('LOGIN VENDEDOR', 'STRING', 'LOGIN_VENDEDOR');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TIPO DE USO', 'STRING', 'TIPO_DE_USO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('CAMPANHA', 'STRING', 'CAMPANHA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('URL', 'STRING', 'URL');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('USUARIO SISTEMA', 'STRING', 'USUARIO_SISTEMA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('SENHA SISTEMA', 'STRING', 'SENHA_SISTEMA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('AMBIENTE SISTEMA', 'STRING', 'AMBIENTE_SISTEMA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('APLICACAO', 'STRING', 'APLICACAO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TRILHA', 'STRING', 'TRILHA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TIPO PESSOA', 'STRING', 'TIPO_PESSOA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('PDV', 'STRING', 'PDV');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('DESCRICAO PEDIDO', 'STRING', 'DESCRICAO_PEDIDO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NOME PRODUTO', 'STRING', 'NOME_PRODUTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NATUREZA PRODUTO', 'STRING', 'NATUREZA_PRODUTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('MIDIA', 'STRING', 'MIDIA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('DIA VENCIMENTO', 'STRING', 'DIA_VENCIMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NUMERO', 'STRING', 'NUMERO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('USUARIO', 'STRING', 'USUARIO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('SENHA', 'STRING', 'SENHA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TEL CONTATO', 'STRING', 'TEL_CONTATO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NOME CONTATO', 'STRING', 'NOME_CONTATO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TIPO ID', 'STRING', 'TIPO_ID');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('DATA ENTREGA', 'STRING', 'DATA_ENTREGA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('QTD PONTOS TV', 'STRING', 'QTD_PONTOS_TV');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('N IDENTIDADE', 'STRING', 'N_IDENTIDADE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('ESTADO', 'STRING', 'ESTADO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('CEP', 'STRING', 'CEP');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TIPO COMPLEMENTO', 'STRING','TIPO_COMPLEMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('N COMPLEMENTO', 'STRING', 'N_COMPLEMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NOME CLIENTE', 'STRING', 'NOME_CLIENTE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NOME MAE', 'STRING', 'NOME_MAE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NOME SOLICITANTE', 'STRING', 'NOME_SOLICITANTE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('SEXO PESSOA', 'STRING', 'SEXO_PESSOA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('SEXO', 'STRING', 'SEXO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('CLASSIFICACAO', 'STRING', 'CLASSIFICACAO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('DIA NASCIMENTO', 'STRING', 'DIA_NASCIMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('MES NASCIMENTO', 'STRING', 'MES_NASCIMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('ANO NASCIMENTO', 'STRING', 'ANO_NASCIMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('DATA NASCIMENTO', 'STRING', 'DATA_NASCIMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NUMERO RESIDENCIA', 'STRING', 'NUMERO_RESIDENCIA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('TIPO RESIDENCIA', 'STRING', 'TIPO_RESIDENCIA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('PONTO REFERENCIA', 'STRING', 'PONTO_REFERENCIA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('NUMERO COMPLEMENTO', 'STRING', 'NUMERO_COMPLEMENTO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('PLANO CONVERGENTE', 'STRING', 'PLANO_CONVERGENTE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: RESULTADO ESPERADO', 'STRING', 'RESULTADO_ESPERADO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: AMBIENTE', 'STRING', 'AMBIENTE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: AUTOR', 'STRING', 'AUTOR');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: DADOS DE ENTRADA', 'STRING', 'DADOS_DE_ENTRADA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: DADOS DE SAIDA', 'STRING', 'DADOS_DE_SAIDA');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: FASE', 'STRING', 'FASE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: NOME DO CASO DE TESTE', 'STRING', 'NOME_DO_CASO_DE_TESTE');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: TITULO', 'STRING', 'TITULO');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: PRJ', 'STRING', 'PRJ');
INSERT INTO PARAMETRO(Descricao, Tipo, ColunaTecnicaTosca)VALUES('EVIDENCIA: NUMERO DO CASO DE TESTE', 'STRING', 'NUMERO_DO_CASO_DE_TESTE');
END