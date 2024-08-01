using System;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Models;



public class CategoryDbContext : DbContext
{
    public CategoryDbContext(DbContextOptions<CategoryDbContext> options)
            : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category()
            {
                Id = 2,
                Name = "Voyage",
            },
            new Category()
            {
                Id = 3,
                Name = "Couple",
            },
            new Category()
            {
                Id = 4,
                Name = "Co-voiturage",
            }
        );
    }

    public DbSet<Expense> Expenses { get; set; }
}



