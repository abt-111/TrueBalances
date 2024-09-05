using TrueBalances.Data;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Models;
using System.Linq;

namespace TrueBalances.Repositories.Services
{
    public class UserService : IUserService
    {
            private readonly TrueBalancesDbContext _context;

            public UserService(TrueBalancesDbContext context)
            {
                _context = context;
            }

        public async Task<List<CustomUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); 
        }

        public async Task<List<CustomUser>> GetAllUsersAsync(int? groupId)
        {
            return await _context.Users.Where(u => u.UserGroups.Any(ug => ug.GroupId == groupId)).ToListAsync();
        }
    }
}
