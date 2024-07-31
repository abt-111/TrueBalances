using System.ComponentModel.DataAnnotations;

namespace TrueBalances.Models;

public class Expense
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    //public string Category { get; set; }
    public string UserId { get; set; }

    public Category Category { get; set; } 
}