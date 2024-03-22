using GPS.GlobalReporting.Common.DatabaseAccess.Entities;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;

public interface IIssuerProductLinkRepository
{
    Task<List<IssuerProductLinkEntity>> GetAllAsync();
}

