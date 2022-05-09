namespace WeeControl.Common.FunctionalService.Interfaces;

public interface IUserCommunication
{
    public string ServerBaseAddress { get; }
    
    public HttpClient HttpClient { get; }
    
}