using System.Net;
using Reqnroll;
using RestfulBooker.Tests.Api;
using RestfulBooker.Tests.Models;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.StepDefinitions;

[Binding]
public sealed class ApiSteps
{
    private readonly BookerApiClient _api;
    private readonly ScenarioState _state;

    public ApiSteps(
        BookerApiClient api,
        ScenarioState state)
    {
        _api = api;
        _state = state;
    }

    private Booking BuildUniqueBooking(
        bool depositPaid = true)
    {
        var uniqueSuffix =
            Guid.NewGuid()
                .ToString("N")[..8];

        return new Booking(
            firstname: $"Auto{uniqueSuffix}",
            lastname: "Tester",
            totalprice: 450,
            depositpaid: depositPaid,
            bookingdates: new BookingDates(
                DateTime.UtcNow
                    .AddDays(7)
                    .ToString("yyyy-MM-dd"),
                DateTime.UtcNow
                    .AddDays(10)
                    .ToString("yyyy-MM-dd")),
            additionalneeds: "Breakfast");
    }

    [Given("the booking service is available")]
    public async Task BookingServiceIsAvailable()
    {
        var response = await _api.Ping();

        Assert.That(
            (int)response.StatusCode,
            Is.EqualTo(201));
    }

    [When("I call the booking service health endpoint")]
    public async Task CallHealthEndpoint()
    {
        _state.LastResponse =
            await _api.Ping();
    }

    [Then("the health response should be successful")]
    public void HealthResponseShouldBeSuccessful()
    {
        Assert.That(
            (int)_state.LastResponse!.StatusCode,
            Is.EqualTo(201));
    }

    [Given("I have a valid admin token")]
    public async Task HaveValidAdminToken()
    {
        var authentication =
            await _api.Authenticate();

        _state.Token =
            authentication.token ?? string.Empty;

        Assert.That(
            _state.Token,
            Is.Not.Empty);
    }

    [When("I create a unique booking")]
    [Given("I created a unique booking")]
    public async Task CreateUniqueBooking()
    {
        _state.Expected =
            BuildUniqueBooking();

        var response =
            await _api.Create(
                _state.Expected);

        _state.BookingId =
            response.bookingid;

        _state.Latest =
            response.booking;
    }

    [When("I create a booking with depositpaid set to {string}")]
    public async Task CreateBookingWithDepositPaid(
        string depositPaidValue)
    {
        var depositPaid =
            bool.Parse(depositPaidValue);

        _state.Expected =
            BuildUniqueBooking(depositPaid);

        var response =
            await _api.Create(
                _state.Expected);

        _state.BookingId =
            response.bookingid;

        _state.Latest =
            response.booking;
    }

    [Then("the booking should be created")]
    public void BookingShouldBeCreated()
    {
        Assert.Multiple(() =>
        {
            Assert.That(
                _state.BookingId,
                Is.GreaterThan(0));

            Assert.That(
                _state.Latest!.firstname,
                Is.EqualTo(
                    _state.Expected!.firstname));
        });
    }

    [Then("the booking should be created with depositpaid set to {string}")]
    public void BookingShouldHaveDepositPaidValue(
        string depositPaidValue)
    {
        var expectedDepositPaid =
            bool.Parse(depositPaidValue);

        Assert.Multiple(() =>
        {
            Assert.That(
                _state.BookingId,
                Is.GreaterThan(0));

            Assert.That(
                _state.Latest!.depositpaid,
                Is.EqualTo(expectedDepositPaid));

            Assert.That(
                _state.Latest.firstname,
                Is.EqualTo(
                    _state.Expected!.firstname));
        });
    }

    [Then("the booking should be retrievable")]
    public async Task BookingShouldBeRetrievable()
    {
        _state.Latest =
            await _api.GetBooking(
                _state.BookingId);

        Assert.That(
            _state.Latest.lastname,
            Is.EqualTo("Tester"));
    }

    [When("I replace the booking details")]
    public async Task ReplaceBookingDetails()
    {
        _state.Expected =
            _state.Expected! with
            {
                totalprice = 600,
                additionalneeds =
                    "Breakfast and late checkout",
                bookingdates =
                    new BookingDates(
                        DateTime.UtcNow
                            .AddDays(8)
                            .ToString("yyyy-MM-dd"),
                        DateTime.UtcNow
                            .AddDays(12)
                            .ToString("yyyy-MM-dd"))
            };

        await _api.Replace(
            _state.BookingId,
            _state.Token,
            _state.Expected);
    }

    [Then("the replaced details should be persisted")]
    public async Task ReplacedDetailsShouldBePersisted()
    {
        _state.Latest =
            await _api.GetBooking(
                _state.BookingId);

        Assert.Multiple(() =>
        {
            Assert.That(
                _state.Latest.totalprice,
                Is.EqualTo(600));

            Assert.That(
                _state.Latest.additionalneeds,
                Is.EqualTo(
                    "Breakfast and late checkout"));
        });
    }

    [When("I partially update the price and additional needs")]
    public async Task PartiallyUpdateBooking()
    {
        await _api.Patch(
            _state.BookingId,
            _state.Token,
            new
            {
                totalprice = 700,
                additionalneeds =
                    "Airport pickup"
            });
    }

    [Then("only the selected booking fields should change")]
    public async Task SelectedBookingFieldsShouldChange()
    {
        _state.Latest =
            await _api.GetBooking(
                _state.BookingId);

        Assert.Multiple(() =>
        {
            Assert.That(
                _state.Latest.totalprice,
                Is.EqualTo(700));

            Assert.That(
                _state.Latest.additionalneeds,
                Is.EqualTo(
                    "Airport pickup"));

            Assert.That(
                _state.Latest.firstname,
                Is.EqualTo(
                    _state.Expected!.firstname));
        });
    }

    [When("I delete the booking")]
    public async Task DeleteBooking()
    {
        _state.LastResponse =
            await _api.Delete(
                _state.BookingId,
                _state.Token);

        Assert.That(
            (int)_state.LastResponse.StatusCode,
            Is.EqualTo(201));
    }

    [Then("retrieving the deleted booking should return 404")]
    public async Task DeletedBookingShouldReturn404()
    {
        var response =
            await _api.Get(
                _state.BookingId);

        Assert.That(
            response.StatusCode,
            Is.EqualTo(
                HttpStatusCode.NotFound));
    }

    [When("I try to replace it without authentication")]
    public async Task ReplaceWithoutAuthentication()
    {
        _state.LastResponse =
            await _api.ReplaceWithoutToken(
                _state.BookingId,
                BuildUniqueBooking());
    }

    [Then("the update response should be 401 or 403")]
    public void UpdateShouldBeRejected()
    {
        Assert.That(
            (int)_state.LastResponse!.StatusCode,
            Is.AnyOf(401, 403));
    }
}