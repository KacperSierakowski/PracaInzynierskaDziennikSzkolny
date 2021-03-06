﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class Uczen
    {
        [Key]
        public int ID { get; set; }

        ////[Display(Name = "Login Ucznia")]
        //[Required(ErrorMessage = "Login ucznia jest wymagany")]
        //public string LoginUcznia { get; set; }
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię ucznia jest wymagane")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko ucznia jest wymagane")]
        public string Nazwisko { get; set; }


        [Display(Name = "ID - Imię i nazwisko")]
        public string PelnaNazwa
        {
            get
            {
                return String.Format("{0} {1}, {2}",ID, Imie, Nazwisko);
            }
        }

        [Display(Name = "Numer Telefonu")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Numer Telefonu nie jest poprawny (000-000-000)")]
        public int NumerTelefonu { get; set; }

        [Display(Name = "Adres Zamieszkania")]
        [Required(ErrorMessage = "Adres ucznia jest wymagany")]
        [StringLength(90, MinimumLength = 3, ErrorMessage = "Adres zbyt długi")]
        public string Adres { get; set; }

        [Display(Name = "login / Adres E-mail")]
        [Required(ErrorMessage = "Adres E-mail ucznia jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny Email (xxx@xx.xx)")]
        public string Email { get; set; }

        public int KlasaID { get; set; }
        public virtual Klasa klasaUcznia { get; set; }

        public virtual ICollection<Ocena> OcenyUcznia { get; set; }
        public virtual ICollection<Nieobecnosc> NieobecnosciUcznia { get; set; }

    }
}