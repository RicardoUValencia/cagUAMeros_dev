using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class TipoUsuario
    {
        public int ID { get; set; }
        public string Tipo_Usuario { get; set; }
        public int U_habilitado { get; set; }
    }
}