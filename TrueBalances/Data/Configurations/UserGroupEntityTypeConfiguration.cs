using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueBalances.Models;

namespace TrueBalances.Data.Configurations
{
    public class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder
           .HasKey(ug => ug.Id);

            builder
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(ug => ug.GroupId);

            builder
                .HasOne(ug => ug.CustomUser)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.CustomUserId);

            // Utilisation de HasData pour insérer les données initiales
            builder.HasData(
                new UserGroup { Id = 1, CustomUserId = "4593d2ab-7651-457a-ab7b-0de95e853529", GroupId = 1 },
                new UserGroup { Id = 2, CustomUserId = "5ac5c00e-aea2-4f2c-b9dd-a536682add18", GroupId = 1 },
                new UserGroup { Id = 4, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10", GroupId = 1 },
                new UserGroup { Id = 5, CustomUserId = "5bc10f58-b4a3-4f22-a363-249394a7f44f", GroupId = 2 },
                new UserGroup { Id = 6, CustomUserId = "6c32351f-9667-457d-89fb-9926df945b0a", GroupId = 2 },
                new UserGroup { Id = 7, CustomUserId = "756ed490-dcfb-44d6-a22f-741743f0db78", GroupId = 2 },
                new UserGroup { Id = 8, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10", GroupId = 2 },
                new UserGroup { Id = 9, CustomUserId = "c7de120e-be52-471f-88b8-d194fde367bd", GroupId = 3 },
                new UserGroup { Id = 10, CustomUserId = "f163064f-4672-4b73-b7c9-0b9b97e77fb4", GroupId = 3 },
                new UserGroup { Id = 11, CustomUserId = "0cd29655-84dc-48d4-a01f-61c180104e10", GroupId = 3 }
            );
        }
    }
}
