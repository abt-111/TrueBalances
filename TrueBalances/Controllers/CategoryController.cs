using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Repositories;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Data;


namespace TrueBalances.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)

        {
            _categoryRepository = categoryRepository;
        }

        // Read
        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepository.GetAllCategoriesAsync());
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
            if (!ModelState.IsValid)
            {
                await _categoryRepository.AddCategoryAsync(category);
                //await _categoryRepository.SaveChangesAsync(); verifier le sauvegard
                return RedirectToAction(actionName: "Index", controllerName: "Category");
            }
            return View(category);

        }

        //Edit(GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? categorieId)
        {
            if (categorieId is null)
            {
                return View();
            }
            var category = await _categoryRepository.GetCategoryByIdAsync(categorieId.Value);
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
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await _categoryRepository.UpdateCategoryAsync(category);
                return RedirectToAction(actionName: "Index", controllerName: "Category");
            }
            return View(category);
        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
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
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
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

            await _categoryRepository.DeleteCategoryAsync(id);
            return RedirectToAction(actionName: "Index", controllerName: "Category");
        }

        //Methode pour Vérifier si une catégorie existe
        private async Task<bool> CategoryExists(int id)
        {
            return await _categoryRepository.CategoryExistsAsync(id);
        }


        //Détails 
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            return View(category);
        }

    }
}
