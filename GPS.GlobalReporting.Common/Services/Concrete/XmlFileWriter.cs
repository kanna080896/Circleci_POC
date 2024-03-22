using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete;

public class XmlFileWriter : IXmlFileWriter
{
    public async Task WriteFileAsync(string path, string contents)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var streamWriter = new StreamWriter(path);
        await streamWriter.WriteAsync(contents);
        await streamWriter.DisposeAsync();
    }
}