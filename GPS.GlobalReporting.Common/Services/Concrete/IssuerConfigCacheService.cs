using GPS.GlobalReporting.Common.DatabaseAccess;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class IssuerConfigCacheService : IIssuerConfigCacheService
{
    private readonly IReportingCache _reportingCache;
    private readonly IIssuerConfigRepository _issuerConfigRepository;

    public IssuerConfigCacheService(IReportingCache reportingCache, IIssuerConfigRepository issuerConfigRepository)
    {
        _issuerConfigRepository = issuerConfigRepository;
        _reportingCache = reportingCache;
    }

    public async Task<List<IssuerConfigModel>> GetIssuerConfigList()
    {
        var issuerConfigList =
            await _reportingCache.GetCache(CacheKeys.IssuerConfig, () => _issuerConfigRepository.GetAllAsync());

        return issuerConfigList
            .Select(MapIssuerConfig)
            .Select(x => x)
            .ToList();
    }

    private static IssuerConfigModel MapIssuerConfig(IssuerConfigEntity issuerConfigEntity)
    {
        return new IssuerConfigModel
        {
            BankMasterId = issuerConfigEntity.BankMasterId,
            XmlFilePrefix = issuerConfigEntity.XmlFilePrefix,
            ShowPubToken = issuerConfigEntity.ShowPubToken,
            XmlType = issuerConfigEntity.XmlType
        };
    }
}