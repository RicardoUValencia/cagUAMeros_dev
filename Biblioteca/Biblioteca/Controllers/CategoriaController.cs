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
    [Admin]
    public class CategoriaController : Controller
    {
        private ApplicationDBContext bd;
        // GET: Categoria
        public ActionResult Index(CategoriaDTO categoriaDTO)
        {
            List<CategoriaDTO> categorias = null;
            string nombreCategoria = categoriaDTO.Nombre_Categoria;

            using (bd = new ApplicationDBContext())
            {
                if (nombreCategoria != null)
                {
                    categorias = (from c in bd.Categorias
                                  where c.C_habilitado == 1
                                  && c.Nombre_Categoria.Contains(nombreCategoria)
                                  select new CategoriaDTO
                                  {
                                      ID = c.ID,
                                      Nombre_Categoria = c.Nombre_Categoria
                                  }).ToList();
                }
                else
                {
                    categorias = (from c in bd.Categorias
                                  where c.C_habilitado == 1
                                  select new CategoriaDTO
                                  {
                                      ID = c.ID,
                                      Nombre_Categoria = c.Nombre_Categoria
                                  }).ToList();
                }

            }

            return View(categorias);
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(CategoriaDTO categoriaDTO)
        {

            int categorias = 0;

            using (bd = new ApplicationDBContext())
            {
                categorias = bd.Categorias.Where(c => c.Nombre_Categoria.Equals(categoriaDTO.Nombre_Categoria)).Count();
            }


            if (!ModelState.IsValid || categorias >= 1)
            {
                if (categorias >= 1)
                {
                    categoriaDTO.mensaje = "La categoria ya existe";
                }

                return View(categoriaDTO);
            }

            using (bd = new ApplicationDBContext())
            {
                Categoria categoria = new Categoria()
                {
                    ID = categoriaDTO.ID,
                    Nombre_Categoria = categoriaDTO.Nombre_Categoria,
                    C_habilitado = 1
                };

                bd.Categorias.Add(categoria);
                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            CategoriaDTO categoriaDTO = null;

            using (bd = new ApplicationDBContext())
            {
                Categoria categoria = bd.Categorias.Where(i => i.ID.Equals(id)).First();

                categoriaDTO = new CategoriaDTO()
                {
                    ID = categoria.ID,
                    Nombre_Categoria = categoria.Nombre_Categoria
                };

            }

            return View(categoriaDTO);
        }

        [HttpPost]
        public ActionResult Editar(CategoriaDTO categoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaDTO);
            }

            using (bd = new ApplicationDBContext())
            {
                Categoria categoria = bd.Categorias.Where(i => i.ID.Equals(categoriaDTO.ID)).First();

                categoria.ID = categoriaDTO.ID;
                categoria.Nombre_Categoria = categoriaDTO.Nombre_Categoria;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (bd = new ApplicationDBContext())
            {
                Categoria categoria = bd.Categorias.Where(c => c.ID.Equals(id)).First();

                categoria.C_habilitado = 0;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}