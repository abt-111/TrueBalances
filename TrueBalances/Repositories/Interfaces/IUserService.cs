using TrueBalances.Areas.Identity.Data;
using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IUserService
    {
        Task<List<CustomUser>> GetAllUsersAsync();
    }
}
