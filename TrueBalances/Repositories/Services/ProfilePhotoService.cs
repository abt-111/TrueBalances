using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
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

        public string RegisterProfilePhotoFile(IFormFile photoFile)
        {
            if (photoFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                string titre = Guid.NewGuid().ToString() + "_" + photoFile.FileName;

                string filePath = Path.Combine(uploadsFolder, titre);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photoFile.CopyTo(fileStream);
                }

                return titre;
            }

            // Gérer le cas au niveau de Register.cshtml.cs
            return string.Empty;
        }

        public string UpdateProfilePhotoFile(IFormFile photoFile, string registeredProfilePhoto)
        {
            if (photoFile != null)
            {
                // Suppression de l'image stockée si il y en a une
                if (registeredProfilePhoto != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    string filePath = Path.Combine(uploadsFolder, registeredProfilePhoto);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                return RegisterProfilePhotoFile(photoFile);
            }

            // Gérer le cas au niveau de Register.cshtml.cs
            return string.Empty;
        }

        public bool HasProfilePhoto(CustomUser user)
        {
            return user.ProfilePhotoUrl != null;
        }

        public string GetProfilePhotoFile(CustomUser user)
        {
            string profilePhotoUrl = user.ProfilePhotoUrl;

            string filePath = Path.Combine("\\images", profilePhotoUrl);

            return filePath;
        }
    }
}
