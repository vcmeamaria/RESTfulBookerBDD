using System.Text.Json;

namespace RestfulBooker.Tests.Config;

public sealed record TestSettings(
    string BaseUrl,
    string Browser,
    bool Headless)
{
    public static TestSettings Load()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "appsettings.json");

        var json = File.ReadAllText(path);

        var settings = JsonSerializer.Deserialize<TestSettings>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return settings
            ?? throw new InvalidOperationException(
                "Invalid appsettings.json");
    }
}