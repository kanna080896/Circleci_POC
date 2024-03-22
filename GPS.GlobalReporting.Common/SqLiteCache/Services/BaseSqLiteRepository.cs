using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using GPS.GlobalReporting.Common.Constants;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Microsoft.Data.Sqlite;

namespace GPS.GlobalReporting.Common.SqLiteCache.Services;

[ExcludeFromCodeCoverage]
public abstract class BaseSqLiteRepository
{
    private readonly IConnectionStringService _connectionStringService;

    protected BaseSqLiteRepository(IConnectionStringService connectionStringService)
    {
        _connectionStringService = connectionStringService;
    }

    protected string GetSqlConnectionString()
    {
        return _connectionStringService.GetConnectionString(DbKeys.AlexisReplica);
    }

    protected string GetTier2SqlConnectionString()
    {
        return _connectionStringService.GetConnectionString(DbKeys.Tier2);
    }

    protected async Task<List<T>> QueryAsync<T>(string key, string sql, CommandType commandType)
    {
        await using var connection = new SqliteConnection(_connectionStringService.GetSqLiteConnectionString(key));
        var result = await connection.QueryAsync<T>(sql, commandType);

        await connection.CloseAsync();

        return result.ToList();
    }

    protected async Task<List<T>> QueryAsync<T>(string key, string sql, DynamicParameters parameters, CommandType commandType)
    {
        await using var connection = new SqliteConnection(GetSqLiteConnectionString(key));
        var result = await connection.QueryAsync<T>(sql, parameters, commandType: commandType);

        await connection.CloseAsync();

        return result.ToList();
    }

    protected string GetSqLiteConnectionString(string key)
    {
        return _connectionStringService.GetSqLiteConnectionString(key);
    }

    protected async Task<int> ExecuteQueryAsync(string key, string sql, CommandType commandType)
    {
        await using var connection = new SqliteConnection(GetSqLiteConnectionString(key));
        var result = await connection.ExecuteAsync(sql, commandType);

        await connection.CloseAsync();

        return result;
    }

    protected async Task<int> ExecuteQueryAsync(string key, string sql, DynamicParameters parameters, CommandType commandType)
    {
        await using var connection = new SqliteConnection(GetSqLiteConnectionString(key));
        var result = await connection.ExecuteAsync(sql, parameters, commandType: commandType);

        await connection.CloseAsync();

        return result;
    }
}