namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;

public interface ITestRepository
{
    Task<int> GetRecordCountAsync();
    Task<int> GetTier2RecordCountAsync();
}