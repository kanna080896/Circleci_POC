namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IClientProductLinkCacheService
{
    Task<int[]> GetProductIdsForClient(int clientId);
}