using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class Plik
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Nazwa Pliku")]
        public string NazwaPliku { get; set; }
        [Display(Name = "Plik")]
        public string UrlPliku { get; set; }
        //[Display(Name = "Plik")]
        //public byte[] WartoscPliku { get; set; }
    }
}