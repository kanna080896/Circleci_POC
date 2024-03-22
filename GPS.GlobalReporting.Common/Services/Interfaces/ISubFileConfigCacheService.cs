using GPS.GlobalReporting.Common.Models;

namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface ISubFileConfigCacheService
{
    Task<List<SubFileConfigModel>> GetSubFileConfigList();
}