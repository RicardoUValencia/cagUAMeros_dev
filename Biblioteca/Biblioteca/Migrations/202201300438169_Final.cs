namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prestamo", "Deuda", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prestamo", "Deuda");
        }
    }
}
