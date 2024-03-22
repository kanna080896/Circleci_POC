using GPS.GlobalReporting.Common.Helpers.Interfaces;

namespace GPS.GlobalReporting.Common.Helpers.Concrete;

public class Helper : IHelper
{
    public string GetAccountNumber(long pan, long primaryId, long? multiFxAccount, long tokenForPrimary, long? tokenForMultiFx, long tokenForCard)
    {
        var isTokenForPrimary = pan != primaryId;
        var isTokenForMultiFx = pan != multiFxAccount && multiFxAccount != null;

        if (isTokenForPrimary) 
            return tokenForPrimary.ToString();
        
        return (isTokenForMultiFx ? tokenForMultiFx.ToString() : tokenForCard.ToString())!;
    }

    public string GetAmountDirection(short flCredit)
    {
        var isCredit = flCredit.Equals(1);
        var result = isCredit ? "credit" : "debit";
        return result;
    }

    public string GetMessageSourceValueFromCardProduct(string cardProduct)
    {
        switch (cardProduct)
        {
            case "MCRD":
            case "MAES":
                return "67";
            case "VISA":
                return "54";
            default:
                return string.Empty;
        }
    }

    public string Left(string? text, int length)
    {
        string result;
        
        if (string.IsNullOrEmpty(text))
            result = string.Empty;
        else if (text.Length < length)
            result = text;
        else
            result = text[..length];

        return result;
    }

    public string Right(string? text, int length)
    {
        string result;

        if (string.IsNullOrEmpty(text))
            result = string.Empty;
        else if (text.Length < length)
            result = text;
        else
            result = text.Substring(text.Length-length);

        return result;
    }

    public bool ContainsChar(string input, params char[] characters)
    {
        var character = characters.FirstOrDefault(x => input.Contains(x));

        if (character != 0)
            return true;            
        
        return false;
    }
}