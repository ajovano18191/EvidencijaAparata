using EvidencijaAparata.Server.Controllers;
using EvidencijaAparata.Server.DTOs;
using EvidencijaAparata.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        [TestCase("rul_base_id", "asc", 1, 30)]
        [TestCase("naziv", "asc", 1, 30)]
        [TestCase("adresa", "asc", 1, 30)]
        [TestCase("IP", "asc", 1, 30)]
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
    }
}
