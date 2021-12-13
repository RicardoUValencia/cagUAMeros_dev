namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sugerencias : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sugerencia",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Email = c.String(),
                        Comentario = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sugerencia");
        }
    }
}
