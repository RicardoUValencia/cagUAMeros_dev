namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Imagen_y_pdf_de_libros : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Libro", "img", c => c.Binary());
            AddColumn("dbo.Libro", "PDF", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Libro", "PDF");
            DropColumn("dbo.Libro", "img");
        }
    }
}
