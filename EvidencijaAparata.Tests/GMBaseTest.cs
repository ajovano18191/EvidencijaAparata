using EvidencijaAparata.Server.Controllers;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvidencijaAparata.Server.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using EvidencijaAparata.Server.Models;

namespace EvidencijaAparata.Tests
{
    [TestFixture]
    internal class GMBaseTest : GlobalSetUp
    {
        public GMBasesController GMBasesController { get; set; }

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            GMBasesController = new GMBasesController(_context);
        }

        IDbContextTransaction transaction;

        [SetUp]
        public void SetUp()
        {
            _context.ChangeTracker.Clear();
            transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
        }

        [TearDown]
        public void TearDown()
        {
            transaction.Rollback();
            transaction.Dispose();
        }

        [Test]
        [TestCase("id", "asc", 1, 30, null, false)] // All
        [TestCase("id", "asc", 1, 30, null, true)] // Only deactive
        [TestCase("id", "asc", 1, 30, 2, false)] // For location
        [TestCase("id", "asc", 1, 30, 2, true)] // For location only deactive
        public void GetGMBases_Normal_GetsGMBasesSuccessfully(string? sort, string? order, int? page, int? limit, int? location_id, bool addOrNotList)
        {
            OkObjectResult httpRes = (GMBasesController.GetGMBases(sort, order, page, limit, location_id, addOrNotList).Result as OkObjectResult)!;
            ReturnDTO<IGMBase> response = (httpRes.Value as ReturnDTO<IGMBase>)!;
            IList<IGMBase> gmBases = response.items;

            Assert.Multiple(() => {
                if (location_id == null) {
                    Assert.That(response.count_items, Is.EqualTo(_context.GMLocations.Count()));
                }
                Assert.That(gmBases.Count, Is.LessThanOrEqualTo(limit!));

                if (order == "asc") {
                    Assert.That(gmBases, Is.Ordered.Ascending.By(sort ?? "id"));
                }
                else if(order == "desc") {
                    Assert.That(gmBases, Is.Ordered.Descending.By(sort ?? "id"));
                }

                if (location_id != null) {
                    Assert.That(gmBases.Select(p => p.act_location_id), Is.All.EqualTo(location_id));
                }

                if (addOrNotList) {
                    Assert.That(gmBases.Select(p => p.act_base_id), Is.All.Null);
                }
            });
        }

        [Test]
        [TestCase("act_location_naziv", "desc", 1, 30, null, false)]
        public void GetGMBases_SortByActLocationNaziv_GetsGMBasesSuccessfully(string? sort, string? order, int? page, int? limit, int? location_id, bool addOrNotList)
        {
            OkObjectResult httpRes = (GMBasesController.GetGMBases(sort, order, page, limit, location_id, addOrNotList).Result as OkObjectResult)!;
            ReturnDTO<IGMBase> response = (httpRes.Value as ReturnDTO<IGMBase>)!;
            IList<IGMBase> gmBases = response.items;

            string nazivPrveLokacije = gmBases[0].act_location_naziv!;
            string nazivDrugeLokacije = gmBases[0].act_location_naziv!;
            Assert.That(nazivPrveLokacije, Is.LessThanOrEqualTo(nazivDrugeLokacije));
        }

        [Test]
        [TestCase("Name", "SerialNum", "OldStickerNo", "SAS")]
        public async Task AddGMBase_Normal_AddedGMBaseSuccessfully(string name, string serial_num, string old_sticker_no, string work_type)
        {
            GMBaseDTO gmBaseDTO = new GMBaseDTO(name, serial_num, old_sticker_no, work_type);
            OkObjectResult httpRes = ((await GMBasesController.AddGMBase(gmBaseDTO)).Result as OkObjectResult)!;
            IGMBase addedGMBase = (httpRes.Value as IGMBase)!;
            GMBase? foundedGMLocation = _context.GMBases.FirstOrDefault(p => p.Id == addedGMBase.id);

            Assert.That(foundedGMLocation, Is.Not.Null);
        }

        [Test]
        [TestCase(3, "Name", "SerialNum", "OldStickerNo", "SAS")]
        public async Task UpdateGMBase_Normal_UpdatedGMBaseSuccessfully(int id, string name, string serial_num, string old_sticker_no, string work_type)
        {
            GMBaseDTO gmBaseDTO = new GMBaseDTO(name, serial_num, old_sticker_no, work_type);
            OkObjectResult httpRes = ((await GMBasesController.UpdateGMBase(id, gmBaseDTO)).Result as OkObjectResult)!;
            IGMBase updatedGMBase = (httpRes.Value as IGMBase)!;
            GMBase? foundedGMBase = _context.GMBases.FirstOrDefault(p => p.Id == id);

            Assert.Multiple(() => {
                Assert.That(foundedGMBase, Is.Not.Null);
                Assert.That(foundedGMBase!.Id, Is.EqualTo(updatedGMBase.id));
                Assert.That(foundedGMBase!.Name, Is.EqualTo(name));
                Assert.That(foundedGMBase!.serial_num, Is.EqualTo(serial_num));
                Assert.That(foundedGMBase!.old_sticker_no, Is.EqualTo(old_sticker_no));
                Assert.That(foundedGMBase!.work_type, Is.EqualTo(work_type));
                Assert.That(updatedGMBase.act_base_id, Is.Not.Null);
                Assert.That(updatedGMBase.act_location_id, Is.Not.Null);
            });
        }

        [Test]
        [TestCase(-1, "Name", "SerialNum", "OldStickerNo", "SAS")]
        public async Task UpdateGMBase_WrongId_ThrowsException(int id, string name, string serial_num, string old_sticker_no, string work_type)
        {
            GMBaseDTO gmBaseDTO = new GMBaseDTO(name, serial_num, old_sticker_no, work_type);
            Assert.That(async () => await GMBasesController.UpdateGMBase(id, gmBaseDTO), Throws.Exception);
        }

        [Test]
        [TestCase(1)]
        public async Task DeleteGMBase_Normal_DeletedGMBaseSuccessfully(int id)
        {
            OkResult? httpRes = (await GMBasesController.DeleteGMBase(id)) as OkResult;
            Assert.Multiple(() => {
                Assert.That(httpRes, Is.Not.Null);
                int numGMBases = _context.GMBases.Count(p => p.Id == id);
                Assert.That(numGMBases, Is.EqualTo(0));
                Assert.That(_context.GMBaseActs.Include(p => p.GMBase).Where(p => p.GMBase.Id == id).Count(), Is.EqualTo(0));
            });
        }

        [Test]
        [TestCase(-1)]
        public async Task DeleteGMBase_WrongId_ThrowsException(int id)
        {
            Assert.That(async () => await GMBasesController.DeleteGMBase(id), Throws.Exception);
        }

        [Test]
        [TestCase(1, "9.9.2024.", "Resenje", 2)]
        public async Task ActivateGMBase_Normal_ActivatedGMBaseSuccessfully(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            OkResult? httpRes = (await GMBasesController.ActivateGMBase(id, gmBaseActDTO)) as OkResult;

            await Assert.MultipleAsync(async () => {
                Assert.That(httpRes, Is.Not.Null);

                GMBase gmBase = (await _context.GMBases
                    .Include(p => p.GMBaseActs)
                    .ThenInclude(p => p.GMLocationAct)
                    .ThenInclude(p => p.GMLocation)
                    .FirstOrDefaultAsync(p => p.Id == id)
                )!;
                Assert.That(gmBase.GetBaseAct(), Is.Not.Null);

                GMBaseAct gmBaseAct = gmBase.GMBaseActs.MaxBy(p => p.DatumAkt)!;
                Assert.That(gmBaseAct.DatumAkt, Is.EqualTo(DateOnly.Parse(datum)));
                Assert.That(gmBaseAct.DatumDeakt, Is.Null);
            });
        }

        [Test]
        [TestCase(3, "2024-09-09", "Resenje", 2)]
        public void ActivateGMBase_ActiveBase_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.ActivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(5, "2024-09-02", "Resenje", 2)]
        public void ActivateGMBase_ActivationBeforeDeactivation_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.ActivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(-1, "2024-09-09", "Resenje", 2)]
        public void ActivateGMBase_NonExistingBase_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.ActivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(1, "2024-09-09", "Resenje", 1)]
        public void ActivateGMBase_NonActiveLocation_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.ActivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(1, "2024-09-09", "Resenje", -1)]
        public void ActivateGMBase_NonExistingLocation_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.ActivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(3, "2024-09-09", "Resenje", 2)]
        public async Task DeactivateGMBase_Normal_DeactivatedGMBaseSuccessfully(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            OkResult? httpRes = (await GMBasesController.DeactivateGMBase(id, gmBaseActDTO)) as OkResult;

            await Assert.MultipleAsync(async () => {
                Assert.That(httpRes, Is.Not.Null);

                GMBase gmBase = (await _context.GMBases
                    .Include(p => p.GMBaseActs)
                    .ThenInclude(p => p.GMLocationAct)
                    .ThenInclude(p => p.GMLocation)
                    .FirstOrDefaultAsync(p => p.Id == id)
                )!;
                Assert.That(gmBase.GetBaseAct(), Is.Null);

                GMBaseAct gmBaseAct = gmBase.GMBaseActs.MaxBy(p => p.DatumAkt)!;
                Assert.That(gmBaseAct.DatumDeakt, Is.EqualTo(DateOnly.Parse(datum)));
            });
        }

        [Test]
        [TestCase(-1, "2024-09-09", "Resenje", -1)]
        public async Task DeactivateGMBase_NonExistingBase_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.DeactivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(5, "2024-09-09", "Resenje", -1)]
        public async Task DeactivateGMBase_DeactiveBase_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.DeactivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }

        [Test]
        [TestCase(3, "2024-09-06", "Resenje", -1)]
        public async Task DeactivateGMBase_DeactivationBeforeActivation_ThrowsException(int id, string datum, string resenje, int location_id)
        {
            GMBaseActDTO gmBaseActDTO = new GMBaseActDTO(DateTime.Parse(datum), resenje, location_id);
            Assert.That(async () => await GMBasesController.DeactivateGMBase(id, gmBaseActDTO), Throws.Exception);
        }
    }
}
