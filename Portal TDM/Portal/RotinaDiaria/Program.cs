using LaefazWeb.Controllers;
using LaefazWeb.Enumerators;
using LaefazWeb.Extensions;
using LaefazWeb.Models;
using LaefazWeb.Models.VOs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RotinaDiaria
{
    class Program
    {
        private static LogTDM Log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            try
            {

                Log.Info("Iniciando Rotina Diária");

                DbEntities db = new DbEntities();

                List<DataPoolVO> DataPoolVOs = new List<DataPoolVO>();

                SqlParameter[] param =
                    {
                    new SqlParameter("@DisplayLength", 50),
                    new SqlParameter("@DisplayStart", "0"),
                    new SqlParameter("@SortCol", 7),
                    new SqlParameter("@SortDir", "desc"),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos", 1),
                    new SqlParameter("@IdTDM", DBNull.Value)
                };

                Log.Info("Executando procedure:  PR_LISTAR_DATAPOOL_SEM_USUARIO");
                Log.Info("Parametros:");
                Log.Info("@DisplayLength =  50");
                Log.Info("@DisplayStart =  0");
                Log.Info("@SortCol =  7");
                Log.Info("@SortDir =  desc");
                Log.Info("@SEARCH =  null");
                Log.Info("@ListarTodos =  1");
                Log.Info("@IdTDM =  null");

                DataPoolVOs = db.Database.SqlQuery<DataPoolVO>(
                        "EXEC PR_LISTAR_DATAPOOL_SEM_USUARIO @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @ListarTodos, @IdTDM ", param).ToList();

                Log.Info("Quantidade de registros retornados: " + DataPoolVOs.Count());

                List<int> idsDatapoolAmarelo = new List<int>();

                foreach (DataPoolVO dtemp in DataPoolVOs)
                {
                    if ((dtemp.QtdDisponivel + dtemp.QtdUtilizada > dtemp.QtdSolicitada) && (dtemp.QtdDisponivel + dtemp.QtdUtilizada < dtemp.QtdSolicitada * 1.2))
                    {
                        if (!idsDatapoolAmarelo.Contains(dtemp.Id))
                        {
                            idsDatapoolAmarelo.Add(dtemp.Id);
                        }

                    }
                }

                Log.Info("Quantidade de datapool com o farol em amarelo: " + idsDatapoolAmarelo.Count());

                int IdScript_CondicaoScript = 0;

                for (int i = 0; i < idsDatapoolAmarelo.Count(); i++)
                {
                    int idTemp = idsDatapoolAmarelo[i];
                    List<TestData> listaTDs = db.TestData.Where(x => x.IdDataPool == idTemp).Where(x => x.IdStatus == (int)EnumStatusTestData.Cadastrada).ToList();

                    Log.Info("Quantidade " + listaTDs.Count() + " de testdata para o datapool " + idTemp);

                    StringBuilder idsTestData = new StringBuilder();
                    for (int w = 0; w < listaTDs.Count(); w++)
                    {


                        //TestData tdTemp = db.TestData.Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript).Where(x => x.IdStatus == (int)EnumStatusTestData.Disponivel || x.IdStatus == (int)EnumStatusTestData.Utilizada).Where(x=>x.IdExecucao != null).OrderByDescending(x => x.Id).FirstOrDefault();
                        //Execucao ExecTemp = db.Execucao.Where(x => x.Id == tdTemp.IdExecucao).FirstOrDefault();
                        //Script_CondicaoScript_Ambiente scsa = db.Script_CondicaoScript_Ambiente.Where(x=>x.Id == ExecTemp.IdScript_CondicaoScript_Ambiente).FirstOrDefault();


                        ////DateTime d = DateTime.Now;
                        ////int dia = d.Day + 1;
                        ////string date = d.Year + "-" + d.Month + "-" + dia + "T00:00";


                        //string idFaseTeste = ExecTemp.IdTipoFaseTeste.ToString();
                        //string idAmbVirt = scsa.IdAmbienteVirtual.ToString();
                        //string idAmbExec = scsa.IdAmbienteExecucao.ToString();

                        ////ag.ManterAgendamento("0", date, idTd, idFaseTeste, idAmbVirt, "1", idAmbExec, true);


                        string idTd = listaTDs[w].Id.ToString();

                        if (w < (listaTDs.Count() - 1))
                        {
                            idsTestData.Append(idTd + ",");
                        }
                        else
                        {
                            idsTestData.Append(idTd);
                        }
                    }


                    IdScript_CondicaoScript = listaTDs[0].IdScript_CondicaoScript;

                    Execucao ultimaExec = (from e in db.Execucao
                                           join scsa in db.Script_CondicaoScript_Ambiente on e.IdScript_CondicaoScript_Ambiente equals scsa.Id
                                           join scs in db.Script_CondicaoScript on scsa.Id equals scs.Id
                                           join td in db.TestData on e.IdTestData equals td.Id
                                           where e.IdStatusExecucao == 4 && td.IdScript_CondicaoScript == IdScript_CondicaoScript
                                           orderby e.Id descending
                                           select e
                                               ).FirstOrDefault();


                    if (ultimaExec != null)
                    {
                        Script_CondicaoScript_Ambiente sca = db.Script_CondicaoScript_Ambiente.FirstOrDefault(x => x.Id == ultimaExec.IdScript_CondicaoScript_Ambiente);

                        Log.Info("Foi encontrada a execucao de ID: " + ultimaExec.Id);

                        DateTime d = DateTime.Now;
                        int dia = d.Day + 1;
                        string date = d.Year + "-" + d.Month + "-" + dia + "T00:10";

                        AgendamentoController ag = new AgendamentoController(); //2018-11-28T22:00 

                            ag.ManterAgendamento("0", date, idsTestData.ToString(), ultimaExec.IdTipoFaseTeste.ToString(), sca.IdAmbienteVirtual.ToString(), true.ToString(), sca.IdAmbienteExecucao.ToString(), true);

                        Log.Info("Foi criado agendamento com sucesso para a data  " + date);

                    }
                    else
                    {
                        Log.Info("Não foi encontrado nenhum historico de execução para o IdScript_CondicaoScript: " + IdScript_CondicaoScript + " como o status de sucesso.");
                    }


                }
            }
            catch (Exception e)
            {
                Log.Error("Erro ao executar a rotina diaria: " + e.Message);
                Console.Write(e.Message);
                Console.Read();
            }
        }
    }
}
