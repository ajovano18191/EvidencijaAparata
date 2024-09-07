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
            [FromQuery(Name = "act_location_id")] int? act_location_id
        )
        {
            IQueryable<GMBase> gmBases;
            if (act_location_id == null) {
                gmBases = Context.GMBases
                .Include(p => p.GMBaseActs)
                .ThenInclude(p => p.GMLocationAct)
                .ThenInclude(p => p.GMLocation);
            }
            else {
                gmBases = Context.GMLocationActs
                .FirstOrDefault(p => p.Id == act_location_id)!
                .GMBaseActs
                .Select(p => p.GMBase)
                .AsQueryable<GMBase>();
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
            return Ok(new ReturnDTO<IGMBase>(igmBases, count_items));
        }
    }
}
