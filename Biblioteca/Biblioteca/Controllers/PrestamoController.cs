using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Filters;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

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

        protected void Correo_infoprestamo(UsuarioDTO usinfo, Prestamo prestamo, Libro libro)
        //UsuarioDTO usfino para mandar correo a otros usuarios.
        {
            //Mensaje que se mandará al alumno que este solicitando el prestamo, en la fecha el .
            string body =
            "<body>" +
                "<h1> Hola"+ usinfo.Nombre +", le proporcionamos la información acerca de su prestamo efectuado el día de hoy " + DateTime.Now.ToString("dddd, dd MMMM yyyy") + "</h1>"+
                "<table> <tr>" +
                "<th> Título</th >  <th> Autor</th>  <th>Edición</th> <th>Fecha de Devolución</th><tr>"+
                "<tr>"+ 
                "<td>" + libro.Titulo + "</td>" + 
                "<td>" + libro.Nombre_Autor + "</td>" + 
                "<td>" + libro.Edicion + "</td>" +
                "<td>" + prestamo.Fecha_Devolucion.ToString("dddd, dd MMMM yyyy") + "</td>" +
                "<tr>"+
                "</table>"+
                "<br/><br/><span>Quedamos a su disposición a través de nuestro apartado de contacto, saludos cordiales.</span>"+
            "</body>";

            //checar para después 
            //var pass = System.Configuration.ConfigurationManager.AppSettings["email-password"].ToString();
            //var from = System.Configuration.ConfigurationManager.AppSettings["sender"].ToString();

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Credentials = new NetworkCredential("bibliotecadig.is@gmail.com", "asd?123456")
            };

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("blibliotecadig.is@gmail.com", "Biblioteca Digital");
            mail.To.Add(new MailAddress(usinfo.Email));
            mail.Subject = "Información acerca de su prestamo";
            mail.IsBodyHtml = true;
            mail.Body = body;

            smtp.Send(mail);

        }

        public void Deuda(PrestamoDTO prestamo, UsuarioDTO usuario)
        {
            if (DateTime.Compare(prestamo.Fecha_Devolucion, DateTime.Now) < 0)
            {
                usuario.Deuda = usuario.Deuda + 50;
            }
        }

        
        public ActionResult Prestamo(int id)
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
                    var usuarioinfo = (from u in bd.Usuarios where u.ID == prestamo.UsuarioID
                                              select new UsuarioDTO
                                              {
                                                  Email = u.Email,
                                                  Nombre = u.Nombre
                                              }).FirstOrDefault(); 


                    if (libro.Total_Actual > 0)
                    {
                        libro.Total_Actual = libro.Total_Actual - 1;
                        bd.SaveChanges();
                        bd.Prestamos.Add(prestamo);
                        bd.SaveChanges();

                        //método el correo del prestamo
                        Correo_infoprestamo(usuarioinfo, prestamo, libro);


                        return RedirectToAction("Index", "Buscar");

                    }
                    else
                    {
                        //List<PrestamoDTO> prestamos = null;
                       
                        ViewBag.mensaje = "El libro que solicitaste no tiene disponibilidad";
                        return RedirectToAction("Index", "Buscar");
                        //return View("../Views/Buscar/Index.cshtml");
                    }

                   
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
