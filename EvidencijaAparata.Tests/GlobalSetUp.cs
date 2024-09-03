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
            _context.SeedDatabase();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Dispose();
            _serviceProvider.Dispose();
        }
    }
}
