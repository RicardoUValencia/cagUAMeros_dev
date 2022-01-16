using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Filters;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    //[Admin]
    public class PrestamoController : Controller
    {
        private ApplicationDBContext bd;
        // GET: Prestamo
        [Admin]
        public ActionResult Index(PrestamoDTO prestamo)
        {
            List<PrestamoDTO> prestamos = null;
            string nombreUsuario = prestamo.nombreUsuario;

            using (bd = new ApplicationDBContext())
            {
                if (nombreUsuario != null)
                {
                    prestamos = (from c in bd.Prestamos
                                  join u in bd.Usuarios
                                  on c.UsuarioID equals u.ID
                                  join l in bd.Libros
                                  on c.LibroID equals l.ID
                                  where c.P_Habilitado == 1
                                  && u.Nombre.Contains(nombreUsuario)
                                  select new PrestamoDTO
                                  {
                                      ID = c.ID,
                                      nombreUsuario = u.Nombre,
                                      tituloLibro = l.Titulo,
                                      Fecha_Devolucion = c.Fecha_Devolucion,
                                      Fecha_Prestamo = c.Fecha_Prestamo
                                  }).ToList();
                }
                else
                {
                    prestamos = (from c in bd.Prestamos
                                  join u in bd.Usuarios
                                  on c.UsuarioID equals u.ID
                                  join l in bd.Libros
                                  on c.LibroID equals l.ID
                                  where c.P_Habilitado == 1
                                  select new PrestamoDTO
                                  {
                                      ID = c.ID,
                                      nombreUsuario = u.Nombre,
                                      tituloLibro = l.Titulo,
                                      Fecha_Devolucion = c.Fecha_Devolucion,
                                      Fecha_Prestamo = c.Fecha_Prestamo
                                  }).ToList();
                }

            }

            return View(prestamos);
        }

        public ActionResult Pedir(int id)
        {
            if (Session["Usuario"] != null)
            {
                Usuario usuario = (Usuario)Session["Usuario"];

                using (bd = new ApplicationDBContext())
                {
                    Prestamo prestamo = new Prestamo()
                    {
                        LibroID = id,
                        UsuarioID = usuario.ID,
                        Fecha_Prestamo = DateTime.Now,
                        Fecha_Devolucion = DateTime.Today.AddDays(5),
                        Cantidad = 1,
                        P_Habilitado = 1
                    };

                    Libro libro = bd.Libros.Where(l => l.ID.Equals(id)).FirstOrDefault();

                    libro.Total_Actual = libro.Total_Actual - 1;

                    bd.SaveChanges();

                    bd.Prestamos.Add(prestamo);
                    bd.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Buscar");
        }
    }
}