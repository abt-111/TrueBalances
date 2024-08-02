using System;
using System.ComponentModel.DataAnnotations;


namespace TrueBalances.Models;
public class Category
{
	public int Id { get; set; }

    [Required(ErrorMessage = "Le nom de la catégorie est obligatoire.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Le nom doit contenir des lettres uniquement.")]
    public string Name { get; set; } = string.Empty;

    //Ajouter un constructeur pour lier la catégorie avec dépense

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}


