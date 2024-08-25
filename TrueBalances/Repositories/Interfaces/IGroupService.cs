using TrueBalances.Models;


namespace TrueBalances.Repositories.Interfaces
{
    public interface IGroupService
    {
        Task <IEnumerable<Group>> GetAllGroups();
        Task<Group?> GetGroupAsync(int groupId);
        Task CreateGroupAsync(Models.Group group, string userId);
        Task UpdateGroupAsync(Models.Group group);
        Task DeleteGroupAsync(int groupId);
        Task<List<string>> AddMembersAsync(int groupId, List<string> userIds);
        Task RemoveMemberAsync(int groupId, string userId);

        Task<bool> IsMemberInGroupAsync(int groupId, string userId);
        //Task AddMembersAsync(int groupId, List<string> selectedUserIds);
        //Ajouter le code pour la partie Solde
    }
}
