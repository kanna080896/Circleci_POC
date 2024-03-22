namespace GPS.GlobalReporting.Common.Models;

public class SubFileConfigModel
{
    public SubFileConfigModel()
    {
        FilePrefix = string.Empty;
    }
    public int BankMasterId { get; set; }
    public string FilePrefix { get; set; }
    public bool ShowPubToken { get; set; }
}