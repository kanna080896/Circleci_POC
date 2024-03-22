namespace GPS.GlobalReporting.Common.Models;

public class CurrencyMasterModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string CName { get; set; }
    public int MastercardExponent { get; set; }
    public string CcySymbol { get; set; }
    public int VisaExponent { get; set; }
    public string Description { get; set; }

    public CurrencyMasterModel()
    {
        Code = string.Empty;
        CName = string.Empty;
        CcySymbol = string.Empty;
        Description = string.Empty;
    }
}