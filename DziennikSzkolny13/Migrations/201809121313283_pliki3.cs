namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pliki3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pliks", "NazwaPliku", c => c.String());
            AlterColumn("dbo.Pliks", "UrlPliku", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pliks", "UrlPliku", c => c.Int(nullable: false));
            AlterColumn("dbo.Pliks", "NazwaPliku", c => c.Int(nullable: false));
        }
    }
}
