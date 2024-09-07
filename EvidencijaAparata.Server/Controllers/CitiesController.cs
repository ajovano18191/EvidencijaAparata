using EvidencijaAparata.Server.Data;
using EvidencijaAparata.Server.DTOs;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace EvidencijaAparata.Server.Controllers
{
    [ApiController]
    [Route("cities")]
    public class CitiesController : ControllerBase
    {
        public ApplicationDbContext Context { get; set; }

        public CitiesController(ApplicationDbContext dbContext)
        {
            Context = dbContext;
            dbContext.SeedDatabase();
        }

        [HttpGet]
        public ActionResult<IQueryable<ICity>> GetCities()
        {
            IQueryable<ICity> cities = Context.Cities.OrderBy(p => p.Naziv).Select(p => new ICity(p.Id, p.Naziv));
            return Ok(cities);
        }
    }
}
