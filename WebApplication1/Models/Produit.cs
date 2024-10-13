using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Produit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required(ErrorMessage = "le Libellé est requis")]
        public string? Libelle { get; set; }
        [Required(ErrorMessage = "le Prix Unitaire est requis")]
        public int PU { get; set; }
       
        [Required(ErrorMessage = "l'image' est requise")]
        public string? Image { get; set; }

        public Category? Category { get; set; }
        public int? CategoryId { get; set; }

        public List<LigneCommande>? LigneCommandes { get; set; }

    }
}
