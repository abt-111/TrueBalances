using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Models;

namespace TrueBalances.Data;

public class DbContext : IdentityDbContext<CustomUser>
{
    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                Name = "Voyage",
            },
            new Category()
            {
                Id = 2,
                Name = "Couple",
            },
            new Category()
            {
                Id = 3,
                Name = "Co-voiturage",
            }
        );
    }
}
