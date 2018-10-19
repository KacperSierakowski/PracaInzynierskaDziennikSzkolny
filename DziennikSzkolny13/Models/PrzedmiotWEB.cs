using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class PrzedmiotWEB
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Nazwa Przedmiotu")]
        public string NazwaPrzedmiotu { get; set; }
        [Display(Name = "Nauczyciel prowadzący")]
        [Required(ErrorMessage = "Nauczyciel jest wymagany")]
        public int NauczycielID { get; set; }
    }
}