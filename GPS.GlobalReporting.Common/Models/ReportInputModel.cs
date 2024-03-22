using System.Diagnostics.CodeAnalysis;

namespace GPS.GlobalReporting.Common.Models;

[ExcludeFromCodeCoverage(Justification = "Temporary")]
public class ReportInputModel
{
    public int Id { get; set; }
    public string FilePrefix { get; set; }
    public bool ShowPubToken { get; set; }
    public List<ReportInputSubFileModel> SubFiles { get; set; }

    public ReportInputModel()
    {
        FilePrefix = string.Empty;
        SubFiles = new List<ReportInputSubFileModel>();
    }
}