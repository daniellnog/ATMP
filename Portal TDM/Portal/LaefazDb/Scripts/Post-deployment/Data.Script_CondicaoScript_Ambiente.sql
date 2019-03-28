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
IF NOT EXISTS (SELECT * FROM Script_CondicaoScript_Ambiente)
BEGIN
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),2);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),2);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAÇÃO DE CONTATO E CONTA PF SEM ENVIO AO CDI'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),2);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAR ANÁLISE DE CRÉDITO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'CRIAR PERFIL DE FATURAMENTO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),2);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),2);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR CRIAÇÃO DE CONTATO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),2);


INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR INCLUSÃO DE ENDEREÇO FIBRA'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);


INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'INCLUSÃO DE CLIENTE PF NO STC'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);

INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR PEDIDO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 141'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR PEDIDO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 219'),1);
INSERT INTO Script_CondicaoScript_Ambiente (IdScript_CondicaoScript, IdAmbienteVirtual,IdAmbienteExecucao)
(SELECT (SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'REALIZAR PEDIDO'),
(SELECT Id FROM AmbienteVirtual WHERE Descricao = 'VDI 220'),1);


END