namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceServerCommunication
{ 
    [Obsolete]
    string ServerBaseAddress { get; }
    
     HttpClient HttpClient { get; }

     string GetFullAddress(string relative);
}