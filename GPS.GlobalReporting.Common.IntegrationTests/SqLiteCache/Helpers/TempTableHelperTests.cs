using System.Threading.Tasks;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Concrete;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.SqLiteCache.Helpers;

public class TempTableHelperTests : SqLiteCacheTestBase
{
    private readonly ITempTableHelper _tempTableHelper;
    public TempTableHelperTests(SqliteCacheTestFixture fixture) : base(fixture)
    {
        var connectionStringService = GetMockConnectionStringService();
        _tempTableHelper = new TempTableHelper(connectionStringService.Object);
    }

    [Fact]
    public async Task CreateAndFillIntListTempTable_Success()
    {
        var input = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var result = await _tempTableHelper.CreateAndFillIntListTempTable(DbKey, input);
        
        Assert.Equal(10,result);
    }
    
    [Fact]
    public async Task CreateAndFillBigIntListTempTable_Success()
    {
        var input = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var result = await _tempTableHelper.CreateAndFillBigIntListTempTable(DbKey, input);
        
        Assert.Equal(10,result);
    }
}