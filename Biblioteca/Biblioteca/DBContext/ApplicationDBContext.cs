using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Biblioteca.DBContext
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext() : base("ApplicationDBContext")
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Sugerencia> Sugerencia { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}