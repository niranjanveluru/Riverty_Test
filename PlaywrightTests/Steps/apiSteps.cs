using Reqnroll;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

[Binding]
public class ApiSteps
{
    private readonly HttpClient _client = new();
    private string _accessKey = "95ad8a81f0dc59d8e99c256cd9c0404f"; 
    private string _baseUrl;
    private HttpResponseMessage? _response;
    private JsonDocument? _jsonBody;

    public ApiSteps()
    {
        _baseUrl = ConfigReader.GetJsonKeyValue("BaseUrl");
    }

    [Given(@"I entered the ""(.*)"" access key")]
    public void Given_I_Entered_The_Valid_AccessKey(string access_key)
    {
        if(access_key == "valid"){
            _accessKey = ConfigReader.GetJsonKeyValue("accessKey");
            ExtentReportHelper.LogStep($"Entered valid access_key"); 
        }else{
            ExtentReportHelper.LogStep($"Entered in valid access_key");
        }
    }

    [Then(@"I see ""(.*)"" error in response")]
    public void And_I_See_Error_In_Response(string errorType)   
    {
        var actualValue = _jsonBody.RootElement.GetProperty("error");
        var errorCodeReceived = actualValue.GetProperty("code").GetInt32();
        var errorTypeReceived = actualValue.GetProperty("type").GetString();
        var errorInfoReceived = actualValue.GetProperty("info").GetString();

            switch (errorType)
            {
                case "invalid_access_key":
                    Assert.That(errorCodeReceived, Is.EqualTo(101));
                    Assert.That(errorTypeReceived, Is.EqualTo("invalid_access_key"));
                    Assert.That(errorInfoReceived, Is.EqualTo("You have not supplied a valid API Access Key. [Technical Support: support@apilayer.com]"));
                    ExtentReportHelper.LogStep($"Response contains errorCode : {errorCodeReceived} errorType : {errorType} error Info : {errorInfoReceived}");
                    break;

                case "invalid_currency_codes":
                    Assert.That(errorCodeReceived, Is.EqualTo(202));
                    Assert.That(errorTypeReceived, Is.EqualTo("invalid_currency_codes"));
                    Assert.That(errorInfoReceived, Is.EqualTo("You have provided one or more invalid Currency Codes. [Required format: currencies=EUR,USD,GBP,...]"));
                    ExtentReportHelper.LogStep($"Response contains errorCode : {errorCodeReceived} errorType : {errorType} error Info : {errorInfoReceived}");
                    break;

                default:
                    Assert.Fail($"Unexpected error type: {errorType}");
                    ExtentReportHelper.LogStep($"Unexpected error type: {errorType}");
                    break;
            }
    }

    [When(@"I send GET request with base as ""(.*)"" and symbols as ""(.*)""")]
    public async Task When_I_Send_Get_Request_With_Base_And_Symbols(string baseCurrency, string targetCurrency)
    {
        var url = $"{_baseUrl}?access_key={_accessKey}&base={baseCurrency}&symbols={targetCurrency}";
        _response = await _client.GetAsync(url);
        url = $"{_baseUrl}?access_key=accessKey&base={baseCurrency}&symbols={targetCurrency}";
        ExtentReportHelper.LogStep($"Request sent : {url}");
        var responseContent = await _response.Content.ReadAsStringAsync();
        _jsonBody = JsonDocument.Parse(responseContent);
        ExtentReportHelper.LogStep($"Response Body: {responseContent}");
    }

    [Then(@"I should get response code as {int}")]
    public void Then_I_Should_Get_Expected_Response_Code(int expectedCode)
    {
        Console.WriteLine((int)_response.StatusCode);

        if ((int)_response.StatusCode != expectedCode)
        {
            ExtentReportHelper.LogError($"Expected status code {expectedCode}, but got {(int)_response.StatusCode}");
            Assert.Fail($"Expected status code {expectedCode}, but got {(int)_response.StatusCode}");
        }
    }

    [Then(@"I see ""(.*)"" in response body as ""(.*)""")]
    public void Then_I_See_JsonObject_In_ResponseBody(string baseKey, string expectedValue)
    {
        var actualValue = _jsonBody.RootElement.GetProperty(baseKey).GetString();
        if (actualValue != expectedValue)
        {
            ExtentReportHelper.LogError($"Expected currency key {expectedValue}, but got {actualValue}");
            Assert.Fail($"Expected currency key {expectedValue}, but got {actualValue}");
        }
    }

    [Then(@"I see ""(.*)"" in response body as boolean value ""(.*)""")]
    public void Then_I_See_JsonObject_In_ResponseBody_As_Boolean(string jsonObjectName, bool expectedValue)
    {
        var actualValue = _jsonBody.RootElement.GetProperty(jsonObjectName).GetBoolean();
        if (actualValue != expectedValue)
        {
            ExtentReportHelper.LogError($"Expected success value {expectedValue}, but got {actualValue}");
            Assert.Fail($"Expected success value {expectedValue}, but got {actualValue}");
        }
    }

    [Then(@"I see ""(.*)"" in response body contains key ""(.*)""")]
    public void Then_I_See_JsonObject_Contains_Key(string jsonObjectName, string expectedKey)
    {
        var jsonObject = _jsonBody.RootElement.GetProperty(jsonObjectName);

        if (jsonObject.ValueKind != JsonValueKind.Object)
           Assert.Fail(jsonObjectName + " is not a JSON object");

        bool hasKey = jsonObject.TryGetProperty(expectedKey, out _);

        if (!hasKey)
        {
            ExtentReportHelper.LogError($"Key '{expectedKey}' not found in '{jsonObjectName}'");
            Assert.Fail($"Key '{expectedKey}' not found in '{jsonObjectName}'");
        }
    }
 
}