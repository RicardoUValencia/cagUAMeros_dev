using Biblioteca.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class SugerenciaController : Controller
    {
        private ApplicationDBContext bd;
        // GET: Sugerencia
        public ActionResult Index()
        {
            return View();
        }
    }
}