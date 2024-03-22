using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class ReportInputService : IReportInputService
{
    private readonly IIssuerConfigCacheService _issuerConfigCacheService;
    private readonly ISubFileConfigCacheService _subFileConfigCacheService;

    public ReportInputService(IIssuerConfigCacheService issuerConfigCacheService, ISubFileConfigCacheService subFileConfigCacheService)
    {
        _issuerConfigCacheService = issuerConfigCacheService;
        _subFileConfigCacheService = subFileConfigCacheService;
    }

    public async Task<List<ReportInputModel>> GetReportInputs(ReportTypeEnum reportType)
    {
        var issuerConfigCache = await _issuerConfigCacheService.GetIssuerConfigList();
        var subFileConfigCache = await _subFileConfigCacheService.GetSubFileConfigList();
        var issuerConfigModels = FilterIssuerConfigList(issuerConfigCache, reportType);
        return issuerConfigModels.Select(x => MapReportInputModel(x, subFileConfigCache)).ToList();
    }

    public async Task<ReportInputModel?> GetReportInput(ReportTypeEnum reportType, int issuerId)
    {
        var issuerCache = await _issuerConfigCacheService.GetIssuerConfigList();
        var subFileCache = await _subFileConfigCacheService.GetSubFileConfigList();
        var issuerModel = issuerCache.FirstOrDefault(x => x.BankMasterId == issuerId);
        if (issuerModel == null) return null;
        return MapReportInputModel(issuerModel, subFileCache);
    }

    private static ReportInputModel MapInputModel(IssuerConfigModel issuerConfigModel, List<SubFileConfigModel> subFileConfigModels)
    {
        return new ReportInputModel
        {
            Id = issuerConfigModel.BankMasterId,
            FilePrefix = issuerConfigModel.XmlFilePrefix,
            ShowPubToken = issuerConfigModel.ShowPubToken,
            SubFiles = subFileConfigModels.Select(MapInputSubFileModel).ToList()
        };
    }

    private static ReportInputSubFileModel MapInputSubFileModel(SubFileConfigModel subFileConfigModel)
    {
        return new ReportInputSubFileModel
        {
            FilePrefix = subFileConfigModel.FilePrefix,
            ShowPubToken = subFileConfigModel.ShowPubToken
        };
    }

    private static List<IssuerConfigModel> FilterIssuerConfigList(List<IssuerConfigModel> issuerConfigList, ReportTypeEnum reportType)
    {
        var issuerConfigModels = reportType switch
        {
            ReportTypeEnum.Balance =>
                issuerConfigList.Where(x => x.XmlType is GlobalReportingXmlTypeEnum.BalanceOnly or GlobalReportingXmlTypeEnum.BothReports).ToList(),
            ReportTypeEnum.NonClearing =>
                issuerConfigList.Where(x => x.XmlType is GlobalReportingXmlTypeEnum.TransactionOnly or GlobalReportingXmlTypeEnum.BothReports).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(reportType), reportType, "Invalid Report Type")
        };

        return issuerConfigModels;
    }

    private static ReportInputModel MapReportInputModel(IssuerConfigModel issuerConfigModel, List<SubFileConfigModel> subFileConfigCache)
    {
        var subFileConfigList = subFileConfigCache.Where(x => x.BankMasterId == issuerConfigModel.BankMasterId).ToList();
        return MapInputModel(issuerConfigModel, subFileConfigList);
    }
}