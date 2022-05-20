namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDeviceServerCommunication
{ 
    string ServerBaseAddress { get; }
    
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}