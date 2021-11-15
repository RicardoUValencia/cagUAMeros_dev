namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Usuarios : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoUsuario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Tipo_Usuario = c.String(),
                        U_habilitado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TipoUsuarioID = c.Int(nullable: false),
                        Nombre = c.String(),
                        Email = c.String(),
                        Direccion = c.String(),
                        Telefono = c.String(),
                        Fecha_Nacimiento = c.DateTime(nullable: false),
                        Fecha_Registro = c.DateTime(nullable: false),
                        Password = c.String(),
                        U_Habilitado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TipoUsuario", t => t.TipoUsuarioID, cascadeDelete: true)
                .Index(t => t.TipoUsuarioID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "TipoUsuarioID", "dbo.TipoUsuario");
            DropIndex("dbo.Usuario", new[] { "TipoUsuarioID" });
            DropTable("dbo.Usuario");
            DropTable("dbo.TipoUsuario");
        }
    }
}
