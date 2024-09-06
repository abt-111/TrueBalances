using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group> GetByIdAsync(int id);
        Task<IEnumerable<Group>> GetAllAsync();
        Task<int> AddAsync(Group entity);
        Task<int> UpdateAsync(Group entity);
        Task<int> DeleteAsync(int id);
    }
}
