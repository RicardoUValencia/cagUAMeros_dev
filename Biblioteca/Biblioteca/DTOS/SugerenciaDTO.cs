using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [StringLength(500, ErrorMessage = "La longutid maxima es de 500 caracteres")]
        public string Comentario { get; set; }
    }
}
