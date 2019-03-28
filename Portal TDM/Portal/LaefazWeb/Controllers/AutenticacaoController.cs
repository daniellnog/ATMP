using System.Linq;
using System.Web.Mvc;
using LaefazWeb.Models;

namespace TDMWeb.Controllers
{
    public class AutenticacaoController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("Index")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IndexPost()
        {
            string usuario = Request.Form.Get("usuario");
            string senha = Request.Form.Get("senha");

            DbEntities db = new DbEntities();

            Usuario usuarioDB = db.Usuario.Where(u => u.Login.Equals(usuario)).Where(u => u.Senha.Equals(senha)).FirstOrDefault();

            if (usuarioDB != null)
            {
                Session["ObjUsuario"] = usuarioDB;
                return RedirectToAction("Index", "DataPool");
            }

            ModelState.AddModelError("", "Credenciais inválidas");
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();

            return RedirectToAction("Index");
        }
    }
}
