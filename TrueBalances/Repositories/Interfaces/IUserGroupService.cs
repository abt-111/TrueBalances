using TrueBalances.Areas.Identity.Data;
using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IUserGroupService
    {
        Task<IEnumerable<Group>> GetGroupsByUserIdAsync(int userId);
        Task AddMembersToGroupAsync(int groupId, List<int> memberIds);
        Task UpdateMembersInGroupAsync(int groupId, List<int> memberIds);
        Task RemoveGroupAsync(int groupId);

        //Task<List<CustomUser>> GetAllUsersAsync();
    }
}
