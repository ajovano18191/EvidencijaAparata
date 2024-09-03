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
        [TestCase("id", "asc", 1, 2)]
        [TestCase("id", "desc", 1, 2)]
        [TestCase("rul_base_id", "asc", 0, 30)]
        [TestCase("rul_base_id", "desc", 0, 30)]
        [TestCase("naziv", "asc", 0, 30)]
        [TestCase("naziv", "desc", 0, 30)]
        [TestCase("adresa", "asc", 0, 30)]
        [TestCase("adresa", "desc", 0, 30)]
        // [TestCase("mesto_naziv", "asc", 0, 30)]
        // [TestCase("mesto_naziv", "desc", 0, 30)]
        [TestCase("IP", "asc", 0, 30)]
        [TestCase("IP", "desc", 0, 30)]
        [TestCase(null, null, 0, 0)]
        public void GetGMLocations_Normal_GetsGMLocationsSuccessfully(string? sort, string? order, int page, int limit)
        {
            OkObjectResult httpRes = (GMLocationsController.GetGMLocations(sort, order, page, limit).Result as OkObjectResult)!;
            ReturnDTO<IGMLocation> response = (httpRes.Value as ReturnDTO<IGMLocation>)!;
            IQueryable<IGMLocation> gmLocations = response.items;
            Assert.Multiple(() => {
                Assert.That(response.count_items, Is.EqualTo(_context.GMLocations.Count()));
                Assert.That(gmLocations.Count, Is.LessThanOrEqualTo(limit));
                if(order == "asc") {
                    Assert.That(gmLocations, Is.Ordered.Ascending.By(sort));
                }
                else if(order == "desc") {
                    Assert.That(gmLocations, Is.Ordered.Descending.By(sort));
                }
            });
        }
    }
}
