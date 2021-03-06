﻿using MagTech.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MagTech.Models
{
    public class KoszykZadanie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int NaglowekId { get; set; }
        [ForeignKey("NaglowekId")]
        public NaglowekKoszykZadania Naglowek { get; set; }
        [Required]
        public string Proces { get; set; }
        [Required]
        public int ArtykulId { get; set; }
        [ForeignKey("ArtykulId")]
        public BazaArtykulow Artykul { get; set; }
        public int Ilosc { get; set; }
        public string Koszty { get; set; }
    }
}
