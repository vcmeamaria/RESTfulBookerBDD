using Allure.Net.Commons;
using Reqnroll;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.Hooks;

[Binding]
public sealed class TestHooks
{
    private readonly DriverFactory _drivers;
    private readonly ScenarioContext _context;

    public TestHooks(
        DriverFactory drivers,
        ScenarioContext context)
    {
        _drivers = drivers;
        _context = context;
    }

    [AfterScenario("ui", Order = 100)]
    public void AfterUiScenario()
    {
        try
        {
            if (_context.TestError is not null &&
                _drivers.Driver is not null)
            {
                var screenshot =
                    _drivers.Screenshot();

                _context.Add(
                    "failureScreenshot",
                    screenshot);

                AllureApi.AddAttachment(
                    "Failure screenshot",
                    "image/png",
                    screenshot,
                    ".png");
            }
        }
        finally
        {
            _drivers.Dispose();
        }
    }
}