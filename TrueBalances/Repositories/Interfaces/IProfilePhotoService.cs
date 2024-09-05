using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IProfilePhotoService
    {
        public string RegisterProfilePhotoFile(IFormFile photoFile);

        public string UpdateProfilePhotoFile(IFormFile photoFile, string registeredProfilePhoto);

        public bool HasProfilePhoto(CustomUser user);

        public string GetProfilePhotoFile(CustomUser user);
    }
}
