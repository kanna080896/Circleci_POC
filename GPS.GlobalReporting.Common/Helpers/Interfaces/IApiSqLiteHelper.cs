using GPS.GlobalReporting.Common.Enums;

namespace GPS.GlobalReporting.Common.Helpers.Interfaces;

public interface IApiSqLiteHelper
{
    string GenerateSqLiteDbKey(ReportTypeEnum reportType);
    void DeleteSqLiteDbFile(string dbKey);
}