using System.Threading.Tasks;
using FluentAssertions;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.DatabaseAccess.Repositories.Common;

public class CurrencyMasterRepositoryTests
{
    private readonly ICurrencyMasterRepository _repository;

    public CurrencyMasterRepositoryTests()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        var configuration = new ReportingConfiguration(config);
        var connectionStringService = new ConnectionStringService(configuration);
        
        _repository = new CurrencyMasterRepository(connectionStringService);
    }
    
    [Fact]
    public async Task GetRecCountAsync_NotNullResult_Success()
    {
        Assert.NotEqual(0, await _repository.GetRecordCountAsync());
    }
    
    [Fact]
    public async Task GetAllAsync_Returns_All()
    {
        var currencyMasterList = await _repository.GetAllAsync();
        var expectedCount = await _repository.GetRecordCountAsync();

        Assert.NotNull(currencyMasterList);
        Assert.Equal(expectedCount, currencyMasterList.Count);
    }
    
    [Fact]
    public async Task GetById_Success()
    {
        var result = await _repository.GetById(389);
        result.Should().BeEquivalentTo(GetExpectedCurrencyMaster());
    }

    private static CurrencyMasterEntity GetExpectedCurrencyMaster()
    {
        return new CurrencyMasterEntity
        {
            Id = 389,
            Code = "826",
            CName = "GBP",
            MastercardExponent = 2,
            CcySymbol = "£",
            VisaExponent = 2,
            Description = string.Empty
        };
    }

}