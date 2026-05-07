using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

[TestFixture]
public class FrameManagementTests : TestBase
{
    [Test]
    public async Task CreateNewFrame()
    {
        await LoginToFrameHome(FrameLogin, FramePassword);
        await _frameHomePopup.Locator("button.v-btn--variant-outlined:has-text('New Frame')").ClickAsync();
        await _frameHomePopup.Locator("input[placeholder='My New Frame']").FillAsync("qa-automation-test-1");
        await _frameHomePopup.Locator("button.v-btn--variant-elevated:has-text('New Frame')").ClickAsync();
        await _frameHomePopup.WaitForURLAsync("**/" + FrameTestURL);
        await Expect(_frameHomePopup).ToHaveURLAsync("https://framevr.io/" + FrameTestURL);
        Console.WriteLine("Test Complete: Create + Connect to New Frame");
    }


    [TearDown]
    public async Task CleanUpFrames()
    {
        //await LoginToFrameHome(FrameLogin, FramePassword);
        //await _frameHomePopup.Locator("#v-menu-v-11").ClickAsync();
    }
}