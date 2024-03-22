using FluentAssertions;
using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Moq;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class ReportInputServiceTests
{
    private readonly IReportInputService _reportInputService;
    private readonly Mock<IIssuerConfigCacheService> _issuerConfigCacheService;
    private readonly Mock<ISubFileConfigCacheService> _subFileConfigCacheService;

    public ReportInputServiceTests()
    {
        _issuerConfigCacheService = new Mock<IIssuerConfigCacheService>();
        _subFileConfigCacheService = new Mock<ISubFileConfigCacheService>();
        _reportInputService = new ReportInputService(_issuerConfigCacheService.Object, _subFileConfigCacheService.Object);
    }

    [Fact]
    public async Task ReportInputService_Balance_Success()
    {
        var expected = ExpectedBalanceResult();
        _issuerConfigCacheService.Setup(x => x.GetIssuerConfigList()).ReturnsAsync(GetIssuerConfigModelList());
        _subFileConfigCacheService.Setup(x => x.GetSubFileConfigList()).ReturnsAsync(GetSubFileConfigModelList());

        var result = await _reportInputService.GetReportInputs(ReportTypeEnum.Balance);

        _issuerConfigCacheService.Verify(x => x.GetIssuerConfigList(), Times.Once);
        _subFileConfigCacheService.Verify(x => x.GetSubFileConfigList(), Times.Once);

        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReportInputService_Balance_IssuerSuccess()
    {
        var expected = ExpectedSingleIssuerBalanceResult();
        _issuerConfigCacheService.Setup(x => x.GetIssuerConfigList()).ReturnsAsync(GetIssuerConfigModelList());
        _subFileConfigCacheService.Setup(x => x.GetSubFileConfigList()).ReturnsAsync(GetSubFileConfigModelList());

        var result = await _reportInputService.GetReportInput(ReportTypeEnum.Balance, 2);

        _issuerConfigCacheService.Verify(x => x.GetIssuerConfigList(), Times.Once);
        _subFileConfigCacheService.Verify(x => x.GetSubFileConfigList(), Times.Once);

        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReportInputService_Transaction_Success()
    {
        var expected = ExpectedTransactionResult();
        _issuerConfigCacheService.Setup(x => x.GetIssuerConfigList()).ReturnsAsync(GetIssuerConfigModelList());
        _subFileConfigCacheService.Setup(x => x.GetSubFileConfigList()).ReturnsAsync(GetSubFileConfigModelList());

        var result = await _reportInputService.GetReportInputs(ReportTypeEnum.NonClearing);

        _issuerConfigCacheService.Verify(x => x.GetIssuerConfigList(), Times.Once);
        _subFileConfigCacheService.Verify(x => x.GetSubFileConfigList(), Times.Once);

        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReportInputService_Transaction_Issuer_Success()
    {
        var expected = ExpectedSingleIssuerTransactionResult();
        _issuerConfigCacheService.Setup(x => x.GetIssuerConfigList()).ReturnsAsync(GetIssuerConfigModelList());
        _subFileConfigCacheService.Setup(x => x.GetSubFileConfigList()).ReturnsAsync(GetSubFileConfigModelList());

        var result = await _reportInputService.GetReportInput(ReportTypeEnum.NonClearing, 3);

        _issuerConfigCacheService.Verify(x => x.GetIssuerConfigList(), Times.Once);
        _subFileConfigCacheService.Verify(x => x.GetSubFileConfigList(), Times.Once);

        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReportInputService_Transaction_Issuer_null()
    {
        _issuerConfigCacheService.Setup(x => x.GetIssuerConfigList()).ReturnsAsync(GetIssuerConfigModelList());
        _subFileConfigCacheService.Setup(x => x.GetSubFileConfigList()).ReturnsAsync(GetSubFileConfigModelList());

        var result = await _reportInputService.GetReportInput(ReportTypeEnum.NonClearing, 30);

        _issuerConfigCacheService.Verify(x => x.GetIssuerConfigList(), Times.Once);
        _subFileConfigCacheService.Verify(x => x.GetSubFileConfigList(), Times.Once);

        Assert.Null(result);
    }

    [Fact]
    public async Task ReportInputService_InvalidEnum_Failure()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _reportInputService.GetReportInputs((ReportTypeEnum)9999));
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await _reportInputService.GetReportInput((ReportTypeEnum)9999,30));
    }

    private static List<IssuerConfigModel> GetIssuerConfigModelList()
    {
        return new List<IssuerConfigModel>
        {
            new()
            {
                BankMasterId = 1,
                XmlFilePrefix = "ABC",
                ShowPubToken = false,
                XmlType = GlobalReportingXmlTypeEnum.None
            },
            new()
            {
                BankMasterId = 2,
                XmlFilePrefix = "DEF",
                ShowPubToken = false,
                XmlType = GlobalReportingXmlTypeEnum.BalanceOnly
            },
            new()
            {
                BankMasterId = 3,
                XmlFilePrefix = "GHI",
                ShowPubToken = false,
                XmlType = GlobalReportingXmlTypeEnum.TransactionOnly
            },
            new()
            {
                BankMasterId = 4,
                XmlFilePrefix = "JKL",
                ShowPubToken = false,
                XmlType = GlobalReportingXmlTypeEnum.BothReports
            }
        };
    }

    private static List<SubFileConfigModel> GetSubFileConfigModelList()
    {
        return new List<SubFileConfigModel>
        {
            new()
            {
                BankMasterId = 1,
                FilePrefix = "AAA",
                ShowPubToken = false
            },
            new()
            {
                BankMasterId = 1,
                FilePrefix = "BBB",
                ShowPubToken = true
            },
            new()
            {
                BankMasterId = 2,
                FilePrefix = "DDD",
                ShowPubToken = false
            },
            new()
            {
                BankMasterId = 2,
                FilePrefix = "EEE",
                ShowPubToken = true
            },
            new()
            {
                BankMasterId = 3,
                FilePrefix = "GGG",
                ShowPubToken = false
            },
            new()
            {
                BankMasterId = 3,
                FilePrefix = "HHH",
                ShowPubToken = true
            },
            new()
            {
                BankMasterId = 4,
                FilePrefix = "JJJ",
                ShowPubToken = false
            },
            new()
            {
                BankMasterId = 4,
                FilePrefix = "KKK",
                ShowPubToken = true
            }
        };
    }

    private static List<ReportInputModel> ExpectedBalanceResult()
    {
        return new List<ReportInputModel>
        {
            new()
            {
                Id = 2,
                FilePrefix = "DEF",
                ShowPubToken = false,
                SubFiles = new List<ReportInputSubFileModel>
                {
                    new()
                    {
                        FilePrefix = "DDD",
                        ShowPubToken = false
                    },
                    new()
                    {
                        FilePrefix = "EEE",
                        ShowPubToken = true
                    }
                }
            },
            new()
            {
                Id = 4,
                FilePrefix = "JKL",
                ShowPubToken = false,
                SubFiles = new List<ReportInputSubFileModel>
                {
                    new()
                    {
                        FilePrefix = "JJJ",
                        ShowPubToken = false
                    },
                    new()
                    {
                        FilePrefix = "KKK",
                        ShowPubToken = true
                    }
                }
            }
        };
    }

    private static List<ReportInputModel> ExpectedTransactionResult()
    {
        return new List<ReportInputModel>
        {
            new()
            {
                Id = 3,
                FilePrefix = "GHI",
                ShowPubToken = false,
                SubFiles = new List<ReportInputSubFileModel>
                {
                    new()
                    {
                        FilePrefix = "GGG",
                        ShowPubToken = false
                    },
                    new()
                    {
                        FilePrefix = "HHH",
                        ShowPubToken = true
                    }
                }
            },
            new()
            {
                Id = 4,
                FilePrefix = "JKL",
                ShowPubToken = false,
                SubFiles = new List<ReportInputSubFileModel>
                {
                    new()
                    {
                        FilePrefix = "JJJ",
                        ShowPubToken = false
                    },
                    new()
                    {
                        FilePrefix = "KKK",
                        ShowPubToken = true
                    }
                }
            }
        };
    }

    private static ReportInputModel ExpectedSingleIssuerTransactionResult()
        {
            var reportInputModel = new ReportInputModel
            {
                Id = 3,
                FilePrefix = "GHI",
                ShowPubToken = false,
                SubFiles = new List<ReportInputSubFileModel>
                {
                    new ReportInputSubFileModel
                    {
                        FilePrefix = "GGG",
                        ShowPubToken = false
                    },
                    new ReportInputSubFileModel
                    {
                        FilePrefix = "HHH",
                        ShowPubToken = true
                    }
                }
            };

            return reportInputModel;
        }

    private static ReportInputModel ExpectedSingleIssuerBalanceResult()
    {
        var reportInputModel = new ReportInputModel
        {
                Id = 2,
                FilePrefix = "DEF",
                ShowPubToken = false,
                SubFiles = new List<ReportInputSubFileModel>
                {
                    new()
                    {
                        FilePrefix = "DDD",
                        ShowPubToken = false
                    },
                    new()
                    {
                        FilePrefix = "EEE",
                        ShowPubToken = true
                    }
                }
        };
        return reportInputModel;
    }
}