using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class LibroDTO
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Autor")]
        [StringLength(100, ErrorMessage = "La longitud maxima es 100 caracteres")]
        public string Nombre_Autor { get; set; }

        [Required]
        [Display(Name = "Titulo")]
        [StringLength(100, ErrorMessage = "La longitud maxima es 100 caracteres")]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Año publicación")]
        public int Anio_Publicacion { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        [IntegerValidator(MaxValue = 50, MinValue = 1)]
        public int Cantidad { get; set; }

        [Required]
        [Display(Name = "Estante")]
        [StringLength(50, ErrorMessage = "La longitud maxima con 50 caracteres")]
        public string Ubicacion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de registro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Registro { get; set; }

        [Required]
        [Display(Name = "Edición")]
        public string Edicion { get; set; }

        
        [Display(Name = "Idioma")]
        public string lenguaje { get; set; }

        [Display(Name = "Categoria")]
        public int IDCategoria { get; set; }

        #region Propiedades adicionales

        [Display(Name = "Categoria")]
        [StringLength(50, ErrorMessage = "La longitud maxima con 50 caracteres")]
        public string categoria_Libro { get; set; }

        public byte[] Foto { get; set; }

        public int Total_Actual { get; set; }
        public string Disponibles { get; set; }
        public string Titulo_Autor { get; set; }
        public string mensaje { get; set; }
        public string mensajeEstante { get; set; }
        #endregion
    }
}