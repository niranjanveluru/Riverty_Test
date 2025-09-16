using Reqnroll;
using AventStack.ExtentReports; // Needed for Status

[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario() =>
        ExtentReportHelper.CreateTest(_scenarioContext.ScenarioInfo.Title);

    [AfterScenario]
    public void AfterScenario()
    {
        var status = _scenarioContext.TestError != null ? Status.Fail : Status.Pass;
        var message = _scenarioContext.TestError?.Message ?? "Scenario passed";
        ExtentReportHelper.test?.Log(status, message);
    }

    [AfterTestRun]
    public static void AfterTestRun() => ExtentReportHelper.FlushReport();
}