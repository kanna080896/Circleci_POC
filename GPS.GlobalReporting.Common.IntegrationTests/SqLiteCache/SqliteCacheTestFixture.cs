using System;
using System.IO;

namespace GPS.GlobalReporting.Common.IntegrationTests.SqLiteCache;

public class SqliteCacheTestFixture : IDisposable
{
    public string FileName { get; set; } = "";

    public void Dispose()
    {
        if (File.Exists(FileName))
        {
            try
            {
                File.Delete(FileName);
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}