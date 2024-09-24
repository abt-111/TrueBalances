using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrueBalances.Models.ViewModels
{
    public class ExpenseViewModel
    {
                                                                // Liste des vues utilisant la propriété
        public int GroupId { get; set; }                        // Index - Create - Edit
        public string? CurrentUserId { get; set; }              // Index
        public Expense Expense { get; set; }                    // Create - Edit
        public SelectList? Categories { get; set; }             // Create - Edit
        public SelectList? Authors { get; set; }                // Create - Edit
        public List<CustomUser>? Users { get; set; }            // Index - Create - Edit
        public List<Expense>? Expenses { get; set; }            // Index
        public List<string> SelectedUserIds { get; set; }       // Create - Edit
        public Dictionary<string, decimal>? DebtsOfCurrentUser { get; set; } // Index
        public List<UserDebtViewModel>? DebtsOfEverybody { get; set; } // Balances
    }
}
