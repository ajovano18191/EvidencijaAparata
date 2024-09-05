using EvidencijaAparata.Server.Controllers;
using EvidencijaAparata.Server.DTOs;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidencijaAparata.Tests
{
    [TestFixture]
    internal class GMLocationTest : GlobalSetUp
    {
        public GMLocationsController GMLocationsController { get; set; }

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            GMLocationsController = new GMLocationsController(_context);
        }

        [SetUp]
        public void SetUp()
        {
            _context.ChangeTracker.Clear();
        }

        [Test]
        [TestCase("id", "asc", 1, 30)]
        [TestCase("rul_base_id", "desc", 1, 30)]
        [TestCase("naziv", "desc", 1, 30)]
        [TestCase("adresa", "desc", 1, 30)]
        [TestCase("IP", "desc", 1, 30)]
        [TestCase("id", "desc", 1, 30)]
        [TestCase(null, null, 0, 0)]
        [TestCase("id", "asc", 5, 2)]
        [TestCase("id", "asc", 1, 2)]
        public void GetGMLocations_Normal_GetsGMLocationsSuccessfully(string? sort, string? order, int page, int limit)
        {
            OkObjectResult httpRes = (GMLocationsController.GetGMLocations(sort, order, page, limit).Result as OkObjectResult)!;
            ReturnDTO<IGMLocation> response = (httpRes.Value as ReturnDTO<IGMLocation>)!;
            IQueryable<IGMLocation> gmLocations = response.items;
            
            Assert.Multiple(() => {
                Assert.That(response.count_items, Is.EqualTo(_context.GMLocations.Count()));
                Assert.That(gmLocations.Count, Is.LessThanOrEqualTo(limit));
                if (order == "asc") {
                    Assert.That(gmLocations, Is.Ordered.Ascending.By(sort));
                }
                else if (order == "desc") {
                    Assert.That(gmLocations, Is.Ordered.Descending.By(sort));
                }
            });
        }

        [Test]
        [TestCase("mesto_naziv", "desc", 1, 30)]
        public void GetGMLocations_SortByMestoNaziv_GetsGMLocationsSuccessfully(string? sort, string? order, int page, int limit)
        {
            OkObjectResult httpRes = (GMLocationsController.GetGMLocations(sort, order, page, limit).Result as OkObjectResult)!;
            ReturnDTO<IGMLocation> response = (httpRes.Value as ReturnDTO<IGMLocation>)!;
            IQueryable<IGMLocation> gmLocations = response.items;
            
            Assert.Multiple(() => {
                Assert.That(response.count_items, Is.EqualTo(_context.GMLocations.Count()));
                Assert.That(gmLocations.Count, Is.LessThanOrEqualTo(limit));
                string nazivPrvogMesta = gmLocations.First().mesto.naziv;
                string nazivDrugogMesta = gmLocations.Skip(1).First().mesto.naziv;
                if (order == "asc") {
                    Assert.That(nazivPrvogMesta, Is.LessThanOrEqualTo(nazivDrugogMesta));
                }
                else if (order == "desc") {
                    Assert.That(nazivPrvogMesta, Is.GreaterThanOrEqualTo(nazivDrugogMesta));
                }
            });
        }

        [Test]
        [TestCase()]
        public async Task AddGMLocation_Normal_AddedGMLocationSuccessfully()
        {            
            GMLocationDTO gmLocationDTO = new GMLocationDTO(1, "AKO", "A", "192.168.0.1", 1);
            OkObjectResult httpRes = ((await GMLocationsController.AddGMLocation(gmLocationDTO)).Result as OkObjectResult)!;
            IGMLocation addedGMLocation = (httpRes.Value as IGMLocation)!;
            GMLocation? foundedGMLocation = _context.GMLocations.FirstOrDefault(p => p.Id == addedGMLocation.id);
            
            Assert.That(foundedGMLocation, Is.Not.Null);
        }

        private static GMLocationDTO[] invalidInputs = {
            new GMLocationDTO(1, null!, "Adresa", "192.168.0.1", 1),
            new GMLocationDTO(1, "Naziv", null!, "192.168.0.1", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", null!, 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.1", -1),
        };

        [Test]
        [TestCaseSource(nameof(invalidInputs))]
        public async Task AddGMLocation_InvalidInput_ThrowsException(GMLocationDTO gmLocationDTO)
        {
            Assert.That(async () => await GMLocationsController.AddGMLocation(gmLocationDTO), Throws.Exception);
        }

        [Test]
        [TestCase(1)]
        public async Task UpdateGMLocation_Normal_UpdatedGMLocationSuccessfully(int id)
        {
            GMLocationDTO gmLocationDTO = new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.1", 1);
            OkObjectResult httpRes = ((await GMLocationsController.UpdateGMLocation(id, gmLocationDTO)).Result as OkObjectResult)!;
            IGMLocation addedGMLocation = (httpRes.Value as IGMLocation)!;
            GMLocation? foundedGMLocation = _context.GMLocations.FirstOrDefault(p => p.Id == addedGMLocation.id);

            Assert.Multiple(() => {
                Assert.That(foundedGMLocation, Is.Not.Null);
                Assert.That(foundedGMLocation?.Id, Is.EqualTo(id));
                Assert.That(foundedGMLocation?.rul_base_id, Is.EqualTo(gmLocationDTO.rul_base_id));
                Assert.That(foundedGMLocation?.Naziv, Is.EqualTo(gmLocationDTO.naziv));
                Assert.That(foundedGMLocation?.Adresa, Is.EqualTo(gmLocationDTO.adresa));
                Assert.That(foundedGMLocation?.IP, Is.EqualTo(gmLocationDTO.IP));
                Assert.That(foundedGMLocation?.Mesto.Id, Is.EqualTo(gmLocationDTO.mesto_id));
            });
        }

        [Test]
        [TestCaseSource(nameof(invalidInputs))]
        public async Task UpdateGMLocation_InvalidInput_ThrowsException(GMLocationDTO gmLocationDTO)
        {
            int id = 1;
            Assert.That(async () => await GMLocationsController.UpdateGMLocation(id, gmLocationDTO), Throws.Exception);
        }

        [Test]
        [TestCase(-1)]
        public async Task UpdateGMLocation_WrongId_ThrowsException(int id)
        {
            GMLocationDTO gmLocationDTO = new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.1", 1);
            Assert.That(async () => await GMLocationsController.UpdateGMLocation(id, gmLocationDTO), Throws.Exception);
        }

        [Test]
        [TestCase(3)]
        public async Task DeleteGMLocation_Normal_DeletedGMLocationSuccessfully(int id)
        {
            int cityId = (await _context.GMLocations.Include(p => p.Mesto).FirstOrDefaultAsync(p => p.Id == id))!.Mesto.Id;
            OkResult? httpRes = (await GMLocationsController.DeleteGMLocation(id)) as OkResult;
            Assert.Multiple(() => {
                Assert.That(httpRes, Is.Not.Null);
                int numGMLocations = _context.GMLocations.Count(p => p.Id == id);
                Assert.That(numGMLocations, Is.EqualTo(0));
                int numCities = _context.Cities.Count(p => p.Id == cityId);
                Assert.That(numCities, Is.EqualTo(1));
            });
        }

        [Test]
        [TestCase(-1)]
        public async Task DeleteGMLocation_WrongId_ThrowsException(int id)
        {
            Assert.That(async () => await GMLocationsController.DeleteGMLocation(id), Throws.Exception);
        }

        //[Test]
        //public void GetActiveGMLocations_Normal_GetsActiveGMLocationsSuccessfully()
        //{
        //    OkObjectResult httpRes = (GMLocationsController.GetActiveGMLocations().Result as OkObjectResult)!;
        //    IQueryable<IGMLocation> activeGMLocations = (httpRes.Value as IQueryable<IGMLocation>)!;

        //    Assert.That(activeGMLocations.Select(p => p.act_location_id), Is.All.Not.Null);
        //}
    }
}
