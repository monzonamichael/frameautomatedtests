
using System.Net.Http.Headers;
using Microsoft.Playwright;
using NUnit.Framework;
using Microsoft.Playwright.NUnit;
public class TestAPIBase : PlaywrightTest
{
    protected string FrameAPIKey = Environment.GetEnvironmentVariable("FRAME_API_KEY")!;
    protected Dictionary<string, string> Headers;
    protected HttpClient FrameHttpClient;

    [SetUp]
    public async Task Authenticate()
    {
        Headers = new Dictionary<string, string>
        {
            {
                "x-api-key",
                FrameAPIKey
            }
        };
        FrameHttpClient = new HttpClient();
        FrameHttpClient.DefaultRequestHeaders.Add("x-api-key", FrameAPIKey);
    }

    [Test]
    public async Task GetActiveScene()
    {
        string frameRef = "qa-whatever"; // replace with your actual frame ref
        
        HttpResponseMessage response = await FrameHttpClient.GetAsync(
            $"https://api.framevr.io/automate/v1/scene/{frameRef}"
        );

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True, $"Request failed: {body}");
    }

    [TearDown]
    public async Task Teardown()
    {
        FrameHttpClient?.Dispose();
    }
}
