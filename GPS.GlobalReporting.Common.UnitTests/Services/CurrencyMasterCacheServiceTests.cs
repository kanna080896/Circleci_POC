using GPS.GlobalReporting.Common.DatabaseAccess.Cache.Interfaces;
using GPS.GlobalReporting.Common.DatabaseAccess.Entities;
using GPS.GlobalReporting.Common.DatabaseAccess.Repositories.Interfaces;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class CurrencyMasterCacheServiceTests
{
    private readonly ICurrencyMasterCacheService _currencyMasterService;
    private readonly Mock<IReportingCache> _currencyMasterCache;

    public CurrencyMasterCacheServiceTests()
    {
        _currencyMasterCache = new Mock<IReportingCache>();
        Mock<ICurrencyMasterRepository> currencyMasterRepository = new();
        _currencyMasterService = new CurrencyMasterCacheService(_currencyMasterCache.Object, currencyMasterRepository.Object);
    }

    #region GetByCodeOrCNameAsync()

    [Theory, InlineData("826"), InlineData("GBP")]
    public async Task GetByCodeOrCNameAsync_Returns_CurrencyMaster(string code)
    {
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(GetCurrencyMasterList);
        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync(code, string.Empty, "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.NotNull(currencyMaster);
        Assert.Equal(389, currencyMaster.Id);
    }

    [Fact]
    public async Task GetByCodeOrCNameAsync_WhenCodeIsInvalid_UsesFallbackValue()
    {
        const string code = "000";
        const string fallbackCode = "826";
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(GetCurrencyMasterList);

        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync(code, fallbackCode, "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.NotNull(currencyMaster);
        Assert.Equal(389, currencyMaster.Id);
    }

    [Fact]
    public async Task GetByCodeOrCNameAsync_WhenCodeIsNull_UsesFallbackValue()
    {
        const string? code = null;
        const string fallbackCode = "GBP";
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(GetCurrencyMasterList);

        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync(code, fallbackCode, "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.NotNull(currencyMaster);
        Assert.Equal("826", currencyMaster.Code);
    }

    [Fact]
    public async Task GetByCodeOrCNameAsync_WhenCodeIsInvalid_AndFallbackCodeIsInvalid_ThenReturnsDefaultCurrency()
    {
        const string code = "000";
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(GetCurrencyMasterList);

        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync(code, "000", "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.NotNull(currencyMaster);
        Assert.Equal(389, currencyMaster.Id);
    }

    [Fact]
    public async Task GetCodeOrCNameByAsync_WhenCodeAndFallbackCodeIsInvalid_AndDefaultValueFailsToLoad_ThrowsArgumentOutOfRangeException()
    {
        const string code = "000";
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(new List<CurrencyMasterEntity>());

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _currencyMasterService.GetByCodeOrCNameAsync(code, string.Empty, "Test/Test"));
    }

    private List<CurrencyMasterEntity> GetCurrencyMasterList()
    {
        return new List<CurrencyMasterEntity> { new() { Id = 389, Code = "826", CName = "GBP" } };
    }

    #endregion

    #region GetLowerExponent()

    /// <summary>
    ///  test 1: visa exponent less than mastercard exponent
    /// </summary>
    [Fact]
    public async Task GetLowerExponent_Success_1()
    {
        var currencyMasterList = new List<CurrencyMasterEntity> { new() { Code = "826", MastercardExponent = 2, VisaExponent = 0 } };
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(currencyMasterList);
        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync("826", "", "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.Equal(currencyMaster.VisaExponent, _currencyMasterService.GetLowerExponent(currencyMaster));
    }

    /// <summary>
    /// test 2: visa exponent equal to mastercard exponent
    /// </summary>
    [Fact]
    public async Task GetLowerExponent_Success_2()
    {
        var currencyMasterList = new List<CurrencyMasterEntity> { new() { Code = "826", MastercardExponent = 2, VisaExponent = 2 } };
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(currencyMasterList);
        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync("826", "", "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.Equal(2, _currencyMasterService.GetLowerExponent(currencyMaster));
    }

    /// <summary>
    /// test 3: visa exponent greater than mastercard exponent
    /// </summary>
    [Fact]
    public async Task GetLowerExponent_Success_3()
    {
        var currencyMasterList = new List<CurrencyMasterEntity> { new() { Code = "826", MastercardExponent = 2, VisaExponent = 3 } };
        _currencyMasterCache.Setup(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>())).ReturnsAsync(currencyMasterList);
        var currencyMaster = await _currencyMasterService.GetByCodeOrCNameAsync("826", "", "Test/Test");
        _currencyMasterCache.Verify(x => x.GetCache(It.IsAny<string>(), It.IsAny<Func<Task<List<CurrencyMasterEntity>>>>()), Times.Once);
        Assert.Equal(currencyMaster.MastercardExponent, _currencyMasterService.GetLowerExponent(currencyMaster));
    }

    #endregion
}