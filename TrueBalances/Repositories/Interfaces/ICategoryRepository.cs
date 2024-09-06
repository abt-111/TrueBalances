using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<int> AddAsync(Category entity);
        Task<int> UpdateAsync(Category entity);
        Task<int> DeleteAsync(int id);
        Task<Category> GetCategoryWithExpensesByIdAsync(int id, int groupId);
    }
}
