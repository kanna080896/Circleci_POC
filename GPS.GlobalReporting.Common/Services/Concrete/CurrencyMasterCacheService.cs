using GPS.GlobalReporting.Common.DatabaseAccess;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Serilog;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class CurrencyMasterCacheService : ICurrencyMasterCacheService
{
    private readonly IReportingCache _reportingCache;
    private readonly ICurrencyMasterRepository _currencyMasterRepository;

    private const string DefaultCurrencyName = "GBP";

    public CurrencyMasterCacheService(IReportingCache reportingCache, ICurrencyMasterRepository currencyMasterRepository)
    {
        _reportingCache = reportingCache;
        _currencyMasterRepository = currencyMasterRepository;
    }

    public async Task<CurrencyMasterModel> GetByCodeOrCNameAsync(string codeOrCName, string fallbackCurrencyCode, string columnName)
    {
        var currencyMasterList = await _reportingCache.GetCache(CacheKeys.CurrencyMaster, () => _currencyMasterRepository.GetAllAsync());
        var currencyMaster = currencyMasterList.FirstOrDefault(x => x.Code == codeOrCName || x.CName == codeOrCName);
        if (currencyMaster != null) return MapEntityToModel(currencyMaster);
        
        Log.Logger.Warning("Currency code/name: {CodeOrCName} from column: {ColumnName} is not valid, using fallback currency code: {FallbackValueCode}", codeOrCName, columnName, fallbackCurrencyCode);
        
        currencyMaster = currencyMasterList.FirstOrDefault(x => x.Code == fallbackCurrencyCode || x.CName == fallbackCurrencyCode);
        
        if (currencyMaster != null) return MapEntityToModel(currencyMaster);
        
        Log.Logger.Warning("Currency code/name: {CodeOrCName} is not valid, using default currency code: {DefaultCurrencyCode}", codeOrCName, DefaultCurrencyName);
        
        currencyMaster = currencyMasterList.FirstOrDefault(x => x.CName == DefaultCurrencyName) ??
                         throw new ArgumentOutOfRangeException(nameof(codeOrCName), "The currency code or name provided was invalid, and both the fallback and default value failed to load");
        
        return MapEntityToModel(currencyMaster);
    }

    public int GetLowerExponent(CurrencyMasterModel currency)
    {
        return new[] { currency.MastercardExponent, currency.VisaExponent }.Min();
    }

    private static CurrencyMasterModel MapEntityToModel(CurrencyMasterEntity currencyMasterEntity)
    {
        return new CurrencyMasterModel
        {
            Code = currencyMasterEntity.Code,
            CName = currencyMasterEntity.CName,
            CcySymbol = currencyMasterEntity.CcySymbol,
            Description = currencyMasterEntity.Description,
            MastercardExponent = currencyMasterEntity.MastercardExponent,
            VisaExponent = currencyMasterEntity.VisaExponent,
            Id = currencyMasterEntity.Id
        };
    }
}