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
    [Acceder]
    public class EmpleadoController : Controller
    {
        private ApplicationDBContext bd;
        private UsuarioDTO empleadoDTOVal;
        // GET: Empleado
        public ActionResult Index(UsuarioDTO empleado)
        {
            List<UsuarioDTO> empleadoDTO = null;
            List<UsuarioDTO> empleadosFiltrados = null;
            List<UsuarioDTO> administradorDTO = null;

            empleadoDTOVal = empleado;

            string nombreEmpleado = empleado.Nombre;

            using (bd = new ApplicationDBContext())
            {
                empleadoDTO = (from a in bd.Usuarios
                               join u in bd.TipoUsuarios
                               on a.TipoUsuarioID equals u.ID
                               where a.U_Habilitado == 1
                               && u.Tipo_Usuario.Equals("Bibliotecario")
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

                administradorDTO = (from a in bd.Usuarios
                                    join u in bd.TipoUsuarios
                                    on a.TipoUsuarioID equals u.ID
                                    where u.Tipo_Usuario.Equals("Administrador")
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

                foreach (var item in administradorDTO)
                {
                    empleadoDTO.Add(item);
                }

                if (empleado.Nombre == null && empleado.Tipo_Usuario == null)
                {
                    empleadosFiltrados = empleadoDTO;
                }
                else
                {
                    Predicate<UsuarioDTO> prediTitulo = new Predicate<UsuarioDTO>(buscarEmpleado);
                    empleadosFiltrados = empleadoDTO.FindAll(prediTitulo);
                }

            }

            return View(empleadosFiltrados);
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            listaTipoUsuario();
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(UsuarioDTO empleadoDTO)
        {
            if (!ModelState.IsValid)
            {
                listaTipoUsuario();
                return View(empleadoDTO);
            }

            #region Cifrar contraseña
            SHA256Managed sha = new SHA256Managed();
            byte[] byteContra = Encoding.Default.GetBytes(empleadoDTO.Password);
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

            using (bd = new ApplicationDBContext())
            {
                Usuario empleado = new Usuario()
                {
                    ID = empleadoDTO.ID,
                    Nombre = empleadoDTO.Nombre,
                    Direccion = empleadoDTO.Direccion,
                    Email = empleadoDTO.Email,
                    Fecha_Nacimiento = empleadoDTO.Fecha_Nacimiento,
                    TipoUsuarioID = empleadoDTO.TipoUsuarioID,
                    Telefono = empleadoDTO.Telefono,
                    Fecha_Registro = DateTime.Now,
                    Password = passwordCifrado,
                    U_Habilitado = 1
                };

                bd.Usuarios.Add(empleado);
                bd.SaveChanges();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            UsuarioDTO empleadoDTO = null;

            using (bd = new ApplicationDBContext())
            {
                Usuario empleado = bd.Usuarios.Where(i => i.ID.Equals(id)).First();

                empleadoDTO = new UsuarioDTO()
                {
                    ID = empleado.ID,
                    Direccion = empleado.Direccion,
                    Email = empleado.Email,
                    Fecha_Nacimiento = empleado.Fecha_Nacimiento,
                    Nombre = empleado.Nombre,
                    Telefono = empleado.Telefono
                };

            }
            return View(empleadoDTO);
        }

        [HttpPost]
        public ActionResult Editar(UsuarioDTO empleadoDTO)
        {
            if (!ModelState.IsValid)
            {
                listaTipoUsuario();
                return View(empleadoDTO);
            }

            using (bd = new ApplicationDBContext())
            {
                Usuario empleado = bd.Usuarios.Where(i => i.ID.Equals(empleadoDTO.ID)).First();

                empleado.ID = empleadoDTO.ID;
                empleado.Nombre = empleadoDTO.Nombre;
                empleado.Telefono = empleadoDTO.Telefono;
                empleado.Fecha_Nacimiento = empleadoDTO.Fecha_Nacimiento;
                empleado.Direccion = empleadoDTO.Direccion;
                empleado.Email = empleadoDTO.Email;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            using (bd = new ApplicationDBContext())
            {
                Usuario empleado = bd.Usuarios.Where(e => e.ID.Equals(id)).First();

                empleado.U_Habilitado = 0;

                bd.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private bool buscarEmpleado(UsuarioDTO empleadoDTO)
        {
            bool nombreEmpleado = true;
            bool puestoEmpleado = true;

            if (empleadoDTOVal.Nombre != null)
            {
                nombreEmpleado = empleadoDTO.Nombre.ToString().Contains(empleadoDTOVal.Nombre);
                if (!nombreEmpleado)
                {
                    empleadoDTOVal.Nombre = empleadoDTOVal.Nombre.ToUpper();
                    nombreEmpleado = empleadoDTO.Nombre.ToString().Contains(empleadoDTOVal.Nombre);
                }
            }

            if (empleadoDTOVal.Tipo_Usuario != null)
            {
                puestoEmpleado = empleadoDTO.Tipo_Usuario.ToString().Contains(empleadoDTOVal.Tipo_Usuario);
                if (!puestoEmpleado)
                {
                    empleadoDTOVal.Tipo_Usuario = empleadoDTOVal.Tipo_Usuario.ToLower();
                    puestoEmpleado = empleadoDTO.Tipo_Usuario.ToString().Contains(empleadoDTOVal.Tipo_Usuario);
                }
            }



            return (nombreEmpleado && puestoEmpleado);
        }

        public void listaTipoUsuario()
        {
            List<SelectListItem> tipoUsuarios;

            using (bd = new ApplicationDBContext())
            {
                tipoUsuarios = (from u in bd.TipoUsuarios
                                where u.U_habilitado == 1
                                select new SelectListItem
                                {
                                    Text = u.Tipo_Usuario,
                                    Value = u.ID.ToString()
                                }).ToList();
            }

            tipoUsuarios.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            ViewBag.usuarios = tipoUsuarios;
        }
    }
}