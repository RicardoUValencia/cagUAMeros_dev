using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;

namespace Biblioteca.DTOS
{
    public class SugerenciaDTO
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Correo")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Comentario")]

        [StringLength(500, ErrorMessage = "La longitud maxima es de 500 caracteres")]
        public string Comentario { get; set; }
       
    }
    internal class MailMessages
    {
        public MailMessages()
        {
            string Nombre, Email, Comentario;
            Nombre = Console.ReadLine();
            Email = Console.ReadLine();
            Comentario = Console.ReadLine();
            MailMessages mail = new MailMessages();
            //mail.From = new MailAddress(mail.ToString());
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            //smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("", "");
        }
    }
}
