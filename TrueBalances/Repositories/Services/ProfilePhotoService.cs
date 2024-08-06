using Microsoft.AspNetCore.Hosting;
using TrueBalances.Models;
using TrueBalances.Repositories.Interfaces;

namespace TrueBalances.Repositories.Services
{
    public class ProfilePhotoService : IProfilePhotoService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilePhotoService(IWebHostEnvironment webHostEnvironment)
        {
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
    }
}
