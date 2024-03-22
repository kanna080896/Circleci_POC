using GPS.GlobalReporting.Common.DatabaseAccess.Entities;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;

public interface ICurrencyMasterRepository
{
    Task<int> GetRecordCountAsync();
    Task<List<CurrencyMasterEntity>> GetAllAsync();
    Task<CurrencyMasterEntity> GetById(int currencyMasterId);
}