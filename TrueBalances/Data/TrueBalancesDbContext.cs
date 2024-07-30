using System;
using Microsoft.EntityFrameworkCore;



public class TrueBalancesDbContext : DbContext
{
    public TrueBalancesDbContext(DbContextOptions<TrueBalancesDbContext> options)
            : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
}

