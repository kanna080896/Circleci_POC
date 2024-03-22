using System.Data.SqlClient;
using System.Text;
using Dapper;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;

public class IssuerProductLinkRepository : RepositoryBase, IIssuerProductLinkRepository
{
    public IssuerProductLinkRepository(IConnectionStringService connectionStringService) : base(connectionStringService)
    {
    }

    public async Task<List<IssuerProductLinkEntity>> GetAllAsync()
    {
        var sql = new StringBuilder(GetPartialQueryString());
        await using var connection = new SqlConnection(GetAlexisConnectionString());
        var result = await connection.QueryAsync<IssuerProductLinkEntity>(sql.ToString());

        return result.ToList();
    }

    private static string GetPartialQueryString()
    {
        var sql = new StringBuilder();
        sql.AppendLine("SELECT PT.ProductID,PT.Product,PT.SubBinHigh,PT.SubBinLow,BM.BankMasterId as [IssuerId],BM.BankMasterName,BM.XmlFilePrefix,BM.XMLType");
        sql.AppendLine("FROM dbo.[ProductType] AS PT WITH (NOLOCK)");
        sql.AppendLine("INNER JOIN dbo.SchemeMaster AS SM WITH (NOLOCK) ON PT.SchemeID = SM.SchemeID");
        sql.AppendLine("INNER JOIN dbo.InstitutionMaster I(NOLOCK) ON SM.institution_id = I.institutionid");
        sql.AppendLine("INNER JOIN dbo.ProgMgrMaster  AS PM WITH (NOLOCK) ON I.ProgMgrId = PM.ProgramID");
        sql.AppendLine("INNER JOIN dbo.Bank AS B WITH (NOLOCK) ON PT.BankID = B.BankID");
        sql.AppendLine("INNER JOIN dbo.BankMaster AS BM WITH (NOLOCK) ON B.BankMasterID = BM.BankMasterId");

        return sql.ToString();
    }
}