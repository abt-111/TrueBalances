using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Repositories;
using TrueBalances.Models;

namespace TrueBalances.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryDbContext _context; //ajouter l'interface

        public CategoryController(CategoryDbContext context)
        {
            _context = context;
        }

        // Read
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Create (Post)
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(actionName: "Index", controllerName: "Category");
            }
            return View(category);

        }

        //Edit(GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? categorieId)
        {
            if (categorieId == null)
            {
                return View();
            }
            var category = await _context.Categories.FindAsync(categorieId);
            if (category is null)
            {
                return RedirectToAction(actionName: "Index", controllerName: "Category");
            }
            return View(category);
        }

        // Edit (POST)

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == category.Id))
                {
                    return NotFound();
                }
                throw;
            }
            //if (id == category.Id)
            //    _context.Update(category);
            //await _context.SaveChangesAsync();
            return RedirectToAction(actionName: "Index", controllerName: "Category");

        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound(); //changement de return
            }

            return View(category);
        }

        //Delete (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            //var expenses = _context.Expenses.Where(e => e.CategoryId == id).ToList(); //Associer avec le context de depenses
            //foreach (var expense in expenses)
            //{
            //    expense.CategoryId = null;
            //}

            
           return RedirectToAction(actionName: "Index", controllerName: "Category");
        }

        //Methode pour Vérifier si une catégorie existe
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }


        //Détails 
        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return View(category);
        }

    }
}
