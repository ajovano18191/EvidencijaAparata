using EvidencijaAparata.Server.Data;
using EvidencijaAparata.Server.DTOs;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvidencijaAparata.Server.Controllers
{
    [ApiController]
    [Route("gm_base")]
    public class GMBaseController : ControllerBase
    {
        public ApplicationDbContext Context { get; set; }

        public GMBaseController(ApplicationDbContext dbContext)
        {
            Context = dbContext;
            dbContext.SeedDatabase();
        }

        [HttpGet]
        public ActionResult<ReturnDTO<IGMBase>> GetGMBases(
            [FromQuery(Name = "_sort")] string? sort,
            [FromQuery(Name = "_order")] string? order,
            [FromQuery(Name = "_page")] int? page,
            [FromQuery(Name = "_limit")] int? limit,
            [FromQuery(Name = "location_id")] int? location_id,
            [FromQuery(Name = "addOrNotList")] bool addOrNotList = false
        )
        {
            IQueryable<GMBase> gmBases;
            if (location_id == null) {
                gmBases = Context.GMBases
                .Include(p => p.GMBaseActs)
                .ThenInclude(p => p.GMLocationAct)
                .ThenInclude(p => p.GMLocation);
            }
            else {
                gmBases = Context.GMLocations
                .Include(p => p.GMLocationActs)
                .ThenInclude(p => p.GMBaseActs)
                .ThenInclude(p => p.GMBase)
                .FirstOrDefault(p => p.Id == location_id)!
                .GetLocationAct()!
                .GMBaseActs
                .Where(p => p.DatumDeakt == null)
                .Select(p => p.GMBase)
                .AsQueryable();
            }
            int count_items = gmBases.Count();
            limit = page == null ? count_items : limit;
            page ??= 1;

            switch(sort) {
                case "name":
                    gmBases = gmBases.OrderBy(p => p.Name);
                    break;
                case "serial_num":
                    gmBases = gmBases.OrderBy(p => p.serial_num);
                    break;
                case "old_sticker_no":
                    gmBases = gmBases.OrderBy(p => p.old_sticker_no);
                    break;
                case "work_type":
                    gmBases = gmBases.OrderBy(p => p.work_type);
                    break;
                case "act_location_naziv":
                    gmBases = gmBases.OrderBy(p => p.GetBaseAct() == null ? "" : p.GetBaseAct()!.GMLocationAct.GMLocation.Naziv);
                    break;
                default:
                    gmBases = gmBases.OrderBy(p => p.Id);
                    break;
            }

            if(order == "desc") {
                gmBases = gmBases.Reverse();
            }
            gmBases = gmBases.Skip(((page - 1) * limit) ?? 0).Take(limit ?? 0);

            IList<IGMBase> igmBases = gmBases.Select(p => new IGMBase(p)).ToList();
            if (addOrNotList) {
                igmBases = igmBases.Where(p => p.act_base_id == null).ToList();
            }
            return Ok(new ReturnDTO<IGMBase>(igmBases, count_items));
        }

        [HttpPost]
        public async Task<ActionResult> AddGMBase([FromBody] GMBaseDTO gmBaseDTO)
        {
            GMBase gmBase = new GMBase();
            gmBase.DTO2GMBase(gmBaseDTO);
            await Context.GMBases.AddAsync(gmBase);
            await Context.SaveChangesAsync();
            return Ok(new IGMBase(gmBase));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateGMBase([FromRoute] int id, [FromBody] GMBaseDTO gmBaseDTO)
        {
            GMBase gmBase = (await Context.GMBases.FirstOrDefaultAsync(p => p.Id == id))!;
            gmBase.DTO2GMBase(gmBaseDTO);
            Context.GMBases.Update(gmBase);
            await Context.SaveChangesAsync();
            return Ok(new IGMBase(gmBase));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteGMBase([FromRoute] int id)
        {
            GMBase gmBase = (await Context.GMBases.FirstOrDefaultAsync(p => p.Id == id))!;
            Context.GMBases.Remove(gmBase);
            await Context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{id}/activate")]
        public async Task<ActionResult> ActivateGMBase([FromRoute] int id, [FromBody] GMBaseActDTO gmBaseActDTO)
        {
            GMBase gmBase = (await Context.GMBases.Include(p => p.GMBaseActs).FirstOrDefaultAsync(p => p.Id == id))!;
            GMLocationAct gmLocationAct = (await Context.GMLocations
                .Include(p => p.GMLocationActs)
                .FirstOrDefaultAsync(p => p.Id == gmBaseActDTO.location_id))!
                .GetLocationAct()!;
            GMBaseAct gmBaseAct = new GMBaseAct {
                DatumAkt = DateOnly.FromDateTime(gmBaseActDTO.datum),
                ResenjeAkt = gmBaseActDTO.resenje,
                GMBase = gmBase,
                GMLocationAct = gmLocationAct,
            };
            await Context.GMBaseActs.AddAsync(gmBaseAct);
            await Context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("{id}/deactivate")]
        public async Task<ActionResult> DeactivateGMBase([FromRoute] int id, [FromBody] GMBaseActDTO gmBaseActDTO)
        {
            GMBase gmBase = (await Context.GMBases.Include(p => p.GMBaseActs).FirstOrDefaultAsync(p => p.Id == id))!;
            GMBaseAct gmBaseAct = gmBase.GetBaseAct()!;
            gmBaseAct.DatumDeakt = DateOnly.FromDateTime(gmBaseActDTO.datum);
            gmBaseAct.ResenjeDeakt = gmBaseActDTO.resenje;
            Context.GMBaseActs.Update(gmBaseAct);
            await Context.SaveChangesAsync();
            return Ok();
        }
    }
}
