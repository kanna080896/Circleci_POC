using GPS.GlobalReporting.Common.Models;

namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface ICurrencyMasterCacheService
{
    Task<CurrencyMasterModel> GetByCodeOrCNameAsync(string codeOrCName, string fallbackCurrencyCode, string columnName);
    int GetLowerExponent(CurrencyMasterModel currency);
}