using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class RateFormatterTests
{
    private readonly IRateFormatter _rateFormatter;

    public RateFormatterTests()
    {
        _rateFormatter = new RateFormatter();
    }

    [Theory, InlineData("61000000"), InlineData("61")]
    public void Format_Success(string input)
    {
        const string expected = "1.000000";
        Assert.Equal(expected, _rateFormatter.Format(input));
    }

    [Fact]
    public void When_FormatCalled_WithNonCompliantRate_ReturnsEmptyString()
    {
        var result = _rateFormatter.Format("test");
        Assert.True(string.IsNullOrEmpty(result));
    }
    
    [Fact]
    public void FormatRate_WhereResultWouldBeginWithDecimalPlace_Success()
    {
        const string expected = "0.892308";
        Assert.Equal(expected, _rateFormatter.FormatRate("78923077"));
    }

    [Fact]
    public void FormatRate_Success()
    {
        const string expected = "0.970928";
        Assert.Equal(expected, _rateFormatter.FormatRate("709709275"));
    }

    [Fact]
    public void FormatRate_Success1()
    {
        const string expected = "0.970927";
        Assert.Equal(expected, _rateFormatter.FormatRate("709709274"));
    }

    [Fact]
    public void FormatConversionRate_Success()
    {
        const string expected = "0.970927500";
        Assert.Equal(expected, _rateFormatter.FormatConversionRate("709709275"));
    }

    [Fact]
    public void FormatConversionRate_Success1()
    {
        const string expected = "0.970927599";
        Assert.Equal(expected, _rateFormatter.FormatConversionRate("90970927599"));
    }
}