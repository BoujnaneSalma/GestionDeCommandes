using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        MyContext db;
        public CategoryController(MyContext db)
        {
            this.db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            List<Category> c = db.Categories.Include(C => C.Produits).ToList();
            return View(c);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Category c)
        {
            if(ModelState.IsValid)
            {
                db.Categories.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Category c = db.Categories.Where(c => c.Id == id).FirstOrDefault();

            return View(db);

        }
        [HttpPost]
        public IActionResult Update(Category c)
        {
           
            if (ModelState.IsValid)
            {
             
            db.Categories.Update(c);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View();
           
        }
        public IActionResult Delete(int id)
        {
            db.Categories.Remove(db.Categories.Where(c => c.Id == id).FirstOrDefault());
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail()
        {
            Category c = db.Categories.Include(c => c.Produits).FirstOrDefault();


            return View(c);
        }
    }
}
