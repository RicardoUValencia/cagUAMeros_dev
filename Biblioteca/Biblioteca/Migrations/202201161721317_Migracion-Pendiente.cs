namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionPendiente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carrusel",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Carrusel");
        }
    }
}
