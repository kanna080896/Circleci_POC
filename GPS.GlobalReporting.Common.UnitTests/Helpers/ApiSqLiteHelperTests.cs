using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Helpers.Concrete;
using GPS.GlobalReporting.Common.Helpers.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Helpers;

public class ApiSqLiteHelperTests
{
    private readonly IApiSqLiteHelper _helper;

    public ApiSqLiteHelperTests()
    {
        _helper = new ApiSqLiteHelper();
    }

    [Fact]
    public void GenerateSqLiteDbKey_ReportTypeTransaction_Success()
    {
        var dbKey = _helper.GenerateSqLiteDbKey(ReportTypeEnum.NonClearing);
        Assert.True(dbKey[..11] == "Transaction");
        Assert.True(dbKey.Substring(dbKey.Length - 3, 3) == ".db");
        Assert.True(dbKey.Length > 14);
    }

    [Fact]
    public void GenerateSqLiteDbKey_ReportTypeBalance_Success()
    {
        var dbKey = _helper.GenerateSqLiteDbKey(ReportTypeEnum.Balance);
        Assert.True(dbKey[..7] == "Balance");
        Assert.True(dbKey.Substring(dbKey.Length - 3, 3) == ".db");
        Assert.True(dbKey.Length > 10);
    }

    [Fact]
    public void GenerateSqLiteDbKey_InvalidReportType_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _helper.GenerateSqLiteDbKey((ReportTypeEnum)999));
    }

    [Fact]
    public void DeleteSqLiteDbFile_Success()
    {
        const string fileName = "Test.db";
        var file = File.Create(fileName);
        file.Close();
        Assert.True(File.Exists(fileName));
        _helper.DeleteSqLiteDbFile(fileName);
        Assert.True(!File.Exists(fileName));
    }
}