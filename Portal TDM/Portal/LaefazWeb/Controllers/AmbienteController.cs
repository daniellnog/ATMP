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
    public class AmbienteVirtualController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.AmbienteVirtual.ToList());
        }

        public ActionResult Adicionar()
        {
            ViewBag.ambientes = db.AmbienteVirtual.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Remover(int id)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                AmbienteVirtual ambiente = db.AmbienteVirtual.SingleOrDefault(a => a.Id == id);

                db.AmbienteVirtual.Remove(ambiente);
                db.SaveChanges();

                result.Data = new { Result = "Ambiente removido com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }
        

        
    public ActionResult Editar(int id)
    {

            ViewBag.ambientes = db.AmbienteVirtual.ToList();
            return View(db.AmbienteVirtual.FirstOrDefault(a => a.Id == id));
    }
    
        
        public ActionResult SalvarEdicao(AmbienteVirtual objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Ambiente editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }
        
        
        [HttpPost]
        public ActionResult Salvar(AmbienteVirtual objeto, bool editar = false)
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

                AmbienteVirtual ambiente;

                if (editar)
                {
                    //objeto.UltimaAtualizacao = DateTime.Now;
                    //// anexar objeto ao contexto
                    //db.AmbienteVirtual.Attach(objeto);
                    //// informa que o obejto será modificado
                    //db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    //db.SaveChanges();
                }
                else
                {
                    ambiente = new AmbienteVirtual()
                    {
                        IP = Request.Form.Get("ip"),
                        Descricao = Request.Form.Get("descricao"),
             
                    };

                    db.AmbienteVirtual.Add(ambiente);
                    db.SaveChanges();

                    this.FlashSuccess("Ambiente adicionado com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Ambiente_Descricao"))
                    this.FlashError("Já existe um Ambiente com essa descrição.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    
    }
}