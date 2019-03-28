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
using LaefazWeb.Extensions;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class DemandaController : Controller
    {
  
        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.Demanda.ToList());
        }

        public ActionResult Adicionar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Remover(int id)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Demanda demanda = db.Demanda.SingleOrDefault(a => a.Id == id);

                db.Demanda.Remove(demanda);
                db.SaveChanges();

                result.Data = new { Result = "Demanda removida com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("FK_DataPool_Demanda"))
                    // this.FlashError("Esse registro contém dependência com outra entidade.");
                    result.Data = new { Result = "Esse registro contém dependência com outra entidade.", Status = (int)WebExceptionStatus.SendFailure };
                else
                    result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }
        

        
    public ActionResult Editar(int id)
    {
        return View(db.Demanda.FirstOrDefault(a => a.Id == id));
    }
    
        
        public ActionResult SalvarEdicao(Demanda objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Demanda editada com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }
        
        
        [HttpPost]
        public ActionResult Salvar(Demanda objeto, bool editar = false)
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

                Demanda demanda;

                if (editar)
                {

                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    demanda = new Demanda()
                    {
                        Descricao = Request.Form.Get("descricao"),
                    };

                    db.Demanda.Add(demanda);
                    db.SaveChanges();

                    this.FlashSuccess("Demanda adicionada com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Demanda_Descricao"))
                    this.FlashError("Já existe uma demanda com essa descrição.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    
    }
}