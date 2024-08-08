using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Repositories;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Data;
using Microsoft.AspNetCore.Authorization;


namespace TrueBalances.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _context;

        public CategoryController(ICategoryService context)

        {
            _context = context;
        }

        // Read
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetAllCategoriesAsync());
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
            if (!ModelState.IsValid) return View(category);
            
                await _context.AddCategoryAsync(category);
                return RedirectToAction(actionName: "Index", controllerName: "Category");
            
            

        }

        //Edit(GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? categorieId)
        {
            if (categorieId is null)
            {
                return View();
            }
            var category = await _context.GetCategoryByIdAsync(categorieId.Value);
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
            if (id == category.Id) await _context.UpdateCategoryAsync(category);
            return RedirectToAction(actionName: "Index", controllerName: "Category");
            
            //return View(category);
        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Delete (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            // Réinitialiser les identifiants de catégorie des dépenses associées
            //var expenses = _context.Expenses.Where(e => e.CategoryId == id).ToList(); 
            //foreach (var expense in expenses)
            //{
            //    expense.CategoryId = null;
            //}

            await _context.DeleteCategoryAsync(id);
            return RedirectToAction(actionName: "Index", controllerName: "Category");
        }

        //Methode pour Vérifier si une catégorie existe
        private async Task<bool> CategoryExists(int id)
        {
            return await _context.CategoryExistsAsync(id);
        }


        //Détails 
        public async Task<IActionResult> Details(int id)
        {
            //var category = await _context.GetCategoryByIdAsync(id).Include(c => c.Expenses).FirstOrDefaultAsync(m => m.Id == id);

            //if (category == null)
            //{
            //    return NotFound();
            //}

            //return View(category);
            var category = await _context.GetCategoryByIdAsync(id);
            return View(category);
        }

    }
}
