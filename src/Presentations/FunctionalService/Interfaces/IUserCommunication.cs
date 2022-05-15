namespace WeeControl.Presentations.FunctionalService.Interfaces;

public interface IUserCommunication
{
    public string ServerBaseAddress { get; }
    
    public HttpClient HttpClient { get; }
    
}