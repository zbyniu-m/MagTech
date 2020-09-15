using System;
using System.ComponentModel.DataAnnotations;

namespace MagTech.Models
{
    public class BazaArtykulow
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(19)]
        public string NumerArtykulu { get; set; }
        [Required]      
        public int NumerRegalu { get; set; }
        [Required]      
        public int NumerPrzedzialu { get; set; }
        [Required]      
        public int NumerPolki { get; set; }
        [Required]      
        public int StanWMiejscuSkladowania { get; set; }
        [Required]       
        public int OznaczenieFifo { get; set; }
        [MaxLength(5)]
        public string WielkoscPojemnika { get; set; }       
        public int StanMinimalny { get; set; }       
        public int NumerSzeregu { get; set; }
        [MaxLength(40)]
        public string NazwaArtykulu { get; set; }
        [MaxLength(10)]
        public string GrupaMaterialowa { get; set; }
        [MaxLength(2)]
        public string Typ { get; set; }
        [MaxLength(10)]
        public string Tag { get; set; }
        [MaxLength(120)]
        public string DodatkoweInformacje { get; set; }
    }
}
