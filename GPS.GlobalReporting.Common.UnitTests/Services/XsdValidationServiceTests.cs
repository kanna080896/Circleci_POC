using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace GPS.GlobalReporting.Common.UnitTests.Services
{
    [Collection("custom")]
    public class XsdValidationServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IXsdValidationService _service;

        public XsdValidationServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _service = new XsdValidationService();
        }

        [Fact]
        public void When_ValidateCalled_WithInvalidXsdFilePath_ThenExceptionIsThrown()
        {
            const string xmlFilePath = @"SampleData\BalanceXMLSample-Valid-V1.xml";
            const string xsdFilePath = @"invalidpath";

            Assert.Throws<Exception>(() => _service.ValidateXmlAgainstXsd(xsdFilePath, xmlFilePath));
        }
        
        [Fact]
        public void When_ValidateCalled_WithInvalidXmlFilePath_ThenExceptionIsThrown()
        {
            const string xmlFilePath = @"invalidpath";
            const string xsdFilePath = @"XSDFiles\Balance-V1.xsd";
            
            Assert.Throws<Exception>(() => _service.ValidateXmlAgainstXsd(xsdFilePath, xmlFilePath));
        }

        #region Balance XML
        [Fact]
        public void Validate_Balance_V1_Success()
        {
            const string xmlFilePath = @"SampleData\BalanceXMLSample-Valid-V1.xml";
            const string xsdFilePath = @"XSDFiles\Balance-V1.xsd";
            var result = _service.ValidateXmlAgainstXsd(xsdFilePath, xmlFilePath);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Validate_Balance_V1_Failure()
        {
            const string xmlFilePath = @"SampleData\BalanceXMLSample-Invalid-V1.xml";
            const string xsdFilePath = @"XSDFiles\Balance-V1.xsd";
            var result = _service.ValidateXmlAgainstXsd(xsdFilePath, xmlFilePath);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var error in result)
            {
                _testOutputHelper.WriteLine($"{error}:{Environment.NewLine}{error}{Environment.NewLine}");
            }
        }
        #endregion
    }
}
