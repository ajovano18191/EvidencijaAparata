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
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));
            _serviceProvider = services.BuildServiceProvider();
            _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            SeedDatabase(_context);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
            _serviceProvider.Dispose();
        }

        private void SeedDatabase(ApplicationDbContext dbContext)
        {
            SeedDatabaseCities(dbContext);
            SeedDatabaseGMLocations(dbContext);
            dbContext.SaveChanges();
        }

        private void SeedDatabaseCities(ApplicationDbContext dbContext)
        {
            dbContext.Cities.AddRange(
                new City { Id = 1, Naziv = "City 1" },
                new City { Id = 2, Naziv = "City 2" },
                new City { Id = 3, Naziv = "City 3" },
                new City { Id = 4, Naziv = "City 4" },
                new City { Id = 5, Naziv = "City 5" }
            );
        }

        private void SeedDatabaseGMLocations(ApplicationDbContext dbContext)
        {
            dbContext.GMLocations.AddRange(
                new GMLocation { Id = 1, rul_base_id = 1, Naziv = "Lokacija 1", Adresa = "Adresa 1", IP = "192.168.0.1" },
                new GMLocation { Id = 2, rul_base_id = 2, Naziv = "Lokacija 2", Adresa = "Adresa 2", IP = "192.168.0.2" },
                new GMLocation { Id = 3, rul_base_id = 3, Naziv = "Lokacija 3", Adresa = "Adresa 3", IP = "192.168.0.3" },
                new GMLocation { Id = 4, rul_base_id = 4, Naziv = "Lokacija 4", Adresa = "Adresa 4", IP = "192.168.0.4" },
                new GMLocation { Id = 5, rul_base_id = 5, Naziv = "Lokacija 5", Adresa = "Adresa 5", IP = "192.168.0.5" }
            );
        }
    }
}
