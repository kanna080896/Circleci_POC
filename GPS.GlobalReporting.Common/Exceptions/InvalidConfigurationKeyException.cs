namespace GPS.GlobalReporting.Common.Exceptions;

public class InvalidConfigurationKeyException : Exception
{
    public InvalidConfigurationKeyException(string key) : base($"Configuration key: {key} does not exist or has an empty value")
    {
        
    }
}