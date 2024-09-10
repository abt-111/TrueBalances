using Microsoft.EntityFrameworkCore;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Services.Interfaces;


namespace TrueBalances.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public GroupService(IGroupRepository groupRepository, IUserGroupRepository userGroupRepository)
        {
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            return groups.ToList();
        }

        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _groupRepository.GetByIdWithExpensesAsync(id);
        }

        //Methode pour Trouver un group via son Id
        public async Task<Group?> GetByIdWithExpensesAsync(int id)
        {
            return await _groupRepository.GetByIdWithExpensesAsync(id);
        }

        //Methode pour creer un group
        public async Task AddAsync(Group group)
        {
            await _groupRepository.AddAsync(group);
        }


        //Methode pour modifier le group
        public async Task UpdateAsync(Group group)
        {
            await _groupRepository.UpdateAsync(group);
        }

        ////Methode pour supprimer le group
        public async Task DeleteAsync(int groupId)
        {
            await _groupRepository.DeleteAsync(groupId);
        }

        public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId)
        {
            return await _userGroupRepository.GetGroupsByUserIdAsync(userId);
        }

        public bool UserIsInGroup(string userId, int groupId)
        {
            return _userGroupRepository.UserIsInGroup(userId, groupId);
        }
    }
}