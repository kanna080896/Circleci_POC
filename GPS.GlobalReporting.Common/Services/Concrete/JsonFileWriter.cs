using GPS.GlobalReporting.Common.Services.Interfaces;

namespace GPS.GlobalReporting.Common.Services.Concrete
{
    public class JsonFileWriter : IJsonFileWriter
    {
        public async Task WriteFileAsync(string path, string contents)
        {
            await File.WriteAllTextAsync(path, contents);
        }
    }
}