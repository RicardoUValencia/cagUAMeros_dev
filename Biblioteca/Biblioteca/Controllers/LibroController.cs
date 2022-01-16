using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Filters;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    //[Admin]
    public class LibroController : Controller
    {
        private ApplicationDBContext bd; 
        private LibroDTO libroDTOVal;
        // GET: Libro
        public ActionResult Index()
        {
            List<LibroDTO> libros = null;
            using (bd = new ApplicationDBContext())
            {
                libros = (from l in bd.Libros
                          join c in bd.Categorias
                          on l.IDCategoria equals c.ID
                          where l.L_Habilitado == 1
                          select new LibroDTO
                          {
                              ID = l.ID,
                              Nombre_Autor = l.Nombre_Autor,
                              Titulo = l.Titulo,
                              Cantidad = l.Cantidad,
                              Anio_Publicacion = l.Anio_Publicacion,
                              lenguaje = l.Idioma,
                              categoria_Libro = c.Nombre_Categoria,
                              Disponibles = l.Total_Actual.ToString() + "/" + l.Cantidad.ToString(),
                              Fecha_Registro = l.Fecha_Registro,
                              Ubicacion = l.Ubicacion,
                              Edicion = l.Edicion
                          }).ToList();
            }

            return View(libros);
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            listarCategorias();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(LibroDTO libro)
        {
            try
            {
                int registros = 0;
                int registrosEstante = 0;
                using (bd = new ApplicationDBContext())
                {
                    registros = bd.Libros.Where(l => l.Titulo.Equals(libro.Titulo)
                                            && l.Edicion.Equals(libro.Edicion)
                                            && l.Idioma.Equals(libro.lenguaje)).Count();

                    registrosEstante = bd.Libros.Where(l => l.Ubicacion.Equals(libro.Ubicacion)).Count();
                }

                if (!ModelState.IsValid || registros >= 1 || registrosEstante >= 1)
                {
                    if (registros >= 1)
                    {
                        libro.mensaje = "El libro ya existe";
                    }

                    if (registrosEstante >= 1)
                    {
                        libro.mensajeEstante = "Ya hay un libro en esa ubicación";
                    }

                    //Verificar la foto
                    //if (foto == null)
                    //{
                    //    libro.mensaje = "La foto es obligatoria";
                    //}

                    listarCategorias();
                    return View(libro);
                }

                using (bd = new ApplicationDBContext())
                {
                    Libro l = new Libro();

                    #region Imagen
                    byte[] fotoBD = null;
                    //if (foto != null)
                    //{
                    //    BinaryReader lector = new BinaryReader(foto.InputStream);
                    //    fotoBD = lector.ReadBytes((int)foto.ContentLength);
                    //}
                    #endregion

                    l.ID = libro.ID;
                    l.Nombre_Autor = libro.Nombre_Autor;
                    l.Titulo = libro.Titulo;
                    l.Ubicacion = libro.Ubicacion;
                    l.IDCategoria = libro.IDCategoria;
                    l.Edicion = libro.Edicion;
                    l.Cantidad = libro.Cantidad;
                    l.Total_Actual = libro.Cantidad;
                    l.Idioma = libro.lenguaje;
                    l.Fecha_Registro = DateTime.Now;
                    l.Anio_Publicacion = libro.Anio_Publicacion;
                    l.L_Habilitado = 1;

                    bd.Libros.Add(l);
                    bd.SaveChanges();
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            listarCategorias();

            LibroDTO libroDTO = new LibroDTO();

            using (bd = new ApplicationDBContext())
            {
                Libro libro = bd.Libros.Where(l => l.ID.Equals(id)).FirstOrDefault();

                libroDTO.ID = libro.ID;
                libroDTO.Titulo = libro.Titulo;
                libroDTO.Nombre_Autor = libro.Nombre_Autor;
                libroDTO.IDCategoria = libro.IDCategoria;
                libroDTO.Fecha_Registro = libro.Fecha_Registro;
                libroDTO.Cantidad = libro.Cantidad;
                libroDTO.Anio_Publicacion = libro.Anio_Publicacion;
                libroDTO.Edicion = libro.Edicion;
                libroDTO.Ubicacion = libro.Ubicacion;
            }

            return View(libroDTO);
        }

        [HttpPost]
        public ActionResult Editar(LibroDTO libroDTO)
        {
            if (!ModelState.IsValid)
            {
                listarCategorias();
                return View(libroDTO);
            }

            using (bd = new ApplicationDBContext())
            {
                Libro libro = bd.Libros.Where(l => l.ID.Equals(libroDTO.ID)).First();

                libro.ID = libroDTO.ID;
                libro.Titulo = libroDTO.Titulo;
                libro.Nombre_Autor = libroDTO.Nombre_Autor;
                libro.IDCategoria = libroDTO.IDCategoria;
                libro.Fecha_Registro = libroDTO.Fecha_Registro;
                libro.Cantidad = libroDTO.Cantidad;
                libro.Anio_Publicacion = libroDTO.Anio_Publicacion;
                libro.Edicion = libroDTO.Edicion;
                libro.Ubicacion = libroDTO.Ubicacion;

                bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (bd = new ApplicationDBContext())
            {
                Libro libro = bd.Libros.Where(l => l.ID.Equals(id)).First();
                libro.L_Habilitado = 0;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /*
         *Regresa la vista de busquedas avanzadas
         */

        [HttpGet]
        public ActionResult BusquedaAvanzada()
        {
            List<LibroDTO> libro = new List<LibroDTO>();
            return View(libro);
        }

        /*
         *Filtro para realizar busquedas avanzadas
         *Recibe uno o más de los siguientes parametros
         *string nombre autor
         *string titulo libro
         *int año de publicación
         *int edicion
         *string idioma
         *
         *Regresa un libro que coincida con la busqueda
         */

        [HttpPost]
        public ActionResult BusquedaAvanzada(LibroDTO libroDTO)
        {
            List<LibroDTO> libros = null;
            List<LibroDTO> librosFiltrados = null;
            libroDTOVal = libroDTO;
            using (bd = new ApplicationDBContext())
            {
                libros = (from l in bd.Libros
                          join c in bd.Categorias
                          on l.IDCategoria equals c.ID
                          where l.L_Habilitado == 1
                          select new LibroDTO
                          {
                              ID = l.ID,
                              Nombre_Autor = l.Nombre_Autor,
                              Titulo = l.Titulo,
                              Cantidad = l.Cantidad,
                              Anio_Publicacion = l.Anio_Publicacion,
                              lenguaje = l.Idioma,
                              categoria_Libro = c.Nombre_Categoria,
                              Disponibles = l.Total_Actual.ToString() + "/" + l.Cantidad.ToString(),
                              Fecha_Registro = l.Fecha_Registro,
                              Ubicacion = l.Ubicacion,
                              Edicion = l.Edicion
                          }).ToList();

                if (libroDTO.Titulo == null && libroDTO.Nombre_Autor == null && libroDTO.lenguaje == null 
                    && libroDTO.categoria_Libro == null && libroDTO.Edicion == null && libroDTO.Anio_Publicacion == 0)
                {
                    librosFiltrados = libros;
                }
                else
                {
                    Predicate<LibroDTO> prediTitulo = new Predicate<LibroDTO>(buscarLibro);
                    librosFiltrados = libros.FindAll(prediTitulo);
                }
            }

            return View(librosFiltrados);
        }

        public void listarCategorias()
        {
            List<SelectListItem> listaCategorias = null;

            using (bd = new ApplicationDBContext())
            {
                listaCategorias = (from c in bd.Categorias
                                   where c.C_habilitado == 1
                                   select new SelectListItem
                                   {
                                       Text = c.Nombre_Categoria,
                                       Value = c.ID.ToString()
                                   }).ToList();
            }

            listaCategorias.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            ViewBag.listaCategoria = listaCategorias;
        }

        private bool buscarLibro(LibroDTO libroDTO)
        {
            bool tituloLibro = true;
            bool autorLbro = true;
            bool lenguajeLibro = true;
            bool categoriaLibro = true;
            bool anioLibro = true;
            bool edicionLibro = true;

            if (libroDTOVal.Titulo != null)
            {
                tituloLibro = libroDTO.Titulo.ToString().Contains(libroDTOVal.Titulo);
            }

            if (libroDTOVal.Nombre_Autor != null)
            {
                autorLbro = libroDTO.Nombre_Autor.ToString().Contains(libroDTOVal.Nombre_Autor);
            }

            if (libroDTOVal.lenguaje != null)
            {
                lenguajeLibro = libroDTO.lenguaje.ToString().Contains(libroDTOVal.lenguaje);
            }

            if (libroDTOVal.categoria_Libro != null)
            {
                categoriaLibro = libroDTO.categoria_Libro.ToString().Contains(libroDTOVal.categoria_Libro);
            }

            if(libroDTOVal.Edicion != null)
            {
                edicionLibro = libroDTO.Edicion.ToString().Contains(libroDTOVal.Edicion);
            }

            if (libroDTOVal.Anio_Publicacion != 0)
            {
                anioLibro = libroDTO.Anio_Publicacion.ToString().Contains(libroDTOVal.Anio_Publicacion.ToString());
            }

            return (tituloLibro && autorLbro && lenguajeLibro && categoriaLibro && edicionLibro && anioLibro);
        }
    }
}