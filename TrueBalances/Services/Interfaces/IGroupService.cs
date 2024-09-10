using TrueBalances.Models;

namespace TrueBalances.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(int id);
        Task<Group> GetByIdWithExpensesAsync(int id);
        Task<Group> GetByIdWithExpensesCategoriesAsync(int id);
        Task AddAsync(Group entity);
        Task UpdateAsync(Group entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId);
        bool UserIsInGroup(string userId, int groupId);
    }
}
