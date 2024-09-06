using Microsoft.AspNetCore.Mvc.Rendering;

namespace TrueBalances.Models.ViewModels
{
    public class ExpenseViewModel
    {
                                                                // Liste des vues utilisant la propriété
        public int GroupId { get; set; }                        // Index - Create
        public string? CurrentUserId { get; set; }              // Index
        public Expense Expense { get; set; }                    // Create
        public SelectList? Categories { get; set; }             // Create
        public SelectList? Authors { get; set; }                // Create
        public List<CustomUser>? Users { get; set; }            // Index - Create
        public List<Expense>? Expenses { get; set; }            // Index
        public List<string> SelectedUserIds { get; set; }       // Create
        public Dictionary<string, decimal>? Debts { get; set; } // Index
    }
}
