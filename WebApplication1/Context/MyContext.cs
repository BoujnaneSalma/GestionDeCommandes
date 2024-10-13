using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class MyContext : IdentityDbContext
    {
        public MyContext(DbContextOptions<MyContext> opt) : base(opt)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Commande> Commandes { get; set; }

        public DbSet<Facture> Factures { get; set; }

        public DbSet<LigneCommande> LigneCommandes { get; set; }

        public DbSet<Produit> Produits { get; set; }

    }
}
