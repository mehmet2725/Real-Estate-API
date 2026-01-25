using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Entity.Concrete;

namespace RealEstate.Data.Concrete;

public class RealEstateDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) : base(options)
    {
    }

    // Tables
    // We add tables like Property, Inquiry, etc. here so they are created in the database.    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<Inquiry> Inquiries { get; set; }

    // Note: AppUser and AppRole already exist within IdentityDbContext, so we don't need to rewrite them.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Privatizations 
    }
}
