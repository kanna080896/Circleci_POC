using GPS.GlobalReporting.Common.Enums;

namespace GPS.GlobalReporting.Common.Models;

public class IssuerConfigModel
{
    public IssuerConfigModel()
    {
        XmlFilePrefix = string.Empty;
    }   
    public int BankMasterId { get; set; }
    public string XmlFilePrefix { get; set; }
    public bool ShowPubToken { get; set; }
    public GlobalReportingXmlTypeEnum XmlType { get; set; }
}