using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class CategoriaDTO
    {
        public int ID { get; set; }
        [Display(Name = "Nombre de la categoria")]
        public string Nombre_Categoria { get; set; }
        public int C_habilitado { get; set; }
        #region Propiedades de validacion
        public string mensaje { get; set; }
        #endregion
    }
}