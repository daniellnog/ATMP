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
    public class AUTController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.AUT.ToList());
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
                AUT aut = db.AUT.SingleOrDefault(a => a.Id == id);

                db.AUT.Remove(aut);
                db.SaveChanges();

                result.Data = new { Result = "Sistema removido com sucesso.", Status = (int)WebExceptionStatus.Success };
               
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("FK_DataPool_Aut"))
                    // this.FlashError("Esse registro contém dependência com outra entidade.");
                    result.Data = new { Result = "Esse registro contém dependência com outra entidade.", Status = (int)WebExceptionStatus.SendFailure };
                else
                    result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
                //this.FlashError(ex.Message);


                //result.Data = new { Result = "Esse registro contém dependência com outra entidade.", Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public ActionResult Editar(int id)
        {
            return View(db.AUT.FirstOrDefault(a => a.Id == id));
        }

        public ActionResult SalvarEdicao(AUT objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Sistema editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Salvar(AUT objeto, bool editar = false)
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

                AUT aut;

                if (editar)
                {
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    aut = new AUT()
                    {
                        Descricao = Request.Form.Get("descricao"),
                    };

                    db.AUT.Add(aut);
                    db.SaveChanges();

                    this.FlashSuccess("Sistema adicionado com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_AUT_Descricao"))
                    this.FlashError("Já existe um Sistema com essa descrição.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
