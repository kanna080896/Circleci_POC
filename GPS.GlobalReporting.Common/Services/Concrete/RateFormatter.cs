using GPS.GlobalReporting.Common.Services.Interfaces;
using Serilog;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class RateFormatter : IRateFormatter
{
    /// <summary>
    /// A function to convert Conversion rate (DE10) to a proper decimal value.
    /// First char is Decimal Indicator indicates the number of positions the decimal point should be moved from the right.
    /// 61000000 - 6 is the decimal indicator and this function converts the value to 1.000000
    /// </summary>
    /// <param name="rate"> Data element 10 </param>
    /// <returns>A proper decimal value</returns>
    public string Format(string rate)
    {
        var decimalPlacementParsed = int.TryParse(rate[0].ToString(), out var decimalPlacement);
        if (!decimalPlacementParsed)
        {
            Log.Logger.Warning("{Rate} does not comply with the expected format of a rate", rate);
            return string.Empty;
        }

        var actualRate = rate.Substring(1, rate.Length - 1);

        //If length == decimal placement i.e. result would begin with the decimal place, pad start with a 0
        if (actualRate.Length - decimalPlacement == 0)
        {
            actualRate = "0" + actualRate;
        }

        //If rate is smaller than needed for decimal placement, then pad end with 0's
        while (actualRate.Length - decimalPlacement <= 0)
        {
            actualRate += "0";
        }

        actualRate = actualRate.Insert(actualRate.Length - decimalPlacement, ".");

        return actualRate;
    }

    public string FormatRate(string rate)
    {
        var actualRate = Format(rate);
        return Convert.ToDouble(actualRate).ToString("N6");
    }

    public string FormatConversionRate(string rate)
    {
        var actualRate = Format(rate);
        return Convert.ToDouble(actualRate).ToString("N9");
    }
}