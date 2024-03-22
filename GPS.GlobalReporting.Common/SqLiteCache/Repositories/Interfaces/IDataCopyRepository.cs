using System.Data;
using System.Data.SqlClient;

namespace GPS.GlobalReporting.Common.SqLiteCache.Repositories.Interfaces;

public interface IDataCopyRepository
{
    Task<int> CopyDataFromAlexis(string dbKey, string sourceSql, SqlParameter[] parameters, CommandType commandType, string tableName, string[] indexes, bool tableExists = false);
    Task<int> CopyDataFromTier2(string dbKey, string sourceSql, SqlParameter[] parameters, CommandType commandType, string tableName, string[] indexes, bool tableExists = false);
}