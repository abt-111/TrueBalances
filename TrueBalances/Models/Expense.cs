using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrueBalances.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire.")]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être positif.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Montant")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Le date est obligatoire.")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        // Clés étrangères
        [Display(Name = "Catégorie")]
        public int? CategoryId { get; set; }
        [Required]
        public int GroupId { get; set; }

        [Display(Name = "Auteur")]
        [Required(ErrorMessage = "L'auteur est obligatoire.")]
        public string UserId { get; set; }

        // Propriétés de navigation
        public Category? Category { get; set; }
        public Group? Group { get; set; }
        public CustomUser? User { get; set; }

        // Liste des participants
        public ICollection<CustomUser> Participants { get; set; } = new List<CustomUser>();
    }
}