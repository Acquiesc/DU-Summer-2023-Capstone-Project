using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DU_Summer_2023_Capstone.Data;

public class DU_Summer_2023_CapstoneContext : IdentityDbContext<IdentityUser>
{
    public DU_Summer_2023_CapstoneContext(DbContextOptions<DU_Summer_2023_CapstoneContext> options)
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
