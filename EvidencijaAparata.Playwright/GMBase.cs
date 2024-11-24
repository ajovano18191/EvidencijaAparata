using Microsoft.Playwright;

namespace EvidencijaAparata.Playwright
{
    [TestFixture]
    public class GMBaseTest : PageTest
    {
        private IBrowser browser;
        private IPage page;

        [SetUp]
        public async Task SetupAsync()
        {
            browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 1000,
            });

            page = await browser.NewPageAsync();
            await page.GotoAsync("https://ea.10.17.2.37.sslip.io/gm-base");
        }

        [TearDown]
        public async Task TeardownAsync()
        {
            await browser.CloseAsync();
        }

        [Test]
        [Order(0)]
        public async Task GetGMBase()
        {
            await Expect(page.Locator("circle").Nth(1)).Not.ToBeVisibleAsync();
        }

        [Test]
        [Order(1)]
        public async Task AddGMBase()
        {
            await page.GetByLabel("Add").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Name").FillAsync("Base");
            await page.GetByLabel("Serial num.").FillAsync("SerialNum");
            await page.GetByLabel("Old Sticker No.").FillAsync("OldStickerNo");
            await page.GetByLabel("Work type").Locator("svg").ClickAsync();
            await page.GetByRole(AriaRole.Option, new() { Name = "COUNTERS" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Add" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base"), Exact = true })).ToBeVisibleAsync();
        }

        [Test]
        [Order(2)]
        public async Task EditGMBase()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base") }).GetByLabel("Edit").ClickAsync();
            await Expect(page.Locator(".cdk-overlay-backdrop")).ToBeVisibleAsync();
            await page.GetByLabel("Serial num.").FillAsync("SerialNummm");
            await page.GetByLabel("Old Sticker No.").FillAsync("OldStickerNooo");
            await page.GetByLabel("Work type").Locator("svg").ClickAsync();
            await page.GetByRole(AriaRole.Option, new() { Name = "ROULETE" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Update" }).ClickAsync();
            await Expect(page.Locator("tbody")).ToContainTextAsync("SerialNummm");
            await Expect(page.Locator("tbody")).ToContainTextAsync("OldStickerNooo");
            await Expect(page.Locator("tbody")).ToContainTextAsync("ROULETE");
        }

        [Test]
        [Order(3)]
        public async Task ActivateGMBase()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base") }).GetByLabel("Deactive").ClickAsync();
            await page.GetByLabel("Resenje").FillAsync("ResenjeAkt");
            await page.GetByLabel("Datum").FillAsync("9/13/2024");
            await page.GetByLabel("Lokacija").Locator("svg").ClickAsync();
            await page.GetByRole(AriaRole.Option, new() { Name = "Lokacija 2" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Activate" }).ClickAsync();
            await Expect(page.Locator("tbody")).ToContainTextAsync("Lokacija 2");
            await Expect(page.Locator("tbody")).ToContainTextAsync("Active");
        }

        [Test]
        [Order(4)]
        public async Task DeactivateGMBase()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base") }).GetByLabel("Active").ClickAsync();
            await page.GetByLabel("Resenje").FillAsync("ResenjeDeakt");
            await page.GetByLabel("Datum").FillAsync("9/14/2024");
            await page.GetByRole(AriaRole.Button, new() { Name = "Deactivate" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base") })).Not.ToContainTextAsync("Lokacija 2");
            await Expect(page.Locator("tbody")).ToContainTextAsync("Deactive");
        }

        [Test]
        [Order(5)]
        public async Task DeleteGMBase()
        {
            await page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base") }).GetByLabel("Delete").ClickAsync();
            await Expect(page.GetByRole(AriaRole.Row, new() { NameRegex = new Regex(".* Base") })).ToHaveCountAsync(0);
        }

        [Test]
        public async Task DeactiveGMBaseOnly()
        {
            await page.GetByLabel("Deactivated only").ClickAsync();
            foreach (var row in await page.GetByRole(AriaRole.Row).AllAsync())
            {
                await Expect(row).Not.ToContainTextAsync("Active");
            }
        }
    }
}