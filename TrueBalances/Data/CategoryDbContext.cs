using System;
using Microsoft.EntityFrameworkCore;



public class CategoryDbContext : DbContext
{
    public CategoryDbContext(DbContextOptions<CategoryDbContext> options)
            : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
}

