using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class ClientProductLinkCacheServiceTests
{
    private readonly Mock<IReportingCache> _reportingCache;
    private readonly IClientProductLinkCacheService _clientProductLinkService;

    public ClientProductLinkCacheServiceTests()
    {
        _reportingCache = new Mock<IReportingCache>();
        var clientProductLinkRepository = new Mock<IClientProductLinkRepository>();

        _clientProductLinkService = new ClientProductLinkCacheService(clientProductLinkRepository.Object, _reportingCache.Object);
    }

    [Fact]
    public async Task GetProductIdsForIssuer_Success()
    {
        _reportingCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<ClientProductEntity>>>>())).ReturnsAsync(new List<ClientProductEntity>());

        await _clientProductLinkService.GetProductIdsForClient(1);

        _reportingCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<ClientProductEntity>>>>()), Times.Once);
    }
}