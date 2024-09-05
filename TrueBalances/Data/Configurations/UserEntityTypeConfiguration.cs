using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrueBalances.Models;

namespace TrueBalances.Data.Configurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<CustomUser>
    {
        public void Configure(EntityTypeBuilder<CustomUser> builder)
        {
            builder.HasData(
                new CustomUser
                {
                    Id = "0cd29655-84dc-48d4-a01f-61c180104e10",
                    FirstName = "Marc",
                    LastName = "L",
                    ProfilePhotoUrl = null,
                    UserName = "marc.l@mail.fr",
                    NormalizedUserName = "MARC.L@MAIL.FR",
                    Email = "marc.l@mail.fr",
                    NormalizedEmail = "MARC.L@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEEVNx5G9yT762nYjueVgk3VF8rCBOpONM+otauLfKZWjXnwYG6MR6XLcvH4rE7FUag==",
                    SecurityStamp = "4BXJ777IQNV64CYH2O6LUK4BTJD4VONR",
                    ConcurrencyStamp = "4c2352fc-da57-4d83-ac66-336535c7469f",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "4593d2ab-7651-457a-ab7b-0de95e853529",
                    FirstName = "Julien",
                    LastName = "P",
                    ProfilePhotoUrl = null,
                    UserName = "julien.p@mail.fr",
                    NormalizedUserName = "JULIEN.P@MAIL.FR",
                    Email = "julien.p@mail.fr",
                    NormalizedEmail = "JULIEN.P@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEBJOPijBEv/l3J3JG9PGgvxD8LsdrQxJ3kLvL+GyTUIBrto+qH+LftABGroTrEC0sg==",
                    SecurityStamp = "6FK6FRYXMOLGCWC3CD5PJN6AZHZMWJAI",
                    ConcurrencyStamp = "ccf6f7d4-0a8e-4e10-af1b-fae03a203d4d",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "5ac5c00e-aea2-4f2c-b9dd-a536682add18",
                    FirstName = "Thomas",
                    LastName = "M",
                    ProfilePhotoUrl = null,
                    UserName = "thomas.m@mail.fr",
                    NormalizedUserName = "THOMAS.M@MAIL.FR",
                    Email = "thomas.m@mail.fr",
                    NormalizedEmail = "THOMAS.M@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEIa63F2APrB+hjMTFLnNmvPtAk7qfXrfBhiq4c3NelVWYri7YHxR+Hu7T8sLcgtRHg==",
                    SecurityStamp = "252OQFVLITBZYCO7PRNAYXOIFB56S5DO",
                    ConcurrencyStamp = "30fe2c1b-8b1b-4300-b59c-e38fbf27c1cc",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "5bc10f58-b4a3-4f22-a363-249394a7f44f",
                    FirstName = "Claire",
                    LastName = "T",
                    ProfilePhotoUrl = null,
                    UserName = "claire.t@mail.fr",
                    NormalizedUserName = "CLAIRE.T@MAIL.FR",
                    Email = "claire.t@mail.fr",
                    NormalizedEmail = "CLAIRE.T@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEKuitraxMChdrC3Ky/VDH9a5OOXsZPEMZqzsntXt6Rsxf/lUXbrdW+kt0o+ScA0iPg==",
                    SecurityStamp = "QPJYL4HYKCZMKA7RJ5CHQ5W44D5OZGLA",
                    ConcurrencyStamp = "58771b9f-4276-4d94-b529-cd6c77ea4c46",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "6c32351f-9667-457d-89fb-9926df945b0a",
                    FirstName = "Sophie",
                    LastName = "B",
                    ProfilePhotoUrl = null,
                    UserName = "sophie.b@mail.fr",
                    NormalizedUserName = "SOPHIE.B@MAIL.FR",
                    Email = "sophie.b@mail.fr",
                    NormalizedEmail = "SOPHIE.B@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEEEBjXd5oLDKO79jYb8T13x0pXilNwtXTEj2xhtQi8JhWG8q0hTSgpB9Pe5g1/1Vfg==",
                    SecurityStamp = "JHPFAJXZXCP2XGBK65Z64ESMKVHMUPYT",
                    ConcurrencyStamp = "bfe41887-3480-4193-ba4b-e0a13d036f88",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "756ed490-dcfb-44d6-a22f-741743f0db78",
                    FirstName = "Olivier",
                    LastName = "G",
                    ProfilePhotoUrl = null,
                    UserName = "olivier.g@mail.fr",
                    NormalizedUserName = "OLIVIER.G@MAIL.FR",
                    Email = "olivier.g@mail.fr",
                    NormalizedEmail = "OLIVIER.G@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEAnZOttHWkvy6hrFCs+xoyUHvUHCWuYSWNyU9tQfERVS6pq5asPHLNUvDyXlom1Dvw==",
                    SecurityStamp = "54MRBFENUFM4FJQ5THS2RCWNQLGLFXS6",
                    ConcurrencyStamp = "4d8e96bc-853c-44b4-bb01-08df2353a494",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "c7de120e-be52-471f-88b8-d194fde367bd",
                    FirstName = "Elise",
                    LastName = "R",
                    ProfilePhotoUrl = null,
                    UserName = "elise.r@mail.fr",
                    NormalizedUserName = "ELISE.R@MAIL.FR",
                    Email = "elise.r@mail.fr",
                    NormalizedEmail = "ELISE.R@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEND3xwEhBYCe/21Eq/w32TLsO602E+KX1qYc39D42W0ASctmS4R4Z58Phdt0WG56tQ==",
                    SecurityStamp = "XGJEAXWAQJRFSBXCOMGIUIYFSZLIFMNQ",
                    ConcurrencyStamp = "5d5c0640-0b2b-4cd0-9965-d40768e0f6a6",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new CustomUser
                {
                    Id = "f163064f-4672-4b73-b7c9-0b9b97e77fb4",
                    FirstName = "Amelie",
                    LastName = "D",
                    ProfilePhotoUrl = null,
                    UserName = "amelie.d@mail.fr",
                    NormalizedUserName = "AMELIE.D@MAIL.FR",
                    Email = "amelie.d@mail.fr",
                    NormalizedEmail = "AMELIE.D@MAIL.FR",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEDbbLahlXegTDn2fBuMHBC4gNgvRWzl4OANZvfkKmYRknxHCCte+Uz2DkB8/zEReDw==",
                    SecurityStamp = "N6RH22SHRKPMVXQBRFWHNFF2C2G7OUKO",
                    ConcurrencyStamp = "7cf87ac2-a9d7-45b7-a99f-4ef70711ee0c",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }
            );
        }
    }
}
