using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CommandeController : Controller
    {
        MyContext db;
        public CommandeController(MyContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public IActionResult Add()
        {

            List<Client> clients = db.Clients.ToList();
            ViewBag.Client = clients;
            List<Category> categories = db.Categories.ToList();
            ViewBag.categorie = categories;

            List<LigneCommande> LC = db.LigneCommandes.ToList();
            ViewBag.LigneCommande = LC;

           CommandeViewModel model = new CommandeViewModel();
            model.DateCommande = DateTime.Now;

            return View(model);
        }
        [HttpPost]
        public IActionResult Add(CommandeViewModel model,string Libelle, int PU, string Image, int CategoryId)
        {
            List<Client> clients = db.Clients.ToList();
            ViewBag.Client = clients;
            List<Category> categories = db.Categories.ToList();
            ViewBag.categorie = categories;

            List<LigneCommande> LC = db.LigneCommandes.ToList();
            ViewBag.LigneCommande = LC;
            if (ModelState.IsValid)
            {


                var LigneCommandes = new List<LigneCommande>();

                var commande = new Commande
                {
                    DateCommande = model.DateCommande,
                    ClientId = model.ClientId,
                };
                db.Commandes.Add(commande);
                db.SaveChanges();

                var produit = new Produit
                {/*
                Libelle = "jabador",
                PU = 701,
                Image = "~/img/jabador.jpg",
                CategoryId = 3,
                */

                    Libelle = Libelle,
                    PU = PU,
                    Image = Image,
                    CategoryId = CategoryId
                };
                db.Produits.Add(produit);
                db.SaveChanges();

                var ligne = new LigneCommande
                {
                    Qte = model.Qte,
                    ProduitId = produit.Id,
                    CommandeId = commande.Id,
                };

                db.LigneCommandes.Add(ligne);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize]
        public IActionResult Index()
        {
            List<Commande> commandes = db.Commandes.Include(cmd => cmd.LigneCommandes).Include(cmd => cmd.Facture).Include(cmd => cmd.Client).ToList();
            return View(commandes);
        }
        public IActionResult ListProduits()
        {
           List<Produit> p= db.Produits.ToList();
            ViewBag.produit = p;

            return View(p);
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            List<Client> clients = db.Clients.ToList();
            ViewBag.Client = clients;
            Commande c = db.Commandes.Where(c => c.Id == id).FirstOrDefault();
            return View(c);
        }
        [HttpPost]
        public IActionResult Update(Commande c)
        {
            List<Client> clients = db.Clients.ToList();
            ViewBag.Client = clients;
            db.Commandes.Update(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            db.Commandes.Remove(db.Commandes.Where(c => c.Id == id).Include(c => c.Client).Include(c => c.LigneCommandes).Include(c => c.Facture).FirstOrDefault());
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            Commande c = db.Commandes.Include(c => c.LigneCommandes).Include(c => c.Client).Include(c => c.Facture).Where(c => c.Id == id).FirstOrDefault();
            return View(c);


        }
        // Méthode pour récupérer le libellé du produit par catégorie
        public IActionResult GetLibelleProduitParCategorie(int categorieId)
        {
            var libelleProduit = db.Produits
                                         .Where(p => p.CategoryId == categorieId)
                                         .Select(p => p.Libelle)
                                         .FirstOrDefault(); // Supposons qu'il y ait un seul produit par catégorie

            return Json(libelleProduit);
        }
  

        // Méthode pour récupérer le PU et l'image du produit par ID
        [HttpGet]
        public IActionResult GetProduitDetails(int produitId)
        {
            var produit = db.Produits.Find(produitId);
            if (produit != null)
            {
                return Json(new { pu = produit.PU, image = produit.Image });
            }
            else
            {
                return Json(null);
            }
        }


        [HttpGet]
        public IActionResult GetProduitsByCategory(int categoryId)
        {
            var produits = db.Produits.Where(p => p.CategoryId == categoryId).ToList();
            return Json(produits);
        }

    }
}
