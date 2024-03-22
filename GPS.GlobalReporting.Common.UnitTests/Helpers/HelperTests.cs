using GPS.GlobalReporting.Common.Helpers.Concrete;
using GPS.GlobalReporting.Common.Helpers.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Helpers;

public class HelperTests
{
    private readonly IHelper _helper;

    public HelperTests()
    {
        _helper = new Helper();
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForCard_WhenAllIdsMatch_test()
    {
        const string expected = "655806992";
        Assert.Equal(expected, _helper.GetAccountNumber(1135407724251983, 1135407724251983, null, 655806992, null, 655806992));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForCard_ForCardAuth()
    {
        const string expected = "153923873";
        Assert.Equal(expected, _helper.GetAccountNumber(1191531689507216, 1191531689507216, null, 153923873, null, 153923873));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForCard_WhenAllIdsMatch()
    {
        const string expected = "60606060";
        Assert.Equal(expected, _helper.GetAccountNumber(10101010, 10101010, 10101010, 40404040, 50505050, 60606060));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForMultiFx_WhenPanAndMultiFxDontMatch()
    {
        const string expected = "50505050";
        Assert.Equal(expected, _helper.GetAccountNumber(10101010, 10101010, 30303030, 40404040, 50505050, 60606060));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForPrimary_WhenPanAndPrimaryIdDontMatch()
    {
        const string expected = "40404040";
        Assert.Equal(expected, _helper.GetAccountNumber(10101010, 20202020, 10101010, 40404040, 50505050, 60606060));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForPrimary_WhenPanAndPrimaryIdDontMatchMultiFxIsNull()
    {
        const string expected = "40404040";
        Assert.Equal(expected, _helper.GetAccountNumber(10101010, 20202020, null, 40404040, null, 60606060));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForCard_WhenPanAndPrimaryIdMatchMultiFxIsNull()
    {
        const string expected = "60606060";
        Assert.Equal(expected, _helper.GetAccountNumber(10101010, 10101010, null, 40404040, null, 60606060));
    }

    [Fact]
    public void GetAccountNumber_ReturnsTokenForPrimary_WhenPanPrimaryIdAndMultiFxDontMatch()
    {
        const string expected = "40404040";
        Assert.Equal(expected, _helper.GetAccountNumber(10101010, 20202020, 30303030, 40404040, 50505050, 60606060));
    }
    
    [Fact]
    public void GetAmountDirection_ReturnsCredit()
    {
        const string expected = "credit";
        Assert.Equal(expected, _helper.GetAmountDirection(1));
    }
    
    [Fact]
    public void GetAmountDirection_ReturnsDebit()
    {
        const string expected = "debit";
        Assert.Equal(expected, _helper.GetAmountDirection(0));
    }

    [Theory, InlineData("MCRD"), InlineData("MAES")]
    public void GetMessageSourceValueFromCardProduct_McrdOrMaestro_Success(string cardProduct)
    {
        const string expected = "67";
        Assert.Equal(expected, _helper.GetMessageSourceValueFromCardProduct(cardProduct));
    }

    [Fact]
    public void GetMessageSourceValueFromCardProduct_Visa_Success()
    {
        const string expected = "54";
        Assert.Equal(expected, _helper.GetMessageSourceValueFromCardProduct("VISA"));
    }

    [Fact]
    public void GetMessageSourceValue_ReturnsEmptyStringWhenCardProductAnythingElse()
    {
        const string expected = "";
        Assert.Equal(expected, _helper.GetMessageSourceValueFromCardProduct("ACE"));
    }

    [Fact]
    public void Left_ReturnEmptyStringWhenTextIsNull()
    {
        const string expected = "";
        Assert.Equal(expected, _helper.Left(null,4));
    }
    
    [Fact]
    public void Left_ReturnEmptyStringWhenTextIsEmpty()
    {
        const string expected = "";
        Assert.Equal(expected, _helper.Left(string.Empty,4));
    }

    [Fact]
    public void Left_ReturnTextWhenTextDotLengthIsLessThanLength()
    {
        const string expected = "GPS";
        Assert.Equal(expected, _helper.Left("GPS",4));
    }

    [Fact]
    public void Left_ReturnTheSubStringOfTextBasedOnLength()
    {
        const string expected = "MCRD";
        Assert.Equal(expected, _helper.Left("MCRD-012345",4));
    }

    [Fact]
    public void Right_ReturnEmptyStringWhenTextIsNull()
    {
        const string expected = "";
        Assert.Equal(expected, _helper.Right(null, 4));
    }

    [Fact]
    public void Right_ReturnEmptyStringWhenTextIsEmpty()
    {
        const string expected = "";
        Assert.Equal(expected, _helper.Right(string.Empty, 4));
    }

    [Fact]
    public void Right_ReturnTextWhenTextDotLengthIsLessThanLength()
    {
        const string expected = "GPS";
        Assert.Equal(expected, _helper.Right("GPS", 4));
    }

    [Fact]
    public void Right_ReturnTheSubStringOfTextBasedOnLength()
    {
        const string expected = "2345";
        Assert.Equal(expected, _helper.Right("MCRD-012345", 4));
    }

    [Fact]
    public void ContainsChar()
    {
        bool expected = true;
        Assert.Equal(expected, _helper.ContainsChar("Q", 'Q', 'R', 'S'));

        expected = false;
        Assert.Equal(expected, _helper.ContainsChar("A", 'Q', 'R', 'S'));
    }
}