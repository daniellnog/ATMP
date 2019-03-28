using TDMWeb.Lib;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using LaefazWeb.Models;

namespace TDMWeb.Controllers
{
    [UsuarioLogado]
    public class IndexController : Controller
    {
        private DbEntities db = new DbEntities();

        //
        // GET: /Index/

        public ActionResult Index()
        {
           return View(); // db.Database.SqlQuery<IndicadorVO>("EXEC PR_INDICADOR_ANALISES").ToList());
        }
    }
}
