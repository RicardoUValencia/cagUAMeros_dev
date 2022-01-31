using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Filters;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    //[Admin]
    public class UsuarioController : Controller
    {
        private ApplicationDBContext bd;
        //GET: Usuario
        [Admin]
        public ActionResult Index(UsuarioDTO alumnoDTO)
        {
            if (Session["Usuario"] != null)
            {
                return RedirectToAction("Perfil", "Login");
            }
            List<UsuarioDTO> alumnos = null;
            List<UsuarioDTO> profesores = null;
            string nombre = alumnoDTO.Nombre;

            using (bd = new ApplicationDBContext())
            {
                if (nombre != null)
                {
                    alumnos = (from a in bd.Usuarios
                               join u in bd.TipoUsuarios
                               on a.TipoUsuarioID equals u.ID
                               where a.U_Habilitado == 1
                               && u.Tipo_Usuario.Equals("Alumno")
                               && a.Nombre.Contains(nombre)
                               select new UsuarioDTO
                               {
                                   ID = a.ID,
                                   Nombre = a.Nombre,
                                   Email = a.Email,
                                   Telefono = a.Telefono,
                                   Direccion = a.Direccion,
                                   Fecha_Nacimiento = a.Fecha_Nacimiento,
                                   Tipo_Usuario = u.Tipo_Usuario
                               }).ToList();

                    profesores = (from a in bd.Usuarios
                                  join u in bd.TipoUsuarios
                                  on a.TipoUsuarioID equals u.ID
                                  where u.Tipo_Usuario.Equals("Profesor")
                                  && a.U_Habilitado == 1
                                  && a.Nombre.Contains(nombre)
                                  select new UsuarioDTO
                                  {
                                      ID = a.ID,
                                      Nombre = a.Nombre,
                                      Email = a.Email,
                                      Telefono = a.Telefono,
                                      Direccion = a.Direccion,
                                      Fecha_Nacimiento = a.Fecha_Nacimiento,
                                      Tipo_Usuario = u.Tipo_Usuario
                                  }).ToList();

                    foreach (var item in profesores)
                    {
                        alumnos.Add(item);
                    }
                }
                else
                {
                    alumnos = (from a in bd.Usuarios
                               join u in bd.TipoUsuarios
                               on a.TipoUsuarioID equals u.ID
                               where u.Tipo_Usuario.Equals("Alumno")
                               && a.U_Habilitado == 1
                               select new UsuarioDTO
                               {
                                   ID = a.ID,
                                   Nombre = a.Nombre,
                                   Email = a.Email,
                                   Telefono = a.Telefono,
                                   Direccion = a.Direccion,
                                   Fecha_Nacimiento = a.Fecha_Nacimiento,
                                   Tipo_Usuario = u.Tipo_Usuario
                               }).ToList();

                    profesores = (from a in bd.Usuarios
                                  join u in bd.TipoUsuarios
                                  on a.TipoUsuarioID equals u.ID
                                  where u.Tipo_Usuario.Equals("Profesor")
                                  && a.U_Habilitado == 1
                                  select new UsuarioDTO
                                  {
                                      ID = a.ID,
                                      Nombre = a.Nombre,
                                      Email = a.Email,
                                      Telefono = a.Telefono,
                                      Direccion = a.Direccion,
                                      Fecha_Nacimiento = a.Fecha_Nacimiento,
                                      Tipo_Usuario = u.Tipo_Usuario
                                  }).ToList();

                    foreach (var item in profesores)
                    {
                        alumnos.Add(item);
                    }
                }


            }
            return View(alumnos);
        }

        [Admin]
        [HttpGet]
        public ActionResult Agregar()
        {
            listaTipoUsuario();
            return View();
        }

        [Admin]
        [HttpPost]
        public ActionResult Agregar(UsuarioDTO alumno)
        {
            alumno.mensaje = "";

            try
            {
                if (!ModelState.IsValid )
                {
                    listaTipoUsuario();
                    return View(alumno);
                }

                using (bd = new ApplicationDBContext())
                {
                    int cantidad = 0;
                    #region Cifrar contraseña
                    SHA256Managed sha = new SHA256Managed();
                    byte[] byteContra = Encoding.Default.GetBytes(alumno.Password);
                    byte[] byteContraCifrada = sha.ComputeHash(byteContra);
                    string passwordCifrado = BitConverter.ToString(byteContraCifrada).Replace("-", "");
                    #endregion

                    #region Imagen
                    byte[] fotoBD = null;
                    //if (foto != null)
                    //{
                    //    BinaryReader lector = new BinaryReader(foto.InputStream);
                    //    fotoBD = lector.ReadBytes((int)foto.ContentLength);
                    //}
                    #endregion

                    cantidad = bd.Usuarios.Where(n => n.Email.Equals(alumno.Email)).Count();

                    if (cantidad >= 1)
                    {
                        alumno.mensaje = "El usuario ya existe";
                        listaTipoUsuario();
                        return View(alumno);
                    }
                    else
                    {
                        Usuario al = new Usuario
                        {
                            ID = alumno.ID,
                            Nombre = alumno.Nombre,
                            Email = alumno.Email,
                            Telefono = alumno.Telefono,
                            Direccion = alumno.Direccion,
                            Fecha_Nacimiento = alumno.Fecha_Nacimiento,
                            Fecha_Registro = DateTime.Now,
                            Password = passwordCifrado,
                            TipoUsuarioID = alumno.TipoUsuarioID,
                            U_Habilitado = 1
                        };

                        bd.Usuarios.Add(al);
                        bd.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            UsuarioDTO alumnoDTO = new UsuarioDTO();

            using (bd = new ApplicationDBContext())
            {
                Usuario alumno = bd.Usuarios.Where(al => al.ID.Equals(id)).First();

                alumnoDTO.ID = alumno.ID;
                alumnoDTO.Nombre = alumno.Nombre;
                alumnoDTO.Email = alumno.Email;
                alumnoDTO.Telefono = alumno.Telefono;
                alumnoDTO.Direccion = alumno.Direccion;
                alumnoDTO.Fecha_Nacimiento = alumno.Fecha_Nacimiento;
            }

            return View(alumnoDTO);
        }

        [HttpPost]
        public ActionResult Editar(UsuarioDTO alumnoDTO)
        {
            if (!ModelState.IsValid)
            {
                listaTipoUsuario();
                return View(alumnoDTO);
            }

            using (bd = new ApplicationDBContext())
            {
                Usuario alumno = bd.Usuarios.Where(i => i.ID.Equals(alumnoDTO.ID)).First();

                alumno.ID = alumnoDTO.ID;
                alumno.Nombre = alumnoDTO.Nombre;
                alumno.Email = alumnoDTO.Email;
                alumno.Telefono = alumnoDTO.Telefono;
                alumno.Direccion = alumnoDTO.Direccion;
                alumno.Fecha_Nacimiento = alumnoDTO.Fecha_Nacimiento;

                bd.SaveChanges();

            }
          

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (bd = new ApplicationDBContext())
            {
                Usuario alumno = bd.Usuarios.Where(a => a.ID.Equals(id)).First();
                alumno.U_Habilitado = 0;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public void listaTipoUsuario()
        {
            List<SelectListItem> tipoUsuarios;
            List<SelectListItem> profesores;

            if (Session["Administrador"] != null)
            {
                using (bd = new ApplicationDBContext())
                {


                    tipoUsuarios = (from u in bd.TipoUsuarios
                                    where u.U_habilitado == 1
                                    && u.ID == 1
                                    select new SelectListItem
                                    {
                                        Text = u.Tipo_Usuario,
                                        Value = u.ID.ToString()
                                    }).ToList();

                    profesores = (from u in bd.TipoUsuarios
                                    where u.U_habilitado == 1
                                    && u.ID == 2
                                    select new SelectListItem
                                    {
                                        Text = u.Tipo_Usuario,
                                        Value = u.ID.ToString()
                                    }).ToList();

                    tipoUsuarios.Add(profesores.First());

                }
                tipoUsuarios.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.usuarios = tipoUsuarios;
            }

            if (Session["Bibliotecario"] != null)
            {
                List<SelectListItem> tipoUsuarios1;
                using (bd = new ApplicationDBContext())
                {
                    tipoUsuarios = (from u in bd.TipoUsuarios
                                    where u.U_habilitado == 1
                                    && u.ID == 1

                                    select new SelectListItem
                                    {
                                        Text = u.Tipo_Usuario,
                                        Value = u.ID.ToString()
                                    }).ToList();

                    tipoUsuarios1 = (from u in bd.TipoUsuarios
                                     where u.U_habilitado == 1
                                     && u.ID == 2

                                     select new SelectListItem
                                     {
                                         Text = u.Tipo_Usuario,
                                         Value = u.ID.ToString()
                                     }).ToList();

                    tipoUsuarios.Add(tipoUsuarios1.First());
                }

                tipoUsuarios.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.usuarios = tipoUsuarios;

            }
        }
    }

}