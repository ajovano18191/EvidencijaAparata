using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace EvidencijaAparata.Playwright
{
    [TestFixture]
    public class GMLocationTest : PageTest
    {
        private IBrowser browser;
        private IPage page;

        [SetUp]
        public async Task SetupAsync()
        {
            browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                SlowMo = 1000,
            });

            page = await browser.NewPageAsync();
            await page.GotoAsync("http://ea.10.17.2.37.sslip.io/gm-location");
        }

        [TearDown]
        public async Task TeardownAsync()
        {
            await browser.CloseAsync();
        }

        [Test]
        [Order(0)]
        public async Task GetTest()
        {
            await Expect(page.Locator("circle").Nth(1)).Not.ToBeVisibleAsync();
        }

        [Test]
        [Order(1)]
        public async Task AddGMLocation()
        {
            await page.GetByLabel("Add", new() { Exact = true }).ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Rul. Base ID").FillAsync("100");
            await page.GetByLabel("Name").FillAsync("Proba");
            await page.GetByLabel("Address").FillAsync("Adresa");
            await page.GetByLabel("IP").FillAsync("192.168.0.100");
            await page.GetByLabel("Mesto").Locator("span").ClickAsync();
            await page.GetByRole(AriaRole.Option, new() { Name = "City 1", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Add", Exact = true }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") })).ToBeVisibleAsync();
        }

        [Test]
        [Order(2)]
        public async Task EditGMLocation()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Edit").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Name").FillAsync("Probaaa");
            await page.GetByLabel("Address").FillAsync("Adresaaa");
            await page.GetByLabel("City").Locator("span").First.ClickAsync();
            await page.GetByRole(AriaRole.Option, new() { Name = "City 2", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Update", Exact = true }).ClickAsync();
            await Expect(page.Locator("tbody")).ToContainTextAsync("Probaaa");
            await Expect(page.Locator("tbody")).ToContainTextAsync("Adresaaa");
            await Expect(page.Locator("tbody")).ToContainTextAsync("City 2");
        }

        [Test]
        [Order(3)]
        public async Task ActivateGMLocation()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Deactive").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Resenje").FillAsync("ResAkt");
            await page.GetByLabel("Datum").FillAsync("9/10/2024");
            // await page.GetByLabel("Open calendar").ClickAsync();
            // await page.GetByLabel("10. новембар").ClickAsync();
            await page.GetByLabel("Napomena").FillAsync("NapomenaAkt");
            await page.GetByRole(AriaRole.Button, new() { Name = "Activate", Exact = true }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Active")).ToHaveCountAsync(1);
        }

        [Test]
        [Order(4)]
        public async Task DeactivateGMLocation()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Active").ClickAsync();
            await page.GetByLabel("Resenje").FillAsync("ResenjeDeakt");
            await page.GetByLabel("Datum").FillAsync("9/12/2024");
            // await page.GetByLabel("Open calendar").ClickAsync();
            // await page.GetByLabel("12. новембар").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Deactivate", Exact = true }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Deactive")).ToHaveCountAsync(1);
        }

        [Test]
        [Order(5)]
        public async Task DeleteGMLocation()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Delete").ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") })).ToHaveCountAsync(0);
        }

        [Test]
        public async Task OpenActiveGMBasesForLocation()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Lokacija 2") }).GetByLabel("List").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            foreach (var row in await page.GetByRole(AriaRole.Row).AllAsync())
            {
                await Expect(row).Not.ToContainTextAsync("Deactive");
            }
        }

        [Test]
        public async Task OpenDeactiveGMBases()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Lokacija 2") }).GetByLabel("Add base").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            foreach (var row in await page.GetByRole(AriaRole.Row).AllAsync())
            {
                await Expect(row).Not.ToContainTextAsync("Active");
            }
        }
    }
}