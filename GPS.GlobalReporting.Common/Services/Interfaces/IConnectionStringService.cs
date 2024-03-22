namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IConnectionStringService
{
    string GetConnectionString(string key);
    int GetCommandTimeout();
    string GetSqLiteConnectionString(string key);
}