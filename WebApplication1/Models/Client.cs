using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le Nom est requise")]
        public string Name { get; set; }

        public string Prenom { get; set; }

        public int Tel { get; set; }


        public IList<Commande>? Commandes { get; set; }
    }
}
