namespace GPS.GlobalReporting.Common.Exceptions;

public class InvalidConfigurationTypeException : Exception
{
    public InvalidConfigurationTypeException(string key, string type) : base($"Configuration value with key of: {key} cannot be converted to type: {type}")
    {
        
    }
}