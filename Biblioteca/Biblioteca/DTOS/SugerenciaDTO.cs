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
        [Display(Name = "Nombre")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Correo")]

        [StringLength(500, ErrorMessage = "La longitud maxima es de 500 caracteres")]
        public string Comentario { get; set; }
        [Required]
        [Display(Name = "Comentario")]
    }
}
