using Microsoft.AspNetCore.Identity;

namespace TrueBalances.Models;

public class CustomUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }
    [PersonalData]
    public string? ProfilePhotoUrl { get; set; }
    public ICollection<Expense> CreatedExpenses { get; set; } = new List<Expense>(); // Pour la relation one-to-many
    public ICollection<Expense> ParticipatingExpenses { get; set; } = new List<Expense>(); // Pour la relation many-to-many
    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
}
