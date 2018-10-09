using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class Klasa
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Nazwa Klasy")]
        [Required(ErrorMessage = "Nazwa Klasy jest wymagana")]
        public string NazwaKlasy { get; set; }

        [Display(Name = "Profil Klasy")]
        public string ProfilKlasy { get; set; }
        public virtual ICollection<Uczen> UczniowieKlasy { get; set; }
        //public virtual Uczen UczenKlasy { get; set; }
    }
}