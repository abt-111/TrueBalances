using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
        Task<Category?> GetCategoryWithExpensesByIdAsync(int id, int groupId);
    }
}
