using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagTech.Models
{
   public class NaglowekKoszykZlecenie
    {
        [Key]   
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Priorytet { get; set; }
    }
}
