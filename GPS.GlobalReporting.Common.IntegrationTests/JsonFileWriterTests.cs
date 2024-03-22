using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests
{
    public class JsonFileWriterTests
    {
        private readonly IJsonFileWriter _writer;

        public JsonFileWriterTests()
        {
            _writer = new JsonFileWriter();
        }

        [Fact]
        public async Task WriteFileAsync_Success()
        {
            var input = new TestClass {Id = Guid.NewGuid(), Name = "hello world"};
            var json = JsonSerializer.Serialize(input, new JsonSerializerOptions
            {
                WriteIndented = true, // just for testing, makes it pretty (JSONP)
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // this makes it true camel case instead of proper case
            });
            const string path = "test.json";
            await _writer.WriteFileAsync(path, json);
            Assert.True(File.Exists(path));
            var contents = await File.ReadAllTextAsync(path);
            Assert.NotNull(contents);
            Assert.Equal(json, contents);
        }

        private class TestClass
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public TestClass()
            {
                Name = string.Empty;
            }
        }
    }
}