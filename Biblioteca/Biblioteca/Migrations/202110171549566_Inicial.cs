namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre_Categoria = c.String(),
                        C_habilitado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Libro",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDCategoria = c.Int(nullable: false),
                        Nombre_Autor = c.String(),
                        Titulo = c.String(),
                        Edicion = c.String(),
                        Anio_Publicacion = c.Int(nullable: false),
                        Fecha_Registro = c.DateTime(nullable: false),
                        Idioma = c.String(),
                        Ubicacion = c.String(),
                        Cantidad = c.Int(nullable: false),
                        Total_Actual = c.Int(nullable: false),
                        L_Habilitado = c.Int(nullable: false),
                        Categoria_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categoria", t => t.Categoria_ID)
                .Index(t => t.Categoria_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Libro", "Categoria_ID", "dbo.Categoria");
            DropIndex("dbo.Libro", new[] { "Categoria_ID" });
            DropTable("dbo.Libro");
            DropTable("dbo.Categoria");
        }
    }
}
