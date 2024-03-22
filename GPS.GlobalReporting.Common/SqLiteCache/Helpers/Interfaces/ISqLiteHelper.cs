using System.Data;

namespace GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;

public interface ISqLiteHelper
{
    string GetSqlToDropTableIfExists(string tableName);
    string GetSqlToCreateTableIfNotExists(DataTable schema, string tableName, string[] indexColumns);
    string GetSqlToCreateIndexes(string tableName, string[] indexColumns);
    string GetSqlToInsertRow(int columnCount, string tableName);
    List<object> GetObjectArrayFromRow(IDataRecord row);
    
}