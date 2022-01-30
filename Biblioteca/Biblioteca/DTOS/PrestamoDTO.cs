 using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class PrestamoDTO
    {
        public int ID { get; set; }

        public int LibroID { get; set; }

        public int UsuarioID { get; set; }

        [Display(Name = "Fecha del prestamo")]
        public DateTime Fecha_Prestamo { get; set; }

        [Display(Name = "Fecha de devolucion")]
        public DateTime Fecha_Devolucion { get; set; }

        public int Cantidad { get; set; }

        public int P_Habilitado { get; set; }

        public virtual Libro Libro { get; set; }

        public virtual Usuario Usuario { get; set; }

        public int Deuda { get; set; }


        #region Propiedades extra
        [Display(Name="Nombre de usuario")]
        public string nombreUsuario { get; set; }
        [Display(Name = "Titulo  del libro")]
        public string tituloLibro { get; set; }
       
        #endregion
    }
}