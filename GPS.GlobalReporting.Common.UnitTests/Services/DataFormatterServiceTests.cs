using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class DataFormatterServiceTests
{
    private readonly IDataFormatterService _dataFormatterService;

    public DataFormatterServiceTests()
    {
        _dataFormatterService = new DataFormatterService();
    }

    [Fact]
    public void FormatDate_DashSeparated_ReturnsFormattedDate()
    {
        var date = new DateTime(2022, 1, 1);
        var expected = "2022-01-01";

        var actual = _dataFormatterService.FormatDate(date, DateTypeEnum.DashSeparated);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatDate_NoSeparation_ReturnsFormattedDate()
    {
        var date = new DateTime(2022, 1, 1);
        var expected = "20220101";

        var actual = _dataFormatterService.FormatDate(date, DateTypeEnum.NoSeparation);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatDate_InvalidDateType_ThrowsException()
    {
        var date = new DateTime(2022, 1, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => _dataFormatterService.FormatDate(date, (DateTypeEnum)9999));
    }

    [Fact]
    public void FormatDateTime_ValidDateTime_ReturnsFormattedDateTime()
    {
        var dateTime = new DateTime(2022, 1, 1, 1, 1, 1);
        var expected = "20220101010101";

        var actual = _dataFormatterService.FormatDateTime(dateTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatMonetaryValue_RoundOffToNonZeroDecimalPlaces()
    {
        const int exponent = 2;
        // not truncating, truncating only zeros.
        var value = 1.123;
        Assert.Equal("1.123", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.123", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 1.128;
        Assert.Equal("1.128", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.128", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 1.100;
        Assert.Equal("1.10", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.10", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 1.000;
        Assert.Equal("1.00", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.00", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 0;
        Assert.Equal("0.00", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("0.00", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));
    }


    [Fact]
    public void FormatMonetaryValue_RoundOffToZeroDecimalPlaces()
    {
        const int exponent = 0;

        var value = 1.123;
        Assert.Equal("1.123", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.123", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 1.128;
        Assert.Equal("1.128", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.128", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 1.100;
        Assert.Equal("1.1", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1.1", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));

        value = 1.000;
        Assert.Equal("1", _dataFormatterService.FormatMonetaryValue(value, exponent));
        Assert.Equal("1", _dataFormatterService.FormatMonetaryValue((decimal)value, exponent));
    }
}