CREATE PROCEDURE PR_LISTAR_DATAPOOL_ENVIO_EMAIL
AS
BEGIN
    SELECT 
		 DataPool.Descricao 'DataPool',
		 DataPool.DataTermino 'DataTermino',
		 Demanda.Descricao 'DescricaoDemanda',
		 AUT.Descricao 'DescricaoSistema',
		 (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=1) 'QtdCadastrada',
		 DataPool.QtdSolicitada 'QtdSolicitada',
		 (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) 'QtdDisponivel',
		 (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=5) 'QtdReservada',
		 (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6) 'QtdUtilizada',
		 CASE
			 WHEN TDM.Id <> 0 and TDM.TdmPublico = 1 AND ((SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) > 0) THEN 'VERDE'
			 WHEN TDM.Id <> 0 and TDM.TdmPublico = 1 AND NOT ((SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) > 0) THEN 'CINZA'
			 WHEN NOT (TDM.Id <> 0 and TDM.TdmPublico = 1) AND ((SELECT DATEADD(day, 14, GETDATE())) < ((SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3))) OR (DataPool.DataTermino < GETDATE()) THEN 'CINZA'
			 WHEN NOT (TDM.Id <> 0 and TDM.TdmPublico = 1) AND (((SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) + (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6)) < DataPool.QtdSolicitada) THEN 'VERMELHO'
			 WHEN NOT (TDM.Id <> 0 and TDM.TdmPublico = 1) AND (((SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) + (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6)) > DataPool.QtdSolicitada) AND (((SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=3) + (SELECT CAST(COUNT(TestData.IdStatus) AS INT)FROM TestData WHERE TestData.IdDataPool = DataPool.Id AND TestData.IdStatus=6)) < DataPool.QtdSolicitada * 1.2) THEN 'AMARELO'
			 ELSE 'VERDE'
		 END 'Farol',
		 DataPool.DataSolicitacao 'DataSolicitacao',
		 TDM.Descricao 'DescricaoTDM',
		 ISNULL(Script.Descricao,'') + ' ' + ISNULL(CondicaoScript.Descricao,'') as 'DescricaoCondicaoScript'    
	FROM 
		 DataPool
		 LEFT JOIN Demanda ON Demanda.Id = DataPool.IdDemanda
		 LEFT JOIN AUT ON AUT.Id = DataPool.IdAut
		 LEFT JOIN Script_CondicaoScript ON Script_CondicaoScript.Id = DataPool.IdScript_CondicaoScript
		 LEFT JOIN Script ON Script.Id = Script_CondicaoScript.IdScript
		 LEFT JOIN CondicaoScript ON CondicaoScript.Id = Script_CondicaoScript.IdCondicaoScript
		 LEFT JOIN TDM ON TDM.Id = DataPool.IdTDM
		 LEFT JOIN TDM_Usuario ON TDM_Usuario.IdTDM = TDM.Id 
	WHERE (TDM_Usuario.IdUsuario = 2 and (DataPool.IdTDM = 12))
END