using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Concrete;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.DatabaseAccess.Repositories.Common;

public class SubFileConfigRepositoryTests
{
    private readonly ISubFileConfigRepository _repository;
    
    public SubFileConfigRepositoryTests()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        var configuration = new ReportingConfiguration(config);
        var connectionStringService = new ConnectionStringService(configuration);
        
        _repository = new SubFileConfigRepository(connectionStringService);
    }     
    
    [Fact]
    public async Task GetAllAsync_Returns_All()
    {
        var list = await _repository.GetAllAsync();        

        Assert.NotNull(list);
        Assert.Equal(180, list.Count);
    }
    
    [Fact]
    public async Task GetById_Success()
    {
        var all = await _repository.GetAllAsync();
        var result = all.Where(x => x.BankMasterId == 29).ToList();
        Assert.Equal(6, result.Count);
        result.First(x => x.XmlFileId == 160).Should().BeEquivalentTo(GetSubFileConfig());
        foreach (var item in all) { Assert.False(item.IsIssuer); }
        foreach (var item in all) { Assert.True(item.FilePrefix.Length > 0); }
    }

    private static SubFileConfigEntity GetSubFileConfig()
    {
        return new SubFileConfigEntity
        {
            XmlFileId = 160,
            BankMasterId = 29,
            FilePrefix = "MOP",
            IsIssuer = false,
            ShowPubToken = true
        };
    }
}