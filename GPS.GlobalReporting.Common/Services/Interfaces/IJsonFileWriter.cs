namespace GPS.GlobalReporting.Common.Services.Interfaces;

public interface IJsonFileWriter
{
    Task WriteFileAsync(string path, string contents);
}