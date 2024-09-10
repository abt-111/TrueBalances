using TrueBalances.Models;


namespace TrueBalances.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllAsync();
        Task<Group?> GetByIdWithExpensesAsync(int groupId);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int groupId);
        Task<List<string>> AddMembersAsync(int groupId, List<string> userIds);
        Task RemoveMemberAsync(int groupId, string userId);
        Task<bool> IsMemberInGroupAsync(int groupId, string userId);
        Task UpdateGroupMembersAsync(int id, List<string> selectedUserIds);
    }
}
