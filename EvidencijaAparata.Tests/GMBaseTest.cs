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

    }
}
