using System.Data.SqlClient;
using System.Text;
using Dapper;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;

public class SubFileConfigRepository : RepositoryBase, ISubFileConfigRepository
{
    public SubFileConfigRepository(IConnectionStringService connectionStringService) : base(connectionStringService)
    {
    }

    public async Task<List<SubFileConfigEntity>> GetAllAsync()
    {
        var sql = new StringBuilder(GetPartialQueryString());

        await using var connection = new SqlConnection(GetAlexisConnectionString());
        var result = await connection.QueryAsync<SubFileConfigEntity>(sql.ToString());

        return result.ToList();
    }

    private static string GetPartialQueryString()
    {
        var sql = new StringBuilder();
        sql.AppendLine("SELECT ID AS XmlFileId, MasterId AS BankMasterId, ClientOrIssuer AS IsIssuer, FilePrefix, ISNULL(ShowPubToken, 0) AS ShowPubToken");
        sql.AppendLine("FROM dbo.XMLFile WITH(NOLOCK)");
        sql.AppendLine("WHERE ClientOrIssuer = 0 and LEN(FilePrefix) > 0");
        return sql.ToString();
    }
}


    
