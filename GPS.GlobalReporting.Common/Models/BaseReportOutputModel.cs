using System.Diagnostics.CodeAnalysis;

namespace GPS.GlobalReporting.Common.Models;

[ExcludeFromCodeCoverage(Justification = "Temporary")]
public abstract class BaseReportOutputModel
{
    protected BaseReportOutputModel()
    {
        FileName = string.Empty;
        XsdErrors = new List<string>();
    }
    public string FileName { get; set; }
    public List<string> XsdErrors { get; set; }
    public List<ReportSectionCountModel> SectionCounts { get; set; } = new();
}