using EvidencijaAparata.Server.Controllers;
using EvidencijaAparata.Server.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvidencijaAparata.Tests
{
    [TestFixture]
    internal class CityTest : GlobalSetUp
    {
        public CitiesController CitiesController { get; set; }

        [OneTimeSetUp]
        public new void OneTimeSetUp()
        {
            CitiesController = new CitiesController(_context);
        }

        [SetUp]
        public void SetUp()
        {
            _context.ChangeTracker.Clear();
        }

        [Test]
        public void GetCities_Normal_GetsCitiesSuccessfully()
        {
            OkObjectResult httpRes = (CitiesController.GetCities().Result as OkObjectResult)!;
            IQueryable<ICity> cities = (httpRes.Value as IQueryable<ICity>)!;
            Assert.That(cities, Is.Not.Empty);
        }
    }
}
