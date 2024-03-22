using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class IssuerProductLinkCacheServiceTests
{
    private readonly Mock<IReportingCache> _reportingCache;
    private readonly IIssuerProductLinkCacheService _issuerProductLinkService;

    public IssuerProductLinkCacheServiceTests()
    {
        _reportingCache = new Mock<IReportingCache>();
        var issuerProductLinkRepository = new Mock<IIssuerProductLinkRepository>();
        _issuerProductLinkService = new IssuerProductLinkCacheService(issuerProductLinkRepository.Object, _reportingCache.Object);
    }

    [Fact]
    public async Task GetProductIdsForIssuer_Success()
    {
        _reportingCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<IssuerProductLinkEntity>>>>())).ReturnsAsync(new List<IssuerProductLinkEntity>());
        
        await _issuerProductLinkService.GetProductIdsForIssuer(1);

        _reportingCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<IssuerProductLinkEntity>>>>()), Times.Once);
    }
}