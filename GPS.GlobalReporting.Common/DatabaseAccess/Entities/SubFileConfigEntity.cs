
namespace GPS.GlobalReporting.Common.DatabaseAccess.Entities;

public class SubFileConfigEntity
{
    public SubFileConfigEntity()
    {
        FilePrefix = string.Empty;
    }
    public int XmlFileId { get; set; }
    public int BankMasterId { get; set; }
    public string FilePrefix { get; set; }
    public bool IsIssuer { get; set; }
    public bool ShowPubToken { get; set; }
}