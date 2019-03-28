using LaefazWeb.Enumerators;
using LaefazWeb.Extensions;
using LaefazWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace Agendamento
{
    class Program
    {
        private static LogTDM Log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            #region Mock Para Simular Execução Agendada
#if DEBUG
            #region Debug
            Log.Info("Foi iniciado o .BAT de Agendamento no modo de DEBUG...");
            #endregion

            args = new string[1];

            args[0] = "idAgendamento:7;idFaseTeste:1;idMaquinaVirtual:1;enviarTelegram:True;idAmbienteExecucao:1;idUsuario:2";

#endif
            #endregion

            #region Debug
            //Verifica se foi passado o ID da Execução
            Log.Info("Iniciando execucao do Metodo Main.");
            #endregion
            if (args.Count() > 0)
            {
                #region Debug
                Log.Info("Validacao da quantidade de argumentos passadas no Main atendida com sucesso.");
                Log.Info("Foram recebidos " + args.Count() + " argumento(s) no args.");
                Log.Info("Argumento(s) recebido(s): " + args[0].ToString());
                #endregion

                #region DbEntities

                #region Debug
                Log.Info("Iniciando coneccao com o DbEntities...");
                #endregion

                DbEntities db = new DbEntities();

                #region Debug
                Log.Info("DbEntities instanciado com sucesso...");
                #endregion

                #endregion

                #region  Pegando os Parâmetros

                #region Debug
                Log.Info("Iniciando tratamento dos parametro(s) recebido(s)...");
                #endregion

                Dictionary<string, string> parameter = new Dictionary<string, string>();

                foreach (string param in args[0].Split(';'))
                {
                    string key = param.Split(':')[0];
                    string value = param.Split(':')[1];

                    if (key.Equals("idAgendamento"))
                    {
                        int IdAgendamentp = Int32.Parse(value);
                        List<Agendamento_TestData> agTd = db.Agendamento_TestData.Where(x => x.IdAgendamento == IdAgendamentp).ToList();

                        key = "idTestData";
                        value = "";

                        foreach (Agendamento_TestData ag in agTd)
                        {

                            if (ag.Equals(agTd.Last()))
                                value += ag.IdTestData;
                            else
                                value += ag.IdTestData + ",";
                        }

                    }



                    #region Debug
                    Log.Info("Key: " + key + ";" + " Value: " + value);
                    #endregion

                    parameter.Add(key, value);
                }

                #region Debug
                Log.Info("Parametros tratados com sucesso!");
                Log.Info("Iniciando carregamento do Usuario...");
                #endregion

                int idUsuario = Int32.Parse(parameter["idUsuario"]);
                Usuario user = db.Usuario.FirstOrDefault(x => x.Id == idUsuario);

                #region Debug
                Log.Info("Usuario carregado com sucesso...");
                #endregion


                #endregion

                #region Validando se o Ambiente Está Disponível para o Agendamento

                int idTestData = Int32.Parse(parameter["idTestData"].Split(',')[0]);
                TestData td = db.TestData.FirstOrDefault(x => x.Id == idTestData);

                #region DebugObject
                Log.DebugObject(td);
                #endregion

                int IdAmbienteVirtual = Int32.Parse(parameter["idMaquinaVirtual"]);

                Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.FirstOrDefault(x => x.Id == td.DataPool.IdScript_CondicaoScript);

                #region DebugObject
                Log.DebugObject(script_CondicaoScript);
                #endregion

                #region Query de Ambientes

                Execucao execucaoParaAmbVirtual =
                        (from exec in db.Execucao
                         join sca in db.Script_CondicaoScript_Ambiente on exec.IdScript_CondicaoScript_Ambiente equals sca.Id
                         join av in db.AmbienteVirtual on sca.IdAmbienteVirtual equals av.Id
                         where exec.SituacaoAmbiente == (int)LaefazWeb.Enumerators.EnumSituacaoAmbiente.EmUso
                         && av.Id == IdAmbienteVirtual
                         select exec).FirstOrDefault();

                #region DebugObject
                Log.DebugObject(execucaoParaAmbVirtual);
                #endregion

                #endregion

                #endregion

                #region Função Play

                #region Debug
                Log.Info("Chamada do método Play()...");
                Log.Info(parameter["idTestData"] + "," + parameter["idFaseTeste"] + "," + parameter["idMaquinaVirtual"] + "," + Convert.ToBoolean(parameter["enviarTelegram"]) + "," + parameter["idAmbienteExecucao"] + "," + user);

                #endregion

                if (execucaoParaAmbVirtual == null)
                {
                    #region Debug
                    Log.Info("Ambiente disponivel para execucao do agendamento.");
                    #endregion
                    Play(parameter["idTestData"], parameter["idFaseTeste"], parameter["idMaquinaVirtual"], Convert.ToBoolean(parameter["enviarTelegram"]), parameter["idAmbienteExecucao"], user);

                    //#region Excluir Job Jenkins
                    //RemoveJobJenkins(parameter["idTestData"]);
                    //#endregion
                }
                else
                {

                    AmbienteVirtual av = db.AmbienteVirtual.FirstOrDefault(x => x.Id == IdAmbienteVirtual);
                    #region DebugObject
                    Log.DebugObject(av);
                    #endregion

                    #region Exception Error
                    Log.Error("Não foi possível iniciar a execução pois o ambiente (VDI " + av.IP + ") não está Disponível para Execucao no momento.");
                    Log.Error("Detalhes da Execucao em Andamento...");
                    Log.Error("Id Execucao: " + execucaoParaAmbVirtual.Id);
                    Log.Error("Usuario: " + execucaoParaAmbVirtual.Usuario);
                    Log.Error("Script: " + execucaoParaAmbVirtual.Script_CondicaoScript_Ambiente.Script_CondicaoScript.NomeTecnicoScript);
                    Log.Error("Inicio Execucão: " + execucaoParaAmbVirtual.InicioExecucao);
                    throw new Exception("Não foi possível iniciar a execução pois o ambiente (VDI " + av.IP + ") não está Disponível para Execucao no momento.");
                    #endregion
                }

                #endregion
            }
            else
            {
                #region Exception Error
                Log.Error("Foi realizada uma chamada do .BAT de Agendamento, mas não foram passados os parâmetros conforme esperado.");
                throw new Exception("Foi realizada uma chamada do .BAT de Agendamento, mas não foram passados os parâmetros conforme esperado.");
                #endregion
            }
        }
        public static void Play(string id, string idFaseTeste, string idMaquinaVirtual, bool opcaoTelegram, string idAmbienteExecucao, Usuario user, bool PlayTestData = true)
        {
            #region Conecção DbEntities
            DbEntities db = new DbEntities();
            #endregion

            try
            {
                #region Debug
                Log.Info("Entrada no metodo Play().");
                #endregion

                string mensagem = "";

                var testDatas = new List<int>();
                List<ListaTestDatas> listaTestDatas = new List<ListaTestDatas>();
                int IdDatapool;
                int[] ids = null;

                #region Debug
                //Verifica se o Play é da entidade DataPool ou da TestData
                #endregion

                #region Play Por TestData
                if (PlayTestData)
                {

                    #region Debug
                    Log.Info("Foi identificado o tipo de Play por TestData.");
                    #endregion

                    Char delimiter = ',';
                    ids = id.Split(delimiter).Select(n => Convert.ToInt32(n)).ToArray();
                    int idTemp = ids[0];
                    TestData TestData = db.TestData.FirstOrDefault(x => x.Id == idTemp);
                    IdDatapool = db.DataPool.FirstOrDefault(x => x.Id == TestData.IdDataPool).Id;

                    #region Debug
                    Log.Info("Iniciando carregamento de informações dos TestData(s).");
                    #endregion

                    listaTestDatas =
                        (from dp in db.DataPool
                         join td in db.TestData on dp.Id equals td.IdDataPool
                         where ids.Contains(td.Id)
                         select new ListaTestDatas
                         {
                             IdDatapool = dp.Id,
                             IdTestData = td.Id,
                             IdStatus = td.IdStatus
                         }).ToList();

                    if (listaTestDatas.Where(x => x.IdStatus != (int)EnumStatusTestData.Cadastrada).ToList().Count() > 0)
                    {
                        #region Debug
                        Log.Info("Não é possível iniciar a execução de massas com o status diferente de CADASTRADA!");
                        #endregion
                        throw new Exception("Não é possível iniciar a execução de massas com o status diferente de CADASTRADA!");
                    }

                }
                #endregion

                #region Play Por DataPool
                else
                {

                    IdDatapool = Int32.Parse(id);
                    listaTestDatas =
                        (from dp in db.DataPool
                         join td in db.TestData on dp.Id equals td.IdDataPool
                         where dp.Id == IdDatapool
                         select new ListaTestDatas
                         {
                             IdDatapool = dp.Id,
                             IdTestData = td.Id
                         }).ToList();
                }
                #endregion

                #region Busca TestData Com Parâmetros Obrigatótios
                for (int i = 0; i < listaTestDatas.Count; i++)
                {
                    int idTestDataAtual = listaTestDatas[i].IdTestData;
                    List<ParametrosValores> listaParametrosObrigatorios =
                    (from pv in db.ParametroValor
                     join ps in db.ParametroScript on pv.IdParametroScript equals ps.Id
                     join p in db.Parametro on ps.IdParametro equals p.Id
                     where pv.IdTestData == idTestDataAtual && ps.Obrigatorio == true
                     select new ParametrosValores
                     {
                         IdTestData = IdDatapool,
                         IdDatapool = idTestDataAtual,
                         IdParametro = p.Id,
                         IdParametroValor = pv.Id,
                         Descricao = p.Descricao,
                         Valor = pv.Valor,
                         Obrigatorio = ps.Obrigatorio
                     }).ToList();

                    for (int w = 0; w < listaParametrosObrigatorios.Count; w++)
                    {
                        //Verifico se o Script tem o parametro Ambiente sistema, caso tenha, o valor do parametro é atualizado com o valor que vem da tela do play
                        if (listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Ambiente_Sistema || listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Ambiente)
                        {

                            int idAmbExec = Int32.Parse(idAmbienteExecucao);
                            int? _idParamValor = listaParametrosObrigatorios[w].IdParametroValor;

                            if (_idParamValor != null)
                            {
                                ParametroValor pv = db.ParametroValor.Where(x => x.Id == _idParamValor).FirstOrDefault();
                                AmbienteExecucao ambExec = db.AmbienteExecucao.Where(x => x.Id == idAmbExec).FirstOrDefault();
                                if (ambExec.Id == (int)EnumAmbienteExec.Ti1_Siebel8 || ambExec.Id == (int)EnumAmbienteExec.Ti8_Siebel8)
                                {
                                    string amb = ambExec.Descricao.Substring(ambExec.Descricao.IndexOf("http"), ambExec.Descricao.Length - ambExec.Descricao.IndexOf("http"));

                                    pv.Valor = amb;
                                    listaParametrosObrigatorios[w].Valor = amb;

                                }
                                else
                                {
                                    pv.Valor = ambExec.Descricao;
                                    listaParametrosObrigatorios[w].Valor = ambExec.Descricao;
                                }
                                // anexar objeto ao contexto
                                db.ParametroValor.Attach(pv);
                                //Prepara a entidade para uma Edição
                                db.Entry(pv).State = System.Data.Entity.EntityState.Modified;

                                // informa que o obejto será modificado
                                db.SaveChanges();
                            }
                        }

                        if (listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Dados_De_Entrada ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Dados_De_Saida ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Fase ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Nome_Do_Caso_De_Teste ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Numero_Do_Caso_De_Teste ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Prj ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Resultado_Esperado ||
                            listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Titulo)
                        {



                            int? _idParamValor = listaParametrosObrigatorios[w].IdParametroValor;
                            ParametroValor pv = db.ParametroValor.Where(x => x.Id == _idParamValor).FirstOrDefault();


                            if (!pv.Valor.Equals(""))
                                pv.Valor = "TESTE";

                            // anexar objeto ao contexto
                            db.ParametroValor.Attach(pv);
                            //Prepara a entidade para uma Edição
                            db.Entry(pv).State = System.Data.Entity.EntityState.Modified;

                            // informa que o obejto será modificado
                            db.SaveChanges();

                            listaParametrosObrigatorios[w].Valor = pv.Valor;
                        }



                        if (listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Autor)
                        {
                            int? _idParamValor = listaParametrosObrigatorios[w].IdParametroValor;
                            ParametroValor pv = db.ParametroValor.Where(x => x.Id == _idParamValor).FirstOrDefault();
                            pv.Valor = user.Login;

                            // anexar objeto ao contexto
                            db.ParametroValor.Attach(pv);
                            //Prepara a entidade para uma Edição
                            db.Entry(pv).State = System.Data.Entity.EntityState.Modified;

                            // informa que o obejto será modificado
                            db.SaveChanges();

                            listaParametrosObrigatorios[w].Valor = user.Login;
                        }
                        else
                        {
                            if (listaParametrosObrigatorios[w].Valor == "" || listaParametrosObrigatorios[w].Valor == null)
                            {
                                if (!testDatas.Contains(listaTestDatas[i].IdTestData))
                                {
                                    testDatas.Add(listaTestDatas[i].IdTestData);
                                }
                            }
                        }
                    }
                }

                #region Debug
                Log.Info("Ibusca de parametros obrigatorios nao preenchidos concluida.");
                #endregion

                #endregion

                #region Valida Se Possui Parâmetros Obrigatórios Não Preenchidos

                #region Debug
                Log.Info("Iniciando validacao de parametros obrigatorios nao preenchidos.");
                #endregion

                if (testDatas.Count > 0)
                {
                    string combindedString = string.Join(",", testDatas.ToArray());
                    string stringFinal = "";
                    for (int i = 0; i < combindedString.Length; i++)
                    {
                        if (i % 100 == 0)
                        {
                            if (i == 0)
                            {
                                stringFinal += combindedString[i];
                            }
                            else
                            {
                                stringFinal += combindedString[i] + "<br >";
                            }
                        }
                        else
                        {
                            stringFinal += combindedString[i];
                        }
                    }

                    #region Debug
                    Log.Info("O(s) TestData(s): abaixo possui(em) parâmetro(s) obrigatório(s) - destacados em vermelho - que não foram preenchidos <br>" + stringFinal);
                    #endregion

                    mensagem = "O(s) TestData(s): abaixo possui(em) parâmetro(s) obrigatório(s) - destacados em vermelho - que não foram preenchidos <br>" + stringFinal;
                }
                else
                {
                    bool EnvioTelegram = opcaoTelegram;
                    // Utilizando o Datapool da tela, substituir a query do script e salvar os dados na tabela de execução (Controle_Ambiente)

                    if (PlayTestData)
                        ReplaceQuery(ids.OfType<int>().ToList(), Int32.Parse(idFaseTeste), Int32.Parse(idMaquinaVirtual), Int32.Parse(idAmbienteExecucao), user, EnvioTelegram); // enviar o Datapool da tela
                    else
                        ReplaceQuery(ObtemIdTestData(Int32.Parse(id)), Int32.Parse(idFaseTeste), Int32.Parse(idMaquinaVirtual), Int32.Parse(idAmbienteExecucao), user, EnvioTelegram); // enviar o Datapool da tela

                    string pAginaDoJob = null;
                    int idAmbv = Int32.Parse(idMaquinaVirtual);
                    AmbienteVirtual ambv = db.AmbienteVirtual.Where(x => x.Id == idAmbv).FirstOrDefault();

                    if (ambv.IP != null)
                    {
                        mensagem = "Execução iniciada com sucesso para a VDI " + ambv.IP;

                        #region Debug
                        Log.Info("Execução iniciada com sucesso para a VDI " + ambv.IP);
                        #endregion

                        pAginaDoJob = ConfigurationSettings.AppSettings[ambv.IP];
                    }
                    else
                    {
                        #region Debug
                        Log.Info("Não foi possível definir o Job do Jenkins.");
                        #endregion

                        mensagem = "Não foi possível definir o Job do Jenkins.";
                    }

                    #region Debug
                    Log.Info("Chamada do metodo runJobJenkinsRemote().");
                    #endregion

                    runJobJenkinsRemote(pAginaDoJob);

                    #region Debug
                    Log.Info("Metodo de execucao do Jenkins finalizado.");
                    #endregion
                    //log.Info("Execução iniciada.");

                    //Usar esta opção para rodar local
                    //runJobJenkinsLocal(pAginaDoJob, "brucilin.de.gouveia", "brucilin.de.gouveia");
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region Log Error
                Log.Error("Ocorreu o seguinte erro no método Play(). " + ex.Message);
                Log.Error(ex.StackTrace);
                #endregion
            }

        }
        private static void ReplaceQuery(List<int> ids, int idFaseTeste, int idMaquinaVirtual, int idAmbienteExecucao, Usuario user, bool EnvioTelegram)
        {
            #region Entities
            DbEntities db = new DbEntities();
            #endregion

            TestData testData = db.TestData.Find(ids.First());

            Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();

            string query = script_CondicaoScript.QueryTosca;

            string testdataList = "";

            foreach (var item in ids)
            {
                #region Replace Query
                String queryTemp = query.Replace("ptdTosca", item.ToString());
                #endregion
                db = new DbEntities();

                #region Edição Entidade Execução
                //Execucao exec = new Execucao();
                Execucao exec = db.Execucao.FirstOrDefault(x => x.IdTestData == item);
                Script_CondicaoScript_Ambiente script_CondicaoScript_Ambiente = db.Script_CondicaoScript_Ambiente.Where(x => x.IdScript_CondicaoScript == testData.IdScript_CondicaoScript).Where(x => x.IdAmbienteVirtual == idMaquinaVirtual).Where(x => x.IdAmbienteExecucao == idAmbienteExecucao).FirstOrDefault();
                exec.IdScript_CondicaoScript_Ambiente = script_CondicaoScript_Ambiente.Id;
                //exec.IdTipoFaseTeste = idFaseTeste; // pegar via campo popup modal play
                exec.IdStatusExecucao = (int)EnumStatusExecucao.AguardandoProcessamento;
                exec.Usuario = user.Id.ToString();
                exec.IdTestData = item; // pegar o id via tela
                exec.SituacaoAmbiente = (int)EnumSituacaoAmbiente.EmUso;
                exec.ToscaInput = queryTemp;
                exec.EnvioTelegram = EnvioTelegram;
                // db.Execucao.Add(exec);
                db.Execucao.Attach(exec);
                db.Entry(exec).State = System.Data.Entity.EntityState.Modified;
                #endregion

                #region Edição Entidade TestData
                DbEntities db1 = new DbEntities();
                TestData td1 = db1.TestData.Where(x => x.Id == item).FirstOrDefault();
                td1.IdStatus = (int)EnumStatusTestData.EmGeracao;
                td1.GeradoPor = user.Login;
                db1.TestData.Attach(td1);

                db1.Entry(td1).State = System.Data.Entity.EntityState.Modified;
                #endregion

                try
                {
                    #region Salvar Entidades
                    db.SaveChanges();
                    db1.SaveChanges();
                    #endregion
                }
                catch (Exception ex)
                {
                    #region Log Error
                    Log.Error("Ocorreu o seguinte erro no método Play(). " + ex.Message);
                    Log.Error(ex.StackTrace);
                    #endregion
                }
            }
        }
        private static List<int> ObtemIdTestData(int id_datapool)
        {
            #region Debug
            // Instancia a Entity
            #endregion

            DbEntities db = new DbEntities();

            #region Debug
            // Query para buscar o id do TestData,passando o id do Datapool que será executado.
            #endregion

            var tdQuery =
            (from td1 in db.TestData
             join dtp in db.DataPool on td1.IdDataPool equals dtp.Id
             where dtp.Id == id_datapool && td1.IdStatus == 1
             select td1.Id).ToList();

            return tdQuery;
        }
        private static void runJobJenkinsRemote(string url)
        {
            #region Debug
            Log.Info("Entrada no metodo runJobJenkinsRemote().");
            Log.Info("Pagina a ser executada: " + url);
            #endregion

            WebRequest wrIniciaJob;

            wrIniciaJob = WebRequest.Create(url);

            wrIniciaJob.Method = "POST";

            WebResponse response = wrIniciaJob.GetResponse();

            response.Close();
        }
        private static void RemoveJobJenkins(string id)
        {

            try
            {
                #region Debug
                Log.Info("Entrada no método RemoveJobJenkins(" + id + ")");
                #endregion
                int idTestData = Int32.Parse(id);

                string url = ConfigurationSettings.AppSettings["UrlJenkinsJobAgendamento"] + "AGENDAMENTO-" + idTestData + "/doDelete";
                string user = ConfigurationSettings.AppSettings["UserJenkins"];
                string password = ConfigurationSettings.AppSettings["PasswordJenkins"];

                #region Debug
                Log.Info("Url Jenkins Job Agendamento: " + url);
                Log.Info("User Jenkins: " + user);
                Log.Info("Password Jenkins: " + password);
                #endregion

                //deleta job
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                #region Debug
                Log.Info("Carregado o obejto HttpWebRequest...");
                #endregion

                string mergedCredentials = string.Format("{0}:{1}", user, password);
                byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
                string base64Credentials = Convert.ToBase64String(byteCredentials);
                request.Headers.Add("Authorization", "Basic " + base64Credentials);
                request.Method = "POST";
                request.ContentType = "application/xml";

                string xml = "";

                byte[] byteArray = Encoding.UTF8.GetBytes(xml);
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                #region Debug
                Log.Info("Chamado da exclusão do Job realizada com sucesso...");
                #endregion

            }
            catch (Exception ex)
            {
                #region Log Error
                Log.Error("Ex: " + ex + Environment.NewLine
                    + "Message: " + ex.Message + Environment.NewLine
                    + "InnerException: " + ex.InnerException);
                #endregion
            }
        }

        #region Internal Classes
        internal class AmbienteExecucao_Popup
        {
            public string DescricaoAmbienteExecucao { get; set; }
            public string DescricaoAmbienteVirtual { get; set; }
            public bool Disponivel { get; set; }
            public int IdAmbienteExecucao { get; set; }
            public int IdAmbienteVirtual { get; set; }
        }
        internal class ListaTestDatas
        {
            public int IdDatapool { get; set; }
            public int IdStatus { get; internal set; }
            public int IdTestData { get; set; }
        }
        internal class ParametrosValores
        {
            public int IdDatapool { get; set; }
            public int IdTestData { get; set; }
            public int? IdParametro { get; set; }
            public int? IdParametroValor { get; set; }

            public bool Obrigatorio { get; set; }

            public string Descricao { get; set; }

            public string Valor { get; set; }
        }
        #endregion
    }
}
