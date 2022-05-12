namespace WeeControl.Common.FunctionalService.Exceptions;

public class ServerNotAvailableException : Exception
{
    public ServerNotAvailableException() : base("Communication with server is not available now, check connection!")
    {
    }
}