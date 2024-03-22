namespace GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;

public interface ITempTableHelper
{
    Task<int> CreateAndFillIntListTempTable(string dbKey, IEnumerable<int> items);
    Task<int> CreateAndFillBigIntListTempTable(string dbKey, IEnumerable<long> items);

}