using EvidencijaAparata.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EvidencijaAparata.Server.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<GMLocation> GMLocations { get; set; }
        public DbSet<GMLocationAct> GMLocationActs { get; set; }

        private static bool isDatabaseSeeded = false;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            City[] cities = [
                new() { Id = 1, Naziv = "City 1" },
                new() { Id = 2, Naziv = "City 2" },
                new() { Id = 3, Naziv = "City 3" },
                new() { Id = 4, Naziv = "City 4" },
                new() { Id = 5, Naziv = "City 5" }
            ];

            modelBuilder.Entity<City>().HasData(cities);

            object[] gmLocations = [
                new { MestoId = 1, Id = 1, rul_base_id = 1, Naziv = "Lokacija 1", Adresa = "Adresa 1", IP = "192.168.0.1" },
                new { MestoId = 2, Id = 2, rul_base_id = 2, Naziv = "Lokacija 2", Adresa = "Adresa 2", IP = "192.168.0.2" },
                new { MestoId = 3, Id = 3, rul_base_id = 3, Naziv = "Lokacija 3", Adresa = "Adresa 3", IP = "192.168.0.3" },
                new { MestoId = 4, Id = 4, rul_base_id = 4, Naziv = "Lokacija 4", Adresa = "Adresa 4", IP = "192.168.0.4" },
                new { MestoId = 5, Id = 5, rul_base_id = 5, Naziv = "Lokacija 5", Adresa = "Adresa 5", IP = "192.168.0.5" }
            ];

            modelBuilder.Entity<GMLocation>().HasData(gmLocations);

            object[] gmLocationsAct  = [
                new { Id = 1, DatumAkt = DateOnly.Parse("1.9.2024."), ResenjeAkt = "ResenjeAkt1", DatumDeakt = DateOnly.Parse("3.9.2024."), ResenjeDeakt = "ResenjeDeakt1", Napomena = "Napomena 1", GMLocationId = 1 },
                new { Id = 2, DatumAkt = DateOnly.Parse("1.9.2024."), ResenjeAkt = "ResenjeAkt2", DatumDeakt = DateOnly.Parse("3.9.2024."), ResenjeDeakt = "ResenjeDeakt2", Napomena = "Napomena 2", GMLocationId = 2 },
                new { Id = 3, DatumAkt = DateOnly.Parse("4.9.2024."), ResenjeAkt = "ResenjeAkt3", Napomena = "Napomena 3", GMLocationId = 2 }
            ];

            modelBuilder.Entity<GMLocationAct>().HasData(gmLocationsAct);
        }

        public void SeedDatabase()
        {
            return;
            if (isDatabaseSeeded) {
                return;
            }
            City[] cities = [
                new() { Id = 1, Naziv = "City 1" },
                new() { Id = 2, Naziv = "City 2" },
                new() { Id = 3, Naziv = "City 3" },
                new() { Id = 4, Naziv = "City 4" },
                new() { Id = 5, Naziv = "City 5" }
            ];

            GMLocation[] gmLocations = [
                new GMLocation { Id = 1, rul_base_id = 1, Naziv = "Lokacija 1", Adresa = "Adresa 1", Mesto = cities[0], IP = "192.168.0.1" },
                new GMLocation { Id = 2, rul_base_id = 2, Naziv = "Lokacija 2", Adresa = "Adresa 2", Mesto = cities[1], IP = "192.168.0.2" },
                new GMLocation { Id = 3, rul_base_id = 3, Naziv = "Lokacija 3", Adresa = "Adresa 3", Mesto = cities[2], IP = "192.168.0.3" },
                new GMLocation { Id = 4, rul_base_id = 4, Naziv = "Lokacija 4", Adresa = "Adresa 4", Mesto = cities[3], IP = "192.168.0.4" },
                new GMLocation { Id = 5, rul_base_id = 5, Naziv = "Lokacija 5", Adresa = "Adresa 5", Mesto = cities[4], IP = "192.168.0.5" }
            ];

            Cities.AddRange(cities);
            GMLocations.AddRange(gmLocations);

            SaveChanges();
            isDatabaseSeeded = true;
        }
    }
}
