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

            if (admin == null)
                {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            if (admin != null)
            {
            }

            base.OnActionExecuting(filterContext);
        }
    }
}