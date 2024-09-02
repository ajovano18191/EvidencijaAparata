using EvidencijaAparata.Server.Controllers;
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
        public void GetGMLocations_Normal_GetsGMLocationsSuccessfully()
        {
            var gmLocations = GMLocationsController.GetGMLocations();
            Assert.That(gmLocations.Count, Is.EqualTo(5));
        }
    }
}
