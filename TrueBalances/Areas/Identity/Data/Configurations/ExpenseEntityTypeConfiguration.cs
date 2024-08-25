using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueBalances.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrueBalances.Areas.Identity.Data.Configurations
{
    public class ExpenseEntityTypeConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasData(
                new Expense { Id = 1, Title = "Dîner de bienvenue", Amount = 150.75m, Date = new DateTime(2024, 08, 10), CategoryId = 1, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" }, // Marc
                new Expense { Id = 2, Title = "Sortie en bateau", Amount = 300.00m, Date = new DateTime(2024, 08, 11), CategoryId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" }, // Julien
                new Expense { Id = 3, Title = "Location de vélos", Amount = 80.00m, Date = new DateTime(2024, 08, 12), CategoryId = 1, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18" }, // Thomas
                new Expense { Id = 4, Title = "Visite du musée", Amount = 45.50m, Date = new DateTime(2024, 08, 13), CategoryId = 1, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f" }, // Claire
                new Expense { Id = 5, Title = "Barbecue sur la plage", Amount = 120.00m, Date = new DateTime(2024, 08, 14), CategoryId = 1, CustomUserId = "6c32351f-9667-457d-89fb-9926df945b0a" }, // Sophie
                new Expense { Id = 6, Title = "Excursion de randonnée", Amount = 60.00m, Date = new DateTime(2024, 08, 15), CategoryId = 1, CustomUserId = "756ed490-dcfb-44d6-a22f-741743f0db78" }, // Olivier
                new Expense { Id = 7, Title = "Dîner au restaurant local", Amount = 200.00m, Date = new DateTime(2024, 08, 16), CategoryId = 1, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd" }, // Elise
                new Expense { Id = 8, Title = "Cours de surf", Amount = 95.00m, Date = new DateTime(2024, 08, 17), CategoryId = 1, CustomUserId = "f163064f-4672-4b73-b7c9-0b9b97e77fb4" }, // Amelie
                new Expense { Id = 9, Title = "Concert sur la plage", Amount = 110.00m, Date = new DateTime(2024, 08, 18), CategoryId = 1, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" }, // Marc
                new Expense { Id = 10, Title = "Excursion dans les vignobles", Amount = 150.00m, Date = new DateTime(2024, 08, 19), CategoryId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" }, // Julien
                new Expense { Id = 11, Title = "Visite guidée de la ville", Amount = 80.00m, Date = new DateTime(2024, 08, 20), CategoryId = 1, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18" }, // Thomas
                new Expense { Id = 12, Title = "Soirée cinéma en plein air", Amount = 70.00m, Date = new DateTime(2024, 08, 21), CategoryId = 1, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f" }, // Claire
                new Expense { Id = 13, Title = "Brunch au café local", Amount = 130.00m, Date = new DateTime(2024, 08, 22), CategoryId = 1, CustomUserId = "6c32351f-9667-457d-89fb-9926df945b0a" }, // Sophie
                new Expense { Id = 14, Title = "Location de canoës", Amount = 90.00m, Date = new DateTime(2024, 08, 23), CategoryId = 1, CustomUserId = "756ed490-dcfb-44d6-a22f-741743f0db78" }, // Olivier
                new Expense { Id = 15, Title = "Excursion en montgolfière", Amount = 250.00m, Date = new DateTime(2024, 08, 24), CategoryId = 1, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd" }, // Elise
                new Expense { Id = 16, Title = "Cours de cuisine locale", Amount = 100.00m, Date = new DateTime(2024, 08, 25), CategoryId = 1, CustomUserId = "f163064f-4672-4b73-b7c9-0b9b97e77fb4" }, // Amelie
                new Expense { Id = 17, Title = "Location de kayaks", Amount = 75.00m, Date = new DateTime(2024, 08, 26), CategoryId = 1, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10" }, // Marc
                new Expense { Id = 18, Title = "Visite de la brasserie locale", Amount = 55.00m, Date = new DateTime(2024, 08, 27), CategoryId = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529" }, // Julien
                new Expense { Id = 19, Title = "Dîner de clôture", Amount = 180.00m, Date = new DateTime(2024, 08, 28), CategoryId = 1, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18" }, // Thomas
                new Expense { Id = 20, Title = "Excursion à cheval", Amount = 140.00m, Date = new DateTime(2024, 08, 29), CategoryId = 1, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f" } // Claire
            );
        }
    }
}
