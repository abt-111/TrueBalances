namespace TrueBalances.Models.ViewModels
{
    public class GroupViewModel
    {
        public CustomUser? CurrentUser { get; set; } // Index
        public IEnumerable<Group>? Groups { get; set; }  // Index
        public List<CustomUser>? Users { get; set; } // Create
        public Group Group { get; set; } // Create - Edit - Details
        public List<string> SelectedUserIds { get; set; } // Create - Edit - Details
        public List<string>? CategoriesChoosed { get; set; } // Details
    }
}
