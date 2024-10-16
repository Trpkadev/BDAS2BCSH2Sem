using BCSH2BDAS2.Models;
using Microsoft.EntityFrameworkCore;

namespace BCSH2BDAS2.Data
{
    public class TransportationContext(DbContextOptions<TransportationContext> options) : DbContext(options)
    {
        public DbSet<Cleaning> Cleaning { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<TariffZone> TariffZones { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Maintenance> Maintanences { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<RouteRecord> RouteRecords { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ST69612");
            modelBuilder.Entity<Maintenance>()
                .HasDiscriminator<char>("TYP_UDRZBY")
                .HasValue<Cleaning>('c')
                .HasValue<Repair>('o')
                .HasValue<Maintenance>('x');
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }
    }
}