using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Microsoft.VisualBasic;

[TestFixture]
public class LoginTests
{
    string FrameLogin = Environment.GetEnvironmentVariable("FRAME_AUTOMATION_EMAIL")!;
    string FramePassword = Environment.GetEnvironmentVariable("FRAME_AUTOMATION_PASSWORD")!;


    //This section and OneTimeSetup for debugging only, not best practice
    private IPlaywright? _playwright;
    private IBrowser? _browser;    
    private IPage? _page;
    private IPage _frameHomePopup;

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


    private async Task OpenFrameHome()
    {
        var _popup = _page.WaitForPopupAsync();
        await _page.Locator("a[data-slot='button'][href='https://framevr.io/home']").ClickAsync();
        _frameHomePopup = await _popup;
        await _frameHomePopup.GetByRole(AriaRole.Textbox, new () {Name = "email address"}).WaitForAsync();
    }

    private async Task LoginToFrameHome(string a_Username, string a_Password)
    {
        await OpenFrameHome();
        await _frameHomePopup.Locator("#input-v-5").FillAsync(a_Username);
        await _frameHomePopup.Locator("#user-password").FillAsync(a_Password);
        await _frameHomePopup.GetByRole(AriaRole.Button, new () {Name = "Login / Signup"}).ClickAsync();
    }

    //Always do Setup - Setup fires before each test to execute
    [SetUp]
    public async Task SetUp()
    {
        _page = await _browser!.NewPageAsync();
        Console.WriteLine("This Setup runs per task");
        await _page.GotoAsync("https://learn.framevr.io/");
    }
    
    [Test]
    public async Task LoginAuthSuccess()
    {
        await LoginToFrameHome(FrameLogin, FramePassword);
        await Expect(_frameHomePopup.GetByText("michael+automation@framevr.io")).ToBeVisibleAsync();
        Console.WriteLine("Finished checking positive case");
        await _page!.WaitForTimeoutAsync(3000);
    }
    [Test]
    public async Task LoginBadCredential()
    {
        await LoginToFrameHome(FrameLogin, "BadPassword");
        await Expect(_frameHomePopup.GetByText("Incorrect password. Please try again.")).ToBeVisibleAsync();
    }

    // [Test]
    // public async Task LogOutSuccess()
    // {
    //     await Login();
    //     await _page.Locator("a[href='/logout']").ClickAsync();
    //     await _page.WaitForURLAsync("**/login");
    //     await Expect(_page.GetByText("Signup / Login")).ToBeVisibleAsync();
    //     Console.WriteLine("End logout test");
    // }

    //Teardown fires after every test is completed
    [TearDown]
     public async Task TearDown()
    {
        await _page!.CloseAsync();
        await _frameHomePopup.CloseAsync();
    }

    [OneTimeTearDown]
    public async Task GlobalTearDown()
    {
        await _browser!.CloseAsync();
        _playwright!.Dispose();
    }
}