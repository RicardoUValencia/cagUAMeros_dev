using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.DTOS
{
    public class UsuarioDTO
    {
        public int ID { get; set; }
        public int TipoUsuarioID { get; set; }//Tipo usuario ID
        [Required]
        [Display(Name = "Nombre completo")]
        [StringLength(100, ErrorMessage = "Longitud maxima 20 caracteres")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Correo")]
        [StringLength(50, ErrorMessage = "La longutid maxima es de 50 caracteres")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        [StringLength(10, ErrorMessage = "La longitud maxima es de 10 digitos")]
        public string Telefono { get; set; }


        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Nacimiento { get; set; }

        //[Required]
        [Display(Name = "Fecha de registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Registro { get; set; }

        
        [Display(Name = "Contraseña")]
        [MinLength(8, ErrorMessage = "La longitud minima son 8 caracteres")]
        public string Password { get; set; }

        #region Campos extra
        [Display(Name = "Tipo usuario")]
        public string Tipo_Usuario { get; set; }//Muestra el nombre de tipo usuario
        public string mensaje { get; set; }
        public string Puesto { get; set; }
        public List<PrestamoDTO> prestamos { get; set; }//se crea un atributo de lista para guardar los prestamos y asi poder usarlos
                                                        //en la vista del perfil
        #endregion
        
    }
}