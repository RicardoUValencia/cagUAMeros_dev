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
    public class SugerenciaController : Controller
    {
        private ApplicationDBContext bd;

        // GET: Sugerencias
        public ActionResult Index()
        {
            var sugerenciaDTO = new List<SugerenciaDTO>();
            using (bd = new ApplicationDBContext())
            {
                sugerenciaDTO = (from s in bd.Sugerencia
                                 select new SugerenciaDTO
                                 {
                                     ID = s.ID,
                                     Nombre = s.Nombre,
                                     Email = s.Email,
                                     Comentario = s.Comentario
                                 }).ToList();
            }
            return View(sugerenciaDTO);
        }

        public ActionResult Eliminar(int id)
        {
            using (bd = new ApplicationDBContext())
            {
                Sugerencia sugerencia = (from s in bd.Sugerencia
                                         where s.ID == id
                                         select s).FirstOrDefault();

                bd.Sugerencia.Remove(sugerencia);

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}