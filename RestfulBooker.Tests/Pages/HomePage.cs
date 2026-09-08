using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RestfulBooker.Tests.Pages;

public sealed class HomePage
{
    private readonly IWebDriver _driver;

    private readonly WebDriverWait _wait;

    public HomePage(IWebDriver driver)
    {
        _driver = driver;

        _wait = new WebDriverWait(
            driver,
            TimeSpan.FromSeconds(10));
    }

    public HomePage Open(string url)
    {
        _driver.Navigate().GoToUrl(url);

        _wait.Until(
            driver =>
                !string.IsNullOrWhiteSpace(driver.Title));

        return this;
    }

    public string Title => _driver.Title;

    public string BodyText =>
        _wait
            .Until(
                driver =>
                    driver.FindElement(
                        By.TagName("body")))
            .Text;

    public bool HasHeading =>
        _driver
            .FindElements(
                By.CssSelector("h1,h2"))
            .Count > 0;
}