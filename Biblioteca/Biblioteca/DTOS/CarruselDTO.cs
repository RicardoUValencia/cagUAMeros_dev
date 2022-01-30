using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class CarruselDTO
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "URL de la imagen")]
        public string ImagePath { get; set; }
        [Required]
        [Display(Name = "Titulo de la imagen")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Descripción de la imagen")]
        public string Descripcion { get; set; }
        public string Url { get; set; }
    }
}