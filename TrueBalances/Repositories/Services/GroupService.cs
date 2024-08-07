using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.DbRepositories;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Repositories.Services;
using Group = TrueBalances.Models.Group;


namespace TrueBalances.Repositories.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGenericRepository<Group> _groupRepository;
        public GroupService(IGenericRepository<Group> groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IEnumerable<Group>> GetAllGroups()
        {

            var groups = await _groupRepository.GetAllAsync();
            return groups.ToList();
        }

        //Methode pour Trouver un group via son Id
        public async Task<Group?> GetGroupAsync(int groupId)
        {
            return await _groupRepository.GetByIdAsync(groupId);
        }

        //Methode pour creer un group
        public async Task CreateGroupAsync(Group group, string userId)
        {
            group.Members = new List<UserGroup>
                    {
                        new UserGroup { CustomUserId = userId}
                    };

            await _groupRepository.AddAsync(group);

        }


        //Methode pour modifier le group
        public async Task UpdateGroupAsync(Group group)
        {
            await _groupRepository.UpdateAsync(group);

        }

        ////Methode pour supprimer le group
        public async Task DeleteGroupAsync(int groupId)
        {
            await _groupRepository.DeleteAsync(groupId);
        }

        //Methode pour vérifier si l'utilisateur est ajouté dans le group
        public async Task<bool> IsMemberInGroupAsync(int groupId, string userId)
        {
            var group = await GetGroupAsync(groupId);
            return group?.Members.Any(m => m.CustomUserId == userId) ?? false;
        }

        //Methode pour ajouter un user dans le group
        public async Task<List<string>> AddMembersAsync(int groupId, List<string> userIds)
        {
            var errors = new List<string>();
            var group = await GetGroupAsync(groupId);
            if (group == null)
            {
                errors.Add($"Group with ID {groupId} not found.");
                return errors;
            }

            var existingUserIds = group.Members.Select(m => m.CustomUserId).ToList();
            foreach (var userId in userIds)
            {
                if (existingUserIds.Contains(userId))
                {
                    errors.Add($"User with ID {userId} is already a member of the group.");
                }
                else
                {
                    group.Members.Add(new UserGroup
                    {
                        GroupId = groupId,
                        CustomUserId = userId
                    });
                }
            }

            await _groupRepository.UpdateAsync(group);
            return errors;

        }

            ////Methode pour supprimer un user dans le Group
            public async Task RemoveMemberAsync(int groupId, string userId)
            {
            var group = await GetGroupAsync(groupId);
            if (group != null)
            {
                var member = group.Members.FirstOrDefault(m => m.CustomUserId == userId);
                if (member != null)
                {
                    group.Members.Remove(member);
                    await _groupRepository.UpdateAsync(group);
                }
            }
        }

        
    }
    }