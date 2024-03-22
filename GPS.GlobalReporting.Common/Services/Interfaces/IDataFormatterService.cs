using GPS.GlobalReporting.Common.Enums;

namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IDataFormatterService
{
    string FormatDate(DateTime date, DateTypeEnum dateType);
    string FormatDateTime(DateTime date);
    string FormatDateTimeUtc(DateTime date);
    string FormatMonetaryValue(decimal value, int exponent);
    string FormatMonetaryValue(double value, int exponent);
}