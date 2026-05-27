
using System.Net.Http.Headers;
using Microsoft.Playwright;
using NUnit.Framework;
using Microsoft.Playwright.NUnit;
using System.Text.Json;


public class APITestBase : PlaywrightTest
{
    protected string FrameAPIKey = Environment.GetEnvironmentVariable("FRAME_API_KEY")!;
    protected Dictionary<string, string> Headers;
    protected HttpClient FrameHttpClient;
    protected string frameRef = "qa-staticautomationframe";
    protected string apiHeader = "https://api.framevr.io/automate/v1";

    [OneTimeSetUp]
    [Category("API")]
    public async Task Authenticate()
    {
        FrameHttpClient = new HttpClient();
        FrameHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", FrameAPIKey); 
    }

    [OneTimeTearDown]
    [Category("API")]
    public async Task Teardown()
    {
        FrameHttpClient?.Dispose();
    }
}
