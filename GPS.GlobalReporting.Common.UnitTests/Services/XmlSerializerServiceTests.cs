using System.Xml.Serialization;
using GPS.GlobalReporting.Common.Attributes;
using GPS.GlobalReporting.Common.Models;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Xunit;

namespace GPS.GlobalReporting.Common.UnitTests.Services;

public class XmlSerializerServiceTests
{
    private readonly IXmlSerializerService _xmlSerializerService;

    public XmlSerializerServiceTests()
    {
        _xmlSerializerService = new XmlSerializerService();
    }

    [Fact]
    public async Task SerializeXmlForVersion_Should_Include_CorrectPropertiesForVersion()
    {
        var model = GetTestModel();
        var xml = await _xmlSerializerService.SerializeXmlForVersion(model, 3);
        Assert.NotNull(xml);
        Assert.Contains("<TestXmlClass", xml);
        Assert.Contains("<TestXmlSubclass", xml);
        Assert.Contains("<TestXmlSubclassName", xml);
        Assert.Contains("<TestXmlSubSubclasses", xml);
        Assert.Contains("<TestXmlSubSubclass", xml);
        Assert.Contains("<TestXmlSubSubclassName", xml);
    }

    [Fact]
    public async Task SerializeXmlForVersion_Should_NotInclude_PropertiesIntroducedInLaterVersion()
    {
        var model = GetTestModel();
        var xml = await _xmlSerializerService.SerializeXmlForVersion(model, 2);
        Assert.NotNull(xml);
        Assert.DoesNotContain("<TestXmlSubSubclasses", xml);
        Assert.DoesNotContain("<TestXmlSubSubclass", xml);
        Assert.DoesNotContain("<TestXmlSubSubclassName", xml);
    }

    [Fact]
    public async Task SerializeXmlForVersion_Should_NotInclude_PropertiesRemovedInEarlierVersion()
    {
        var model = GetTestModel();
        var xml = await _xmlSerializerService.SerializeXmlForVersion(model, 3);
        Assert.NotNull(xml);
        Assert.Contains("<TestXmlClass", xml);
        Assert.DoesNotContain("<TestXmlClassName", xml);
    }

    private static TestXmlClass GetTestModel()
    {
        return new TestXmlClass()
        {
            Name = "Test",
            Subclass = new TestXmlSubclass
            {
                Name = "Test",
                SubSubclasses = new()
                {
                    new TestXmlSubSubclass
                    {
                        Name = "Test"
                    }
                }
            }
        };
    }

    [XmlRoot(ElementName = "TestXmlClass")]
    public class TestXmlClass : IReportModel
    {
        [XmlVersion(1, 3)]
        [XmlElement(ElementName = "TestXmlClassName")]
        public string Name { get; set; }

        [XmlVersion(2)]
        [XmlElement(ElementName = "TestXmlSubclass")]
        public TestXmlSubclass Subclass { get; set; }
    }

    [XmlRoot(ElementName = "TestXmlSubClass")]
    public class TestXmlSubclass : IReportModel
    {
        [XmlVersion(2)]
        [XmlElement(ElementName = "TestXmlSubclassName")]
        public string Name { get; set; }

        [XmlVersion(3)]
        [XmlElement(ElementName = "TestXmlSubSubclasses")]
        public List<TestXmlSubSubclass> SubSubclasses { get; set; }
    }

    [XmlRoot(ElementName = "TestXmlSubSubclass")]
    public class TestXmlSubSubclass : IReportModel
    {
        [XmlVersion(3)]
        [XmlElement(ElementName = "TestXmlSubSubclassName")]
        public string Name { get; set; }
    }
}