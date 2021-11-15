using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class CategoriaDTO
    {
        public int ID { get; set; }
        public string Nombre_Categoria { get; set; }
        public int C_habilitado { get; set; }
        #region Propiedades de validacion
        public string mensaje { get; set; }
        #endregion
    }
}