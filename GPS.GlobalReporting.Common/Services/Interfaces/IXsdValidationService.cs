namespace GPS.GlobalReporting.Common.Services.Interfaces
{
    public interface IXsdValidationService
    {
        string[] ValidateXmlAgainstXsd(string xsdFile, string xmlFile);
    }
}
