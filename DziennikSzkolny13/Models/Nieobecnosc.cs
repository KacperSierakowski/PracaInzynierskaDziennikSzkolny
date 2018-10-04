using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class Nieobecnosc
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Data nieobecności")]
        [Required(ErrorMessage = "Nauczyciel jest wymagany")]
        public string Data { get; set; }
        [Display(Name = "Uczeń")]
        [Required(ErrorMessage = "Uczeń jest wymagany")]
        public int UczenID { get; set; }
        public virtual Uczen OcenaUczen { get; set; }
        [Display(Name = "Nauczyciel")]
        [Required(ErrorMessage = "Nauczyciel jest wymagany")]
        public int NauczycielID { get; set; }
        public virtual Nauczyciel przedmiotNauczyciel { get; set; }
    }
}