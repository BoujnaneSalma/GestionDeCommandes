using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LigneCommande
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "la Quantité est requise")]
        public int Qte { get; set; }

        public Produit? Produit { get; set; }
        public int ProduitId { get; set; }

        public Commande? Commande { get; set; }
        public int CommandeId { get; set; }
    }
}
