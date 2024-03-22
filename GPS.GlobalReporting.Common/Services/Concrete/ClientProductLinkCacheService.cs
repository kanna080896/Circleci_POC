using GPS.GlobalReporting.Common.DatabaseAccess;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class ClientProductLinkCacheService : IClientProductLinkCacheService
{
    private readonly IClientProductLinkRepository _clientProductLinkRepository;
    private readonly IReportingCache _reportingCache;

    public ClientProductLinkCacheService(IClientProductLinkRepository clientProductLinkRepository, IReportingCache reportingCache)
    {
        _clientProductLinkRepository = clientProductLinkRepository;
        _reportingCache = reportingCache;
    }

    public async Task<int[]> GetProductIdsForClient(int clientId)
    {
        var result = await _reportingCache.GetCache(CacheKeys.ClientProductLink, () => _clientProductLinkRepository.GetAllAsync());
        return result.Where(x => x.ClientId == clientId).Select(x => x.ProductId).ToArray();
    }
}