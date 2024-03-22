using GPS.GlobalReporting.Common.Constants;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories;

public abstract class RepositoryBase
{
    private readonly IConnectionStringService _connectionStringService;

    protected RepositoryBase(IConnectionStringService connectionStringService)
    {
        _connectionStringService = connectionStringService;
    }

    public virtual string GetAlexisConnectionString()
    {
        return _connectionStringService.GetConnectionString(DbKeys.AlexisReplica);
    }

    public virtual string GetTier2ConnectionString()
    {
        return _connectionStringService.GetConnectionString(DbKeys.Tier2);
    }

    public virtual int GetCommandTimeout()
    {
        return _connectionStringService.GetCommandTimeout();
    }
}