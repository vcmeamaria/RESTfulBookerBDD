using RestfulBooker.Tests.Config;
using RestfulBooker.Tests.Pages;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests;

[TestFixture]
public sealed class UiSmokeTest
{
    [Test]
    public void RestfulBookerLandingPageShouldBeAccessible()
    {
        using var drivers = new DriverFactory();

        var driver = drivers.Create();

        var page = new HomePage(driver)
            .Open(TestSettings.Load().BaseUrl);

        Assert.Multiple(() =>
        {
            Assert.That(
                page.Title.Contains(
                    "restful",
                    StringComparison.OrdinalIgnoreCase)
                ||
                page.BodyText.Contains(
                    "restful-booker",
                    StringComparison.OrdinalIgnoreCase),
                Is.True);

            Assert.That(
                page.HasHeading,
                Is.True);

            Assert.That(
                page.BodyText,
                Does.Contain("API").IgnoreCase);
        });
    }
}