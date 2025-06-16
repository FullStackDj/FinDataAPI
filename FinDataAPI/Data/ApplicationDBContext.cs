using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FinDataAPI.Models;

namespace FinDataAPI.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    private static readonly List<IdentityRole> roles = new List<IdentityRole>
    {
        new IdentityRole
        {
            Id = "1",
            Name = "Admin",
            NormalizedName = "ADMIN"
        },

        new IdentityRole
        {
            Id = "2",
            Name = "User",
            NormalizedName = "USER"
        }
    };

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}