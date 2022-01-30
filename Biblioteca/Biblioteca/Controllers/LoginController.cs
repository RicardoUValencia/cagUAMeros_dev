using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
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
        public ActionResult Perfil()
        {

            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = (Usuario)Session["Usuario"];
            var nuevoUsuario = new Usuario();
            using (bd = new ApplicationDBContext())
            {
                nuevoUsuario = bd.Usuarios.Where(i => i.ID.Equals(user.ID)).First();
            }
            user = nuevoUsuario;
            UsuarioDTO usuario = new UsuarioDTO
            {
                Direccion = user.Direccion,
                Email = user.Email,
                Fecha_Nacimiento = user.Fecha_Nacimiento,
                ID = user.ID,
                Fecha_Registro = user.Fecha_Registro,
                Nombre = user.Nombre,
                Telefono = user.Telefono


            };

            //tabla de prestamos
            List<PrestamoDTO> prestamos = null;
            var nombreUsuario = usuario.Nombre;
            //consulta para recuperar los prestamos del usuario
            using(bd = new ApplicationDBContext())
            {
                prestamos = (from p in bd.Prestamos
                             join u in bd.Usuarios
                             on p.UsuarioID equals u.ID
                             join l in bd.Libros on p.LibroID equals l.ID
                             where u.Nombre.Contains(nombreUsuario)
                             select new PrestamoDTO
                             {
                                 ID = p.ID,
                                 nombreUsuario = u.Nombre,
                                 tituloLibro = l.Titulo,
                                 Fecha_Devolucion = p.Fecha_Devolucion,
                                 Fecha_Prestamo = p.Fecha_Prestamo

                             }).ToList();
            }
            usuario.prestamos = prestamos;

            return View(usuario);
        }

        
        public ActionResult EnviarTabla( String nombre, String email, List<PrestamoDTO> prestamos)
        {
            
            //prestamos = null;
            
            
            

           
            using (bd = new ApplicationDBContext())
               {
                  prestamos = (from p in bd.Prestamos
                                 join u in bd.Usuarios
                                 on p.UsuarioID equals u.ID
                                 join l in bd.Libros on p.LibroID equals l.ID
                                 where u.Nombre.Contains(nombre)
                                 select new PrestamoDTO
                                 {
                                     ID = p.ID,
                                     nombreUsuario = u.Nombre,
                                     tituloLibro = l.Titulo,
                                     Fecha_Devolucion = p.Fecha_Devolucion,
                                     Fecha_Prestamo = p.Fecha_Prestamo

                                 }).ToList();
              }
            
           
            String body ="<h2> Hola "+ nombre +", le proporcionamos la información acerca de sus prestamos</h2>"+
                            "<table>" +
                                 "<tr>" +
                                    "<th>Libro</th>" +
                                    "<th>Fecha de prestamo</th>" +
                                    "<th>Fecha de devolucion</th>"
                                +"</tr>";




           foreach (var item in prestamos)
            {
                body=body+ "" +
                    "<tr>" +
                        "<td>" +
                            item.tituloLibro+
                         "</td>"+
                          "<td>" +
                            item.Fecha_Prestamo.ToString("dddd, dd MMMM yyyy") +
                         "</td>"+
                          "<td>" +
                            item.Fecha_Devolucion.ToString("dddd, dd MMMM yyyy") +
                         "</td>"+
                         "</tr>";

            }
            body = body + "</table>";
            if(prestamos.Count==0)
            {
                body = "No tiene prestamos";
            }

           
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Credentials = new NetworkCredential("bibliotecadig.is@gmail.com", "asd?123456")
            };

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("blibliotecadig.is@gmail.com", "Biblioteca Digital");
            mail.To.Add(new MailAddress(email));
            mail.Subject = "Información acerca de sus prestamos";
            mail.IsBodyHtml = true;
            mail.Body = body;
     
            
            smtp.Send(mail);
            if(body=="No tiene prestamos")
            {
                return RedirectToAction("Index", "Buscar");
            }
            else
            {
                return RedirectToAction("Perfil","Login");
            }
            
            
           
            
            
            
           
            //System.Web.Mvc.ActionResult

        }




    }
}