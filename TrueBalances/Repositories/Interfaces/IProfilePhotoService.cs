using TrueBalances.Areas.Identity.Data;
using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IProfilePhotoService
    {
        public string RegisterProfilePhotoFile(IFormFile photoFile);

        public void UpdateProfilePhotoFile(IFormFile photoFile, ProfilePhoto registeredProfilePhoto);

        public bool HasProfilePhoto(CustomUser user);

        public string GetProfilePhotoFile(CustomUser user);

        public Task<ProfilePhoto> GetProfilePhoto(string customUserId);
    }
}
