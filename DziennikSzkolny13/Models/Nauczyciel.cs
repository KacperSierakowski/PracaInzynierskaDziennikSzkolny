using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class Nauczyciel
    {
        [Key]
        public int ID { get; set; }
        //[Display(Name = "Login Nauczyciela")]
        //[Required(ErrorMessage = "Login nauczyciela jest wymagany")]
        //public string Login { get; set; }
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię nauczyciela jest wymagane")]
        public string Imie { get; set; }
        [Required(ErrorMessage = "Nazwisko nauczyciela jest wymagane")]
        public string Nazwisko { get; set; }
        [Display(Name = "Numer Telefonu")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Numer Telefonu nie jest poprawny (000-000-000)")]
        public int NumerTelefonu { get; set; }
        [Display(Name = "Adres zamieszkania")]
        [Required(ErrorMessage = "Adres nauczyciela jest wymagany")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Adres zbyt długi")]
        public string Adres { get; set; }
        [Display(Name = "Login / Adres E-mail")]
        [Required(ErrorMessage = "Adres E-mail nauczyciela jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny Email (xxx@xx.xx)")]
        public string Email { get; set; }
        public virtual ICollection<Przedmiot> PrzedmiotyNauczyciela { get; set; }
    }
}