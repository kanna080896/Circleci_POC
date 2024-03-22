using System.Data;
using System.Data.SqlClient;
using GPS.GlobalReporting.Common.Services.Interfaces;
using GPS.GlobalReporting.Common.SqLiteCache.Helpers.Interfaces;
using GPS.GlobalReporting.Common.SqLiteCache.Repositories.Interfaces;
using GPS.GlobalReporting.Common.SqLiteCache.Services;
using Microsoft.Data.Sqlite;
using Serilog;

namespace GPS.GlobalReporting.Common.SqLiteCache.Repositories.Concrete;

public class DataCopyRepository : BaseSqLiteRepository, IDataCopyRepository
{
    private readonly IConnectionStringService _connectionStringService;
    private readonly ISqLiteHelper _helper;

    public DataCopyRepository(IConnectionStringService connectionStringService, ISqLiteHelper helper) : base(connectionStringService)
    {
        _connectionStringService = connectionStringService;
        _helper = helper;
    }

    public async Task<int> CopyDataFromAlexis(string dbKey, string sourceSql, SqlParameter[] parameters, CommandType commandType, string tableName, string[] indexes, bool tableExists = false)
    {
        if (string.IsNullOrEmpty(dbKey)) throw new ArgumentNullException(nameof(dbKey));
        if (string.IsNullOrEmpty(sourceSql)) throw new ArgumentNullException(nameof(sourceSql));
        if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));
        if (!indexes.Any() && !tableExists) throw new ArgumentNullException(nameof(indexes));

        var connectionString = GetSqlConnectionString();
        return await CopyData(dbKey, sourceSql, parameters, commandType, tableName, indexes, connectionString, tableExists);
    }

    public async Task<int> CopyDataFromTier2(string dbKey, string sourceSql, SqlParameter[] parameters, CommandType commandType, string tableName, string[] indexes, bool tableExists = false)
    {
        if (string.IsNullOrEmpty(dbKey)) throw new ArgumentNullException(nameof(dbKey));
        if (string.IsNullOrEmpty(sourceSql)) throw new ArgumentNullException(nameof(sourceSql));
        if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));
        if (!indexes.Any() && !tableExists) throw new ArgumentNullException(nameof(indexes));

        var connectionString = GetTier2SqlConnectionString();
        return await CopyData(dbKey, sourceSql, parameters, commandType, tableName, indexes, connectionString, tableExists);
    }

    private async Task<int> CopyData(string dbKey, string sourceSql, SqlParameter[] parameters, CommandType commandType, string tableName, string[] indexes, string connectionString,
        bool tableExists = false)
    {
        Log.Logger.Information("Copying data to table: {TableName}", tableName);
        await using var sqlConnection = new SqlConnection(connectionString);
        await using var sqlCommand = new SqlCommand(sourceSql, sqlConnection) { CommandType = commandType, CommandTimeout = _connectionStringService.GetCommandTimeout() };
        await sqlConnection.OpenAsync();
        sqlCommand.Parameters.AddRange(parameters);
        await using var sqlReader = await sqlCommand.ExecuteReaderAsync();

        var schema = await sqlReader.GetSchemaTableAsync();

        await using var sqlLiteConnection = new SqliteConnection(GetSqLiteConnectionString(dbKey));

        if (!tableExists)
        {
            await DropTableIfExists(tableName, sqlLiteConnection);

            await CreateTableIfNotExists(schema!, tableName, indexes, sqlLiteConnection);

            await CreateIndexes(tableName, indexes, sqlLiteConnection);
        }

        if (sqlLiteConnection.State != ConnectionState.Open)
        {
            await sqlLiteConnection.OpenAsync();
        }

        var counter = 0;
        var insertSql = _helper.GetSqlToInsertRow(schema!.Rows.Count, tableName);
        await using var transaction = sqlLiteConnection.BeginTransaction();
        while (await sqlReader.ReadAsync()) counter += await ReadAndInsertWithParameters(sqlReader, insertSql, sqlLiteConnection, transaction);
        await transaction.CommitAsync();

        await sqlConnection.CloseAsync();
        await sqlLiteConnection.CloseAsync();
        
        Log.Logger.Information("Successfully copied {RowCount} rows to table: {Table}", counter, tableName);
        return counter;
    }

    private async Task DropTableIfExists(string tableName, SqliteConnection connection)
    {
        var sql = _helper.GetSqlToDropTableIfExists(tableName);
        await using var command = new SqliteCommand(sql, connection) { CommandType = CommandType.Text };
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    private async Task CreateTableIfNotExists(DataTable schema, string tableName, string[] indexColumns, SqliteConnection connection)
    {
        var sql = _helper.GetSqlToCreateTableIfNotExists(schema, tableName, indexColumns);
        await using var command = new SqliteCommand(sql, connection) { CommandType = CommandType.Text };
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    private async Task CreateIndexes(string tableName, string[] indexColumns, SqliteConnection connection)
    {
        var sql = _helper.GetSqlToCreateIndexes(tableName, indexColumns);
        await using var command = new SqliteCommand(sql, connection) { CommandType = CommandType.Text };
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    private async Task<int> ReadAndInsertWithParameters(IDataRecord reader, string sql, SqliteConnection connection, SqliteTransaction transaction)
    {
        var values = _helper.GetObjectArrayFromRow(reader);
        var tmpParameters = values.Select((x, y) => new SqliteParameter($"@Parameter{y}", x));
        await using var command = new SqliteCommand(sql, connection, transaction) { CommandType = CommandType.Text };
        command.Parameters.AddRange(tmpParameters);
        return await command.ExecuteNonQueryAsync();
    }
}