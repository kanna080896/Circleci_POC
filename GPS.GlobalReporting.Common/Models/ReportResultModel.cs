using System.Diagnostics.CodeAnalysis;

namespace GPS.GlobalReporting.Common.Models;
[ExcludeFromCodeCoverage(Justification = "Temporary")]
public class ReportResultModel : BaseReportOutputModel
{
    public int Id { get; set; }
    public List<SubFileModel> SubFiles { get; set; } = new();
}