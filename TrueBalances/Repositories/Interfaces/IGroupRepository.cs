using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(int id);
        Task<Group> GetByIdWithExpensesAsync(int id);
        Task<Group> GetByIdWithExpensesCategoriesAsync(int id);
        Task<int> AddAsync(Group entity);
        Task<int> UpdateAsync(Group entity);
        Task<int> DeleteAsync(int id);
    }
}
