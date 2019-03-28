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
    public class TipoFaseTesteController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.TipoFaseTeste.ToList());
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
                TipoFaseTeste tipoFaseTeste = db.TipoFaseTeste.SingleOrDefault(a => a.Id == id);

                db.TipoFaseTeste.Remove(tipoFaseTeste);
                db.SaveChanges();

                result.Data = new { Result = "Fase removida com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public ActionResult Editar(int id)
        {
            return View(db.TipoFaseTeste.FirstOrDefault(a => a.Id == id));
        }

        public ActionResult SalvarEdicao(TipoFaseTeste objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Fase editada com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Salvar(TipoFaseTeste objeto, bool editar = false)
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

                TipoFaseTeste tipoFaseTeste;

                if (editar)
                {

                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    tipoFaseTeste = new TipoFaseTeste()
                    {
                        Descricao = Request.Form.Get("descricao"),
                    };

                    db.TipoFaseTeste.Add(tipoFaseTeste);
                    db.SaveChanges();

                    this.FlashSuccess("Fase adicionada com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_TipoFaseTeste_Descricao"))
                    this.FlashError("Já existe uma Fase com essa descrição.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
