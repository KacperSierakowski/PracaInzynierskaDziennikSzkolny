namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaNauczycielaNaPrzedmiotWNieobecnosci : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Nieobecnoscs", "NauczycielID", "dbo.Nauczyciels");
            DropIndex("dbo.Nieobecnoscs", new[] { "NauczycielID" });
            AddColumn("dbo.Nieobecnoscs", "PrzedmiotID", c => c.Int(nullable: false));
            CreateIndex("dbo.Nieobecnoscs", "PrzedmiotID");
            AddForeignKey("dbo.Nieobecnoscs", "PrzedmiotID", "dbo.Przedmiots", "ID", cascadeDelete: true);
            DropColumn("dbo.Nieobecnoscs", "NauczycielID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Nieobecnoscs", "NauczycielID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Nieobecnoscs", "PrzedmiotID", "dbo.Przedmiots");
            DropIndex("dbo.Nieobecnoscs", new[] { "PrzedmiotID" });
            DropColumn("dbo.Nieobecnoscs", "PrzedmiotID");
            CreateIndex("dbo.Nieobecnoscs", "NauczycielID");
            AddForeignKey("dbo.Nieobecnoscs", "NauczycielID", "dbo.Nauczyciels", "ID", cascadeDelete: true);
        }
    }
}
