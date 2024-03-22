using System.Diagnostics.CodeAnalysis;

namespace GPS.GlobalReporting.Common.Models;
[ExcludeFromCodeCoverage(Justification = "Temporary")]
public class ReportInputSubFileModel
{
    public string FilePrefix { get; set; }
    public bool ShowPubToken { get; set; }

    public ReportInputSubFileModel()
    {
        FilePrefix = string.Empty;
    }
}