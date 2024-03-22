using System.Collections;
using System.Data;
using System.Text;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;

namespace GPS.GlobalReporting.Common.SqLiteCache.Helpers.Concrete;

public class SqLiteHelper : ISqLiteHelper
{
    public string GetSqlToDropTableIfExists(string tableName)
    {
        return $"DROP TABLE IF EXISTS [{tableName}];";
    }

    public string GetSqlToCreateTableIfNotExists(DataTable schema, string tableName, string[] indexColumns)
    {
        var columns = indexColumns.Length == 0 ? GetSqlToCreateColumns(schema.Rows) : $"{GetSqlToCreateColumns(schema.Rows)}, primary key([{indexColumns[0]}])";
        return $"create table if not exists [{tableName}] ({columns});";
    }

    public string GetSqlToCreateIndexes(string tableName, string[] indexColumns)
    {
        if (!indexColumns.Any()) return string.Empty;
        var builder = new StringBuilder();
        builder.AppendLine($"create unique index [Idx_{tableName}_{indexColumns[0]}] on [{tableName}]([{indexColumns[0]}]);");
        for (var i = 1; i < indexColumns.Length; i++)
        {
            builder.AppendLine($"CREATE INDEX [Idx_{tableName}_{indexColumns[i]}] ON [{tableName}]([{indexColumns[i]}]);");
        }

        return builder.ToString();
    }

    public string GetSqlToInsertRow(int columnCount, string tableName)
    {
        var parameterNames = new List<string>(columnCount);
        for (var i = 0; i < columnCount; i++) parameterNames.Add($"@Parameter{i}");
        return $"INSERT INTO [{tableName}] VALUES ({string.Join(", ", parameterNames)})";
    }

    public List<object> GetObjectArrayFromRow(IDataRecord row)
    {
        var result = new List<object>(row.FieldCount);
        for (var i = 0; i < row.FieldCount; i++)
        {
            if (row.GetFieldType(i).ToString() == "System.String" && !row.IsDBNull(i)) result.Add(row.GetString(i).Trim());
            else result.Add(row.GetValue(i));
        }

        return result;
    }

    private static string GetSqlToCreateColumns(ICollection rows)
    {
        var builder = new List<string>(rows.Count);
        builder.AddRange(from DataRow row in rows select GetSqlToCreateColumn(row));
        return string.Join(", ", builder);
    }

    private static string GetSqlToCreateColumn(DataRow row)
    {
        var realTypes = new[] { "money", "decimal" };
        var columnName = (string)row["ColumnName"];
        var dataTypeName = realTypes.Contains((string)row["DataTypeName"])? "real" : (string)row["DataTypeName"];
        var fieldType = row["DataType"];
        var columnSize =  Convert.ToInt32(row["ColumnSize"].ToString());
        var allowDbNull = Convert.ToBoolean(row["AllowDBNull"].ToString());

        var isTypeString = fieldType.ToString() == "System.String";
        var isTypeByteArray = fieldType.ToString() == "System.Byte[]";
        if (isTypeString) dataTypeName = "nvarchar";

        var builder = new StringBuilder($"[{columnName}] {dataTypeName}");
        if (isTypeString || isTypeByteArray) builder.Append($"({columnSize})");

        if (!allowDbNull) builder.Append(" not null");
        return builder.ToString();
    }
}

public static class CustomExtension
{
    public static string Quote(this string input)
    {
        return $"'{input.Replace("'", "`")}'";
    }
}