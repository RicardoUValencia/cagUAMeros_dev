using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDBContext bd;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(SugerenciaDTO sugerenciaDTO)
        {
            if (!ModelState.IsValid )
            {
                return View(sugerenciaDTO);
            }

            using (var bd = new ApplicationDBContext())
            {
                Sugerencia sugerencia = new Sugerencia()
                {
                    ID = sugerenciaDTO.ID,
                    Nombre = sugerenciaDTO.Nombre,
                    Email = sugerenciaDTO.Email,
                    Comentario = sugerenciaDTO.Comentario
                };

                bd.Sugerencia.Add(sugerencia);
                bd.SaveChanges();
            }

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

    }
}