﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

    public DbSet<Group> Groups { get; set; }
    public DbSet<UserGroup> UsersGroup { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(ug => ug.GroupId);

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
    }
}




//builder.Entity<UserGroup>()
//            .HasKey(ug => new { ug.GroupId, ug.UserId });


//builder.Entity<Group>()
//    .HasMany(g => g.Members)
//    .WithOne(m => m.Group)
//    .HasForeignKey(m => m.GroupId);