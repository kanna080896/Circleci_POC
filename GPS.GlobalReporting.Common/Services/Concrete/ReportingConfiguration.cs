using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Exceptions;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class ReportingConfiguration : IReportingConfiguration
{
    private readonly IConfiguration _configuration;

    public ReportingConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public T GetValue<T>(string key)
    {
        var value = _configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            Log.Logger.Warning("Getting the configuration value for the key: {Key} returned an empty value", key);
        }
        
        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            throw new InvalidConfigurationTypeException(key, typeof(T).ToString());
        }
    }

    public string GetConnectionString(string key)
    {
        var connectionString = _configuration.GetConnectionString(key);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidConfigurationKeyException(key);
        }

        return connectionString;
    }
}