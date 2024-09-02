using EvidencijaAparata.Server.Data;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvidencijaAparata.Server.Controllers
{
    [ApiController]
    [Route("gm_location")]
    public class GMLocationsController(ApplicationDbContext dbContext) : ControllerBase
    {
        public ApplicationDbContext Context { get; set; } = dbContext;

        [HttpGet]
        public IEnumerable<GMLocation> GetGMLocations()
        {
            return [.. Context.GMLocations.Include(p => p.Mesto)];
        }
    }
}
