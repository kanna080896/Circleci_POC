using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Models;

namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IReportInputService
{
    Task<List<ReportInputModel>> GetReportInputs(ReportTypeEnum reportType);
    Task<ReportInputModel?> GetReportInput(ReportTypeEnum reportType, int issuerId);
}