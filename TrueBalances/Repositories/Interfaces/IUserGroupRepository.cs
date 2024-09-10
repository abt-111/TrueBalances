using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IUserGroupRepository
    {
        Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId);
        public bool UserIsInGroup(string userId, int groupId);
    }
}
