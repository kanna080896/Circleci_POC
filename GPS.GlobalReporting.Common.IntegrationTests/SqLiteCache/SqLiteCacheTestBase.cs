using GPS.GlobalReporting.Common.Constants;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.SqLiteCache;

[TestCaseOrderer("GPS.GlobalReporting.SqliteCache.IntegrationTests.PriorityOrderer","GPS.GlobalReporting.SqliteCache.IntegrationTests")]
public abstract class SqLiteCacheTestBase : IClassFixture<SqliteCacheTestFixture>
{
    protected SqLiteCacheTestBase(SqliteCacheTestFixture fixture)
    {
        fixture.FileName = DbKey;
    }
    protected string DbKey => $"{GetType().Name}.db";

    protected string GetSqLiteConnectionString()
    {
        return $"Data Source={DbKey};Pooling=false;";
    }

    protected Mock<IConnectionStringService> GetMockConnectionStringService()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        var connectionStringService = new Mock<IConnectionStringService>();

        connectionStringService.Setup(x => x.GetSqLiteConnectionString(It.Is<string>(z => z == DbKey))).Returns(GetSqLiteConnectionString());
        connectionStringService.Setup(x => x.GetConnectionString(It.Is<string>(z => z.Contains("Alexis")))).Returns(config.GetConnectionString(DbKeys.AlexisReplica));
        connectionStringService.Setup(x => x.GetConnectionString(It.Is<string>(z => z.Equals("Tier2")))).Returns(config.GetConnectionString(DbKeys.Tier2));
        connectionStringService.Setup(x => x.GetSqLiteConnectionString(It.Is<string>(z => z == DbKeys.TransactionSqLite))).Returns(GetSqLiteConnectionString());
        connectionStringService.Setup(x => x.GetSqLiteConnectionString(It.Is<string>(z => z == DbKeys.BalanceSqLite))).Returns(GetSqLiteConnectionString());
        connectionStringService.Setup(x => x.GetCommandTimeout()).Returns(400);
        
        return connectionStringService;
    }
}