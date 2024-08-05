using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrueBalances.Models;

namespace TrueBalances.Areas.Identity.Data;

// Add profile data for application users by adding properties to the CustomUser class
public class CustomUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ProfilePhoto? ProfilPhoto { get; set; }
}