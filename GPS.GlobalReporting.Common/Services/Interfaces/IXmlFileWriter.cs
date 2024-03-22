namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IXmlFileWriter
{
    Task WriteFileAsync(string path, string contents);
}