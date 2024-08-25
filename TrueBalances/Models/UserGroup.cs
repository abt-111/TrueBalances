using TrueBalances.Areas.Identity.Data;

namespace TrueBalances.Models
{
    public class UserGroup
    {
        public int Id { get; set; }
        public string CustomUserId { get; set; }
        public CustomUser CustomUser { get; set; }
        
        //public ApplicationUser User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        
    }
}
