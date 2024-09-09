using Microsoft.AspNetCore.Mvc;
using TrueBalances.Models;
using Microsoft.AspNetCore.Authorization;
using TrueBalances.Services.Interfaces;
using TrueBalances.Models.ViewModels;


namespace TrueBalances.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Read
        public async Task<IActionResult> Index(int groupId)
        {
            var categories = await _categoryService.GetAllAsync();

            CategoryViewModel viewModel = new CategoryViewModel()
            {
                GroupId = groupId,
                Categories = categories
            };

            return View(viewModel);
        }

        // Create (GET)
        [HttpGet]
        public IActionResult Create(int groupId)
        {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                GroupId = groupId
            };

            return View(viewModel);
        }

        // Create (Post)
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            await _categoryService.AddAsync(viewModel.Category);
            return RedirectToAction("Index", new { groupId = viewModel.GroupId });
        }

        //Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int categorieId, int groupId)
        {
            if (categorieId <= 0 || groupId <= 0)
            {
                return NotFound();
            }

            var category = await _categoryService.GetByIdAsync(categorieId);

            if (category is null)
            {
                return NotFound();
            }

            CategoryViewModel viewModel = new CategoryViewModel()
            {
                GroupId = groupId,
                Category = category
            };

            return View(viewModel);
        }

        // Edit (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            await _categoryService.UpdateAsync(viewModel.Category);
            return RedirectToAction("Index", new { groupId = viewModel.GroupId });
        }



        //Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(int categorieId, int groupId)
        {
            if (categorieId <= 0 || groupId <= 0)
            {
                return NotFound();
            }

            var category = await _categoryService.GetByIdAsync(categorieId);

            if (category is null)
            {
                return NotFound();
            }

            CategoryViewModel viewModel = new CategoryViewModel()
            {
                GroupId = groupId,
                Category = category
            };

            return View(viewModel);
        }

        //Delete (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(CategoryViewModel viewModel)
        {
            var category = await _categoryService.GetByIdAsync(viewModel.Category.Id);

            if (category is null)
            {
                return NotFound();
            }

            await _categoryService.DeleteAsync(viewModel.Category.Id);
            return RedirectToAction("Index", new { groupId = viewModel.GroupId });
        }

        //Détails 
        public async Task<IActionResult> Details(int categorieId, int groupId)
        {
            if (categorieId <= 0 || groupId <= 0)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryWithExpensesByIdAsync(categorieId, groupId);

            if (category is null)
            {
                return NotFound();
            }

            CategoryViewModel viewModel = new CategoryViewModel()
            {
                GroupId = groupId,
                Category = category
            };

            return View(viewModel);
        }
    }
}
