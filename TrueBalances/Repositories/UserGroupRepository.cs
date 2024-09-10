using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Repositories
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly TrueBalancesDbContext _context;

        public UserGroupRepository(TrueBalancesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> GetGroupsByUserIdAsync(string userId)
        {
            return await _context.Groups.Include(g => g.Members).Where(g => g.Members.Any(m => m.CustomUserId == userId)).ToListAsync();
        }

        public bool UserIsInGroup(string userId, int groupId)
        {
            return _context.UserGroups
                .Where(ug => ug.CustomUserId == userId)
                .Select(ug => ug.Group).Any(g => g.Id == groupId);
        }

        public async Task AddMembersToGroupAsync(int groupId, List<string> memberIds)
        {
            foreach (var memberId in memberIds)
            {
                _context.UserGroups.Add(new UserGroup { GroupId = groupId, CustomUserId = memberId });
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMembersInGroupAsync(int groupId, List<string> memberIds)
        {
            var existingMembers = _context.UserGroups.Where(ug => ug.GroupId == groupId);
            _context.UserGroups.RemoveRange(existingMembers);
            await AddMembersToGroupAsync(groupId, memberIds);
        }

        public async Task RemoveGroupAsync(int groupId)
        {
            var userGroups = _context.UserGroups.Where(ug => ug.GroupId == groupId);
            _context.UserGroups.RemoveRange(userGroups);
            await _context.SaveChangesAsync();
        }
    }
}

