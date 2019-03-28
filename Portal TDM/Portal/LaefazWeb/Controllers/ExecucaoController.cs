using LaefazWeb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TDMWeb.Extensions;
using WebGrease.Css.Extensions;
using TDMWeb.Lib;
using LaefazWeb.Models.VOs;
using System.Data.SqlClient;
using LaefazWeb.Enumerators;
using Newtonsoft.Json;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class ExecucaoController : Controller
    {

        private DbEntities db = new DbEntities();

        //public ActionResult Index()
        //{
        //    return View(db.Execucao.ToList());
        //}

        public ActionResult Index()
        {
            return View(db.Execucao.ToList());
        }

        public ActionResult Adicionar()
        {
            ViewBag.listTipoFaseTeste = db.TipoFaseTeste.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Remover(int id)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Execucao execucao = db.Execucao.SingleOrDefault(a => a.Id == id);

                db.Execucao.Remove(execucao);
                db.SaveChanges();

                result.Data = new { Result = "Execução removida com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Execucao = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public ActionResult Editar(int id)
        {
            ViewBag.listTipoFaseTeste = db.TipoFaseTeste.ToList();

            return View(db.Execucao.FirstOrDefault(a => a.Id == id));
        }

        public ActionResult SalvarEdicao(Execucao objeto)
        {
            //try
            //{
                Salvar(objeto, true);
                this.FlashSuccess("Execução editada com sucesso.");
            //}
            //catch (Exception ex)
            //{
            //    this.FlashError(ex.Message);
            //}
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Salvar(Execucao objeto, bool editar = false)
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

                Execucao execucao;

                if (editar)
                {
                                       
                    objeto.IdTipoFaseTeste = Int32.Parse(Request.Form.Get("listfase"));
                    objeto.TipoFaseTeste.Id = objeto.IdTipoFaseTeste;

                    // anexar objeto ao contexto
                    db.Execucao.Attach(objeto);
                    // informa que o obejto será modificado
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    
                    execucao = new Execucao()
                    {
                        //Descricao = Request.Form.Get("descricao"),
                        IdTipoFaseTeste = Int32.Parse(Request.Form.Get("listfase"))
                };

                    //db.Execucao.Add(execucao);
                    //db.SaveChanges();

                    this.FlashSuccess("Execução adicionada com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}