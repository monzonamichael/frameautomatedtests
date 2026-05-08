using Microsoft.Playwright;


public class TestBase
{
    protected string FrameLogin = Environment.GetEnvironmentVariable("FRAME_AUTOMATION_EMAIL")!;
    protected string FramePassword = Environment.GetEnvironmentVariable("FRAME_AUTOMATION_PASSWORD")!;
    protected IPlaywright? _playwright;
    protected IBrowser? _browser;
    protected IPage? _page;
    protected IPage? _frameHomePopup;
    protected string FrameTestURL = "qa-automation-test-1";

    protected async Task OpenFrameHome()
    {
        var _popup = _page.WaitForPopupAsync();
        await _page.Locator("a[data-slot='button'][href='https://framevr.io/home']").ClickAsync();
        _frameHomePopup = await _popup;
        await _frameHomePopup.GetByRole(AriaRole.Textbox, new () {Name = "email address"}).WaitForAsync();
    }
    protected async Task LoginToFrameHome(string a_Username, string a_Password)
    {
        await OpenFrameHome();
        await _frameHomePopup.Locator("#input-v-5").FillAsync(a_Username);
        await _frameHomePopup.Locator("#user-password").FillAsync(a_Password);
        await _frameHomePopup.GetByRole(AriaRole.Button, new () {Name = "Login / Signup"}).ClickAsync();
    }

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
    //Always do Setup - Setup fires before each test to execute
    [SetUp]
    public async Task SetUp()
    {
        _page = await _browser!.NewPageAsync(new()
        {
            //Setting viewport size is required as some layouts in Frame Home are different depending 
            // on Window Sizing, such as the logout nav drawer
            ViewportSize = new ViewportSize { Width = 1280, Height = 720 }
        });
        await _page.GotoAsync("https://learn.framevr.io/");
    }
    //Teardown fires after every test is completed
    [TearDown]
     public async Task TearDown()
    {
        await _page!.CloseAsync();
        if (_frameHomePopup != null ) await _frameHomePopup.CloseAsync();
    }
    [OneTimeTearDown]
    public async Task GlobalTearDown()
    {
        await _browser!.CloseAsync();
        _playwright!.Dispose();
    }


}