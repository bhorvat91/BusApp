using BusApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Infrastructure.Data;

public class BusAppDbContext : IdentityDbContext<AppUser>
{
    public BusAppDbContext(DbContextOptions<BusAppDbContext> options) : base(options) { }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<Driver> Drivers => Set<Driver>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<FleetNotification> Notifications => Set<FleetNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>(e =>
        {
            e.HasKey(c => c.Id);
        });

        modelBuilder.Entity<Bus>(e =>
        {
            e.HasKey(b => b.Id);
            e.OwnsOne(b => b.Location);
        });

        modelBuilder.Entity<Driver>(e =>
        {
            e.HasKey(d => d.Id);
        });

        modelBuilder.Entity<Reservation>(e =>
        {
            e.HasKey(r => r.Id);
        });

        modelBuilder.Entity<FleetNotification>(e =>
        {
            e.HasKey(n => n.Id);
        });
    }
}
