using TrueBalances.Models;

namespace TrueBalances.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<int> AddAsync(Category entity);
        Task<int> UpdateAsync(Category entity);
        Task<int> DeleteAsync(int id);
        Task<Category> GetCategoryWithExpensesByIdAsync(int id, int groupId);
    }
}
