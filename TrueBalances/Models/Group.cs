using System.ComponentModel.DataAnnotations;

namespace TrueBalances.Models
{
    public class Group
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le nom du group est obligatoire.")]
        public string Name { get; set; } = string.Empty;
        public ICollection<UserGroup> Members { get; set; } = new List<UserGroup>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

        //public byte[] BannerImage { get; set; }
    }
}
