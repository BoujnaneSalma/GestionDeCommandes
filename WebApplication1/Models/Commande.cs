using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication1.Models
{
    public class Commande
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "La date est requise")]
        public DateTime? DateCommande { get; set; }

        public Client? Client { get; set; }
        public int ClientId { get; set; }

        public Facture? Facture { get; set; }


        public IList<LigneCommande>? LigneCommandes { get; set; }


    }
}
