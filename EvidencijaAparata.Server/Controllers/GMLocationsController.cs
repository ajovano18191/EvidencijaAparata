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
            [FromQuery(Name = "_page")] int? page,
            [FromQuery(Name = "_limit")] int? limit,
            [FromQuery(Name = "act_location_id_ne")] string? act_location_only
            )
        {
            Console.WriteLine(act_location_only);
            IQueryable<GMLocation> gmLocations = Context.GMLocations
                .Include(p => p.GMLocationActs)
                .Include(p => p.Mesto);
            int count_items = gmLocations.Count();
            limit = page == null ? count_items : limit;
            page ??= 1;

            switch(sort) {
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
                case "ip":
                    gmLocations = gmLocations.OrderBy(p => p.IP);
                    break;
                default:
                    gmLocations = gmLocations.OrderBy(p => p.Id);
                    break;
            }

            if (order == "desc") {
                gmLocations = gmLocations.Reverse();
            }
            gmLocations = gmLocations.Skip(((page - 1) * limit) ?? 0).Take(limit ?? 0);

            IList<IGMLocation> igmLocations = gmLocations.Select(p => new IGMLocation(p)).ToList();
            if (HttpContext?.Request.QueryString.ToString().Contains("act_location_id_ne") ?? act_location_only == "yes") {
                igmLocations = igmLocations.Where(p => p.act_location_id != null).ToList();
            }
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
        [Route("{id}")]
        public async Task<ActionResult<IGMLocation>> UpdateGMLocation([FromRoute] int id, [FromBody] GMLocationDTO gmLocationDTO)
        {
            GMLocation gmLocation = await Context.GMLocations.Include(p => p.Mesto).FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception();
            City city = await Context.Cities.FirstOrDefaultAsync(p => p.Id == gmLocationDTO.mesto_id) ?? throw new Exception();
            gmLocation.DTO2GMLocation(gmLocationDTO, city);
            Context.GMLocations.Update(gmLocation);
            await Context.SaveChangesAsync();
            return Ok(new IGMLocation(gmLocation));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteGMLocation([FromRoute] int id)
        {
            GMLocation gmLocation = await Context.GMLocations.FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception();
            Context.GMLocations.Remove(gmLocation);
            await Context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<ActionResult> ActivateGMLocation([FromRoute] int id, [FromBody] GMLocationActDTO gmLocationActDTO)
        {
            GMLocation gmLocation = (await Context.GMLocations.Include(p => p.GMLocationActs).FirstOrDefaultAsync(p => p.Id == id)) ?? throw new Exception();
            GMLocationAct? lastGMLocationAct = gmLocation.GMLocationActs.MaxBy(p => p.DatumAkt);
            if(lastGMLocationAct != null) {
                if(lastGMLocationAct.DatumDeakt == null) {
                    throw new Exception();
                }
                if(lastGMLocationAct.DatumDeakt > DateOnly.FromDateTime(gmLocationActDTO.datum)) {
                    throw new Exception();
                }
            }

            GMLocationAct gmLocationAct = new GMLocationAct {
                DatumAkt = DateOnly.FromDateTime(gmLocationActDTO.datum),
                ResenjeAkt = gmLocationActDTO.resenje,
                Napomena = gmLocationActDTO.napomena,
                GMLocation = gmLocation,
            };
            await Context.GMLocationActs.AddAsync(gmLocationAct);
            await Context.SaveChangesAsync(); 
            return Ok();
        }

        [HttpPut]
        [Route("{id}/deactivate")]
        public async Task<ActionResult> DeactivateGMLocation([FromRoute] int id, [FromBody] GMLocationActDTO gmLocationActDTO)
        {
            GMLocation gmLocation = (await Context.GMLocations.Include(p => p.GMLocationActs).FirstOrDefaultAsync(p => p.Id == id)) ?? throw new Exception();
            GMLocationAct lastGMLocationAct = gmLocation.GMLocationActs.MaxBy(p => p.DatumAkt) ?? throw new Exception();
            if (lastGMLocationAct.DatumDeakt != null) {
                throw new Exception();
            }
            if (lastGMLocationAct.DatumAkt > DateOnly.FromDateTime(gmLocationActDTO.datum)) {
                throw new Exception();
            }

            lastGMLocationAct.DatumDeakt = DateOnly.FromDateTime(gmLocationActDTO.datum);
            lastGMLocationAct.ResenjeDeakt = gmLocationActDTO.resenje;
            lastGMLocationAct.Napomena = gmLocationActDTO.napomena;

            Context.Update(lastGMLocationAct);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
