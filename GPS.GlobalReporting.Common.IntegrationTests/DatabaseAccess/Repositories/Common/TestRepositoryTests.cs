using System.Threading.Tasks;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.DatabaseAccess.Repositories.Common
{
    public class TestRepositoryTests
    {
        private readonly ITestRepository _repository;

        public TestRepositoryTests()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
            var configuration = new ReportingConfiguration(config);
            var connectionStringService = new ConnectionStringService(configuration);
            
            _repository = new TestRepository(connectionStringService);
        }

        [Fact]
        public async Task GetRecCountAsync_NotNullResult_Success()
        {
            Assert.Equal(1, await _repository.GetRecordCountAsync());
        }

        [Fact]
        public async Task GetTier2RecCountAsync_NotNullResult_Success()
        {
            Assert.Equal(1, await _repository.GetTier2RecordCountAsync());
        }
    }
}
