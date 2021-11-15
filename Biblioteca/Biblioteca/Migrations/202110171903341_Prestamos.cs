namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prestamos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prestamo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LibroID = c.Int(nullable: false),
                        UsuarioID = c.Int(nullable: false),
                        Fecha_Prestamo = c.DateTime(nullable: false),
                        Fecha_Devolucion = c.DateTime(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        P_Habilitado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Libro", t => t.LibroID, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.UsuarioID, cascadeDelete: true)
                .Index(t => t.LibroID)
                .Index(t => t.UsuarioID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prestamo", "UsuarioID", "dbo.Usuario");
            DropForeignKey("dbo.Prestamo", "LibroID", "dbo.Libro");
            DropIndex("dbo.Prestamo", new[] { "UsuarioID" });
            DropIndex("dbo.Prestamo", new[] { "LibroID" });
            DropTable("dbo.Prestamo");
        }
    }
}
