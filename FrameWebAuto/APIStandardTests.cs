using System.Text.Json;


public class APIStandardTests : APITestBase
{
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

    [Test]
    [Category("API")]
    public async Task ReadFrame()
    {
        HttpResponseMessage response = await FrameHttpClient.GetAsync(
            $"{apiHeader}/frame/{frameRef}"
        );
        string body = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Calling Readframe");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True, $"Request failed: {body}");
        var jsonResponse = JsonDocument.Parse(body);
        Assert.That(jsonResponse.RootElement.GetProperty("message").ToString(), Is.EqualTo($"Settings of {frameRef}"));
        Assert.That(jsonResponse.RootElement.GetProperty("data").ToString(), Is.Not.Empty);
    }

    [Test]
    [Category("API")]
    public async Task UpdateFrame()
    {
        //Send arbitrary data
        var deliverBody = new { isSinglePlayerMode = false };
        //Convert our change to JSON content    
        var content = new StringContent(
            JsonSerializer.Serialize(deliverBody),
            System.Text.Encoding.UTF8,
            "application/json"
        );
         HttpResponseMessage response = await FrameHttpClient.PatchAsync(
            $"{apiHeader}/frame/{frameRef}", content
        );

        string body = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonDocument.Parse(body);
        Console.WriteLine("Calling Updateframe");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");
        
        Assert.That(response.IsSuccessStatusCode, Is.True, $"Request failed: {body}");
        Assert.That(jsonResponse.RootElement.GetProperty("isSinglePlayerMode").ToString().ToLower(), Is.EqualTo("false"));
    }
}