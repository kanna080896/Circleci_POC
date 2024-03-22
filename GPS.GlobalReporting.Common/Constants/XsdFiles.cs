using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GPS.GlobalReporting.Common.Constants;

[ExcludeFromCodeCoverage]
public class XsdFiles
{
    public static readonly string TransactionLatest = Path.Combine(GetDirectoryName(), $"XSDFiles{Path.DirectorySeparatorChar}Transaction-V2.xsd");
    public static readonly string TransactionV1 = Path.Combine(GetDirectoryName(), $"XSDFiles{Path.DirectorySeparatorChar}Transaction-V1.xsd");
    public static readonly string BalanceLatest = Path.Combine(GetDirectoryName(), $"XSDFiles{Path.DirectorySeparatorChar}Balance-V1.xsd");
    
    private static string GetDirectoryName()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
    }
}