using System.Diagnostics.CodeAnalysis;

namespace GPS.GlobalReporting.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
[ExcludeFromCodeCoverage]
public class XmlVersionAttribute : Attribute
{
    public XmlVersionAttribute(int versionIntroduced, int versionRemoved = 0)
    {
        VersionIntroduced = versionIntroduced;
        VersionRemoved = versionRemoved;
    }

    public int VersionIntroduced { get; }
    
    public int VersionRemoved { get; }
}