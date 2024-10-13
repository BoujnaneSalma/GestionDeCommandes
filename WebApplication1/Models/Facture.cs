using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Facture
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "la date est requise")]
        public DateTime DateFacture { get; set; }

        public Commande? Commande { get; set; }
        public int? CommandeId { get; set; }

    }
}
