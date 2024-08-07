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

        builder.Entity<UserGroup>()
            .HasKey(ug => new { ug.GroupId, ug.CustomUserId });

        builder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(ug => ug.GroupId);

        //builder.Entity<UserGroup>()
        //    .HasOne(ug => ug.CustomUser)
        //    .WithMany(u => u.UserGroups)
        //    .HasForeignKey(ug => ug.CustomUserId);

        builder.Entity<UserGroup>()
           .HasKey(ug => ug.Id);


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

        /*
         * A ne pas utilisé finalement. Les CustomUserId sont spécifique à ma BDD et donc posent problème pour les migrations.
         * 
         * builder.Entity<Expense>().HasData(
            new Expense()
            {
                Id = 1,
                Title = "Lunch",
                Amount = 16,
                Date = DateTime.Parse("2023-08-01 12:30:00"),
                CategoryId = 1,
                CustomUserId = "94981d8e-a7f9-4ce0-a695-5420627372ed",
                GroupId = null,
            },
            new Expense()
            {
                Id = 2,
                Title = "Office Supplies",
                Amount = 26,
                Date = DateTime.Parse("2023-08-02 09:15:00"),
                CategoryId = 1,
                CustomUserId = "9b208126-4a01-4e86-bac5-b272e239c47b",
                GroupId = null,
            },
            new Expense()
            {
                Id = 3,
                Title = "Transportation",
                Amount = 7,
                Date = DateTime.Parse("2023-08-03 08:00:00"),
                CategoryId = 1,
                CustomUserId = "fba2ff14-ea5b-460e-a832-6957513e97c1",
                GroupId = null,
            },
            new Expense()
            {
                Id = 4,
                Title = "Coffee",
                Amount = 4,
                Date = DateTime.Parse("2023-08-04 10:45:00"),
                CategoryId = 1,
                CustomUserId = "94981d8e-a7f9-4ce0-a695-5420627372ed",
                GroupId = null,
            },
            new Expense()
            {
                Id = 5,
                Title = "Dinner",
                Amount = 20,
                Date = DateTime.Parse("2023-08-05 19:30:00"),
                CategoryId = 1,
                CustomUserId = "9b208126-4a01-4e86-bac5-b272e239c47b",
                GroupId = null,
            },
            new Expense()
            {
                Id = 6,
                Title = "Parking",
                Amount = 5,
                Date = DateTime.Parse("2023-08-06 14:00:00"),
                CategoryId = 1,
                CustomUserId = "fba2ff14-ea5b-460e-a832-6957513e97c1",
                GroupId = null,
            },
            new Expense()
            {
                Id = 7,
                Title = "Books",
                Amount = 30,
                Date = DateTime.Parse("2023-08-07 11:00:00"),
                CategoryId = 1,
                CustomUserId = "94981d8e-a7f9-4ce0-a695-5420627372ed",
                GroupId = null,
            },
            new Expense()
            {
                Id = 8,
                Title = "Groceries",
                Amount = 46,
                Date = DateTime.Parse("2023-08-08 17:00:00"),
                CategoryId = 1,
                CustomUserId = "9b208126-4a01-4e86-bac5-b272e239c47b",
                GroupId = null,
            },
            new Expense()
            {
                Id = 9,
                Title = "Gym Membership",
                Amount = 50,
                Date = DateTime.Parse("2023-08-09 07:00:00"),
                CategoryId = 1,
                CustomUserId = "fba2ff14-ea5b-460e-a832-6957513e97c1",
                GroupId = null,
            },
            new Expense()
            {
                Id = 10,
                Title = "Medical Expenses",
                Amount = 100,
                Date = DateTime.Parse("2023-08-10 15:00:00"),
                CategoryId = 1,
                CustomUserId = "94981d8e-a7f9-4ce0-a695-5420627372ed",
                GroupId = null,
            }
        );*/

    }
}