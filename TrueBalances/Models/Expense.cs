using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TrueBalances.Areas.Identity.Data;

namespace TrueBalances.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La description est requise.")]
        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Le montant est requis.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être positif.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Montant")]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        // Clés étrangères
        public int? CategoryId { get; set; }
        public string CustomUserId { get; set; }

        // Propriétés de navigation
        [Display(Name = "Catégorie")]
        public Category? Category { get; set; }

        [Display(Name = "Auteur")]
        public CustomUser? CustomUser { get; set; }

        // Liste des participants
        public ICollection<CustomUser> Participants { get; set; } = new List<CustomUser>();
        
        [NotMapped]
        public List<string> SelectedUserIds { get; set; } = new List<string>();
    }
}