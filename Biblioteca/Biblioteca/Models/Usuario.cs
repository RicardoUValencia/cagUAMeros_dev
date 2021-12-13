using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public int TipoUsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public string Password { get; set; }
        public int U_Habilitado { get; set; }//Indica si el usuario esta activo
        public TipoUsuario TipoUsuario { get; set; }

        //Garantizará que el usuario tenga que iniciar sesión y se le asigne el rol de Administrador para poder ver la página.
        //public Usuario[] Administrador { get; set; }
        //public Usuario[] Everyone { get; set; }

        internal Usuario Obtener(object p)
        {
            throw new NotImplementedException();
        }
    }

}