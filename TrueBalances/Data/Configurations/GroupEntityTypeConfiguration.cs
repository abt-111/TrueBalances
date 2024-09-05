using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueBalances.Models;

namespace TrueBalances.Data.Configurations
{
    public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasData(
                new Group { Id = 1, Name = "Collègues", CategoryId = null },
                new Group { Id = 2, Name = "Amis", CategoryId = null },
                new Group { Id = 3, Name = "Famille", CategoryId = null }
            );
        }
    }
}