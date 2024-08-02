using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TrueBalances.Areas.Identity.Data;

namespace TrueBalances.Models;

public class Expense
{
    public int Id { get; set; }
    

    [Required(ErrorMessage = "La description est requise.")]  
    
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Le montant est requis.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être positif.")]    
    public decimal Amount { get; set; }
    
    public DateTime Date { get; set; }

    // Clés étrangères
    public int CategoryId { get; set; }
    public string CustomUserId { get; set; }

    // Propriétés de navigation
    public Category? Category { get; set; }
    public CustomUser? CustomUser { get; set; }
   
}