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
            ViewBag.GroupId = groupId;
            return View(await _context.GetAllCategoriesAsync());
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View();
        }

        // Create (Post)
        [HttpPost]
        public async Task<IActionResult> Create(Category category, int groupId)
        {
            if (!ModelState.IsValid) return View(category);
            
                await _context.AddCategoryAsync(category);
                return RedirectToAction("Index", new { groupId = groupId });
        }

        //Edit(GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? categorieId, int groupId)
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
            ViewBag.GroupId = groupId;
            return View(category);
        }

        // Edit (POST)

        [HttpPost]

        public async Task<IActionResult> Edit(int id, Category category, int groupId)
        {
            if (id == category.Id) await _context.UpdateCategoryAsync(category);
            return RedirectToAction("Index", new { groupId = groupId });
            return RedirectToAction(actionName: "Index", controllerName: "Category");
            
            //return View(category);
        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int groupId)
        {
            var category = await _context.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            ViewBag.GroupId = groupId;
            return View(category);
        }

        //Delete (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int groupId)
        {
            var category = await _context.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            //Réinitialiser les identifiants de catégorie des dépenses associées
            //var expenses = _context.Expenses.Where(e => e.CategoryId == id).ToList();
            //foreach (var expense in expenses)
            //{
            //    expense.CategoryId = null;
            //}

            await _context.DeleteCategoryAsync(id);
            return RedirectToAction("Index", new { groupId = groupId });
            return RedirectToAction(actionName: "Index", controllerName: "Category");
        }

        //Methode pour Vérifier si une catégorie existe
        private async Task<bool> CategoryExists(int id)
        {
            return await _context.CategoryExistsAsync(id);
        }


        //Détails 
        public async Task<IActionResult> Details(int id, int groupId)
        {
            var category = await _context.GetCategoryWithExpensesByIdAsync(id, groupId);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.GroupId = groupId;
            return View(category);
        }

    }
}
