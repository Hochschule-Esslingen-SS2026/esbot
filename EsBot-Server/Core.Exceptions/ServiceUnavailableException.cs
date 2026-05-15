namespace Core.Exceptions;

public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message, Exception ex) : base(message, ex)
    {

    }
}
