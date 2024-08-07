using TrueBalances.Areas.Identity.Data;

namespace TrueBalances.Models
{
    public class GroupDetailsViewModel
    {
        public Group Group { get; set; }
        public List<CustomUser> AvailableUsers { get; set; }
        public List<string> SelectedUserIds { get; set; }
        //public IFormFile BannerImage { get; set; }
    }
}
