using EvidencijaAparata.Server.Data;
using EvidencijaAparata.Server.DTOs;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvidencijaAparata.Server.Controllers
{
    [ApiController]
    [Route("gm_location")]
    public class GMLocationsController : ControllerBase
    {
        public ApplicationDbContext Context { get; set; }

        public GMLocationsController(ApplicationDbContext dbContext)
        {
            Context = dbContext;
            dbContext.SeedDatabase();
        }

        [HttpGet]
        public ActionResult<ReturnDTO<IGMLocation>> GetGMLocations(
            [FromQuery(Name = "_sort")] string? sort, 
            [FromQuery(Name = "_order")] string? order,
            [FromQuery(Name = "_page")] int page,
            [FromQuery(Name = "_limit")] int limit)
        {
            IQueryable<GMLocation> gmLocations = Context.GMLocations
                .Include(p => p.Mesto);
            int count_items = gmLocations.Count();

            switch(sort) {
                case "id":
                    gmLocations = gmLocations.OrderBy(p => p.Id);
                    break;
                case "rul_base_id":
                    gmLocations = gmLocations.OrderBy(p => p.rul_base_id);
                    break;
                case "naziv":
                    gmLocations = gmLocations.OrderBy(p => p.Naziv);
                    break;
                case "adresa":
                    gmLocations = gmLocations.OrderBy(p => p.Adresa);
                    break;
                case "mesto_naziv":
                    gmLocations = gmLocations.OrderBy(p => p.Mesto.Naziv);
                    break;
                case "IP":
                    gmLocations = gmLocations.OrderBy(p => p.IP);
                    break;
            }

            if (order == "desc") {
                gmLocations = gmLocations.Reverse();
            }
            gmLocations = gmLocations.Skip((page - 1) * limit).Take(limit);

            IQueryable<IGMLocation> igmLocations = gmLocations.Select(p => new IGMLocation(p));
            return Ok(new ReturnDTO<IGMLocation>(igmLocations, count_items));
        }

        [HttpPost]
        public async Task<ActionResult<IGMLocation>> AddGMLocation([FromBody] GMLocationDTO gmLocationDTO)
        {
            City city = await Context.Cities.FirstOrDefaultAsync(p => p.Id == gmLocationDTO.mesto_id) ?? throw new Exception();
            GMLocation gmLocation = new GMLocation();
            gmLocation.DTO2GMLocation(gmLocationDTO, city!);
            await Context.GMLocations.AddAsync(gmLocation);
            await Context.SaveChangesAsync();
            return Ok(new IGMLocation(gmLocation));
        }

        [HttpPut]
        public async Task<ActionResult<IGMLocation>> UpdateGMLocation([FromRoute] int id, [FromBody] GMLocationDTO gmLocationDTO)
        {
            GMLocation gmLocation = await Context.GMLocations.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception();
            City city = await Context.Cities.FirstOrDefaultAsync(p => p.Id == gmLocationDTO.mesto_id) ?? throw new Exception();
            gmLocation.DTO2GMLocation(gmLocationDTO, city);
            Context.GMLocations.Update(gmLocation);
            await Context.SaveChangesAsync();
            return Ok(new IGMLocation(gmLocation));
        }
    }
}
