using Microsoft.Playwright.NUnit;
using Microsoft.Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
        };
    }

    [Test]
    public async Task MyTest()
    {
        await Page.GotoAsync("http://localhost:4200/gm-location");
    }
}
