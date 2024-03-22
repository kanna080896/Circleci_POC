using GPS.GlobalReporting.Common.Models;

namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IIssuerConfigCacheService
{
    Task<List<IssuerConfigModel>> GetIssuerConfigList();
}