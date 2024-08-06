using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IProfilePhotoService
    {
        public ProfilePhoto RegisterProfilePhotoFile(IFormFile photoFile);
    }
}
