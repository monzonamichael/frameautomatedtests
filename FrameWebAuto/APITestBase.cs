
using System.Net.Http.Headers;
using Microsoft.Playwright.NUnit;

public class APITestBase : PlaywrightTest
{
    protected string FrameAPIKey = Environment.GetEnvironmentVariable("FRAME_API_KEY")!;
    protected Dictionary<string, string> Headers;
    protected HttpClient FrameHttpClient;
    protected string frameRef = "qa-staticautomationframe";
    protected string apiHeader = "https://api.framevr.io/automate/v1";

    [OneTimeSetUp]
    [Category("API")]
    [Category("GitHub")]
    public async Task Authenticate()
    {
        //Builds our connection reference and sets the API Key
        FrameHttpClient = new HttpClient();
        FrameHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", FrameAPIKey); 
    }

    [OneTimeTearDown]
    [Category("API")]
    [Category("GitHub")]
    public async Task Teardown()
    {
        FrameHttpClient?.Dispose();
    }
}
