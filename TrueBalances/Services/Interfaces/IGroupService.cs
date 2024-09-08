using TrueBalances.Models;


namespace TrueBalances.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetAllGroups();
        Task<Group?> GetGroupAsync(int groupId);
        Task CreateGroupAsync(Group group, string userId);
        Task UpdateGroupAsync(Group group);
        Task DeleteGroupAsync(int groupId);
        Task<List<string>> AddMembersAsync(int groupId, List<string> userIds);
        Task RemoveMemberAsync(int groupId, string userId);

        Task<bool> IsMemberInGroupAsync(int groupId, string userId);
        Task UpdateGroupMembersAsync(int id, List<string> selectedUserIds);
    }
}
