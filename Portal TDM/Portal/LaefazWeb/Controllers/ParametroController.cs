using LaefazWeb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TDMWeb.Extensions;
using WebGrease.Css.Extensions;
using TDMWeb.Lib;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class ParametroController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.Parametro.ToList());
        }

        public ActionResult Adicionar()
        {
            ViewBag.TipoDadoParametro = db.TipoDadoParametro.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Remover(int id)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Parametro parametro = db.Parametro.SingleOrDefault(a => a.Id == id);

                db.Parametro.Remove(parametro);
                db.SaveChanges();

                result.Data = new { Result = "Parâmetro removido com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }
        

        
    public ActionResult Editar(int id)
    {

        ViewBag.TipoDadoParametro = db.TipoDadoParametro.ToList();
        Parametro p = db.Parametro.FirstOrDefault(a => a.Id == id);
        return View(p);
    }
    
        
        public ActionResult SalvarEdicao(Parametro objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Parâmetro editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }
        
        
        [HttpPost]
        public ActionResult Salvar(Parametro objeto, bool editar = false)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var msg = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ForEach(m => msg = string.Concat(m.ErrorMessage.ToString(), @"\n"));
                    if (!msg.IsNullOrWhiteSpace())
                        this.FlashWarning(msg);

                    return View("Adicionar", objeto);
                }

                Parametro parametro;

                if (editar)
                {
                    objeto.Tipo = Request.Form.Get("tipo_dado_parametro");
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    parametro = new Parametro()
                    {
                        Descricao = Request.Form.Get("descricao"),
                        ColunaTecnicaTosca = Request.Form.Get("colunaTecnicaTosca"),
                        Tipo = Request.Form.Get("tipo_dado_parametro")
                        
                };

                    db.Parametro.Add(parametro);
                    db.SaveChanges();

                    this.FlashSuccess("Parâmetro adicionado com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Parametro_Descricao"))
                    this.FlashError("Já existe um Parâmetro com essa descrição.");
                else if(ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Parametro_ColunaTecnicaTosca"))
                    this.FlashError("Já existe um Parâmetro com esse Nome Técnico Tosca.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    
    }
}