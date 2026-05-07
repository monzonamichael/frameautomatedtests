using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Microsoft.VisualBasic;

[TestFixture]
public class LoginTests : TestBase
{
    [Test]
    public async Task LoginAuthSuccess()
    {
        await LoginToFrameHome(FrameLogin, FramePassword);
        await Expect(_frameHomePopup.GetByText("michael+automation@framevr.io")).ToBeVisibleAsync();
        Console.WriteLine("Test Complete: Login with proper auth");
        await _page!.WaitForTimeoutAsync(3000);
    }

    [Test]
    public async Task LoginBadCredential()
    {
        await LoginToFrameHome(FrameLogin, "BadPassword");
        await Expect(_frameHomePopup.GetByText("Incorrect password. Please try again.")).ToBeVisibleAsync();
        Console.WriteLine("Test Complete: Login with Bad credential");
    }

    [Test]
    public async Task LogOutSuccess()
    {
        await LoginToFrameHome(FrameLogin, FramePassword);
        //Executing a force command since Log Out is hidden behind a nav drawer
        await _frameHomePopup.GetByText("Log Out").ClickAsync(new() { Force = true });
        await Expect(_frameHomePopup.GetByRole(AriaRole.Button, new () {Name = "Login / Signup"})).ToBeVisibleAsync();
        Console.WriteLine("Test Complete: Log out");
    }
}