namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDeviceServerCommunication
{ 
    [Obsolete]
    string ServerBaseAddress { get; }
    
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}