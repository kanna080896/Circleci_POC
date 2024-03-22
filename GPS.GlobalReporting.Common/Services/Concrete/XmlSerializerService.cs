using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GPS.GlobalReporting.Common.Attributes;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Interfaces;
using GPS.GlobalReporting.Common.Utilities;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class XmlSerializerService : IXmlSerializerService
{
    public async Task<string> SerializeXmlForVersion(IReportModel model, int version)
    {
        var type = model.GetType();
        var propsToOverride = GetPropertiesToOverrideForVersion(type, version);
        var overrides = new XmlAttributeOverrides();
        foreach (var prop in propsToOverride)
        {
            if (prop.DeclaringType != null)
            {
                overrides.Add(prop.DeclaringType, prop.Name, new XmlAttributes { XmlIgnore = true });
            }
        }

        await using var writer = new CustomStringWriter(Encoding.UTF8);
        var xmlWriterSettings = new XmlWriterSettings { Async = true, Indent = true};
        await using (var xmlWriter = XmlWriter.Create(writer, xmlWriterSettings))
        {
            var xmlSerializer = new XmlSerializer(type, overrides);
            xmlSerializer.Serialize(xmlWriter, model);
        }

        var xml = writer.ToString();

        return xml;
    }

    private List<PropertyInfo> GetPropertiesToOverrideForVersion(Type modelType, int version)
    {
        var result = new List<PropertyInfo>();
        foreach (var prop in modelType.GetProperties())
        {
            var versionAttribute = prop.GetCustomAttribute(typeof(XmlVersionAttribute)) as XmlVersionAttribute;
            if (!(versionAttribute?.VersionIntroduced <= version) || (versionAttribute.VersionRemoved > 0 && versionAttribute.VersionRemoved >= version))
            {
                result.Add(prop);
            }
            else
            {
                if (prop.PropertyType.GetInterface("IReportModel") != null)
                {
                    result.AddRange(GetPropertiesToOverrideForVersion(prop.PropertyType, version));
                }

                if (prop.PropertyType.Name.Contains("List"))
                {
                    var listType = prop.PropertyType.GetGenericArguments().FirstOrDefault();
                    if (listType?.GetInterface("IReportModel") != null)
                    {
                        result.AddRange(GetPropertiesToOverrideForVersion(listType, version));
                    }
                }
            }
        }

        return result;
    }
}