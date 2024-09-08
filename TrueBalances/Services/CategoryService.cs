using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Services.Interfaces;

namespace TrueBalances.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryrepository;

        public CategoryService(ICategoryRepository categoryrepository)
        {
            _categoryrepository = categoryrepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryrepository.GetAllAsync();
        }


        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _categoryrepository.GetByIdAsync(id);
        }

        public async Task<Category?> GetCategoryWithExpensesByIdAsync(int id, int groupId)
        {
            return await _categoryrepository.GetCategoryWithExpensesByIdAsync(id, groupId);
        }
        public async Task<int> AddCategoryAsync(Category category)
        {

            return await _categoryrepository.AddAsync(category);
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            return await _categoryrepository.UpdateAsync(category);
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            return await _categoryrepository.DeleteAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            var categoryRepository = _categoryrepository as ICategoryService;
            return categoryRepository != null && await categoryRepository.CategoryExistsAsync(id);
        }
    }

}
