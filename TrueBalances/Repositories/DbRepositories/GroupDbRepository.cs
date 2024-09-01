using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;
using Group = TrueBalances.Models.Group;

namespace TrueBalances.Repositories.DbRepositories
{
    public class GroupDbRepository : IGenericRepository<Group>
    {
        private readonly UserContext _context;

        public GroupDbRepository(UserContext context)
        {
            _context = context;
        }

        // Méthode pour obtenir tous les groupes
        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Groups
                .Include(g => g.Members)
                .Include(g => g.Expenses)
                .ToListAsync();
        }

        // Méthode pour obtenir un groupe par son ID
        public async Task<Group?> GetByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .Include(g => g.Expenses)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<int> AddAsync(Group entity)
        {
            _context.Groups.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Group entity)
        {
            _context.Groups.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                return await _context.SaveChangesAsync();
            }
            return -1;
        }

        public Task<Category?> GetCategoryWithExpensesByIdAsync(int id, int groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<Group> GetGroupByIdAsync(int groupId)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .FirstOrDefaultAsync(g => g.Id == groupId);
        }

        public async Task AddUserToGroupAsync(int groupId, string userId)
        {
            var userGroup = new UserGroup
            {
                GroupId = groupId,
                CustomUserId = userId
            };
            _context.UsersGroup.Add(userGroup);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromGroupAsync(int groupId, string userId)
        {
            var userGroup = await _context.UsersGroup
                .FirstOrDefaultAsync(ug => ug.GroupId == groupId && ug.CustomUserId == userId);

            if (userGroup != null)
            {
                _context.UsersGroup.Remove(userGroup);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateGroupAsync(Group group)
        {
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }
    }
}


