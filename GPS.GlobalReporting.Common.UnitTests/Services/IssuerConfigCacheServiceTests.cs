using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class IssuerConfigCacheServiceTests
{
    private readonly IIssuerConfigCacheService _issuerConfigCacheService;
    private readonly Mock<IReportingCache> _reportingCache;

    public IssuerConfigCacheServiceTests()
    {
        _reportingCache = new Mock<IReportingCache>();
        Mock<IIssuerConfigRepository> issuerConfigRepository = new();
        _issuerConfigCacheService = new IssuerConfigCacheService(_reportingCache.Object, issuerConfigRepository.Object);
    }

    [Fact]
    public async Task GetIssuerConfigList()
    {
        _reportingCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<IssuerConfigEntity>>>>()))
            .ReturnsAsync(GetIssuerConfigs());
        var result = await _issuerConfigCacheService.GetIssuerConfigList();
        Assert.NotNull(result);
        Assert.Equal("TST", result.Select(x => x.XmlFilePrefix).FirstOrDefault());
    }

    private static List<IssuerConfigEntity> GetIssuerConfigs()
    {
        return new List<IssuerConfigEntity>
        {
            new()
            {
                BankMasterId = 29,
                XmlFilePrefix = "TST",
                ShowPubToken = true,
                IsIssuer = true,
                XmlType = GlobalReportingXmlTypeEnum.TransactionOnly
            }
        };
    }
}