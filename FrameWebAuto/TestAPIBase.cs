
using System.Net.Http.Headers;
using Microsoft.Playwright;
using NUnit.Framework;
using Microsoft.Playwright.NUnit;
using System.Text.Json;


public class TestAPIBase : PlaywrightTest
{
    protected string FrameAPIKey = Environment.GetEnvironmentVariable("FRAME_API_KEY")!;
    protected Dictionary<string, string> Headers;
    protected HttpClient FrameHttpClient;
    protected string frameRef = "qa-staticautomationframe";
    protected string apiHeader = "https://api.framevr.io/automate/v1";

    [SetUp]
    [Category("API")]
    public async Task Authenticate()
    {
        FrameHttpClient = new HttpClient();
        FrameHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", FrameAPIKey); 
    }

    [Test]
    [Category("API")]
    public async Task GetActiveScene()
    {
        
        
        HttpResponseMessage response = await FrameHttpClient.GetAsync(
            $"{apiHeader}/scene/{frameRef}"
        );
        string body = await response.Content.ReadAsStringAsync();
        Console.WriteLine("calling GetActiveScene");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True, $"Request failed: {body}");
        var jsonResponse = JsonDocument.Parse(body);
        Assert.That(jsonResponse.RootElement.GetProperty("data").GetProperty("activeSlideId").GetString(),
                Is.Not.Null.And.Not.Empty);
        
    }

    [Test]
    [Category("API")]
    public async Task WriteActiveScene()
    {
        string SceneNum = "1";
        HttpResponseMessage response = await FrameHttpClient.PutAsync($"{apiHeader}/scene/{frameRef}/{SceneNum}", null);
        string body = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Calling WriteActiveScene, setting to {SceneNum}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
        var jsonResponse = JsonDocument.Parse(body);
        Assert.That(jsonResponse.RootElement.GetProperty("data").GetProperty("activeSlideIdIndex").GetInt32(),
                Is.EqualTo(int.Parse(SceneNum)));
    }

    [TearDown]
    [Category("API")]
    public async Task Teardown()
    {
        FrameHttpClient?.Dispose();
    }
}
