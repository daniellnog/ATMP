CREATE PROCEDURE PR_LISTAR_AMBIENTE_VIRTUAL_DISPONIVEL
	
AS
BEGIN
	 SELECT av.[Id],
		    av.[Descricao],
		    av.[IP] 
       FROM [TDM.Db].[dbo].[AmbienteVirtual] av 
      WHERE av.Id not in (SELECT sca.IdAmbienteVirtual
					        FROM [TDM.Db].[dbo].[Script_CondicaoScript_Ambiente] sca
				      INNER JOIN [TDM.Db].[dbo].[Execucao] execucao on sca.Id = execucao.IdScript_CondicaoScript_Ambiente
					       WHERE execucao.SituacaoAmbiente = 2 );
END