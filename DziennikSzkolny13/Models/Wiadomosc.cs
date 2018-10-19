using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DziennikSzkolny13.Models
{
    public class Wiadomosc
    {
        [Key]
        public int ID { get; set; }
        public int NadawcaID { get; set; }
        public int OdbiorcaID { get; set; }

    }
}