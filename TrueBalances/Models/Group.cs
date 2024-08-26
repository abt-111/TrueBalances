using System.ComponentModel.DataAnnotations;

namespace TrueBalances.Models
{
    public class Group
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Le nom du group est obligatoire.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Le nom doit contenir des lettres uniquement.")]
        [Display(Name = "Nom")]
        public string Name { get; set; } = string.Empty;
        public ICollection<UserGroup> Members { get; set; } = new List<UserGroup>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    }
}
