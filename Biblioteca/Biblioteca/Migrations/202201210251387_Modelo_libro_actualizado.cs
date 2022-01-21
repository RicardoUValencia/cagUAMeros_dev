namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modelo_libro_actualizado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Libro", "NombreImg", c => c.String());
            AddColumn("dbo.Libro", "NombrePDF", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Libro", "NombrePDF");
            DropColumn("dbo.Libro", "NombreImg");
        }
    }
}
