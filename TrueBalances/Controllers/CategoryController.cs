using Microsoft.AspNetCore.Mvc;
using TrueBalances.Models;
using Microsoft.AspNetCore.Authorization;
using TrueBalances.Services.Interfaces;


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
            return View(await _context.GetAllAsync());
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
            
                await _context.AddAsync(category);
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
            var category = await _context.GetByIdAsync(categorieId.Value);
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
            if (id == category.Id) await _context.UpdateAsync(category);
            return RedirectToAction("Index", new { groupId = groupId });
            
            //return View(category);
        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int groupId)
        {
            var category = await _context.GetByIdAsync(id);
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
            var category = await _context.GetByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }

            await _context.DeleteAsync(id);
            return RedirectToAction("Index", new { groupId = groupId });
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
