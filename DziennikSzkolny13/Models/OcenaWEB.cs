using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class OcenaWEB
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Wartość Oceny")]
        [Required(ErrorMessage = "Wymagana wartość oceny, którą wystawiamy")]
        public int WartoscOceny { get; set; }
        [Display(Name = "Uczeń")]
        [Required(ErrorMessage = "Wymagana nazwa Ucznia, któremu wystawiamy ocenę")]
        public int UczenID { get; set; }
        [Display(Name = "Przedmiot")]
        [Required(ErrorMessage = "Wymagana nazwa Przedmiotu, z którego wystawiamy ocenę")]
        public int PrzedmiotID { get; set; }
    }
}