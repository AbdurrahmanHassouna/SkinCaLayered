namespace SkinCa.Common.Exceptions;

public class ServiceException: Exception
{
    public ServiceException(string error, Exception innerException = null):base(error, innerException)
    {
        
    }
    
}