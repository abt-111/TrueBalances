using TrueBalances.Models;

namespace TrueBalances.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<CustomUser>> GetAllUsersAsync();
        Task<List<CustomUser>> GetAllUsersAsync(int? groupId);
    }
}
