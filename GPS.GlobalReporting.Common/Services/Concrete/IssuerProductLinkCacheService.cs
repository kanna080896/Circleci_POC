using GPS.GlobalReporting.Common.DatabaseAccess;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class IssuerProductLinkCacheService : IIssuerProductLinkCacheService
{
    private readonly IIssuerProductLinkRepository _issuerProductLinkRepository;
    private readonly IReportingCache _reportingCache;

    public IssuerProductLinkCacheService(IIssuerProductLinkRepository issuerProductLinkRepository, IReportingCache reportingCache)
    {
        _issuerProductLinkRepository = issuerProductLinkRepository;
        _reportingCache = reportingCache;
    }

    public async Task<int[]> GetProductIdsForIssuer(int issuerId)
    {
        var result = await _reportingCache.GetCache(CacheKeys.IssuerProductLink, () => _issuerProductLinkRepository.GetAllAsync());
        return result.Where(x => x.IssuerId == issuerId).Select(x => x.ProductId).ToArray();
    }
}