using System.Data;
using System.Text;
using Dapper;
using GPS.GlobalReporting.Common.Services.Interfaces;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;
using Microsoft.Data.Sqlite;
using Serilog;

namespace GPS.GlobalReporting.Common.SqLiteCache.Helpers.Concrete;

public class TempTableHelper : ITempTableHelper
{
    private readonly IConnectionStringService _connectionStringService;

    public TempTableHelper(IConnectionStringService connectionStringService)
    {
        _connectionStringService = connectionStringService;
    }

    public async Task<int> CreateAndFillIntListTempTable(string dbKey, IEnumerable<int> items)
    {
        Log.Logger.Information("Inserting Ints to temp table");

        await using var connection = new SqliteConnection(_connectionStringService.GetSqLiteConnectionString(dbKey));
        var builder = new StringBuilder();
        builder.AppendLine("drop table if exists [IntList];");
        builder.AppendLine("create table if not exists [IntList] ([Value] int);");
        builder.AppendLine("create unique index [Idx_IntList_Value] on [IntList] ([Value]);");

        await connection.ExecuteAsync(builder.ToString(), CommandType.Text);

        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var rowsInserted = 0;

        foreach (var item in items)
        {
            rowsInserted += await connection.ExecuteAsync("insert into [IntList] values (@Value)", new DynamicParameters(new { Value = item }), commandType: CommandType.Text);
        }

        await transaction.CommitAsync();
        await connection.CloseAsync();

        Log.Logger.Information("Inserted {Count} ints to temp table", rowsInserted);


        return rowsInserted;
    }

    public async Task<int> CreateAndFillBigIntListTempTable(string dbKey, IEnumerable<long> items)
    {
        Log.Logger.Information("Inserting BigInts to temp table");
        await using var connection = new SqliteConnection(_connectionStringService.GetSqLiteConnectionString(dbKey));
        var builder = new StringBuilder();
        builder.AppendLine("drop table if exists [BigIntList];");
        builder.AppendLine("create table if not exists [BigIntList] ([Value] bigint);");
        builder.AppendLine("create unique index [Idx_BigIntList_Value] on [BigIntList] ([Value]);");

        await connection.ExecuteAsync(builder.ToString(), CommandType.Text);

        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();

        var rowsInserted = 0;

        foreach (var item in items)
        {
            rowsInserted += await connection.ExecuteAsync("insert into [BigIntList] values (@Value)", new DynamicParameters(new { Value = item }), commandType: CommandType.Text);
        }

        await transaction.CommitAsync();
        await connection.CloseAsync();

        Log.Logger.Information("Inserted {Count} Bigints to temp table", rowsInserted);

        return rowsInserted;
    }
}