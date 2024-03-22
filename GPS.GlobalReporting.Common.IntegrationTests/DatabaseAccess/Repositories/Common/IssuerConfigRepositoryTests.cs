using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.DatabaseAccess.Repositories.Common;

public class IssuerConfigRepositoryTests
{
    private readonly IIssuerConfigRepository _repository;
    
    public IssuerConfigRepositoryTests()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        var configuration = new ReportingConfiguration(config);
        var connectionStringService = new ConnectionStringService(configuration);
        
        _repository = new IssuerConfigRepository(connectionStringService);
    }     
    
    [Fact]
    public async Task GetAllAsync_Returns_All()
    {
        var list = await _repository.GetAllAsync();        

        Assert.NotNull(list);
        Assert.Equal(9, list.Count);
    }
    
    [Fact]
    public async Task GetById_Success()
    {
        var all = await _repository.GetAllAsync();
        var result = all.FirstOrDefault(x => x.BankMasterId == 29);
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(GetExpectedIssuerConfig());       
        foreach (var item in all) { Assert.True(item.IsIssuer); }
        foreach (var item in all) { Assert.True(item.XmlFilePrefix.Length > 0); }
    }

    private static IssuerConfigEntity GetExpectedIssuerConfig()
    {
        return new IssuerConfigEntity
        {            
            BankMasterId = 29,
            XmlFilePrefix = "TST",
            ShowPubToken = true,
            IsIssuer = true,
            XmlType = GlobalReportingXmlTypeEnum.TransactionOnly
        };
    }
}