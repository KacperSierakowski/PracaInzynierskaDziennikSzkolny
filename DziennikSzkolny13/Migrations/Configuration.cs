namespace DziennikSzkolny13.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DziennikSzkolny13.Models.DziennikSzkolny13DB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "DziennikSzkolny13.Models.DziennikSzkolny13DB";
        }

        protected override void Seed(DziennikSzkolny13.Models.DziennikSzkolny13DB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
