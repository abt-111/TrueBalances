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

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryrepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryrepository.GetAllAsync();
        }

        public async Task<int> AddAsync(Category category)
        {

            return await _categoryrepository.AddAsync(category);
        }

        public async Task<int> UpdateAsync(Category category)
        {
            return await _categoryrepository.UpdateAsync(category);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _categoryrepository.DeleteAsync(id);
        }

        public async Task<Category> GetCategoryWithExpensesByIdAsync(int id, int groupId)
        {
            return await _categoryrepository.GetCategoryWithExpensesByIdAsync(id, groupId);
        }
    }
}
