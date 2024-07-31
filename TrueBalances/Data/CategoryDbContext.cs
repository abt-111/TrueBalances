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
}

