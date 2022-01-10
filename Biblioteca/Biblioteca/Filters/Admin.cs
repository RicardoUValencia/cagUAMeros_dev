using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace Biblioteca.Filters
{
    public class Admin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var admin = HttpContext.Current.Session["Administrador"];
            var bibliotecario = HttpContext.Current.Session["Bibliotecario"];

            if (admin == null && bibliotecario == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            if (admin != null)
            {

            }

            if (bibliotecario != null)
            {

            }

            base.OnActionExecuting(filterContext);
        }
    }

    
}