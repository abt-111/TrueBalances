using TrueBalances.Models;


namespace TrueBalances.Repositories.Interfaces
{
    public interface IGroupService
    {
        List<Group> GetAllGroups();
        Task<Group?> GetGroupAsync(int groupId);
        Task CreateGroupAsync(Models.Group group, string userId);
        Task UpdateGroupAsync(Models.Group group);
        Task DeleteGroupAsync(int groupId);
        Task AddMemberAsync(int groupId, string userId);
        Task RemoveMemberAsync(int groupId, string userId);
        //Ajouter le code pour la partie Solde
    }
}
