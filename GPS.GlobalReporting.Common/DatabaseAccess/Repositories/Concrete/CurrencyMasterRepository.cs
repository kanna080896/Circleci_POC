using System.Data.SqlClient;
using System.Text;
using Dapper;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;

public class CurrencyMasterRepository : RepositoryBase, ICurrencyMasterRepository
{
    public CurrencyMasterRepository(IConnectionStringService connectionStringService) : base(connectionStringService)
    {
    }

    public async Task<int> GetRecordCountAsync()
    {
        const string sql = "select count(*) as RecCount from [dbo].[CurrencyMaster]";

        await using var connection = new SqlConnection(GetAlexisConnectionString());
        return await connection.QuerySingleAsync<int>(sql, null, null, GetCommandTimeout());
    }

    public async Task<List<CurrencyMasterEntity>> GetAllAsync()
    {
        var sql = new StringBuilder(GetPartialQueryString());

        await using var connection = new SqlConnection(GetAlexisConnectionString());
        var result = await connection.QueryAsync<CurrencyMasterEntity>(sql.ToString());

        return result.ToList();
    }

    public async Task<CurrencyMasterEntity> GetById(int currencyMasterId)
    {
        var sql = new StringBuilder(GetPartialQueryString());
        sql.AppendLine("WHERE [ID] = @CurrencyMasterId");

        await using var connection = new SqlConnection(GetAlexisConnectionString());
        return await connection.QuerySingleAsync<CurrencyMasterEntity>(sql.ToString(), new { CurrencyMasterId = currencyMasterId }, null, GetCommandTimeout());
    }

    private static string GetPartialQueryString()
    {
        var sql = new StringBuilder();
        sql.AppendLine(
            "select [ID] as Id, RTRIM(LTRIM([Code])) as Code, RTRIM(LTRIM([CName])) as CName, [Exponent] as MastercardExponent, RTRIM(LTRIM([Ccy_Symbol])) as CcySymbol, [VisaExponent], RTRIM(LTRIM([Description])) as Description");
        sql.AppendLine("from [dbo].[CurrencyMaster]");
        return sql.ToString();
    }
}