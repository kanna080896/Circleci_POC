using GPS.GlobalReporting.Common.DatabaseAccess.Entities;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;

public interface IIssuerConfigRepository
{   
    Task<List<IssuerConfigEntity>> GetAllAsync();    
}