namespace WeeControl.User.UserApplication.Interfaces;

public interface IDeviceLocation
{
    Task<(double? Latitude, double? Longitude)> GetAccurateLocationAsync();
    Task<(double? Latitude, double? Longitude)> GetLastKnownLocationAsync();
}