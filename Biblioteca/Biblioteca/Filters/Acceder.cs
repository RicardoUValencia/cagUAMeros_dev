using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Filters
{
    public class Acceder: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var bibliotecario = HttpContext.Current.Session["Bibliotecario"];
            var admin = HttpContext.Current.Session["Administrador"];

            if (admin == null && bibliotecario == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}