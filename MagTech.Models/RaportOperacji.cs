using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagTech.Models
{
   public class RaportOperacji
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        [MaxLength(1)]
        public string Proces { get; set; }
        [MaxLength(20)]
        public string NumerArtykulu { get; set; }
        public int Ilosc { get; set; }
        public string Uzytkownik { get; set; }
        public string StanowiskoKosztow { get; set; }
        public string NumerMPK { get; set; }
        public string Zadanie { get; set; }
        public string MiejsceSkladowania { get; set; }
        public string WielkoscPojemnika { get; set; }
        public string Status { get; set; }    

    }
}
