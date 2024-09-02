using EvidencijaAparata.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvidencijaAparata.Server.Models;

namespace EvidencijaAparata.Tests
{
    [TestFixture]
    internal class GlobalSetUp
    {
        protected ServiceProvider _serviceProvider;
        protected ApplicationDbContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            SeedDatabase(_context);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Dispose();
            _serviceProvider.Dispose();
        }

        private void SeedDatabase(ApplicationDbContext dbContext)
        {
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

            dbContext.Cities.AddRange(cities);
            dbContext.GMLocations.AddRange(gmLocations);

            dbContext.SaveChanges();
        }
    }
}
