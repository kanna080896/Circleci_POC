using GPS.GlobalReporting.Common.Services.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests;

public class ConnectionStringServiceTests
{
    private readonly ConnectionStringService _connectionStringService;
    private readonly Mock<IReportingConfiguration> _configuration;
    public ConnectionStringServiceTests()
    {
        _configuration = new Mock<IReportingConfiguration>();
        _connectionStringService = new ConnectionStringService(_configuration.Object);
    }

    [Fact]
    public void GetConnectionString_Success()
    {
        _configuration.Setup(x => x.GetConnectionString(It.IsAny<string>())).Returns("Test");
        var result = _connectionStringService.GetConnectionString("Test");
        _configuration.Verify(x => x.GetConnectionString(It.IsAny<string>()), Times.Once);
        Assert.Equal("Test", result);
    }

    [Fact]
    public void GetCommandTimeout_Success()
    {
        _configuration.Setup(x => x.GetValue<int>(It.IsAny<string>())).Returns(30);
        var result = _connectionStringService.GetCommandTimeout();
        _configuration.Verify(x => x.GetValue<int>(It.IsAny<string>()), Times.Once);
        Assert.Equal(30, result);
    }

    [Fact]
    public void GetSqLiteConnectionString_Success()
    {
        const string dbKey = "Test.db";
        var result = _connectionStringService.GetSqLiteConnectionString(dbKey);
        Assert.Equal("Data Source=Test.db;Pooling=false;", result);
    }
}