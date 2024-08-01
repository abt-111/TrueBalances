using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface ICategory
    {
        Task<Category> GetStudentByIdAsync(int id);
    }
}
