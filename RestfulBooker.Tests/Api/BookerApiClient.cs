using RestSharp;
using RestfulBooker.Tests.Config;
using RestfulBooker.Tests.Models;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.Api;

public sealed class BookerApiClient
{
    private readonly RestClient _client;

    public BookerApiClient()
    {
        _client = new RestClient(
            TestSettings.Load().BaseUrl);

        // RESTful Booker is strict about content negotiation.
        // RestSharp supports multiple serializers by default,
        // so we explicitly request JSON responses only.
        _client.AcceptedContentTypes =
            new[] { "application/json" };
    }

    private async Task<RestResponse> Execute(
        RestRequest request)
    {
        Log.Instance.Information(
            "{Method} {Resource}",
            request.Method,
            request.Resource);

        var response =
            await _client.ExecuteAsync(request);

        Log.Instance.Information(
            "Status {Status}; Body {Body}",
            (int)response.StatusCode,
            response.Content);

        return response;
    }

    private async Task<RestResponse<T>> Execute<T>(
        RestRequest request)
    {
        Log.Instance.Information(
            "{Method} {Resource}",
            request.Method,
            request.Resource);

        var response =
            await _client.ExecuteAsync<T>(request);

        Log.Instance.Information(
            "Status {Status}; Body {Body}",
            (int)response.StatusCode,
            response.Content);

        return response;
    }

    public Task<RestResponse> Ping()
    {
        return Execute(
            new RestRequest(
                "/ping",
                Method.Get));
    }

    public async Task<AuthResponse> Authenticate()
    {
        var request =
            new RestRequest(
                "/auth",
                Method.Post)
            .AddJsonBody(
                new AuthRequest(
                    "admin",
                    "password123"));

        var response =
            await Execute<AuthResponse>(
                request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return response.Data
            ?? throw new InvalidOperationException(
                "Authentication response could not be deserialized.");
    }

    public async Task<CreateBookingResponse> Create(
        Booking booking)
    {
        var request =
            new RestRequest(
                "/booking",
                Method.Post)
            .AddJsonBody(booking);

        var response =
            await Execute<CreateBookingResponse>(
                request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return response.Data
            ?? throw new InvalidOperationException(
                "Create booking response could not be deserialized.");
    }

    public Task<RestResponse> Get(int id)
    {
        return Execute(
            new RestRequest(
                $"/booking/{id}",
                Method.Get));
    }

    public async Task<Booking> GetBooking(int id)
    {
        var request =
            new RestRequest(
                $"/booking/{id}",
                Method.Get);

        var response =
            await Execute<Booking>(
                request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return response.Data
            ?? throw new InvalidOperationException(
                "Booking response could not be deserialized.");
    }

    private RestRequest AuthRequest(
        string path,
        Method method,
        string token)
    {
        return new RestRequest(
                path,
                method)
            .AddCookie(
                "token",
                token);
    }

    public async Task<Booking> Replace(
        int id,
        string token,
        Booking booking)
    {
        var request =
            AuthRequest(
                $"/booking/{id}",
                Method.Put,
                token)
            .AddJsonBody(booking);

        var response =
            await Execute<Booking>(
                request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return response.Data
            ?? throw new InvalidOperationException(
                "Replace booking response could not be deserialized.");
    }

    public async Task<Booking> Patch(
        int id,
        string token,
        object body)
    {
        var request =
            AuthRequest(
                $"/booking/{id}",
                Method.Patch,
                token)
            .AddJsonBody(body);

        var response =
            await Execute<Booking>(
                request);

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(200));

        return response.Data
            ?? throw new InvalidOperationException(
                "Patch booking response could not be deserialized.");
    }

    public Task<RestResponse> Delete(
        int id,
        string token)
    {
        return Execute(
            AuthRequest(
                $"/booking/{id}",
                Method.Delete,
                token));
    }

    public Task<RestResponse> ReplaceWithoutToken(
        int id,
        Booking booking)
    {
        var request =
            new RestRequest(
                $"/booking/{id}",
                Method.Put)
            .AddJsonBody(booking);

        return Execute(request);
    }
}