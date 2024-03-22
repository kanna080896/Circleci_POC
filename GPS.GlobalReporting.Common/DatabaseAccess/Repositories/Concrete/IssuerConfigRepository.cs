using System.Data.SqlClient;
using System.Text;
using Dapper;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;

public class IssuerConfigRepository : RepositoryBase, IIssuerConfigRepository
{
    public IssuerConfigRepository(IConnectionStringService connectionStringService) : base(connectionStringService)
    {
    }
    
    public async Task<List<IssuerConfigEntity>> GetAllAsync()
    {
        var sql = new StringBuilder(GetPartialQueryString());

        await using var connection = new SqlConnection(GetAlexisConnectionString());
        var result = await connection.QueryAsync<IssuerConfigEntity>(sql.ToString());

        return result.ToList();
    }    

    private static string GetPartialQueryString()
    {
        var sql = new StringBuilder();
        sql.AppendLine("SELECT MasterId AS BankMasterId, XF.ClientOrIssuer IsIssuer, BM.XmlFilePrefix, ISNULL(XF.ShowPubToken, 0) AS ShowPubToken, ISNULL(BM.GlobalReportingXMLType, 0) as XMLType");
        sql.AppendLine("FROM dbo.BankMaster BM WITH (NOLOCK)");
        sql.AppendLine("INNER JOIN dbo.XMLFile XF WITH(NOLOCK) ON BM.BankMasterId = XF.MasterID");
        sql.AppendLine("Where ClientOrIssuer = 1 and len(XmlFilePrefix) > 0 ");
        return sql.ToString();
    }
}