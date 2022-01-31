using Biblioteca.DBContext;
using Biblioteca.DTOS;
using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDBContext bd;
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

        public ActionResult About()
        {
            ViewBag.Message = "Este proyecto fue realizado por.";

            return View();
        }

        [HttpPost]

        public ActionResult Contact(SugerenciaDTO sugerenciaDTO)
        {
            using (var bd = new ApplicationDBContext())
            {
                Sugerencia sugerencia = new Sugerencia()
                {
                    //ID = sugerenciaDTO.ID,
                    Nombre = sugerenciaDTO.Nombre,
                    Email = sugerenciaDTO.Email,
                    Comentario = sugerenciaDTO.Comentario
                };

                if (!ModelState.IsValid)
                {
                        return View(sugerenciaDTO);
                }

                bd.Sugerencia.Add(sugerencia);
                bd.SaveChanges();
            }

            return RedirectToAction("Contact");
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        /*internal class MailMessages
        {
            public MailMessages()
            {
                MailMessages mail = new MailMessages();
                mail.From = new MailAddress(Email);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("", "");
            }
        }*/
    }
}
