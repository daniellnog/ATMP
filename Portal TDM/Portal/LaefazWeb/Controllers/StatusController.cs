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
using TDMWeb.Models.VOs;

namespace LaefazWeb.Controllers
{
    [UsuarioLogado]
    public class StatusController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            List<StatusVO> status = new List<StatusVO>();

            foreach(Status s in db.Status.ToList())
            {
                status.Add(new StatusVO { Status = s, StatusExecucao = null, Tipo = "STATUS" });
            }

            foreach (StatusExecucao s in db.StatusExecucao.ToList())
            {
                status.Add(new StatusVO { Status = null, StatusExecucao = s, Tipo = "STATUS EXECUÇÃO" });
            }

            return View(status);
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
                Status status = db.Status.SingleOrDefault(a => a.Id == id);

                db.Status.Remove(status);
                db.SaveChanges();

                result.Data = new { Result = "Status removido com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }

        public ActionResult Editar(int id)
        {
            return View(db.Status.FirstOrDefault(a => a.Id == id));
        }

        public ActionResult SalvarEdicao(Status objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Status editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Salvar(Status objeto, bool editar = false)
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

                Status status;

                if (editar)
                {
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    string tipo = Request.Form.Get("tipoStatus");

                    if (tipo.Equals("STATUS")) {
                        status = new Status()
                        {
                            Descricao = Request.Form.Get("descricao"),
                        };

                        db.Status.Add(status);
                        db.SaveChanges();
                    }else {

                        StatusExecucao statusExecucao = new StatusExecucao()
                        {
                            Descricao = Request.Form.Get("descricao"),
                        };

                        db.StatusExecucao.Add(statusExecucao);
                        db.SaveChanges();

                    }

                    this.FlashSuccess("Status adicionado com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Status_Column"))
                    this.FlashError("Já existe um Status com essa descrição.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public JsonResult EditarStatus(string id, string descricao, string tipo, string editar)
        {
            try
            {

                if (editar.Equals("true"))
                {
                    if (tipo.Equals("STATUS"))
                    {
                        Status status = new Status()
                        {
                            Descricao = descricao,
                            Id = Int32.Parse(id)
                        };
                        db.Entry(status).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {

                        StatusExecucao statusExecucao = new StatusExecucao()
                        {
                            Descricao = descricao,
                            Id = Int32.Parse(id)
                        };

                        db.Entry(statusExecucao).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                else
                {
                    if (tipo.Equals("STATUS"))
                    {
                        Status status = new Status()
                        {
                            Descricao = descricao
                            
                        };

                        db.Status.Add(status);
                        db.SaveChanges();
                    }
                    else
                    {
                        db = new DbEntities();
                        StatusExecucao statusExecucao = new StatusExecucao()
                        {
                            Descricao = descricao
                            
                        };

                        db.StatusExecucao.Add(statusExecucao);
                        db.SaveChanges();

                    }
                }


                
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
                
            }

            return Json("Operação realizada com sucesso!", JsonRequestBehavior.AllowGet);


        }
    }
}
