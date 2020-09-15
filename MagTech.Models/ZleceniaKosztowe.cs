using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagTech.Models
{
    public class ZleceniaKosztowe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Linia { get; set; }
        public string Maszyna { get; set; }
        public string MPK { get; set; }
        public string Zlecenie { get; set; }
    }
}
