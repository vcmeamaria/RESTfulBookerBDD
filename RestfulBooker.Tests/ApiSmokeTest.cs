using RestfulBooker.Tests.Api;

namespace RestfulBooker.Tests;

[TestFixture]
public sealed class ApiSmokeTest
{
    [Test]
    public async Task PingEndpointShouldBeAvailable()
    {
        var api = new BookerApiClient();

        var response = await api.Ping();

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(201));
    }
}