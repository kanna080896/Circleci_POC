using System.Data;
using System.Data.SqlClient;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete
{
    public class TestRepository : RepositoryBase, ITestRepository
    {
        const string Sql = " select count(GetUTCDate())";
        public TestRepository(IConnectionStringService connectionStringService) : base(connectionStringService)
        {
        }

        public async Task<int> GetRecordCountAsync()
        {
            return await GetRecordCountAsync(Sql, GetAlexisConnectionString());
        }

        public async Task<int> GetTier2RecordCountAsync()
        {
            return await GetRecordCountAsync(Sql, GetTier2ConnectionString());
        }

        private async Task<int> GetRecordCountAsync(string sql, string getConnectionString)
        {
            int result;
            await using var connection = new SqlConnection(getConnectionString);
            await using var command = new SqlCommand(sql, connection) { CommandType = CommandType.Text };
            await connection.OpenAsync();
            await using (var reader = await command.ExecuteReaderAsync())
            {
                await reader.ReadAsync(); // there will always be a record to read
                result = reader.GetInt32(0);
            }

            await connection.CloseAsync();

            return result;
        }

    }
}