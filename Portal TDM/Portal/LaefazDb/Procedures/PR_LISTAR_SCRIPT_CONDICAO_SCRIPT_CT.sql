CREATE PROCEDURE PR_LISTAR_SCRIPT_CONDICAO_SCRIPT_CT 
  AS 
  BEGIN 
      SELECT DISTINCT
			scsCt.Id,
			scsCt.IdCT,
			scsCt.IdScript_CondicaoScript,
			ae.Id IdAmbienteExecucao,
			ct.Fase FaseCT,
			ae.Descricao DescricaoAmbienteExecucao,
			scs.NomeTecnicoScript NomeTecnicoScript,
			ct.Nome DescricaoCT,
			s.Descricao DescricaoScript,
			(
				SELECT 
					TOP 1 ex.Id 
				FROM 
					Execucao ex 
					INNER JOIN TestData td ON ex.Id = td.IdExecucao
					INNER JOIN TestData_CT tdCT ON td.Id = tdCT.IdTestData
				WHERE 
					tdCT.IdCT = ct.Id
				ORDER BY 
					ex.TerminoExecucao DESC
			) IdStatusUltimaExecucao,
			ISNULL
			(
				(
					SELECT 
						TOP 1 se.Descricao
					FROM 
						Execucao ex 
						INNER JOIN TestData td ON ex.Id = td.IdExecucao
						INNER JOIN TestData_CT tdCT ON td.Id = tdCT.IdTestData
						INNER JOIN StatusExecucao se ON ex.IdStatusExecucao = se.Id
					WHERE 
						tdCT.IdCT = ct.Id
					ORDER BY 
						ex.TerminoExecucao DESC
				)
			, 'Nunca Executado') DescricaoStatusUltimaExecucao,
			scs.IdPlataforma,
			p.Descricao DescricaoPlataforma,
			aut.Id IdAut,
			aut.Descricao DescricaoAut
      FROM			
						Script_CondicaoScript_CT scsCt 
             INNER JOIN CT ct ON scsCt.IdCT = ct.Id
             INNER JOIN Script_CondicaoScript scs ON scsCt.IdScript_CondicaoScript = scs.Id
             INNER JOIN Script s ON scs.IdScript = s.Id
             INNER JOIN DeParaAtmpAlmFase dpFase ON ct.Fase = dpFase.Fase
             INNER JOIN AmbienteExecucao ae ON dpFase.IdAmbienteExecucao = ae.Id
             INNER JOIN Plataforma p ON scs.IdPlataforma = p.Id
             INNER JOIN AUT aut ON s.IdAUT = aut.Id
  END