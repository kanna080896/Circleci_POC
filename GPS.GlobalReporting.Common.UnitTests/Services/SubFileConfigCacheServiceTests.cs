using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class SubFileConfigCacheServiceTests
{
    private readonly ISubFileConfigCacheService _subFileConfigCacheService;
    private readonly Mock<IReportingCache> _reportingCache;

    public SubFileConfigCacheServiceTests()
    {
        _reportingCache = new Mock<IReportingCache>();
        Mock<ISubFileConfigRepository> subFileConfigRepository = new();
        _subFileConfigCacheService = new SubFileConfigCacheService(_reportingCache.Object, subFileConfigRepository.Object);
    }

    [Fact]
    public async Task GetSubFileConfigList()
    {
        _reportingCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<SubFileConfigEntity>>>>()))
            .ReturnsAsync(GetSubFileConfigs());
        var result = await _subFileConfigCacheService.GetSubFileConfigList();
        Assert.NotNull(result);
        Assert.Equal("CSL", result.Select(x => x.FilePrefix).FirstOrDefault());
    }

    private static List<SubFileConfigEntity> GetSubFileConfigs()
    {
        return new List<SubFileConfigEntity>
        {
            new()
            {
                XmlFileId = 160,
                BankMasterId = 29,
                FilePrefix = "CSL",
                IsIssuer = false,
                ShowPubToken = false
            }
        };
    }
}