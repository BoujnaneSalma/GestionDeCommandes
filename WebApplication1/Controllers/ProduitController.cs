using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.service;

namespace WebApplication1.Controllers
{
    public class ProduitController : Controller
    {

        MyContext db;
        IWebHostEnvironment _webHostEnvironment;

        public ProduitController(MyContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Add()
        {
            List<Category> categories = db.Categories.ToList();
            ViewBag.Category = categories;
            List<LigneCommande> LC = db.LigneCommandes.ToList();
            ViewBag.ligneCommande = LC;
            return View();

           
        }



        [HttpPost]
        public async Task<IActionResult> AddAsync(IFormFile MyImage, ProduitViewModel model)
        {
            List<Category> categories = db.Categories.ToList();
            ViewBag.Category = categories;
            List<LigneCommande> ligneCommande = db.LigneCommandes.ToList();
            ViewBag.ligneCommande = ligneCommande;
            if (ModelState.IsValid)
            {


                var imageUploader = new uplead();
                var uploadedFileName = await imageUploader.UploadImageAsync(MyImage, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img"));

                model.Image = uploadedFileName;
                var LigneCommandes = new List<LigneCommande>();

                var p = new Produit
                {
                    Id = model.Id,
                    Libelle = model.Libelle,
                    PU = model.PU,
                    Image = model.Image,
                    CategoryId = model.CategoryId,
                };

                var cmd = new Commande
                {
                    DateCommande = DateTime.Now,
                    ClientId = 3,
                };
                db.Produits.Add(p);
                db.Commandes.Add(cmd);

                db.SaveChanges();
                var LC = new LigneCommande
                {
                    Qte = model.Qte,
                    ProduitId = p.Id,
                    CommandeId = cmd.Id,
                };

                db.LigneCommandes.Add(LC);

                db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        
   
    



        /*
            if(MyImage != null && MyImage.Length >0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(MyImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                // Valider l'extension du fichier
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    ModelState.AddModelError("ImageFile", "Veuillez sélectionner un fichier avec une extension .jpg, .jpeg ou .png.");
                    return View(p);
                }
                // Valider la taille du fichier
                if (MyImage.Length > 1 * 1024 * 1024) // 1MB
                {
                    ModelState.AddModelError("ImageFile", "La taille du fichier ne doit pas dépasser 1MO.");
                    return View(p);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await MyImage.CopyToAsync(stream);
                }
                p.Image = fileName;
            }
        */
        [Authorize]
        public IActionResult Index()
        {
            List<Produit> produit = db.Produits.Include(p => p.LigneCommandes).Include(cmd => cmd.Category).ToList();
            return View(produit);
          


        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            List<Category> categories = db.Categories.ToList();
            ViewBag.Category = categories;
            Produit p = db.Produits.Where(c => c.Id == id).FirstOrDefault();
            return View(p);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Produit p, IFormFile MyImage)
        {
            if (MyImage != null)
            {
                var randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(MyImage.FileName);
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                string filePath = Path.Combine(uploadsFolder, randomFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await MyImage.CopyToAsync(fileStream);
                }
                p.Image = randomFileName;
            }
            List<Category> categories = db.Categories.ToList();
            ViewBag.Category = categories;

            db.Produits.Update(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            db.Produits.Remove(db.Produits.Where(c => c.Id == id).Include(c => c.Category).Include(c => c.LigneCommandes).FirstOrDefault());
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            Produit p = db.Produits.Include(c => c.LigneCommandes).Include(c => c.Category).Where(c => c.Id == id).FirstOrDefault();
            return View(p);


        }
        // Action pour afficher les commandes entre deux dates précises
        public IActionResult AfficherCommandesEntreDates()
        {
            List<Client> clients = db.Clients.ToList();
            ViewBag.Client = clients;
            return View();
        }

        [HttpPost]
        public IActionResult AfficherCommandesEntreDates(DateTime dateDebut, DateTime dateFin)
        {
            List<Client> clients = db.Clients.ToList();
            ViewBag.Client = clients;
            if (ModelState.IsValid)
            {

                var commandes = db.Commandes
                    .Where(c => c.DateCommande >= dateDebut && c.DateCommande <= dateFin)
                    .ToList();
                return View("ResultatCommandesEntreDates", commandes);
            }
            return View();
        }
        public IActionResult Imprimer()
        {
            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Rapport", "Report1.rdlc"); //récupère le chemin racine du répertoire de contenu
            LocalReport report = new LocalReport(path); //pour générer des rapports

            List<Produit> p = db.Produits.AsNoTracking().ToList(); //améliorer les performances en évitant le suivi inutile des entités 

            // Ajout du chemin de l'image pour chaque voiture
            foreach (var produit in p)
            {
                string imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot/img", $"{produit.Image}");

                // Vérifiez si le fichier image existe
                using (Image image = Image.FromFile(imagePath))
                {
                    // Convertit l'image en tableau d'octets
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        image.Save(memoryStream, ImageFormat.Jpeg);
                        byte[] imageData = memoryStream.ToArray();

                        // Convertir le tableau d'octets en chaîne Base64
                        string base64Image = Convert.ToBase64String(imageData);
                        // Stocke les données de l'image dans l'objet Voiture
                        produit.Image = base64Image;
                    }
                }

            }


            report.AddDataSource("DataSet1", p);

            ReportResult res = report.Execute(RenderType.Pdf);
            return File(res.MainStream, "application/pdf", "ListeProduits.pdf");

          
        }
    }
       
    
}
