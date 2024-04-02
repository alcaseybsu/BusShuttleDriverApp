using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BusShuttleDriver.Domain.Models;

namespace BusShuttleDriver.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Change DbContext to IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Your DbSet properties for other entities
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Loop> Loops { get; set; }
    public DbSet<RouteModel> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Account> Accounts { get; set; }
}


