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
    [Category("FrameManagement")]
    public async Task CreateNewFrame()
    {
        await LoginToFrameHome(FrameLogin, FramePassword);
        await _frameHomePopup!.Locator("button.v-btn--variant-outlined:has-text('New Frame')").ClickAsync();
        await _frameHomePopup.Locator("input[placeholder='My New Frame']").FillAsync("qa-automation-test-1");
        await _frameHomePopup.Locator("button.v-btn--variant-elevated:has-text('New Frame')").ClickAsync();
        await Expect(_frameHomePopup).ToHaveURLAsync("https://framevr.io/" + FrameTestURL);
        Console.WriteLine("Test Complete: Create + Connect to New Frame");
    }


    [TearDown]
    [Category("FrameManagement")]
    public async Task CleanUpFrames()
    {
        await _frameHomePopup!.GotoAsync("https://framevr.io/home");
        await _frameHomePopup.WaitForURLAsync("**/home");
        await _frameHomePopup
            .Locator(".position-relative:has(a:has-text('qa-automation-test-1')) button:has(.mdi-dots-vertical)")
            .ClickAsync(new() { Force = true });
        await _frameHomePopup.Locator(".v-list-item:has(.mdi-delete)").ClickAsync();
        await _frameHomePopup.GetByText("Yes").ClickAsync();
        await _frameHomePopup.WaitForTimeoutAsync(1000);
        await _frameHomePopup.ReloadAsync();
        await Expect(_frameHomePopup.GetByText(FrameTestURL)).Not.ToBeVisibleAsync();
        Console.WriteLine("Complete: Delete frame " + FrameTestURL);
    }
}