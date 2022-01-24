namespace Biblioteca.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deuda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "Deuda", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Deuda");
        }
    }
}
