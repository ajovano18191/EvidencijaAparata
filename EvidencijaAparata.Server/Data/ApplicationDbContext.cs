using EvidencijaAparata.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EvidencijaAparata.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<GMLocation> GMLocations { get; set; }
    }
}
