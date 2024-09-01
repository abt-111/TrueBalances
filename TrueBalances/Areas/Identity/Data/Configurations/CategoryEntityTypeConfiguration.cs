using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueBalances.Models;

namespace TrueBalances.Areas.Identity.Data.Configurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category() { Id = 1, Name = "Voyage", },
                new Category() { Id = 2, Name = "Couple", },
                new Category() { Id = 3, Name = "Co-voiturage" },
                new Category() { Id = 4, Name = "Catégorie supprimée", }
            );
        }
    }
}
