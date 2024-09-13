using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

namespace EvidencijaAparata.Playwright
{
    // [Parallelizable(ParallelScope.Self)]
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
                Headless = false,
                SlowMo = 2000,

            });

            page = await browser.NewPageAsync();
        }

        [Test]
        [Order(0)]
        public async Task GetTest()
        {
            await page.GotoAsync("http://localhost:4200");
            // await Expect(Page.Locator("circle").Nth(1)).Not.ToBeVisibleAsync();
            // await browser.CloseAsync();
        }

        [Test]
        [Order(1)]
        public async Task AddGMLocation()
        {
            await page.GotoAsync("http://localhost:4200");
            await page.GetByLabel("Add", new() { Exact = true }).ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Rul. Base ID").FillAsync("100");
            await page.GetByLabel("Name").FillAsync("Proba");
            await page.GetByLabel("Address").FillAsync("Adresa");
            await page.GetByLabel("IP").FillAsync("192.168.0.100");
            await page.GetByLabel("Mesto").Locator("span").ClickAsync();
            await page.GetByRole(AriaRole.Option, new() { Name = "City 1", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Add", Exact = true }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Cell, new() { Name = "100", Exact = true })).ToBeVisibleAsync(); // Exact = true
            await browser.CloseAsync();
        }

        [Test]
        [Order(2)]
        public async Task EditGMLocation()
        {
            await page.GotoAsync("http://localhost:4200");
            Thread.Sleep(2000);
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
            await browser.CloseAsync();
        }

        [Test]
        [Order(3)]
        public async Task ActivateGMLocation()
        {
            await page.GotoAsync("http://localhost:4200");
            Thread.Sleep(2000);
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Deactive").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Resenje").FillAsync("ResAkt");
            await page.GetByLabel("Open calendar").ClickAsync();
            await page.GetByLabel("10. септембар").ClickAsync();
            await page.GetByLabel("Napomena").FillAsync("NapomenaAkt");
            await page.GetByRole(AriaRole.Button, new() { Name = "Activate", Exact = true }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Active")).ToHaveCountAsync(1);
            await browser.CloseAsync();
        }

        [Test]
        [Order(4)]
        public async Task DeactivateGMLocation()
        {
            await page.GotoAsync("http://localhost:4200");
            Thread.Sleep(2000);
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Active").ClickAsync();
            await page.GetByLabel("Resenje").FillAsync("ResenjeDeakt");
            await page.GetByLabel("Open calendar").ClickAsync();
            await page.GetByLabel("12. септембар").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Deactivate", Exact = true }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Deactive")).ToHaveCountAsync(1);
            await browser.CloseAsync();
        }

        [Test]
        [Order(5)]
        public async Task DeleteGMLocation()
        {
            await page.GotoAsync("http://localhost:4200");
            Thread.Sleep(2000);
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") }).GetByLabel("Delete").ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* 100") })).ToHaveCountAsync(0);
            await browser.CloseAsync();
        }
    }
}