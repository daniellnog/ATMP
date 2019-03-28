IF NOT EXISTS (SELECT * FROM TestData)
BEGIN
INSERT INTO TestData (IdExecucao, IdDataPool, IdStatus, IdUsuario, IdScript_CondicaoScript, Descricao, GerarMigracao, CasoTesteRelativo, Observacao, InicioExecucao)
(SELECT null, (SELECT Id FROM DataPool WHERE Descricao = 'DATAPOOL TESTE'), 
(SELECT Id FROM Status WHERE Descricao='CADASTRADA'), (SELECT Id FROM Usuario WHERE Login='walter'), 
(SELECT scrcon.Id FROM Script_CondicaoScript scrcon LEFT JOIN Script scr ON scrcon.IdScript = scr.Id WHERE scr.Descricao = 'AJA - CADASTRA - ALTERAR CLIENTE'),
'AJA - CADASTRA - ALTERAR CLIENTE', 0, '01.01', 'OBSERVAÇÃO PARA GERAÇÃO DE MASSA TESTE', GETDATE());

END




















