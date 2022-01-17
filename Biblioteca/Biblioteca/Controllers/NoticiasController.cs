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
    public class NoticiasController : Controller
    {
        private ApplicationDBContext bd;
        // GET: Noticias
        public ActionResult Index()
        {
            var carruselDTO = new List<CarruselDTO>();

            using (bd = new ApplicationDBContext())
            {
                carruselDTO = (from c in bd.Carrusel
                                   select new CarruselDTO
                                   {
                                       ID = c.ID,
                                       ImagePath = c.ImagePath,
                                       Nombre = c.Nombre,
                                       Descripcion = c.Descripcion,
                                       Url = c.Url
                                   }).ToList();
            }
            

            return View(carruselDTO);
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(CarruselDTO carruselDTO)
        {
              using (bd = new ApplicationDBContext())
            {
                Carrusel carrusel = new Carrusel()
                {
                    ID = carruselDTO.ID,
                    ImagePath = carruselDTO.ImagePath,
                    Nombre = carruselDTO.Nombre,
                    Descripcion = carruselDTO.Descripcion,
                    Url = carruselDTO.Url
                };

                bd.Carrusel.Add(carrusel);
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Editar(int id)
        {
            CarruselDTO carruselDTO = new CarruselDTO();

            using (bd = new ApplicationDBContext())
            {
                Carrusel carrusel = bd.Carrusel.Where(i => i.ID.Equals(id)).First();

                carruselDTO = new CarruselDTO()
                {
                    ID = carrusel.ID,
                    Nombre = carruselDTO.Nombre
                };
            }

                return View(carruselDTO);
        }


        [HttpPost]
        public ActionResult Editar(CarruselDTO carruselDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(carruselDTO);
            }

            using (bd = new ApplicationDBContext())
            {
                Carrusel carrusel = bd.Carrusel.Where(i =>i.ID.Equals(carruselDTO.ID)).First();

                carrusel.ID = carruselDTO.ID;
                carrusel.Nombre = carruselDTO.Nombre;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (bd = new ApplicationDBContext())
            {
                Carrusel carrusel = bd.Carrusel.Where(c => c.ID.Equals(id)).First();

                bd.Carrusel.Remove(carrusel);
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}