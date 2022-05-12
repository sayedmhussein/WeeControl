namespace WeeControl.Frontend.FunctionalService.Exceptions;

public class ServerNotAvailableException : Exception
{
    public ServerNotAvailableException() : base("Communication with server is not available now, check connection!")
    {
    }
}