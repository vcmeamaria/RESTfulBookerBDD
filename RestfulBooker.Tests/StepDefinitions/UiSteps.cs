using Reqnroll;
using RestfulBooker.Tests.Config;
using RestfulBooker.Tests.Pages;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.StepDefinitions;

[Binding]
public sealed class UiSteps
{
    private readonly DriverFactory _drivers;

    private HomePage? _page;

    public UiSteps(DriverFactory drivers)
    {
        _drivers = drivers;
    }

    [Given("I open the RESTful Booker landing page")]
    public void OpenRestfulBookerLandingPage()
    {
        var driver = _drivers.Create();

        _page = new HomePage(driver)
            .Open(TestSettings.Load().BaseUrl);
    }

    [Then("the page should identify RESTful Booker")]
    public void PageShouldIdentifyRestfulBooker()
    {
        Assert.That(
            _page!.Title.Contains(
                "restful",
                StringComparison.OrdinalIgnoreCase)
            ||
            _page.BodyText.Contains(
                "restful-booker",
                StringComparison.OrdinalIgnoreCase),
            Is.True);
    }

    [Then("the page should contain API learning content")]
    public void PageShouldContainApiLearningContent()
    {
        Assert.Multiple(() =>
        {
            Assert.That(
                _page!.HasHeading,
                Is.True);

            Assert.That(
                _page.BodyText,
                Does.Contain("API").IgnoreCase);
        });
    }
}