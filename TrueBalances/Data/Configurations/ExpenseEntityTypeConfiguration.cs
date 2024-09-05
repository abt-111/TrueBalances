using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueBalances.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrueBalances.Data.Configurations
{
    public class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder
                .Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)");

            // Configuration de la relation one-to-many entre Expense et CustomUser (en tant que créateur)
            builder
                .HasOne(e => e.CustomUser)
                .WithMany(u => u.CreatedExpenses)
                .HasForeignKey(e => e.CustomUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration de la relation many-to-many entre Expense et CustomUser (en tant que participant)
            builder
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

            // Configuration de la relation entre Group et Expense pour la suppression en cascade
            builder
                .HasOne(e => e.Group)
                .WithMany(g => g.Expenses)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // Groupe 1
            builder.HasData(
                new Expense { Id = 1, Title = "Déjeuner", Amount = 58.60m, Date = new DateTime(2024, 9, 1), CategoryId = 1, GroupId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" },
                new Expense { Id = 2, Title = "Boissons", Amount = 73.50m, Date = new DateTime(2024, 9, 2), CategoryId = 1, GroupId = 1, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18" },
                new Expense { Id = 3, Title = "Repas du midi", Amount = 40.25m, Date = new DateTime(2024, 9, 3), CategoryId = 1, GroupId = 1, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 4, Title = "Concert", Amount = 120.00m, Date = new DateTime(2024, 9, 4), CategoryId = 1, GroupId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" },
                new Expense { Id = 5, Title = "Réunion déjeuner", Amount = 95.00m, Date = new DateTime(2024, 9, 5), CategoryId = 1, GroupId = 1, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18" },
                new Expense { Id = 6, Title = "Karaoké", Amount = 89.90m, Date = new DateTime(2024, 9, 6), CategoryId = 1, GroupId = 1, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 7, Title = "Café et viennoiseries", Amount = 22.45m, Date = new DateTime(2024, 9, 7), CategoryId = 1, GroupId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" },
                new Expense { Id = 8, Title = "Billiard", Amount = 55.20m, Date = new DateTime(2024, 9, 8), CategoryId = 1, GroupId = 1, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18" },
                new Expense { Id = 9, Title = "Petit déjeuner", Amount = 37.80m, Date = new DateTime(2024, 9, 9), CategoryId = 1, GroupId = 1, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 10, Title = "Escape Game", Amount = 85.30m, Date = new DateTime(2024, 9, 10), CategoryId = 1, GroupId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" },
                // Groupe 2
                new Expense { Id = 11, Title = "Restaurant", Amount = 150.00m, Date = new DateTime(2024, 9, 1), CategoryId = 1, GroupId = 2, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f" },
                new Expense { Id = 12, Title = "Hôtel", Amount = 300.00m, Date = new DateTime(2024, 9, 2), CategoryId = 1, GroupId = 2, CustomUserId = "6c32351f-9667-457d-89fb-9926df945b0a" },
                new Expense { Id = 13, Title = "Cinéma", Amount = 45.00m, Date = new DateTime(2024, 9, 3), CategoryId = 1, GroupId = 2, CustomUserId = "756ed490-dcfb-44d6-a22f-741743f0db78" },
                new Expense { Id = 14, Title = "Transport", Amount = 120.00m, Date = new DateTime(2024, 9, 4), CategoryId = 1, GroupId = 2, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 15, Title = "Parc d'attraction", Amount = 70.00m, Date = new DateTime(2024, 9, 5), CategoryId = 1, GroupId = 2, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f" },
                new Expense { Id = 16, Title = "Excursion", Amount = 200.00m, Date = new DateTime(2024, 9, 6), CategoryId = 1, GroupId = 2, CustomUserId = "6c32351f-9667-457d-89fb-9926df945b0a" },
                new Expense { Id = 17, Title = "Escape Room", Amount = 60.00m, Date = new DateTime(2024, 9, 7), CategoryId = 1, GroupId = 2, CustomUserId = "756ed490-dcfb-44d6-a22f-741743f0db78" },
                new Expense { Id = 18, Title = "Musée", Amount = 30.00m, Date = new DateTime(2024, 9, 8), CategoryId = 1, GroupId = 2, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 19, Title = "Bowling", Amount = 35.50m, Date = new DateTime(2024, 9, 9), CategoryId = 1, GroupId = 2, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f" },
                new Expense { Id = 20, Title = "Camping", Amount = 90.00m, Date = new DateTime(2024, 9, 10), CategoryId = 1, GroupId = 2, CustomUserId = "6c32351f-9667-457d-89fb-9926df945b0a" },
                // Groupe 3
                new Expense { Id = 21, Title = "Zoo", Amount = 65.00m, Date = new DateTime(2024, 9, 1), CategoryId = 1, GroupId = 3, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd" },
                new Expense { Id = 22, Title = "Hébergement", Amount = 180.00m, Date = new DateTime(2024, 9, 2), CategoryId = 1, GroupId = 3, CustomUserId = "f163064f-4672-4b73-b7c9-0b9b97e77fb4" },
                new Expense { Id = 23, Title = "Pique-nique", Amount = 25.00m, Date = new DateTime(2024, 9, 3), CategoryId = 1, GroupId = 3, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 24, Title = "Aqua Park", Amount = 50.00m, Date = new DateTime(2024, 9, 4), CategoryId = 1, GroupId = 3, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd" },
                new Expense { Id = 25, Title = "Dîner", Amount = 75.00m, Date = new DateTime(2024, 9, 5), CategoryId = 1, GroupId = 3, CustomUserId = "f163064f-4672-4b73-b7c9-0b9b97e77fb4" },
                new Expense { Id = 26, Title = "Théâtre", Amount = 60.00m, Date = new DateTime(2024, 9, 6), CategoryId = 1, GroupId = 3, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 27, Title = "Visite guidée", Amount = 40.00m, Date = new DateTime(2024, 9, 7), CategoryId = 1, GroupId = 3, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd" },
                new Expense { Id = 28, Title = "Parc national", Amount = 55.00m, Date = new DateTime(2024, 9, 8), CategoryId = 1, GroupId = 3, CustomUserId = "f163064f-4672-4b73-b7c9-0b9b97e77fb4" },
                new Expense { Id = 29, Title = "Concert", Amount = 95.00m, Date = new DateTime(2024, 9, 9), CategoryId = 1, GroupId = 3, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" },
                new Expense { Id = 30, Title = "Sortie nocturne", Amount = 80.00m, Date = new DateTime(2024, 9, 10), CategoryId = 1, GroupId = 3, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd" }
            );
        }
    }
}
