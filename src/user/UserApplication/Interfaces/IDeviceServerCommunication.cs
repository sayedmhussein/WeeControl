namespace WeeControl.User.UserApplication.Interfaces;

public interface IDeviceServerCommunication
{ 
    [Obsolete]
    string ServerBaseAddress { get; }
    
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}