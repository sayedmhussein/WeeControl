namespace WeeControl.Frontend.ServiceLibrary.Interfaces;

public interface IUserCommunication
{
    public string ServerBaseAddress { get; }
    
    public HttpClient HttpClient { get; }
    
}