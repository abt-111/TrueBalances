using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<int> AddCategoryAsync(Category category);
        Task<int> UpdateCategoryAsync(Category category);
        Task<int> DeleteCategoryAsync(int id);
        Task<bool> CategoryExistsAsync(int id);

        Task<Category?> GetCategoryWithExpensesByIdAsync(int id);

        Task<IEnumerable<Category>> GetAllByGroupIdAsync(int groupId);

    }
}
