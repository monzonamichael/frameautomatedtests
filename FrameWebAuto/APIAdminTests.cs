using System.Text.Json;


public class APIAdminTests : APITestBase
{
    string testEmail = "michael+automationAdmin@framevr.io";

        //Checks if an email exists in Frame Admin using the api, true or false
    public async Task CheckReadFrameAdminExists(string email, bool toCheck)
    {
        HttpResponseMessage response = await FrameHttpClient.GetAsync(
            $"{apiHeader}/admin/{frameRef}");

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Calling ReadFrameAdmin, expecting {testEmail} exists to be {toCheck.ToString()}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
        var jsonResponse = JsonDocument.Parse(body);
        if (toCheck == true) Assert.That(jsonResponse.RootElement.GetProperty("data").ToString().Contains(testEmail), Is.True);
        else Assert.That(jsonResponse.RootElement.GetProperty("data").ToString().Contains(testEmail), Is.False);
    }

    [OneTimeSetUp]
    [Category("Current")]
    [Category("GitHub")]
    public async Task CreateFrameAdmin()
    {
        HttpResponseMessage response = await FrameHttpClient.PostAsync(
            $"{apiHeader}/admin/{frameRef}/{testEmail}", null);

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Calling CreateFrameAdmin with {testEmail}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    [Category("Current")]
    [Category("GitHub")]
    public async Task ReadFrameAdmin()
    {
        await CheckReadFrameAdminExists(testEmail, true);
    }

    [OneTimeTearDown]
    [Category("Current")]
    [Category("GitHub")]
    public async Task DeleteFrameAdmin()
    {
        HttpResponseMessage response = await FrameHttpClient.DeleteAsync(
            $"{apiHeader}/admin/{frameRef}/{testEmail}");

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Calling DeleteFrameAdmin on {testEmail}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
        await CheckReadFrameAdminExists(testEmail, false);
    }
}