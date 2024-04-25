using BusShuttleDriver.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BusShuttleDriver.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Db Sets
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Loop> Loops { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ASP.NET Identity model customization
            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"));
            modelBuilder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
                entity.ToTable(name: "UserRoles")
            );
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

            // Configuration for Loop entity
            modelBuilder.Entity<Loop>(entity =>
            {
                entity.ToTable("Loops");
                entity.HasKey(l => l.Id);
                entity.Property(l => l.LoopName).IsRequired().HasColumnType("TEXT");
                entity
                    .HasMany(l => l.Stops)
                    .WithOne(s => s.Loop)
                    .HasForeignKey(s => s.LoopId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Stop properties
            modelBuilder.Entity<Stop>(entity =>
            {
                entity.ToTable("Stops");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Latitude).HasColumnType("REAL");
                entity.Property(s => s.Longitude).HasColumnType("REAL");
                entity.Property(s => s.Order).HasColumnType("INTEGER");
                entity.HasIndex(s => new { s.LoopId, s.Order }).IsUnique();
            });

            // Configuration for Route entity
            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("Routes");
                entity.HasKey(r => r.RouteId);
                entity.Property(r => r.RouteName).IsRequired().HasMaxLength(100);
                entity
                    .HasOne(r => r.Loop)
                    .WithMany()
                    .HasForeignKey(r => r.LoopId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuration for RouteStop entity
            modelBuilder.Entity<RouteStop>().HasKey(rs => new { rs.RouteId, rs.StopId });

            modelBuilder
                .Entity<RouteStop>()
                .HasOne(rs => rs.Route)
                .WithMany(r => r.RouteStops)
                .HasForeignKey(rs => rs.RouteId);

            modelBuilder
                .Entity<RouteStop>()
                .HasOne(rs => rs.Stop)
                .WithMany()
                .HasForeignKey(rs => rs.StopId); // Ensure you define a navigation property in Stop if needed.

            modelBuilder.Entity<RouteStop>().Property(rs => rs.Order).IsRequired();

            // Configuration for Session
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Sessions");
                entity.HasKey(s => s.Id);
                entity
                    .HasOne(s => s.Driver)
                    .WithMany()
                    .HasForeignKey(s => s.DriverId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(s => s.Bus).WithMany().HasForeignKey(s => s.BusId);
                entity.HasOne(s => s.Loop).WithMany().HasForeignKey(s => s.LoopId);
                entity
                    .HasOne(s => s.Driver)
                    .WithOne(d => d.ActiveSession)
                    .HasForeignKey<Driver>(d => d.ActiveSessionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
