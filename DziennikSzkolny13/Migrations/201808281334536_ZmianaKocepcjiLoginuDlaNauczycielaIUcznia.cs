namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZmianaKocepcjiLoginuDlaNauczycielaIUcznia : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Uczens", "LoginUcznia");
            DropColumn("dbo.Nauczyciels", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Nauczyciels", "Login", c => c.String(nullable: false));
            AddColumn("dbo.Uczens", "LoginUcznia", c => c.String(nullable: false));
        }
    }
}
