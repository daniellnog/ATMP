using LaefazWeb.Enumerators;
using LaefazWeb.Extensions;
using LaefazWeb.Models;
using LaefazWeb.Models.VOs;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TDMWeb.Extensions;
using TDMWeb.Lib;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class ExecucaoDeTesteController : Controller
    {
        //#if (DEBUG)
        //        public static string SqlConnectionString = @"Data Source=localhost;Database=TDM.Db;Trusted_Connection=true;Persist Security Info=True";
        //#else
        //        public static string SqlConnectionString = ConfigurationManager.ConnectionStrings["BulkInsert"].ConnectionString;
        //#endif

        private static LogTDM log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            ViewBag.Projeto = (from ct in db.PlanoTeste_TI
                               group ct by ct.PROJETO into g
                               orderby g.Key
                               select new listaVO { Descricao = g.Key, Id = g.Select(x => x.ID).Max() }
                               ).ToList();

            ViewBag.ListaAmbienteExec = new List<AmbienteExecucao>();
            ViewBag.ListaAmbienteVirt = new List<AmbienteVirtual>();
            ViewBag.ListaTipoFaseTeste = new List<TipoFaseTeste>();
            ViewBag.ListaScript = new List<Script>();
            ViewBag.ListaTestData = new List<TestData>();

            //ViewBag.Sistema = new List<listaVO>();
            //ViewBag.Pasta = new List<listaVO>();
            //ViewBag.PlanoDeTeste = new List<listaVO>();

            ViewBag.Motivo = db.MotivoExecucao.ToList();

            ViewBag.Sistema = new List<listaVO>();

            ViewBag.Pasta = new List<listaVO>();

            ViewBag.PlanoDeTeste = new List<listaVO>();

            ViewBag.ListaTipoFaseTeste = db.TipoFaseTeste.ToList();

            return View();
        }

        public JsonResult CarregarDadosScript(int IdPlanoTeste_TI)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);
            try
            {
                log.Info("entrada no try ");

                List<ListaScriptsVO> listaScripts = new List<ListaScriptsVO>();

                SqlParameter[] param =
                {
                    new SqlParameter("@IDPLANOTESTE", IdPlanoTeste_TI),
                };
                log.Info("Carregou os parametros");

                listaScripts = db.Database.SqlQuery<ListaScriptsVO>(
                        "EXEC PR_LISTAR_SCRIPTS_EXECUCAO @IDPLANOTESTE ", param).ToList();

                log.Info("Executou a proc");

                result = JsonConvert.SerializeObject(listaScripts, Formatting.Indented);

                //resultado = new { listaScript = listaScripts, mensagem = "Sucesso" };
            }
            catch (Exception ex)
            {
                result = JsonConvert.SerializeObject("Erro: " + ex.Message, Formatting.Indented);
                //return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarParametrosTestData(int IdTestData)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);

            TestData td = db.TestData.Where(x => x.Id == IdTestData).FirstOrDefault();

            List<ParametroValorVO> parametros = (from pv in db.ParametroValor
                                                 join ps in db.ParametroScript on pv.IdParametroScript equals ps.Id
                                                 join tp in db.TipoParametro on ps.IdTipoParametro equals tp.Id
                                                 join p in db.Parametro on ps.IdParametro equals p.Id
                                                 where pv.IdTestData == IdTestData
                                                 select new ParametroValorVO
                                                 {
                                                     IdParametroValor = pv.Id,
                                                     IdParametroScript = pv.IdParametroScript,
                                                     Valor = pv.Valor,
                                                     IdTestData = pv.IdTestData,
                                                     DescricaoTestData = td.Descricao,
                                                     DescricaoParametro = p.Descricao,
                                                     DescricaoTipoParametro = tp.Descricao,
                                                     IdParametroValor_Origem = pv.IdParametroValor_Origem,
                                                     IdTipoParametro = ps.IdTipoParametro,
                                                     Tipo = p.Tipo
                                                 }).ToList();

            return Json(parametros, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Play(int IdTestData, int IdFaseTeste, int IdMaquinaVirtual, int OpcaoTelegram, int IdAmbienteExecucao, bool PlayTestData = false)
        {
            try
            {
                Usuario user = (Usuario)Session["ObjUsuario"];
                //baixarEvidencia("http:///10.43.6.160:8081//PortalTDM//Evidencias//10_05_2018//SIEBEL%208-ETRG-TRG_MAIO_2018-10_05_2018_03_33_53.ZIP");
                string mensagem = "";
                var testDatas = new List<int>();
                List<ListaTestDatas> listaTestDatas = new List<ListaTestDatas>();
                int IdDatapool;
                int[] ids = null;
                //Verifica se o Play é da entidade DataPool ou da TestData
                if (PlayTestData)
                {
                    TestData TestData = db.TestData.FirstOrDefault(x => x.Id == IdTestData);
                    IdDatapool = db.DataPool.FirstOrDefault(x => x.Id == TestData.IdDataPool).Id;

                    listaTestDatas =
                        (from dp in db.DataPool
                         join td in db.TestData on dp.Id equals td.IdDataPool
                         where td.Id == IdTestData
                         select new ListaTestDatas
                         {
                             IdDatapool = dp.Id,
                             IdTestData = td.Id,
                             IdStatus = td.IdStatus
                         }).ToList();

                    if (listaTestDatas.Where(x => x.IdStatus != (int)EnumStatusTestData.Cadastrada).ToList().Count() > 0)
                        throw new Exception("Não é possível iniciar a execução de massas com o status diferente de CADASTRADA!");

                    List<ParametrosValores> listaParametrosObrigatorios =
                                            (from pv in db.ParametroValor
                                             join ps in db.ParametroScript on pv.IdParametroScript equals ps.Id
                                             join p in db.Parametro on ps.IdParametro equals p.Id
                                             where pv.IdTestData == IdTestData && ps.Obrigatorio == true
                                             select new ParametrosValores
                                             {
                                                 IdTestData = pv.IdTestData,
                                                 IdDatapool = IdDatapool,
                                                 IdParametro = p.Id,
                                                 IdParametroValor = pv.Id,
                                                 Descricao = p.Descricao,
                                                 Valor = pv.Valor,
                                                 Obrigatorio = ps.Obrigatorio
                                             }).ToList();

                    for (int w = 0; w < listaParametrosObrigatorios.Count; w++)
                    {
                        #region
                        log.Debug("Indice de repetição: " + w);
                        log.Debug("listaParametrosObrigatorios[w].IdParametro: " + listaParametrosObrigatorios[w].IdParametro);
                        log.Debug("idAmbienteExecucao: " + IdAmbienteExecucao);

                        #endregion

                        //Verifico se o Script tem o parametro Ambiente sistema, caso tenha, o valor do parametro é atualizado com o valor que vem da tela do play
                        if (listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Ambiente_Sistema || listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.Evidencia_Ambiente || listaParametrosObrigatorios[w].IdParametro == (int)EnumParametro.URL)
                        {
                            #region Debug
                            log.Debug("Entrada na condição Verifico se o Script tem o parametro Ambiente sistema, caso tenha, o valor do parametro é atualizado com o valor que vem da tela do play.");
                            #endregion

                            int idAmbExec = IdAmbienteExecucao;
                            int? _idParamValor = listaParametrosObrigatorios[w].IdParametroValor;

                            #region Debug
                            log.Debug("idAmbExec: " + idAmbExec);
                            log.Debug("_idParamValor: " + _idParamValor);
                            #endregion

                            if (_idParamValor != null)
                            {
                                ParametroValor pv = db.ParametroValor.Where(x => x.Id == _idParamValor).FirstOrDefault();

                                #region Debug
                                log.DebugObject(pv);
                                #endregion

                                AmbienteExecucao ambExec = db.AmbienteExecucao.Where(x => x.Id == idAmbExec).FirstOrDefault();

                                #region Debug 

                                log.DebugObject(ambExec);

                                log.Debug("idAmbExec: " + idAmbExec);
                                log.Debug("_idParamValor: " + _idParamValor);
                                #endregion

                                if (ambExec.Id == (int)EnumAmbienteExec.Ti1_Siebel8 || ambExec.Id == (int)EnumAmbienteExec.Ti8_Siebel8)
                                {
                                    string amb = ambExec.Descricao.Substring(ambExec.Descricao.IndexOf("http"), ambExec.Descricao.Length - ambExec.Descricao.IndexOf("http"));

                                    #region Debug 
                                    log.Debug("amb: " + amb);
                                    #endregion

                                    pv.Valor = amb;
                                    listaParametrosObrigatorios[w].Valor = amb;

                                    #region Debug 
                                    log.Debug("pv.Valor: " + pv.Valor);
                                    log.Debug("listaParametrosObrigatorios[w].Valor: " + listaParametrosObrigatorios[w].Valor);

                                    #endregion
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

                            #region Debug 
                            log.Debug("_idParamValor: " + _idParamValor);
                            log.DebugObject(pv);

                            #endregion

                            if (pv.Valor.Equals(""))
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
                                if (!testDatas.Contains(IdTestData))
                                {
                                    testDatas.Add(IdTestData);
                                }
                            }
                        }
                    }

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
                        mensagem = "O(s) TestData(s): abaixo possui(em) parâmetro(s) obrigatório(s) que não foram preenchidos <br>" + stringFinal;
                    }
                    else
                    {
                        bool EnvioTelegram = Convert.ToBoolean(OpcaoTelegram);
                        // Utilizando o Datapool da tela, substituir a query do script e salvar os dados na tabela de execução (Controle_Ambiente)

                        if (PlayTestData)
                            ReplaceQuery(IdTestData, IdFaseTeste, IdMaquinaVirtual, IdAmbienteExecucao, EnvioTelegram); // enviar o testData selecionado para execução
                        //else
                            //ReplaceQuery(ObtemIdTestData(Int32.Parse(id)), Int32.Parse(idFaseTeste), Int32.Parse(idMaquinaVirtual), Int32.Parse(idAmbienteExecucao), EnvioTelegram); // enviar o Datapool da tela

                        #region 
                        //Realizar execução através de requisição Jenkins

                        string pAginaDoJob = null;

                        AmbienteVirtual ambv = db.AmbienteVirtual.Where(x => x.Id == IdMaquinaVirtual).FirstOrDefault();

                        if (ambv.IP != null)
                        {
                            mensagem = "Execução iniciada com sucesso!";
                            pAginaDoJob = ConfigurationSettings.AppSettings[ambv.IP];
                        }
                        else
                        {
                            mensagem = "Não foi possível definir o Job do Jenkins.";
                        }

                        runJobJenkinsRemote(pAginaDoJob);
                        log.Info("Execução iniciada.");
                        #endregion
                        //Usar esta opção para rodar local
                        //runJobJenkinsLocal(pAginaDoJob, "brucilin.de.gouveia", "brucilin.de.gouveia");
                    }

                }
                else
                {

                    //IdDatapool = Int32.Parse(id);
                    //listaTestDatas =
                    //    (from dp in db.DataPool
                    //     join td in db.TestData on dp.Id equals td.IdDataPool
                    //     where dp.Id == IdDatapool
                    //     select new ListaTestDatas
                    //     {
                    //         IdDatapool = dp.Id,
                    //         IdTestData = td.Id
                    //     }).ToList();
                }



                return Json(new { Data = mensagem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Data = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        private void runJobJenkinsRemote(string url)
        {
            try
            {
                WebRequest wrIniciaJob;

                wrIniciaJob = WebRequest.Create(url);

                wrIniciaJob.Method = "POST";

                WebResponse response = wrIniciaJob.GetResponse();
                log.Info("ToString : " + response.ToString());
                log.Info("responseUri: " + response.ResponseUri);
                log.Info("hashcode: " + response.GetHashCode());
                response.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex);
            }

        }

        private void ReplaceQuery(int IdTestData, int idFaseTeste, int idMaquinaVirtual, int idAmbienteExecucao, bool EnvioTelegram)
        {
            Entities db = new Entities();
            
            //recuperando objeto testdata, para ter recuperar o IdScript_CondicaoScript
            TestData testData = db.TestData.Where(x => x.Id == IdTestData).FirstOrDefault();

            Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();

            string query = script_CondicaoScript.QueryTosca;

            String queryTemp = query.Replace("ptdTosca", testData.Id.ToString());

            Usuario user = (Usuario)Session["ObjUsuario"];
            Execucao exec = new Execucao();
            Script_CondicaoScript_Ambiente script_CondicaoScript_Ambiente = db.Script_CondicaoScript_Ambiente.Where(x => x.IdScript_CondicaoScript == testData.IdScript_CondicaoScript).Where(x => x.IdAmbienteVirtual == idMaquinaVirtual).Where(x => x.IdAmbienteExecucao == idAmbienteExecucao).FirstOrDefault();
            exec.IdScript_CondicaoScript_Ambiente = script_CondicaoScript_Ambiente.Id;
            exec.IdTipoFaseTeste = idFaseTeste; // pegar via campo popup modal play
            exec.IdStatusExecucao = (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento;
            exec.Usuario = user.Id.ToString();
            exec.IdTestData = testData.Id; // pegar o id via tela
            exec.SituacaoAmbiente = (int)Enumerators.EnumSituacaoAmbiente.EmUso;
            exec.ToscaInput = queryTemp;
            exec.EnvioTelegram = EnvioTelegram;
            db.Execucao.Add(exec);

            //Entities db1 = new Entities();
            //TestData td1 = db1.TestData.Where(x => x.Id == testData.Id).FirstOrDefault();
            TestData td1 = db.TestData.Where(x => x.Id == testData.Id).FirstOrDefault();
            td1.IdStatus = (int)Enumerators.EnumStatusTestData.EmGeracao;
            td1.GeradoPor = user.Login;

            db.TestData.Attach(td1);
            //db1.TestData.Attach(td1);

            db.Entry(td1).State = System.Data.Entity.EntityState.Modified;
            //db1.Entry(td1).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
                //db1.SaveChanges();
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }

        }

        public JsonResult CarregarAmbientes(int IdScript_CondicaoScript)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);
            try
            {
                var QueryAmbientes =
               (from av in db.AmbienteVirtual
                join sca in db.Script_CondicaoScript_Ambiente on av.Id equals sca.IdAmbienteVirtual
                join aexec in db.AmbienteExecucao on sca.IdAmbienteExecucao equals aexec.Id
                where sca.IdScript_CondicaoScript == IdScript_CondicaoScript
                select new AmbienteExecucao_Popup
                {
                    IdAmbienteVirtual = av.Id,
                    DescricaoAmbienteVirtual = av.Descricao,
                    IdAmbienteExecucao = aexec.Id,
                    DescricaoAmbienteExecucao = aexec.Descricao,
                    Disponivel = true

                }).ToList().Distinct();

                // Recuperando todos os ambientes execução que estão em uso
                var QueryAmbienteVirtualDisponivel =
                    (from exec in db.Execucao
                     where exec.SituacaoAmbiente == (int)Enumerators.EnumSituacaoAmbiente.EmUso
                     select exec.Script_CondicaoScript_Ambiente.AmbienteVirtual.Id).ToList();

                // Percorrendo todos os ambientes execução e virtual possíveis e atualizando o status (disponível)
                foreach (var item in QueryAmbientes)
                {
                    if (QueryAmbienteVirtualDisponivel.Contains(item.IdAmbienteVirtual))
                        item.Disponivel = false;
                }

                // Recuperando todos os ambientes que possuem status disponível
                ViewBag.ListaAmbienteVirt = QueryAmbientes.Where(i => i.Disponivel == true).Select(i => new { i.IdAmbienteVirtual, i.DescricaoAmbienteVirtual }).ToList().Distinct();
                ViewBag.ListaAmbienteExec = QueryAmbientes.Where(i => i.Disponivel == true).Select(i => new { i.IdAmbienteExecucao, i.DescricaoAmbienteExecucao }).ToList().Distinct();

                //string json = JsonConvert.SerializeObject(ViewBag.ListaAmbienteExec, Formatting.Indented);

                string jsonAmbExec = JsonConvert.SerializeObject(ViewBag.ListaAmbienteExec, Formatting.Indented);
                string jsonAmbVirtual = JsonConvert.SerializeObject(ViewBag.ListaAmbienteVirt, Formatting.Indented);

                var ambientes = new { ambexec = jsonAmbExec, ambvirtu = jsonAmbVirtual };

                result = JsonConvert.SerializeObject(ambientes, Formatting.Indented);
            }
            catch (Exception ex)
            {
                result = JsonConvert.SerializeObject("Erro: " + ex.Message, Formatting.Indented);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarTestDatas(int IdScript_CondicaoScript, int IdPlanoTeste_TI)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);
            try
            {
                PlanoTeste_TI ptTi = db.PlanoTeste_TI.FirstOrDefault(x => x.ID == IdPlanoTeste_TI);
                string Nome = ptTi.NOME.Replace("\r\n", " ");
                CT ct = db.CT.Where(x => x.Nome == ptTi.NOME.Replace("\r\n", " ") && x.Sistema == ptTi.SISTEMA && x.Fase == ptTi.CICLO).FirstOrDefault();

                List<TestDataCtVO> TestDataCtVo = new List<TestDataCtVO>();

                SqlParameter[] param =
                {
                    new SqlParameter("@IDCT", ct.Id),
                    new SqlParameter("@IDSCRIPTCONDICAOSCRIPT", IdScript_CondicaoScript),
                };

                TestDataCtVo = db.Database.SqlQuery<TestDataCtVO>(
                        "EXEC PR_LISTAR_TESTDATA_CT @IDCT ", param).ToList();

                log.Info("Executou a proc");
                result = JsonConvert.SerializeObject(TestDataCtVo, Formatting.Indented);
            }
            catch (Exception ex)
            {
                result = JsonConvert.SerializeObject("Erro: " + ex.Message, Formatting.Indented);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalvarLinha(string execAutomatizada, string idPlanoTeste_TI, string motivoExecucao = "", string nrOfensor = "", string descricaoScript = null, string plataforma = null)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);

            try
            {
                int IdPlanoTeste = Int32.Parse(idPlanoTeste_TI);
                int ExecAutoma = Int32.Parse(execAutomatizada);

                PlanoTeste_TI pt_ti = db.PlanoTeste_TI.Where(x => x.ID == IdPlanoTeste).FirstOrDefault();

                //encontrou algum registro para o script_condicaoScript, será UPDATE
                if (pt_ti.ID != 0)
                {
                    //execucao NÃO automatizada
                    if (ExecAutoma == 0)
                    {
                        pt_ti.ExecucaoAutomatizada_ATMP = false;

                        int idMotivo = Int32.Parse(motivoExecucao);
                        MotivoExecucao motivo = db.MotivoExecucao.Where(x => x.Id == idMotivo).FirstOrDefault();

                        pt_ti.MotivoExecucao_ATMP = motivo.Descricao;

                        //Preenchendo Número do Ofensor
                        if (nrOfensor != "")
                            pt_ti.OFENSOR_ATMP = nrOfensor;
                        else
                            pt_ti.OFENSOR_ATMP = null;

                        pt_ti.SCRIPT_RFT_ATMP = null;
                        pt_ti.SCRIPT_TOSCA_ATMP = null;

                        db.PlanoTeste_TI.Attach(pt_ti);

                        db.Entry(pt_ti).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();

                        result = JsonConvert.SerializeObject(pt_ti, Formatting.Indented);
                    }
                    //execução automatizada
                    else if (ExecAutoma == 1)
                    {
                        pt_ti.ExecucaoAutomatizada_ATMP = true;
                        pt_ti.MotivoExecucao_ATMP = null;
                        pt_ti.OFENSOR_ATMP = null;

                        if (plataforma.Equals("TOSCA"))
                        {
                            pt_ti.SCRIPT_TOSCA_ATMP = descricaoScript;
                            pt_ti.SCRIPT_RFT_ATMP = null;
                        }
                        else
                        {
                            pt_ti.SCRIPT_RFT_ATMP = descricaoScript;
                            pt_ti.SCRIPT_TOSCA_ATMP = null;
                        }

                        db.PlanoTeste_TI.Attach(pt_ti);

                        db.Entry(pt_ti).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();

                        result = JsonConvert.SerializeObject(pt_ti, Formatting.Indented);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("Exceção: " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarPasta(string projeto, string sistema)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);

            try
            {
                List<listaVO> pastas = (from ct in db.PlanoTeste_TI
                                        where ct.PROJETO == projeto && ct.SISTEMA == sistema
                                        group ct by ct.PASTA_LM into g
                                        orderby g.Key
                                        select new listaVO { Descricao = g.Key, Id = g.Select(x => x.ID).Max() }
                               ).ToList();

                List<listaVO> ListaPastas = new List<listaVO>();

                foreach (listaVO ob in pastas)
                {
                    if (ob.Descricao == null)
                    {
                        ob.Descricao = "";
                    }

                    listaVO obj = new listaVO
                    {
                        Id = ob.Id,
                        Descricao = ob.Descricao
                    };

                    int index = ListaPastas.FindIndex(x => x.Descricao.Equals(obj.Descricao, StringComparison.Ordinal));

                    if (index == -1)
                    {
                        ListaPastas.Add(obj);
                    }
                }

                result = JsonConvert.SerializeObject(pastas, Formatting.Indented);
            }
            catch (Exception ex)
            {
                log.Info("Exceção: " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarSistema(string projeto)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);

            try
            {
                List<listaVO> sistemas = (from ct in db.PlanoTeste_TI
                                          where ct.PROJETO == projeto
                                          group ct by ct.SISTEMA into g
                                          orderby g.Key
                                          select new listaVO { Descricao = g.Key, Id = g.Select(x => x.ID).Max() }
                                         ).ToList();

                log.Info("sistemas carregados: " + sistemas);

                List<listaVO> ListaSistemas = new List<listaVO>();

                foreach (listaVO ob in sistemas)
                {
                    listaVO obj = new listaVO
                    {
                        Id = ob.Id,
                        Descricao = ob.Descricao
                    };
                    int index = ListaSistemas.FindIndex(x => x.Descricao.Equals(obj.Descricao, StringComparison.Ordinal));

                    if (index == -1)
                    {
                        ListaSistemas.Add(obj);
                    }
                }

                result = JsonConvert.SerializeObject(sistemas, Formatting.Indented);
            }
            catch (Exception ex)
            {
                log.Info("Exceção: " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarPlanoTeste(string pasta)
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);

            try
            {
                int idLinha = Int32.Parse(pasta);

                PlanoTeste_TI objTemp = db.PlanoTeste_TI.Where(x => x.ID == idLinha).FirstOrDefault();

                List<listaVO> ListaPlanoTeste = new List<listaVO>();

                List<PlanoTeste_TI> listaPlanos = db.PlanoTeste_TI.Where(x => x.PROJETO == objTemp.PROJETO).Where(x => x.SISTEMA == objTemp.SISTEMA).Where(x => x.PASTA_LM == objTemp.PASTA_LM).OrderBy(x => x.PLANO_TESTE).ToList();

                foreach (PlanoTeste_TI pt_ti in listaPlanos)
                {
                    listaVO obj = new listaVO
                    {
                        Id = pt_ti.ID,
                        Descricao = pt_ti.PLANO_TESTE
                    };
                    int index = ListaPlanoTeste.FindIndex(x => x.Descricao.Equals(obj.Descricao, StringComparison.Ordinal));

                    if (index == -1)
                    {
                        ListaPlanoTeste.Add(obj);
                    }
                }
                //(from ct in db.PlanoTeste_TI
                //               where ct.PROJETO == objTemp.PROJETO && ct.SISTEMA == objTemp.SISTEMA && ct.PASTA_LM == objTemp.PASTA_LM
                //               group ct by ct.PASTA_LM into g
                //               select g
                //           ).ToList();

                result = JsonConvert.SerializeObject(ListaPlanoTeste, Formatting.Indented);
            }
            catch (Exception ex)
            {
                log.Info("Exceção: " + ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarDados(string projeto, string sistema = "", string pasta = "", string plano_teste = "")
        {
            string result = JsonConvert.SerializeObject(false, Formatting.Indented);
            log.Info("resultado setado como " + result);
            try
            {
                log.Info("entrada no try ");
                List<MotivoExecucao> motivos = db.MotivoExecucao.ToList();
                log.Info("Count Motivo execucao: " + motivos.Count());
                List<CTVO> CT_VOs = new List<CTVO>();

                int prj = Int32.Parse(projeto);
                int sist = Int32.Parse(sistema);
                int past = Int32.Parse(pasta);



                SqlParameter[] param =
                    {
                    new SqlParameter("@DisplayLength", 50),
                    new SqlParameter("@DisplayStart", "0"),
                    new SqlParameter("@SortCol", 7),
                    new SqlParameter("@SortDir", "desc"),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos", 1),
                    new SqlParameter("@IDPROJETO", prj),
                    new SqlParameter("@IDSISTEMA", sist),
                    new SqlParameter("@IDPASTA", past),
                    new SqlParameter("@IDPLANOTESTE", DBNull.Value),
                };
                log.Info("Carregou os parametros");


                if (!plano_teste.Equals("") && plano_teste != "null")
                    param[9].Value = Int32.Parse(plano_teste);

                log.Info("verificou se o plano de teste está null ou diferente de  vazio");

                CT_VOs = db.Database.SqlQuery<CTVO>(
                        "EXEC PR_LISTAR_EXECUCAO_TESTE @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @ListarTodos, @IDPROJETO, @IDSISTEMA, @IDPASTA, @IDPLANOTESTE ", param).ToList();
                log.Info("Executou a proc");

                List<CTVO> CT_VOs_Comp = new List<CTVO>();

                log.Info("criou lista temporaria");

                foreach (CTVO ctvoTemp in CT_VOs)
                {
                    log.Info("entrou no loop" + ctvoTemp);
                    CTVO ct_Temp = new CTVO
                    {
                        Plano_Teste = ctvoTemp.Plano_Teste,
                        NOME = ctvoTemp.NOME,
                        NRO_CENARIO = ctvoTemp.NRO_CENARIO,
                        NRO_CT = ctvoTemp.NRO_CT,
                        ExecucaoAutomatizada_ATMP = ctvoTemp.ExecucaoAutomatizada_ATMP,
                        IdPlanoTeste_TI = ctvoTemp.IdPlanoTeste_TI,
                        MotivoExecucao_ATMP = ctvoTemp.MotivoExecucao_ATMP,
                        Ofensor_ATMP = ctvoTemp.Ofensor_ATMP,
                        MotivosExecucoes = motivos,
                        SCRIPT_SELECIONADO = ctvoTemp.SCRIPT_SELECIONADO
                    };

                    //Declarando Lista de scripts para o CT
                    List<listaVO> scriptsCtList = new List<listaVO>();

                    //Capturando o campo string que retorna na PROC e populando a lista de objetos scripts no CTVO
                    if (ctvoTemp.LISTA_SCRIPTS != null)
                    {
                        // Pega a string que retornou da proc da posicao 1 em diante e quebra para iterar
                        ctvoTemp.LISTA_SCRIPTS.Substring(1).Split(';').ToList().ForEach(dadosScript =>
                        {

                            //Declarando objeto de Dados do Script
                            listaVO dadosScriptTemp = new listaVO
                            {
                                Plataforma = dadosScript.Split('|').First().Trim(),
                                Id = Int32.Parse(dadosScript.Split('|')[1].Trim()),
                                Descricao = dadosScript.Split('|').Last().Trim()
                            };

                            //Adicionando dados do script na Lista de scripts
                            scriptsCtList.Add(dadosScriptTemp);
                        });
                    }
                    //Atribuindo a Lista de scripts tratadas ao atributo do CTVO ListScripts
                    ct_Temp.ListaScripts = scriptsCtList;

                    log.Info("obeto carregado: " + ct_Temp);
                    CT_VOs_Comp.Add(ct_Temp);
                }
                log.Info("populou todos os CT's");
                result = JsonConvert.SerializeObject(CT_VOs_Comp, Formatting.Indented);
                log.Info("retornou com sucesso.");
            }
            catch (Exception ex)
            {
                log.Info("fim result" + ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarTestDataAssociados(int IdPlanoTeste_TI)
        {

            string result = JsonConvert.SerializeObject(false, Formatting.Indented);
            PlanoTeste_TI ptTi = db.PlanoTeste_TI.FirstOrDefault(x => x.ID == IdPlanoTeste_TI);
            string Nome = ptTi.NOME.Replace("\r\n", " ");
            CT ct = db.CT.Where(x => x.Nome == ptTi.NOME.Replace("\r\n", " ") && x.Sistema == ptTi.SISTEMA && x.Fase == ptTi.CICLO).FirstOrDefault();

            try
            {
                if (ct != null)
                {
                    List<TestDataCtVO> TestDataCtVo = new List<TestDataCtVO>();

                    SqlParameter[] param =
                                        {
                                            new SqlParameter("@IDCT", ct.Id),
                                        };

                    TestDataCtVo = db.Database.SqlQuery<TestDataCtVO>(
                            "EXEC PR_LISTAR_TESTDATA_CT @IDCT ", param).ToList();

                    result = JsonConvert.SerializeObject(TestDataCtVo, Formatting.Indented);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExcluirAssociacaoCT_TestData(int IdTestData_CT)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            try
            {
                TestData_CT td_ct = db.TestData_CT.Where(x => x.Id == IdTestData_CT).FirstOrDefault();
                if (td_ct != null)
                {
                    db.TestData_CT.Remove(td_ct);
                    db.SaveChanges();
                    log.Info("Associação excluída com sucesso.");
                    result.Data = new { Result = "Associação excluída com sucesso.", Status = (int)WebExceptionStatus.UnknownError };
                }
            }
            catch (Exception ex)
            {
                log.Error("Erro ao excluir a associação.");
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }

            return result;
        }

        public JsonResult ImportarArquivo()
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                var IdPlanoTeste_TI = Request.Form["IdPlanoTeste_TI"];
                var IdScript_CondicaoScript = Request.Form["scripts-idScript_CondicaoScript"];
                if (Request.Files.Count > 0)
                {
                    if (IdPlanoTeste_TI != "null" && IdScript_CondicaoScript != "null")
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                            file.SaveAs(path);

                            SalvarImportacao(path, Convert.ToInt32(IdPlanoTeste_TI), Convert.ToInt32(IdScript_CondicaoScript));
                            log.Info("Planilha importada: " + file.FileName + "IdUsuario: " + Util.GetUsuarioLogado().Id);
                            //Remove o arquivo
                            System.IO.File.Delete(path);

                            result.Data = new { Result = "Arquivo importado com sucesso.", Status = (int)WebExceptionStatus.Success };
                        }
                        else
                        {
                            result.Data = new { Result = "Arquivo informado está vazio.", Status = (int)WebExceptionStatus.UnknownError };
                        }
                    }
                    else
                    {
                        result.Data = new { Result = "Os campos 'Arquivo com Massa de Testes' e 'Script' são obrigatórios.", Status = (int)WebExceptionStatus.UnknownError };
                    }
                }
                else
                {
                    result.Data = new { Result = "Nenhum arquivo encontrado.", Status = (int)WebExceptionStatus.UnknownError };
                }

            }
            catch (Exception ex)
            {
                log.Error("Erro ao importar a planilha {IdUsuario: " + Util.GetUsuarioLogado().Id + "}", ex);
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }

            return result;
        }

        public void InserirDados(DataTable dr, DataPool pool, List<Coluna> ValidacaoColuna, CT ct)
        {
            if (dr != null && dr.Rows.Count > 0)
            {
                foreach (DataRow row in dr.Rows)
                {
                    Usuario user = (Usuario)Session["ObjUsuario"];
                    TestData test = new TestData();
                    test.IdStatus = (int)EnumStatusTestData.Cadastrada;
                    test.IdScript_CondicaoScript = pool.IdScript_CondicaoScript;
                    test.Descricao = pool.Descricao;
                    test.IdUsuario = user.Id;
                    test.ClassificacaoMassa = (int)EnumClassificacaoMassa.CT;
                    Script_CondicaoScript scs = db.Script_CondicaoScript.Where(x => x.Id == pool.IdScript_CondicaoScript).FirstOrDefault();
                    test.TempoEstimadoExecucao = scs.TempoEstimadoExecucao;

                    foreach (DataColumn column in dr.Columns)
                    {
                        if (column.Caption.Equals("Gerar antes de código migrado?"))
                        {
                            test.GerarMigracao = row[column.Caption].ToString().ToUpper() == "SIM" ? true : false;
                        }
                        else if (column.Caption.Equals("Caso de Teste"))
                        {
                            test.CasoTesteRelativo = row[column.Caption].ToString();
                        }
                        else if (column.Caption.Equals("Observações"))
                        {
                            test.Observacao = row[column.Caption].ToString();
                        }

                        if (ValidacaoColuna.Where(item => item.Parametro == column.Caption).FirstOrDefault() != null)
                            test.ParametroValor.Add(new ParametroValor { IdParametroScript = ValidacaoColuna.Where(item => item.Parametro == column.Caption).FirstOrDefault().IdParametroScript, Valor = row[column.Caption].ToString() });
                    }

                    pool.TestData.Add(test);

                    TestData_CT td_ct = new TestData_CT
                    {
                        IdCT = ct.Id,
                        IdTestData = test.Id
                    };
                    db.TestData_CT.Add(td_ct);

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        this.FlashError(ex.Message);
                    }
                }
            }
        }

        public JsonResult AlterarStatusTestData(string IdTestData, string IdStatus)
        {
            int idStatus = Int32.Parse(IdStatus);
            int testDataId = Int32.Parse(IdTestData);
            TestData testData = db.TestData.FirstOrDefault(a => a.Id == testDataId);

            var caminhoEvidencia = testData.CaminhoEvidencia;

            var result = new JsonResult
            {
                Data = caminhoEvidencia
            };

            if (idStatus == (int)EnumStatusTestData.Disponivel)
            {
                //alterando o status para UTILIZADA
                testData.IdStatus = (int)EnumStatusTestData.Utilizada;
            }
            // anexar objeto ao contexto
            db.TestData.Attach(testData);
            db.Entry(testData).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }
        private void SalvarImportacao(string path, int IdPlanoTeste_TI, int IdScript_CondicaoScript)
        {
            DataTable dt;

            PlanoTeste_TI pt_ti = db.PlanoTeste_TI.Where(x => x.ID == IdPlanoTeste_TI).FirstOrDefault();
            log.Info("PlatoTeste_TI carregado: " + pt_ti);

            CT ct = db.CT.Where(x => x.Nome == pt_ti.NOME).Where(x => x.Sistema == pt_ti.SISTEMA).Where(x => x.Fase == pt_ti.CICLO).FirstOrDefault();
            log.Info("CT carregado: " + ct);

            DataPool dp_tmp = db.DataPool.Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript).FirstOrDefault();

            if (!Util.ValidaPlanilhaExcelExecucao(path, dp_tmp))
            {
                throw new Exception("Esta planilha não corresponde ao DataPool selecionado, favor verificar os campos 'Sistema','Script' e 'Condição' na planilha.");
                //log.Info("A planilha não corresponde ao Datapool selecionado.");
            }

            List<Script_CondicaoScript_CT> scs_ct = db.Script_CondicaoScript_CT.Where(x => x.IdCT == ct.Id).Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript).ToList();
            log.Info("Script_CondicaoScript_CT carregado: " + scs_ct);

            Script_CondicaoScript scs_cons = db.Script_CondicaoScript.Where(x => x.Id == IdScript_CondicaoScript).FirstOrDefault();
            log.Info("Script_CondicaoScript carregado: " + scs_cons);

            TDM tdm = db.TDM.Where(x => x.Descricao == "TDM EXECUCAO DE TESTES").FirstOrDefault();
            log.Info("TDM carregado: " + tdm);

            if (tdm == null)
            {
                tdm = new TDM
                {
                    Descricao = "TDM EXECUCAO DE TESTES",
                    TdmPublico = false
                };
                db.TDM.Add(tdm);
                db.SaveChanges();
                log.Info("TDM criado: " + tdm);
            }

            Script s = db.Script.Where(x => x.Id == scs_cons.IdScript).FirstOrDefault();
            log.Info("Script carregado: " + s);

            string sistema = "";
            if (pt_ti.SISTEMA == "STC VOZ" || pt_ti.SISTEMA == "STC DADOS")
            {
                sistema = "STC";
            }
            else
            {
                sistema = pt_ti.SISTEMA;
            }

            AUT aut_tmp = db.AUT.Where(x => x.Descricao == sistema).FirstOrDefault();
            log.Info("AUT carregado: " + aut_tmp);

            Demanda d = db.Demanda.Where(x => x.Descricao == pt_ti.PROJETO).FirstOrDefault();
            log.Info("Demanda carregado: " + d);

            if (d == null)
            {
                d = new Demanda
                {
                    Descricao = pt_ti.PROJETO,
                };
                db.Demanda.Add(d);
                db.SaveChanges();
                log.Info("Demanda criada: " + d);
            }

            DataPool dp = db.DataPool.Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript).Where(x => x.IdTDM == tdm.Id).Where(x => x.IdAut == aut_tmp.Id).Where(x => x.IdDemanda == d.Id).FirstOrDefault();
            log.Info("DataPool carregado: " + dp);

            //Caso não haja datapool para o scs em questão, será criado um novo datapool
            #region
            if (dp == null)
            {
                dp = new DataPool
                {
                    IdTDM = tdm.Id,
                    IdAut = aut_tmp.Id,
                    IdDemanda = d.Id,
                    IdScript_CondicaoScript = scs_cons.Id,
                    Descricao = "DATAPOOL " + aut_tmp.Descricao + " - " + s.Descricao,
                    QtdSolicitada = 0,
                    DataSolicitacao = DateTime.Now,
                    DataInicioExecucao = null,
                    DataTermino = DateTime.Now.AddDays(7),
                    ConsiderarRotinaDiaria = false
                };
                db.DataPool.Add(dp);
                db.SaveChanges();

                log.Info("Datapool criado: " + dp);
            }
            #endregion

            //if (!Util.ValidaPlanilhaExcelExecucao(path, dp))
            //{
            //    throw new Exception("Esta planilha não corresponde ao DataPool selecionado, favor verificar os campos 'Sistema','Script' e 'Condição' na planilha.");
            //    log.Info("A planilha não corresponde ao Datapool selecionado.");
            //}

            dt = Util.LerExcel(path);

            List<DataPool> dps = new List<DataPool>();
            dps.Add(dp);

            List<Coluna> ValidacaoColuna = (from datapool in dps
                                            join scs in db.Script_CondicaoScript on datapool.IdScript_CondicaoScript equals scs.Id
                                            join paramscript in db.ParametroScript on scs.Id equals paramscript.IdScript_CondicaoScript
                                            join param in db.Parametro on paramscript.IdParametro equals param.Id
                                            where paramscript.IdTipoParametro == (int)EnumTipoParametroScript.Input
                                            select new Coluna
                                            {
                                                IdParametroScript = paramscript.Id,
                                                Parametro = param.Descricao,
                                                Tipo = param.Tipo,
                                                Obrigatorio = paramscript.Obrigatorio,
                                                Script_CondicaoScript = scs.IdCondicaoScript,
                                                DescricaoTestData = datapool.Descricao
                                            }).ToList();

            foreach (DataColumn column in dt.Columns)
            {
                var Registro = (from item in ValidacaoColuna
                                where item.Parametro == column.Caption
                                select new
                                {
                                    Nulo = item.Obrigatorio,
                                    Tipo = item.Tipo
                                }).FirstOrDefault();

                if (Registro != null)
                {
                    bool obrigatorio = bool.Parse(Registro.Nulo.ToString());

                    int contRow = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (obrigatorio && row[column.Caption].ToString().IsNullOrWhiteSpace())
                        {
                            throw new Exception("O Parâmetro '" + column.Caption + "' é obrigatório e não foi preenchido na linha " + contRow);
                        }
                        else if (Registro.Tipo == "NUMBER")
                        {
                            long NumeroValido;
                            bool retorno = Int64.TryParse(row[column.Caption].ToString(), out NumeroValido);

                            if (!retorno)
                            {

                                throw new Exception("A coluna '" + column.Caption + "' não está preenchida com valor numérico na linha " + contRow);
                            }
                        }
                        else if (Registro.Tipo == "DATE")
                        {
                            string Dateformat = "dd/MM/yyyy";
                            CultureInfo cult = new CultureInfo("pt-BR");
                            DateTime DataValida = DateTime.MinValue;

                            bool retorno = DateTime.TryParseExact(row[column.Caption].ToString().Substring(0, 10), Dateformat, cult, DateTimeStyles.None, out DataValida);

                            if (!retorno)
                            {
                                throw new Exception("A coluna '" + column.Caption + "' não está preenchida com data na linha " + contRow);
                            }
                        }
                        contRow++;
                    }
                }
            }

            if (dt != null)
            {
                InserirDados(dt, dp, ValidacaoColuna, ct);
            }
        }
    }
}
