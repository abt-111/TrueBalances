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
        //public async Task<Group?> GetByIdAsync(int id, bool hasAllInfo = false)
        //{
        //    return await _context.Groups
        //        .Include(g => g.Members)
        //        .Include(g => g.Expenses)
        //        .FirstOrDefaultAsync(g => g.Id == id);
        //}

        //public async Task<IEnumerable<Group>> GetAllAsync(bool hasAllInfo = false)
        //{
        //    return await _context.Groups
        //        .Include(g => g.Members)
        //        .Include(g => g.Expenses)
        //        .ToListAsync();
        //}

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

        //Methode pour creer un group
        //public async Task CreateGroupAsync(Group group, string userId)
        //{
        //    group.Members = new List<UserGroup>
        //    {
        //        new UserGroup { CustomUserId = userId}
        //    };

        //    _context.Groups.Add(group);
        //    await _context.SaveChangesAsync();

        //}

        ////Methode pour Trouver un group via son Id
        //public async Task<Group> GetGroupAsync(int groupId)
        //{
        //    return await _context.Groups
        //        .Include(g => g.Members)
        //        .Include(g => g.Expenses)
        //        .FirstOrDefaultAsync(g => g.Id == groupId);
        //}

        ////Methode pour modifier le group
        //public async Task UpdateGroupAsync(Group group)
        //{
        //    var existingGroup = await _context.Groups.FindAsync(group.Id);
        //    if (existingGroup == null)
        //    {
        //        throw new KeyNotFoundException($"Group with ID {group.Id} not found.");
        //    }

        //    // Mettez à jour les propriétés nécessaires
        //    existingGroup.Name = group.Name;
        //    existingGroup.Members = group.Members;
        //    existingGroup.Expenses = group.Expenses;

        //    _context.Groups.Update(existingGroup);
        //    await _context.SaveChangesAsync();

        //    //_context.Update(group);
        //    //await _context.SaveChangesAsync();

        //}

        //////Methode pour supprimer le group
        //public async Task DeleteGroupAsync(int id)
        //{
        //    var group = await _context.Groups.FindAsync(id);
        //    if (group != null)
        //    {
        //        _context.Groups.Remove(group);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //////Methode pour ajouter un user dans le group
        //public async Task AddMemberAsync(int groupId, string userId)
        //{
        //    var userGroup = new UserGroup
        //    {
        //        GroupId = groupId,
        //        CustomUserId = userId
        //    };

        //    _context.UsersGroup.Add(userGroup);
        //    await _context.SaveChangesAsync();
        //}

        //////Methode pour supprimer un user dans le Group
        //public async Task RemoveMemberAsync(int groupId, string userId)
        //{
        //    var groupMember = await _context.UsersGroup
        //        .FirstOrDefaultAsync(m => m.GroupId == groupId && m.CustomUserId == userId);
        //    if (groupMember != null)
        //    {
        //        _context.UsersGroup.Remove(groupMember);
        //        await _context.SaveChangesAsync();
        //    }
        //}

    }

}

