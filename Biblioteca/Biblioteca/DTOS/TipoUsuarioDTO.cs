using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class TipoUsuarioDTO
    {
        public int ID { get; set; }
        [Display(Name = "Tipo usuario")]
        public string Tipo_Usuario { get; set; }
        public int U_habilitado { get; set; }
    }
}