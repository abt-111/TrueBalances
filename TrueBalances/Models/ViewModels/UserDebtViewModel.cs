namespace TrueBalances.Models.ViewModels
{
    public class UserDebtViewModel
    {
        public string Id { get; set; } = string.Empty;
        public Dictionary<string, decimal> Debts { get; set; } = new Dictionary<string, decimal>();

        public UserDebtViewModel(string id)
        {
            Id = id;
        }
    }
}
