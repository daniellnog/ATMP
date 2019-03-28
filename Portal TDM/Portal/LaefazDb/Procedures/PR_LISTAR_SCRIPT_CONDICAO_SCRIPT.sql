CREATE PROCEDURE PR_LISTAR_SCRIPT_CONDICAO_SCRIPT
  @ID_AMBIENTE_EXECUCAO NVARCHAR(255) = NULL,
  @ID_AUT NVARCHAR(255) = NULL
  AS 
  BEGIN 
      SELECT DISTINCT
			scs.Id,
			s.Descricao,
			cs.Descricao,
			a.Id IdAut
      FROM		
		    Script_CondicaoScript scs
			INNER JOIN Script s on scs.IdScript = s.Id
			INNER JOIN AUT a on s.IdAUT = a.Id
			LEFT JOIN Script_CondicaoScript_Ambiente scsa on scs.Id = scsa.IdScript_CondicaoScript
			LEFT JOIN CondicaoScript cs on scs.IdCondicaoScript = cs.Id
	  WHERE 
			(scsa.IdAmbienteExecucao = @ID_AMBIENTE_EXECUCAO OR @ID_AMBIENTE_EXECUCAO IS NULL) AND
			(a.Id = @ID_AUT OR @ID_AUT IS NULL)
  END