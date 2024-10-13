namespace WebApplication1.Models
{
    public class ProduitViewModel
    {
        public int Id { get; set; }
        public string? Libelle { get; set; }
        public int PU { get; set; }
        public string? Image { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public int Qte { get; set; }
    }
}
