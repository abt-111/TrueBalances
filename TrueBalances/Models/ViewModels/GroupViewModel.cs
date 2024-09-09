namespace TrueBalances.Models.ViewModels
{
    public class GroupViewModel
    {
        public CustomUser CurrentUser { get; set; } // Index
        public IEnumerable<Group> Groups { get; set; }  // Index
        public Group Group { get; set; }
        public List<string> SelectedUserIds { get; set; }
        public List<CustomUser>? AvailableUsers { get; set; }
        public List<Expense>? Expenses { get; set; }
        public string? CategoryName { get; set; }

        public int? CategoryId { get; set; }

    }
}
