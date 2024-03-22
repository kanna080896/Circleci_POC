namespace GPS.GlobalReporting.Common.DatabaseAccess.Entities;

public class CurrencyMasterEntity
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string CName { get; set; }
    public int MastercardExponent { get; set; }
    public string CcySymbol { get; set; }
    public int VisaExponent { get; set; }
    public string Description { get; set; }

    public CurrencyMasterEntity()
    {
        Code = string.Empty;
        CName = string.Empty;
        CcySymbol = string.Empty;
        Description = string.Empty;
    }
}