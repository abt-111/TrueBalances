using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Areas.Identity.Data.Configurations;
using TrueBalances.Models;

namespace TrueBalances.Data;

public class UserContext : IdentityDbContext<CustomUser>
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UsersGroup { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<UserGroup>()
            .HasKey(ug => new { ug.GroupId, ug.CustomUserId });

        builder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(ug => ug.GroupId);

        builder.Entity<UserGroup>()
            .HasOne(ug => ug.CustomUser)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.CustomUserId);

        builder.Entity<UserGroup>()
           .HasKey(ug => ug.Id);


        base.OnModelCreating(builder);

        // Configuration de la relation one-to-many entre Expense et CustomUser (en tant que créateur)
        builder.Entity<Expense>()
            .HasOne(e => e.CustomUser)
            .WithMany(u => u.CreatedExpenses)
            .HasForeignKey(e => e.CustomUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuration de la relation many-to-many entre Expense et CustomUser (en tant que participant)
        builder.Entity<Expense>()
            .HasMany(e => e.Participants)
            .WithMany(u => u.ParticipatingExpenses)
            .UsingEntity<Dictionary<string, object>>(
                "ExpenseParticipant",
                j => j.HasOne<CustomUser>()
                    .WithMany()
                    .HasForeignKey("CustomUserId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Expense>()
                    .WithMany()
                    .HasForeignKey("ExpenseId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ExpenseId", "CustomUserId");
                    j.ToTable("ExpenseParticipants");
                }
            );

        builder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasColumnType("decimal(18, 2)");

        builder.Entity<Expense>()
       .HasOne(e => e.Category)
       .WithMany(c => c.Expenses)
       .HasForeignKey(e => e.CategoryId)
       .OnDelete(DeleteBehavior.SetNull);

        // Données par défaut

        // Catégories
        builder.Entity<Category>().HasData(
            new Category() { Id = 1, Name = "Voyage", },
            new Category() { Id = 2, Name = "Couple", },
            new Category() { Id = 3, Name = "Co-voiturage", }
        );

        // Utilisateurs
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());

        // Dépenses
        builder.ApplyConfiguration(new ExpenseEntityTypeConfiguration());
    }
}
