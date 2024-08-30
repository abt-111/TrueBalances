using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Repositories;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Data;
using Microsoft.AspNetCore.Authorization;
using TrueBalances.Repositories.Services;


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
        public async Task<IActionResult> Index(int groupId)
        {
            var categories = await _context.GetAllCategoriesAsync();
            ViewBag.GroupId = groupId;
            return View(categories);
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create(int groupId)
        {
            var model = new Category
            {
                GroupId = groupId
            };

            return View(model);
        }

        // Create (Post)
        [HttpPost]
        public async Task<IActionResult> Create(Category category, int groupId)
        {
            if (!ModelState.IsValid) return View(category);

            // Associer la catégorie au groupe
            category.GroupId = groupId;
            try
            {
                await _context.AddCategoryAsync(category);
                return RedirectToAction(actionName: "Index");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Une erreur est survenue lors de l'enregistrement de la catégorie.");
                return View(category);
            }
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
            var category = await _context.GetCategoryWithExpensesByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

    }
}
