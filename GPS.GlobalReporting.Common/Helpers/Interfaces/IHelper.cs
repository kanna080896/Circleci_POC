namespace GPS.GlobalReporting.Common.Helpers.Interfaces;

public interface IHelper
{
    string GetAccountNumber(long pan, long primaryId, long? multiFxAccount, long tokenForPrimary, long? tokenForMultiFx,
        long tokenForCard);
    string GetAmountDirection(short credit);
    string GetMessageSourceValueFromCardProduct(string cardProduct);
    string Left(string? text, int length);
    string Right(string? text, int length);
    bool ContainsChar(string input, params char[] characters);
}