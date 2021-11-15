using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Libro
    {
        public int ID { get; set; }
        public int IDCategoria { get; set; }
        public string Nombre_Autor { get; set; }
        public string Titulo { get; set; }
        public string Edicion { get; set; }
        public int Anio_Publicacion { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public string Idioma { get; set; }
        public string Ubicacion { get; set; }
        public int Cantidad { get; set; }
        public int Total_Actual { get; set; }
        public int L_Habilitado { get; set; }

        public virtual Categoria Categoria { get; set; }
    }
}