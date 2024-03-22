using System.Diagnostics.CodeAnalysis;

namespace GPS.GlobalReporting.Common.Models;
[ExcludeFromCodeCoverage(Justification = "Temporary")]
public class ReportSectionCountModel
{
    public ReportSectionCountModel(string sectionName, int sectionCount)
    {
        SectionName = sectionName;
        SectionCount = sectionCount;
    }
    public ReportSectionCountModel()
    {
        SectionName = string.Empty;
    }
    public string SectionName { get; set; }
    public int SectionCount { get; set; }
}