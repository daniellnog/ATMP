using LaefazWeb.Extensions;
using LaefazWeb.Models;
using LaefazWeb.Models.VOs;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TDMWeb.Extensions;
using TDMWeb.Lib;
using TDMWeb.Models.VOs;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    [System.Runtime.InteropServices.Guid("A298738E-9B7C-4950-9201-041AA1506FCF")]
    public class EncadeamentoController : Controller
    {

        private static LogTDM log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DbEntities db = new DbEntities();

        private Usuario GetLoggedUser()
        {
            return (Usuario)Session["ObjUsuario"];
        }

        public ActionResult Index()
        {
            ViewBag.ListaAmbienteVirt = "";
            ViewBag.ListaTipoFaseTeste = db.TipoFaseTeste.ToList();

            return View();
        }

        public ActionResult Editar(int idEncadeamento)
        {
            Encadeamento enc = db.Encadeamento.Where(x => x.Id == idEncadeamento).FirstOrDefault();

            EncadeamentoVO encVo = new EncadeamentoVO();


            if (enc != null)
            {
                encVo.IdEncadeamento = enc.Id;
                encVo.Descricao = enc.Descricao;

                List<Encadeamento_TestData> enctds = db.Encadeamento_TestData.Where(x => x.IdEncadeamento == enc.Id).OrderByDescending(x => x.Ordem).ToList();

                encVo.qtdTds = enctds.Count();

                List<EncadeamentoVO.TestDataEncadeamento> testDatas = new List<EncadeamentoVO.TestDataEncadeamento>();

                foreach (Encadeamento_TestData enctd in enctds)
                {
                    TestData td = db.TestData.Where(x => x.Id == enctd.IdTestData).FirstOrDefault();
                    DataPool dp = db.DataPool.Where(x => x.Id == td.IdDataPool).FirstOrDefault();
                    AUT aut = db.AUT.Where(x => x.Id == dp.IdAut).FirstOrDefault();

                    List<ParametroScript> ListParametroScript = db.ParametroScript.Where(x => x.IdScript_CondicaoScript == td.IdScript_CondicaoScript).ToList();

                    List<EncadeamentoVO.TestDataEncadeamento.ParametroEncadeamento> parametros = new List<EncadeamentoVO.TestDataEncadeamento.ParametroEncadeamento>();

                    foreach (ParametroScript ps in ListParametroScript)
                    {
                        Parametro par = db.Parametro.Where(x => x.Id == ps.IdParametro).FirstOrDefault();
                        TipoParametro tp = db.TipoParametro.Where(x => x.Id == ps.IdTipoParametro).FirstOrDefault();
                        ParametroValor pv = db.ParametroValor.Where(x => x.IdParametroScript == ps.Id).Where(x => x.IdTestData == td.Id).FirstOrDefault();
                        EncadeamentoVO.TestDataEncadeamento.ParametroEncadeamento p = new EncadeamentoVO.TestDataEncadeamento.ParametroEncadeamento
                        {
                            Descricao = enc.Descricao,
                            DescricaoParametro = par.Descricao,
                            DescricaoTestData = td.Descricao,
                            DescricaoTipoParametro = tp.Descricao,
                            IdParametroScript = ps.Id,
                            IdParametroValor = pv == null ? (int?) null : pv.Id,
                            IdParametroValor_Origem = pv == null ? null : pv.IdParametroValor_Origem,
                            IdTestData = td.Id,
                            IdTipoParametro = tp.Id,
                            Obrigatorio = ps.Obrigatorio,
                            IdParametro = par.Id,
                            Tipo = tp.Descricao,
                            Valor = pv == null ? "" : pv.Valor
                        };
                        parametros.Add(p);
                    }

                    EncadeamentoVO.TestDataEncadeamento testData = new EncadeamentoVO.TestDataEncadeamento
                    {
                        Descricao = td.Descricao,
                        Id = enctd.Id,
                        IdTestData = td.Id,
                        Ordem = enctd.Ordem,
                        DescricaoAut = aut.Descricao,
                        parametros = parametros,
                        IdAmbienteExecucao = enctd.IdAmbienteExecucao,
                    };
                    testDatas.Add(testData);
                }
                encVo.testDatas = testDatas;
            }
            Usuario user = GetLoggedUser();

            ViewBag.listaTDMs = (from t in db.TDM
                                 join tdm in db.TDM_Usuario on t.Id equals tdm.IdTDM
                                 where tdm.IdUsuario == user.Id
                                 select new TDMVO
                                 {
                                     IdTestData = t.Id,
                                     Descricao = t.Descricao
                                 }
                                ).ToList();
            return View(encVo);
        }

        public JsonResult CarregarEncadeamento()
        {
            // get Start (paging start index) and length (page size for paging)
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string start = Request.Form.GetValues("start").FirstOrDefault();
            string length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            string sortColumn = Request.Form.GetValues("order[0][column]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int TotalRows = 0;

            SqlParameter[] param =
            {
                    new SqlParameter("@DisplayLength", length),
                    new SqlParameter("@DisplayStart", start),
                    new SqlParameter("@SortCol", sortColumn),
                    new SqlParameter("@SortDir", sortColumnDir),
                    new SqlParameter("@SEARCH", DBNull.Value),
                    new SqlParameter("@ListarTodos", 1),
                };

            List<EncadeamentoVO> EncadeamentosVo = db.Database.SqlQuery<EncadeamentoVO>("EXEC PR_CARREGAR_ENCADEAMENTOS @DisplayLength, @DisplayStart, @SortCol, @SortDir, @SEARCH, @ListarTodos ", param).ToList();

            if (EncadeamentosVo.Any())
                TotalRows = EncadeamentosVo.FirstOrDefault().TotalCount;

            return Json(new { draw = draw, recordsFiltered = TotalRows, recordsTotal = TotalRows, data = EncadeamentosVo }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult listarTestDatas(string datapool)
        {
            #region Debug
            log.Info("Entrada no método listarTestDatas do Encadeamento.");
            log.Info("Id do DataPool: " + datapool);
            #endregion

            int idDatapool = Int32.Parse(datapool);

            SqlParameter[] param =
            {
                new SqlParameter("@IdDatapool", idDatapool),
            };

            #region
            log.Info("Preparando a execução da PROC PR_LISTAR_TEST_DATA, passando o parâmtetro: " + idDatapool);
            #endregion
            //Executa a PROC que retorna os TestData
            List<TestDataVO> TestDataVOs = db.Database.SqlQuery<TestDataVO>("EXEC PR_LISTAR_TESTDATA_ENCADEAMENTO @IdDatapool ", param).ToList();

            #region
            log.Info("PROC 'PR_LISTAR_TESTDATA_ENCADEAMENTO' executada com sucesso e foram retornados " + TestDataVOs.Count() + " registros.");
            #endregion

            int qtdErr = 0;
            int qtdTds = 0;
            int qtdSuc = 0;
            int qtdEmGer = 0;
            int qtdCadas = 0;
            qtdTds = TestDataVOs.Count();
            DataPool dp = new DataPool();
            List<TestDataVO> tds = new List<TestDataVO>();
            AUT aut = new AUT();
            TestData tdn = new TestData();
            Execucao exec = new Execucao();

            for (int i = 0; i < TestDataVOs.Count(); i++)
            {
                int? idTemp = TestDataVOs[i].IdTestData;
                string descTD = TestDataVOs[i].Descricao;

                tdn = db.TestData.Where(x => x.Id == idTemp).FirstOrDefault();
                dp = db.DataPool.Where(x => x.Id == tdn.IdDataPool).FirstOrDefault();

                List<AmbienteExecucaoVO> amb_exec = (from scsa in db.Script_CondicaoScript_Ambiente
                                                     join av in db.AmbienteExecucao on scsa.IdAmbienteExecucao equals av.Id
                                                     join scs in db.Script_CondicaoScript on scsa.IdScript_CondicaoScript equals scs.Id
                                                     where scs.Id == dp.IdScript_CondicaoScript
                                                     select new AmbienteExecucaoVO
                                                     {
                                                         Id = scsa.IdAmbienteExecucao,
                                                         Descricao = av.Descricao
                                                     }).DistinctBy(x => x.Descricao).ToList();

                TestDataVOs[i].AmbientesExecs = amb_exec;

                List<ParametroValorVO> parametros = (from pv in db.ParametroValor
                                                     join ps in db.ParametroScript on pv.IdParametroScript equals ps.Id
                                                     join tp in db.TipoParametro on ps.IdTipoParametro equals tp.Id
                                                     join p in db.Parametro on ps.IdParametro equals p.Id
                                                     where pv.IdTestData == idTemp
                                                     select new ParametroValorVO
                                                     {
                                                         IdParametroValor = pv.Id,
                                                         IdParametroScript = pv.IdParametroScript,
                                                         Valor = pv.Valor,
                                                         IdTestData = pv.IdTestData,
                                                         DescricaoTestData = descTD,
                                                         DescricaoParametro = p.Descricao,
                                                         DescricaoTipoParametro = tp.Descricao,
                                                         IdParametroValor_Origem = pv.IdParametroValor_Origem,
                                                         IdTipoParametro = ps.IdTipoParametro,
                                                         Tipo = p.Tipo
                                                     }).ToList();

                List<ChaveVO> listaChaveVO = (from item in parametros
                                              select new ChaveVO
                                              {
                                                  value = string.Concat(item.DescricaoTestData, " - ", item.DescricaoParametro, " (", item.DescricaoTipoParametro, ") "),
                                                  key = item.IdParametroValor
                                              }).ToList();

                foreach (ParametroValorVO vo in parametros)
                {
                    string key = string.Concat(vo.DescricaoTestData, " - ", vo.DescricaoParametro, " (", vo.DescricaoTipoParametro, ") ");

                    Dictionary<string, int?> dicChaves = new Dictionary<string, int?>();
                    dicChaves.Add("ATMP", 0);
                    foreach (ChaveVO item in listaChaveVO)
                    {
                        dicChaves.Add(item.value, item.key);
                    }

                    if (!dicChaves.ContainsKey(key))
                    {
                        vo.DescricaoOrigem = dicChaves;
                    }
                    else
                    {
                        dicChaves.Remove(key);
                        vo.DescricaoOrigem = dicChaves;
                    }
                }

                #region
                log.Info("Preenchimento dos parametros do testData executado com sucesso e foram retornados " + parametros.Count() + " registros.");
                #endregion

                TestDataVOs[i].valores = parametros;


                aut = db.AUT.Where(x => x.Id == dp.IdAut).FirstOrDefault();
                exec = db.Execucao.Where(x => x.IdTestData == tdn.Id).FirstOrDefault();

                //string temp = tdn.TempoEstimadoExecucao.ToString();

                DateTime? tempInicial = null;
                DateTime? tempFim = null;
                int? IdStatusExec = null;

                if (!(exec == null))
                {
                    tempInicial = exec.InicioExecucao;
                    tempFim = exec.TerminoExecucao;
                    IdStatusExec = exec.IdStatusExecucao;
                }

                string caminho_evidencia = "";

                if (!(tdn.CaminhoEvidencia == null))
                {
                    caminho_evidencia = tdn.CaminhoEvidencia;
                }

                string temp = "";

                if (!(tempInicial == null || tempInicial.Equals("")) && !(tempFim == null || tempFim.Equals("")))
                {
                    temp = tempInicial + " - " + tempFim;
                }
                if (tdn.IdStatus == 4)
                {
                    qtdErr += 1;
                }
                else if (tdn.IdStatus == 3 || tdn.IdStatus == 5 || tdn.IdStatus == 6)
                {
                    qtdSuc += 1;
                }
                else if (tdn.IdStatus == 2)
                {
                    qtdEmGer += 1;
                }
                else if (tdn.IdStatus == 1)
                {
                    qtdCadas += 1;
                }

                TestDataVOs[i].tempoExecString = temp;
                TestDataVOs[i].CaminhoEvidencia = caminho_evidencia;
                TestDataVOs[i].idStatusExec = IdStatusExec;
                TestDataVOs[i].tempoExecString = temp;
                TestDataVOs[i].DescricaoAut = aut.Descricao;
                //tds.Add(new TestDataVO { Descricao = tdn.Descricao, IdStatus = tdn.IdStatus, DescricaoAut = aut.Descricao, tempoExecString = temp, CaminhoEvidencia = caminho_evidencia, IdTestData = tdn.Id, idStatusExec = IdStatusExec });
            }

            string json = JsonConvert.SerializeObject(TestDataVOs, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }

        public JsonResult listarDatapools(int tdm)
        {

            List<DataPoolVO> dp = (from datapool in db.DataPool
                                   where datapool.IdTDM == tdm
                                   select new DataPoolVO
                                   {
                                       Id = datapool.Id,
                                       DescricaoDataPool = datapool.Descricao
                                   }
                                   ).ToList();


            string json = JsonConvert.SerializeObject(dp, Formatting.Indented);

            return (Json(json, JsonRequestBehavior.AllowGet));
        }

        public ActionResult Adicionar()
        {
            Usuario user = GetLoggedUser();

            ViewBag.listaTDMs = (from t in db.TDM
                                 join tdm in db.TDM_Usuario on t.Id equals tdm.IdTDM
                                 where tdm.IdUsuario == user.Id
                                 select new TDMVO
                                 {
                                     IdTestData = t.Id,
                                     Descricao = t.Descricao
                                 }
                                ).ToList();

            return View();
        }

        public JsonResult Salvar(string encadeamentoJson)
        {
            EncadeamentoVO encadeamentoVO = JsonConvert.DeserializeObject<EncadeamentoVO>(encadeamentoJson);
            using (var context = new DbEntities())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        //edição de um encadeamento
                        #region
                        if (encadeamentoVO.IdEncadeamento != null)
                        {

                            log.Info("Entrou na edição do encadeamento " + encadeamentoVO.IdEncadeamento + ".");

                            Encadeamento enc = context.Encadeamento.Where(x => x.Id == encadeamentoVO.IdEncadeamento).FirstOrDefault();
                            enc.Descricao = encadeamentoVO.Descricao;
                            context.Encadeamento.Attach(enc);

                            context.Entry(enc).State = System.Data.Entity.EntityState.Modified;

                            context.SaveChanges();
                            log.Info("Alterando a descrição do encadeamento.");

                            List<EncadeamentoVO.TestDataEncadeamento> listaTdsBanco = (from entd in context.Encadeamento_TestData
                                                                                       where entd.IdEncadeamento == enc.Id
                                                                                       select new EncadeamentoVO.TestDataEncadeamento
                                                                                       {
                                                                                           Descricao = enc.Descricao,
                                                                                           Id = entd.Id,
                                                                                           Ordem = entd.Ordem,
                                                                                           IdTestData = entd.IdTestData,
                                                                                           IdAmbienteExecucao = entd.IdAmbienteExecucao,
                                                                                       }
                                                                                       ).ToList();

                            List<EncadeamentoVO.TestDataEncadeamento> listaObjTela = encadeamentoVO.testDatas;


                            for (int i = 0; i < listaObjTela.Count(); i++)
                            {
                                int idTemp = listaObjTela[i].Id;
                                int enca = enc.Id;
                                int idTestDataTemp = listaObjTela[i].IdTestData;
                                EncadeamentoVO.TestDataEncadeamento encTemp = listaTdsBanco.Where(x => x.Id == idTemp).FirstOrDefault();

                                if (encTemp == null)
                                {
                                    listaTdsBanco.Add(listaObjTela[i]);
                                    TestData tdAdd = context.TestData.Where(x => x.Id == idTestDataTemp).FirstOrDefault();
                                    Encadeamento_TestData encTdTemp = new Encadeamento_TestData
                                    {
                                        IdTestData = tdAdd.Id,
                                        IdEncadeamento = enca,
                                        Ordem = listaObjTela[i].Ordem,
                                        IdAmbienteExecucao = listaObjTela[i].IdAmbienteExecucao
                                    };
                                    context.Encadeamento_TestData.Add(encTdTemp);
                                    context.SaveChanges();
                                    log.Info("Associando o testData" + tdAdd.Id + " ao Encadeamento.");
                                }
                            }

                            for (int z = 0; z < listaTdsBanco.Count(); z++)
                            {
                                int idTemp = listaTdsBanco[z].Id;
                                EncadeamentoVO.TestDataEncadeamento encTemp = listaObjTela.Where(x => x.Id == idTemp).FirstOrDefault();

                                if (encTemp == null)
                                {
                                    listaTdsBanco.Remove(listaTdsBanco[z]);
                                    Encadeamento_TestData encDelete = context.Encadeamento_TestData.Where(x => x.Id == idTemp).FirstOrDefault();

                                    context.Encadeamento_TestData.Remove(encDelete);
                                    context.SaveChanges();
                                    log.Info("Desassociando o testData" + encDelete.IdTestData + " do Encadeamento.");
                                }
                            }

                            for (int w = 0; w < listaTdsBanco.Count(); w++)
                            {
                                int idTemp = listaTdsBanco[w].Id;

                                EncadeamentoVO.TestDataEncadeamento encTemp = listaObjTela.Where(x => x.Id == idTemp).FirstOrDefault();

                                if (encTemp != null)
                                {
                                    for (int y = 0; y < encTemp.parametros.Count(); y++)
                                    {
                                        int? idParametroScript = encTemp.parametros[y].IdParametroScript;
                                        int? idTestData = encTemp.parametros[y].IdTestData;
                                        ParametroValor pv = context.ParametroValor.Where(x => x.IdParametroScript == idParametroScript).Where(x => x.IdTestData == idTestData).FirstOrDefault();

                                        if (pv != null)
                                        {
                                            pv.Valor = encTemp.parametros[y].Valor;
                                            pv.IdParametroValor_Origem = encTemp.parametros[y].IdParametroValor_Origem;

                                            context.ParametroValor.Attach(pv);

                                            context.Entry(pv).State = System.Data.Entity.EntityState.Modified;

                                            context.SaveChanges();
                                            log.Info("Atualizando o ParametroValor " + pv.Id + ", do ParametroScript " + idParametroScript + " e do TestData " + idTestData + ".");
                                        }
                                    }
                                }
                            }

                            string json = JsonConvert.SerializeObject(true, Formatting.Indented);

                            dbContextTransaction.Commit();
                            log.Info("Salvando a edição do encadeamento " + encadeamentoVO.IdEncadeamento);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                        //novo encadeamento
                        #region
                        else
                        {
                            //adicionando Encadeamento
                            Encadeamento encadeamento = new Encadeamento { Descricao = encadeamentoVO.Descricao };
                            context.Encadeamento.Add(encadeamento);
                            context.SaveChanges();
                            log.Info("Inclusão do encadeamento " + encadeamento.Id);

                            foreach (EncadeamentoVO.TestDataEncadeamento testDataEncadeamento in encadeamentoVO.testDatas)
                            {
                                //criando relacao testdata encadeamento
                                Encadeamento_TestData encadeamentoTestData = new Encadeamento_TestData
                                {
                                    IdEncadeamento = encadeamento.Id,
                                    IdTestData = testDataEncadeamento.IdTestData,
                                    Ordem = testDataEncadeamento.Ordem,
                                    IdAmbienteExecucao = testDataEncadeamento.IdAmbienteExecucao
                                };

                                context.Encadeamento_TestData.Add(encadeamentoTestData);
                                context.SaveChanges();
                                log.Info("Associando o testData " + testDataEncadeamento.IdTestData + " ao Encadeamento " + encadeamento.Id);

                                //populando a tabela de parametro valor
                                foreach (EncadeamentoVO.TestDataEncadeamento.ParametroEncadeamento parametrosEncadeamento in testDataEncadeamento.parametros)
                                {

                                    ParametroValor pv = context.ParametroValor.Where(x => x.Id == parametrosEncadeamento.IdParametroValor).FirstOrDefault();

                                    pv.Valor = parametrosEncadeamento.Valor;

                                    context.ParametroValor.Attach(pv);

                                    context.Entry(pv).State = System.Data.Entity.EntityState.Modified;

                                    context.SaveChanges();
                                    log.Info("Salvando o ParametroValor " + pv.Id + " do TestData " + testDataEncadeamento.IdTestData);
                                }

                            }
                            string json = JsonConvert.SerializeObject(true, Formatting.Indented);

                            dbContextTransaction.Commit();
                            log.Info("Salvando o Encadeamento " + encadeamento.Id);
                            return Json(json, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string json = JsonConvert.SerializeObject(false, Formatting.Indented);
                        dbContextTransaction.Rollback();
                        return Json(json, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        public ActionResult AcompanharExecucoes()
        {

            List<Encadeamento> encs = (from enc in db.Encadeamento
                                           //join enc_td in db.Encadeamento_TestData on enc.Id equals enc_td.IdEncadeamento
                                           //join td in db.TestData on enc_td.IdTestData equals td.Id
                                           //join exec in db.Execucao on td.IdExecucao equals exec.Id
                                       select enc
                                        ).Distinct().ToList();

            List<Encadeamento_TestDataVO> encad_testDatas = new List<Encadeamento_TestDataVO>();

            foreach (Encadeamento enc in encs)
            {
                List<Encadeamento_TestData> encTDs = (from td in db.Encadeamento_TestData
                                                      where td.IdEncadeamento == enc.Id
                                                      select td
                                                      ).ToList();
                int qtdErr = 0;
                int qtdTds = 0;
                int qtdSuc = 0;
                int qtdEmGer = 0;
                qtdTds = encTDs.Count();
                DataPool dp = new DataPool();
                List<TestDataVO> tds = new List<TestDataVO>();
                AUT aut = new AUT();
                TestData tdn = new TestData();
                Execucao exec = new Execucao();

                foreach (Encadeamento_TestData encTd in encTDs)
                {
                    tdn = db.TestData.Where(x => x.Id == encTd.IdTestData).FirstOrDefault();
                    dp = db.DataPool.Where(x => x.Id == tdn.IdDataPool).FirstOrDefault();
                    aut = db.AUT.Where(x => x.Id == dp.IdAut).FirstOrDefault();
                    exec = db.Execucao.Where(x => x.IdTestData == tdn.Id).FirstOrDefault();

                    //string temp = tdn.TempoEstimadoExecucao.ToString();

                    DateTime? tempInicial = null;
                    DateTime? tempFim = null;
                    int? IdStatusExec = null;

                    if (!(exec == null))
                    {
                        tempInicial = exec.InicioExecucao;
                        tempFim = exec.TerminoExecucao;
                        IdStatusExec = exec.IdStatusExecucao;
                    }

                    string caminho_evidencia = "";

                    if (!(tdn.CaminhoEvidencia == null))
                    {
                        caminho_evidencia = tdn.CaminhoEvidencia;
                    }

                    string temp = "";

                    if (!(tempInicial == null || tempInicial.Equals("")) && !(tempFim == null || tempFim.Equals("")))
                    {
                        temp = tempInicial + " - " + tempFim;
                    }
                    if (tdn.IdStatus == 4)
                    {
                        qtdErr += 1;
                    }
                    else if (tdn.IdStatus == 3 || tdn.IdStatus == 5 || tdn.IdStatus == 6)
                    {
                        qtdSuc += 1;
                    }
                    else if (tdn.IdStatus == 2)
                    {
                        qtdEmGer += 1;
                    }
                    tds.Add(new TestDataVO { Descricao = tdn.Descricao, IdStatus = tdn.IdStatus, DescricaoAut = aut.Descricao, tempoExecString = temp, CaminhoEvidencia = caminho_evidencia, IdTestData = tdn.Id, idStatusExec = IdStatusExec });
                }

                encad_testDatas.Add(new Encadeamento_TestDataVO { Descricao = enc.Descricao, Id = enc.Id, testDatas = tds, qtdErros = qtdErr, qtdTDs = qtdTds, qtdSuccess = qtdSuc, qtdEmGeracao = qtdEmGer });
            }
            return View(encad_testDatas);
        }

        public JsonResult CarregarTestDatas(string idEncadeamento)
        {
            DbEntities db = new DbEntities();
            int IdEncadeamento = Int32.Parse(idEncadeamento);

            List<Encadeamento_TestData> listaEncadeamento_TestDatas = db.Encadeamento_TestData.Where(x => x.IdEncadeamento == IdEncadeamento).ToList();

            List<TestDataVO> listaTestDatasVO = new List<TestDataVO>();

            foreach (Encadeamento_TestData etd in listaEncadeamento_TestDatas)
            {
                TestData td = db.TestData.Where(y => y.Id == etd.IdTestData).First();
                AUT a = db.AUT.Where(w => w.Id == td.DataPool.IdAut).First();
                DataPool dp = (from d in db.DataPool
                               where d.Id == td.IdDataPool
                               select d
                              ).First();
                AUT daut = (from au in db.AUT
                            where au.Id == a.Id
                            select au
                              ).First();
                Status stat = (from s in db.Status
                               where s.Id == td.IdStatus
                               select s
                              ).First();


                listaTestDatasVO.Add(new TestDataVO
                {
                    IdTestData = td.Id,
                    CaminhoEvidencia = td.CaminhoEvidencia,
                    DescricaoDataPool = dp.Descricao,
                    DescricaoAut = daut.Descricao,
                    CasoTesteRelativo = td.CasoTesteRelativo,
                    Descricao = td.Descricao,
                    DataGeracao = td.DataGeracao,
                    DescricaoStatus = stat.Descricao,
                    Ordem = etd.Ordem,
                    OrdemEncadeamento = etd.Ordem
                }
                );
            }

            string json = JsonConvert.SerializeObject(listaTestDatasVO, Formatting.Indented);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDadosModalPlay(string id)
        {
            int IdEncadeamento = Int32.Parse(id);

            List<Encadeamento_TestData> EncadeamentoTestDataList = db.Encadeamento_TestData.Where(x => x.IdEncadeamento == IdEncadeamento).ToList();

            List<int?> IdsAmbienteExecucao = new List<int?>();

            EncadeamentoTestDataList.ForEach(element =>
            {
                IdsAmbienteExecucao.Add(element.IdAmbienteExecucao);
            });

            DataPool datapool = db.DataPool.Find(Int32.Parse(id));

            //Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.Find(datapool.IdScript_CondicaoScript);

            // Recuperando todos os ambientes execução e virtual possível para o Script+Condicao da tela
            var QueryAmbientes =
               (from av in db.AmbienteVirtual
                join sca in db.Script_CondicaoScript_Ambiente on av.Id equals sca.IdAmbienteVirtual
                join aexec in db.AmbienteExecucao on sca.IdAmbienteExecucao equals aexec.Id
                //where sca.IdScript_CondicaoScript == script_CondicaoScript.Id // pegar id da tela
                where IdsAmbienteExecucao.Contains(sca.IdScript_CondicaoScript)
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
            //ViewBag.ListaAmbienteExec = QueryAmbientes.Where(i => i.Disponivel == true).Select(i => new { i.IdAmbienteExecucao, i.DescricaoAmbienteExecucao }).ToList().Distinct();

            //string json = JsonConvert.SerializeObject(ViewBag.ListaAmbienteExec, Formatting.Indented);

            //string jsonAmbExec = JsonConvert.SerializeObject(ViewBag.ListaAmbienteExec, Formatting.Indented);
            string jsonAmbVirtual = JsonConvert.SerializeObject(ViewBag.ListaAmbienteVirt, Formatting.Indented);

            var ambientes = new { ambvirtu = jsonAmbVirtual };

            return Json(ambientes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Play(string Ids, string IdFaseTeste, string IdMaquinaVirtual, string NotificacaoTelegram)
        {
            #region Debug
            log.Info("Entrada no método Play de encadeamento.");
            log.Info("Parâmetros passados no método Play():");
            log.Debug("Ids: " + Ids);
            log.Debug("IdFaseTeste: " + IdFaseTeste);
            log.Debug("IdMaquinaVirtual: " + IdMaquinaVirtual);
            log.Debug("NotificacaoTelegram: " + NotificacaoTelegram);
            #endregion

            try
            {
                Char delimiter = ';';
                string mensagem = "";
                string[] idEncadeamentos = Ids.Split(delimiter);
                int idFaseTeste = Int32.Parse(IdFaseTeste);
                int idMaquinaVirtual = Int32.Parse(IdMaquinaVirtual);
                bool EnvioTelegram = bool.Parse(NotificacaoTelegram);

                #region Debug
                log.Info("Foram identificados " + idEncadeamentos.Length + " encadeamentos para execução.");
                #endregion

                foreach (string encadeamento in idEncadeamentos)
                {
                    #region Debug
                    log.Info("Preparando para processar a execução do encadeamento de Id " + encadeamento + ".");
                    #endregion

                    int idEncadeamento = Int32.Parse(encadeamento);

                    DbEntities db = new DbEntities();
                    List<Encadeamento_TestData> EncadeamentoTdList = db.Encadeamento_TestData.Where(x => x.IdEncadeamento == idEncadeamento).OrderBy(x => x.Ordem).ToList();

                    ReplaceQuery(EncadeamentoTdList, idFaseTeste, idMaquinaVirtual, EnvioTelegram);

                }


                string pAginaDoJob = null;
                int idAmbv = idMaquinaVirtual;
                AmbienteVirtual ambv = db.AmbienteVirtual.Where(x => x.Id == idAmbv).FirstOrDefault();

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



                return Json(new { Data = mensagem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Data = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ExecutaEncadeamento(string qtd)
        {
            int idFaseTeste = 1;
            int idMaquinaVirtual = 1;
            bool EnvioTelegram = false;
            int idAmbienteExecucao = 1;
            string mensagem = "";

            try
            {
                #region

                List<int> idsTestData = new List<int>();
                List<int> idsTestData2 = new List<int>();
                List<Encadeamento> encadeamentos = new List<Encadeamento>();
                Random rnd = new Random();


                //criar o encadeamento na tabela
                if (qtd.Equals("1"))
                {

                    Encadeamento encadeamento = new Encadeamento { Descricao = "Encadeamento " + rnd.Next(1, 100).ToString() };

                    db.Encadeamento.Add(encadeamento);
                    db.SaveChanges();

                    encadeamentos.Add(encadeamento);

                }
                else
                {

                    for (int i = 0; i < 2; i++)
                    {
                        Encadeamento encadeamento = new Encadeamento { Descricao = "Encadeamento " + rnd.Next(1, 100).ToString() };

                        db.Encadeamento.Add(encadeamento);
                        db.SaveChanges();

                        encadeamentos.Add(encadeamento);
                    }

                }


                if (encadeamentos.Count == 1)
                {

                    string ids = ConfigurationSettings.AppSettings["Encadeamento1"];
                    ids.Split(',').ToList().ForEach(item => { idsTestData.Add(Int32.Parse(item)); });

                    //idsTestData.Add(1);
                    //idsTestData.Add(2);
                    //idsTestData.Add(3);

                    for (int i = 0; i < idsTestData.Count; i++)
                    {
                        Encadeamento_TestData encadeamentoTestData = new Encadeamento_TestData
                        {
                            IdEncadeamento = encadeamentos[0].Id,
                            IdTestData = idsTestData[i],
                            Ordem = (i + 1)
                        };

                        db.Encadeamento_TestData.Add(encadeamentoTestData);
                        db.SaveChanges();
                    }

                    ReplaceQuery(idsTestData, idFaseTeste, idMaquinaVirtual, idAmbienteExecucao, EnvioTelegram);


                }
                else
                {

                    string ids = ConfigurationSettings.AppSettings["Encadeamento1"];
                    ids.Split(',').ToList().ForEach(item => { idsTestData.Add(Int32.Parse(item)); });

                    ids = ConfigurationSettings.AppSettings["Encadeamento2"];
                    ids.Split(',').ToList().ForEach(item => { idsTestData2.Add(Int32.Parse(item)); });

                    //idsTestData.Add(1);
                    //idsTestData.Add(2);
                    //idsTestData.Add(3);

                    //idsTestData2.Add(11);
                    //idsTestData2.Add(12);

                    for (int i = 0; i < idsTestData.Count(); i++)
                    {
                        Encadeamento_TestData encadeamentoTestData = new Encadeamento_TestData
                        {
                            IdEncadeamento = encadeamentos[0].Id,
                            IdTestData = idsTestData[i],
                            Ordem = (i + 1)
                        };

                        db.Encadeamento_TestData.Add(encadeamentoTestData);
                        db.SaveChanges();


                    }

                    for (int i = 0; i < idsTestData2.Count(); i++)
                    {
                        Encadeamento_TestData encadeamentoTestData = new Encadeamento_TestData
                        {
                            IdEncadeamento = encadeamentos[1].Id,
                            IdTestData = idsTestData2[i],
                            Ordem = (i + 1)
                        };

                        db.Encadeamento_TestData.Add(encadeamentoTestData);
                        db.SaveChanges();

                    }

                    ReplaceQuery(idsTestData, idFaseTeste, idMaquinaVirtual, idAmbienteExecucao, EnvioTelegram);
                    ReplaceQuery(idsTestData2, idFaseTeste, idMaquinaVirtual, idAmbienteExecucao, EnvioTelegram);

                }



                #endregion


                string pAginaDoJob = null;
                int idAmbv = idMaquinaVirtual;
                AmbienteVirtual ambv = db.AmbienteVirtual.Where(x => x.Id == idAmbv).FirstOrDefault();

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



                return Json(new { Data = mensagem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Data = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        private void ReplaceQuery(List<int> ids, int idFaseTeste, int idMaquinaVirtual, int idAmbienteExecucao, bool EnvioTelegram)
        {

            Entities db = new Entities();
            // validar ambiente disponível e setar a flag em uso antes de inserir a query

            string testdataList = "";

            foreach (var item in ids)
            {
                //recuperando objeto testdata, para ter recuperar o IdScript_CondicaoScript
                TestData testData = db.TestData.Find(item);

                Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();

                string query = script_CondicaoScript.QueryTosca;

                String queryTemp = query.Replace("ptdTosca", item.ToString());

                Usuario user = GetLoggedUser();
                Execucao exec = new Execucao();
                Script_CondicaoScript_Ambiente script_CondicaoScript_Ambiente =

                db.Script_CondicaoScript_Ambiente
                .Where(x => x.IdScript_CondicaoScript == testData.IdScript_CondicaoScript)
                .Where(x => x.IdAmbienteVirtual == idMaquinaVirtual)
                .Where(x => x.IdAmbienteExecucao == idAmbienteExecucao).FirstOrDefault();

                exec.IdScript_CondicaoScript_Ambiente = script_CondicaoScript_Ambiente.Id;
                exec.IdTipoFaseTeste = idFaseTeste; // pegar via campo popup modal play
                exec.IdStatusExecucao = (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento;
                exec.Usuario = user.Id.ToString();
                exec.IdTestData = item; // pegar o id via tela
                exec.SituacaoAmbiente = (int)Enumerators.EnumSituacaoAmbiente.EmUso;
                exec.ToscaInput = queryTemp;
                exec.EnvioTelegram = false;
                db.Execucao.Add(exec);

                Entities db1 = new Entities();
                TestData td1 = db1.TestData.Where(x => x.Id == item).FirstOrDefault();
                td1.IdStatus = (int)Enumerators.EnumStatusTestData.EmGeracao;
                td1.GeradoPor = Util.GetUsuarioLogado().Login;
                db1.TestData.Attach(td1);

                db1.Entry(td1).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    db1.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.FlashError(ex.Message);
                }

            }


        }

        private void ReplaceQuery(List<Encadeamento_TestData> EncadeamentoTdList, int idFaseTeste, int idMaquinaVirtual, bool EnvioTelegram)
        {

            Entities db = new Entities();
            // validar ambiente disponível e setar a flag em uso antes de inserir a query

            string testdataList = "";

            foreach (var Encadeamento_TestData in EncadeamentoTdList)
            {
                //recuperando objeto testdata, para ter recuperar o IdScript_CondicaoScript
                TestData testData = db.TestData.FirstOrDefault(x => x.Id == Encadeamento_TestData.IdTestData);

                Script_CondicaoScript script_CondicaoScript = db.Script_CondicaoScript.Where(x => x.Id == testData.IdScript_CondicaoScript).FirstOrDefault();

                string query = script_CondicaoScript.QueryTosca;

                String queryTemp = query.Replace("ptdTosca", Encadeamento_TestData.IdTestData.ToString());

                Usuario user = GetLoggedUser();
                Execucao exec = new Execucao();
                Script_CondicaoScript_Ambiente script_CondicaoScript_Ambiente =

                db.Script_CondicaoScript_Ambiente
                .Where(x => x.IdScript_CondicaoScript == testData.IdScript_CondicaoScript)
                .Where(x => x.IdAmbienteVirtual == idMaquinaVirtual)
                .Where(x => x.IdAmbienteExecucao == Encadeamento_TestData.IdAmbienteExecucao).FirstOrDefault();

                exec.IdScript_CondicaoScript_Ambiente = script_CondicaoScript_Ambiente.Id;
                exec.IdTipoFaseTeste = idFaseTeste; // pegar via campo popup modal play
                exec.IdStatusExecucao = (int)Enumerators.EnumStatusExecucao.AguardandoProcessamento;
                exec.Usuario = user.Id.ToString();
                exec.IdTestData = Encadeamento_TestData.IdTestData; // pegar o id via tela
                exec.SituacaoAmbiente = (int)Enumerators.EnumSituacaoAmbiente.EmUso;
                exec.ToscaInput = queryTemp;
                exec.EnvioTelegram = false;
                db.Execucao.Add(exec);

                Entities db1 = new Entities();
                TestData td1 = db1.TestData.Where(x => x.Id == Encadeamento_TestData.IdTestData).FirstOrDefault();
                td1.IdStatus = (int)Enumerators.EnumStatusTestData.EmGeracao;
                td1.GeradoPor = Util.GetUsuarioLogado().Login;
                db1.TestData.Attach(td1);

                db1.Entry(td1).State = System.Data.Entity.EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    db1.SaveChanges();
                }
                catch (Exception ex)
                {
                    this.FlashError(ex.Message);
                }

            }


        }

        private List<int> ObtemIdTestData(int id_datapool)
        {

            // Instancia a Entity
            Entities db = new Entities();

            // Query para buscar o id do TestData,passando o id do Datapool que será executado.
            var tdQuery =
            (from td1 in db.TestData
             join dtp in db.DataPool on td1.IdDataPool equals dtp.Id
             where dtp.Id == id_datapool && td1.IdStatus == 1
             select td1.Id).ToList();

            return tdQuery;
        }

        private void runJobJenkinsRemote(string url)
        {
            WebRequest wrIniciaJob;

            wrIniciaJob = WebRequest.Create(url);

            wrIniciaJob.Method = "POST";

            WebResponse response = wrIniciaJob.GetResponse();

            response.Close();
        }

        public ActionResult Remover(IList<string> ids)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            string json = JsonConvert.SerializeObject(false, Formatting.Indented);
            string message = String.Empty;

            foreach (string id in ids)
            {
                int Id = Int32.Parse(id);

                using (var context = new DbEntities())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {

                        try
                        {
                            List<Encadeamento_TestData> encadTd = context.Encadeamento_TestData.Where(x => x.IdEncadeamento == Id).ToList();

                            encadTd.ForEach(encadTds =>
                            {
                                context.Encadeamento_TestData.Remove(encadTds);
                                context.SaveChanges();
                            });

                            Encadeamento encadeamento = context.Encadeamento.FirstOrDefault(x => x.Id == Id);
                            context.Encadeamento.Remove(encadeamento);
                            context.SaveChanges();

                            dbContextTransaction.Commit();

                            message = "Encadeamento excluido com sucesso!";

                            json = JsonConvert.SerializeObject(message, Formatting.Indented);
                            result = Json(json, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();

                            if (ex.Message.Contains("FK_"))
                                message = "O encadeamento não pode ser excluído pois possui relacionamento com outras entidades.";
                            else
                                message = ex.Message;

                            json = JsonConvert.SerializeObject(message, Formatting.Indented);
                            result = Json(json, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

            }

            return result;
        }

    }
}