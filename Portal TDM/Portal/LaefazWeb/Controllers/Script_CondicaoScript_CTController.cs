using LaefazWeb.Extensions;
using LaefazWeb.Models;
using LaefazWeb.Models.VOs;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TDMWeb.Extensions;
using TDMWeb.Lib;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class Script_CondicaoScript_CTController : Controller
    {
        private DbEntities db = new DbEntities();
        private static LogTDM log = new LogTDM(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {

            #region Debug
            log.Debug("Entrada no método Index...");
            #endregion Debug

            List<Script_CondicaoScript_CT_VO> Cts = new List<Script_CondicaoScript_CT_VO>();
            List<Script_CondicaoScriptVO> scripts = new List<Script_CondicaoScriptVO>();
            #region Debug
            log.Debug("Inicializada a lista de CTs...");
            log.Debug("Executando PROC na base...");
            #endregion Debug
            ViewBag.ListaFaseCT = db.CT.DistinctBy(x => x.Fase).ToList();
            ViewBag.ListCTs = db.CT.ToList();

            SqlParameter[] param =
                {
                    new SqlParameter("@ID_AMBIENTE_EXECUCAO", DBNull.Value),
                    new SqlParameter("@ID_AUT", DBNull.Value)
                };

            scripts = db.Database.SqlQuery<Script_CondicaoScriptVO>("EXEC PR_LISTAR_SCRIPT_CONDICAO_SCRIPT @ID_AMBIENTE_EXECUCAO, @ID_AUT", param).ToList();

            List<listaVO> listaVOs = new List<listaVO>();
            for (int i = 0; i < scripts.Count; i++)
            {
                string desc = scripts[i].DescricaoCondicaoScript == null ? scripts[i].DescricaoScript : scripts[i].DescricaoScript + " - " + scripts[i].DescricaoCondicaoScript;

                listaVO obj = new listaVO
                {
                    Id = scripts[i].Id,
                    Descricao = desc
                };

                listaVOs.Add(obj);
            }

            ViewBag.ListScripts = listaVOs;
            ViewBag.ListaSistemas = db.AUT.ToList();
            ViewBag.ListaAmbienteExecucao = db.AmbienteExecucao.ToList();

            Cts = db.Database.SqlQuery<Script_CondicaoScript_CT_VO>("EXEC PR_LISTAR_SCRIPT_CONDICAO_SCRIPT_CT ").ToList();

            #region Debug
            log.Debug("PROC executada com sucesso, foram retornados " + Cts.Count() + " registros.");
            #endregion Debug

            return View(Cts);
        }

        public ActionResult Salvar(int IdCt, int IdScript_CondicaoScript)
        {

            #region Debug
            log.Info("Entrada no método Salvar!");
            log.Debug("Parâmetros passados: " + Environment.NewLine + "Id do CT: " + IdCt + Environment.NewLine + "Id do Script_CondicaoScript: " + IdScript_CondicaoScript);
            #endregion

            try
            {
                #region Debug
                log.Info("Criando um novo Objeto do tipo Script_CondicaoScript_CT...");
                #endregion

                Script_CondicaoScript_CT scsCT = new Script_CondicaoScript_CT()
                {
                    IdCT = IdCt,
                    IdScript_CondicaoScript = IdScript_CondicaoScript
                };

                #region Debug
                log.Info("Objeto foi criado.");
                log.Info("Adicionando objeto para salvar na base...");
                #endregion

                db.Script_CondicaoScript_CT.Add(scsCT);

                #region Debug
                log.Info("Objeto foi Adicionado.");
                log.Info("Salvando objeto na base...");
                #endregion

                db.SaveChanges();

                #region Debug
                log.Info("Objeto foi salvo com sucesso na base! " + Environment.NewLine);
                log.DebugObject(scsCT);
                #endregion

                this.FlashSuccess("Associação realizada com sucesso!.");
            }
            catch (Exception ex)
            {
                this.FlashError("Ocorreu um erro ao tentar realizar a associação. Favor procure o administrador do sistema!");

                #region Debug
                log.Error("Message: " + ex.Message);
                log.Error("StackTrace" + ex.StackTrace);
                #endregion

                this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public JsonResult CarregarCT(int? IdAut = null, string FaseCT = null)
        {
            try
            {
                List<listaVO> list = new List<listaVO>();

                SqlParameter[] param =
                    {
                    new SqlParameter("@IDAUT", IdAut),
                    new SqlParameter("@IDCT", DBNull.Value),
                    new SqlParameter("@FASECT", FaseCT)
                };

                if (IdAut == null)
                    param[0].Value = DBNull.Value;

                if (FaseCT == "null")
                    param[2].Value = DBNull.Value;

                ViewBag.CTs = db.Database.SqlQuery<listaVO>("EXEC PR_LISTAR_CT @IDAUT, @IDCT, @FASECT", param).ToList();
                log.Info("Proc. executada com sucesso. Retornou " + ViewBag.CTs.Count + " registros.");

                for (int i = 0; i < ViewBag.CTs.Count; i++)
                {
                    listaVO obj = new listaVO
                    {
                        Id = ViewBag.CTs[i].Id,
                        Descricao = ViewBag.CTs[i].Descricao
                    };

                    list.Add(obj);
                }
                return Json(new { result = list }, JsonRequestBehavior.AllowGet);
                log.Info("retornou com sucesso");
            }
            catch(Exception ex)
            {
                return Json(new { result = "Erro: " + ex.Message }, JsonRequestBehavior.AllowGet);
                log.Error("Erro: " + ex.Message);
            }
            
        }

        public JsonResult CarregarScript(int? IdAut = null, int? IdAmbienteExecucao = null)
        {
            try
            {
                List<listaVO> list = new List<listaVO>();
                List<Script_CondicaoScriptVO> scripts = new List<Script_CondicaoScriptVO>();

                SqlParameter[] param =
                    {
                    new SqlParameter("@ID_AUT", IdAut),
                    new SqlParameter("@ID_AMBIENTE_EXECUCAO", IdAmbienteExecucao)
                };

                if (IdAut == null)
                    param[0].Value = DBNull.Value;

                if (IdAmbienteExecucao == null)
                    param[1].Value = DBNull.Value;

                scripts = db.Database.SqlQuery<Script_CondicaoScriptVO>("EXEC PR_LISTAR_SCRIPT_CONDICAO_SCRIPT @ID_AMBIENTE_EXECUCAO, @ID_AUT", param).ToList();
                log.Info("Proc. executada com sucesso. Retornou " + scripts.Count + " registros.");

                for (int i = 0; i < scripts.Count; i++)
                {
                    string desc = scripts[i].DescricaoCondicaoScript == null ? scripts[i].DescricaoScript : scripts[i].DescricaoScript + " - " + scripts[i].DescricaoCondicaoScript;

                    listaVO obj = new listaVO
                    {
                        Id = scripts[i].Id,
                        Descricao = desc
                    };

                    list.Add(obj);
                }

                return Json(new { result = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Erro: " + ex.Message }, JsonRequestBehavior.AllowGet);
                log.Error("Erro: " + ex.Message);
            }
            
        }
        public ActionResult Remover(IList<string> ids)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            for (int i = 0; i < ids.Count; i++)
            {
                Script_CondicaoScript_CT scs_ct = null;
                try
                {
                    int idAtual = Int32.Parse(ids[i]);
                    scs_ct = db.Script_CondicaoScript_CT.SingleOrDefault(a => a.Id == idAtual);

                    db.Script_CondicaoScript_CT.Remove(scs_ct);
                    db.SaveChanges();
                    log.Info("Associação de Script_CondicaoScript_CT excluído com sucesso!");

                    result.Data = new { Result = "Associações removidas com sucesso.", Status = (int)WebExceptionStatus.Success };
                }
                catch (Exception ex)
                {
                    result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };

                    log.Error("Erro ao excluir o Associação de Script_CondicaoScript_CT.", ex);
                    log.Debug("Associação de Script_CondicaoScript_CT: " + Util.ToString(scs_ct));
                }
            }
            return result;
        }

        public JsonResult SalvarAssociacao(int IdCT, int IdScript_CondicaoScript)
        {
            try
            {
                Script_CondicaoScript_CT scs_ct = db.Script_CondicaoScript_CT.Where(x => x.IdCT == IdCT).Where(x => x.IdScript_CondicaoScript == IdScript_CondicaoScript).FirstOrDefault();
                log.Info("Verificando se existe o registro para essa associação: " + scs_ct);

                if (scs_ct == null)
                {
                    log.Info("Não existe, sendo assim, será feita a associação.");
                    scs_ct = new Script_CondicaoScript_CT
                    {
                        IdCT = IdCT,
                        IdScript_CondicaoScript = IdScript_CondicaoScript
                    };
                    db.Script_CondicaoScript_CT.Add(scs_ct);
                    db.SaveChanges();
                    log.Info("Associação realizada com sucesso. Foi salvo a associação: " + scs_ct);

                    return Json(new { result = scs_ct }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "Já existe essa associação." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "Erro: " + ex.Message }, JsonRequestBehavior.AllowGet);
                log.Error("Erro: " + ex.Message);
            }
        }
    }
}