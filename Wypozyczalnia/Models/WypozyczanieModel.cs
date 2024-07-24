using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Wypozyczalnia.Models
{
    public class WypozyczenieModel
    {
        [Key]
        public int WypozyczenieId { get; set; }

        [Required]
        [ForeignKey("SprzetId")]
        [Display(Name = "Id sprzetu ")]
        public int SprzetId { get; set; }
        public SprzetModel? Sprzet { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data wypożyczenia ")]  
        public DateTime DataWypozyczenia { get; set; }

        
        [Display(Name = "Data zwrotu ")]
        [DataType(DataType.Date)]
        public DateTime DataZwrotu { get; set; }


        [ForeignKey("User")]
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }


        [DataType(DataType.Currency)]
        [Display(Name = "Cena całkowita wypozyczenia ")]
        public decimal CenaCalkowita { get; set; }

        public bool Status { get; set; }
    }

}


