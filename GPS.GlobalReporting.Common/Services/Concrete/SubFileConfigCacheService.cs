using GPS.GlobalReporting.Common.DatabaseAccess;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class SubFileConfigCacheService : ISubFileConfigCacheService
{
    private readonly IReportingCache _reportingCache;
    private readonly ISubFileConfigRepository _subFileConfigRepository;

    public SubFileConfigCacheService(IReportingCache reportingCache, ISubFileConfigRepository subFileConfigRepository)
    {
        _subFileConfigRepository = subFileConfigRepository;
        _reportingCache = reportingCache;
    }

    public async Task<List<SubFileConfigModel>> GetSubFileConfigList()
    {
        var subFileConfigList =
            await _reportingCache.GetCache(CacheKeys.SubFileConfig, () => _subFileConfigRepository.GetAllAsync());

        return subFileConfigList
            .Select(MapSubFileConfig)
            .Select(x => x)
            .ToList();
    }

    private static SubFileConfigModel MapSubFileConfig(SubFileConfigEntity subFileConfigEntity)
    {
        return new SubFileConfigModel
        {
            FilePrefix = subFileConfigEntity.FilePrefix,
            ShowPubToken = subFileConfigEntity.ShowPubToken,
            BankMasterId = subFileConfigEntity.BankMasterId
        };
    }
}