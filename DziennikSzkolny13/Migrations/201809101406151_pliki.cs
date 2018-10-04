namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pliki : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pliks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WartoscOceny = c.Int(nullable: false),
                        Rozmiar = c.Int(nullable: false),
                        WartoscPliku = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pliks");
        }
    }
}
