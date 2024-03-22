using System.Data.SqlClient;
using System.Text;
using Dapper;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;

public class ClientProductLinkRepository : RepositoryBase, IClientProductLinkRepository
{
    public ClientProductLinkRepository(IConnectionStringService connectionStringService) : base(connectionStringService)
    {
    }

    public async Task<List<ClientProductEntity>> GetAllAsync()
    {
        var sql = new StringBuilder(GetPartialQueryString());
        await using var connection = new SqlConnection(GetAlexisConnectionString());
        var result = await connection.QueryAsync<ClientProductEntity>(sql.ToString());

        return result.ToList();
    }

    private static string GetPartialQueryString()
    {
        var sql = new StringBuilder();

        sql.AppendLine("SELECT PT.ProductID, I.ProgMgrId as [ClientId],PT.Product,PT.SubBinHigh,PT.SubBinLow,PM.ProgMgrCode");
        sql.AppendLine("FROM dbo.[ProductType] AS PT WITH (NOLOCK)");
        sql.AppendLine("INNER JOIN dbo.SchemeMaster AS SM WITH (NOLOCK) ON PT.SchemeID = SM.SchemeID");
        sql.AppendLine("INNER JOIN dbo.InstitutionMaster I(NOLOCK) ON SM.institution_id = I.institutionid");
        sql.AppendLine("INNER JOIN dbo.ProgMgrMaster  AS PM WITH (NOLOCK) ON I.ProgMgrId = PM.ProgramID");

        return sql.ToString();
    }
}