namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IRateFormatter
{
    string Format(string rate);
    string FormatRate(string rate);
    string FormatConversionRate(string rate);
}