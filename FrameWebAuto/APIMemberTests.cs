using System.Text.Json;


public class APIMemberTests : APITestBase
{
    string testEmail = "michael+automationMember@framevr.io";

    //Checks if an email exists in Frame Member using the api, true or false
    public async Task CheckReadFrameMemberExists(string email, bool toCheck)
    {
        HttpResponseMessage response = await FrameHttpClient.GetAsync(
            $"{apiHeader}/member/{frameRef}");

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Calling ReadFrameMember, expecting {testEmail} exists to be {toCheck.ToString()}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
        var jsonResponse = JsonDocument.Parse(body);
        if (toCheck == true) Assert.That(jsonResponse.RootElement.GetProperty("data").ToString().Contains(testEmail), Is.True);
        else Assert.That(jsonResponse.RootElement.GetProperty("data").ToString().Contains(testEmail), Is.False);
    }

    [OneTimeSetUp]
    [Category("API")]
    public async Task CreateFrameMember()
    {
        HttpResponseMessage response = await FrameHttpClient.PostAsync(
            $"{apiHeader}/member/{frameRef}/{testEmail}", null);

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Calling CreateFrameMember with {testEmail}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
    }

    [Test]
    [Category("API")]
    public async Task ReadFrameMember()
    {
        await CheckReadFrameMemberExists(testEmail, true);
    }

    [OneTimeTearDown]
    [Category("API")]
    public async Task DeleteFrameMember()
    {
        HttpResponseMessage response = await FrameHttpClient.DeleteAsync(
            $"{apiHeader}/member/{frameRef}/{testEmail}");

        string body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Calling DeleteFrameMember on {testEmail}");
        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"Body: {body}");

        Assert.That(response.IsSuccessStatusCode, Is.True);
        await CheckReadFrameMemberExists(testEmail, false);
    }
}