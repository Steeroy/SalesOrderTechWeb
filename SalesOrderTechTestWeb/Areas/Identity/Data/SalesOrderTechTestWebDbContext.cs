using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesOrderTechTestWeb.Areas.Identity.Data;
using SalesOrderTechTestWeb.Models;

namespace SalesOrderTechTestWeb.Data;

public class SalesOrderTechTestWebDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLine { get; set; }
    public SalesOrderTechTestWebDbContext(DbContextOptions<SalesOrderTechTestWebDbContext> options)
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

    
}
