using Microsoft.Playwright;


public class TestBase
{
    protected IPlaywright? _playwright;
    protected IBrowser? _browser;

    [OneTimeSetUp]
    public async Task GlobalSetUp()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false,
            SlowMo = 50
        });
    }

    [OneTimeTearDown]
    public async Task GlobalTearDown()
    {
        await _browser!.CloseAsync();
        _playwright!.Dispose();
    }
}