using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Data;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Repositories.Services
{
    public class ProfilePhotoService : IProfilePhotoService
    {
        private readonly UserContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilePhotoService(UserContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public ProfilePhoto RegisterProfilePhotoFile(IFormFile photoFile)
        {
            var profilePhoto = new ProfilePhoto();
            if (photoFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string titre = Guid.NewGuid().ToString() + "_" + photoFile.FileName;
                string filePath = Path.Combine(uploadsFolder, titre);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photoFile.CopyTo(fileStream);
                }

                profilePhoto.Url = titre;
            }
            return profilePhoto;
        }

        public void UpdateProfilePhotoFile(IFormFile photoFile, ProfilePhoto registeredProfilePhoto)
        {
            var profilePhoto = new ProfilePhoto();
            if (photoFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string titre = registeredProfilePhoto.Url;
                string filePath = Path.Combine(uploadsFolder, titre);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photoFile.CopyTo(fileStream);
                }
            }
        }

        public string GetProfilePhotoFile(string customUserId)
        {
            ProfilePhoto profilePhoto = _context.ProfilePhotos.FirstOrDefault(x => x.CustomUserId == customUserId);
            string filePath = Path.Combine("images", profilePhoto.Url);

            return filePath;
        }

        public ProfilePhoto GetProfilePhoto(string customUserId)
        {
            return _context.ProfilePhotos.FirstOrDefault(x => x.CustomUserId == customUserId);
        }
    }
}
