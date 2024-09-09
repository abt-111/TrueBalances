using Microsoft.AspNetCore.Mvc;
using TrueBalances.Models;
using Microsoft.AspNetCore.Authorization;
using TrueBalances.Services.Interfaces;


namespace TrueBalances.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService context)

        {
            _categoryService = context;
        }

        // Read
        public async Task<IActionResult> Index(int groupId)
        {
            ViewBag.GroupId = groupId;
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
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
            
                await _categoryService.AddAsync(category);
                return RedirectToAction("Index", new { groupId = groupId });
        }

        //Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int? categorieId, int groupId)
        {
            if (categorieId is null)
            {
                return View();
            }
            var category = await _categoryService.GetByIdAsync(categorieId.Value);
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
            if (id == category.Id) await _categoryService.UpdateAsync(category);
            return RedirectToAction("Index", new { groupId = groupId });
            
            //return View(category);
        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int groupId)
        {
            var category = await _categoryService.GetByIdAsync(id);
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
            var category = await _categoryService.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            await _categoryService.DeleteAsync(id);
            return RedirectToAction("Index", new { groupId = groupId });
        }

        //Détails 
        public async Task<IActionResult> Details(int id, int groupId)
        {
            var category = await _categoryService.GetCategoryWithExpensesByIdAsync(id, groupId);
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.GroupId = groupId;
            return View(category);
        }

    }
}
