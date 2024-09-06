namespace TrueBalances.Models.ViewModels
{
    public class ExpenseIndexViewModel
    {
        public int GroupId { get; set; }
        public string CurrentUserId { get; set; }
        public List<CustomUser> Users { get; set; }
        public List<Expense> Expenses { get; set; }
        public Dictionary<string, decimal> Debts { get; set; }
    }
}
