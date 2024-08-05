using TrueBalances.Data;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Repositories.Interfaces;
using TrueBalances.Areas.Identity.Data;

namespace TrueBalances.Repositories.Services
{
    public class UserService : IUserService
    {
            private readonly UserContext _context;

            public UserService(UserContext context)
            {
                _context = context;
            }

        public async Task<List<CustomUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); 
        }

    }
}
