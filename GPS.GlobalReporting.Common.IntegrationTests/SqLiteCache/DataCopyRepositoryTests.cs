using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GPS.GlobalReporting.Common.Constants;
using GPS.GlobalReporting.Common.SqLiteCache;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Concrete;
using GPS.GlobalReporting.Common.SqLiteCache.Repositories.Concrete;
using GPS.GlobalReporting.Common.SqLiteCache.Repositories.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.SqLiteCache;

public class DataCopyRepositoryTests : SqLiteCacheTestBase
{
    private readonly IDataCopyRepository _dataCopyRepository;

    public DataCopyRepositoryTests(SqliteCacheTestFixture fixture) : base(fixture)
    {
        var connectionStringService = GetMockConnectionStringService();
        _dataCopyRepository = new DataCopyRepository(connectionStringService.Object, new SqLiteHelper());
    }

    [Fact]
    public async Task CopyData_Success()
    {
        var result = await _dataCopyRepository.CopyDataFromAlexis(DbKeys.TransactionSqLite, "SELECT * FROM [AlexisPtsTest].[dbo].[ProductType]", Array.Empty<SqlParameter>(), CommandType.Text, "Transaction1",
            new[] { "ProductID" });

        Assert.Equal(3837, result);
    }

    [Fact]
    public async Task CopyData_IfDbKeyEmpty_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _dataCopyRepository.CopyDataFromAlexis(string.Empty, "SELECT * FROM [AlexisPtsTest].[dbo].[ProductType]", Array.Empty<SqlParameter>(),
            CommandType.Text,
            "ProductType",
            new[] { "ProductID" }));
    }

    [Fact]
    public async Task CopyData_IfSourceSqlEmpty_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _dataCopyRepository.CopyDataFromAlexis(DbKeys.TransactionSqLite, string.Empty, Array.Empty<SqlParameter>(), CommandType.Text,
            "ProductType",
            new[] { "ProductID" }));
    }

    [Fact]
    public async Task CopyData_IfTableNameEmpty_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _dataCopyRepository.CopyDataFromAlexis(DbKeys.TransactionSqLite, "SELECT * FROM [AlexisPtsTest].[dbo].[ProductType]", Array.Empty<SqlParameter>(),
            CommandType.Text,
            string.Empty,
            new[] { "ProductID" }));
    }

    [Fact]
    public async Task CopyData_IfIndexesEmpty_ThrowsException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _dataCopyRepository.CopyDataFromAlexis(DbKeys.TransactionSqLite, "SELECT * FROM [AlexisPtsTest].[dbo].[ProductType]", Array.Empty<SqlParameter>(),
            CommandType.Text,
            "ProductType",
            Array.Empty<string>()));
    }
}