using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class DziennikSzkolny13DB : DbContext
    {
        public DbSet<Uczen> Uczens { get; set; }
        public DbSet<Klasa> Klasas { get; set; }
        public DbSet<Ocena> Ocenas { get; set; }
        public DbSet<Przedmiot> Przedmiots { get; set; }
        public DbSet<Nauczyciel> Nauczyciels { get; set; }
        public DbSet<Nieobecnosc> Nieobecnoscs { get; set; }
        public DbSet<Plik> Pliks { get; set; }
    }
}