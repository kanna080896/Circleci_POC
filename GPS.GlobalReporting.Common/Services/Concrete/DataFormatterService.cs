using System.Globalization;
using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class DataFormatterService : IDataFormatterService
{
    public string FormatDate(DateTime date, DateTypeEnum dateType)
    {
        return dateType switch
        {
            DateTypeEnum.NoSeparation => date.ToString("yyyyMMdd"),
            DateTypeEnum.DashSeparated => date.ToString("yyyy-MM-dd"),
            _ => throw new ArgumentOutOfRangeException(nameof(dateType), dateType, null)
        };
    }

    public string FormatDateTime(DateTime date)
    {
        return date.ToString("yyyyMMddHHmmss");
    }

    public string FormatDateTimeUtc(DateTime date)
    {
        return date.ToString("ddMMyyHHmm");
    }

    /// <summary>
    /// Remove trailing zeros up to the said decimal point without truncating or rounding off values.
    /// </summary>
    public string FormatMonetaryValue(decimal value, int exponent)
    {
        var splitString = value.ToString(CultureInfo.InvariantCulture).Split('.');
        var nonDecPart = splitString[0].Trim();
        var decPart = "000000000";

        if (splitString.Length > 1) //If no decimal part in value
        {
            decPart = splitString[1].Trim() + decPart;
        }

        while (decPart.Length > exponent)
        {
            if (decPart.EndsWith("0"))
            {
                decPart = decPart.Remove(decPart.Length - 1, 1);
            }
            else
            {
                break;
            }
        }

        string result;
        if (decPart.Length > 0)
        {
            result = nonDecPart + '.' + decPart;
        }
        else
        {
            result = nonDecPart;
        }

        return result;
    }

    public string FormatMonetaryValue(double value, int exponent)
    {
        var result = FormatMonetaryValue(Convert.ToDecimal(value), exponent);
        return result;
    }
}