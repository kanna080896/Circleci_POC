using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Models;

namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IReportingConfiguration
{
    T GetValue<T>(string key);
    string GetConnectionString(string key);
}