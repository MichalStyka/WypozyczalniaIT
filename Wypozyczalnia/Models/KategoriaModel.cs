using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wypozyczalnia.Models
{
    public class KategoriaModel
    {
        [Key]
        public int KategoriaId { get; set; }


        [Required]
        [Display(Name ="Nazwa Kategorii: ")]
        public string KategoriaNazwa { get; set; }

        public ICollection<SprzetModel> Sprzety { get; set; }
    }
}
