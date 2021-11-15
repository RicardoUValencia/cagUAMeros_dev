using Biblioteca.DBContext;
using Biblioteca.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class BuscarController : Controller
    {
        private ApplicationDBContext bd;
        // GET: Buscar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buscar(LibroDTO libroDTO)
        {
            List<LibroDTO> libros = null;
            string titulo_autor = libroDTO.Titulo_Autor;

            using (bd = new ApplicationDBContext())
            {

                //Busqueda por titulo

                libros = (from l in bd.Libros
                          join c in bd.Categorias
                          on l.IDCategoria equals c.ID
                          where l.L_Habilitado == 1
                          && l.Titulo.Contains(titulo_autor)
                          select new LibroDTO
                          {
                              ID = l.ID,
                              Nombre_Autor = l.Nombre_Autor,
                              Titulo = l.Titulo,
                              Cantidad = l.Cantidad,
                              Total_Actual = l.Total_Actual,
                              lenguaje = l.Idioma,
                              Disponibles = l.Total_Actual.ToString() + "/" + l.Cantidad.ToString(),
                              Anio_Publicacion = l.Anio_Publicacion,
                              categoria_Libro = c.Nombre_Categoria,
                              Ubicacion = l.Ubicacion,
                              Edicion = l.Edicion
                          }).ToList();

                if (libros.Count() == 0)
                {
                    //Busqueda por autor
                    libros = (from l in bd.Libros
                              join c in bd.Categorias
                              on l.IDCategoria equals c.ID
                              where l.L_Habilitado == 1
                              && l.Nombre_Autor.Contains(titulo_autor)
                              select new LibroDTO
                              {
                                  ID = l.ID,
                                  Nombre_Autor = l.Nombre_Autor,
                                  Titulo = l.Titulo,
                                  Cantidad = l.Cantidad,
                                  Total_Actual = l.Total_Actual,
                                  Disponibles = l.Total_Actual.ToString() + "/" + l.Cantidad.ToString(),
                                  Anio_Publicacion = l.Anio_Publicacion,
                                  categoria_Libro = c.Nombre_Categoria,
                                  Ubicacion = l.Ubicacion,
                                  Edicion = l.Edicion
                              }).ToList();
                }

            }

            return View(libros);
        }
    }
}