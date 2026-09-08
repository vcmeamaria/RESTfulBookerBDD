using AventStack.ExtentReports;
using Reqnroll;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.Hooks;

[Binding]
public sealed class ExtentHooks
{
    private readonly ScenarioContext _context;

    private ExtentTest? _test;

    public ExtentHooks(
        ScenarioContext context)
    {
        _context = context;
    }

    [BeforeScenario(Order = -50)]
    public void Before()
    {
        _test =
            ExtentReport.Instance.CreateTest(
                _context.ScenarioInfo.Title);

        foreach (
            var tag in
            _context.ScenarioInfo.CombinedTags)
        {
            _test.AssignCategory(tag);
        }
    }

    [AfterStep]
    public void Step()
    {
        _test?.Info(
            _context.StepContext.StepInfo.Text);
    }

    [AfterScenario(Order = 200)]
    public void After()
    {
        if (_context.TestError is null)
        {
            _test?.Pass(
                "Scenario passed");
        }
        else
        {
            _test?.Fail(
                _context.TestError);
        }
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        ExtentReport.Instance.Flush();
    }
}