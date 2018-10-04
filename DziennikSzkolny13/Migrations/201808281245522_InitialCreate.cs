namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Klasas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NazwaKlasy = c.String(nullable: false),
                        ProfilKlasy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Uczens",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LoginUcznia = c.String(nullable: false),
                        Imie = c.String(nullable: false),
                        Nazwisko = c.String(nullable: false),
                        NumerTelefonu = c.Int(nullable: false),
                        Adres = c.String(nullable: false, maxLength: 90),
                        Email = c.String(nullable: false),
                        KlasaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Klasas", t => t.KlasaID, cascadeDelete: true)
                .Index(t => t.KlasaID);
            
            CreateTable(
                "dbo.Ocenas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WartoscOceny = c.Int(nullable: false),
                        UczenID = c.Int(nullable: false),
                        PrzedmiotID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Przedmiots", t => t.PrzedmiotID, cascadeDelete: true)
                .ForeignKey("dbo.Uczens", t => t.UczenID, cascadeDelete: true)
                .Index(t => t.UczenID)
                .Index(t => t.PrzedmiotID);
            
            CreateTable(
                "dbo.Przedmiots",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NazwaPrzedmiotu = c.String(),
                        NauczycielID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Nauczyciels", t => t.NauczycielID, cascadeDelete: true)
                .Index(t => t.NauczycielID);
            
            CreateTable(
                "dbo.Nauczyciels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Imie = c.String(nullable: false),
                        Nazwisko = c.String(nullable: false),
                        NumerTelefonu = c.Int(nullable: false),
                        Adres = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Nieobecnoscs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Data = c.String(nullable: false),
                        UczenID = c.Int(nullable: false),
                        NauczycielID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Uczens", t => t.UczenID, cascadeDelete: true)
                .ForeignKey("dbo.Nauczyciels", t => t.NauczycielID, cascadeDelete: true)
                .Index(t => t.UczenID)
                .Index(t => t.NauczycielID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Nieobecnoscs", "NauczycielID", "dbo.Nauczyciels");
            DropForeignKey("dbo.Nieobecnoscs", "UczenID", "dbo.Uczens");
            DropForeignKey("dbo.Ocenas", "UczenID", "dbo.Uczens");
            DropForeignKey("dbo.Przedmiots", "NauczycielID", "dbo.Nauczyciels");
            DropForeignKey("dbo.Ocenas", "PrzedmiotID", "dbo.Przedmiots");
            DropForeignKey("dbo.Uczens", "KlasaID", "dbo.Klasas");
            DropIndex("dbo.Nieobecnoscs", new[] { "NauczycielID" });
            DropIndex("dbo.Nieobecnoscs", new[] { "UczenID" });
            DropIndex("dbo.Przedmiots", new[] { "NauczycielID" });
            DropIndex("dbo.Ocenas", new[] { "PrzedmiotID" });
            DropIndex("dbo.Ocenas", new[] { "UczenID" });
            DropIndex("dbo.Uczens", new[] { "KlasaID" });
            DropTable("dbo.Nieobecnoscs");
            DropTable("dbo.Nauczyciels");
            DropTable("dbo.Przedmiots");
            DropTable("dbo.Ocenas");
            DropTable("dbo.Uczens");
            DropTable("dbo.Klasas");
        }
    }
}
