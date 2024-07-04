using Lost_And_Found.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Lost_And_Found.Models;
using Lost_And_Found.Admin;

namespace Lost_And_Found.Areas.Identity.Data;

public class CoreProjectContext : IdentityDbContext<AuthUser>
{
    public CoreProjectContext(DbContextOptions<CoreProjectContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<Lost_And_Found.Models.Main>? Main { get; set; }

    public DbSet<Lost_And_Found.Models.LostItem>? LostItem { get; set; }

    public DbSet<Lost_And_Found.Models.Finded>? Finded { get; set; }

    public DbSet<Lost_And_Found.Models.Help>? Help { get; set; }

    public DbSet<Lost_And_Found.Admin.Admin>? Admin { get; set; }
}
