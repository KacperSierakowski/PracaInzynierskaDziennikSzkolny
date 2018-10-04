namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pliki2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pliks", "NazwaPliku", c => c.Int(nullable: false));
            AddColumn("dbo.Pliks", "UrlPliku", c => c.Int(nullable: false));
            DropColumn("dbo.Pliks", "WartoscOceny");
            DropColumn("dbo.Pliks", "Rozmiar");
            DropColumn("dbo.Pliks", "WartoscPliku");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pliks", "WartoscPliku", c => c.Binary());
            AddColumn("dbo.Pliks", "Rozmiar", c => c.Int(nullable: false));
            AddColumn("dbo.Pliks", "WartoscOceny", c => c.Int(nullable: false));
            DropColumn("dbo.Pliks", "UrlPliku");
            DropColumn("dbo.Pliks", "NazwaPliku");
        }
    }
}
