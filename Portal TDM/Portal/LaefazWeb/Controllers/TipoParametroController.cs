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
    public class TipoParametroController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.TipoParametro.ToList());
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
                TipoParametro tipoParametro = db.TipoParametro.SingleOrDefault(a => a.Id == id);

                db.TipoParametro.Remove(tipoParametro);
                db.SaveChanges();

                result.Data = new { Result = "Tipo de Parâmetro removido com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }



        public ActionResult Editar(int id)
        {
            return View(db.TipoParametro.FirstOrDefault(x => x.Id == id));
        }


        public ActionResult SalvarEdicao(TipoParametro objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Tipo de Parâmetro editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Salvar(TipoParametro objeto, bool editar = false)
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

                TipoParametro tipoParametro;

                if (editar)
                {
                    
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    tipoParametro = new TipoParametro()
                    {
                        Descricao = Request.Form.Get("tipo_parametro_descricao"),
                    };

                    db.TipoParametro.Add(tipoParametro);
                    db.SaveChanges();

                    this.FlashSuccess("Tipo de Parâmetro adicionado com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Parametro_Descricao"))
                    this.FlashError("Já existe um Tipo de Parâmetro com essa descrição.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

    }
}