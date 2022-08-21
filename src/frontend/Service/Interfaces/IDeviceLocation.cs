namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IDeviceLocation
{
    Task<(double? Latitude, double? Longitude)> GetAccurateLocationAsync();
    Task<(double? Latitude, double? Longitude)> GetLastKnownLocationAsync();
}