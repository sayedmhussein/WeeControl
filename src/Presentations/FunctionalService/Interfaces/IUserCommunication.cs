namespace WeeControl.Presentations.FunctionalService.Interfaces;

public interface IUserCommunication
{ 
    string ServerBaseAddress { get; }
    
     HttpClient HttpClient { get; }

     string FullAddress(string relative);
}