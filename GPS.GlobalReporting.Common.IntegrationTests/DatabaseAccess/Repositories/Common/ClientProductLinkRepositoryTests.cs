using System.Threading.Tasks;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.DatabaseAccess.Repositories.Common;

public class ClientProductLinkRepositoryTests
{
    private readonly IClientProductLinkRepository _repository;
    public ClientProductLinkRepositoryTests()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        var configuration = new ReportingConfiguration(config);
        var connectionStringService = new ConnectionStringService(configuration);
        
        _repository = new ClientProductLinkRepository(connectionStringService);
    }
    
    [Fact]
    public async Task GetAllAsync_Success()
    {
        var result = await _repository.GetAllAsync();
        Assert.Equal(2627, result.Count);
    }
}