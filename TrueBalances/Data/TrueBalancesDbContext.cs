using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Data.Configurations;
using TrueBalances.Models;

namespace TrueBalances.Data;

public class TrueBalancesDbContext : IdentityDbContext<CustomUser>
{
    public TrueBalancesDbContext(DbContextOptions<TrueBalancesDbContext> options) : base(options)
    { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuration et données par défaut :
        // - Catégories
        builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        // - Utilisateurs
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        // - Groupes
        builder.ApplyConfiguration(new GroupEntityTypeConfiguration());
        // - Association Utilisateurs et Groupes
        builder.ApplyConfiguration(new UserGroupEntityTypeConfiguration());
        // - Dépenses
        builder.ApplyConfiguration(new ExpenseEntityTypeConfiguration());
    }
}
