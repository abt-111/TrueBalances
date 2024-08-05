using System.Text.RegularExpressions;
using TrueBalances.Models;
using Group = System.Text.RegularExpressions.Group;


namespace TrueBalances.Repositories.Interfaces
{
    public interface IGroupService
    {
        Task<Group?> GetGroupAsync(int groupId);
        Task CreateGroupAsync(Models.Group group, string userId);
        Task UpdateGroupAsync(Models.Group group);
        Task DeleteGroupAsync(int groupId);
        Task AddMemberAsync(int groupId, string userId);
        Task RemoveMemberAsync(int groupId, string userId);
        //Ajouter le code pour la partie Solde
    }
}




//Task<Group> GetGroupByIdAsync(int id);
//Task<IEnumerable<Group>> GetAllGroupsAsync();
//Task<int> AddGroupAsync(Group group);
//Task<int> UpdateGroupAsync(Group group);
//Task<int> DeleteGroupAsync(int id);
//Task<int> AddMemberToGroupAsync(int groupId, UserGroup member);
//Task<int> RemoveMemberFromGroupAsync(int groupId, int memberId);
