using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    //[HttpPost]
    //[Authorize(Roles = "Administrador")]
    public class LoginController : Controller
    {
        private ApplicationDBContext bd;
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(UsuarioDTO alumnoDTO)
        {
            string mensaje = "";
            string correo = alumnoDTO.Email;
            string password = alumnoDTO.Password;

            #region Cifrar contraseña
            SHA256Managed sha = new SHA256Managed();
            byte[] byteContra = Encoding.Default.GetBytes(password);
            byte[] byteContraCifrada = sha.ComputeHash(byteContra);
            string passwordCifrado = BitConverter.ToString(byteContraCifrada).Replace("-", "");
            #endregion

            using (bd = new ApplicationDBContext())
            {
                int habilitado = bd.Usuarios.Where(a => a.U_Habilitado == 1 && a.Email.Equals(correo)).Count();

                if (habilitado == 0)
                {
                    mensaje = "El usuario no existe";
                    return View(alumnoDTO);
                }
                else
                {
                    int numeroVeces = bd.Usuarios.Where(a => a.Email.Equals(correo)
                                     && a.Password.Equals(passwordCifrado)).Count();

                    mensaje = numeroVeces.ToString();

                    if (mensaje.Equals("0"))
                    {
                        mensaje = "Usuario o contraseña incorrectos";
                    }
                    else
                    {
                        Usuario usuario = bd.Usuarios.Where(a => a.Email.Equals(correo)
                                                  && a.Password.Equals(passwordCifrado)).First();

                        if (usuario.TipoUsuarioID == 1 || usuario.TipoUsuarioID == 2)
                        {
                            Session["Usuario"] = usuario;
                        }
                        else if (usuario.TipoUsuarioID == 3)
                        {
                            Session["Bibliotecario"] = usuario;
                        }
                        else
                        {
                            UsuarioDTO administrador = (from a in bd.Usuarios
                                                        join u in bd.TipoUsuarios
                                                        on a.TipoUsuarioID equals u.ID
                                                        where a.U_Habilitado == 1
                                                        && u.Tipo_Usuario.Equals("Administrador")
                                                        select new UsuarioDTO
                                                        {
                                                            Nombre = a.Nombre,
                                                            Tipo_Usuario = u.Tipo_Usuario
                                                        }).First();

                            Session["Administrador"] = administrador;
                        }
                    }
                }

                if (!mensaje.Equals("1"))
                {

                    return View(alumnoDTO);
                }
            }

            return RedirectToAction("Index", "Home");
        }//Fin login

        public ActionResult CerrarSesion()
        {
            if (Session["Usuario"] != null)
            {
                Session["Usuario"] = null;
            }
            else if (Session["Administrador"] != null)
            {
                Session["Administrador"] = null;
            }
            else if (Session["Bibliotecario"] != null)
            {
                Session["Bibliotecario"] = null;
            }
            return RedirectToAction("Index", "Home");
        }

    }
}