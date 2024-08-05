using System.ComponentModel.DataAnnotations;
using TrueBalances.Areas.Identity.Data;

namespace TrueBalances.Models
{
    public class ProfilePhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        // Clé étrangère
        public string CustomUserId { get; set; }
        // Propriété de navigation
        public CustomUser CustomUser { get; set; }
    }
}
