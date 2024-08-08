using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TrueBalances.Areas.Identity.Data;
using TrueBalances.Models;

namespace TrueBalances.Data;

public class UserContext : IdentityDbContext<CustomUser>
{
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ProfilePhoto> ProfilePhotos { get; set; }

    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UsersGroup { get; set; }
    public DbSet<CustomUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        //builder.Entity<UserGroup>()
        //    .HasKey(ug => new { ug.GroupId, ug.CustomUserId });

        //builder.Entity<UserGroup>()
        //    .HasOne(ug => ug.Group)
        //    .WithMany(g => g.Members)
        //    .HasForeignKey(ug => ug.GroupId);

        //builder.Entity<UserGroup>()
        //    .HasOne(ug => ug.CustomUser)
        //    .WithMany(u => u.UserGroups)
        //    .HasForeignKey(ug => ug.CustomUserId);

        //builder.Entity<UserGroup>()
        //   .HasKey(ug => ug.Id);


        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Expense>()
        .Property(e => e.Amount)
        .HasColumnType("decimal(18, 2)");

        builder.Entity<Expense>()
       .HasOne(e => e.Category)
       .WithMany(c => c.Expenses)
       .HasForeignKey(e => e.CategoryId)
       .OnDelete(DeleteBehavior.SetNull);

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