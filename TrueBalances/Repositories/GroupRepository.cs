using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly TrueBalancesDbContext _context;

        public GroupRepository(TrueBalancesDbContext context)
        {
            _context = context;
        }

        // Méthode pour obtenir tous les groupes
        public async Task<IEnumerable<Group>> GetAllAsync()
        {
            return await _context.Groups.Include(g => g.Members).Include(g => g.Expenses).ToListAsync();
        }

        // Méthode pour obtenir un groupe par son identifiant
        public async Task<Group> GetByIdAsync(int id)
        {
            return await _context.Groups.Include(g => g.Members).FirstOrDefaultAsync(g => g.Id == id);
        }

        // Méthode pour obtenir un groupe par son identifiant en incluant les dépenses associées
        public async Task<Group> GetByIdWithExpensesAsync(int id)
        {
            return await _context.Groups.Include(g => g.Members).Include(g => g.Expenses).FirstOrDefaultAsync(g => g.Id == id);
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
    }
}


