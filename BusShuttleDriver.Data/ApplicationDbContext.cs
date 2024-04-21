using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Db Sets
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Loop> Loops { get; set; }
    public DbSet<RouteModel> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<RouteSession> RouteSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customize the ASP.NET Identity model and override the defaults if needed
        modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"));
        modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        modelBuilder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            entity.ToTable(name: "UserClaims")
        );
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            entity.ToTable(name: "UserLogins")
        );
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            entity.ToTable(name: "RoleClaims")
        );
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            entity.ToTable(name: "UserTokens")
        );

        // Route to Loop relationship
        modelBuilder
            .Entity<RouteModel>()
            .HasOne(r => r.Loop) // Route has one Loop
            .WithMany(l => l.Routes) // Loop has many Routes
            .HasForeignKey(r => r.LoopId) // Foreign key in Route
            .OnDelete(DeleteBehavior.SetNull); // Set LoopId to null if Loop is deleted

        // Stop to Route relationship
        modelBuilder
            .Entity<Stop>()
            .HasOne(s => s.Route) // Stop has one Route
            .WithMany(r => r.Stops) // Route has many Stops
            .HasForeignKey(s => s.RouteId) // Foreign key in Stop pointing to Route
            .IsRequired(false) // Make foreign key optional
            .OnDelete(DeleteBehavior.SetNull); // Set RouteId to null if Route is deleted

        modelBuilder.Entity<Stop>().Property(s => s.Name).IsRequired().HasMaxLength(100); // Stop name max length

        // Ensure unique order for Stops within the same Route
        modelBuilder.Entity<Stop>().HasIndex(s => new { s.RouteId, s.Order }).IsUnique();

        // Driver to RouteSession relationship
        modelBuilder
            .Entity<Driver>()
            .HasOne(d => d.ActiveRouteSession)
            .WithOne() // Driver has one ActiveRouteSession at a time
            .HasForeignKey<Driver>(d => d.ActiveRouteSessionId) // Foreign key in Driver
            .OnDelete(DeleteBehavior.SetNull); // ActiveRouteSessionId to null when RouteSession deleted

        // RouteSession Configuration
        modelBuilder
            .Entity<RouteSession>()
            .HasOne(rs => rs.Driver)
            .WithMany()
            .HasForeignKey(rs => rs.DriverId)
            .OnDelete(DeleteBehavior.SetNull); // Set DriverId null if Driver deleted

        modelBuilder
            .Entity<RouteSession>()
            .HasOne(rs => rs.Bus)
            .WithMany()
            .HasForeignKey(rs => rs.BusId);

        modelBuilder
            .Entity<RouteSession>()
            .HasOne(rs => rs.Loop)
            .WithMany()
            .HasForeignKey(rs => rs.LoopId);
    }
}
