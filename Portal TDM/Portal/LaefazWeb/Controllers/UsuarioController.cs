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
    public class UsuarioController : Controller
    {

        private DbEntities db = new DbEntities();

        public ActionResult Index()
        {
            return View(db.Usuario.ToList());
        }

        public ActionResult Adicionar()
        {
            ViewBag.ListaTDM = GetTDMByUser();
            return View();
        }

        [HttpPost]
        public ActionResult Remover(int id)
        {
            var result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            try
            {
                Usuario usuario = db.Usuario.SingleOrDefault(a => a.Id == id);

                db.Usuario.Remove(usuario);
                db.SaveChanges();

                result.Data = new { Result = "Usuário removido com sucesso.", Status = (int)WebExceptionStatus.Success };
            }
            catch (Exception ex)
            {
                result.Data = new { Result = ex.Message, Status = (int)WebExceptionStatus.UnknownError };
            }
            return result;
        }



        public ActionResult Editar(int id)
        {
            return View(db.Usuario.FirstOrDefault(x => x.Id == id));
        }


        public ActionResult SalvarEdicao(Usuario objeto)
        {
            try
            {
                Salvar(objeto, true);
                this.FlashSuccess("Usuário editado com sucesso.");
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Salvar(Usuario objeto, bool editar = false)
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

                Usuario usuario;

                if (editar)
                {
                    
                    db.Entry(objeto).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    usuario = new Usuario()
                    {
                        Login = Request.Form.Get("login"),
                        Senha = Request.Form.Get("senha"),
                    };

                    db.Usuario.Add(usuario);
                    db.SaveChanges();

                    this.FlashSuccess("Usuário adicionado com sucesso!.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.ToString().Contains("AK_Usuario_Login"))
                    this.FlashError("Já existe um Usuário com esse Login.");
                else
                    this.FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        private List<TDM> GetTDMByUser()
        {
            List<TDM> TDMs = new List<TDM>();
            int idUser = Util.GetUsuarioLogado().Id;
            List<TDM_Usuario> tdmUsuario = db.TDM_Usuario.Where(x=> x.IdUsuario == idUser).ToList();

            tdmUsuario.ForEach(element=>{
                TDM tdm = db.TDM.FirstOrDefault(x=>x.Id == element.IdTDM);
                if (!tdm.TdmPublico)
                    TDMs.Add(tdm);
            });

            return TDMs;
        }

    }
}