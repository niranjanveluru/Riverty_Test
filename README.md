# Riverty_Test

# Application tested - Exchange Rate API Test Suite

This repository contains an API automated test framework for validating the https://fixer.io/ Exchange Rate API using **Reqnroll**, **NUnit**, and **ExtentReporting with logging**

## Test Automation Framework

```
PlaywrightTests/
├── Steps/
│   └── ApiSteps.cs           # Step definitions for API scenarios
├── Helpers/
│   └── ExtentReportHelper.cs # report and logging setup
├── Features/
│   └── api.feature           # BDD scenarios
├── TestResults/
│   └── ExtentReport.html     #  HTML report
├── appsettings.json          # Config file with base URL and access key
└── Hooks.cs                  # Reqnroll hooks for extended reporting
```

---

## Running Tests

```bash
dotnet test
```

After execution, the HTML report will be available at:

```
PlaywrightTests/TestResults/ExtentReport.html
```

## Configuration

Update `appsettings.json` with your API credentials:

```json
{
  "BaseUrl": "http://data.fixer.io/api/latest",
  "accessKey": "your_actual_access_key"
}
```

## Logging & Reporting

- Response with detaied logging
- Failed assertions are logged with detailed error messages
- ExtentReports shows pass/fail status, timestamps, and step-by-step logs

