namespace WebApplication1.Models
{
    public class CommandeViewModel
    {
       public int Id { get; set; }
        public DateTime DateCommande { get; set; }
        public Client? Client { get; set; }
        public int ClientId { get; set; }
        public Facture? Facture { get; set; }
        public int Qte { get; set; }
    }      
}
