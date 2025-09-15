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
    private string _accessKey = "wrongAcessKey"; 
    private string _baseUrl;
    private HttpResponseMessage? _response;
    private JsonDocument? _jsonBody;

    public ApiSteps()
    {
        _baseUrl = ConfigReader.GetJsonKeyValue("BaseUrl");
    }

    [Given("I entered the valid access key")]
    public void Given_I_Entered_The_Valid_AccessKey()
    {
        _accessKey = ConfigReader.GetJsonKeyValue("accessKey"); 
    }

    [When(@"I send GET request with base as ""(.*)"" and symbols as ""(.*)""")]
    public async Task When_I_Send_Get_Request_With_Base_And_Symbols(string baseCurrency, string targetCurrency)
    {
        var url = $"{_baseUrl}?access_key={_accessKey}&base={baseCurrency}&symbols={targetCurrency}";
        _response = await _client.GetAsync(url);
        var responseContent = await _response.Content.ReadAsStringAsync();
        _jsonBody = JsonDocument.Parse(responseContent);
    }

    [Then(@"I should get response code as {int}")]
    public void Then_I_Should_Get_Expected_Response_Code(int expectedCode)
    {
        Assert.That((int)_response.StatusCode, Is.EqualTo(expectedCode));
    }

    [Then(@"I see ""(.*)"" in response body as ""(.*)""")]
    public void Then_I_See_JsonObject_In_ResponseBody(string baseKey, string expectedValue)
    {
        var actualValue = _jsonBody.RootElement.GetProperty(baseKey).GetString();
        Assert.That(actualValue, Is.EqualTo(expectedValue));
    }

    [Then(@"I see ""(.*)"" in response body as boolean value ""(.*)""")]
    public void Then_I_See_JsonObject_In_ResponseBody_As_Boolean(string jsonObjectName, bool expectedValue)
    {
        var actualValue = _jsonBody.RootElement.GetProperty(jsonObjectName).GetBoolean();
        Assert.That(actualValue, Is.EqualTo(expectedValue));
    }

    [Then(@"I see ""(.*)"" in response body contains key ""(.*)""")]
    public void Then_I_See_JsonObject_Contains_Key(string jsonObjectName, string expectedKey)
    {
        var jsonObject = _jsonBody.RootElement.GetProperty(jsonObjectName);

        if (jsonObject.ValueKind != JsonValueKind.Object)
           Assert.Fail(jsonObjectName + " is not a JSON object");

        bool hasKey = jsonObject.TryGetProperty(expectedKey, out _);
        Assert.IsTrue(hasKey, "Key '" + expectedKey + "' not found in '" + jsonObjectName + "'");
    }
 
}