using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class ConnectionStringService : IConnectionStringService
{
    private readonly IReportingConfiguration _configuration;

    public ConnectionStringService(IReportingConfiguration configuration)
    {
        _configuration = configuration;
    }

    public virtual string GetConnectionString(string key)
    {
        return _configuration.GetConnectionString(key);
    }

    public int GetCommandTimeout()
    {
        return _configuration.GetValue<int>("CommandTimeout");
    }

    public string GetSqLiteConnectionString(string key)
    {
        return $"Data Source={key};Pooling=false;";
    }
}