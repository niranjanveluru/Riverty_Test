using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

public static class ExtentReportHelper
{
    private static readonly Lazy<ExtentReports> lazyExtent = new(() =>
    {
        var reportPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "TestResults", "ExtentReport.html");
        reportPath = Path.GetFullPath(reportPath);
        Directory.CreateDirectory(Path.GetDirectoryName(reportPath));
        var sparkReporter = new ExtentSparkReporter(reportPath);
        var extent = new ExtentReports();
        extent.AttachReporter(sparkReporter);
        Console.WriteLine("Extent report initialized.");
        return extent;
    });

    public static ExtentReports extent => lazyExtent.Value;
    public static ExtentTest test;

    public static void CreateTest(string testName)
    {
        Console.WriteLine("Creating test: " + testName);
        test = extent.CreateTest(testName);
    }

    public static void LogStep(string message)
    {
        test?.Log(Status.Info, message);
    }

    public static void LogError(string message)
    {
        test?.Log(Status.Fail, message);
    }

    public static void FlushReport()
    {
        if (test == null)
        {
            Console.WriteLine("WARNING: No test was created. Report will be empty.");
        }

        extent.Flush();
        Console.WriteLine("Extent report flushed.");
    }
}