using GPS.GlobalReporting.Common.Enums;

namespace GPS.GlobalReporting.Common.DatabaseAccess.Entities;

public class IssuerConfigEntity
{
    public IssuerConfigEntity()
    {
        XmlFilePrefix = string.Empty;
    }    
    public int BankMasterId { get; set; }
    public string XmlFilePrefix { get; set; }
    public bool ShowPubToken { get; set; }
    public bool IsIssuer { get; set; }
    public GlobalReportingXmlTypeEnum XmlType { get; set; }
}