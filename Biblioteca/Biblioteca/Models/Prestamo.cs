using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Prestamo
    {
        public int ID { get; set; }
        public int LibroID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha_Prestamo { get; set; }
        public DateTime Fecha_Devolucion { get; set; }
        public int Cantidad { get; set; }
        public int P_Habilitado { get; set; }
        public virtual Libro Libro { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}