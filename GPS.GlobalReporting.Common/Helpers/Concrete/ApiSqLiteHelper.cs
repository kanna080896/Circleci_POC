using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Helpers.Interfaces;
using Serilog;

namespace GPS.GlobalReporting.Common.Helpers.Concrete;

public class ApiSqLiteHelper : IApiSqLiteHelper
{
    public string GenerateSqLiteDbKey(ReportTypeEnum reportType)
    {
        var guid = Guid.NewGuid();
        return reportType switch
        {
            ReportTypeEnum.NonClearing => $"Transaction{guid}.db",
            ReportTypeEnum.Balance => $"Balance{guid}.db",
            _ => throw new ArgumentOutOfRangeException(nameof(reportType))
        };
    }

    public void DeleteSqLiteDbFile(string dbKey)
    {
        if (File.Exists(dbKey))
        {
            Log.Logger.Information("Deleting database file: {DbKey}", dbKey);
            File.Delete(dbKey);
        }
    }
}