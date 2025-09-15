using Microsoft.Extensions.Configuration;
using System;

public static class ConfigReader
{
    public static string GetJsonKeyValue(string key)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json");

        var config = builder.Build();
        var value = config[key];

        if (value == null)
        {
            throw new Exception("Key not found: " + key);
        }

        return value;
    }
}