namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IIssuerProductLinkCacheService
{
    Task<int[]> GetProductIdsForIssuer(int issuerId);
}