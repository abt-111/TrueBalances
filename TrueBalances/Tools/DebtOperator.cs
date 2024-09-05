using TrueBalances.Models;

namespace TrueBalances.Tools
{
    public static class DebtOperator
    {
        public static Decimal GetDebtValue(List<Expense> expenses, string creditor, string debtor)
        {
            return Math.Round(
                            expenses
                            .Where(e => e.CustomUserId == creditor && e.Participants.Any(p => p.Id == debtor))
                            .Sum(e => e.Participants.Count != 0 ? e.Amount / e.Participants.Count : 0)
                        , 2);
        }

        public static Dictionary<string, decimal> GetSomeoneDebts(List<Expense> expenses, List<CustomUser> users, string UserId)
        {
            Dictionary<string, decimal> debts = new Dictionary<string, decimal>();

            var others = users.FindAll(u => u.Id != UserId);

            if (others != null && others.Count > 0)
            {
                foreach (var other in others)
                {
                    var debt = GetDebtValue(expenses, other.Id, UserId);
                    var credit = GetDebtValue(expenses, UserId, other.Id);
                    var trueBalance = debt - credit;

                    debts.Add(other.Id, trueBalance);
                }
            }
            return debts;
        }

        public static Dictionary<string, decimal> GetCredits(List<Expense> expenses, List<CustomUser> users, string UserId)
        {
            Dictionary<string, decimal> credits = new Dictionary<string, decimal>();

            var others = users.FindAll(u => u.Id != UserId);

            if (others != null && others.Count > 0)
            {
                foreach (var other in others)
                {
                    var credit = GetDebtValue(expenses, UserId, other.Id);
                    credits.Add(other.Id, credit);
                }
            }
            return credits;
        }

        public static List<UserDebtViewModel> GetDebtsOfEverybody(List<Expense> expenses, List<CustomUser> users)
        {
            List<UserDebtViewModel> debtsOfEverybody = new List<UserDebtViewModel>();

            foreach (var user in users)
            {
                var userDebt = new UserDebtViewModel(user.Id);
                userDebt.Debts = GetSomeoneDebts(expenses, users, user.Id);
                debtsOfEverybody.Add(userDebt);
            }
            return debtsOfEverybody;
        }
    }
}
