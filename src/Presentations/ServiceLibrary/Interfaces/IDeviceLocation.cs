namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDeviceLocation
{
    Task<(double? Latitude, double? Longitude)> GetAccurateLocationAsync();
    Task<(double? Latitude, double? Longitude)> GetLastKnownLocationAsync();
    bool LocationIsAvailable();
}