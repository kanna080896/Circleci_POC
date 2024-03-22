using System.Data;
using System.Data.SqlClient;
using GPS.GlobalReporting.Common.SqLiteCache.Repositories.Interfaces;

namespace GPS.GlobalReporting.Common.SqLiteCache.Services;

public abstract class BaseCopyService
{
    protected readonly IDataCopyRepository Repository;

    protected BaseCopyService(IDataCopyRepository repository)
    {
        Repository = repository;
    }
    protected static SqlParameter GetIntListParameter(List<int> intList)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(int));
        foreach (var item in intList) table.Rows.Add(item);

        return new SqlParameter
        {
            ParameterName = "IdList",
            SqlDbType = SqlDbType.Structured,
            Value = table,
            TypeName = "GlobalReporting.IntList"
        };
    }

    protected static SqlParameter GetBigIntListParameter(List<long> longList, string parameterName)
    {
        var table = new DataTable();
        table.Columns.Add("Id", typeof(long));
        foreach (var item in longList) table.Rows.Add(item);

        return new SqlParameter
        {
            ParameterName = parameterName,
            SqlDbType = SqlDbType.Structured,
            Value = table,
            TypeName = "GlobalReporting.BigIntList"
        };
    }
}