using TrueBalances.Models;

namespace TrueBalances.Repositories.Interfaces
{
    public interface IProfilePhotoService
    {
        public ProfilePhoto RegisterProfilePhotoFile(IFormFile photoFile);

        public void UpdateProfilePhotoFile(IFormFile photoFile, ProfilePhoto registeredProfilePhoto);

        public string GetProfilePhotoFile(string customUserId);

        public Task<ProfilePhoto> GetProfilePhoto(string customUserId);

        public Task<bool> HasProfilePhoto(string customUserId);
    }
}
