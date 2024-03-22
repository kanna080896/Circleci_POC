using System.Xml;
using System.Xml.Schema;
using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete
{
    public class XsdValidationService : IXsdValidationService
    {
        public string[] ValidateXmlAgainstXsd(string xsdFile, string xmlFile)
        {
            if (!File.Exists(xsdFile)) throw new Exception($"Invalid XSD File path: {xsdFile}");
            if (!File.Exists(xmlFile)) throw new Exception($"Invalid XML File path: {xmlFile}");

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdFile);
            settings.ValidationType = ValidationType.Schema;

            var errors = new List<ValidationEventArgs>();
            settings.ValidationEventHandler += (sender, args) =>
            {
                errors.Add(args);
            };

            var reader = XmlReader.Create(xmlFile, settings);

            while (reader.Read()) { }
            reader.Dispose();
            return errors.Where(x => x.Severity == XmlSeverityType.Error).Select(s => s.Message).Distinct().ToArray();
        }
    }
}
