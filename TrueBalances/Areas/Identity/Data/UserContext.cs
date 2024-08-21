using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrueBalances.Areas.Identity.Data;
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
        builder.Entity<CustomUser>().HasData(
            new CustomUser()
            {
                Id = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42",
                FirstName = "Edouard",
                LastName = "M",
                UserName = "edouard@gmail.com",
                NormalizedUserName = "EDOUARD@GMAIL.COM",
                Email = "edouard@gmail.com",
                NormalizedEmail = "EDOUARD@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEE4G00eF57dJyX9oJJF0NN1510nEVbYKSyyTf2RFAGAYccKmlrvePEK44cZ2U6Dv7Q==",
                SecurityStamp = "XRJUJ7OVV4TMV4KTVJLCPQYKKCKAOQ3U",
                ConcurrencyStamp = "69dfc0db-09c8-4c52-b05e-15fabad0636d",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ProfilePhotoUrl = null
            },
            new CustomUser()
            {
                Id = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8",
                FirstName = "Khaoulat",
                LastName = "A",
                UserName = "khaoulat@gmail.com",
                NormalizedUserName = "KHAOULAT@GMAIL.COM",
                Email = "khaoulat@gmail.com",
                NormalizedEmail = "KHAOULAT@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEKQlOAcZifglIvPk08frVKNMbCkpRd4SUGVyar2F6B7dL9AuyJ7nr7exFLyM2ztcKQ==",
                SecurityStamp = "7WKVP5RF3JGHO34D2DJQQU53ZRC23UF4",
                ConcurrencyStamp = "855ac92c-7605-4903-b0b6-5b85d45d45a8",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ProfilePhotoUrl = null
            },
            new CustomUser()
            {
                Id = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee",
                FirstName = "Anthony",
                LastName = "T",
                UserName = "anthony@gmail.com",
                NormalizedUserName = "ANTHONY@GMAIL.COM",
                Email = "anthony@gmail.com",
                NormalizedEmail = "ANTHONY@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEL6sV74DLNsCZ2tr9rXLMmRd9AVJyfxwTketogtqrjWZ6Z8VYcgbhopSHXYoiGe02w==",
                SecurityStamp = "62WR6ELWBKB2OXPGZAZNJ62LGVFM442C",
                ConcurrencyStamp = "e0e6a53b-686f-416e-9f5f-dbdf7425a0bc",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                ProfilePhotoUrl = null
            }
        );

        // Dépenses
        builder.Entity<Expense>().HasData(
            new Expense { Id = 1, Title = "Réservation de l'hôtel", Amount = 120.00M, Date = new DateTime(2024, 8, 1), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 2, Title = "Billets d'avion", Amount = 450.00M, Date = new DateTime(2024, 8, 2), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" },
            new Expense { Id = 3, Title = "Location de voiture", Amount = 200.00M, Date = new DateTime(2024, 8, 3), CategoryId = 1, CustomUserId = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee" },
            new Expense { Id = 4, Title = "Dîner au restaurant", Amount = 85.50M, Date = new DateTime(2024, 8, 4), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 5, Title = "Excursion en bateau", Amount = 150.00M, Date = new DateTime(2024, 8, 5), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" },
            new Expense { Id = 6, Title = "Visite guidée", Amount = 60.00M, Date = new DateTime(2024, 8, 6), CategoryId = 1, CustomUserId = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee" },
            new Expense { Id = 7, Title = "Achat de souvenirs", Amount = 30.00M, Date = new DateTime(2024, 8, 7), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 8, Title = "Petit déjeuner", Amount = 25.00M, Date = new DateTime(2024, 8, 8), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" },
            new Expense { Id = 9, Title = "Entrée au musée", Amount = 18.00M, Date = new DateTime(2024, 8, 9), CategoryId = 1, CustomUserId = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee" },
            new Expense { Id = 10, Title = "Déjeuner en bord de mer", Amount = 45.00M, Date = new DateTime(2024, 8, 10), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 11, Title = "Session de plongée", Amount = 100.00M, Date = new DateTime(2024, 8, 11), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" },
            new Expense { Id = 12, Title = "Soirée en ville", Amount = 70.00M, Date = new DateTime(2024, 8, 12), CategoryId = 1, CustomUserId = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee" },
            new Expense { Id = 13, Title = "Transport en taxi", Amount = 35.00M, Date = new DateTime(2024, 8, 13), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 14, Title = "Glaces pour tout le groupe", Amount = 22.00M, Date = new DateTime(2024, 8, 14), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" },
            new Expense { Id = 15, Title = "Billets de train", Amount = 65.00M, Date = new DateTime(2024, 8, 15), CategoryId = 1, CustomUserId = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee" },
            new Expense { Id = 16, Title = "Location de vélos", Amount = 40.00M, Date = new DateTime(2024, 8, 16), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 17, Title = "Cocktails en soirée", Amount = 55.00M, Date = new DateTime(2024, 8, 17), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" },
            new Expense { Id = 18, Title = "Dîner à la pizzeria", Amount = 30.00M, Date = new DateTime(2024, 8, 18), CategoryId = 1, CustomUserId = "3859a9ac-e6e2-42fa-9ef7-364c3d4042ee" },
            new Expense { Id = 19, Title = "Location de parasols", Amount = 15.00M, Date = new DateTime(2024, 8, 19), CategoryId = 1, CustomUserId = "1d8b5ce1-aca4-4eb0-b469-bd91cb534c42" },
            new Expense { Id = 20, Title = "Pique-nique sur la plage", Amount = 50.00M, Date = new DateTime(2024, 8, 20), CategoryId = 1, CustomUserId = "37e4f03c-2e75-4c48-ba9e-4b6bb71694f8" }
        );
    }
}
