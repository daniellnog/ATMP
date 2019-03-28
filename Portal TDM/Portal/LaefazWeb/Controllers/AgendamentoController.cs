using LaefazWeb.Extensions;
using LaefazWeb.Models;
using LaefazWeb.Models.VOs;
using Microsoft.Ajax.Utilities;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;
using TDMWeb.Extensions;
using TDMWeb.Lib;
using TDMWeb.Models.VOs;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class AgendamentoController : Controller
    {
        private DbEntities db = new DbEntities();
        LogTDM Log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string host = ConfigurationSettings.AppSettings["hostJenkins"];
        private string port = ConfigurationSettings.AppSettings["portJenkins"];

        public void CreateJobJenkins(string nomeUsuario, DateTime? dataAgendamento, int idAgendamento, int idDatapool, int idFaseTeste, int? idMaquinaVirtual, bool enviarTelegram, int? idAmbienteExecucao, int idUsuario, bool rotinaDiaria = false)
        {

            #region Debug
            Log.Info("Entrada no metodo de Criar Job no Jenkins.");
            #endregion

            //Execucao exec = db.Execucao.Where(x => x.Id == idExecucao).FirstOrDefault();
            //int? idTestData = exec.IdTestData;
            string argumentos = "idAgendamento:" + idAgendamento + ";" + "idFaseTeste:" + idFaseTeste + ";" + "idMaquinaVirtual:" + idMaquinaVirtual + ";" + "enviarTelegram:" + enviarTelegram + ";" + "idAmbienteExecucao:" + idAmbienteExecucao + ";" + "idUsuario:" + idUsuario;

            #region Debug
            Log.Info("argumentos: "+argumentos);
            #endregion
            //49 11 26 10 *
            //MINUTE	Minutes within the hour (0–59)
            //HOUR The hour of the day (0–23)
            //DOM The day of the month (1–31)
            //MONTH The month(1–12)
            //DOW
            string comand_job = "C:/TDM_Portal/Agendamento/Agendamento.exe " + argumentos;
            string timer = dataAgendamento.Value.Minute + " " + dataAgendamento.Value.Hour + " " + dataAgendamento.Value.Day + " " + dataAgendamento.Value.Month + " *";

            #region Debug
            Log.Info("comand_job: " + comand_job);
            Log.Info("timer: " + timer);
            #endregion

            string agendamentoDesc = "";

            #region Debug
            Log.Info("rotinaDiaria: " + rotinaDiaria);
            #endregion

            if (rotinaDiaria)
                agendamentoDesc = "AGENDAMENTO_ROTINA_DIARIA";
            else
                agendamentoDesc = "AGENDAMENTO";

            string url = "http://" + host + ":" + port + "/createItem?name=" + agendamentoDesc + "-" + idAgendamento;

            #region Debug
            Log.Info("agendamentoDesc: " + agendamentoDesc);
            Log.Info("Url: " + url);
            #endregion

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string mergedCredentials = string.Format("{0}:{1}", "thiago.b.teixeira", "thiago.b.teixeira");
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            string base64Credentials = Convert.ToBase64String(byteCredentials);
            request.Headers.Add("Authorization", "Basic " + base64Credentials);
            request.Method = "POST";
            request.ContentType = "application/xml";

            string xml = @"<project><description/><keepDependencies>false</keepDependencies><properties/><scm class=""hudson.scm.NullSCM""/><canRoam>true</canRoam><disabled>false</disabled><blockBuildWhenDownstreamBuilding>false</blockBuildWhenDownstreamBuilding><blockBuildWhenUpstreamBuilding>false</blockBuildWhenUpstreamBuilding><triggers><hudson.triggers.TimerTrigger><spec>" + timer + "</spec></hudson.triggers.TimerTrigger></triggers><concurrentBuild>false</concurrentBuild><builders><hudson.tasks.BatchFile><command>" + comand_job + "</command></hudson.tasks.BatchFile></builders><publishers/><buildWrappers/></project>";

            #region Debug
            Log.Info("xml: " + xml);
            #endregion

            byte[] byteArray = Encoding.UTF8.GetBytes(xml);
            request.ContentLength = byteArray.Length;

            #region Debug
            Log.Info("Abertura do Stream...");
            #endregion

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            #region Debug
            Log.Info("Strem fehada com...");
            #endregion

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            #region Debug
            Log.DebugObject(response);
            #endregion

            string result = string.Empty;
            using (StreamReader readerRes = new StreamReader(response.GetResponseStream()))
            {
                result = readerRes.ReadToEnd();
            }

        }

        public void UpdateJobJenkins(string nomeUsuario, DateTime? dataAgendamento, int idExecucao, int idDatapool, int idFaseTeste, int idMaquinaVirtual, bool enviarTelegram, int idAmbienteExecucao, int idUsuario, int? idTestDataAntigo)
        {

            DeleteJobJenkins(idTestDataAntigo);

            CreateJobJenkins(nomeUsuario, dataAgendamento, idExecucao, idDatapool, idFaseTeste, idMaquinaVirtual, enviarTelegram, idAmbienteExecucao, idUsuario);

        }

        public void DeleteJobJenkins(int? idAgendamento)
        {

            #region Debug
            Log.Debug("Inicio do delete job");
            #endregion


            string comand_job = "C:/TDM_Portal/Agendamento/Agendamento.exe ";

            string url = "http://" + host + ":" + port + "/view/Agendamentos/job/AGENDAMENTO-" + idAgendamento + "/doDelete";

            #region Debug
            Log.Debug("url job:"+url);
            #endregion

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            string mergedCredentials = string.Format("{0}:{1}", "thiago.b.teixeira", "thiago.b.teixeira");
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
            Log.Debug("Job deletado.");
            #endregion

            //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //string result = string.Empty;
            //using (StreamReader readerRes = new StreamReader(response.GetResponseStream()))
            //{
            //    result = readerRes.ReadToEnd();
            //}

        }


        public void SalvarTaskService(string nomeUsuario, DateTime? dataAgendamento, int idExecucao, int idDatapool, int idFaseTeste, int idMaquinaVirtual, bool enviarTelegram, int idAmbienteExecucao, int idUsuario)
        {
            #region Debug
            Log.Debug("Entrada no método de SalvarTaskService.");
            #endregion

            try
            {
                string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                // Usando o taskservice da maquina local

                #region Debug
                Log.Debug("Usuario capturado: " + user);
                #endregion

                string nomeMaquinaLocal = Environment.MachineName;

                #region Debug
                Log.Debug("Nome da máquina Local: " + nomeMaquinaLocal);
                #endregion

                using (TaskService ts = new TaskService(@"\\" + nomeMaquinaLocal))
                {
                    // Criando uma nova definição de atividade e atribuindo suas propriedades
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = "Rotina de agendamento ATMP.";

                    // td.RegistrationInfo.Author = nomeMaquinaLocal+"\\"+nomeUsuario;
                    td.RegistrationInfo.Author = user; //nomeMaquinaLocal + "\\Administrador";

                    //definindo o gatilho onde será definido qual a hora que irá ser executado a tarefa
                    td.Triggers.Add(new TimeTrigger(dataAgendamento ?? DateTime.Now));

                    //td.Settings.Compatibility
                    Execucao exec = db.Execucao.Where(x => x.Id == idExecucao).FirstOrDefault();

                    string argumentos = "idTestData:" + exec.IdTestData + ";" + "idFaseTeste:" + idFaseTeste + ";" + "idMaquinaVirtual:" + idMaquinaVirtual + ";" + "enviarTelegram:" + enviarTelegram + ";" + "idAmbienteExecucao:" + idAmbienteExecucao + ";" + "idUsuario:" + idUsuario;

                    string acao = "C://TDM_Portal//Agendamento//Agendamento.exe";

                    // criando a ação que será executada quando a trigger for chamada
                    td.Actions.Add(new ExecAction(acao, argumentos, null));


                    string usuario = "";
                    usuario = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                    td.Principal.UserId = usuario;
                    td.Principal.RunLevel = TaskRunLevel.Highest;

                    string nomeTask = "Agendamento ATMP-" + idExecucao;

                    #region Debug
                    Log.Debug("XmlText: " + td.XmlText);
                    #endregion

                    // Registrando a task na pasta raiz
                    ts.RootFolder.RegisterTaskDefinition(@nomeTask, td);
                }

            }
            catch (Exception ex)
            {

                Log.Error("Ex: " + ex + Environment.NewLine
                    + "Message: " + ex.Message + Environment.NewLine
                    + "InnerException: " + ex.InnerException);
                //throw
                //return (Json(ex.ToString() + " .. " + ex.StackTrace.ToString() + " .. " + ex.InnerException.ToString(), JsonRequestBehavior.AllowGet));
            }
        }

        public bool ExcluirTaskService(int idExecucao)
        {
            bool ret = false;
            string nomeMaquinaLocal = Environment.MachineName;
            using (TaskService ts = new TaskService(@"\\" + nomeMaquinaLocal))
            {
                Regex rx = new Regex(@"Agendamento ATMP*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                List<Task> t = ts.FindAllTasks(rx).ToList();

                string nomeTask = "Agendamento ATMP-" + idExecucao;

                foreach (Task item in t)
                {
                    if (item.Name.Equals(nomeTask))
                    {
                        ts.RootFolder.DeleteTask(nomeTask);
                        ret = true;
                    }
                }
            }
            return ret;
        }

        public ActionResult Index()
        {
            Usuario user = (Usuario)Session["ObjUsuario"];

            ViewBag.listaTDMs = (from t in db.TDM
                                 join tdm in db.TDM_Usuario on t.Id equals tdm.IdTDM
                                 select new TDMVO
                                 {
                                     IdTestData = t.Id,
                                     Descricao = t.Descricao
                                 }
                                ).DistinctBy(p => p.IdTestData).OrderBy(x => x.Descricao).ToList();

            ViewBag.ListaTipoFaseTeste = db.TipoFaseTeste.ToList();

            ViewBag.ListaAmbienteVirtual = db.AmbienteVirtual.ToList();


            //    ViewBag.ListaAmbienteVirt =  db.Database.SqlQuery<AmbienteVirtual>("EXEC PR_LISTAR_AMBIENTE_VIRTUAL_DISPONIVEL").ToList();




            return View();
        }

        public JsonResult CarregarExecucoes(string idAgendamento)
        {
            List<ExecucaoVO> execucoes = new List<ExecucaoVO>();

            int IdAgendamento = Int32.Parse(idAgendamento);

            List<Agendamento_TestData> agendamentos = db.Agendamento_TestData.Where(x => x.IdAgendamento == IdAgendamento).ToList();

            for (int i = 0; i < agendamentos.Count(); i++)
            {
                int IdTestData = agendamentos[i].IdTestData;
                Execucao execTemp = db.Execucao.Where(x => x.IdTestData == IdTestData).FirstOrDefault();
                TestData td = db.TestData.Where(x => x.Id == IdTestData).FirstOrDefault();
                DataPool dp = db.DataPool.Where(x => x.Id == td.IdDataPool).FirstOrDefault();
                TDM tdm = db.TDM.Where(x => x.Id == dp.IdTDM).FirstOrDefault();
                Agendamento ag = db.Agendamento.Where(x => x.Id == IdAgendamento).FirstOrDefault();

                ExecucaoVO exVO = new ExecucaoVO();
                exVO.IdStatusExecucao = execTemp.IdStatusExecucao;
                exVO.DescricaoTestData = td.Descricao;
                exVO.IdTestData = execTemp.IdTestData;
                exVO.DescricaoDatapool = dp.Descricao;
                exVO.DescricaoATMP = tdm.Descricao;
                exVO.DataAgendamento = ag.InicioAgendamento.ToString();

                execucoes.Add(exVO);
            }

            return (Json(execucoes, JsonRequestBehavior.AllowGet));
        }

        public JsonResult CarregaEventos(string date)
        {
            IList<AgendamentoVO> eventos = new List<AgendamentoVO>();
            List<Agendamento> agendamentos = new List<Agendamento>();

            // Recupera execuções do mês atual
            if (String.IsNullOrEmpty(date))
            {
                agendamentos = db.Agendamento.Where(a => a.InicioAgendamento.Value.Month.Equals(DateTime.Now.Month) && a.InicioAgendamento.Value.Year.Equals(DateTime.Now.Year)).ToList();
            }
            // Recupera execuções do mês em questão (parâmetro)
            else
            {
                agendamentos = db.Agendamento.Where(a => a.InicioAgendamento.Value.Month.Equals(DateTime.Parse(date).Month) && a.InicioAgendamento.Value.Year.Equals(DateTime.Parse(date).Year)).ToList();
            }

            for (int i = 0; i < agendamentos.Count(); i++)
            {
                int IdAgendTemp = agendamentos[i].Id;
                List<Execucao> execs = db.Execucao.Where(x => x.IdAgendamento == IdAgendTemp).ToList();
                List<int?> tds = new List<int?>();

                int countAgendada = 0;
                int countAguardandoProcessamento = 0;
                int countCancelada = 0;
                int countFalha = 0;
                int countSucesso = 0;
                int countTds = 0;

                #region
                for (int w = 0; w < execs.Count(); w++)
                {
                    int? IdTestData = execs[w].IdTestData;
                    //TestData td = db.TestData.Where(x=>x.Id == IdTestData).FirstOrDefault();
                    int IdStatus = execs[w].IdStatusExecucao;

                    switch (IdStatus)
                    {
                        case (int)Enumerators.EnumStatusExecucao.Agendada:
                            countAgendada++;
                            break;
                        case (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento:
                            countAguardandoProcessamento++;
                            break;
                        case (int)Enumerators.EnumStatusExecucao.Cancelada:
                            countCancelada++;
                            break;
                        case (int)Enumerators.EnumStatusExecucao.Falha:
                            countFalha++;
                            break;
                        case (int)Enumerators.EnumStatusExecucao.Sucesso:
                            countSucesso++;
                            break;
                    }
                    countTds++;
                    tds.Add(IdTestData);
                }
                #endregion

                if (tds.Count > 0)
                {

                    int? IdTd = tds[0];
                    TestData td = db.TestData.Where(x => x.Id == IdTd).FirstOrDefault();
                    //Pego o Id do datapool do primeiro testdata, pois todos os testdatas de um agendamento farão parte do mesmo datapool
                    int? IdDataPool = td.IdDataPool;

                    DataPool dp = db.DataPool.Where(x => x.Id == IdDataPool).FirstOrDefault();

                    AUT aut = db.AUT.Where(x => x.Id == dp.IdAut).FirstOrDefault();

                    int? IdScsa = execs[0].IdScript_CondicaoScript_Ambiente;
                    int? IdUsuario = Int32.Parse(execs[0].Usuario);

                    Usuario user = db.Usuario.Where(x => x.Id == IdUsuario).FirstOrDefault();

                    Script_CondicaoScript_Ambiente scsa = db.Script_CondicaoScript_Ambiente.Where(x => x.Id == IdScsa).FirstOrDefault();

                    Script_CondicaoScript scs = db.Script_CondicaoScript.Where(x => x.Id == scsa.IdScript_CondicaoScript).FirstOrDefault();

                    Script s = db.Script.Where(x => x.Id == scs.IdScript).FirstOrDefault();

                    CondicaoScript cs = db.CondicaoScript.Where(x => x.Id == scs.IdCondicaoScript).FirstOrDefault();

                    string condicaoScript = cs != null ? cs.Descricao : "";

                    AmbienteVirtual av = db.AmbienteVirtual.Where(x => x.Id == scsa.IdAmbienteVirtual).FirstOrDefault();

                    AmbienteExecucao ae = db.AmbienteExecucao.Where(x => x.Id == scsa.IdAmbienteExecucao).FirstOrDefault();


                    string start = agendamentos[i].InicioAgendamento != null ? agendamentos[i].InicioAgendamento.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "";
                    string end = agendamentos[i].TerminoAgendamento != null ? agendamentos[i].TerminoAgendamento.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "";
                    bool? rotinaDiaria = agendamentos[i].RotinaDiaria != null ? agendamentos[i].RotinaDiaria : false;
                    string title = user.Login + " - " + av.Descricao;
                    int IdFaseTeste = execs[0].IdTipoFaseTeste;
                    int idTDM = dp.IdTDM;
                    string color = "blue";
                    string status = "Agendada";
                    string textColor = "white";

                    if (countCancelada > 0 || countFalha > 0)
                    {
                        color = "red";
                        if (countCancelada == countTds)
                        {
                            status = "Cancelado";
                        }
                        else if (countFalha == countTds)
                        {
                            status = "Falha";
                        }
                        else
                        {
                            status = "Cancelado/Falha";
                        }
                    }
                    else if (countTds == countSucesso)
                    {
                        color = "green";
                        status = "Sucesso";
                    }
                    else if (countTds == countAgendada)
                    {
                        color = rotinaDiaria == true ? "yellow" : "blue";
                        textColor = rotinaDiaria == true ? "black" : "white";
                    }
                    else if (countTds == countAguardandoProcessamento)
                    {
                        color = "gray";
                        status = "Aguardando Processamento";
                    }

                    DateTime dt = new DateTime();

                    int hora = scs.TempoEstimadoExecucao.Hour;
                    int min = scs.TempoEstimadoExecucao.Minute;


                    for (int x = 0; x < tds.Count(); x++)
                    {
                        dt = dt.AddHours(hora).AddMinutes(min);
                    }

                    string TempoEstimadoCalculado = dt.ToString("yyyy-MM-ddTHH:mm:ss");

                    #region
                    //for (int y = 0; y < listaAgTds.Count(); y++)
                    //{
                    //    #region
                    //    TestData testData = db.TestData.FirstOrDefault(x => x.Id == listaAgTds[y].IdTestData);
                    //    if (testData != null)
                    //    {
                    //        Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();

                    //        if (script_CondicaoScript != null)
                    //        {
                    //            Script script = db.Script.FirstOrDefault(x => x.Id == script_CondicaoScript.IdScript);
                    //            CondicaoScript condicao = db.CondicaoScript.FirstOrDefault(x => x.Id == script_CondicaoScript.IdCondicaoScript);
                    //            AUT aut = db.AUT.FirstOrDefault(x => x.Id == script.IdAUT);
                    //            Execucao exec = db.Execucao.Where(x => x.IdTestData == testData.Id).FirstOrDefault();
                    //            Script_CondicaoScript_Ambiente sca = db.Script_CondicaoScript_Ambiente.FirstOrDefault(x => x.Id == exec.IdScript_CondicaoScript_Ambiente);
                    //            StatusExecucao statusExecucao = db.StatusExecucao.FirstOrDefault(x => x.Id == exec.IdStatusExecucao);
                    //            DataPool dpTemp = db.DataPool.FirstOrDefault(x => x.Id == testData.IdDataPool);

                    //            start = agendamentos[i].InicioAgendamento != null ? agendamentos[i].InicioAgendamento.Value.ToString("yyyy-MM-ddTHH:mm:ss") : "";
                    //            title = "" + exec.IdTestData;
                    //            idAmbienteVirtual = sca.IdAmbienteVirtual;
                    //            idAmbienteExecucao = sca.IdAmbienteExecucao;
                    //            idTDM = dpTemp.IdTDM;
                    //            IdStatusExecucao = exec.IdStatusExecucao + "";
                    //            color = "";

                    //            int idStatus = exec.IdStatusExecucao;
                    //            #region
                    //            switch (idStatus)
                    //            {
                    //                case (int)Enumerators.EnumStatusExecucao.Agendada:
                    //                    color = "blue";
                    //                    break;
                    //                case (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento:
                    //                    color = "gray";
                    //                    break;
                    //                case (int)Enumerators.EnumStatusExecucao.Cancelada:
                    //                    color = "red";
                    //                    break;
                    //                case (int)Enumerators.EnumStatusExecucao.Falha:
                    //                    color = "red";
                    //                    break;
                    //                case (int)Enumerators.EnumStatusExecucao.Sucesso:
                    //                    color = "green";
                    //                    break;
                    //                default:
                    //                    color = "blue";
                    //                    break;
                    //            }
                    //            #endregion
                    //        }
                    //    }
                    //    #endregion
                    //}
                    #endregion

                    AgendamentoVO evento = new AgendamentoVO
                    {
                        IdAgendamento = IdAgendTemp,
                        idFase = IdFaseTeste,
                        idAmbienteExecucao = scsa.IdAmbienteExecucao,
                        idDatapool = dp.Id,
                        DescricaoAmbienteExecucao = ae.Descricao != "" ? ae.Descricao : "",
                        DescricaoAmbienteVirtual = av.Descricao != "" ? av.Descricao : "",
                        testDatas = tds,
                        LoginUsuario = user.Login,
                        QtdTds = tds.Count(),
                        status = status,
                        title = title,
                        start = start,
                        end = end,
                        allDay = false,
                        idAmbienteVirtual = scsa.IdAmbienteVirtual,
                        idTdm = idTDM,
                        script = s.Descricao,
                        condicaoScript = condicaoScript,
                        sistema = aut.Descricao,
                        color = color,
                        textColor = textColor,
                        Output = agendamentos[i].Output,
                        TempoEstimadoExecucao = scs.TempoEstimadoExecucao,
                        TempoEstimadoExecucaoCalculado = TempoEstimadoCalculado
                    };
                    eventos.Add(evento);
                }
            }

            return (Json(eventos, JsonRequestBehavior.AllowGet));

        }

        public JsonResult listarTestDatas(string datapool, string id = "0")
        {
            #region Debug

            #endregion

            int idDatapool = Int32.Parse(datapool);

            SqlParameter[] param =
            {
                new SqlParameter("@IdDatapool", idDatapool),
            };

            #region
            //log.Info("Preparando a execução da PROC PR_LISTAR_TEST_DATA, passando o parâmtetro: " + idDatapool);
            #endregion
            //Executa a PROC que retorna os TestData
            List<TestDataVO> TestDataVOs = db.Database.SqlQuery<TestDataVO>("EXEC PR_LISTAR_TESTDATA @IdDatapool ", param).OrderBy(x => x.Descricao).ToList();

            #region
            //log.Info("PROC 'PR_LISTAR_TEST_DATA' executada com sucesso e foram retornados " + TestDataVOs.Count() + " registros.");
            #endregion

            if (!id.Equals("0"))
            {
                string[] ids = id.Split(',');

                foreach (string idTemp in ids)
                {
                    int IdTemp = Int32.Parse(idTemp);
                    TestData td = db.TestData.Find(IdTemp);

                    TestDataVO tdVO = new TestDataVO
                    {
                        Descricao = td.Descricao,
                        IdTestData = td.Id,
                        Selected = true
                    };

                    TestDataVOs.Add(tdVO);
                }
            }


            string json = JsonConvert.SerializeObject(TestDataVOs, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }

        public JsonResult listarDatapools(int tdm, string id = "0")
        {
            SqlParameter[] param =
            {
                new SqlParameter("@IDTDM", tdm),
            };

            List<DataPoolVO> dps = db.Database.SqlQuery<DataPoolVO>("EXEC PR_LISTAR_DATAPOOL_COM_TESTDATA @IDTDM", param).OrderBy(x => x.DescricaoTDM).ToList();

            int idDP = Int32.Parse(id);

            if (!id.Equals("0"))
            {
                DataPoolVO dpVO = null;
                DataPool datapool = db.DataPool.FirstOrDefault(x => x.Id == idDP);
                bool exist = false;

                if (datapool != null)
                {
                    foreach (DataPoolVO dp in dps)
                    {
                        if (dp.Id == datapool.Id)
                        {
                            exist = true;
                        }
                    }

                    if (!exist)
                    {
                        dpVO = new DataPoolVO { Id = datapool.Id, DescricaoDataPool = datapool.Descricao };
                        dps.Add(dpVO);
                    }

                }
            }

            string json = JsonConvert.SerializeObject(dps, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }

        public JsonResult listarAmbienteVirtualLivre()
        {

            List<AmbienteVirtual> ambientes = db.Database.SqlQuery<AmbienteVirtual>("EXEC PR_LISTAR_AMBIENTE_VIRTUAL_DISPONIVEL").OrderBy(x => x.Descricao).ToList();


            string json = JsonConvert.SerializeObject(ambientes, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }

        public JsonResult listarAmbientes(string idDatapool)
        {

            int id = Int32.Parse(idDatapool);



            SqlParameter[] param =
           {
                new SqlParameter("@ID_SCRIPT_CONDICAOSCRIPT", id),
            };

            List<AmbienteExecucao> ambientes = db.Database.SqlQuery<AmbienteExecucao>("EXEC PR_LISTAR_AMBIENTE_EXECUCAO @ID_SCRIPT_CONDICAOSCRIPT", param).ToList();


            string json = JsonConvert.SerializeObject(ambientes, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }

        public bool ValidarDisponibilidadeAmbiente(string idMaquinaVirtual, string idAmbienteExecucao, string dtAgendamento, string idAgendamento)
        {
            #region Debug
            Log.Info("Entrada no metodo de Validar Disponibilidade de Ambiente.");
            #endregion

            bool ret = true;
            int idAmbExec = Int32.Parse(idAmbienteExecucao);
            int idAmbVirt = Int32.Parse(idMaquinaVirtual);
            int idAgend = Int32.Parse(idAgendamento);

            DateTime date = DateTime.Parse(dtAgendamento);

            #region Debug
            Log.Info("Ret: " + ret);
            Log.Debug("idAmbExec: " + idAmbExec);
            Log.Debug("idAmbVirt: " + idAmbVirt);
            Log.Debug("idAgend: " + idAgend);
            Log.Debug("date: " + date);
            #endregion

            List<AgendamentoVO> ags = (from ag in db.Agendamento
                                       join ex in db.Execucao on ag.Id equals ex.IdAgendamento
                                       join scsa in db.Script_CondicaoScript_Ambiente on ex.IdScript_CondicaoScript_Ambiente equals scsa.Id
                                       where ag.InicioAgendamento == date
                                       && scsa.IdAmbienteExecucao == idAmbExec
                                       && scsa.IdAmbienteVirtual == idAmbVirt
                                       && ag.Id != idAgend
                                       select new AgendamentoVO
                                       {
                                           IdAgendamento = ag.Id,
                                           idAmbienteVirtual = scsa.IdAmbienteVirtual,
                                           idAmbienteExecucao = scsa.IdAmbienteExecucao
                                       }).ToList();
            #region Debug
            Log.Info("Capturado os Agendamentos.");
            Log.DebugObject(ags);
            #endregion


            // Já possui algum agendamento para o ambiente selecionado.
            if (ags.Count() > 0)
            {
                ret = false;
            }

            #region Debug
            Log.Info("Retornando o valor de Ret: " + ret);
            #endregion

            return ret;
        }


        public JsonResult ManterAgendamento(string idAgendamento, string dtAgendamento, string idTestData, string idFaseTeste, string idMaquinaVirtual, string opcaoTelegram, string idAmbienteExecucao, bool rotinaDiaria = false)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            #region Debug
            Log.Debug("Resultado Json: " + result);
            #endregion

            if (!String.IsNullOrEmpty(dtAgendamento) && !String.IsNullOrEmpty(idTestData) && !String.IsNullOrEmpty(idFaseTeste) && !String.IsNullOrEmpty(idMaquinaVirtual) &&
                !String.IsNullOrEmpty(opcaoTelegram) && !String.IsNullOrEmpty(idAmbienteExecucao))
            {
                #region Debug
                Log.Debug("Entrada na Condicao de objetos diferentes de Null.");

                Log.Debug("Parametros do ManterAgendamento: ");
                Log.Debug("idAgendamento: " + idAgendamento);
                Log.Debug("dtAgendamento: " + dtAgendamento);
                Log.Debug("idTestData: " + idTestData);
                Log.Debug("idFaseTeste: " + idFaseTeste);
                Log.Debug("idMaquinaVirtual: " + idMaquinaVirtual);
                Log.Debug("opcaoTelegram: " + opcaoTelegram);
                Log.Debug("idAmbienteExecucao: " + idAmbienteExecucao);
                Log.Debug("rotinaDiaria: " + rotinaDiaria);

                #endregion
                //string json = "";

                DbEntities db = new DbEntities();
                #region Debug
                Log.Debug("Nova instancia do DbEntities criada com sucesso.");
                Log.Debug("Iniciando validação de disponibilidade...");
                #endregion

                bool ret = ValidarDisponibilidadeAmbiente(idMaquinaVirtual, idAmbienteExecucao, dtAgendamento, idAgendamento);

                #region Debug

                Log.Debug("Ambiente disponivel: " + ret);
                #endregion

                if (ret)
                {
                    #region Debug
                    Log.Info("O ambiente encontra-se disponivel para execucao...");
                    #endregion

                    string[] testDatas = idTestData.Split(',');

                    #region
                    //Criacao de um novo agendamento
                    if (idAgendamento.Equals("0"))
                    {

                        #region Debug
                        Log.Info("Foi verificado que a acão é um cadastro de um agendamento novo.");

                        Log.Debug("Criando novo agendamento");
                        #endregion


                        int qtd = testDatas.Length;

                        #region Debug
                        Log.Debug("Qtd de TestData: " + qtd);
                        #endregion

                        DateTime dtInicioAgen = DateTime.Parse(dtAgendamento);
                        int IdTdTemp = Int32.Parse(testDatas[0]);
                        TestData tdtemp = db.TestData.Where(x => x.Id == IdTdTemp).FirstOrDefault();

                        #region Debug
                        Log.Debug("dtInicioAgen: " + dtInicioAgen);
                        Log.Debug("IdTdTemp: " + IdTdTemp);
                        Log.DebugObject(tdtemp);
                        #endregion

                        //int hora = tdtemp.TempoEstimadoExecucao.Hour * qtd;
                        //int min = tdtemp.TempoEstimadoExecucao.Minute * qtd;


                        Script_CondicaoScript script_CondicaoScript = new Script_CondicaoScript();
                        script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == tdtemp.IdScript_CondicaoScript).FirstOrDefault();

                        int hora = script_CondicaoScript.TempoEstimadoExecucao.Hour * qtd;
                        int min = script_CondicaoScript.TempoEstimadoExecucao.Minute * qtd;

                        DateTime dtTermAgen = dtInicioAgen.AddHours(hora).AddMinutes(min);

                        hora = dtInicioAgen.Hour;
                        min = dtInicioAgen.Minute;

                        DateTime TempoEstimadoCalculado = dtTermAgen.AddHours(-hora).AddMinutes(-min);

                        string TempoEstimadoExecucaoCalculado = TempoEstimadoCalculado.ToString("yyyy-MM-ddTHH:mm:ss");

                        #region Debug
                        Log.Debug("Tempo estimado: " + TempoEstimadoExecucaoCalculado);
                        Log.DebugObject(script_CondicaoScript);
                        Log.Debug("hora: " + hora);
                        Log.Debug("min: " + min);

                        #endregion
                        ////Repete de acordo com a quantidade de vezes 
                        //testDatas.ToList().ForEach(td=>{
                        //    dtTermAgen = dtTermAgen.AddHours(hora).AddMinutes(min);
                        //});

                        string horaTermino = dtTermAgen.GetDateTimeFormats()[54].ToString();
                        Agendamento ag = new Agendamento
                        {
                            InicioAgendamento = dtInicioAgen,
                            TerminoAgendamento = dtTermAgen,
                            RotinaDiaria = rotinaDiaria
                        };

                        Log.Debug("Tempo estimado: " + TempoEstimadoExecucaoCalculado);
                        db.Agendamento.Add(ag);
                        Log.Debug("Salvando Agendamento...");
                        db.SaveChanges();
                        Log.Debug("Agendamneto criado com sucesso!");
                        Usuario user;
                        if (rotinaDiaria == false)
                        {
                            user = (Usuario)Session["ObjUsuario"];
                        }
                        else
                        {
                            user = db.Usuario.Where(x => x.Login == "administrador").FirstOrDefault();
                        }

                        List<int?> listaTestDatas = new List<int?>();
                        //Script_CondicaoScript script_CondicaoScript = new Script_CondicaoScript();
                        Script script = new Script();
                        DataPool dp = new DataPool();
                        CondicaoScript condicao = new CondicaoScript();
                        AUT aut = new AUT();
                        Execucao exec = new Execucao();

                        int faseTesteId = 0;
                        int? maquinaVirtualId = null;
                        int? ambienteExecucaoId = null;

                        Script_CondicaoScript_Ambiente script_CondicaoScript_Ambiente = new Script_CondicaoScript_Ambiente();

                        for (int i = 0; i < testDatas.Length; i++)
                        {
                            string idTd = testDatas[i];
                            // validar ambiente disponível e setar a flag em uso antes de inserir a query
                            int testdataId = Int32.Parse(idTd);

                            Agendamento_TestData agTd = new Agendamento_TestData
                            {
                                IdAgendamento = ag.Id,
                                IdTestData = testdataId
                            };

                            db = new DbEntities();
                            db.Agendamento_TestData.Add(agTd);
                            db.SaveChanges();

                            //recuperando objeto testdata, para ter recuperar o IdScript_CondicaoScript
                            TestData testData = db.TestData.Where(x => x.Id == testdataId).FirstOrDefault();

                            listaTestDatas.Add(testData.Id);
                            Log.Debug("Adicionado TestData na lista ID: " + testData.Id);

                            //script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();
                            script = db.Script.FirstOrDefault(x => x.Id == script_CondicaoScript.IdScript);
                            condicao = db.CondicaoScript.FirstOrDefault(x => x.Id == script_CondicaoScript.IdCondicaoScript);
                            aut = db.AUT.FirstOrDefault(x => x.Id == script.IdAUT);
                            dp = db.DataPool.FirstOrDefault(x => x.Id == testData.IdDataPool);
                            string query = script_CondicaoScript.QueryTosca;

                            faseTesteId = Int32.Parse(idFaseTeste);
                            maquinaVirtualId = Int32.Parse(idMaquinaVirtual);
                            ambienteExecucaoId = Int32.Parse(idAmbienteExecucao);

                            String queryTemp = query.Replace("ptdTosca", testdataId.ToString());

                            exec = new Execucao();
                            script_CondicaoScript_Ambiente = db.Script_CondicaoScript_Ambiente.Where(x => x.IdScript_CondicaoScript == testData.IdScript_CondicaoScript).Where(x => x.IdAmbienteVirtual == maquinaVirtualId).Where(x => x.IdAmbienteExecucao == ambienteExecucaoId).FirstOrDefault();
                            exec.IdScript_CondicaoScript_Ambiente = script_CondicaoScript_Ambiente.Id;
                            exec.IdTipoFaseTeste = faseTesteId; // pegar via campo popup modal play
                            exec.SituacaoAmbiente = (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento;
                            exec.Usuario = user.Id.ToString();
                            exec.IdTestData = testdataId; // pegar o id via tela
                            exec.IdAgendamento = ag.Id;
                            exec.ToscaInput = queryTemp;
                            exec.EnvioTelegram = opcaoTelegram.Equals("1") ? true : false;
                            exec.IdStatusExecucao = (int)Enumerators.EnumStatusExecucao.Agendada;
                            exec.InicioExecucao = DateTime.Parse(dtAgendamento);

                            db.Execucao.Add(exec);

                            testData.IdExecucao = exec.Id;
                            db.TestData.Attach(testData);
                            //Prepara a entidade para uma Edição
                            db.Entry(testData).State = System.Data.Entity.EntityState.Modified;
                            Log.Debug("Salvando alteração do testdata..." + testData.Id);
                            try
                            {
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                Log.Error(e);
                            }

                            Log.Debug("Testdata atualizado com sucesso! ID:" + testData.Id);
                        }

                        AmbienteExecucao ae = db.AmbienteExecucao.Where(x => x.Id == script_CondicaoScript_Ambiente.IdAmbienteExecucao).FirstOrDefault();
                        int? IdAmbVirt = script_CondicaoScript_Ambiente.IdAmbienteVirtual;
                        AmbienteVirtual av = db.AmbienteVirtual.Where(x => x.Id == IdAmbVirt).FirstOrDefault();



                        AgendamentoVO agendamento = new AgendamentoVO();
                        agendamento.IdAgendamento = ag.Id;
                        agendamento.color = rotinaDiaria == true ? "yellow" : "blue";
                        agendamento.textColor = rotinaDiaria == true ? "black" : "white";
                        agendamento.telegram = opcaoTelegram.Equals("1") ? true : false;
                        agendamento.testDatas = listaTestDatas;
                        agendamento.idTdm = dp.IdTDM;
                        agendamento.idFase = faseTesteId;
                        agendamento.idAmbienteVirtual = maquinaVirtualId;
                        agendamento.idAmbienteExecucao = ambienteExecucaoId;
                        agendamento.start = dtAgendamento;
                        agendamento.end = horaTermino;
                        agendamento.script = script.Descricao;
                        agendamento.condicaoScript = condicao == null ? "" : condicao.Descricao;
                        agendamento.sistema = aut.Descricao;
                        agendamento.title = user.Login + " - " + av.Descricao;
                        agendamento.QtdTds = testDatas.Length;
                        agendamento.LoginUsuario = user.Login;
                        agendamento.DescricaoAmbienteExecucao = ae.Descricao;
                        agendamento.DescricaoAmbienteVirtual = av.Descricao;
                        agendamento.status = "Agendada";
                        agendamento.testDatas = listaTestDatas;
                        agendamento.TempoEstimadoExecucaoCalculado = TempoEstimadoExecucaoCalculado;

                        Log.DebugObject(agendamento);

                        try
                        {
                            db.SaveChanges();
                            agendamento.status = "Agendada";

                            CreateJobJenkins(user.Login,
                                dtInicioAgen,
                                ag.Id,
                                dp.Id,
                                faseTesteId,
                                maquinaVirtualId,
                                exec.EnvioTelegram,
                                ambienteExecucaoId,
                                user.Id);

                            result.Data = new { Result = "true", Status = (int)WebExceptionStatus.Success, agendamento };
                        }
                        catch (Exception ex)
                        {
                            this.FlashError(ex.Message);
                            return (Json(ex.ToString() + " .. " + ex.StackTrace.ToString() + " .. " + ex.InnerException.ToString(), JsonRequestBehavior.AllowGet));
                        }
                    }
                    #endregion
                    else
                    #region
                    {
                        int IdAgendamento = Int32.Parse(idAgendamento);

                        #region Debug
                        Log.Debug("IdAgendamento: " + IdAgendamento);
                        #endregion

                        Usuario user;
                        if (rotinaDiaria == false)
                        {
                            user = (Usuario)Session["ObjUsuario"];
                        }
                        else
                        {
                            user = db.Usuario.Where(x => x.Login == "administrador").FirstOrDefault();
                        }

                        #region Debug
                        Log.Debug("User: " + user.Login);
                        #endregion

                        int qtd = testDatas.Length;
                        DateTime dtInicioAgen = DateTime.Parse(dtAgendamento);
                        int IdTdTemp = Int32.Parse(testDatas[0]);
                        TestData tdtemp = db.TestData.Where(x => x.Id == IdTdTemp).FirstOrDefault();

                        int hora = tdtemp.TempoEstimadoExecucao.Hour * qtd;
                        int min = tdtemp.TempoEstimadoExecucao.Minute * qtd;

                        DateTime dtTermAgen = dtInicioAgen.AddHours(hora).AddMinutes(min);
                        string horaTermino = dtTermAgen.GetDateTimeFormats()[54].ToString();

                        #region Debug
                        Log.Debug("qtd: " + qtd);
                        Log.Debug("dtInicioAgen: " + dtInicioAgen);
                        Log.Debug("IdTdTemp: " + IdTdTemp);
                        Log.DebugObject(tdtemp);
                        Log.Debug("hora: " + hora);
                        Log.Debug("min: " + min);
                        Log.Debug("horaTermino: " + horaTermino);
                        #endregion

                        Agendamento ag = db.Agendamento.Where(x => x.Id == IdAgendamento).FirstOrDefault();
                        ag.InicioAgendamento = dtInicioAgen;
                        ag.TerminoAgendamento = dtTermAgen;

                        #region Debug

                        Log.DebugObject(ag);

                        #endregion

                        db.Agendamento.Attach(ag);
                        //Prepara a entidade para uma Edição
                        db.Entry(ag).State = System.Data.Entity.EntityState.Modified;

                        #region Debug

                        Log.Info("Salvando Agendamento...");

                        #endregion

                        db.SaveChanges();

                        #region Debug

                        Log.Info("Agendamento Editado com Sucesso.");

                        #endregion

                        db = new DbEntities();

                        List<int?> listaTestDatas = new List<int?>();
                        Script_CondicaoScript script_CondicaoScript = new Script_CondicaoScript();
                        Script script = new Script();
                        DataPool dp = new DataPool();
                        CondicaoScript condicao = new CondicaoScript();
                        AUT aut = new AUT();
                        Execucao exec = new Execucao();

                        int faseTesteId = 0;
                        int? maquinaVirtualId = null;
                        int? ambienteExecucaoId = null;

                        List<Agendamento_TestData> agtds = db.Agendamento_TestData.Where(x => x.IdAgendamento == ag.Id).ToList();

                        #region Debug

                        Log.DebugObject(agtds);

                        #endregion

                        Script_CondicaoScript_Ambiente script_CondicaoScript_Ambiente = new Script_CondicaoScript_Ambiente();

                        #region

                        #region Debug

                        Log.Info("Iniciando Loop de Agendamento_TestData");

                        #endregion

                        foreach (Agendamento_TestData agtd in agtds)
                        {
                            TestData td = db.TestData.Where(x => x.Id == agtd.IdTestData).FirstOrDefault();
                            td.IdExecucao = null;
                            db.TestData.Attach(td);
                            //Prepara a entidade para uma Edição
                            db.Entry(td).State = System.Data.Entity.EntityState.Modified;

                            #region Debug
                            Log.Info("Salvando Alteracoes do TestData.");
                            Log.DebugObject(td);
                            #endregion

                            db.SaveChanges();

                            #region Debug
                            Log.Info("Alteracoes realizadas com sucesso..");
                            Log.DebugObject(td);
                            #endregion

                            Execucao ex = db.Execucao.Where(x => x.IdTestData == td.Id).FirstOrDefault();
                            db.Execucao.Remove(ex);
                            db.SaveChanges();

                            db.Agendamento_TestData.Remove(agtd);

                            #region Debug
                            Log.Info("Salvando Alteracoes do Execucao.");
                            Log.DebugObject(ex);
                            #endregion

                            db.SaveChanges();

                            #region Debug
                            Log.Info("Alteracoes realizadas com sucesso..");
                            Log.DebugObject(ex);
                            #endregion
                        }
                        #endregion

                        #region 
                        for (int i = 0; i < testDatas.Length; i++)
                        {
                            int idtd = Int32.Parse(testDatas[i]);
                            listaTestDatas.Add(idtd);

                            #region Debug
                            Log.Info("Adicionado Id " + idtd + "na lista de testdata.");
                            Log.DebugObject(listaTestDatas);
                            #endregion

                            Agendamento_TestData agtd = new Agendamento_TestData
                            {
                                IdAgendamento = ag.Id,
                                IdTestData = idtd
                            };
                            db.Agendamento_TestData.Add(agtd);

                            #region Debug
                            Log.Info("Salvando Objeto de Agendamento TestData");
                            Log.DebugObject(agtd);
                            #endregion

                            db.SaveChanges();

                            #region Debug
                            Log.Info("Agendamento TestData Salvo com sucesso.");
                            Log.DebugObject(agtd);
                            #endregion

                            TestData testData = db.TestData.Where(x => x.Id == idtd).FirstOrDefault();

                            script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();
                            script = db.Script.FirstOrDefault(x => x.Id == script_CondicaoScript.IdScript);
                            condicao = db.CondicaoScript.FirstOrDefault(x => x.Id == script_CondicaoScript.IdCondicaoScript);
                            aut = db.AUT.FirstOrDefault(x => x.Id == script.IdAUT);
                            dp = db.DataPool.FirstOrDefault(x => x.Id == testData.IdDataPool);
                            string query = script_CondicaoScript.QueryTosca;

                            faseTesteId = Int32.Parse(idFaseTeste);
                            maquinaVirtualId = Int32.Parse(idMaquinaVirtual);
                            ambienteExecucaoId = Int32.Parse(idAmbienteExecucao);

                            #region Debug
                            Log.DebugObject(testData);
                            Log.DebugObject(script_CondicaoScript);
                            Log.DebugObject(script);
                            Log.DebugObject(condicao);
                            Log.DebugObject(aut);
                            Log.DebugObject(dp);
                            Log.DebugObject(query);
                            Log.Debug("faseTesteId: " + faseTesteId);
                            Log.Debug("maquinaVirtualId: " + maquinaVirtualId);
                            Log.Debug("ambienteExecucaoId: " + ambienteExecucaoId);
                            #endregion

                            String queryTemp = query.Replace("ptdTosca", idtd.ToString());

                            exec = new Execucao();
                            script_CondicaoScript_Ambiente = db.Script_CondicaoScript_Ambiente.Where(x => x.IdScript_CondicaoScript == testData.IdScript_CondicaoScript).Where(x => x.IdAmbienteVirtual == maquinaVirtualId).Where(x => x.IdAmbienteExecucao == ambienteExecucaoId).FirstOrDefault();
                            exec.IdScript_CondicaoScript_Ambiente = script_CondicaoScript_Ambiente.Id;
                            exec.IdTipoFaseTeste = faseTesteId; // pegar via campo popup modal play
                            exec.SituacaoAmbiente = (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento;
                            exec.Usuario = user.Id.ToString();
                            exec.IdTestData = idtd; // pegar o id via tela
                            exec.IdAgendamento = ag.Id;
                            exec.ToscaInput = queryTemp;
                            exec.EnvioTelegram = opcaoTelegram.Equals("1") ? true : false;
                            exec.IdStatusExecucao = (int)Enumerators.EnumStatusExecucao.Agendada;
                            exec.InicioExecucao = DateTime.Parse(dtAgendamento);

                            db.Execucao.Add(exec);

                            testData.IdExecucao = exec.Id;
                            db.TestData.Attach(testData);
                            //Prepara a entidade para uma Edição
                            db.Entry(testData).State = System.Data.Entity.EntityState.Modified;

                            #region Debug
                            Log.Debug("Salvando Execucao...");
                            Log.DebugObject(exec);
                            #endregion

                            db.SaveChanges();

                            #region Debug
                            Log.Debug("Alteracoes realizadas com sucesso.");
                            Log.DebugObject(exec);
                            #endregion

                        }
                        #endregion

                        AmbienteExecucao ae = db.AmbienteExecucao.Where(x => x.Id == script_CondicaoScript_Ambiente.IdAmbienteExecucao).FirstOrDefault();
                        int? IdAmbVirt = script_CondicaoScript_Ambiente.IdAmbienteVirtual;
                        AmbienteVirtual av = db.AmbienteVirtual.Where(x => x.Id == IdAmbVirt).FirstOrDefault();


                        #region Debug
                        Log.DebugObject(ae);
                        Log.DebugObject(av);
                        #endregion

                        AgendamentoVO agendamento = new AgendamentoVO();
                        agendamento.IdAgendamento = ag.Id;
                        agendamento.color = "blue";
                        agendamento.textColor = "white";
                        agendamento.telegram = opcaoTelegram.Equals("1") ? true : false;
                        agendamento.testDatas = listaTestDatas;
                        agendamento.idTdm = dp.IdTDM;
                        agendamento.idFase = faseTesteId;
                        agendamento.idAmbienteVirtual = maquinaVirtualId;
                        agendamento.idAmbienteExecucao = ambienteExecucaoId;
                        agendamento.start = dtAgendamento;
                        agendamento.end = horaTermino;
                        agendamento.script = script.Descricao;
                        agendamento.condicaoScript = condicao == null ? "" : condicao.Descricao;
                        agendamento.sistema = aut.Descricao;
                        agendamento.title = user.Login + " - " + av.Descricao;
                        agendamento.QtdTds = testDatas.Length;
                        agendamento.LoginUsuario = user.Login;
                        agendamento.DescricaoAmbienteExecucao = ae.Descricao;
                        agendamento.status = "Agendada";
                        agendamento.testDatas = listaTestDatas;

                        #region Debug
                        Log.Info("Salvando alteracoes no AgendamentoVO...");
                        Log.DebugObject(agendamento);
                        #endregion

                        try
                        {
                            db.SaveChanges();
                            //agendamento.status = "Agendada";

                            #region Debug
                            Log.Info("Alteracoes no AgendamentoVO realizadas com sucesso.");
                            Log.DebugObject(agendamento);
                            Log.Info("Deletndo Job no Jenkins...");
                            #endregion

                            DeleteJobJenkins(ag.Id);

                            #region Debug
                            Log.Info("Job Deletdo com sucesso.");
                            Log.Info("Criando Job no Jenkins...");
                            #endregion

                            Thread.Sleep(1000);

                            CreateJobJenkins(user.Login,
                                dtInicioAgen,
                                ag.Id,
                                dp.Id,
                                faseTesteId,
                                maquinaVirtualId,
                                exec.EnvioTelegram,
                                ambienteExecucaoId,
                                user.Id);

                            #region Debug
                            Log.Info("Job Criando com sucesso.");
                            Log.Debug("result.Data: " + result.Data);
                            Log.DebugObject(result.Data);

                            #endregion
                            //CreateJobJenkins(user.Login,
                            //    exec.InicioExecucao,
                            //    exec.Id,
                            //    testData.IdDataPool,
                            //    faseTesteId,
                            //    maquinaVirtualId,
                            //    exec.EnvioTelegram,
                            //    ambienteExecucaoId,
                            //    user.Id);
                            result.Data = new { Result = "true", Status = (int)WebExceptionStatus.Success, agendamento };
                            //json = JsonConvert.SerializeObject(agendamento, Formatting.Indented);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            this.FlashError(ex.Message);
                            return (Json(ex.ToString() + " .. " + ex.StackTrace.ToString() + " .. " + ex.InnerException.ToString(), JsonRequestBehavior.AllowGet));
                        }
                    }
                    #endregion
                }
                else
                {
                    result.Data = new { Result = "false", Status = (int)WebExceptionStatus.UnknownError };
                    this.FlashWarning("O ambiente selecionado não estará disponível neste horário.");
                }
            }
            return result;

        }

        public JsonResult Remover(string idAgendamento)
        {
            int IdAgendamento = Int32.Parse(idAgendamento);

            List<Execucao> execs = db.Execucao.Where(x => x.IdAgendamento == IdAgendamento).ToList();

            foreach (Execucao exec in execs)
            {
                List<TestData> tds = db.TestData.Where(x => x.Id == exec.IdTestData).ToList();

                foreach (TestData td in tds)
                {
                    td.IdExecucao = null;
                    db.TestData.Attach(td);
                    //Prepara a entidade para uma Edição
                    db.Entry(td).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                db.Execucao.Remove(exec);
                db.SaveChanges();
            }

            List<Agendamento_TestData> agtds = db.Agendamento_TestData.Where(x => x.IdAgendamento == IdAgendamento).ToList();
            foreach (Agendamento_TestData agtd in agtds)
            {
                db.Agendamento_TestData.Remove(agtd);
                db.SaveChanges();
            }

            db = new DbEntities();

            Agendamento ag = db.Agendamento.FirstOrDefault(x => x.Id == IdAgendamento);
            db.Agendamento.Remove(ag);
            db.SaveChanges();
            //ExcluirTaskService(execucao.Id);
            DeleteJobJenkins(IdAgendamento);

            return (Json(true, JsonRequestBehavior.AllowGet));
        }
    }

}
