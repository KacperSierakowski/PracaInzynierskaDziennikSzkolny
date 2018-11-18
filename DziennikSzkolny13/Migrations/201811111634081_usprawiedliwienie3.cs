namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usprawiedliwienie3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Nieobecnoscs", "CzyUsprawiedliwiona", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Nieobecnoscs", "CzyUsprawiedliwiona");
        }
    }
}
