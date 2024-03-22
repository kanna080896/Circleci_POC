using GPS.GlobalReporting.Common.DatabaseAccess.Entities;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;

public interface ISubFileConfigRepository
{
    Task<List<SubFileConfigEntity>> GetAllAsync();
}