using EvidencijaAparata.Server.Controllers;
using EvidencijaAparata.Server.DTOs;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [Test]
        [TestCase("id", "asc", 1, 30)]
        [TestCase("rul_base_id", "desc", 1, 30)]
        [TestCase("naziv", "desc", 1, 30)]
        [TestCase("adresa", "desc", 1, 30)]
        [TestCase("IP", "desc", 1, 30)]
        [TestCase("id", "desc", 1, 30)]
        [TestCase(null, null, 0, 0)]
        [TestCase("id", "asc", 5, 2)]
        [TestCase("id", "asc", 0, 2)]
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
            GMLocationDTO gmLocationDTO = new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.17", 1);
            OkObjectResult httpRes = ((await GMLocationsController.AddGMLocation(gmLocationDTO)).Result as OkObjectResult)!;
            IGMLocation addedGMLocation = (httpRes.Value as IGMLocation)!;
            GMLocation? foundedGMLocation = _context.GMLocations.FirstOrDefault(p => p.Id == addedGMLocation.id);
            
            Assert.That(foundedGMLocation, Is.Not.Null);

            // _context.GMLocations.Remove(foundedGMLocation);
            // await _context.SaveChangesAsync();
        }

        private static GMLocationDTO[] invalidInputs = {
            new GMLocationDTO(1, "", "Adresa", "192.168.0.1", 1),
            new GMLocationDTO(1, "Naziv", "", "192.168.0.1", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.1.", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.256", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", ".192.168.0.1", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192,168,0,1", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192..168.0.1", 1),
            new GMLocationDTO(1, "Naziv", "Adresa", "192.168.0.1", -1),
        };

        [Test]
        [TestCaseSource(nameof(invalidInputs))]
        public void AddGMLocation_InvalidInput_ThrowsException(GMLocationDTO gmLocationDTO)
        {
            Assert.That(async () => await GMLocationsController.AddGMLocation(gmLocationDTO), Throws.Exception);
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
