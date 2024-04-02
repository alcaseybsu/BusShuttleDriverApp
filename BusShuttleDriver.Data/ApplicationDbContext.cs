using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using BusShuttleDriver.Domain.Models;
using BusShuttleDriver.Domain;

namespace BusShuttleDriver.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bus> Buses { get; set; }

    public DbSet<Driver> Drivers { get; set; }

    public DbSet<Entry> Entries { get; set; }

    public DbSet<Loop> Loops { get; set; }

    public DbSet<RouteModel> Routes { get; set; }

    public DbSet<Stop> Stops { get; set; }

    public DbSet<Account> Accounts { get; set; }
}

