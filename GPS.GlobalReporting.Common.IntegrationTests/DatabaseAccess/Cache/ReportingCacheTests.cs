using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GPS.GlobalReporting.Common.DatabaseAccess;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Concrete;
using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests.DatabaseAccess.Cache;

public class ReportingCacheTests
{
    private readonly IReportingCache _reportingCache;
    private readonly Mock<ICurrencyMasterRepository> _currencyMasterRepository;

    public ReportingCacheTests()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        var reportingConfig = new ReportingConfiguration(config);
        
        _reportingCache = new ReportingCache(new MemoryCache(new MemoryCacheOptions()), reportingConfig);
        _currencyMasterRepository = new Mock<ICurrencyMasterRepository>();
    }

    [Fact]
    public async Task GetCache_WhenCalledAndCacheIsEmpty_CallsFallbackAndReturnsExpectedList()
    {
        var cacheList = new List<CurrencyMasterEntity>(new []{new CurrencyMasterEntity{Id = 999}});
        _currencyMasterRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cacheList);
        
        var result = await _reportingCache.GetCache(CacheKeys.CurrencyMaster,  () => _currencyMasterRepository.Object.GetAllAsync());

        _currencyMasterRepository.Verify(x => x.GetAllAsync(), Times.Once);
        result.Should().BeEquivalentTo(cacheList);
    }

    [Fact]
    public async Task GetCache_WhenCalledAndCacheIsNotEmpty_ReturnsExpectedList()
    {
        var cacheList = new List<CurrencyMasterEntity>(new []{new CurrencyMasterEntity{Id = 999}});
        _currencyMasterRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cacheList);

        _reportingCache.SetCache(CacheKeys.CurrencyMaster, cacheList);
        var result = await _reportingCache.GetCache(CacheKeys.CurrencyMaster,  () => _currencyMasterRepository.Object.GetAllAsync());

        _currencyMasterRepository.Verify(x => x.GetAllAsync(), Times.Never);
        result.Should().BeEquivalentTo(cacheList);
    }

    [Fact]
    public async Task SetCache_Success()
    {
        var expected = new List<CurrencyMasterEntity> { new() { Id = 1 } };
        _reportingCache.SetCache("Test", expected);

        var result = await _reportingCache.GetCache("Test", () => _currencyMasterRepository.Object.GetAllAsync());
        _currencyMasterRepository.Verify(x => x.GetAllAsync(), Times.Never);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ClearCache_Success()
    {
        _reportingCache.SetCache("Test", new List<CurrencyMasterEntity> { new() { Id = 1 } });
        
        var cacheList = new List<CurrencyMasterEntity>(new []{new CurrencyMasterEntity{Id = 999}});
        _currencyMasterRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cacheList);

        await _reportingCache.GetCache("Test", () => _currencyMasterRepository.Object.GetAllAsync());
        _currencyMasterRepository.Verify(x => x.GetAllAsync(), Times.Never);
        
        _reportingCache.ClearCache("Test");
        var result2 = await _reportingCache.GetCache("Test", () => _currencyMasterRepository.Object.GetAllAsync());
        _currencyMasterRepository.Verify(x => x.GetAllAsync(), Times.Once);

        result2.Should().BeEquivalentTo(cacheList);
    }
}