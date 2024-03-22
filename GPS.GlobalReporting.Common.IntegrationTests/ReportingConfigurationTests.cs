using System;
using System.Linq;
using GPS.GlobalReporting.Common.Enums;
using GPS.GlobalReporting.Common.Services.Concrete;
using GPS.GlobalReporting.Common.Exceptions;
using GPS.GlobalReporting.Common.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GPS.GlobalReporting.Common.IntegrationTests;

public class ReportingConfigurationTests
{
    private readonly IReportingConfiguration _configuration;

    public ReportingConfigurationTests()
    {
        IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json", true, true).Build();
        _configuration = new ReportingConfiguration(config);
    }

    [Fact]
    public void GetValueString_WhenCalledWithValidConfigKey_ReturnsExpectedValue()
    {
        var result = _configuration.GetValue<string>("TestString");
        Assert.Equal("Test", result);
    }
    
    [Fact]
    public void GetValueBool_WhenCalledWithValidConfigKey_ReturnsExpectedValue()
    {
        var result = _configuration.GetValue<bool>("TestBool");
        Assert.True(result);
    }
    
    [Fact]
    public void GetValueInt_WhenCalledWithValidConfigKey_ReturnsExpectedValue()
    {
        var result = _configuration.GetValue<int>("TestInt");
        Assert.Equal(28, result);
    }
    
    [Fact]
    public void GetValueDateTime_WhenCalledWithValidConfigKey_ReturnsExpectedValue()
    {
        var result = _configuration.GetValue<DateTime>("TestDate");
        Assert.Equal(new DateTime(2020,1,1), result);
    }

    [Fact]
    public void GetValue_WhenCalledWithType_AndConversionIsUnsuccessful_ThrowsInvalidConfigurationTypeException()
    {
        Assert.Throws<InvalidConfigurationTypeException>(() => _configuration.GetValue<bool>("TestString"));
    }

    [Fact]
    public void GetValue_LogsWarningIfNullOrEmpty()
    {
        Assert.Null(_configuration.GetValue<string>("InvalidKey"));
    }

    [Fact]
    public void GetConnectionString_WhenCalledWithValidConnectionStringKey_ReturnsExpectedConnectionString()
    {
        var result = _configuration.GetConnectionString("Test");
        Assert.Equal("Test Connection String", result);
    }
    
    [Fact]
    public void GetConnectionString_WhenCalledWithInvalidConnectionStringKey_ThrowsInvalidConfigurationKeyException()
    { 
        Assert.Throws<InvalidConfigurationKeyException>(() => _configuration.GetConnectionString("InvalidConnectionStringKey"));
    }
}