using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests;

public class XmlFileWriterTests
{
    private readonly IXmlFileWriter _fileWriter;

    public XmlFileWriterTests()
    {
        _fileWriter = new XmlFileWriter();
    }

    [Fact]
    public async Task WriteFileAsync_Success()
    {
        var settings = new XmlWriterSettings
        {
            Async = true
        };

        var input = new TestClass { Id = Guid.NewGuid(), Name = "Hello world" };
        var serializer = new XmlSerializer(input.GetType());

        string xml;

        await using (var writer = new StringWriter())
        {
            await using (var xmlWriter = XmlWriter.Create(writer, settings))
            {
                serializer.Serialize(xmlWriter, input);
            }

            xml = writer.ToString();
        }

        var path = "test.xml";
        await _fileWriter.WriteFileAsync(path, xml);
        Assert.True(File.Exists(path));
        var contents = await File.ReadAllTextAsync(path);
        Assert.NotNull(contents);
        Assert.Equal(xml, contents);

        path = Path.Combine("Test", "test.xml");
        await _fileWriter.WriteFileAsync(path, xml);
        Assert.True(File.Exists(path));
        contents = await File.ReadAllTextAsync(path);
        Assert.NotNull(contents);
        Assert.Equal(xml, contents);
    }

    public class TestClass
    {
        [XmlElement(ElementName = "Id")] public Guid Id { get; set; }
        [XmlElement(ElementName = "Name")] public string Name { get; set; }

        public TestClass()
        {
            Name = string.Empty;
        }
    }
}