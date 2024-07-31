using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TrueBalances.Models;

namespace TrueBalances.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryDbContext _context;

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
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index"); //a verifier 
        }

        // Edit (GET)
        //[HttpGet]
        //public async Task<IActionResult> Edit(int? categorieId)
        //{
        //    if (categorieId == null)
        //    {
        //        return View();
        //    }
        //    var category = await _context.Categories.FindAsync(categorieId);
        //    if (category == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}

        // Edit (POST)

        //[HttpPost]
        //public async Task<IActionResult> Edit(int id, Category category)
        //{
        //    if (id == category.Id)

        //    { 
        //        _context.Update(category);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction("Index");

        //}
    }
}
