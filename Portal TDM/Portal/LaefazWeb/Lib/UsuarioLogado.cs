using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TDMWeb.Lib
{
    public class UsuarioLogado : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (filterContext.HttpContext.Session["ObjUsuario"] == null)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Logout", controller = "Autenticacao" }));
        //    }
        //}



        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            bool redirectLoggin = !(filterContext.HttpContext.Session["ObjUsuario"] != null);

            if (redirectLoggin)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Code = 401, //Usuario não autorizado
                            Result = "/Account/LogOn"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Logout", controller = "Autenticacao" }));
                    //filterContext.Result = new RedirectResult("~/Account/LogOn?returnUrl=" +
                                   //filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}