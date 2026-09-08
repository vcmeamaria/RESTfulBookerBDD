using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace RestfulBooker.Tests.Support;

public static class ExtentReport
{
    public static readonly ExtentReports Instance;

    static ExtentReport()
    {
        var resultsDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "TestResults");

        Directory.CreateDirectory(
            resultsDirectory);

        var reportPath = Path.Combine(
            resultsDirectory,
            "extent-report.html");

        var spark = new ExtentSparkReporter(
            reportPath);

        spark.Config.DocumentTitle =
            "RESTful Booker Automation";

        Instance = new ExtentReports();

        Instance.AttachReporter(
            spark);

        Instance.AddSystemInfo(
            "Framework",
            "Reqnroll + NUnit + RestSharp + Selenium");
    }
}