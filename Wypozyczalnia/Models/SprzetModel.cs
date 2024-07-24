using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wypozyczalnia.Models
{
    public class SprzetModel
    {
        [Key]
        [Display(Name = "Id sprzętu ")]
        public int SprzetId { get; set; }

        [Required(ErrorMessage ="Nazwa jest wymagana!")]
        [Display(Name = "Nazwa ")]
        public string SprzetNazwa { get; set; }



        [ForeignKey("KategoriaId")]
        [Display(Name = "Kategoria ")]
        public int? KategoriaId { get; set; }
        public KategoriaModel Kategoria { get; set; }





        [Required(ErrorMessage = "Cena jest wymagana!")]
        [Display(Name = "Cena za dzień ")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Cena musi być liczbą dodatnią z opcjonalną separatorem jako KROPKA . ")]
        public decimal CenaZaDzien { get; set; }








        [Required]
        [Display(Name = "Dostępność")]
        public bool CzyDostepne { get; set; }


        public ICollection<WypozyczenieModel> Wypozyczenia { get; set; }
    }
}
